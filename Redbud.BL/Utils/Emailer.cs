using Redbud.BL.DL;
using Redbud.BL.Helpers;
using Redbud.BL.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Web;

namespace Redbud.BL.Utils
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
            string extraEmailSubject;
            string extraEmailBody;
            List<string> extraRecipients;
            string siteURL;
            HttpRequest request;
            Configuration appSetting;

            try
            {
                appSetting = ConfigurationUtils.GetConfiguration();

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
                string fromAddress = string.IsNullOrWhiteSpace(appSetting.LostPwdFromAddress) ? "info@redbud.com" : appSetting.LostPwdFromAddress;
                emailSubject = EmailerResources.ForgetPasswordEmailSubject;

                emailBody = String.Format(EmailerResources.ForgetPasswordEmail, name, emailAddress, pwd);
                emailBody += "<br /><br />";
                emailBody += EmailerResources.EmailFooter;
                //emailBody = "Dear " + name + ", ";
                //emailBody += "\n\nA request was received to send your login information for " + appName + ". ";
                //emailBody += "Your login information is as follows:";
                //emailBody += "\n\nEmail address:  " + emailAddress;
                //emailBody += "\nPassword:  " + pwd;
                //emailBody += "\n\n<a href=\"" + siteURL + "\">Login to " + appName + "</a>";
                //emailBody += "\n\nSincerely,\n\nWebsite Administrator";

                // Run through and make sure that when someone resets their password we send an email off to everett@redbud.com
                extraEmailSubject = "New Password Requested";
                extraEmailBody = $"{emailAddress} has requested a new Password, the system has emailed them a new password.";
                extraRecipients = new List<string>
                {
                    "everett@redbud.com"
                };

                if (appSetting.EmailUseExternalSMTP)
                {
                    return (SendSMTPEmail(appSetting, fromAddress, fromAddress, recipients, emailSubject, emailBody)
                        && SendSMTPEmail(appSetting, fromAddress, fromAddress, extraRecipients, extraEmailSubject, extraEmailBody));

                }
                else
                {
                    return (SendPostmarkEmail(appSetting, appSetting.LostPwdFromAddress, appSetting.LostPwdFromAddress, recipients, emailSubject, emailBody)
                        && SendPostmarkEmail(appSetting, appSetting.LostPwdFromAddress, appSetting.LostPwdFromAddress, extraRecipients, extraEmailSubject, extraEmailBody));
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
                appSetting = ConfigurationUtils.GetConfiguration();

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
                Configuration appSetting = ConfigurationUtils.GetConfiguration();

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
                var emailSendHelper = new EmailSendHelper();

                htmlBody = "<html><body style=\"font-family:calibri,arial,helvetica,verdana,sans-serif;font-size:11pt\">" + body.Replace("\n", "<br />") + "</body></html>";

                var success = true;
                foreach (string address in recipients)
                {
                    MailMessage message = new MailMessage();
                    message.ReplyToList.Add(new MailAddress(replyAddress));
                    message.From = new MailAddress(fromAddress);
                    message.To.Add(new MailAddress(address));

                    message.Subject = subject;

                    message.Body = htmlBody;
                    message.IsBodyHtml = true;
                    success &= emailSendHelper.SendEmail(message).Result;
                }

                return success;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool SendEmailWithAttachment(Dictionary<int, string> recipients,
                                    string subject, string body, byte[] attachment, string fileName, string contentType, List<string> cc = null)
        {
            string htmlBody;
            Configuration appSetting = ConfigurationUtils.GetConfiguration();

            try
            {
                var emailSendHelper = new EmailSendHelper();

                htmlBody = "<html><body style=\"font-family:calibri,arial,helvetica,verdana,sans-serif;font-size:11pt\">" + body.Replace("\n", "<br />") + "</body></html>";

                foreach (var address in recipients)
                {
                    MailMessage message = new MailMessage();
                    message.ReplyToList.Add(new MailAddress(appSetting.CompanyEmail));
                    message.From = new MailAddress(appSetting.CompanyEmail);
                    message.To.Add(new MailAddress(address.Value));
                    if (cc != null)
                    {
                        foreach (var ccAddress in cc)
                        {
                            if (!string.IsNullOrWhiteSpace(ccAddress))
                            {
                                message.CC.Add(new MailAddress(ccAddress));
                            }
                        }
                    }
                    message.Subject = subject;

                    message.Body = htmlBody;
                    message.IsBodyHtml = true;

                    var ms = new MemoryStream(attachment);
                    System.Net.Mime.ContentType ct = new System.Net.Mime.ContentType(contentType);
                    System.Net.Mail.Attachment attach = new System.Net.Mail.Attachment(ms, ct);
                    attach.Name = fileName;
                    attach.ContentDisposition.FileName = fileName;
                    message.Attachments.Add(attach);

                    emailSendHelper.SendEmail(message).Wait();

                    using (MadduxEntities entities = new MadduxEntities())
                    {
                        var journal = entities.Journals.Create();
                        journal.CustomerID = address.Key; //Redbud Supply Inc.
                        journal.DateStamp = DateTime.Now;
                        journal.IsResolved = true;
                        journal.ResolvedDate = DateTime.Now;
                        journal.CreateDate = DateTime.Now;
                        journal.AssignedToId = 1;
                        journal.Notes = $"{subject} email was sent out to {address.Value} on {DateTime.Now:MMMM dd, yyyy}";

                        entities.Journals.Add(journal);
                        entities.SaveChanges();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool SendEmailWithoutAttachment(List<string> recipients,
                                    string subject, string body, List<string> cc = null)
        {
            string htmlBody;
            Configuration appSetting = ConfigurationUtils.GetConfiguration();

            try
            {
                var emailSendHelper = new EmailSendHelper();

                htmlBody = "<html><body style=\"font-family:calibri,arial,helvetica,verdana,sans-serif;font-size:11pt\">" + body.Replace("\n", "<br />") + "</body></html>";

                foreach (string address in recipients)
                {
                    MailMessage message = new MailMessage();
                    message.ReplyToList.Add(new MailAddress(appSetting.CompanyEmail));
                    message.From = new MailAddress(appSetting.CompanyEmail);

                    message.To.Add(new MailAddress(address));
                    if (cc != null)
                    {
                        foreach (var ccAddress in cc)
                        {
                            if (!string.IsNullOrWhiteSpace(ccAddress))
                            {
                                message.CC.Add(new MailAddress(ccAddress));
                            }
                        }
                    }
                    message.Subject = subject;

                    message.Body = htmlBody;
                    message.IsBodyHtml = true;

                    emailSendHelper.SendEmail(message).Wait();

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
