using System;
using System.Collections.Generic;
using System.Linq;

namespace Redbud.BL.DL
{
    public partial class ProductCatalog
    {
        public string DisplayCatalogName
        {
            get
            {
                if (CustomerCatalogName.Length == 0)
                {
                    return CatalogName;
                }
                else
                {
                    return CustomerCatalogName;
                }
            }
        }
        public List<ProductCatalogShipDate> FutureShipDates
        {
            get
            {
                using (var db = new MadduxEntities())
                {
                    return db.ProductCatalogShipDates
                        .Where(r => r.CatalogID == this.CatalogId && r.OrderDeadlineDate >= DateTime.Now)
                        .OrderBy(r => r.ShipDate).ToList();
                }
            }
        }
        public List<ProductCatalogShipDate> FutureShipDatesForCatch
        {
            get
            {
                using (var db = new MadduxEntities())
                {
                    return db.ProductCatalogShipDates
                        .Where(r => r.CatalogID == this.CatalogId && r.ShipDate > DateTime.Now)
                        .OrderBy(r => r.ShipDate).ToList();
                }
            }
        }
    }
}
