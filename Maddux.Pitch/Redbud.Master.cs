using Maddux.Pitch.LocalClasses;
using Redbud.BL;
using Redbud.BL.DL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;

namespace Maddux.Pitch
{
    public class CatalogModal
    {
        public int CatalogID { get; set; }
        public string CatalogDisplayName { get; set; }
        public string CatalogName { get; set; }
        public int? ProgramID { get; set; }
        public string ProgramName { get; set; }
        public string ShipDates { get; set; }
    }

    public class WeekModal
    {
        public int WeekID { get; set; }
        public DateTime ShipDate { get; set; }
        public string Event { get; set; }
    }

    public partial class Redbud : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Customer currentCustomer = AppSession.Current.CurrentCustomer;
                var membershipTable = currentCustomer.MembershipAssociations();
                if (membershipTable.Count > 0)
                {
                    string memberOf = "";
                    int membershipCounter = membershipTable.Count;
                    foreach (var membership in membershipTable)
                    {
                        memberOf += membership.AsscDesc.Substring(membership.AsscDesc.IndexOf("- ") + 1);
                        if (membershipCounter > 1)
                        {
                            memberOf += ",";
                        }
                        membershipCounter--;
                    }
                }

                SetCurrentPage();

                if (!Page.IsPostBack)
                {
                    int totalDraftOrders = 0;
                    List<int> subCartTable = new List<int>();
                    try
                    {
                        subCartTable = currentCustomer.GetSubCustomersWithDraftOrders(false).Select(x => x.CustomerID).Distinct().ToList();

                    }
                    catch (NullReferenceException ex)
                    {
                    }
                    foreach (var query in subCartTable)
                    {
                        using (MadduxEntities db = new MadduxEntities())
                        {
                            var cust = db.Customers.FirstOrDefault(x => x.CustomerId == query);
                            totalDraftOrders += cust.DraftOrders().Count;
                        }

                    }
                    cartTotal = totalDraftOrders.ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DateTime CalcDate(int month, int day)
        {
            int year = DateTime.Now.Year;
            DateTime now = DateTime.Now;

            if (month < DateTime.Now.Month && day < DateTime.Now.Day)
            {
                year++;
            }
            return new DateTime(year, month, day);
        }

        public string cartTotal
        {
            get
            {

                return supCartTotal.InnerText;
            }
            set
            {
                if (value != "0")
                {
                    supCartTotal.Visible = true;
                }
                else
                {
                    supCartTotal.Visible = false;
                }

                supCartTotal.InnerText = value;
            }
        }
        private void SetCurrentPage()
        {
            //Get the current name of your Page


            //Apply the class based on your page name
            switch (Request.Url.AbsolutePath.Split('/').LastOrDefault())
            {
                case "account.aspx":
                case "changepassword.aspx":
                    mnuAccount.Attributes["class"] = "active";
                    break;
                case "default.aspx":
                    mnuHome.Attributes["class"] = "active";
                    break;
                case "order.aspx":
                    mnuOrder.Attributes["class"] = "active";
                    break;
                case "cart.aspx":
                    break;
                case "resources.aspx":
                    mnuResources.Attributes["class"] = "active";
                    break;
                case "shop.aspx":
                case "neworder.aspx":
                case "orderdetail.aspx":
                case "filterproducts.aspx":
                    mnuShop.Attributes["class"] = "active";

                    break;
                default:
                    //Page was not found
                    break;
            }
        }

        public string GenerateError(string error)
        {
            return $@"<div class='alert alert-danger'> 
                        <button type='button' class='close' data-dismiss='alert' aria-label='Close'> 
                        <i class='fa fa-times'></i>
                        </button > 
                        <span >{error}</ span ></div >";
        }

        protected void btnCacti_Click(object sender, EventArgs e)
        {
            Response.Redirect("shop.aspx?filter=0");
        }

        protected void btnPond_Click(object sender, EventArgs e)
        {
            Response.Redirect("shop.aspx?filter=1");
        }

        protected void btnChristmasCollection_Click(object sender, EventArgs e)
        {
            Response.Redirect("shop.aspx?filter=2");
        }

        protected void btnBulb_Click(object sender, EventArgs e)
        {
            Response.Redirect("shop.aspx?filter=3");
        }

        protected void btnEaster_Click(object sender, EventArgs e)
        {
            Response.Redirect("shop.aspx?filter=4");
        }

        protected void btnMothersDay_Click(object sender, EventArgs e)
        {
            Response.Redirect("shop.aspx?filter=5");
        }

        protected void btnLabourDay_Click(object sender, EventArgs e)
        {
            Response.Redirect($@"shop.aspx?filter={(int)ProgramFilter.LabourDay}");
        }

        protected void btnHalloween_Click(object sender, EventArgs e)
        {
            Response.Redirect("shop.aspx?filter=7");
        }

        protected void btnChristmas_Click(object sender, EventArgs e)
        {
            Response.Redirect($@"shop.aspx?filter={(int)ProgramFilter.Christmas}");
        }



        protected void btnThanksgiving_Click(object sender, EventArgs e)
        {
            Response.Redirect($@"shop.aspx?filter={(int)ProgramFilter.Thanksgiving}");
        }
    }
}