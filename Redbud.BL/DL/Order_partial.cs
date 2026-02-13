using Redbud.BL.Helpers;
using Redbud.BL.Utils;
using System;
using System.Linq;

namespace Redbud.BL.DL
{
    public partial class Order
    {
        public string Catalogues
        {
            get
            {
                if (this.OrderRacks.Count > 0)
                {
                    return string.Join(", ", this.OrderRacks.Select(r => r.ProductCatalogRack.CatalogName));
                }
                else
                {
                    return string.Join(", ", this.OrderItems.Select(r => r.Product.ProductCatalog.CatalogName).Distinct());
                }
            }
        }

        public decimal SubTotal
        {
            get
            {
                return this.OrderItems.Where(i => i.ProductNotAvailable == false).Sum(i => Convert.ToDecimal(OrderHelper.GetTotalPrice(i.UnitPrice, i.DiscountPercent, i.Quantity)));
            }
        }

        public decimal DiscountTotal
        {
            get
            {
                return Math.Round(this.OrderItems.Where(i => i.ProductNotAvailable == false).Select(i => Convert.ToDecimal(i.Quantity) * Convert.ToDecimal(i.UnitPrice) * Convert.ToDecimal(i.DiscountPercent)).Sum(), 2);
            }
        }

        public decimal DiscountedSubTotal
        {
            get
            {
                return this.OrderItems.Where(i => i.ProductNotAvailable == false).Sum(x => Convert.ToDecimal(OrderHelper.GetTotalPrice(x.UnitPrice, x.DiscountPercent, x.Quantity)));
            }
        }

        public decimal GlobalDiscountAmount1
        {
            get
            {
                try
                {
                    return Math.Round(this.DiscountedSubTotal * Convert.ToDecimal(this.GlobalDiscountPercent), 2);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public decimal GlobalDiscountAmount2
        {
            get
            {
                try
                {
                    return Math.Round((this.DiscountedSubTotal - this.GlobalDiscountAmount1) * Convert.ToDecimal(this.GlobalDiscount2Percent), 2);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public decimal GlobalDiscountAmount3
        {
            get
            {
                try
                {
                    return Math.Round((this.DiscountedSubTotal - this.GlobalDiscountAmount1 - this.GlobalDiscountAmount2) * Convert.ToDecimal(this.GlobalDiscount3Percent), 2);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public decimal GlobalDiscountAmount4
        {
            get
            {
                try
                {
                    return Math.Round((this.DiscountedSubTotal - this.GlobalDiscountAmount1 - this.GlobalDiscountAmount2 - this.GlobalDiscountAmount3) * Convert.ToDecimal(this.GlobalDiscount4Percent), 2);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public decimal GlobalDiscountAmount5
        {
            get
            {
                try
                {
                    return Math.Round((this.DiscountedSubTotal - this.GlobalDiscountAmount1 - this.GlobalDiscountAmount2 - this.GlobalDiscountAmount3 - this.GlobalDiscountAmount4) * Convert.ToDecimal(this.GlobalDiscount5Percent), 2);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public decimal GlobalDiscountedSubTotal
        {
            get
            {
                try
                {
                    return Math.Round(this.DiscountedSubTotal - this.GlobalDiscountAmount1 - this.GlobalDiscountAmount2 - this.GlobalDiscountAmount3 - this.GlobalDiscountAmount4 - this.GlobalDiscountAmount5, 2);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public decimal GrandTotal
        {
            get { return this.DiscountedSubTotal - this.GlobalDiscountAmount1 - this.GlobalDiscountAmount2 - this.GlobalDiscountAmount3 - this.GlobalDiscountAmount4 - this.GlobalDiscountAmount5 + this.ShippingCharge + this.GSTAmount + this.PSTAmount; }
        }

        public DateTime? InvoiceDate
        {
            get
            {
                return this.OrderItems.FirstOrDefault()?.ShipmentItems.FirstOrDefault()?.Shipment?.InvoiceSentDate;
            }
        }

        public void CalculateFreightAndTaxes()
        {
            using (var db = new MadduxEntities())
            {
                var taxRates = db.States.Where(s => s.StateID == this.ShippingState).FirstOrDefault();
                if (taxRates != null)
                {
                    var invoiceDate = this.InvoiceDate.HasValue ? this.InvoiceDate : this.RequestedShipDate;
                    this.GSTAmount = Convert.ToDecimal(this.GlobalDiscountedSubTotal + this.ShippingCharge) * Convert.ToDecimal(TaxUtilities.GetTaxPercentage(taxRates.StateID, invoiceDate));

                    if (this.CustomShippingCharge == false)
                    {
                        this.ShippingCharge = Convert.ToDecimal(FreightCalculator.CalculateFreighCharge(Convert.ToDouble(this.GlobalDiscountedSubTotal), taxRates.StateID, this.ShippingZip));
                    }

                    this.PSTAmount = 0;
                    this.PSTExempt = false;
                }
                else
                {
                    this.GSTAmount = 0;
                    this.PSTAmount = 0;
                    this.PSTExempt = false;
                }
            }
        }

        public bool IsRenewable
        {
            get
            {
                using (var db = new MadduxEntities())
                {
                    int catalogYear = this.OrderItems.Select(i => i.Product.ProductCatalog.CatalogYear).FirstOrDefault();
                    int catalogClass = this.OrderItems.Select(i => i.Product.ProductCatalog.CatalogClassId).FirstOrDefault();
                    if (catalogYear == 0 || catalogClass == 0)
                    {
                        return false;
                    }

                    int nextCatalogCount = db.ProductCatalogs.Where(c => c.Active && c.CatalogYear > catalogYear && c.CatalogClassId == catalogClass).Count();

                    return nextCatalogCount > 0;
                }
            }
        }

        public int GetNextActiveCatalog()
        {
            using (var db = new MadduxEntities())
            {
                int catalogYear = this.OrderItems.Select(i => i.Product.ProductCatalog.CatalogYear).FirstOrDefault();
                int catalogClass = this.OrderItems.Select(i => i.Product.ProductCatalog.CatalogClassId).FirstOrDefault();
                if (catalogYear == 0 || catalogClass == 0)
                {
                    return 0;
                }

                return db.ProductCatalogs.OrderByDescending(c => c.CatalogYear).Where(c => c.Active && c.CatalogYear > catalogYear && c.CatalogClassId == catalogClass).Select(c => c.CatalogId).FirstOrDefault();
            }
        }

    }
}
