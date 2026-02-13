using Redbud.BL.DL;
using Redbud.BL.Utils;
using System;
using System.Linq;
using System.Web.UI;

namespace Maddux.Pitch
{
    public partial class forgotpwd : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string emailAddress;
            this.Title = "Redbud | Forgot Password";

            if (Request.Form.Count > 0)
            {
                Page.Validate();

                if (Page.IsValid)
                {
                    this.lblInvalidUserName.Visible = false;
                    this.lblInactiveUser.Visible = false;

                    emailAddress = txtEmailAddress.Text.Trim();
                    txtEmailAddress.Text = emailAddress;
                    using (var db = new MadduxEntities())
                    {
                        Customer theCustomer = db.Customers.FirstOrDefault(r => r.Email == emailAddress);

                        if (theCustomer != null)
                        {
                            if (theCustomer.Active)
                            {
                                try
                                {
                                    var emailer = new Emailer();
                                    Guid guid = Guid.NewGuid();
                                    var newPass = guid.ToString().Substring(0, 8);

                                    // Store password in database FIRST to ensure it's saved
                                    theCustomer.WebPassword = newPass;
                                    theCustomer.WebPassword_Hash = FCSEncryption.Encrypt(newPass);
                                    theCustomer.IsTemporaryPassword = true;
                                    
                                    // Save changes before sending email
                                    db.SaveChanges();
                                    
                                    // Send email after database is updated
                                    if (emailer.SendLostPwdEmail(theCustomer.FirstName, theCustomer.Email, newPass, false))
                                    {
                                        pnlPwdSent.Visible = true;
                                    }
                                    else
                                    {
                                        // Even if email fails, password has been changed
                                        LogActivity(theCustomer, "Password reset successful but email sending failed");
                                        pnlPwdSentError.Visible = true;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    // Log the specific exception
                                    LogActivity(theCustomer, $"Password reset failure: {ex.Message}");
                                    this.lblInactiveUser.Visible = true;
                                }
                            }
                            else
                            {
                                LogActivity(theCustomer, "Pwd sent failure - inactive user");
                                this.lblInactiveUser.Visible = true;
                            }
                        }
                        else
                        {
                            this.lblInvalidUserName.Visible = true;
                        }
                    }

                }
            }
            else
            {
                if (Request.QueryString.Count > 0 && Request.QueryString["e"] != null && Request.QueryString["e"].Trim() != "")
                {
                    this.txtEmailAddress.Text = Request.QueryString["e"].Trim();
                }

                if (txtEmailAddress.Text.Length == 0)
                {
                    if (Request.Cookies["lastEmailAddress"] != null)
                    {
                        txtEmailAddress.Text = Server.HtmlEncode(Request.Cookies["lastEmailAddress"].Value);
                    }
                }

                pnlPwdSent.Visible = false;
                pnlPwdSentError.Visible = false;
            }
        }

        private void LogActivity(Customer theCustomer, string logDesc)
        {
            theCustomer.LogActivity(Session.SessionID, txtEmailAddress.Text, "forgotpwd.aspx", logDesc, Request.Browser.Platform, Request.Browser.Browser, Request.UserAgent, Request.Browser.Version, Request.UserHostAddress);
        }
    }
}