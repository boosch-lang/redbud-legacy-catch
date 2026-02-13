using Redbud.BL.Helpers;
using System;
using System.Linq;

namespace Redbud.BL.DL
{
    public partial class OrderRack
    {
        public string RackName
        {
            get
            {
                using (var db = new MadduxEntities())
                {
                    if (this.ProductCatalogRack == null)
                    {
                        ProductCatalogRack productCatalogRack = db.ProductCatalogRacks.FirstOrDefault(pcr => pcr.RackID == this.RackId);
                        return productCatalogRack.RackName;
                    }
                    else
                    {
                        return this.ProductCatalogRack.RackName;
                    }
                }

            }
        }
        public double GetRackTotal
        {
            get
            {
                return Math.Round(this.OrderItems.Where(i => i.ProductNotAvailable == false).Sum(i => OrderHelper.GetTotalPrice(i.UnitPrice, i.DiscountPercent, i.Quantity)), 2);
            }
        }
    }
}
