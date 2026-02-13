using Redbud.BL.DL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;

namespace Maddux.Catch.campaign.request
{
    public class RackDetails
    {
        public int? RackID { get; set; }
        public int CatalogID { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public string CatalogName { get; set; }
    }
    /// <summary>
    /// Summary description for GetCampaignDetail
    /// </summary>
    /// 
    public class GetCampaignDetail : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                int CampaignID = int.Parse(context.Request.Form["campaignID"]);
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
                                    Name = rack.ProductCatalogRack.RackName,
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
                        context.Response.StatusCode = (int)HttpStatusCode.OK;
                        System.Web.Script.Serialization.JavaScriptSerializer jsonSerializer =
                                    new System.Web.Script.Serialization.JavaScriptSerializer();
                        context.Response.ContentType = "text/json";
                        context.Response.Write(
                                    jsonSerializer.Serialize(
                                        new
                                        {
                                            success = true,
                                            details = racks
                                        }
                                    )
                                );
                    }
                    else
                    {
                        System.Web.Script.Serialization.JavaScriptSerializer jsonSerializer =
                           new System.Web.Script.Serialization.JavaScriptSerializer();
                        context.Response.ContentType = "text/json";
                        context.Response.Write(
                                    jsonSerializer.Serialize(
                                        new
                                        {
                                            success = false,
                                            errors = "Can't find any details about requested campign!"
                                        }
                                    )
                                );
                    }


                }
            }
            catch (Exception ex)
            {
                System.Web.Script.Serialization.JavaScriptSerializer jsonSerializer =
                           new System.Web.Script.Serialization.JavaScriptSerializer();
                context.Response.ContentType = "text/json";
                context.Response.Write(
                            jsonSerializer.Serialize(
                                new
                                {
                                    success = false,
                                    errors = ex.Message
                                }
                            )
                        );
            }

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}