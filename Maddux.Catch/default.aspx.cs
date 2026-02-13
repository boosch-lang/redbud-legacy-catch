using Maddux.Catch.LocalClasses;
using Redbud.BL.DL;
using Redbud.BL.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Maddux.Catch
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    User currentUser = AppSession.Current.CurrentUser;
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Literal litPageHeader = (Literal)Master.FindControl("litPageHeader");
                    litPageHeader.Text = "Dashboard";
                    litTotalShipments.Text = currentUser.TotalShipments.ToString();
                    litFollowUps.Text = currentUser.TotalFollowups.ToString();
                    litMyFollowUps.Text = currentUser.TotalMyFollowups.ToString();
                    phShipments.Visible = currentUser.CanEditShipments;
                    phYearOverYear.Visible = currentUser.ShowSettings;
                    phYearOverYear3.Visible = currentUser.ShowSettings;

                    int totalSold = 0;
                    int customersReached = 0;
                    int customerCount = 0;
                    using (MadduxEntities madduxEntities = new MadduxEntities())
                    {
                        List<Redbud.BL.DL.Campaign> campaigns =
                        madduxEntities
                           .Campaigns
                           .Include(c => c.CampaignShipdates)
                           .Include(c => c.CampaignShipdates.Select(x => x.ProductCatalogShipDate))
                           .AsNoTracking()
                           .Where(c => c.CampaignShipdates.Any(cs => cs.ProductCatalogShipDate.ShipDate > DateTime.Today) || ((c.Shipdate.HasValue && c.Shipdate.Value > DateTime.Today)))
                           .OrderBy(c => c.SalesEnd)
                           .ToList();

                        foreach (Redbud.BL.DL.Campaign campaign in campaigns)
                        {
                            Dictionary<int, DateTime> shipDates = campaign.CampaignShipdates.ToDictionary(c => c.CatalogID, c => c.ProductCatalogShipDate.ShipDate);
                            List<int> catalogsShipDates = campaign.CampaignShipdates.Select(cs => cs.CatalogID).ToList();

                            var orders = madduxEntities.Orders.AsNoTracking()
                                              .Include(o => o.OrderItems)
                                              .Where(o => o.RequestedShipDate.HasValue && o.OrderItems.Any(oi => catalogsShipDates.Contains(oi.Product.CatalogId)))
                                              .AsEnumerable()
                                              .Where(o => shipDates.Any(sd => o.OrderItems.Any(oi => oi.Product.CatalogId == sd.Key) && o.RequestedShipDate.Value == sd.Value))
                                              .ToList();
                            totalSold += orders.Count();
                            customersReached += campaign.GetCustomersReached();
                            customerCount += campaign.GetCustomerCount();
                        }
                        litCampaigns.Text = campaigns.Count().ToString();
                        litCampaignSold.Text = totalSold.ToString();
                        if (customerCount != 0)
                        {
                            decimal percent = (decimal)customersReached / customerCount;
                            litCampaignReached.Text = $"{customersReached} ({percent:#.## %})";
                        }
                        else
                        {
                            litCampaignReached.Text = "0";
                        }
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