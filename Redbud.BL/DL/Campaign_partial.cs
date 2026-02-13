using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Redbud.BL.DL
{
    public partial class Campaign
    {
        public int GoalReached
        {
            get
            {
                try
                {
                    int count = 0;
                    using (var db = new MadduxEntities())
                    {
                        var catalogIds = this.CampaignShipdates.Select(cs => cs.CatalogID);
                        var campaignShipdates = this.CampaignShipdates.Select(x => x.ProductCatalogShipDate.ShipDate);

                        count = db.Orders.AsNoTracking().Count(o => (o.RequestedShipDate.HasValue && campaignShipdates.Any(x => DbFunctions.TruncateTime(x) == DbFunctions.TruncateTime(o.RequestedShipDate.Value))) && o.OrderRacks.Any(or => catalogIds.Contains(or.ProductCatalogRack.CatalogID)));
                    }
                    return count;
                }
                catch
                {
                    return 0;
                }
            }
        }
        public int GetRacksOrdered(IEnumerable<Order> orders)
        {

            try
            {
                var csd = CampaignShipdates.ToList();
                var catalogIds = csd.Select(cs => cs.CatalogID);
                var campaignShipdates = csd.Select(x => x.ProductCatalogShipDate.ShipDate);
                return orders.Count(o => (o.RequestedShipDate.HasValue && campaignShipdates.Any(x => x.Date == o.RequestedShipDate.Value.Date)) && o.OrderRacks.Any(or => catalogIds.Contains(or.ProductCatalogRack.CatalogID)));
            }
            catch
            {
                return 0;
            }

        }

        public double GoalPercentageReached
        {
            get
            {
                try
                {
                    int count = 0;
                    using (var db = new MadduxEntities())
                    {
                        /**
                          we want all orders for this program with this shipdate
                        */
                        var catalogIds = this.CampaignShipdates.Select(cs => cs.CatalogID);
                        count = db.Orders.Count(o => o.RequestedShipDate.HasValue && this.CampaignShipdates.Any(cs => cs.ProductCatalogShipDate.ShipDate == o.RequestedShipDate.Value && catalogIds.Contains(cs.CatalogID)) && o.OrderItems.Any(oi => catalogIds.Contains(oi.Product.CatalogId)));
                    }
                    return ((double)count / this.Goal);
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        public int GetCustomerCount(int assocId = 0)
        {

            try
            {
                using (var db = new MadduxEntities())
                {
                    DateTime cutoffDate = DateTime.Today.AddYears(-3);

                    var catalogids = this.CampaignShipdates.Select(cs => cs.CatalogID);
                    var custs = db.CustomerAsscs.Where(ca => ca.Customer.Orders.Any(o => o.OrderDate >= cutoffDate && o.OrderItems.Any(oi => catalogids.Contains(oi.Product.ProductCatalog.CatalogId))));

                    if (assocId != 0)
                    {
                        custs = custs.Where(c => c.AssociationID == assocId);
                    }

                    return custs.Select(c => c.CustomerID).Distinct().Count();
                }
            }
            catch (Exception)
            {
                return 0;
            }

        }

        public int GetCustomersReached(int assocId = 0)
        {
            try
            {
                using (var db = new MadduxEntities())
                {
                    var catalogIds = this.ProductProgram.ProductCatalogs.Select(c => c.CatalogId);

                    var countQry = db.Customers.Where(c => c.Orders.Any(o => o.OrderItems.Any(oi => catalogIds.Contains(oi.Product.CatalogId)) && o.RequestedShipDate.Value == this.Shipdate) || c.Journals.Any(j => j.CampaignID == this.CampaignID));

                    if (assocId != 0)
                    {
                        countQry = countQry.Where(c => c.CustomerAsscs.Any(ca => ca.AssociationID == assocId));
                    }

                    return countQry.Count();
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public double GetCustomersPercentageReached(int assocId = 0)
        {
            try
            {
                return ((double)this.GetCustomersReached(assocId) / this.GetCustomerCount(assocId));

            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
