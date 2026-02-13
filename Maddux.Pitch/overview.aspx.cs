using Maddux.Pitch.LocalClasses;
using Redbud.BL.DL;
using Redbud.BL.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Maddux.Pitch
{
    public partial class overview : System.Web.UI.Page
    {
        public class ReorderProduct
        {
            public int ProductID { get; set; }
            public double Quantity { get; set; }
        }
        public class ShipdateObject
        {
            public DateTime ShipDate { get; set; }
        }
        public List<ShipdateObject> shipadates = new List<ShipdateObject>();
        private List<Customer> subsTable;
        public List<ReorderProduct> reorderProductList;

        public int RackID
        {
            get
            {
                if (ViewState["RackID"] == null)
                {
                    if (Request.QueryString["rackid"] == null || Request.QueryString["rackid"] == "")
                        ViewState["RackID"] = -1;
                    else
                        ViewState["RackID"] = Request.QueryString["rackid"];
                }
                return Convert.ToInt32(ViewState["RackID"].ToString());
            }

            set
            {
                ViewState["RackID"] = value;
            }
        }

        public int OrderID
        {
            get
            {
                if (ViewState["OrderID"] == null)
                {
                    if (Request.QueryString["orderid"] == null || Request.QueryString["orderid"] == "")
                        ViewState["OrderID"] = -1;
                    else
                        ViewState["OrderID"] = Request.QueryString["orderid"];
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
                if (!Page.IsPostBack)
                {
                    using (var db = new MadduxEntities())
                    {
                        if (OrderID != -1)
                        {
                            RackID = db.OrderRacks.Where(or => or.OrderId == OrderID).Select(or => or.RackId).FirstOrDefault();
                        }
                        Customer theCustomer = AppSession.Current.CurrentCustomer;
                        OrderByField.Text = theCustomer.FirstName + " " + theCustomer.LastName;
                        ProductCatalogRack theRack = db.ProductCatalogRacks.Include(x => x.RackProducts.Select(z => z.Product.Photos)).FirstOrDefault(r => r.RackID == RackID);
                        pnlCustomize.Visible = theRack.AllowCustomization;
                        imgRackPhoto.ImageUrl = theRack.DisplayPhotoPath;

                        hfRackID.Value = theRack.RackID.ToString();
                        string catalogName = theRack.RackNameHTML;
                        litCheckoutRack.Text = theRack.RackNameHTML;
                        litRackProgram.Text = theRack.CatalogName;
                        imgRackOverview.ImageUrl = theRack.DisplayPhotoPath;
                        catalogNameTitle.InnerHtml = catalogName;
                        RackChoiceName.Text = catalogName;
                        string minItems = Convert.ToString(theRack.MinimumItems);
                        string maxItems = Convert.ToString(theRack.MaximumItems);
                        bool allowCustomization = theRack.AllowCustomization;
                        if (allowCustomization)
                        {
                            allowCustomString.Value = "allow";
                        }
                        else
                        {
                            allowCustomString.Value = "notAllow";
                        }

                        if (theRack != null && OrderID != -1)
                        {
                            var order = db.OrderItems.Where(r => r.OrderId == OrderID);
                            reorderProductList = new List<ReorderProduct>();
                            foreach (var item in order)
                            {
                                reorderProductList.Add(new ReorderProduct { ProductID = item.ProductId, Quantity = item.Quantity });
                            }
                            rptSelectedProducts.DataSource = theRack.RackProducts.Select(rp => new
                            {
                                rp.RackID,
                                rp.ProductID,
                                rp.UnitPrice,
                                rp.HasProductPhoto,
                                rp.ProductPhotoPath,
                                rp.ProductName,
                                UnitPriceFormatted = (rp.UnitPrice / rp.UnitPerCase).ToString("C"),
                                rp.UnitPerCase,
                                DefaultQuantity = reorderProductList.Where(r => r.ProductID == rp.ProductID).Select(r => r.Quantity).FirstOrDefault(),
                                rp.Product.UPCCode,
                                Photos = rp.Product.Photos.Select(p => new
                                {
                                    p.PhotoPath,
                                    rp.ProductName,
                                    rp.ProductID
                                }).Skip(1)
                            }).OrderBy(rp => rp.ProductName).ToList();
                            rptSelectedProducts.DataBind();

                            repBestMix.DataSource = theRack.RackProducts.Select(rp => new
                            {
                                rp.RackID,
                                rp.ProductID,
                                rp.UnitPrice,
                                rp.HasProductPhoto,
                                rp.ProductPhotoPath,
                                rp.ProductName,
                                UnitPriceFormatted = (rp.UnitPrice / rp.UnitPerCase).ToString("C"),
                                rp.UnitPerCase,
                                DefaultQuantity = reorderProductList.Where(r => r.ProductID == rp.ProductID).Select(r => r.Quantity).FirstOrDefault(),
                                rp.Product.UPCCode,
                                Photos = rp.Product.Photos.Select(p => new
                                {
                                    p.PhotoPath,
                                    rp.ProductName,
                                    rp.ProductID
                                }).Skip(1)
                            })
                                .Where(x => x.DefaultQuantity > 0)
                                .OrderBy(rp => rp.ProductName).ToList();
                            repBestMix.DataBind();
                        }
                        else if (theRack != null && RackID != -1)
                        {
                            rptSelectedProducts.DataSource = theRack.RackProducts.Select(rp => new
                            {
                                rp.RackID,
                                rp.ProductID,
                                rp.UnitPrice,
                                rp.HasProductPhoto,
                                rp.ProductPhotoPath,
                                rp.ProductName,
                                UnitPriceFormatted = (rp.UnitPrice / rp.UnitPerCase).ToString("C"),
                                rp.UnitPerCase,
                                rp.DefaultQuantity,
                                rp.Product.UPCCode,
                                Photos = rp.Product.Photos.Select(p => new
                                {
                                    p.PhotoPath,
                                    rp.ProductName,
                                    rp.ProductID
                                }).Skip(1)
                            }).OrderBy(rp => rp.ProductName).ToList();
                            rptSelectedProducts.DataBind();

                            repBestMix.DataSource = theRack.RackProducts.Select(rp => new
                            {
                                rp.RackID,
                                rp.ProductID,
                                rp.UnitPrice,
                                rp.HasProductPhoto,
                                rp.ProductPhotoPath,
                                rp.ProductName,
                                UnitPriceFormatted = (rp.UnitPrice / rp.UnitPerCase).ToString("C"),
                                rp.UnitPerCase,
                                rp.DefaultQuantity,
                                rp.Product.UPCCode,
                                Photos = rp.Product.Photos.Select(p => new
                                {
                                    p.PhotoPath,
                                    rp.ProductName,
                                    rp.ProductID
                                }).Skip(1)
                            })
                                .Where(x => x.DefaultQuantity > 0)
                                .OrderBy(rp => rp.ProductName).ToList();
                            repBestMix.DataBind();
                        }

                        //Product rack available ship dates
                        var productRackShipDates = db.ProductRackShipDates.Where(prsd => prsd.RackID == RackID && prsd.ShipDate > DateTime.Now && prsd.Active)
                                .Select(prsd => new RackShipDate
                                {
                                    ShipDate = prsd.ShipDate,
                                    OrderDeadlineDate = prsd.ProductCatalogRack.ProductCatalog.ProductCatalogShipDates.FirstOrDefault(x => x.ShipDate == prsd.ShipDate).OrderDeadlineDate,
                                    Available = prsd.Available
                                })
                                .OrderBy(prsd => prsd.ShipDate)
                                .ToList();


                        foreach (var date in productRackShipDates)
                        {
                            if (date.OrderDeadlineDate > DateTime.Now)
                            {
                                shipadates.Add(new ShipdateObject { ShipDate = date.ShipDate });
                            }
                            else
                            {
                                if (date.Available > 0)
                                {
                                    shipadates.Add(new ShipdateObject { ShipDate = date.ShipDate });
                                }
                            }
                        }

                        subsTable = theCustomer.GetSubCustomers().OrderBy(c => c.Company).ToList();
                        subsTable.Insert(0, new Customer
                        {
                            CustomerId = theCustomer.CustomerId,
                            Company = theCustomer.Company
                        });
                        repStores.DataSource = subsTable;
                        repStores.DataBind();

                    }
                }
            }
            catch (Exception ex)
            {
                Response.Redirect("/");
                throw ex;
            }
        }

        protected void btnPlaceOrder_Click(object sender, EventArgs e)
        {
            try
            {
                Customer theCustomer = AppSession.Current.CurrentCustomer;
                bool error = false;
                string errorMessage = "";
                //string OrderPONumber = PONumber.Text.TrimEnd();
                string OrderPlacedBy = theCustomer.FirstName + " " + theCustomer.LastName;
                string OrderPONumber = "";
                int rackID = Convert.ToInt32(RackID);

                int companyId = theCustomer.CustomerId;
                Repeater rptProducts = rptSelectedProducts;
                if (rptProducts.Items.Count == 0)
                {
                    error = true;
                    errorMessage += "There is no products in the rack! Please contact Administrator for details. <br />";
                }

                if (error == false)
                {
                    using (MadduxEntities db = new MadduxEntities())
                    {
                        foreach (RepeaterItem store in repStores.Items)
                        {
                            HiddenField hfCustomerID = store.FindControl("hfCompanyID") as HiddenField;
                            Repeater shipDates = store.FindControl("repShipDates") as Repeater;
                            int customerID = int.Parse(hfCustomerID.Value);

                            foreach (RepeaterItem shipDate in shipDates.Items)
                            {
                                HiddenField hdnShipDate = shipDate.FindControl("hdnShipDate") as HiddenField;
                                TextBox txtRackQuantity = shipDate.FindControl("txtRackQuantity") as TextBox;
                                DateTime theShipDate = Convert.ToDateTime(hdnShipDate.Value);
                                int rackQuantity = int.Parse(txtRackQuantity.Text);

                                if (rackQuantity > 0)
                                {
                                    for (int i = 0; i < rackQuantity; i++)
                                    {
                                        Customer customer = db.Customers.FirstOrDefault(c => c.CustomerId == companyId);
                                        List<OrderRack> racks = new List<OrderRack>();
                                        OrderRack orderRack = new OrderRack
                                        {
                                            Quantity = 1,
                                            RackId = rackID
                                        };
                                        List<OrderItem> items = new List<OrderItem>();

                                        int itemCount = 0;
                                        foreach (RepeaterItem product in rptProducts.Items)
                                        {
                                            ProductCatalogRack theRack = db.ProductCatalogRacks.FirstOrDefault(r => r.RackID == RackID);
                                            TextBox txtQuantity = product.FindControl("txtProductQuantity") as TextBox;
                                            HiddenField productID = product.FindControl("hdnProductID") as HiddenField;
                                            int currentID = int.Parse(productID.Value);
                                            string hdnUPCCode = theRack.RackProducts.Where(rp => rp.ProductID == currentID).Select(x => x.Product.UPCCode).FirstOrDefault();
                                            double productPrice = theRack.RackProducts.Where(rp => rp.ProductID == currentID).Select(x => x.UnitPrice).FirstOrDefault();
                                            if (int.Parse(txtQuantity.Text) > 0)
                                            {
                                                if (!string.IsNullOrWhiteSpace(hdnUPCCode))
                                                {
                                                    itemCount += int.Parse(txtQuantity.Text);
                                                }
                                                items.Add(new OrderItem
                                                {
                                                    Quantity = int.Parse(txtQuantity.Text),
                                                    ProductId = int.Parse(productID.Value),
                                                    DiscountPercent = 0,
                                                    LinePosition = 0,
                                                    NoGlobalDiscount = true,
                                                    ProductNotAvailable = false,
                                                    ProductNotAvailableDesc = "",
                                                    UnitPrice = productPrice

                                                });
                                            }
                                        }
                                        // Handle rack sizing issues
                                        List<OrderItem> expandedItems = new List<OrderItem>();
                                        foreach (OrderItem item in items)
                                        {
                                            for (int x = 0; x < item.Quantity; x++)
                                            {
                                                expandedItems.Add(new OrderItem
                                                {
                                                    Quantity = 1,
                                                    ProductId = item.ProductId,
                                                    DiscountPercent = 0,
                                                    LinePosition = 0,
                                                    NoGlobalDiscount = true,
                                                    ProductNotAvailable = false,
                                                    ProductNotAvailableDesc = "",
                                                    UnitPrice = item.UnitPrice
                                                });
                                            }
                                        }

                                        foreach (OrderItem item in expandedItems)
                                        {
                                            OrderItem existingProduct = orderRack.OrderItems.FirstOrDefault(r => r.ProductId == item.ProductId);
                                            if (existingProduct != null)
                                            {
                                                existingProduct.Quantity += 1;
                                            }
                                            else
                                            {
                                                orderRack.OrderItems.Add(item);
                                            }
                                        }

                                        racks.Add(orderRack);

                                        if (error == false)
                                        {

                                            int ShippingMethodID = db.supShippingMethods.Where(x => x.ShippingMethodDesc.StartsWith("LTL")).Select(x => x.ShippingMethodID).FirstOrDefault();
                                            var _state = db.States.FirstOrDefault(x => x.StateID == theCustomer.State);

                                            if (string.IsNullOrWhiteSpace(theCustomer.Zip) || string.IsNullOrWhiteSpace(theCustomer.Company) || string.IsNullOrWhiteSpace(theCustomer.Address) || string.IsNullOrWhiteSpace(theCustomer.State) || string.IsNullOrWhiteSpace(theCustomer.Country) || string.IsNullOrWhiteSpace(theCustomer.City))
                                            {
                                                throw new Exception($@"Shipping information is required for {theCustomer.Company}!");
                                            }
                                            if (string.IsNullOrWhiteSpace(theCustomer.BillingZip) || string.IsNullOrWhiteSpace(theCustomer.BillingCompany) || string.IsNullOrWhiteSpace(theCustomer.BillingAddress) || string.IsNullOrWhiteSpace(theCustomer.BillingState) || string.IsNullOrWhiteSpace(theCustomer.BillingCountry) || string.IsNullOrWhiteSpace(theCustomer.BillingCity))
                                            {
                                                throw new Exception($@"Billing information is required for {theCustomer.Company}!");
                                            }
                                            foreach (OrderRack item in racks)
                                            {
                                                double freightCharge = FreightCalculator.CalculateFreighCharge(item.GetRackTotal, _state.StateID, theCustomer.Zip);

                                                Order order = new Order
                                                {
                                                    OrderStatus = 0,
                                                    CustomerID = companyId,
                                                    RequestedShipDate = theShipDate,
                                                    BillingAddress = theCustomer.BillingAddress,
                                                    BillingCity = theCustomer.BillingCity,
                                                    BillingCountry = theCustomer.BillingCountry,
                                                    BillingName = theCustomer.BillingCompany,
                                                    BillingState = theCustomer.BillingState,
                                                    BillingZip = theCustomer.BillingZip,
                                                    DateCreated = DateTime.Now,
                                                    DateUpdated = DateTime.Now,
                                                    CreatedBy = OrderPlacedBy,
                                                    UpdatedBy = "",
                                                    ConfirmationSentDate = null,
                                                    CustomShippingCharge = false,
                                                    GlobalDiscount2Desc = null,
                                                    GlobalDiscount2Percent = 0,
                                                    GlobalDiscount3Desc = null,
                                                    GlobalDiscount3Percent = 0,
                                                    GlobalDiscount4Desc = null,
                                                    GlobalDiscount4Percent = 0,
                                                    GlobalDiscount5Desc = null,
                                                    GlobalDiscount5Percent = 0,
                                                    GlobalDiscountDesc = null,
                                                    GlobalDiscountPercent = 0,
                                                    GSTAmount = Convert.ToDecimal(TaxUtilities.GetTaxPercentage(_state.StateID, null) * (item.GetRackTotal + freightCharge)),
                                                    HST = _state.HST,
                                                    OfficeNotes = null,
                                                    OrderNotes = null,
                                                    PONumber = OrderPONumber,
                                                    OrderDate = null,
                                                    QuoteDate = null,
                                                    PSTAmount = 0,
                                                    PSTExempt = false,
                                                    PurchaseOrdersSentDate = null,
                                                    QuoteExpiryDate = null,
                                                    ShipDate = null,
                                                    SalesPersonID = theCustomer.SalesPersonID.GetValueOrDefault(0),
                                                    PaymentTermsID = theCustomer.DefaultTermsId,
                                                    ShippingMethodID = ShippingMethodID,
                                                    PaymentTypeID = 1,
                                                    ShippingCharge = Convert.ToDecimal(freightCharge),
                                                    ShippingAddress = customer.Address,
                                                    ShippingCity = customer.City,
                                                    ShippingCountry = customer.Country,
                                                    ShippingEmail = customer.Email,
                                                    ShippingName = customer.Company,
                                                    ShippingState = customer.State,
                                                    ShippingZip = customer.Zip,
                                                    ReceivedVia = 1

                                                };
                                                order.OrderRacks.Add(item);
                                                var subTotal = order.SubTotal;
                                                db.Orders.Add(order);

                                                db.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (!error)
                {
                    Response.Redirect("/cart.aspx?oSuccess=true", false);
                }
            }
            catch (Exception ex)
            {
                litError.InnerHtml = StringTools.GenerateError(ex.Message);
            }
        }

        protected void repStores_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    Repeater repShipDates = e.Item.FindControl("repShipDates") as Repeater;
                    repShipDates.DataSource = shipadates;
                    repShipDates.DataBind();

                }
            }
            catch (Exception ex)
            {
                //litError.InnerText = StringTools.GenerateError(ex.Message);
            }
        }
    }
}