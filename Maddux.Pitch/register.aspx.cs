using Redbud.BL.DL;
using Redbud.BL.Utils;
using System;
using System.Linq;
using System.Web.UI;
using static Redbud.BL.Utils.Recaptcha;

namespace Maddux.Pitch
{
    public partial class register : System.Web.UI.Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            try
            {
                using (var db = new MadduxEntities())
                {
                    this.Title = "Redbud | Register";
                    var recaptchaService = new RecaptchaService();

                    if (!Page.IsPostBack)
                    {
                        Lookup.LoadProvinceDropDown(ref cboProvince, false, false, false, -1);
                        cboProvince.SelectedValue = "ON";
                    }

                    if (Request.Form.Count > 0)
                    {
                        Page.Validate();
                        var recaptchaValid = false;
                        if (Page.IsValid)
                        {
                            try
                            {
                                var recaptchaToken = hidRecaptchaToken.Value;
                                recaptchaValid = await recaptchaService.ValidateRecaptcha(recaptchaToken, "register", Request.UserHostAddress);
                                if (!recaptchaValid)
                                    throw new Exception("Recaptcha verification failed.");
                            }
                            catch
                            {
                                throw new Exception("Recaptcha verification failed.");
                            }
                        }

                        if (Page.IsValid && recaptchaValid)
                        {
                            ClientScriptManager cs = Page.ClientScript;
                            Customer testCustomer = db.Customers.FirstOrDefault(r => r.Email == txtEmailAddress.Text.Trim());
                            if (testCustomer != null)
                            {
                                string confirmScript = "retval = confirm(\"It appears that we already have your email address on file.\\n\\nWould you like to be taken to the password request form to have your password emailed to you?\");" +
                                                        "if (retval == true) {" +
                                                        "window.location = \"forgotpwd.aspx?e=" + txtEmailAddress.Text.Trim() + "\"" +
                                                        "}";
                                cs.RegisterStartupScript(this.GetType(), "ConfirmForgotPwdScript", confirmScript, true);
                            }
                            else
                            {
                                Emailer emailTool = new Emailer();
                                if (emailTool.SendRegistrationEmail(txtCompanyName.Text.Trim(), txtContact.Text.Trim(), txtEmailAddress.Text.Trim(), txtAddress.Text.Trim(), txtCity.Text.Trim(), cboProvince.SelectedValue, txtPostalCode.Text.Trim(), txtPhone.Text.Trim(), txtComments.Text.Trim()))
                                {
                                    pnlSuccess.Visible = true;
                                    pnlFailure.Visible = false;
                                    string registrationSentScript = "alert('Your request has been emailed to the Redbud Sales Team.  You will receive your login information via email when your request has been processed.'); window.location = \"default.aspx\";";
                                    cs.RegisterStartupScript(this.GetType(), "ConfirmRegistrationScript", registrationSentScript, true);
                                }
                                else
                                {
                                    pnlSuccess.Visible = false;
                                    pnlFailure.Visible = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        pnlSuccess.Visible = false;
                        pnlFailure.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }
    }
}