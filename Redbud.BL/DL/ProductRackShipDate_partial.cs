using System;
using System.Linq;

namespace Redbud.BL.DL
{
    public partial class ProductRackShipDate
    {
        public DateTime? ProperOrderDeadlineDate => ProductCatalogRack.ProductCatalog.ProductProgram.ProductProgramShipDates.FirstOrDefault(x => x.ShipDate == ShipDate)?.OrderDeadlineDate;
    }
}
