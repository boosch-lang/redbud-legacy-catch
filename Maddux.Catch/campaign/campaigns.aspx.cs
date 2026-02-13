using Redbud.BL.DL;
using Redbud.BL.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Maddux.Catch.Campaign
{
    public class CampaignListObject
    {
        public DateTime? Shipdate { get; set; }
        public DateTime SalesEnd { get; set; }
        public DateTime SalesStart { get; set; }
        public int RacksOrdered { get; set; }
        public string CustomersReachedPercent { get; set; }
        public int CustomersReachedNumber { get; set; }
        public int CampaignId { get; set; }
        public int Goal { get; set; }
        public string CampaignName { get; set; }
    }

    public class RackDetails
    {
        public int? RackID { get; set; }
        public int CatalogID { get; set; }
        public string RackName { get; set; }
        public int Count { get; set; }
        public string CatalogName { get; set; }
    }

    public partial class campaigns : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            try
            {

                if (!Page.IsPostBack)
                {
                    Literal litPageHeader = (Literal)Master.FindControl("litPageHeader");
                    Literal litTotal = (Literal)Master.FindControl("litTotal");
                    litPageHeader.Text = "Campaigns";
                    litTotal.Text = "";
                    LoadFilters();
                    LoadCampaigns();
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }

        private void LoadFilters()
        {
            try
            {
                Lookup.LoadProgramDropDown(ref ddlFilterProgram, true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadCampaigns()
        {
            using (MadduxEntities madduxEntities = new MadduxEntities())
            {
                IQueryable<Redbud.BL.DL.Campaign> campaigns =
                    madduxEntities
                       .Campaigns
                       .Include(c => c.CampaignShipdates)
                       .Include(c => c.CampaignShipdates.Select(x => x.ProductCatalogShipDate))
                       .AsNoTracking()
                       .Where(c => c.CampaignShipdates.Any(cs => cs.ProductCatalogShipDate.ShipDate > DateTime.Today) || ((c.Shipdate.HasValue && c.Shipdate.Value > DateTime.Today)))
                       .OrderBy(c => c.SalesEnd);

                if (int.TryParse(ddlFilterProgram.SelectedValue, out int selectedProgram) && selectedProgram > 0)
                {
                    campaigns = campaigns.Where(c => c.ProgramID == selectedProgram);
                }

                List<CampaignListObject> campaignList = new List<CampaignListObject>();

                try
                {
                    var catalogIDs = campaigns.SelectMany(x => x.CampaignShipdates).Select(x => x.CatalogID);

                    var orders = madduxEntities.Orders
                        .Include(o => o.OrderRacks)
                        .Include(o => o.OrderRacks.Select(x => x.ProductCatalogRack))
                        .Where(o => o.OrderRacks.Any(x => catalogIDs.Contains(x.ProductCatalogRack.CatalogID)))
                        .ToList();

                    List<Redbud.BL.DL.Campaign> test = campaigns.ToList();
                    foreach (Redbud.BL.DL.Campaign campaign in test)
                    {

                        CampaignListObject campaignListObject = new CampaignListObject
                        {
                            CampaignName = campaign.CampaignName,
                            CampaignId = campaign.CampaignID,
                            Goal = campaign.Goal,
                            SalesEnd = campaign.SalesEnd,
                            SalesStart = campaign.SalesStart,
                            Shipdate = campaign.Shipdate,
                            RacksOrdered = campaign.GetRacksOrdered(orders),
                            CustomersReachedNumber = campaign.GetCustomersReached()
                        };
                        campaignListObject.CustomersReachedPercent = ((double)campaignListObject.CustomersReachedNumber / campaign.GetCustomerCount()).ToString("#.## %");
                        campaignList.Add(campaignListObject);
                    }
                }
                catch
                {

                    throw;
                }
                dgvCampaigns.DataSource = campaignList;
                dgvCampaigns.DataBind();
            }
        }

        protected void ddlFilterProgram_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadCampaigns();
        }


        protected void dgvCampaigns_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    GridView dgvCampaignDetails = e.Row.FindControl("dgvCampaignDetails") as GridView;
                    HiddenField hdnCampaignID = e.Row.FindControl("hdnCampaignID") as HiddenField;
                    int CampaignID = int.Parse(hdnCampaignID.Value);

                    using (MadduxEntities madduxEntities = new MadduxEntities())
                    {

                        Redbud.BL.DL.Campaign campaign = madduxEntities.Campaigns.FirstOrDefault(c => c.CampaignID == CampaignID);
                        if (campaign != null)
                        {
                            List<RackDetails> racks = new List<RackDetails>();
                            Dictionary<int, DateTime> shipDates = campaign.CampaignShipdates.ToDictionary(c => c.CatalogID, c => c.ProductCatalogShipDate.ShipDate);
                            List<int> catalogsShipDates = campaign.CampaignShipdates.Select(cs => cs.CatalogID).ToList();

                            var orders = madduxEntities.Orders.AsNoTracking()
                                                .Include(o => o.OrderItems)
                                                .Where(o => o.RequestedShipDate.HasValue && o.OrderItems.Any(oi => catalogsShipDates.Contains(oi.Product.CatalogId)))
                                                .AsEnumerable()
                                                .Where(o => shipDates.Any(sd => o.OrderItems.Any(oi => oi.Product.CatalogId == sd.Key) && o.RequestedShipDate.Value == sd.Value))
                                                .ToList();

                            foreach (Order order in orders)
                            {
                                if (order.OrderRacks.Any())
                                {
                                    var rack = order.OrderRacks.FirstOrDefault();
                                    RackDetails obj = new RackDetails
                                    {
                                        RackID = rack.RackId,
                                        RackName = rack.ProductCatalogRack.RackName,
                                        Count = 1,
                                        CatalogName = rack.ProductCatalogRack.CatalogName,
                                        CatalogID = rack.ProductCatalogRack.CatalogID
                                    };

                                    bool flag = false;
                                    for (int r = 0; r < racks.Count; r++)
                                    {
                                        if (racks[r].RackID == obj.RackID)
                                        {
                                            flag = true;
                                            racks[r].Count = (racks[r].Count + 1);
                                        }
                                    }
                                    if (!flag)
                                    {
                                        racks.Add(obj);
                                    }

                                }
                                else
                                {
                                    // If order doesn't have any rack in it means orders was entered in the old system
                                    // old system doesn't have anything to track order via program

                                }
                            }
                            dgvCampaignDetails.DataSource = racks;
                            dgvCampaignDetails.DataBind();
                        }
                        else
                        {
                            // campaign is null
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
}
