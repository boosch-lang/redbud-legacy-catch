using Redbud.BL.DL;
using Redbud.BL.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Maddux.Pitch
{
    public partial class orderdetail : System.Web.UI.Page
    {
        public int OrderID
        {
            get
            {
                if (ViewState["OrderID"] == null)
                {
                    if (Request.QueryString["id"] == null || Request.QueryString["id"] == "")
                        ViewState["OrderID"] = -1;
                    else
                        ViewState["OrderID"] = Request.QueryString["id"];
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
            try
            {
                using (var db = new MadduxEntities())
                {
                    var order = db.Orders
                        .Include(o => o.OrderItems)
                        .FirstOrDefault(r => r.OrderID == OrderID);

                    if (order == null)
                    {
                        //show a message
                        litError.InnerHtml = StringTools.GenerateError("Order not found");

                    }
                    else
                    {
                        if (!Page.IsPostBack)
                        {
                            //if order is bulk,  get the discount from parent rack  (use bulkRackID)
                            double discount = 0;
                            if (order.BulkRackID.HasValue)
                            {
                                var rDiscount = db.ProductCatalogRacks.Where(r => r.RackID == order.BulkRackID).Select(r => r.Discount).FirstOrDefault();
                                discount = rDiscount.HasValue ? (rDiscount.Value * 100) : 0;
                            }

                            var shippedOrder = db.vwCustomerShippedOrders.FirstOrDefault(r => r.OrderID == OrderID);
                            var rack = order.OrderRacks.FirstOrDefault();
                            if (rack != null)
                            {
                                var orderModel = new OrderRackModel
                                {
                                    RackID = rack.RackId,
                                    RackName = rack.ProductCatalogRack.RackName,
                                    CatalogName = rack.ProductCatalogRack.CatalogName,
                                    RackDescription = rack.ProductCatalogRack.RackDesc,
                                    RackImage = rack.ProductCatalogRack.DisplayPhotoPath,
                                    PurchaseOrderNumber = string.IsNullOrEmpty(order.PONumber) ? "N/A" : order.PONumber,
                                    ShipWeek = order.RequestedShipDate.Value.ToString("MMMM dd, yyyy"),
                                    ShipTo = order.ShippingAddress + "<br/> " + order.ShippingCity + ", " + order.ShippingState + "<br/> " + order.ShippingCountry,
                                    Discount = discount,
                                    SubTotal = order.SubTotal.ToString("C"),
                                    IsShipped = shippedOrder != null
                                };

                                orderModel.OrderItems = rack.OrderItems.Where(r => r.Quantity > 0).Select(r => new OrderItemModel
                                {
                                    ProductID = r.ProductId,
                                    Quantity = r.Quantity,
                                    QuantityTotal = r.Quantity + " x " + r.Product.PackagesPerUnit,
                                    ProductName = r.Product.ProductName,
                                    Size = r.Product.Size,
                                    ItemNumber = r.Product.ItemNumber,
                                    PhotoCount = r.Product.Photos.Count(),
                                    Photos = r.Product.Photos.Select(x => new RackProductPhotoResponse
                                    {
                                        PhotoPath = x.PhotoPath,
                                        ProductId = r.ProductId,
                                        PhotoId = x.PhotoID,
                                        ProductName = r.Product.ProductName

                                    }).Skip(1).ToList(),
                                    ItemPhoto = r.Product.Photos.FirstOrDefault() != null ? r.Product.Photos.FirstOrDefault().PhotoPath : "",
                                }).OrderBy(x => x.Size).ThenBy(x => x.ProductName).ToList();

                                lvRacks.DataSource = new[] { orderModel }.ToList();
                            }
                            else
                            {
                                var orderModel = new OrderRackModel
                                {
                                    RackID = 0,
                                    RackName = "Not available",
                                    CatalogName = string.Empty,
                                    RackDescription = string.Empty,
                                    RackImage = string.Empty,
                                    PurchaseOrderNumber = string.IsNullOrEmpty(order.PONumber) ? "N/A" : order.PONumber,
                                    ShipWeek = order.RequestedShipDate.Value.ToString("MMMM dd, yyyy"),
                                    ShipTo = order.ShippingAddress + "<br/> " + order.ShippingCity + ", " + order.ShippingState + "<br/> " + order.ShippingCountry,
                                    Discount = discount,
                                    SubTotal = order.SubTotal.ToString("C"),
                                    IsShipped = shippedOrder != null
                                };

                                orderModel.OrderItems = order.OrderItems.Where(r => r.Quantity > 0).Select(r => new OrderItemModel
                                {
                                    ProductID = r.ProductId,
                                    Quantity = r.Quantity,
                                    QuantityTotal = r.Quantity + " x " + r.Product.PackagesPerUnit,
                                    ProductName = r.Product.ProductName,
                                    Size = r.Product.Size,
                                    ItemNumber = r.Product.ItemNumber,
                                    PhotoCount = r.Product.Photos.Count(),
                                    Photos = r.Product.Photos.Select(x => new RackProductPhotoResponse
                                    {
                                        PhotoPath = x.PhotoPath,
                                        ProductId = r.ProductId,
                                        PhotoId = x.PhotoID,
                                        ProductName = r.Product.ProductName

                                    }).Skip(1).ToList(),
                                    ItemPhoto = r.Product.Photos.FirstOrDefault() != null ? r.Product.Photos.FirstOrDefault().PhotoPath : "",
                                }).OrderBy(x => x.Size).ThenBy(x => x.ProductName).ToList();

                                lvRacks.DataSource = new[] { orderModel }.ToList();
                            }

                            lvRacks.DataBind();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                litError.InnerText = ex.Message;
            }
        }

    }

    public class OrderRackModel
    {
        public int RackID { get; set; }

        public string RackName { get; set; }

        public string RackDescription { get; set; }

        public string CatalogName { get; set; }

        public string RackImage { get; set; }

        public string PurchaseOrderNumber { get; set; }

        public string ShipTo { get; set; }

        public string ShipWeek { get; set; }

        public double Discount { get; set; }

        public string SubTotal { get; set; }

        public bool IsShipped { get; set; }

        public List<OrderItemModel> OrderItems { get; set; } = new List<OrderItemModel>();

    }
    public class OrderItemModel
    {
        public int ProductID { get; set; }
        public double Quantity { get; set; }
        public string QuantityTotal { get; set; }

        public string ProductName { get; set; }

        public string Size { get; set; }
        public string ItemNumber { get; set; }

        public string ItemPhoto { get; set; }

        public List<RackProductPhotoResponse> Photos { get; set; } = new List<RackProductPhotoResponse>();

        public int PhotoCount { get; set; }

    }

}