using Redbud.BL.DL;
using Redbud.BL.Helpers;
using Redbud.BL.Utils;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace Maddux.Catch.order
{
    public partial class confirmation : System.Web.UI.Page
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
                var order = db.Orders.FirstOrDefault(r => r.OrderID == OrderID);
                if (order != null)
                {
                    var shippedOrder = db.vwCustomerShippedOrders.FirstOrDefault(r => r.OrderID == OrderID);
                    
                    if (shippedOrder == null)
                    {
                        shipDateArea.Visible = false;
                    }
                    else
                    {
                        lblType.InnerText = "Order Confirmation";
                        lblConfirmationSent.InnerText = order.ConfirmationSentDate.HasValue ? order.ConfirmationSentDate.Value.ToString("MMMM d, yyyy") : "";
                        lblPOSent.InnerText = order.PurchaseOrdersSentDate.HasValue ? order.PurchaseOrdersSentDate.Value.ToString("MMMM d, yyyy") : "";
                    }

                    lblOrderDate.InnerText = order.OrderDate != null ? order.OrderDate?.ToString("MMMM d, yyyy") : "N/A";
                    lblDate.InnerText = DateTime.Now.ToString("MMMM d, yyyy");
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
                    lblnotes.InnerText = order.OrderNotes;
                    lblVendorNumber.InnerText = order.Customer.VendorNumber;


                    decimal gst = 0;
                    decimal pst = 0;
                    decimal shipping = 0;

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

                    var products = order.OrderItems.Select(r => new
                    {
                        Quantity = $"{r.Quantity} x {r.Product.PackagesPerUnit}",
                        r.Product.ProductName,
                        r.Product.UPCCode,
                        r.Product.Size,
                        Discount = r.DiscountPercent > 0 ? r.DiscountPercent.ToString("P2") : string.Empty,
                        EachPrice = OrderHelper.CalculateEachPrice(r.UnitPrice, r.DiscountPercent, r.Product.PackagesPerUnit),
                        ItemNo = r.Product.ItemNumber,
                        Available = !r.ProductNotAvailable,
                        Total = OrderHelper.GetTotalPrice(r.UnitPrice, r.DiscountPercent, r.Quantity)
                    }).ToList();

                    dgvProducts.DataSource = products;
                    dgvProducts.DataBind();



                    double subTotal = products.Where(p => p.Available).Sum(r => r.Total);
                    double discount = 0;

                    if (order.GlobalDiscountPercent == 0)
                    {
                        discount1.Visible = false;
                    }
                    else
                    {
                        discount += order.GlobalDiscountPercent * subTotal;
                        double dis = order.GlobalDiscountPercent * subTotal;
                        lblDiscount1Desc.InnerText = order.GlobalDiscountDesc;
                        lblDiscount1.InnerText = (dis - (2 * dis)).ToString("C");

                    }

                    if (order.GlobalDiscount2Percent == 0)
                    {
                        discount2.Visible = false;
                    }
                    else
                    {
                        discount += order.GlobalDiscount2Percent * subTotal;
                        double dis = order.GlobalDiscount2Percent * subTotal;
                        lblDiscount2Desc.InnerText = order.GlobalDiscount2Desc;
                        lblDiscount2.InnerText = (dis - (2 * dis)).ToString("C");
                    }

                    if (order.GlobalDiscount3Percent == 0)
                    {
                        discount3.Visible = false;
                    }
                    else
                    {
                        discount += order.GlobalDiscount3Percent * subTotal;
                        double dis = order.GlobalDiscount3Percent * subTotal;
                        lblDiscount3Desc.InnerText = order.GlobalDiscount3Desc;
                        lblDiscount3.InnerText = (dis - (2 * dis)).ToString("C");
                    }

                    if (order.GlobalDiscount4Percent == 0)
                    {
                        discount4.Visible = false;
                    }
                    else
                    {
                        discount += order.GlobalDiscount4Percent * subTotal;
                        double dis = order.GlobalDiscount4Percent * subTotal;
                        lblDiscount4Desc.InnerText = order.GlobalDiscount4Desc;
                        lblDiscount4.InnerText = (dis - (2 * dis)).ToString("C");

                    }

                    if (order.GlobalDiscount5Percent == 0)
                    {
                        discount5.Visible = false;
                    }
                    else
                    {
                        discount += order.GlobalDiscount5Percent * subTotal;
                        double dis = order.GlobalDiscount5Percent * subTotal;
                        lblDiscount5Desc.InnerText = order.GlobalDiscount5Desc;
                        lblDiscount5.InnerText = (dis - (2 * dis)).ToString("C");
                    }

                    pSubtotal.InnerText = order.DiscountedSubTotal.ToString("C");

                    TaxTypeText.InnerText = $"{TaxUtilities.GetTaxTypeText(order.ShippingState)} (#869614107)";
                    GSTText.InnerText = (gst + pst).ToString("C");
                    pShipping.InnerText = shipping.ToString("C");
                    pTotal.InnerText = ((decimal)order.DiscountedSubTotal + order.GSTAmount + order.PSTAmount + shipping - (decimal)discount).ToString("C");
                }
            }
        }
       
    }

}