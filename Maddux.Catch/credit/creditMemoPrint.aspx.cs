using Redbud.BL.DL;
using Redbud.BL.Utils;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.UI.WebControls;

namespace Maddux.Catch.credit
{
    public partial class creditMemoPrint : System.Web.UI.Page
    {
        private int CreditID
        {
            get
            {
                if (ViewState["CreditID"] == null)
                {
                    ViewState["CreditID"] = Request.QueryString["creditID"] == null || Request.QueryString["creditID"] == "" ? -1 : (object)Request.QueryString["creditID"];
                }
                return Convert.ToInt32(ViewState["CreditID"].ToString());
            }

            set
            {
                ViewState["CreditID"] = value;
            }
        }
        private int CustomerID
        {
            get
            {
                if (ViewState["CustomerID"] == null)
                {
                    ViewState["CustomerID"] = Request.QueryString["customerID"] == null || Request.QueryString["customerID"] == "" ? -1 : (object)Request.QueryString["customerID"];
                }
                return Convert.ToInt32(ViewState["CustomerID"].ToString());
            }

            set
            {
                ViewState["CustomerID"] = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            using (var db = new MadduxEntities())
            {
                Customer customer = db.Customers.FirstOrDefault(cust => cust.CustomerId == CustomerID);
                Credit credit = db.Credits.Include(c => c.CreditItems).FirstOrDefault(c => c.CreditID == CreditID);

                lblnotes.InnerText = credit.CreditNotes;
                pSubtotal.InnerText = credit.SubTotal;
                GSTText.InnerText = (credit.GSTAmount + credit.PSTAmount).ToString("C");
                pShipping.InnerText = credit.FreightCredit.ToString("C");
                pTotal.InnerText = credit.Total.ToString("C");
                TaxTypeText.InnerText = $"{TaxUtilities.GetTaxTypeText(customer.State)} (#869614107)";
                //if (credit.PSTExempt)
                //{
                //    pstDiv.Attributes.Add("style", "display:none");
                //}
                lblCrediMemoNumber.InnerText = credit.CreditID.ToString();
                lblMemoDate.InnerText = credit.CreateDate.ToString("dd-MMM-yyyy");

                lblShippingName.InnerText = customer.Company;
                lblShippingAddress.InnerText = customer.Address;
                lblShippingCity.InnerText = customer.City;
                lblShippingState.InnerText = customer.State;
                lblShippingPostal.InnerText = customer.Zip;

                lblBillingName.InnerText = customer.BillingCompany;
                lblBillingAddress.InnerText = customer.BillingAddress;
                lblBillingCity.InnerText = customer.BillingCity;
                lblBillingState.InnerText = customer.BillingState;
                lblBillingZip.InnerText = customer.BillingZip;

                //List<CreditItem> creditItems = db.CreditItems.Where(ci => ci.CreditID == CreditID).ToList();

                dgvCreditItems.DataSource = credit.CreditItems.Select(ci => new
                {
                    ItemNo = ci.ItemNumber,
                    Quantity = $"{ci.Units}",
                    ci.Description,
                    ci.EachPrice,
                    ci.Total
                }).ToList();
                dgvCreditItems.DataBind();

            }
        }
    }
}