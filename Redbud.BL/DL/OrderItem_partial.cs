using System.Linq;

namespace Redbud.BL.DL
{
    public partial class OrderItem
    {
        public double UnshippedQuantity
        {
            get
            {
                return this.Quantity - this.ShipmentItems.Select(s => s.Quantity).Sum();
            }
        }
        public double GetLatestUnitPrize
        {
            get
            {
                using (var db = new MadduxEntities())
                {
                    return db.Products.Where(x => x.ProductId == this.ProductId).Select(x => x.UnitPrice).FirstOrDefault();
                }

            }
        }
    }
}
