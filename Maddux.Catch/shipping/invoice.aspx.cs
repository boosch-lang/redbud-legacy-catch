using Redbud.BL.DL;
using Redbud.BL.Helpers;
using Redbud.BL.Utils;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.UI.WebControls;

namespace Maddux.Catch.shipping
{
    public partial class invoice : System.Web.UI.Page
    {
        private int OrderID
        {
            get
            {
                if (ViewState["OrderID"] == null)
                {
                    ViewState["OrderID"] = Request.QueryString["id"] == null || Request.QueryString["id"] == "" ? -1 : (object)Request.QueryString["id"];
                }
                return Convert.ToInt32(ViewState["OrderID"].ToString());
            }

            set
            {
                ViewState["OrderID"] = value;
            }
        }
        private int ShipmentID
        {
            get
            {
                if (ViewState["ShipmentID"] == null)
                {
                    ViewState["ShipmentID"] = Request.QueryString["sID"] == null || Request.QueryString["sID"] == "" ? -1 : (object)Request.QueryString["sID"];
                }
                return Convert.ToInt32(ViewState["ShipmentID"].ToString());
            }

            set
            {
                ViewState["ShipmentID"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            using (var db = new MadduxEntities())
            {
                var order = db.Orders.AsNoTracking().FirstOrDefault(r => r.OrderID == OrderID);
                if (order != null)
                {
                    order.CalculateFreightAndTaxes();
                    var shippedOrder = db.vwCustomerShippedOrders.FirstOrDefault(r => r.OrderID == OrderID);
                    var shipment = db.Shipments.AsNoTracking().Include(s => s.ShipmentItems).Include(s => s.ShipmentItems.Select(si => si.OrderItem)).FirstOrDefault(x => x.ShipmentID == ShipmentID);
                    shipment.CalculateFreightAndTaxes();
                    db.SaveChanges();

                    if (shippedOrder == null)
                    {
                        shipDateArea.Visible = false;
                    }
                    else
                    {
                        lblType.InnerText = "Invoice";
                        lblConfirmationSent.InnerText = order.ConfirmationSentDate.HasValue ? order.ConfirmationSentDate.Value.ToString("MMMM d, yyyy") : "";
                        lblPOSent.InnerText = order.PurchaseOrdersSentDate.HasValue ? order.PurchaseOrdersSentDate.Value.ToString("MMMM d, yyyy") : "";
                    }
                    lblInvoiceDate.InnerText = shipment.DateShipped != null
                        ? shipment.DateShipped?.ToString("MMMM d, yyyy")
                        : shipment.InvoiceSentDate?.ToString("MMMM d, yyyy");
                    lblInvoiceNumber.InnerText = ShipmentID.ToString();
                    lblID.InnerText = order.OrderID.ToString();

                    lblRequestedShipDate.InnerText = order.RequestedShipDate.HasValue ? order.RequestedShipDate.Value.ToString("MMMM d, yyyy") : "";
                    lblPONumber.InnerText = order.PONumber;
                    lblSalesperson.InnerText = order.User.FullName;
                    lblShipDate.InnerText = order.ShipDate.HasValue ? order.ShipDate.Value.ToString("MMMM d, yyyy") : "";
                    lblShippingMethod.InnerText = order.supShippingMethod.ShippingMethodDesc;
                    lblBillingName.InnerText = order.BillingName;
                    lblBillingAddress.InnerText = order.BillingAddress;
                    lblBillingCity.InnerText = order.BillingCity;
                    lblBillingState.InnerText = order.BillingState;
                    lblBillingZip.InnerText = order.BillingZip;
                    lblShippingName.InnerText = order.ShippingName;
                    lblShippingAddress.InnerText = order.ShippingAddress;
                    lblShippingCity.InnerText = order.ShippingCity;
                    lblShippingState.InnerText = order.ShippingState;
                    lblShippingPostal.InnerText = order.ShippingZip;
                    lblnotes.InnerText = shipment.ShipmentNotes;
                    supPaymentTerm terms = db.supPaymentTerms.FirstOrDefault(pt => pt.PaymentTermsId == order.Customer.DefaultTermsId);
                    lblTerms.InnerText = terms.PaymentTermsDesc;
                    lblVendorNumber.InnerText = order.Customer.VendorNumber;
                    decimal gst = 0;
                    decimal pst = 0;
                    decimal shipping = 0;
                    double subTotal = 0;


                    gst = order.GSTAmount;
                    pst = order.PSTAmount;
                    shipping = order.ShippingCharge;

                    if (order.OrderRacks.Any())
                    {
                        var rack = order.OrderRacks.FirstOrDefault().ProductCatalogRack;
                        lblRackName.InnerText = rack.CatalogName + " " + rack.RackName;
                    }
                    else
                    {
                        lblRackName.InnerText = order.OrderItems.First().Product.ProductCatalog.CatalogName;
                    }

                    var products = shipment.ShipmentItems.Select(r => new
                    {
                        Quantity = $"{r.Quantity} x {r.OrderItem.Product.PackagesPerUnit}",
                        r.OrderItem.Product.ProductName,
                        r.OrderItem.Product.UPCCode,
                        r.OrderItem.Product.Size,
                        Discount = r.OrderItem.DiscountPercent > 0 ? r.OrderItem.DiscountPercent.ToString("P2") : string.Empty,
                        DiscountedEachPrice = OrderHelper.CalculateEachPrice(r.OrderItem.UnitPrice, r.OrderItem.DiscountPercent, r.OrderItem.Product.PackagesPerUnit),
                        Total = OrderHelper.GetTotalPrice(r.OrderItem.UnitPrice, r.OrderItem.DiscountPercent, r.Quantity),
                        ItemNo = r.OrderItem.Product.ItemNumber,
                    }).ToList();
                    subTotal = products.Sum(r => r.Total);
                    dgvProducts.DataSource = products;
                    dgvProducts.DataBind();


                    if (shipment.GlobalDiscountPercent == 0)
                    {
                        discount1.Visible = false;
                    }
                    else
                    {
                        lblDiscount1Desc.InnerText = shipment.GlobalDiscountDesc;
                        lblDiscount1.InnerText = shipment.GlobalDiscountAmount1.ToString("C");

                    }

                    if (shipment.GlobalDiscount2Percent == 0)
                    {
                        discount2.Visible = false;
                    }
                    else
                    {
                        lblDiscount2Desc.InnerText = shipment.GlobalDiscount2Desc;
                        lblDiscount2.InnerText = shipment.GlobalDiscountAmount2.ToString("C");
                    }
                    if (shipment.GlobalDiscount3Percent == 0)
                    {
                        discount3.Visible = false;
                    }
                    else
                    {
                        lblDiscount3Desc.InnerText = shipment.GlobalDiscount3Desc;
                        lblDiscount3.InnerText = shipment.GlobalDiscountAmount3.ToString("C");
                    }
                    if (shipment.GlobalDiscount4Percent == 0)
                    {
                        discount4.Visible = false;
                    }
                    else
                    {
                        lblDiscount4Desc.InnerText = shipment.GlobalDiscount3Desc;
                        lblDiscount4.InnerText = shipment.GlobalDiscountAmount4.ToString("C");

                    }
                    if (shipment.GlobalDiscount5Percent == 0)
                    {
                        discount5.Visible = false;
                    }
                    else
                    {
                        lblDiscount5Desc.InnerText = shipment.GlobalDiscount5Desc;
                        lblDiscount5.InnerText = shipment.GlobalDiscountAmount5.ToString("C");
                    }

                    pSubtotal.InnerText = subTotal.ToString("C");

                    //2024 bulk rack changes
                    //as part of this project, the product discounts have been moved up into each line item
                    //we display the discounted 'each' price on each item
                    //the 'DiscountTotal' will now always be $0.00 - so for now we are just hiding it
                    //in the future,  we might need to delete this entirely,  or refactor to display something else
                    discountTotal.Visible = false;

                    TaxTypeText.InnerText = $"{TaxUtilities.GetTaxTypeText(order.ShippingState)} (#869614107)";
                    GSTText.InnerText = (shipment.GSTAmount + shipment.PSTAmount).ToString("C");
                    pShipping.InnerText = shipment.ShippingCharge.ToString("C");
                    pTotal.InnerText = shipment.ShipmentGrandTotal.ToString("C");
                }
            }
        }
    }
}