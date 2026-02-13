using Maddux.Pitch.LocalClasses;
using Redbud.BL.DL;
using Redbud.BL.Utils;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Maddux.Pitch
{
    public partial class account : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    LoadComboBoxes();
                    LoadFields();
                }
                catch (Exception ex)
                {
                    litMessage.Text = StringTools.GenerateError(ex.Message);
                }

            }
        }

        private void LoadComboBoxes()
        {
            try
            {
                Customer theCustomer = AppSession.Current.CurrentCustomer;

                Lookup.LoadContactTitlesDropDown(ref ddlShippingSalutation);
                Lookup.LoadContactTitlesDropDown(ref ddlBillingSalutation);

                if (string.Equals(theCustomer.Country.ToLower(), "canada"))
                    Lookup.LoadCanadianProvinceDropDown(ref ddlShippingProvince, false, false, false, -1);
                else if (string.Equals(theCustomer.Country.ToLower(), "usa"))
                    Lookup.LoadUSProvinceDropDown(ref ddlShippingProvince, false, false, false, -1);
                else
                    Lookup.LoadProvinceDropDown(ref ddlShippingProvince, false, false, false, -1);

                if (string.Equals(theCustomer.BillingCountry.ToLower(), "canada"))
                    Lookup.LoadCanadianProvinceDropDown(ref ddlBillingProvince, false, false, false, -1);
                else if (string.Equals(theCustomer.BillingCountry.ToLower(), "usa"))
                    Lookup.LoadUSProvinceDropDown(ref ddlBillingProvince, false, false, false, -1);
                else
                    Lookup.LoadProvinceDropDown(ref ddlShippingProvince, false, false, false, -1);


                Lookup.LoadCountryDropDown(ref ddlShippingCountry, false);
                Lookup.LoadCountryDropDown(ref ddlBillingCountry, false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadFields()
        {
            try
            {
                Customer theCustomer = AppSession.Current.CurrentCustomer;

                txtShippingCompany.Text = theCustomer.Company;
                ddlShippingSalutation.SelectedValue = theCustomer.Salutation;
                txtShippingFirstName.Text = theCustomer.FirstName;
                txtShippingLastName.Text = theCustomer.LastName;
                txtShippingAddress.Text = theCustomer.Address;
                txtShippingCity.Text = theCustomer.City;
                ddlShippingProvince.SelectedValue = theCustomer.State;
                txtShippingPostalCode.Text = theCustomer.Zip;
                ddlShippingCountry.SelectedValue = theCustomer.Country;

                txtBillingCompany.Text = theCustomer.BillingCompany;
                ddlBillingSalutation.SelectedValue = theCustomer.BillingSalutation;
                txtBillingFirstName.Text = theCustomer.BillingFirstName;
                txtBillingLastName.Text = theCustomer.BillingLastName;
                txtBillingAddress.Text = theCustomer.BillingAddress;
                txtBillingCity.Text = theCustomer.BillingCity;
                ddlBillingProvince.SelectedValue = theCustomer.BillingState;
                txtBillingPostalCode.Text = theCustomer.BillingZip;
                ddlBillingCountry.SelectedValue = theCustomer.BillingCountry;
                txtBillingPhone.Text = theCustomer.BillingPhone;

                txtPhone.Text = theCustomer.Phone;
                txtMobile.Text = theCustomer.CellPhone;
                txtFax.Text = theCustomer.Fax;

                txtEmail.Text = theCustomer.Email;
                txtAltEmail.Text = theCustomer.AlternateEmail;
                txtWebsite.Text = theCustomer.WebSite;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool SaveAccountDetail()
        {
            try
            {
                Page.Validate();
                if (Page.IsValid)
                {
                    using (var db = new MadduxEntities())
                    {
                        var customer = AppSession.Current.CurrentCustomer;

                        Customer theCustomer = db.Customers.FirstOrDefault(r => r.CustomerId == customer.CustomerId);

                        theCustomer.Salutation = ddlShippingSalutation.SelectedValue.ToString();
                        theCustomer.FirstName = txtShippingFirstName.Text.Trim();
                        theCustomer.LastName = txtShippingLastName.Text.Trim();

                        theCustomer.BillingSalutation = ddlBillingSalutation.SelectedValue.ToString();
                        theCustomer.BillingFirstName = txtBillingFirstName.Text.Trim();
                        theCustomer.BillingLastName = txtBillingLastName.Text.Trim();
                        theCustomer.BillingPhone = txtBillingPhone.Text.Trim();

                        theCustomer.Phone = txtPhone.Text.Trim();
                        theCustomer.CellPhone = txtMobile.Text.Trim();
                        theCustomer.Fax = txtFax.Text.Trim();
                        if (StringTools.IsEmail(txtEmail.Text.Trim()) == true)
                        {
                            theCustomer.Email = txtEmail.Text.Trim();
                        }
                        else
                        {
                            litMessage.Text = StringTools.GenerateError("Provided email address is not a Valid email address.");
                            return false;
                        }
                        if (string.IsNullOrWhiteSpace(txtAltEmail.Text.Trim()) == false)
                        {
                            if (StringTools.IsEmail(txtAltEmail.Text.Trim()) == true)
                            {
                                theCustomer.AlternateEmail = txtAltEmail.Text.Trim();
                            }
                            else
                            {
                                litMessage.Text = StringTools.GenerateError("Provided alternate email address is not a valid email address.");
                                return false;
                            }
                        }
                        theCustomer.WebSite = txtWebsite.Text.Trim();

                        try
                        {
                            db.SaveChanges();
                            pnlAccountChanged.Visible = true;


                            // repopulate the AppSession.Current.Customer with the new entity
                            // (we would like to revisit how this customer data is handled in the session)
                            AppSession.Current.CurrentCustomer = theCustomer;

                            return true;
                        }
                        catch (Exception)
                        {
                            return false;
                        }

                    }

                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            try
            {
                SaveAccountDetail();
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }
    }
}