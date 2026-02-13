using Redbud.BL.Helpers;
using Redbud.BL.Utils;
using System;
using System.Linq;

namespace Redbud.BL.DL
{
    public partial class Shipment
    {
        public int OrderID
        {
            get
            {
                return this.ShipmentItems.FirstOrDefault().OrderItem.OrderId;
            }
        }
        public Order Order
        {
            get
            {
                return this.ShipmentItems.FirstOrDefault().OrderItem.Order;
            }
        }
        public string RackName
        {
            get
            {
                return this.ShipmentItems.FirstOrDefault().OrderItem?.OrderRack?.RackName;
            }
        }

        public string Catalog
        {
            get
            {
                using (var db = new MadduxEntities())
                {
                    return this.ShipmentItems.Select(si => si.OrderItem.Product.ProductCatalog.CatalogName).FirstOrDefault();
                }
            }
        }

        public string ShipVia
        {
            get
            {
                if (ShippingMethodId != 0)
                {
                    return supShippingMethod.ShippingMethodDesc;
                }
                else
                {
                    return "";
                }

            }
        }

        public string Total
        {
            get
            {

                var total = this.ShipmentItems.Sum(r => r.OrderItem.UnitPrice * r.Quantity);
                return total.ToString("C");
            }
        }

        public decimal SubTotal
        {
            get
            {
                try
                {
                    return Math.Round(this.ShipmentItems.Select(s => Convert.ToDecimal(s.Quantity) * Convert.ToDecimal(s.OrderItem.UnitPrice)).Sum(), 2);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public decimal DiscountTotal
        {
            get
            {
                try
                {
                    return this.ShipmentItems.Sum(s => Convert.ToDecimal(OrderHelper.GetTotalPrice(s.OrderItem.UnitPrice, s.OrderItem.DiscountPercent, s.Quantity)));

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public decimal DiscountedSubTotal
        {
            get
            {
                try
                {
                    return this.ShipmentItems.Sum(x => Convert.ToDecimal(OrderHelper.GetTotalPrice(x.OrderItem.UnitPrice, x.OrderItem.DiscountPercent, x.Quantity)));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
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

        public decimal ShipmentGrandTotal
        {
            get { return this.DiscountedSubTotal - this.GlobalDiscountAmount1 - this.GlobalDiscountAmount2 - this.GlobalDiscountAmount3 - this.GlobalDiscountAmount4 - this.GlobalDiscountAmount5 + this.ShippingCharge + this.GSTAmount + this.PSTAmount; }
        }

        public void CalculateFreightAndTaxes()
        {
            using (var db = new MadduxEntities())
            {

                var order = db.Orders.Find(this.OrderID);
                var taxRates = db.States.Where(s => s.StateID == order.ShippingState).FirstOrDefault();
                if (taxRates != null)
                {
                    var invoiceDate = this.InvoiceSentDate.HasValue ? this.InvoiceSentDate : this.Order.RequestedShipDate;
                    this.GSTAmount = Convert.ToDecimal((this.GlobalDiscountedSubTotal + this.ShippingCharge) * Convert.ToDecimal(TaxUtilities.GetTaxPercentage(taxRates.StateID, invoiceDate)));

                    this.PSTAmount = 0;
                    this.PSTExempt = false;
                }
                else
                {
                    this.GSTAmount = 0;
                    this.PSTExempt = false;
                    this.PSTAmount = 0;
                }
            }
        }

    }
}
