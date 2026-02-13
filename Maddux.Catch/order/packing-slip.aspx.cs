using Redbud.BL.DL;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.UI.WebControls;

namespace Maddux.Catch.order
{
    public partial class packing_slip : System.Web.UI.Page
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
                var order = db.Orders
                    .Include(x => x.OrderRacks)
                    .Include(x => x.OrderItems)
                    .AsNoTracking()
                    .FirstOrDefault(r => r.OrderID == OrderID);

                if (order != null)
                {
                    var hasShippedOrders = db.vwCustomerShippedOrders.Any(r => r.OrderID == OrderID);
                    var shipment = db.Shipments.AsNoTracking()
                        .Include(s => s.ShipmentItems)
                        .Include(s => s.ShipmentItems.Select(si => si.OrderItem))
                        .FirstOrDefault(x => x.ShipmentID == ShipmentID);

                    if (!hasShippedOrders)
                    {
                        shipDateArea.Visible = false;
                    }
                    else
                    {
                        lblType.InnerText = "Packing Slip";
                        lblConfirmationSent.InnerText = order.ConfirmationSentDate.HasValue ? order.ConfirmationSentDate.Value.ToString("MMMM d, yyyy") : "";
                        lblPOSent.InnerText = order.PurchaseOrdersSentDate.HasValue ? order.PurchaseOrdersSentDate.Value.ToString("MMMM d, yyyy") : "";
                    }

                    lblDatePrinted.InnerText = DateTime.Now.ToString("dd-MMM-yyyy");
                    lblShipmentNumber.InnerText = shipment.ShipmentID.ToString();
                    lblOrderDate.InnerText = order.OrderDate?.ToString("dd-MMM-yyyy");

                    //lblDate.InnerText = DateTime.Now.ToString("MMMM d, yyyy");
                    lblID.InnerText = order.OrderID.ToString();

                    lblRequestedShipDate.InnerText = order.RequestedShipDate.HasValue ? order.RequestedShipDate.Value.ToString("MMMM d, yyyy") : "";
                    //lblOrderDate.InnerText = order.OrderDate.HasValue ? order.OrderDate.Value.ToString("MMMM d, yyyy") : "";
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
                    if (order.GlobalDiscountPercent == 0)
                    {
                        discount1.Visible = false;
                    }
                    else
                    {
                        lblDiscount1Desc.InnerText = order.GlobalDiscountDesc;
                        lblDiscount1.InnerText = order.GlobalDiscountPercent.ToString();

                    }

                    if (order.GlobalDiscount2Percent == 0)
                    {
                        discount2.Visible = false;
                    }
                    else
                    {
                        lblDiscount2Desc.InnerText = order.GlobalDiscount2Desc;
                        lblDiscount2.InnerText = order.GlobalDiscount2Percent.ToString();
                    }
                    if (order.GlobalDiscount3Percent == 0)
                    {
                        discount3.Visible = false;
                    }
                    else
                    {

                        lblDiscount3Desc.InnerText = order.GlobalDiscount3Desc;
                        lblDiscount3.InnerText = order.GlobalDiscount3Percent.ToString();
                    }
                    if (order.GlobalDiscount4Percent == 0)
                    {
                        discount4.Visible = false;
                    }
                    else
                    {
                        lblDiscount4Desc.InnerText = order.GlobalDiscount4Desc;
                        lblDiscount4.InnerText = order.GlobalDiscount4Percent.ToString();

                    }
                    if (order.GlobalDiscount5Percent == 0)
                    {
                        discount5.Visible = false;
                    }
                    else
                    {
                        lblDiscount5Desc.InnerText = order.GlobalDiscount5Desc;
                        lblDiscount5.InnerText = order.GlobalDiscount5Percent.ToString();
                    }


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

                        var products = shipment.ShipmentItems.Select(r => new
                        {
                            Quantity = r.Quantity + " x " + r.OrderItem.Product.PackagesPerUnit + " x " + r.OrderItem.Product.ItemsPerPackage,
                            r.OrderItem.Product.ProductName,
                            r.OrderItem.Product.UPCCode,
                            r.OrderItem.Product.Size,
                            UnitPrice = r.OrderItem.UnitPrice * r.Quantity,
                            PgNo = r.OrderItem.Product.CatalogPageStart.ToString(),
                            SuggRetail = r.OrderItem.Product.SuggestedPackageRetail,
                            EachPrice = r.OrderItem.UnitPrice / r.OrderItem.Product.PackagesPerUnit,
                            ItemNo = r.OrderItem.Product.ItemNumber,
                        }).ToList();
                        subTotal = products.Sum(r => r.UnitPrice * int.Parse(r.Quantity.Split('x')[0]));
                        dgvProducts.DataSource = products;
                        dgvProducts.DataBind();
                    }
                    else
                    {
                        lblRackName.InnerText = order.OrderItems.First().Product.ProductCatalog.CatalogName;
                        var products = shipment.ShipmentItems.Select(r => new
                        {
                            QuantityOrdered = r.Quantity,
                            Quantity = r.Quantity + " x " + r.OrderItem.Product.PackagesPerUnit + " x " + r.OrderItem.Product.ItemsPerPackage,
                            PgNo = r.OrderItem.Product.CatalogPageStart.ToString(),
                            ItemNo = r.OrderItem.Product.ItemNumber,
                            r.OrderItem.Product.ProductName,
                            r.OrderItem.Product.UPCCode,
                            SuggRetail = r.OrderItem.Product.SuggestedPackageRetail,
                            EachPrice = (r.OrderItem.UnitPrice / r.OrderItem.Product.PackagesPerUnit).ToString("C"),
                            UnitPrice = r.OrderItem.UnitPrice * r.Quantity
                        }).ToList();
                        subTotal = products.Sum(r => r.UnitPrice * r.QuantityOrdered);
                        dgvProducts.DataSource = products;
                        dgvProducts.DataBind();
                    }
                    pSubtotal.InnerText = subTotal.ToString("C");
                    pTaxes.InnerText = (gst + pst).ToString("C");
                    pShipping.InnerText = shipping.ToString("C");
                    pTotal.InnerText = ((decimal)subTotal + gst + pst + shipping).ToString("C");
                }
            }
        }
    }
}