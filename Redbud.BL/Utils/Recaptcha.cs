using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Redbud.BL.Utils
{
    public class Recaptcha
    {
        public class RecaptchaOptions
        {
            [Required]
            public string SiteKey { get; set; } = default;

            [Required]
            public string SecretKey { get; set; } = default;

            [Required]
            [Range(0.0, 1.0)]
            public double MinimumScore { get; set; }

            public List<string> IPAddressWhitelist { get; set; } = new List<string>();

            /// <summary>
            /// For development only
            /// </summary>
            public bool DisableValidation { get; set; } = false;
        }

        public class RecaptchaService
        {
            private readonly RecaptchaOptions _config;

            public RecaptchaService()
            {
                _config = new RecaptchaOptions
                {
                    SiteKey = ConfigurationManager.AppSettings["reCaptchaSiteKey"],
                    SecretKey = ConfigurationManager.AppSettings["reCaptchaSecretKey"],
                    MinimumScore = double.TryParse(ConfigurationManager.AppSettings["reCaptchaMinimumScore"], out var _minimumScore) ? _minimumScore : 0.8,
                };
            }

            public string SiteKey
            {
                get
                {
                    return _config.SiteKey;
                }
            }

            private string logInfo(string remoteIP)
            {
                try
                {
                    var info = new List<string>();

                    if (remoteIP != null) info.Add($"Remote IP: {remoteIP}");

                    return info.Count > 0 ? string.Join("; ", info) : "";
                }
                catch
                {
                    return "";
                }
            }

            public async Task<bool> ValidateRecaptcha(string recaptchaToken, string action, string remoteIP)
            {
                try
                {
                    if (_config.DisableValidation)
                        return true;

                    var whitelistedIPAddresses = _config.IPAddressWhitelist
                        .Select(x => IPAddress.TryParse(x, out var addr) ? addr : null);
                    if (whitelistedIPAddresses.Any(x => x != null && x.Equals(remoteIP)))
                        return true;

                    using (var httpClient = new HttpClient())
                    {
                        httpClient.BaseAddress = new Uri("https://www.google.com/recaptcha/api/");

                        var postData = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("secret", _config.SecretKey),
                    new KeyValuePair<string, string>("response", recaptchaToken),
                };

                        if (remoteIP != null)
                        {
                            postData.Add(new KeyValuePair<string, string>("remoteip", remoteIP.ToString()));
                        }

                        HttpResponseMessage response;
                        try
                        {
                            response = await httpClient.PostAsync("siteverify", new FormUrlEncodedContent(postData));
                        }
                        catch (Exception ex)
                        {
                            throw new Exception($"Exception while sending recaptcha validation request: {ex.Message}", ex);
                        }

                        var responseString = await response.Content.ReadAsStringAsync();

                        var responseJson = (JObject)JsonConvert.DeserializeObject(responseString);

                        var success = responseJson.Value<bool>("success");
                        if (!success)
                        {
                            throw new Exception($"Failed recaptcha verification. API returned failure {responseJson.ToString()}. {logInfo(remoteIP)}");
                        }

                        var score = responseJson.Value<double?>("score");
                        var responseAction = responseJson.Value<string>("action");

                        if (responseAction == null || responseAction != action)
                        {
                            throw new Exception($"Failed recaptcha verification for action {action}. Received incorrect action: {action}. {logInfo(remoteIP)}");
                        }

                        if (score == null || score < _config.MinimumScore)
                        {
                            throw new Exception($"Failed recaptcha verification for action {responseAction} with score {score}. (Below minimum score of {_config.MinimumScore}) {logInfo(remoteIP)}");
                        }

                        return true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception while validating recaptcha: {ex.Message} {logInfo(remoteIP)}");
                    throw new Exception($"Exception while validating recaptcha: {ex.Message}", ex);
                }
            }
        }

    }
}