using Maddux.Catch.LocalClasses;
using Redbud.BL.DL;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Maddux.Catch
{
    public partial class myaccount : System.Web.UI.Page
    {
        /// <summary>
        /// User ID of a user admin wants to reset password
        /// </summary>
        private int ResetPasswordUserID
        {
            get
            {
                if (ViewState["ResetPasswordUserID"] == null)
                {
                    ViewState["ResetPasswordUserID"] = Request.QueryString["ResetPasswordUserID"] == null || Request.QueryString["UserId"] == ""
                        ? -1
                        : (object)Request.QueryString["ResetPasswordUserID"];
                }
                return Convert.ToInt32(ViewState["ResetPasswordUserID"].ToString());
            }

            set
            {
                ViewState["UserId"] = value;
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            Literal litPageHeader = (Literal)Master.FindControl("litPageHeader");

            if (ResetPasswordUserID != -1)
            {
                //Resets a user password if logged in user has rights
                try
                {
                    litPageHeader.Text = "Reset User Password";
                    using (var db = new MadduxEntities())
                    {
                        User userAccount = db.Users.FirstOrDefault(x => x.UserID == ResetPasswordUserID);
                        lblName.Text = "Name: " + userAccount.FirstName + " " + userAccount.LastName;
                        lblEmail.Text = "Email Address: " + userAccount.EmailAddress;
                        txtOriginalPassword.Text = userAccount.Password;
                        if (Request.Form.Count > 0)
                        {
                            Page.Validate();
                            if (Page.IsValid)
                            {
                                lblInvalidPassword.Visible = false;
                                userAccount.Password = Redbud.BL.Utils.FCSEncryption.Encrypt(txtPassword.Text);

                                if (db.SaveChanges() == 1)
                                {
                                    pnlPasswordChanged.Visible = true;
                                }
                            }
                        }
                    }
                }
                catch
                {

                }
            }
            else
            {
                //Resets the current logged in user password
                try
                {
                    lblName.Text = "Name: " + AppSession.Current.CurrentUser.FullName;
                    lblEmail.Text = "Email Address: " + AppSession.Current.CurrentUser.EmailAddress;

                    if (Request.Form.Count > 0)
                    {
                        Page.Validate();

                        if (Page.IsValid)
                        {
                            lblInvalidPassword.Visible = false;
                            using (var db = new MadduxEntities())
                            {
                                string unEncrypted = AppSession.Current.CurrentUser.PasswordUnEncrypted;
                                if (string.Equals(txtOriginalPassword.Text.Trim(), unEncrypted))
                                {
                                    User user = db.Users.FirstOrDefault(u => u.UserID == AppSession.Current.CurrentUser.UserID);
                                    if (user != null)
                                    {
                                        user.Password = Redbud.BL.Utils.FCSEncryption.Encrypt(txtPassword.Text);
                                        int changes = db.SaveChanges();
                                        if (changes == 1)
                                        {
                                            pnlPasswordChanged.Visible = true;
                                        }
                                    }

                                }
                                else
                                {
                                    lblInvalidPassword.Visible = true;
                                }
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        protected void cmdChangePassword_Click(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}