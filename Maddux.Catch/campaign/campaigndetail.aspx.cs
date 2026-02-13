using Redbud.BL.DL;
using Redbud.BL.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Maddux.Catch.campaign
{
    public class CatalogModel
    {
        public int CatalogID { get; set; }
        public string CatalogName { get; set; }
        public List<ProductCatalogShipDate> ShipDates { get; set; }
    }
    public partial class campaigndetail : Page
    {
        private int CampaignId
        {
            get
            {
                if (ViewState["CampaignId"] == null)
                {
                    ViewState["CampaignId"] = Request.QueryString["id"] == null || Request.QueryString["id"] == "" ? 0 : (object)Request.QueryString["id"];
                }
                return Convert.ToInt32(ViewState["CampaignId"].ToString());
            }

            set
            {
                ViewState["CampaignId"] = value;
            }
        }

        private int ActiveTab
        {
            get
            {
                if (ViewState["ActiveTab"] == null)
                {
                    ViewState["ActiveTab"] = Request.QueryString["tab"] == null || Request.QueryString["tab"] == "" ? "0" : Request.QueryString["tab"];
                }
                return Convert.ToInt32(ViewState["ActiveTab"].ToString());
            }

            set
            {
                ViewState["ActiveTab"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            try
            {
                tabStatus.Visible = false;

                if (!Page.IsPostBack)
                {
                    Literal litPageHeader = (Literal)Master.FindControl("litPageHeader");
                    LoadPrograms();

                    if (CampaignId == 0)
                    {
                        btnDeleteCampaign.Visible = false;
                        this.Title = "Maddux | New Campaign";
                        litPageHeader.Text = "New Campaign";
                        tabStatus.Visible = false;
                        ActiveTab = 0;
                        //ddlShipdate.Attributes.Add("disabled", "true");
                    }
                    else
                    {
                        using (var db = new MadduxEntities())
                        {
                            var campaign = db.Campaigns.Find(CampaignId);
                            if (campaign != null)
                            {
                                //tabStatus.Visible = true;

                                this.Title = "Maddux | Campaign";
                                litPageHeader.Text = "Campaign";

                                //details tab
                                txtName.Text = campaign.CampaignName;
                                ddlProgram.SelectedValue = campaign.ProgramID.ToString();
                                LoadShipdates();
                                //ddlShipdate.SelectedValue = string.Format("{0}|{1}", campaign.Shipdate.Date.ToString("MMMM dd, yyyy"), campaign.SalesEnd.ToString("MMMM dd, yyyy"));
                                txtStartDate.Text = campaign.SalesStart.ToString("MMMM dd, yyyy");
                                txtEndDate.Text = campaign.SalesEnd.ToString("MMMM dd, yyyy");
                                //hidEndDate.Value = campaign.SalesEnd.ToString("MMMM dd, yyyy");
                                txtGoal.Text = campaign.Goal.ToString();

                            }
                            else
                            {
                                throw new Exception("Campaign not found");
                            }
                        }
                    }
                    TabName.Value = ActiveTab == 0 ? "details" : "status";
                }
                hidCampaignId.Value = CampaignId.ToString();
            }
            catch
            {
                Response.Redirect("/campaign/campaigns.aspx");
            }
        }

        [WebMethod]
        public static List<object> GetChartData(int cID)
        {
            List<object> chartData = new List<object>();

            using (var db = new MadduxEntities())
            {
                DateTime cutoffDate = DateTime.Today.AddYears(-3);
                var campaign = db.Campaigns.Find(cID);
                var catalogids = campaign.ProductProgram.ProductCatalogs.Select(c => c.CatalogId).ToList();

                var custs = db.CustomerAsscs.Where(ca => ca.Customer.Orders.Any(o => o.OrderDate >= cutoffDate && o.OrderItems.Any(oi => catalogids.Contains(oi.Product.ProductCatalog.CatalogId))));

                var assocQry = custs.Join(db.Associations.Where(x => x.AsscShort.StartsWith("Member - ")), c => c.AssociationID, a => a.AssociationID, (c, a) => a).Distinct().ToList();

                object[] dataObj = new object[4];
                dataObj[0] = string.Format("Goal: {0} racks", campaign.Goal);
                dataObj[1] = campaign.GoalPercentageReached;
                dataObj[2] = campaign.GoalPercentageReached.ToString("P0", CultureInfo.InvariantCulture);
                dataObj[3] = string.Format("Reached {0} of {1}", campaign.GoalReached, campaign.Goal);
                chartData.Add(dataObj);

                dataObj = new object[4];
                dataObj[0] = "Total Customers Reached";
                dataObj[1] = campaign.GetCustomersPercentageReached();
                dataObj[2] = campaign.GetCustomersPercentageReached().ToString("P0", CultureInfo.InvariantCulture);
                dataObj[3] = string.Format("Reached {0} of {1}", campaign.GetCustomersReached(), campaign.GetCustomerCount());

                chartData.Add(dataObj);

                foreach (var a in assocQry)
                {
                    dataObj = new object[4];
                    dataObj[0] = a.AsscShort.Replace("Member - ", "");
                    dataObj[1] = campaign.GetCustomersPercentageReached(a.AssociationID);
                    dataObj[2] = campaign.GetCustomersPercentageReached(a.AssociationID).ToString("P0", CultureInfo.InvariantCulture);
                    dataObj[3] = string.Format("Reached {0} of {1}", campaign.GetCustomersReached(a.AssociationID), campaign.GetCustomerCount(a.AssociationID));
                    chartData.Add(dataObj);
                }
            }
            return chartData;
        }

        private void LoadPrograms()
        {

            using (var db = new MadduxEntities())
            {
                var programs = db.ProductPrograms.Where(r => r.ProductCatalogs.Any(x => x.Active)).ToList();
                programs.Insert(0, new ProductProgram
                {
                    ProgramID = -1,
                    ProgramName = "-- Select a Program --",

                });

                ddlProgram.DataTextField = "ProgramName";
                ddlProgram.DataValueField = "ProgramId";
                ddlProgram.DataSource = programs;
                ddlProgram.DataBind();
            }
        }

        private void LoadShipdates()
        {
            //ddlShipdate.Items.Clear();
            int selectedProgram = Convert.ToInt32(ddlProgram.SelectedValue);

            if (selectedProgram == -1)
            {
                //ddlShipdate.Items.Insert(0, new ListItem { Text = "-- Select a Shipdate --", Value = "0" });
                //ddlShipdate.DataSource = null;
                //ddlShipdate.DataBind();

                rptCatalogs.DataSource = null;
                rptCatalogs.DataBind();
            }
            else
            {
                using (var db = new MadduxEntities())
                {
                    //var shipDates = db.ProductProgramShipDates.AsNoTracking()
                    //                    .Where(d => d.ProgramID == selectedProgram && d.ShipDate >= DateTime.Today)
                    //                    .GroupBy(d => d.ShipDate)
                    //                    .Select(g => new
                    //                    {
                    //                        ShipDate = g.Key,
                    //                        OrderDeadline = g.Select(x => x.OrderDeadlineDate).FirstOrDefault()
                    //                    });

                    //var dateItems = shipDates.OrderBy(d => d.ShipDate).ToList();

                    //dateItems.ForEach(d => ddlShipdate.Items.Add(new ListItem
                    //{
                    //    Text = d.ShipDate.ToString("MMMM dd, yyyy"),
                    //    Value = string.Format("{0}|{1}", d.ShipDate.ToString("MMMM dd, yyyy"), d.OrderDeadline.ToString("MMMM dd, yyyy"))
                    //}));

                    //ddlShipdate.Items.Insert(0, new ListItem { Text = "-- Select a Shipdate --", Value = "0" });

                    var programCatalogs = db.ProductCatalogs.AsNoTracking()
                                            .Include(pc => pc.ProductCatalogShipDates)
                                            .Where(pc => pc.Active && pc.ProgramID == selectedProgram);

                    rptCatalogs.DataSource = programCatalogs
                                                .Select(pc =>
                                                    new CatalogModel
                                                    {
                                                        CatalogID = pc.CatalogId,
                                                        CatalogName = pc.CatalogName,
                                                        ShipDates = pc.ProductCatalogShipDates.ToList()
                                                    })
                                                .ToList();
                    rptCatalogs.DataBind();



                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    using (var db = new MadduxEntities())
                    {
                        Redbud.BL.DL.Campaign campaign;
                        if (CampaignId == 0)
                        {
                            campaign = new Redbud.BL.DL.Campaign();
                            db.Campaigns.Add(campaign);
                        }
                        else
                        {
                            campaign = db.Campaigns.Find(CampaignId);

                        }
                        if (ddlProgram.SelectedIndex == 0)
                        {
                            litMessage.Text = StringTools.GenerateError("Please select program.");
                            return;
                        }
                        bool dateSelected = false;
                        foreach (RepeaterItem catalog in rptCatalogs.Items)
                        {
                            DropDownList ddShipDates = catalog.FindControl("ddShipDates") as DropDownList;
                            int selectedDateID = int.Parse(ddShipDates.SelectedValue);
                            if (selectedDateID > 0)
                                dateSelected = true;
                        }
                        if (dateSelected == false)
                        {
                            litMessage.Text = StringTools.GenerateError("Please select shipdate.");
                            return;
                        }
                        //    if (ddlShipdate.SelectedIndex == 0)
                        //{
                        //    litMessage.Text = StringTools.GenerateError("Please select shipdate.");
                        //    return;
                        //}
                        if (campaign != null)
                        {
                            campaign.CampaignName = txtName.Text.Trim();
                            campaign.ProgramID = Convert.ToInt32(ddlProgram.SelectedValue);
                            campaign.SalesStart = DateTime.Parse(txtStartDate.Text);
                            campaign.SalesEnd = DateTime.Parse(txtEndDate.Text);
                            //campaign.Shipdate = DateTime.Parse(ddlShipdate.SelectedItem.Text);  //selected value is actually the order deadline - use the item text
                            campaign.Goal = Convert.ToInt32(txtGoal.Text);

                            foreach (RepeaterItem catalog in rptCatalogs.Items)
                            {
                                int catalogID = int.Parse((catalog.FindControl("chkCatalogID") as HiddenField).Value);
                                CampaignShipdate shipDate = campaign.CampaignShipdates.FirstOrDefault(sd => sd.CatalogID == catalogID);
                                DropDownList ddShipDates = catalog.FindControl("ddShipDates") as DropDownList;

                                int selectedDateID = int.Parse(ddShipDates.SelectedValue);

                                if (shipDate != null)
                                {
                                    if (selectedDateID > 0)
                                    {
                                        shipDate.ShipDateID = selectedDateID;
                                    }
                                    else
                                    {
                                        db.CampaignShipdates.Remove(shipDate);
                                    }
                                }
                                else
                                {
                                    if (selectedDateID > 0)
                                    {
                                        shipDate = db.CampaignShipdates.Create();

                                        shipDate.CatalogID = catalogID;
                                        shipDate.ShipDateID = selectedDateID;
                                        shipDate.CampaignID = campaign.CampaignID;

                                        campaign.CampaignShipdates.Add(shipDate);
                                    }
                                }



                            }

                        }

                        db.SaveChanges();
                    }
                }
                catch
                {

                    throw;
                }


                litMessage.Text = StringTools.GenerateSuccess("Campaign saved successfully");
                Response.AddHeader("REFRESH", "1;URL=/campaign/campaigns.aspx");
                tabStatus.Visible = true;
            }

        }

        protected void btnDeleteCampaign_Click(object sender, EventArgs e)
        {
            using (var db = new MadduxEntities())
            {
                var campaignShipDates = db.CampaignShipdates.Where(cs => cs.CampaignID == CampaignId);
                var campaign = db.Campaigns.Find(CampaignId);
                try
                {
                    db.CampaignShipdates.RemoveRange(campaignShipDates);
                    db.Campaigns.Remove(campaign);
                    db.SaveChanges();
                }
                catch
                {

                }
                finally
                {
                    Response.Redirect("/campaign/campaigns.aspx");
                }
            }
        }

        protected void ddlProgram_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ddlShipdate.Attributes.Remove("disabled");
            LoadShipdates();

            //if (ddlShipdate.SelectedIndex != 0)
            //{
            //    try
            //    {
            //        string endDate = ddlShipdate.SelectedValue.Replace(string.Format("{0}|", ddlShipdate.SelectedItem.Text), string.Empty);
            //        hidEndDate.Value = DateTime.Parse(endDate).ToString("MMMM dd, yyyy");
            //    }
            //    catch (Exception ex)
            //    {
            //        litMessage.Text = StringTools.GenerateError(ex.Message);
            //    }
            //}
            //else
            //{
            //    hidEndDate.Value = string.Empty;
            //}
        }

        protected void ddlShipdate_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (ddlShipdate.SelectedIndex != 0)
            //{
            //    try
            //    {
            //        string endDate = ddlShipdate.SelectedValue.Replace(string.Format("{0}|", ddlShipdate.SelectedItem.Text), string.Empty);
            //        hidEndDate.Value = DateTime.Parse(endDate).ToString("MMMM dd, yyyy");
            //    }
            //    catch (Exception ex)
            //    {
            //        litMessage.Text = StringTools.GenerateError(ex.Message);
            //    }
            //}
            //else
            //{
            //    hidEndDate.Value = string.Empty;
            //}
        }

        protected void rptCatalogs_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                CatalogModel model = e.Item.DataItem as CatalogModel;
                using (var db = new MadduxEntities())
                {
                    var shipDate = db.CampaignShipdates.FirstOrDefault(cs => cs.CatalogID == model.CatalogID && cs.CampaignID == CampaignId);

                    DropDownList ddShipDates = e.Item.FindControl("ddShipDates") as DropDownList;
                    List<ListItem> ShipDates = new List<ListItem>();

                    model.ShipDates.ForEach(sd =>
                        ShipDates.Add(new ListItem
                        {
                            Text = sd.ShipDate.ToString("MMMM dd, yyyy"),
                            Value = sd.ShipDateID.ToString()
                        }));

                    ShipDates.Insert(0, new ListItem { Text = "-- Select a Shipdate --", Value = "0" });
                    ddShipDates.DataSource = ShipDates;
                    ddShipDates.DataTextField = "Text";
                    ddShipDates.DataValueField = "Value";
                    ddShipDates.DataBind();

                    if (shipDate != null)
                    {
                        ddShipDates.SelectedValue = shipDate.ShipDateID.ToString();
                    }



                }


            }
        }
    }
}