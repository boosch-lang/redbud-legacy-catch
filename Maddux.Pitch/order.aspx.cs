using Maddux.Pitch.LocalClasses;
using Redbud.BL.DL;
using Redbud.BL.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using static Redbud.BL.DL.Customer;

namespace Maddux.Pitch
{
    public partial class order : System.Web.UI.Page
    {
        private Dictionary<int, ShipDatesWithRackID> shipDatesWithRackID;

        protected void Page_Load(object sender, EventArgs e)
        {
            shipDatesWithRackID = new Dictionary<int, ShipDatesWithRackID>();
            try
            {
                if (!Page.IsPostBack)
                {
                    litMessage.Visible = false;
                    if (Request.QueryString["oSuccess"] != null && string.Equals(Request.QueryString["oSuccess"], "true"))
                    {
                        litMessage.Visible = true;
                        litMessage.Text = GenerateSuccess("Your order(s) have been successfully submitted.");
                    }

                    LoadAllPreviousOrders();
                    LoadOpenOrders();

                    //current logged in user
                    Customer currentCustomer = AppSession.Current.CurrentCustomer;

                    //Current user associations
                    List<AssociationResult> membershipAssociations = currentCustomer.MembershipAssociations();
                    if (membershipAssociations.Count > 0)
                    {
                        var html = string.Empty;

                        List<string> bannerMessages = new List<string>();
                        List<string> salesBannerMessages = new List<string>();

                        foreach (var membership in membershipAssociations)
                        {
                            if (!string.IsNullOrWhiteSpace(membership.BannerMessage) && membership.BannerEndDate >= DateTime.Now && membership.BannerStartDate <= DateTime.Now)
                            {
                                bannerMessages.Add(membership.BannerMessage);
                            }
                            if (!string.IsNullOrWhiteSpace(membership.SalesBannerMessage) && membership.SalesBannerEndDate >= DateTime.Now && membership.SalesBannerStartDate <= DateTime.Now)
                            {
                                salesBannerMessages.Add(membership.SalesBannerMessage);
                            }
                        }
                        foreach (string message in bannerMessages.Distinct())
                        {
                            html += StringTools.GenerateBanner(message);
                        }
                        foreach (string message in salesBannerMessages.Distinct())
                        {
                            html += StringTools.GenerateSalesBanner(message);
                        }

                        litWelcomeMessage.Text = html;
                    }
                }
            }
            catch (Exception ex)
            {
                litMessage.Visible = true;
                litMessage.Text = GenerateError(ex.Message);
            }
        }

        private void LoadAllPreviousOrders()
        {
            try
            {
                Customer theCustomer = AppSession.Current.CurrentCustomer;
                int programID = 1;

                var subCustomersTable = theCustomer.GetAllCustomerWithOrderHistory(programID);
                rptOtherShippedOrders.DataSource = subCustomersTable.Where(c => c != null);
                rptOtherShippedOrders.DataBind();

                litNoOtherShippedOrders.Visible = rptOtherOpenOrders.Items.Count == 0;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void LoadOrderHistory()
        {
            try
            {
                Customer theCustomer = AppSession.Current.CurrentCustomer;
                int programID = 1;

                List<QueryResult> subCustomersTable = theCustomer.GetSubCustomersWithOrderHistory(programID);
                rptOtherShippedOrders.DataSource = subCustomersTable.Where(c => c != null);
                rptOtherShippedOrders.DataBind();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void LoadOpenOrders()
        {
            try
            {
                Customer theCustomer = AppSession.Current.CurrentCustomer;

                var subCustomersTable = theCustomer.GetSubCustomersWithUnshippedOrders();
                rptOtherOpenOrders.DataSource = subCustomersTable;
                rptOtherOpenOrders.DataBind();

                if (subCustomersTable.Count > 0)
                {
                    divOtherOpenOrders.Visible = true;
                }
                else
                {
                    divOtherOpenOrders.Visible = false;
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = GenerateError(ex.Message);
            }
        }

        protected void rptOtherOpenOrders_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                Repeater repOpenOrders = e.Item.FindControl("repOpenOrders") as Repeater;
                var itemRow = (QueryResult)e.Item.DataItem;
                using (var db = new MadduxEntities())
                {
                    if (itemRow != null)
                    {
                        var customer = Convert.ToInt32(itemRow.CustomerID);
                        Customer subCustomer = db.Customers.FirstOrDefault(r => r.CustomerId == customer);

                        var unshippedOrders = subCustomer.UnshippedOrders().OrderBy(uo => uo.RequestedShipDate).ThenBy(x => x.OrderDate).ToList();

                        repOpenOrders.DataSource = unshippedOrders;
                        repOpenOrders.DataBind();

                    }
                }

            }
            catch (Exception ex)
            {
                litMessage.Text = GenerateError(ex.Message);
            }
        }

        protected void rptOtherShippedOrders_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                int programID = 1;
                Repeater repShippedOrders = e.Item.FindControl("repShippedOrders") as Repeater;
                var itemRow = (QueryResult)e.Item.DataItem;
                if (itemRow != null)
                {
                    using (var db = new MadduxEntities())
                    {
                        var id = Convert.ToInt32(itemRow.CustomerID);
                        Customer subCustomer = db.Customers.FirstOrDefault(r => r.CustomerId == id);

                        var otherShippedOrders = subCustomer.GetOrderHistory(programID);
                        otherShippedOrders = otherShippedOrders.OrderByDescending(so => so.ShipDate).ToList();

                        repShippedOrders.DataSource = otherShippedOrders;
                        repShippedOrders.DataBind();

                    }
                }

            }
            catch (Exception ex)
            {
                litMessage.Text = GenerateError(ex.Message);
            }
        }

        private string GenerateError(string error)
        {
            return $@"<div class='alert alert-danger d-flex justify-content-between align-items-center'>                          
                        <span >{error}</ span >
                       <button class='btn btn-outline btn-alert-close'  data-bs-dismiss='alert' aria-label='Close'>
                            <i class='far fa-times-circle'></i>
                        </button>
                        </div >";
        }
        private string GenerateSuccess(string message)
        {
            return $@"<div class='alert alert-success d-flex justify-content-between align-items-center'> 
                        <span >{message}</ span >
                         <button class='btn btn-outline btn-alert-close'  data-bs-dismiss='alert' aria-label='Close'>
                            <i class='far fa-times-circle'></i>
                        </button>
                        </div >";
        }
    }

    public class ShipdateObject
    {
        public string ShipDate { get; set; }
    }

    public class RackShipDate
    {
        public DateTime ShipDate { get; set; }
        public DateTime? OrderDeadlineDate { get; set; }
        public int Available { get; set; }
        public bool DeadlinePassed { get; set; }
        public string Availability { get; set; }
        public int RackID { get; set; }
    }
}