using Redbud.BL.DL;
using Redbud.BL.Utils;
using System;
using System.Linq;
using System.Web.UI;

namespace Maddux.Catch
{
    public partial class forgotpwd : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Form.Count > 0)
                {
                    Page.Validate();

                    if (Page.IsValid)
                    {
                        this.lblInvalidUserName.Visible = false;
                        this.lblInactiveUser.Visible = false;
                        using (MadduxEntities db = new MadduxEntities())
                        {
                            string email = Request.Form["txtEmailAddress"].Trim();
                            User theUser = db.Users.FirstOrDefault(u => u.EmailAddress == email);
                            txtEmailAddress.Text = email;

                            Guid guid = Guid.NewGuid();
                            var newPass = guid.ToString().Substring(0, 8);

                            if (theUser != null)
                            {
                                if (theUser.Active)
                                {
                                    theUser.Password = FCSEncryption.Encrypt(newPass);
                                    db.SaveChanges();
                                    var emailer = new Emailer();
                                    if (emailer.SendLostPwdEmail(theUser.FirstName, theUser.EmailAddress, newPass, false))
                                    {
                                        pnlPwdSent.Visible = true;
                                    }

                                }
                                else
                                {
                                    LogActivity(theUser, "Pwd sent failure - inactive user");
                                    this.lblInactiveUser.Visible = true;
                                }
                            }
                            else
                            {
                                LogActivity(theUser, "Pwd sent failure - invalid email address (" + Request.Form["txtEmailAddress"].Trim() + ")");
                                this.lblInvalidUserName.Visible = true;
                            }
                        }

                    }
                }
                else
                {
                    if (Request.Cookies["lastEmailAddress"] != null)
                    {
                        txtEmailAddress.Text = Server.HtmlEncode(Request.Cookies["lastEmailAddress"].Value);
                    }
                    pnlPwdSent.Visible = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LogActivity(User theUser, string logDesc)
        {
            theUser.LogActivity(Session.SessionID, txtEmailAddress.Text, "forgotpwd.aspx", logDesc, Request.Browser.Platform, Request.Browser.Browser, Request.UserAgent, Request.Browser.Version, Request.UserHostAddress);
        }
    }
}