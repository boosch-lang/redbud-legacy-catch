using Redbud.BL.DL;
using System;
using System.Linq;
using System.Web.Security;
using System.Web.UI;

namespace Maddux.Catch
{
    public partial class login : System.Web.UI.Page
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
                        this.lblInvalidPassword.Visible = false;
                        this.lblInactiveUser.Visible = false;

                        using (var db = new MadduxEntities())
                        {
                            db.Database.CommandTimeout = 180;

                            var email = Request.Form["txtEmailAddress"].Trim();
                            User theUser = db.Users.FirstOrDefault(u => u.EmailAddress == email);
                            txtEmailAddress.Text = Request.Form["txtEmailAddress"].ToString().Trim();

                            if (theUser != null)
                            {
                                if (String.Compare(theUser.PasswordUnEncrypted, Request.Form["txtPassword"].Trim(), false) == 0)
                                {
                                    if (theUser.Active)
                                    {
                                        LogActivity(theUser, "Login success");

                                        //AppSession.Current.CurrentUser = theUser;
                                        FormsAuthentication.SetAuthCookie(theUser.UserID.ToString(), true);
                                        //Add the login name to a cookie so we can set it next time
                                        Response.Cookies["lastEmailAddress"].Value = theUser.EmailAddress;
                                        Response.Cookies["lastEmailAddress"].Expires = DateTime.Now.AddYears(1);

                                        FormsAuthentication.RedirectFromLoginPage(theUser.EmailAddress, false);
                                    }
                                    else
                                    {
                                        LogActivity(theUser, "Login failure - inactive user");
                                        this.lblInactiveUser.Visible = true;
                                    }
                                }
                                else
                                {
                                    LogActivity(theUser, "Password failure (" + Request.Form["txtPassword"].Trim() + ")");
                                    this.lblInvalidPassword.Visible = true;
                                }
                            }
                            else
                            {
                                //LogActivity(theUser, "Login failure - invalid email address (" + Request.Form["txtEmailAddress"].Trim() + ")");
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

                    pnlSignedOut.Visible = Request.QueryString["logout"] != null;
                }
            }
            catch (Exception ex)
            {

                litError.Text = Redbud.BL.Utils.StringTools.GenerateError(ex.Message);
            }

        }

        private void LogActivity(User theUser, string logDesc)
        {
            theUser.LogActivity(Session.SessionID, txtEmailAddress.Text, "login.aspx", logDesc, Request.Browser.Platform, Request.Browser.Browser, Request.UserAgent, Request.Browser.Version, Request.UserHostAddress);
        }
    }
}