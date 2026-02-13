using Maddux.Catch.Helpers;
using Maddux.Catch.LocalClasses;
using Redbud.BL.DL;
using Redbud.BL.Helpers;
using Redbud.BL.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Maddux.Catch.mailing
{
    public partial class newsletters : Page
    {
        private User currentUser;
        bool m_bUseExternalSMTP;
        string m_sSMTPServer;
        string m_sSMTPLogin;
        string m_sSTMPPwd;
        bool m_bUseSSL;
        int? m_lSTMPPort;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                litError.Text = string.Empty;
                currentUser = AppSession.Current.CurrentUser;
                Configuration config;
                using (var db = new MadduxEntities())
                {
                    config = db.Configurations.FirstOrDefault();
                    if (config == null)
                    {
                        throw new Exception("Configuration setting is missing");
                    }
                }

                m_bUseExternalSMTP = config.NewsletterUseExternalSMTP;
                m_sSMTPServer = config.NewsletterSMTPServer;
                m_sSMTPLogin = config.NewsletterSMTPLogin;
                m_sSTMPPwd = config.NewsletterSMTPPwd;
                m_bUseSSL = config.NewsletterUseSSL;
                m_lSTMPPort = config.NewsletterSMTPPort;

                if (!Page.IsPostBack)
                {
                    hfGuid.Value = Guid.NewGuid().ToString();
                    Title = "Maddux.Catch | Newsletters";
                    Literal litPageHeader = (Literal)Master.FindControl("litPageHeader");
                    litPageHeader.Text = "Create Newsletter";

                    var newsletterTemplatesFolder = config.NewsletterTemplatesFolder;
                    if (string.IsNullOrEmpty(newsletterTemplatesFolder))
                    {
                        throw new Exception("NewsletterTemplatesFolder configuration setting is missing");
                    }

                    string newsletterPath = Server.MapPath(newsletterTemplatesFolder);
                    string[] newslettersList = System.IO.Directory.GetFiles(newsletterPath, "*.html");

                    LoadLastMessage();
                    LoadAssociations();
                    LoadRegions();

                }
            }
            catch (Exception ex)
            {
                litError.Text = "<div class=\"alert alert-danger\" role=\"alert\"><strong>An error occurred when loading the newsletter templates:  " + ex.Message.ToString() + "</strong><div>";
            }
        }

        private void LoadLastMessage()
        {
            try
            {
                using (var db = new MadduxEntities())
                {
                    var config = db.Configurations.FirstOrDefault();
                    txtFrom.Text = config.LastNewsletterFromAddress;
                    txtSubject.Text = config.LastNewsletterSubject;
                    txtMessage.Text = config.LastNewsletterBody;
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }

        private void LoadAssociations()
        {
            try
            {
                using (var db = new MadduxEntities())
                {
                    lbAssociations.DataSource = db.Associations.Where(a => a.Active).OrderBy(a => a.AsscDesc).Select(a => new ListItem
                    {
                        Text = a.AsscDesc,
                        Value = a.AssociationID.ToString()
                    }).ToList();
                    lbAssociations.DataBind();
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }

        private void LoadRegions()
        {
            try
            {
                //Regions
                lbRegion.DataSource = RegionHelper.GetRegionsForSelectList(false);
                lbRegion.DataBind();
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }

        protected void cmdSendNewsletter_Click(object sender, EventArgs e)
        {
            string _sBodyHTML;
            System.IO.Stream _ioStreamAttachment;
            string TargetLocation = Server.MapPath("~/uploads/temp/" + hfGuid.Value);
            System.IO.DirectoryInfo di = new DirectoryInfo(TargetLocation);

            Server.ScriptTimeout = 6000;
            cmdSendNewsletter.Enabled = false;

            try
            {
                using (var db = new MadduxEntities())
                {
                    System.IO.StreamReader _srReader = new System.IO.StreamReader(Server.MapPath("NewsletterTemplate.html"));
                    _sBodyHTML = _srReader.ReadToEnd();
                    _srReader.Close();

                    string _sMessageText = txtMessage.Text.Trim();
                    _sBodyHTML = _sBodyHTML.Replace("[MAILBODY]", _sMessageText);

                    long _success = 0;
                    long _failed = 0;

                    var mailToCusts = (from c in FilterCustomers(db)
                                       where (c.Email != null && c.EmailRecievesNewsletters.HasValue && c.EmailRecievesNewsletters == true) || (c.AlternateEmail != null && c.AlternateEmailRecievesNewsletters.HasValue && c.AlternateEmailRecievesNewsletters == true)
                                       orderby c.Email
                                       group c by c.Email into r
                                       select new Reciepient()
                                       {
                                           Company = r.FirstOrDefault().Company,
                                           State = r.FirstOrDefault().State,
                                           CustID = r.Min(z => z.CustomerId),
                                           Email = r.Where(x => x.EmailRecievesNewsletters.HasValue && x.EmailRecievesNewsletters == true).Select(z => z.Email).FirstOrDefault(),
                                           AltEmail = r.Where(x => x.AlternateEmailRecievesNewsletters.HasValue && x.AlternateEmailRecievesNewsletters == true).Select(z => z.AlternateEmail).FirstOrDefault(),
                                       }).ToList();

                    List<Reciepient> successfulReciepients = new List<Reciepient>();
                    List<Reciepient> failedReciepients = new List<Reciepient>();

                    //Send the email
                    var emailSendHelper = new EmailSendHelper();

                    foreach (var cust in mailToCusts)
                    {
                        string customerEmails = string.Empty;
                        try
                        {
                            //Setup the mail message
                            MailMessage _mailMessage = new System.Net.Mail.MailMessage
                            {
                                From = new MailAddress(txtFrom.Text.Trim())
                            };
                            _mailMessage.Subject = txtSubject.Text.Trim();
                            _mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;

                            _mailMessage.IsBodyHtml = true;
                            _mailMessage.Body = _sBodyHTML;
                            _mailMessage.BodyEncoding = System.Text.Encoding.UTF8;

                            foreach (FileInfo file in di.GetFiles())
                            {
                                _ioStreamAttachment = file.OpenRead();
                                String _sAttachmentName = file.Name;
                                Attachment _mailAttachment = new Attachment(_ioStreamAttachment, _sAttachmentName);
                                _mailMessage.Attachments.Add(_mailAttachment);
                            }
                            if (!string.IsNullOrEmpty(cust.Email))
                            {
                                customerEmails += cust.Email.Trim() + ", ";
                                _mailMessage.To.Add(new MailAddress(cust.Email.Trim()));
                            }
                            if (!string.IsNullOrEmpty(cust.AltEmail))
                            {
                                customerEmails += cust.AltEmail.Trim() + ", ";
                                _mailMessage.To.Add(new MailAddress(cust.AltEmail.Trim()));
                            }

                            if (customerEmails.Length >= 2)
                            {
                                customerEmails = customerEmails.Substring(0, customerEmails.Length - 2);
                            }
                            emailSendHelper.SendEmail(_mailMessage).Wait();

                            Redbud.BL.DL.Journal journal = new Redbud.BL.DL.Journal
                            {
                                Notes = $"Sent newsletter \"{_mailMessage.Subject}\" to {customerEmails}" ?? string.Empty,
                                AssignedToId = currentUser.UserID,
                                IsResolved = true,
                                ResolvedDate = DateTime.Now,
                                CreateDate = DateTime.Today,
                                WhoInserted = currentUser.FullName,
                                DateUpdated = DateTime.Today,
                                WhoUpdated = currentUser.FullName,
                                CustomerID = cust.CustID,
                                DateStamp = DateTime.Now
                            };
                            db.Journals.Add(journal);
                            db.SaveChanges();

                            cust.Result = "Newsletter sent successfully";
                            _success++;
                        }
                        catch (Exception ex)
                        {
                            Redbud.BL.DL.Journal journal = new Redbud.BL.DL.Journal
                            {
                                Notes = $"Failed to send newsletter \"{txtSubject.Text.Trim()}\" to {customerEmails}, Reason: {ex.Message}" ?? string.Empty,
                                AssignedToId = currentUser.UserID,
                                IsResolved = false,
                                CreateDate = DateTime.Today,
                                WhoInserted = currentUser.FullName,
                                DateUpdated = DateTime.Today,
                                WhoUpdated = currentUser.FullName,
                                CustomerID = cust.CustID,
                                DateStamp = DateTime.Now
                            };
                            db.Journals.Add(journal);
                            db.SaveChanges();
                            cust.Result = ex.Message;
                            _failed++;
                        }
                    }

                    btnCancel.Visible = false;
                    cmdSendNewsletter.Visible = false;
                    litSuccessfullySent.Text = _success.ToString();
                    litFailedToSend.Text = _failed.ToString();
                    gvResults.DataSource = mailToCusts;
                    gvResults.DataBind();

                    MailMessage _summaryMessage;
                    _summaryMessage = new System.Net.Mail.MailMessage
                    {
                        From = new MailAddress(txtFrom.Text.Trim())
                    };

                    _summaryMessage.To.Add(new MailAddress(txtFrom.Text.Trim()));
                    _summaryMessage.Subject = txtSubject.Text.Trim() + " Summary";
                    _summaryMessage.SubjectEncoding = System.Text.Encoding.UTF8;
                    _summaryMessage.IsBodyHtml = true;
                    _summaryMessage.Body = GenerateSummary(mailToCusts);
                    emailSendHelper.SendEmail(_summaryMessage).Wait();

                    Literal litPageHeader = (Literal)Master.FindControl("litPageHeader");
                    litPageHeader.Text = "Newsletter Results";
                    pnlForm.Visible = false;
                    pnlPreview.Visible = true;

                    pnlForm.Visible = false;
                    pnlPreview.Visible = false;
                    pnlResult.Visible = true;

                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                string message = ex.Message;
                foreach (var eve in ex.EntityValidationErrors)
                {
                    message += string.Format("<br/>Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        message += string.Format("- {0}: {1}", ve.PropertyName, ve.ErrorMessage);
                    }
                }

                litError.Text = "<div class=\"alert alert-danger\" role=\"alert\">Error: " + " -- " + message + "</div>";
            }
            catch (Exception ex)
            {
                litError.Text = "<div class=\"alert alert-danger\" role=\"alert\">Error: " + " -- " + ex.Message.ToString() + "</div>";
            }
        }

        private string GenerateSummary(List<Reciepient> reciepients)
        {
            string HTML = $"<p>Hello {currentUser.FullName},</p><p>Below are the results for the \"{txtSubject.Text.Trim()}\" Newsletter</p>";
            HTML += "<table><thead><tr><th>Company</th><th>Province</th><th>Email</th><th>Alternate Email</th><th>Result</th></tr></thead><tbody>";
            foreach (Reciepient reciepient in reciepients)
            {
                HTML += $"<tr><td>{reciepient.Company}</td><td>{reciepient.State}</td><td>{reciepient.Email}</td><td>{reciepient.AltEmail}</td><td>{reciepient.Result}</td></tr>";
            }
            HTML += "</tbody></table>";
            return HTML;
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            string TargetLocation = Server.MapPath("~/uploads/temp/" + hfGuid.Value);
            System.IO.Directory.CreateDirectory(TargetLocation);

            if (Request.Files.Count > 0)
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFile postedFile = Request.Files[i];
                    if (postedFile.ContentLength > 0)
                    {
                        string FileName = postedFile.FileName;
                        //Determining file size.
                        int FileSize = postedFile.ContentLength;
                        //Creating a byte array corresponding to file size.
                        byte[] FileByteArray = new byte[FileSize];
                        //Posted file is being pushed into byte array.
                        postedFile.InputStream.Read(FileByteArray, 0, FileSize);
                        //Uploading properly formatted file to server.
                        string path = TargetLocation + "\\" + FileName;
                        postedFile.SaveAs(path);
                    }
                }
            }

            using (var db = new MadduxEntities())
            {
                var config = db.Configurations.FirstOrDefault();
                try
                {
                    config.LastNewsletterFromAddress = txtFrom.Text.Trim();
                    config.LastNewsletterSubject = txtSubject.Text.Trim();
                    config.LastNewsletterBody = txtMessage.Text.Trim();
                    db.SaveChanges();
                }
                catch (Exception exc_Config)
                {
                    throw new ApplicationException("<strong>The following error occurred when attempting to save the newsletter information:</strong> " + exc_Config.Message.ToString());
                }

                System.IO.StreamReader _srReader = new System.IO.StreamReader(Server.MapPath("NewsletterTemplate.html"));
                string _sBodyHTML = _srReader.ReadToEnd();
                _srReader.Close();

                string _sMessageText = txtMessage.Text.Trim();
                _sBodyHTML = _sBodyHTML.Replace("[MAILBODY]", _sMessageText);
                litEmailPreview.Text = _sBodyHTML;

                System.IO.DirectoryInfo di = new DirectoryInfo(TargetLocation);
                repUploadedFiles.DataSource = di.GetFiles();
                repUploadedFiles.DataBind();
                repAttachments.DataSource = di.GetFiles();
                repAttachments.DataBind();
                phNoAttachments.Visible = repUploadedFiles.Items.Count == 0;

                var mailToCusts = (from c in FilterCustomers(db)
                                   where (!string.IsNullOrEmpty(c.Email) && c.EmailRecievesNewsletters.HasValue && c.EmailRecievesNewsletters == true) || (c.AlternateEmail != null && c.AlternateEmailRecievesNewsletters.HasValue && c.AlternateEmailRecievesNewsletters == true)
                                   orderby c.Email
                                   group c by c.Email into r
                                   select new Reciepient()
                                   {
                                       Company = r.FirstOrDefault().Company,
                                       State = r.FirstOrDefault().State,
                                       CustID = r.Min(z => z.CustomerId),
                                       Email = r.Where(x => x.EmailRecievesNewsletters.HasValue && x.EmailRecievesNewsletters == true).Select(z => z.Email).FirstOrDefault(),
                                       AltEmail = r.Where(x => x.AlternateEmailRecievesNewsletters.HasValue && x.AlternateEmailRecievesNewsletters == true).Select(z => z.AlternateEmail).FirstOrDefault(),
                                   }).ToList();
                gvRecipients.DataSource = mailToCusts;
                gvRecipients.DataBind();

                Literal litPageHeader = (Literal)Master.FindControl("litPageHeader");
                litPageHeader.Text = "Preview Newsletter";
                pnlForm.Visible = false;
                pnlPreview.Visible = true;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Literal litPageHeader = (Literal)Master.FindControl("litPageHeader");
            litPageHeader.Text = "Create Newsletter";
            pnlPreview.Visible = false;
            pnlForm.Visible = true;
        }
        private List<Customer> FilterCustomers(MadduxEntities db)
        {
            List<int> _sAssocIncludeIds = new List<int>();
            List<string> _sRegion = new List<string>();

            foreach (ListItem _li in lbAssociations.Items)
            {
                if (_li.Selected)
                {
                    _sAssocIncludeIds.Add(int.Parse(_li.Value));
                }
            }

            foreach (ListItem _liRegion in lbRegion.Items)
            {
                if (_liRegion.Selected)
                {
                    _sRegion.Add(_liRegion.Value);
                }
            }

            var customerAsscsFilter = db.CustomerAsscs.Include("Customer").Where(x => x.Association.Active);
            List<CustomerAssc> customerAsscs = new List<CustomerAssc>();
            List<int> customerIDS = new List<int>();
            List<CustomerAssc> customerAsscsFiltered = new List<CustomerAssc>();
            List<Customer> customers = new List<Customer>();

            customerAsscs = db.CustomerAsscs.Include("Customer").Where(c => _sAssocIncludeIds.Contains(c.AssociationID) && c.Association.Active).ToList();
            customers = customerAsscs.Select(x => x.Customer).Distinct().ToList();

            if (_sRegion.Count > 0)
            {
                var selectedAreaIds = db.FreightCharges.Where(x => _sRegion.Contains(x.Region)).Select(x => x.AreaID).ToList();

                // handle bad zip code data,  there's a customer in the db with a zip code of '34'
                customers = customers
                    .Where(x => !string.IsNullOrWhiteSpace(x.Zip) && x.Zip.Length >= 3)
                    .Select(x => new { Customer = x, ZipPrefix = x.Zip.Trim().Substring(0, 3) })
                    .Where(x => selectedAreaIds.Contains(x.ZipPrefix))
                    .Select(x => x.Customer)
                    .ToList();
            }

            return customers.Where(x => x.Active).ToList();
        }

        protected void btnFileDelete_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton deleteButton = (LinkButton)sender;
                string fileName = deleteButton.CommandArgument;
                string path = Server.MapPath("~/uploads/temp/" + hfGuid.Value + "/" + fileName);
                FileInfo file = new FileInfo(path);
                if (file.Exists)
                {
                    file.Delete();
                }

                string TargetLocation = Server.MapPath("~/uploads/temp/" + hfGuid.Value);
                System.IO.DirectoryInfo di = new DirectoryInfo(TargetLocation);
                System.IO.Directory.CreateDirectory(TargetLocation);

                repUploadedFiles.DataSource = di.GetFiles();
                repUploadedFiles.DataBind();
                repAttachments.DataSource = di.GetFiles();
                repAttachments.DataBind();
                phNoAttachments.Visible = repUploadedFiles.Items.Count == 0;
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }
    }
    public class Reciepient
    {
        public string Company { get; set; }
        public string State { get; set; }
        public int CustID { get; set; }
        public string Email { get; set; }
        public string AltEmail { get; set; }
        public string Result { get; set; }
    }
}