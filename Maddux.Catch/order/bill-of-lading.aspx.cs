using Maddux.Catch.LocalClasses;
using Redbud.BL.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Maddux.Catch.order
{
    public class BOLRackItem
    {
        public int NoOFPics { get; set; }
        public string Catalogue { get; set; }
        public string Dimensions { get; set; }
        public double Weight { get; set; }
    }
    public partial class bill_of_lading : System.Web.UI.Page
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

                var order = db.Orders.FirstOrDefault(x => x.OrderID == OrderID);
                if (order != null)
                {

                    var customer = db.Customers.FirstOrDefault(x => x.CustomerId == order.CustomerID);
                    var currentUser = AppSession.Current.CurrentUser;

                    txtShipperRefNo.Text = order.OrderID.ToString();
                    txtPurchaseOrder.Text = order.PONumber;
                    txtPrintName.Text = currentUser.FullName;
                    txtDay.Text = order.RequestedShipDate?.ToString("dd");
                    txtMonth.Text = order.RequestedShipDate?.ToString("MM");
                    txtYear.Text = order.RequestedShipDate?.ToString("yy");

                    txtConsigneeTelephoneNo.Text = customer.Phone;
                    txtConsigneeName.Text = customer.Company;
                    txtConsigneeDeliveryAddress.Text = order.ShippingAddress;
                    txtConsigneeCity.Text = order.ShippingCity;
                    txtConsigneeProvince.Text = order.ShippingState;
                    txtConsigneePostalCode.Text = order.ShippingZip;

                    txtShipperName.Text = "REDBUD SUPPLY INC.";
                    if (Request.QueryString["shipperInfo"] != null)
                    {
                        string shipperInfo = Request.QueryString["shipperInfo"];
                        shipperInfo = HttpUtility.HtmlDecode(shipperInfo);
                        var info = shipperInfo.Split('#');
                        txtShipperAccountNo.Text = info[0];
                        txtShipperPhoneNo.Text = info[1];
                        txtShipperName.Text = info[2];
                        txtShipperPickupAddress.Text = info[3];
                        txtShipperCity.Text = info[4];
                        txtShipperProvince.Text = info[5];

                        txtShipperPostalCode.Text = info[6];

                    }

                    if (order.OrderRacks.Any())
                    {
                        var _product = order.OrderRacks.FirstOrDefault().OrderItems.Select(x => x.Product).FirstOrDefault();


                        var products = order.OrderItems.Select(r => new
                        {
                            Quantity = r.Quantity + " x " + r.Product.PackagesPerUnit + " x " + r.Product.ItemsPerPackage,
                            r.Product.ProductName,
                            r.Product.UPCCode,
                            r.Product.Size,
                            UnitPrice = r.UnitPrice * r.Quantity,
                            Volume = r.Product.UnitSize,
                            Weight = r.Product.UnitWeight,
                            UnshippedQty = r.UnshippedQuantity,
                            EachPrice = r.UnitPrice / r.Product.PackagesPerUnit,
                            ItemNo = r.Product.ItemNumber,
                            Qty = r.Quantity
                        }).ToList();
                        var totalSize = products.Sum(r => r.Volume);
                        var totalWeight = products.Sum(r => r.Weight);

                        List<BOLRackItem> rackItems = new List<BOLRackItem>();
                        foreach (var rack in order.OrderRacks)
                        {
                            var catalog = rack.ProductCatalogRack;
                            BOLRackItem bolRackItem = new BOLRackItem
                            {
                                NoOFPics = 1,
                                Catalogue = catalog.RackName,
                                Dimensions = rack.ProductCatalogRack.Dimensions,
                                Weight = string.IsNullOrEmpty(rack.ProductCatalogRack.Weight) ? 0.0 : double.Parse(rack.ProductCatalogRack.Weight)
                            };
                            rackItems.Add(bolRackItem);


                        }
                        rptOrderRack.DataSource = rackItems;
                        rptOrderRack.DataBind();
                        //txtNoOfPics.Text = rackItems.Sum(x => x.NoOFPics).ToString();


                    }
                    else
                    {
                        var _product = order.OrderItems.Select(x => x.Product).FirstOrDefault();


                        List<BOLRackItem> rackItems = new List<BOLRackItem>();
                        BOLRackItem bolRackItem = new BOLRackItem
                        {
                            Catalogue = order.OrderItems.First().Product.ProductCatalog.CatalogName
                        };

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

                        var totalSize = products.Sum(r => r.Volume);
                        bolRackItem.Weight = products.Sum(r => r.Weight);
                        bolRackItem.NoOFPics = 1;
                        bolRackItem.Dimensions = totalSize > 0 ? totalSize.ToString() : string.Empty;



                        rackItems.Add(bolRackItem);

                        txtNoOfPics.Text = rackItems.Sum(x => x.NoOFPics).ToString();

                        rptOrderRack.DataSource = rackItems;
                        rptOrderRack.DataBind();
                    }

                }
            }

        }
    }
}