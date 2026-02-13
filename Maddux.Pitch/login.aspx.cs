using Maddux.Pitch.LocalClasses;
using Redbud.BL.DL;
using Redbud.BL.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using System.Web.UI;

namespace Maddux.Pitch
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            using (var db = new MadduxEntities())
            {
                db.Database.CommandTimeout = 180;

                string emailAddress;

                this.Title = "Redbud | Horticultural Broker";

                try
                {
                    // If a token is passed in, try to auto login using the token
                    var token = Request.QueryString.Get("token");
                    if (!String.IsNullOrWhiteSpace(token))
                    {
                        int customerId = JWTHelper.GetUserId(token);
                        Customer theCustomer = db.Customers.FirstOrDefault(r => r.CustomerId == customerId);
                        if (theCustomer != null && theCustomer.Active)
                        {
                            AppSession.Current.CurrentCustomer = theCustomer;
                            FormsAuthentication.SetAuthCookie(theCustomer.CustomerId.ToString(), false);
                            FormsAuthentication.RedirectFromLoginPage(theCustomer.Email, false);
                        }
                        else
                        {
                            this.lblInvalidUserName.Visible = true;
                        }
                    }
                    else if (Request.Form.Count > 0)
                    {
                        Page.Validate();

                        if (Page.IsValid)
                        {

                            this.lblInvalidUserName.Visible = false;
                            this.lblInvalidPassword.Visible = false;
                            this.lblInactiveUser.Visible = false;

                            emailAddress = txtEmailAddress.Text.Trim();
                            //Customer theCustomer = new Customer(emailAddress);
                            Customer theCustomer = db.Customers.FirstOrDefault(r => r.Email == emailAddress);
                            if (theCustomer != null && theCustomer.Active)
                            {
                                if (String.Compare(FCSEncryption.Decrypt(theCustomer.WebPassword_Hash), (txtPassword.Text.Trim()), false) == 0)
                                {
                                    if (theCustomer.Active)
                                    {
                                        LogActivity(theCustomer, "Login success");

                                        AppSession.Current.CurrentCustomer = theCustomer;
                                        FormsAuthentication.SetAuthCookie(theCustomer.CustomerId.ToString(), chkRememberMe.Checked);
                                        //Add the login name to a cookie so we can set it next time
                                        Response.Cookies["lastEmailAddress"].Value = theCustomer.Email;
                                        Response.Cookies["lastEmailAddress"].Expires = DateTime.Now.AddYears(1);
                                        FormsAuthentication.RedirectFromLoginPage(theCustomer.Email, false);
                                    }
                                    else
                                    {
                                        LogActivity(theCustomer, "Login failure - inactive customer");
                                        this.lblInactiveUser.Visible = true;
                                        ScrollToLoginForm();

                                    }
                                }
                                else
                                {
                                    LogActivity(theCustomer, "Password failure (" + txtPassword.Text.Trim() + ")");
                                    this.lblInvalidPassword.Visible = true;
                                    ScrollToLoginForm();
                                }
                            }
                            else
                            {
                                LogActivity(theCustomer, "Login failure - invalid email address (" + txtEmailAddress.Text.Trim() + ")");
                                this.lblInvalidUserName.Visible = true;
                                ScrollToLoginForm();
                            }
                        }
                    }
                    else
                    {
                        if (Request.Cookies["lastEmailAddress"] != null)
                        {
                            txtEmailAddress.Text = Server.HtmlEncode(Request.Cookies["lastEmailAddress"].Value);
                        }
                    }
                }
                catch (Exception ex)
                {
                    litMessage.Text = StringTools.GenerateError(ex.Message);
                }

            }

        }

        private void ScrollToLoginForm()
        {
            // Register the script to execute after the page is fully loaded
            ScriptManager.RegisterStartupScript(
                this,
                GetType(),
                "ScrollToLogin",
                "window.onload = function() { scrollToLoginForm(); };",
                true);
        }

        private void LogActivity(Customer theCustomer, string logDesc)
        {
            if (theCustomer != null)
            {
                if (!theCustomer.Email.ToLower().Contains("redbud") && !theCustomer.Company.ToLower().Contains("redbud"))
                {
                    theCustomer.LogActivity(Session.SessionID, txtEmailAddress.Text, "login.aspx", logDesc, Request.Browser.Platform, Request.Browser.Browser, Request.UserAgent, Request.Browser.Version, Request.UserHostAddress);
                }
            }
        }
    }
}