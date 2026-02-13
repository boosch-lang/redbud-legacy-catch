using Maddux.Catch.LocalClasses;
using Redbud.BL.DL;
using Redbud.BL.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Maddux.Catch.calllist
{
    public partial class calllist : System.Web.UI.Page
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
                    litPageHeader.Text = "Call List";
                    litTotal.Text = "";
                    LoadFilters();
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
                LoadCampaignDropdown();
                LoadProvinceDropdown();
                ddlCustomerFilter.Items.Add(new ListItem { Text = "Customers", Value = "1" });
                ddlCustomerFilter.Items.Add(new ListItem { Text = "Prospects", Value = "0" });

                ddlCustomerFilter.SelectedIndex = 0; //preselect customers

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadCampaignDropdown()
        {
            using (var db = new MadduxEntities())
            {
                var campaignTable = db.Campaigns.Where(c => c.SalesEnd >= DateTime.Today).OrderBy(c => c.CampaignName).ToList();

                ddlCampaignFilter.DataValueField = "CampaignId";
                ddlCampaignFilter.DataTextField = "CampaignName";
                ddlCampaignFilter.DataSource = campaignTable;
                ddlCampaignFilter.DataBind();
            }

        }

        private void LoadProvinceDropdown()
        {
            User currentUser = AppSession.Current.CurrentUser;

            using (var db = new MadduxEntities())
            {
                var states = from s in db.States
                             select s;

                if (currentUser.CanOnlyViewAssignedProvinces)
                {
                    states = states.Where(s => s.UserStates.Any(u => u.UserID == currentUser.UserID));
                }
                //states = states.OrderBy(s => s.Country).ThenBy(s => s.StateName);
                var stateTable = states.Where(s => s.Country != "USA").Select(r => new
                {
                    r.StateID,
                    r.StateName
                }).ToList();

                ddlProvinceFilter.DataValueField = "StateId";
                ddlProvinceFilter.DataTextField = "StateName";
                ddlProvinceFilter.DataSource = stateTable;
                ddlProvinceFilter.DataBind();
            }

        }

        protected void ddlCampaignFilter_SelectedIndexChanged(object sender, EventArgs e)
        {

            List<int> campaignIds = new List<int>();
            foreach (var i in ddlCampaignFilter.GetSelectedIndices())
            {
                campaignIds.Add(int.Parse(ddlCampaignFilter.Items[i].Value));
            }

            if (campaignIds.Count > 0)
            {
                using (var db = new MadduxEntities())
                {
                    var catalogids = db.ProductCatalogs.Where(pc => pc.ProductProgram.Campaigns.Any(c => campaignIds.Contains(c.CampaignID))).Select(pc => pc.CatalogId).ToList();

                    ddlMembershipFilter.DataSource = (from ac in db.AssociationCatalogs
                                                      join a in db.Associations on ac.AssociationID equals a.AssociationID
                                                      where catalogids.Contains(ac.CatalogID) && a.AsscDesc.StartsWith("Member")
                                                      select new
                                                      {
                                                          AsscShort = a.AsscShort.Substring(8),
                                                          a.AssociationID
                                                      }).Distinct().OrderBy(a => a.AsscShort).ToList();
                    ddlMembershipFilter.DataTextField = "AsscShort";
                    ddlMembershipFilter.DataValueField = "AssociationID";
                    ddlMembershipFilter.DataBind();
                    ddlMembershipFilter.Visible = true;
                }
            }
            else
            {
                ddlMembershipFilter.Items.Clear();
                // ddlMembershipFilter.Items.Add(new ListItem { Text = "None Selected", Value = "0" });
                //ddlMembershipFilter.Visible = false;
            }

        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                //get selected campaigns
                List<int> campaignIds = new List<int>();
                foreach (var i in ddlCampaignFilter.GetSelectedIndices())
                {
                    campaignIds.Add(int.Parse(ddlCampaignFilter.Items[i].Value));
                }

                //get selected memberships
                List<int> assocIds = new List<int>();
                foreach (var i in ddlMembershipFilter.GetSelectedIndices())
                {
                    assocIds.Add(int.Parse(ddlMembershipFilter.Items[i].Value));
                }

                //get selected provinces
                List<string> provinceIds = new List<string>();
                foreach (var i in ddlProvinceFilter.GetSelectedIndices())
                {
                    provinceIds.Add(ddlProvinceFilter.Items[i].Value);
                }

                //get selected cusomter types
                bool selectCustomers = ddlCustomerFilter.Items[0].Selected;
                bool selectProspects = ddlCustomerFilter.Items[1].Selected;

                if (campaignIds.Count > 0)
                {
                    using (var db = new MadduxEntities())
                    {
                        DateTime cutoffDate = DateTime.Today.AddYears(-3);

                        var campaigns = db.Campaigns.Where(c => campaignIds.Contains(c.CampaignID));

                        var shipDates = campaigns.Select(c => c.Shipdate.ToString());

                        //get catalogs for selected programs
                        var catalogs = (from c in campaigns
                                        join p in db.ProductPrograms on c.ProgramID equals p.ProgramID
                                        join pc in db.ProductCatalogs on p.ProgramID equals pc.ProgramID
                                        select pc.CatalogId).Distinct().ToList();

                        //start customer query
                        var _association = db.AssociationCatalogs.Where(x => catalogs.Contains(x.CatalogID)).Select(x => x.AssociationID).Distinct().ToList();
                        var associations = (from a in db.Associations
                                            join ac in db.AssociationCatalogs on a.AssociationID equals ac.AssociationID
                                            where catalogs.Contains(ac.CatalogID)
                                            select a.AssociationID).ToList();
                        if (assocIds.Count > 0)
                        {
                            associations = associations.Where(a => assocIds.Contains(a)).ToList(); //filter to selected associations
                        }
                        var custs = db.Customers.Where(c => c.CustomerAsscs.Any(ca => associations.Contains(ca.AssociationID))); //get all customers that match associations

                        if (selectCustomers && !selectProspects)
                        {
                            //filter to only those customers that have ordered in last 3 years
                            custs = custs.Where(c => c.Orders.Any(o => o.OrderItems.Any(oi => catalogs.Contains(oi.Product.CatalogId)) && o.OrderDate >= cutoffDate));
                        }
                        else if (selectProspects && !selectCustomers)
                        {
                            //customers that have not ordered in past 3 years
                            custs = custs.Where(c => !c.Orders.Any(o => o.OrderItems.Any(oi => catalogs.Contains(oi.Product.CatalogId)) && o.OrderDate >= cutoffDate));
                        }

                        //filter out anyone who has already been contacted for this ship date
                        custs = custs.Where(c => !c.Orders.Any(o => o.OrderItems.Any(oi => catalogs.Contains(oi.Product.CatalogId)) && shipDates.Contains(o.RequestedShipDate.ToString())) && !c.Journals.Any(j => campaignIds.Any(i => i == j.CampaignID)));


                        if (provinceIds.Count > 0)
                        {
                            custs = custs.Where(c => provinceIds.Contains(c.State));
                        }
                        //custs.ToList().Sort((x, y) => x.Rating.CompareTo(y.Rating));
                        dgvCalllist.DataSource = custs.ToList().OrderByDescending(m => m.Rating).ThenBy(x => x.Company);
                        dgvCalllist.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }

        protected void dgvCalllist_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    GridViewRow gvr = e.Row;
                    Customer drv = (Customer)gvr.DataItem;
                    Literal litStars = (Literal)gvr.FindControl("litStars");
                    StringBuilder sb = new StringBuilder();

                    for (var i = 0; i < drv.Rating; i++)
                    {
                        sb.Append("<span><i class=\"fa fa-star text-light-blue\"></i></span>");
                    }
                    litStars.Text = sb.ToString();
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }
    }
}