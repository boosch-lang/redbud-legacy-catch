using Maddux.Pitch.LocalClasses;
using Redbud.BL.DL;
using Redbud.BL.Utils;
using System;
using System.Linq;

namespace Maddux.Pitch
{
    public partial class changepassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void cmdChangePassword_Click(object sender, EventArgs e)
        {
            try
            {
                lblInvalidPassword.Visible = false;
                pnlPasswordChanged.Visible = false;

                Customer currentCustomer = AppSession.Current.CurrentCustomer;

                if (txtOriginalPassword.Text == FCSEncryption.Decrypt(currentCustomer.WebPassword_Hash))
                {
                    if (string.Equals(txtPassword.Text, txtConfirmPassword.Text))
                    {
                        using (var db = new MadduxEntities())
                        {
                            var cust = db.Customers.FirstOrDefault(r => r.CustomerId == currentCustomer.CustomerId);
                            cust.WebPassword_Hash = FCSEncryption.Encrypt(txtPassword.Text);
                            db.SaveChanges();
                            pnlPasswordChanged.Visible = true;
                        }
                    }
                    else
                    {
                        lblInvalidPassword.Visible = true;
                        lblInvalidPassword.Focus();
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
    }
}