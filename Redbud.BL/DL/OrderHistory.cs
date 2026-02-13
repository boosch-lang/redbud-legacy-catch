using System;

namespace Redbud.BL.DL
{
    public partial class Customer
    {
        public class OrderHistory
        {
            public bool Approved { get; set; }
            public int OrderID { get; set; }
            public int RackID { get; set; }
            public Guid? BulkOrderKey { get; set; }
            public DateTime? OrderDate { get; set; }
            public DateTime? QuoteDate { get; set; }
            public DateTime? RequestedShipDate { get; set; }
            public DateTime? ShipDate { get; set; }
            public string ShippedTo { get; set; }
            public string ShippingName { get; set; }
            public double? Subtotal { get; set; }
            public double? Total { get; set; }
            public int CustomerID { get; set; }
            public decimal temp { get; set; }

            public int? ShipmentID { get; set; }
            public string ShipDateFormatted
            {
                get
                {
                    if (ShipDate.HasValue)
                    {
                        return ShipDate.Value.ToString("dd-MMM-yyyy");
                    }
                    else
                    {
                        return "";
                    }
                }
            }
            public string ShipDateDisplay
            {
                get
                {
                    if (ShipDate.HasValue)
                    {
                        return ShipDate.Value.ToString("MMMM d, yyyy");
                    }
                    else
                    {
                        return "";
                    }
                }
            }
            public string RequestedShipDateDisplay
            {
                get
                {
                    if (RequestedShipDate.HasValue)
                    {
                        return RequestedShipDate.Value.ToString("MMMM d, yyyy");
                    }
                    else
                    {
                        return "";
                    }
                }
            }
            public string OrderDateDisplay
            {
                get
                {
                    if (OrderDate.HasValue)
                    {
                        return OrderDate.Value.ToString("MMMM d, yyyy");
                    }
                    else
                    {
                        return "";
                    }
                }
            }
            public string ProgramHTML
            {
                get
                {
                    return RackName;
                }
            }

            public string Catalogue
            {
                get
                {
                    return CatalogueName;
                }
            }
            public string RackName { get; set; }

            public string CatalogueName { get; set; }
            public string PhotoPath { get; internal set; }
            public string PONumber { get; internal set; }
        }
    }
}
