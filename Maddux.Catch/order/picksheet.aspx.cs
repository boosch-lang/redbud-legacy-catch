using Redbud.BL.DL;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace Maddux.Catch.order
{
    public partial class picksheet : System.Web.UI.Page
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

        protected void Page_Load(object sender, EventArgs e)
        {
            using (var db = new MadduxEntities())
            {
                var order = db.Orders.FirstOrDefault(r => r.OrderID == OrderID);
                if (order != null)
                {
                    var shippedOrder = db.vwCustomerShippedOrders.FirstOrDefault(r => r.OrderID == OrderID);
                    var customer = db.Customers.FirstOrDefault(x => x.CustomerId == order.CustomerID);
                    //var shipment = db.vwMyShipments.Where(x => x.ShipmentID == ShipmentID).FirstOrDefault();
                    //var invoiceBatch = db.InvoicePostBatchItems.Include(x => x.InvoicePostBatch).FirstOrDefault(x => x.ShipmentID == ShipmentID);
                    if (shippedOrder == null)
                    {
                        shipDateArea.Visible = false;
                    }
                    else
                    {
                        lblType.InnerText = "Pick Sheet";
                        lblConfirmationSent.InnerText = order.ConfirmationSentDate.HasValue ? order.ConfirmationSentDate.Value.ToString("MMMM d, yyyy") : "";
                        lblPOSent.InnerText = order.PurchaseOrdersSentDate.HasValue ? order.PurchaseOrdersSentDate.Value.ToString("MMMM d, yyyy") : "";
                    }
                    lblPhone.InnerText = customer.Phone;
                    lblOrderDate.InnerText = order.OrderDate != null ? order.OrderDate?.ToString("MMMM d, yyyy") : "N/A";
                    lblDate.InnerText = DateTime.Now.ToString("MMMM d, yyyy");
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
                    lblVendorNumber.InnerText = customer.VendorNumber;


                    decimal gst = 0;
                    decimal pst = 0;
                    decimal shipping = 0;
                    double subTotal = 0;
                    double totalWeight = 0;
                    double totalSize = 0;
                    int totalPackages = 0;

                    gst = order.GSTAmount;
                    pst = order.PSTAmount;
                    shipping = order.ShippingCharge;

                    if (order.OrderRacks.Any())
                    {
                        var rack = order.OrderRacks.FirstOrDefault().ProductCatalogRack;
                        lblRackName.InnerText = rack.CatalogName + " " + rack.RackName;

                        var products = order.OrderItems.Where(p => p.ProductNotAvailable == false).Select(r => new
                        {
                            Quantity = r.Quantity + " x " + r.Product.PackagesPerUnit + " x " + r.Product.ItemsPerPackage,
                            r.Product.ProductName,
                            r.Product.UPCCode,
                            r.Product.Size,
                            UnitPrice = r.UnitPrice * r.Quantity,
                            Volume = r.Product.UnitSize,
                            Weight = r.Product.UnitWeight,
                            UnshippedQty = r.UnshippedQuantity,
                            //PgNo = r.Product.CatalogPageStart.ToString(),
                            //SuggRetail = r.Product.SuggestedPackageRetail,
                            EachPrice = r.UnitPrice / r.Product.PackagesPerUnit,
                            ItemNo = r.Product.ItemNumber,
                            Qty = r.Quantity
                        }).ToList();
                        subTotal = products.Sum(r => r.UnitPrice);
                        totalPackages = products.Sum(r => Convert.ToInt32(r.Qty));
                        totalSize = products.Sum(r => r.Volume);
                        totalWeight = products.Sum(r => r.Weight);
                        dgvProducts.DataSource = products;
                        dgvProducts.DataBind();
                    }
                    else
                    {
                        lblRackName.InnerText = order.OrderItems.First().Product.ProductCatalog.CatalogName;
                        var products = order.OrderItems.Select(r => new
                        {
                            QuantityOrdered = r.Quantity,
                            Quantity = r.Quantity + " x " + r.Product.PackagesPerUnit + " x " + r.Product.ItemsPerPackage,
                            PgNo = r.Product.CatalogPageStart.ToString(),
                            ItemNo = r.Product.ItemNumber,
                            r.Product.ProductName,
                            r.Product.UPCCode,
                            Volume = r.Product.UnitSize,
                            Weight = r.Product.UnitWeight,
                            SuggRetail = r.Product.SuggestedPackageRetail,
                            EachPrice = (r.UnitPrice / r.Product.PackagesPerUnit).ToString("C"),
                            UnshippedQty = r.UnshippedQuantity,
                            UnitPrice = r.UnitPrice * r.Quantity
                        }).ToList();
                        subTotal = products.Sum(r => r.UnitPrice);
                        totalPackages = products.Sum(r => Convert.ToInt32(r.QuantityOrdered));
                        totalSize = products.Sum(r => r.Volume);
                        totalWeight = products.Sum(r => r.Weight);
                        dgvProducts.DataSource = products;
                        dgvProducts.DataBind();
                    }
                    TotalPackages.InnerText = totalPackages.ToString();
                    //TotalSize.InnerText = totalSize.ToString()+" lbs.";
                    //TotalWeight.InnerText = totalWeight.ToString() + " cu. ft.";
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

                    pSubtotal.InnerText = subTotal.ToString("C");
                    pTaxes.InnerText = (gst + pst).ToString("C");
                    pShipping.InnerText = shipping.ToString("C");
                    pTotal.InnerText = ((decimal)subTotal + gst + pst + shipping - (decimal)discount).ToString("C");
                }
            }
        }
    }
}