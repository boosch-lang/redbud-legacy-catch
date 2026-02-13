using System;
using System.Text.RegularExpressions;

namespace Redbud.BL.Utils
{
    public class StringTools
    {
        public static string GenerateError(string error)
        {
            return $@"<div class='alert alert-danger'> 
                        <button type='button' class='close' data-dismiss='alert' aria-label='Close'> 
                        <i class='fa fa-times'></i>
                        </button > 
                        <span >{error}</ span ></div >";
        }
        public static string GenerateWarning(string error)
        {
            return $@"<div class='alert alert-warning'> 
                        <button type='button' class='close' data-dismiss='alert' aria-label='Close'> 
                        <i class='fa fa-times'></i>
                        </button > 
                        <span >{error}</ span ></div >";
        }
        public static string GenerateSuccess(string message)
        {
            return $@"<div class='alert alert-success'> 
                        <button type='button' class='close' data-dismiss='alert' aria-label='Close'> 
                        <i class='fa fa-times'></i>
                        </button > 
                        <span >{message}</ span ></div >";
        }
        public static string GenerateBanner(string message)
        {
            return $@"<div class='alert alert-welcome'> 
                        <span class='font-weight-bold' >{message}</ span >
                     </div >";
        }
        public static string GenerateSalesBanner(string message)
        {
            return $@"<div class='alert alert-success'> 
                        <span class='font-weight-bold' >{message}</ span >
                     </div >";
        }
        public static bool IsEmail(string email)
        {
            bool result = false;
            try
            {
                email = email.Trim();
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }
        public static string GenerateSlug(string phrase)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(phrase.ToLower());
            string str = System.Text.Encoding.ASCII.GetString(bytes);
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            str = Regex.Replace(str, @"\s+", " ").Trim();
            str = Regex.Replace(str, @"\s", "-");
            return str;
        }
    }
}
