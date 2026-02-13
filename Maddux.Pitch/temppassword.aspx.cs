using Maddux.Pitch.LocalClasses;
using Redbud.BL.DL;
using Redbud.BL.Utils;
using System;
using System.Linq;

namespace Maddux.Pitch
{
    public partial class temppassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                litSuccessMessage.Text = string.Empty;
                pnlPasswordChanged.Visible = false;
                pnlForm.Visible = true;

                var theCustomer = AppSession.Current.CurrentCustomer;
                if (!theCustomer.IsTemporaryPassword)
                {
                    Response.Redirect("/", false);
                    Context.ApplicationInstance.CompleteRequest();
                }

            }
        }

        protected void cmdChangePassword_Click(object sender, EventArgs e)
        {
            try
            {
                lblInvalidPassword.Visible = false;
                pnlPasswordChanged.Visible = false;
                litSuccessMessage.Text = string.Empty;

                Customer currentCustomer = AppSession.Current.CurrentCustomer;

                if (string.Equals(txtPassword.Text, txtConfirmPassword.Text))
                {
                    using (var db = new MadduxEntities())
                    {
                        var cust = db.Customers.FirstOrDefault(r => r.CustomerId == currentCustomer.CustomerId);
                        cust.WebPassword_Hash = FCSEncryption.Encrypt(txtPassword.Text);
                        cust.IsTemporaryPassword = false;
                        db.SaveChanges();

                        //update session customer
                        AppSession.Current.CurrentCustomer = cust;
                        litSuccessMessage.Text = "Your password has been successfully changed.";
                        pnlPasswordChanged.Visible = true;
                        pnlForm.Visible = false;

                    }
                }
                else
                {
                    lblInvalidPassword.Visible = true;
                    lblInvalidPassword.Focus();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnContinue_Click(object sender, EventArgs e)
        {
            Response.Redirect("/", false);
        }

        protected void btnNo_Click(object sender, EventArgs e)
        {
            try
            {
                lblInvalidPassword.Visible = false;
                litSuccessMessage.Text = string.Empty;
                pnlPasswordChanged.Visible = false;

                //customer has chosen to skip,  set password as no longer temporary
                Customer currentCustomer = AppSession.Current.CurrentCustomer;

                using (var db = new MadduxEntities())
                {
                    var cust = db.Customers.FirstOrDefault(r => r.CustomerId == currentCustomer.CustomerId);
                    cust.IsTemporaryPassword = false;
                    db.SaveChanges();

                    //update session customer
                    AppSession.Current.CurrentCustomer = cust;
                    litSuccessMessage.Text = "If you change your mind and want to update your password in the future, you can do so by using the change password section of your profile page. ";
                    pnlPasswordChanged.Visible = true;
                    pnlForm.Visible = false;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}