using System.Linq;

namespace Redbud.BL.DL
{
    public partial class PurchaseOrder
    {
        public int HalfRackCount
        {
            get
            {
                return this.Orders.Where(o => o.OrderRacks.First().ProductCatalogRack.RackSize == "1/2").Count();
            }
        }

        public int QuarterRackCount
        {
            get
            {
                return this.Orders.Where(o => o.OrderRacks.First().ProductCatalogRack.RackSize == "1/4").Count();
            }
        }

        public int FullRackCount
        {
            get
            {
                return this.Orders.Where(o => o.OrderRacks.First().ProductCatalogRack.RackSize.Equals("Full", System.StringComparison.OrdinalIgnoreCase)).Count();
            }
        }

        public double TotalFeet
        {
            get
            {
                return (this.FullRackCount * 2) + (this.HalfRackCount * 1) + (this.QuarterRackCount * .5);
            }
        }
    }
}
