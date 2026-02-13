using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Redbud.BL.Helpers
{
    public class EmailSendHelper
    {
        private readonly string _smtpServer;
        private readonly ushort _smtpPort;
        private readonly string _allowedDomain;
        private readonly string _sendGridApiKey;

        public EmailSendHelper()
        {
            _allowedDomain = ConfigurationManager.AppSettings["Email_AllowedDomain"];
            _smtpServer = ConfigurationManager.AppSettings["SMTP_Server"] ?? "localhost";
            _smtpPort = ushort.Parse(ConfigurationManager.AppSettings["SMTP_Port"] ?? "25");
            _sendGridApiKey = ConfigurationManager.AppSettings["SendGrid_API_Key"];
        }

        /// <summary>
        /// Sends an email (edit this function to change which function gets called)
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<bool> SendEmail(MailMessage message)
        {
            return await SendEmailUsingSendGrid(message).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends an email using SendGrid
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<bool> SendEmailUsingSendGrid(MailMessage message)
        {
            try
            {
                var sendGridMsg = new SendGridMessage();
                if (!message.From.Address.EndsWith($"@{_allowedDomain}"))
                {
                    throw new Exception($"Emails must be sent from an email address ending in @{_allowedDomain}");
                }
                sendGridMsg.SetFrom(new EmailAddress(message.From.Address));
                sendGridMsg.SetSubject(message.Subject);
                if (message.IsBodyHtml)
                {
                    sendGridMsg.AddContent(MimeType.Html, message.Body);
                }
                else
                {
                    sendGridMsg.AddContent(MimeType.Text, message.Body);
                }
                //sendGridMsg.AddTo(new EmailAddress("developer@webility.ca"));
                foreach (var to in message.To)
                {
                    sendGridMsg.AddTo(!string.IsNullOrEmpty(to.DisplayName) ? new EmailAddress(to.Address, to.DisplayName) : new EmailAddress(to.Address));
                }
                foreach (var cc in message.CC)
                {
                    sendGridMsg.AddCc(!string.IsNullOrEmpty(cc.DisplayName) ? new EmailAddress(cc.Address, cc.DisplayName) : new EmailAddress(cc.Address));
                }
                foreach (var bcc in message.Bcc)
                {
                    sendGridMsg.AddBcc(!string.IsNullOrEmpty(bcc.DisplayName) ? new EmailAddress(bcc.Address, bcc.DisplayName) : new EmailAddress(bcc.Address));
                }
                if (message.ReplyToList.Count > 0)
                {
                    sendGridMsg.ReplyTos = new List<EmailAddress>();
                    foreach (var replyTo in message.ReplyToList)
                    {
                        sendGridMsg.ReplyTos.Add(!string.IsNullOrEmpty(replyTo.DisplayName) ? new EmailAddress(replyTo.Address, replyTo.DisplayName) : new EmailAddress(replyTo.Address));
                    }
                }
                if (message.Attachments.Count > 0)
                {
                    sendGridMsg.Attachments = new List<SendGrid.Helpers.Mail.Attachment>();
                    foreach (var attachment in message.Attachments)
                    {
                        var newAttachment = new SendGrid.Helpers.Mail.Attachment();
                        using (var attachmentContentMemoryStream = new MemoryStream())
                        {
                            attachment.ContentStream.CopyTo(attachmentContentMemoryStream);
                            newAttachment.Content = Convert.ToBase64String(attachmentContentMemoryStream.ToArray());
                        }
                        newAttachment.Filename = !string.IsNullOrEmpty(attachment.Name)
                            ? attachment.Name
                            : !string.IsNullOrEmpty(attachment.ContentDisposition.FileName)
                                ? attachment.ContentDisposition.FileName
                                : "attachment";
                        newAttachment.Type = attachment.ContentType.MediaType;
                        newAttachment.Disposition = "attachment";
                        sendGridMsg.Attachments.Add(newAttachment);
                    }
                }

                var client = new SendGridClient(_sendGridApiKey);
                var response = await client.SendEmailAsync(sendGridMsg).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Sending Sendgrid email returned non-success status code: {response.StatusCode}");
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        /// <summary>
        /// Sends an email using SMTP
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<bool> SendEmailUsingSmtp(MailMessage message)
        {
            try
            {
                using (var client = new SmtpClient(_smtpServer, _smtpPort))
                {
                    await client.SendMailAsync(message);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
