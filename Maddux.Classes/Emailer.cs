using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Web;

namespace Maddux.Classes
{
    public class Emailer
    {
        public Emailer()
        {

        }

        public bool SendLostPwdEmail(string name, string emailAddress, string pwd, bool useAppName)
        {
            List<string> recipients;
            string appName;
            string emailSubject;
            string emailBody;
            string siteURL;
            HttpRequest request;
            Configuration appSetting;

            try
            {
                appSetting = new Configuration();

                request = HttpContext.Current.Request;

                siteURL = request.Url.Scheme + "://" + request.Url.Authority + request.ApplicationPath.TrimEnd('/') + "/";

                recipients = new List<string>
                {
                    emailAddress
                };

                if (useAppName)
                {
                    appName = appSetting.AppName;
                }
                else
                {
                    appName = "the Redbud website";
                }

                emailSubject = "Your password for " + appName;

                emailBody = "Dear " + name + ", ";
                emailBody += "\n\nA request was received to send your login information for " + appName + ". ";
                emailBody += "Your login information is as follows:";
                emailBody += "\n\nEmail address:  " + emailAddress;
                emailBody += "\nPassword:  " + pwd;
                emailBody += "\n\n<a href=\"" + siteURL + "\">Login to " + appName + "</a>";
                emailBody += "\n\nSincerely,\n\nWebsite Administrator";

                if (appSetting.EmailUseExternalSMTP)
                {
                    return SendSMTPEmail(appSetting, appSetting.LostPwdFromAddress, appSetting.LostPwdFromAddress, recipients, emailSubject, emailBody);
                }
                else
                {
                    return SendPostmarkEmail(appSetting, appSetting.LostPwdFromAddress, appSetting.LostPwdFromAddress, recipients, emailSubject, emailBody);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SendRegistrationEmail(string companyName, string contactName, string emailAddress, string address, string city, string province, string postalCode, string phone, string comments)
        {
            List<string> recipients;
            string fromAddress;
            string emailSubject;
            string emailBody;
            Configuration appSetting;

            try
            {
                appSetting = new Configuration();

                fromAddress = "info@redbud.com";
                recipients = new List<string>
                {
                    "sales@redbud.com"
                };

                emailSubject = "New Redbud registration request";

                emailBody = "<html><body style=\"font-family:calibri,arial,helvetica,verdana,sans-serif;font-size:11pt\">";
                emailBody += "\n\nA request was received from " + companyName + " for access to the Redbud website. ";
                emailBody += "The information provided is as follows:";
                emailBody += "<br /><br />Company name:  " + companyName;
                emailBody += "<br />Contact:  " + contactName;
                emailBody += "<br />Email address:  " + emailAddress;
                emailBody += "<br /><br />Address:  " + address;
                emailBody += "<br />City / Province:  " + city + ", " + province;
                emailBody += "<br />Postal Code:  " + postalCode;
                emailBody += "<br />Phone Number: " + phone;
                emailBody += "<br /><br />Comments:  " + comments;
                emailBody += "</body></html>";

                if (appSetting.EmailUseExternalSMTP)
                {
                    return SendSMTPEmail(appSetting, fromAddress, fromAddress, recipients, emailSubject, emailBody);
                }
                else
                {
                    return SendPostmarkEmail(appSetting, fromAddress, fromAddress, recipients, emailSubject, emailBody);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SendEmail(string replyAddress, List<string> recipients, string subject, string body)
        {
            try
            {
                Configuration appSetting = new Configuration();

                if (appSetting.EmailUseExternalSMTP)
                {
                    return SendSMTPEmail(appSetting, appSetting.EmailerFromAddress, replyAddress, recipients, subject, body);
                }
                else
                {
                    return SendPostmarkEmail(appSetting, appSetting.EmailerFromAddress, replyAddress, recipients, subject, body);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool SendSMTPEmail(Configuration appSetting, string fromAddress, string replyAddress, List<string> recipients,
                                    string subject, string body)
        {
            string htmlBody;

            try
            {
                SmtpClient smtpClient = new SmtpClient("localhost");

                htmlBody = "<html><body style=\"font-family:calibri,arial,helvetica,verdana,sans-serif;font-size:11pt\">" + body.Replace("\n", "<br />") + "</body></html>";

                foreach (string address in recipients)
                {
                    MailMessage message = new MailMessage();
                    message.ReplyToList.Add(new MailAddress(replyAddress));
                    message.From = new MailAddress(fromAddress);
                    message.To.Add(new MailAddress(address));

                    message.Subject = subject;

                    message.Body = htmlBody;
                    message.IsBodyHtml = true;

                    smtpClient.Send(message);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool SendPostmarkEmail(Configuration appSetting, string fromAddress, string replyAddress, List<string> recipients,
                                        string subject, string body)
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}