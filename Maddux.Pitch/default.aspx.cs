using Maddux.Pitch.LocalClasses;
using Redbud.BL;
using Redbud.BL.DL;
using Redbud.BL.Helpers;
using Redbud.BL.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using static Redbud.BL.DL.Customer;

namespace Maddux.Pitch
{
    public class RackDate
    {
        public DateTime ShipDate { get; set; }
        public bool IsAvailable { get; set; }
        public bool SoldOut { get; set; }
        public int RackID { get; set; }
        public int CatalogID { get; set; }
        public int? ProgramID { get; set; }
        public string styleClass
        {
            get
            {
                if (IsAvailable)
                {
                    if (SoldOut)
                    {
                        return "sold-out";
                    }
                    else
                    {
                        return "available";
                    }
                }
                else
                {
                    return "unavailable";
                }
            }
        }
    }
    public class SelectedRack
    {
        public int Quantity { get; set; }
        public DateTime ShipDate { get; set; }
        public int RackID { get; set; }
        public int CatalogID { get; set; }
        public int? ProgramID { get; set; }
    }

    public class CatalogRackModel
    {
        public int RackID { get; set; }
        public int CatalogID { get; set; }
        public int? ProgramID { get; set; }
        public string RackDescription { get; set; }
        public string ProgramName { get; set; }
        public string RackImage { get; set; }
        public List<CatalogRackImage> RackImages { get; set; }
        public bool HasMultipleImages => RackImages.Any();
        public string TrayCount { get; set; }
        public bool AllowCustomization { get; set; }
        public List<RackDate> ShipDates { get; set; }
        public Double Price { get; set; }
        public bool HasNoShipDates => ShipDates.Count() == ShipDates.Count(x => x.IsAvailable == false);
        public string ProductCatalogDescription { get; set; }
        public int? MinimumItems { get; set; }
        public double? UnitCount { get; set; }
        public string ItemType { get; set; }
        public double Discount { get; set; }
        public int RackType { get; set; }
    }

    public class CatalogRackImage
    {
        public string PhotoPath { get; set; }
        public string RackName { get; set; }
        public int RackID { get; set; }
        public int PhotoId { get; set; }
    }

    public class ShipdateQuantity
    {
        public DateTime ShipDate { get; set; }
        public int Quantity { get; set; }
    }


    public partial class _default : System.Web.UI.Page
    {
        private decimal gridTotal;
        private decimal grandTotal;
        public string deadline;


        public int NumberOfStores
        {
            get
            {
                if (ViewState["NumberOfStores"] == null)
                {
                    ViewState["NumberOfStores"] = 0;
                }

                return Convert.ToInt32(ViewState["NumberOfStores"].ToString());
            }
            set
            {
                ViewState["NumberOfStores"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {

                    if (Request.QueryString["oSuccess"] != null && string.Equals(Request.QueryString["oSuccess"], "true"))
                    {
                        //litMessage.Text = GenerateSuccess("Your order(s) have been successfully submitted.");
                    }

                    //current logged in user
                    Customer theCustomer = AppSession.Current.CurrentCustomer;
                    //litFirstName.Text = theCustomer.FirstName;

                    var catalogs = theCustomer.GetActiveProgramWithRacks(false);
                    var shipDates = GetShipDates(catalogs);

                    // Set text and value manually
                    foreach (DateTime date in shipDates)
                    {
                        ddlShipWeekFilter.Items.Add(new ListItem(date.ToString("MMM dd yyyy"), date.ToString("MMM dd yyyy")));
                    }

                    //Current user associations
                    List<AssociationResult> membershipAssociations = theCustomer.MembershipAssociations();
                    if (membershipAssociations.Count > 0)
                    {
                        var html = string.Empty;

                        List<string> bannerMessages = new List<string>();
                        List<string> salesBannerMessages = new List<string>();

                        foreach (var membership in membershipAssociations)
                        {
                            if (!string.IsNullOrWhiteSpace(membership.BannerMessage) && membership.BannerEndDate >= DateTime.Now && membership.BannerStartDate <= DateTime.Now)
                            {
                                bannerMessages.Add(membership.BannerMessage);
                            }
                            if (!string.IsNullOrWhiteSpace(membership.SalesBannerMessage) && membership.SalesBannerEndDate >= DateTime.Now && membership.SalesBannerStartDate <= DateTime.Now)
                            {
                                salesBannerMessages.Add(membership.SalesBannerMessage);
                            }
                        }
                        foreach (string message in bannerMessages.Distinct())
                        {
                            html += StringTools.GenerateBanner(message);
                        }
                        foreach (string message in salesBannerMessages.Distinct())
                        {
                            html += StringTools.GenerateSalesBanner(message);
                        }

                        //litWelcomeMessage.Text = html;
                    }

                    using (var db = new MadduxEntities())
                    {
                        List<CatalogRackModel> catalogRacks = new List<CatalogRackModel>();
                        foreach (ProductCatalog catalog in catalogs.OrderBy(x => x.DisplayOrder))
                        {
                            foreach (ProductCatalogRack rack in catalog.ProductCatalogRacks.OrderBy(x => x.DisplayOrder))
                            {
                                ProductCatalogRack theRack = db.ProductCatalogRacks.FirstOrDefault(r => r.RackID == rack.RackID);
                                var itemType = theRack.RackType == (int)RackType.Standard ? "tray" : "Smart Stack";
                                CatalogRackModel model = new CatalogRackModel
                                {
                                    RackID = theRack.RackID,
                                    CatalogID = catalog.CatalogId,
                                    ProgramID = catalog.ProgramID,
                                    ProgramName = catalog.ProductProgram.ProgramName,
                                    RackDescription = rack.RackName,
                                    RackType = rack.RackType,
                                    RackImage = theRack.Photos.Any() ? theRack.Photos.Select(p => p.PhotoPath).First() : "/img/rack-not-available.jpg",
                                    RackImages = theRack.Photos.Select(p => new CatalogRackImage
                                    {
                                        PhotoId = p.PhotoID,
                                        PhotoPath = p.PhotoPath,
                                        RackName = theRack.RackName,
                                        RackID = theRack.RackID
                                    }).Skip(1).ToList(),
                                    TrayCount = $"{rack.MaximumItems} tray(s)",
                                    AllowCustomization = theRack.AllowCustomization,
                                    ShipDates = new List<RackDate>(),
                                    ProductCatalogDescription = theRack.RackDesc,
                                    MinimumItems = theRack.MinimumItems,
                                    ItemType = theRack.MinimumItems > 1 ? $"{itemType}s" : itemType,
                                    Discount = theRack.Discount != null ? ((theRack.Discount ?? 0) * 100) : 0
                                };

                                var rackDiscount = theRack.Discount.HasValue ? theRack.Discount.Value : 0;

                                if (theRack.RackType == (int)RackType.Standard)
                                {
                                    //add unit price
                                    model.Price = theRack.RackProducts
                                        .Where(p => p.DefaultQuantity > 0)
                                        .Sum(p => OrderHelper.GetTotalPrice(p.UnitPrice, rackDiscount, p.DefaultQuantity));

                                    model.UnitCount = theRack.RackProducts
                                        .Where(x => x.DefaultQuantity > 0 && !string.IsNullOrEmpty(x.Product.ProductCode))
                                        .Sum(x => x.DefaultQuantity * x.UnitPerCase);
                                }
                                else
                                {
                                    var products = theRack.RackRacks
                                        .Where(r => r.DefaultQuantity > 0)
                                        .Select(r => new
                                        {
                                            Price = r.ProductRack.RackProducts
                                                        .Where(x => x.DefaultQuantity > 0)
                                                        .Sum(x => OrderHelper.GetTotalPrice(x.UnitPrice, rackDiscount, r.DefaultQuantity * x.DefaultQuantity)),
                                            UnitCount = r.ProductRack.RackProducts
                                                        .Where(x => x.DefaultQuantity > 0)
                                                        .Sum(x => r.DefaultQuantity * x.DefaultQuantity * x.UnitPerCase)

                                        });

                                    model.Price = products.Sum(x => x.Price);
                                    model.UnitCount = products.Sum(x => x.UnitCount);
                                }

                                foreach (var shipdate in shipDates)
                                {
                                    RackDate rackDate = new RackDate()
                                    {
                                        ShipDate = shipdate,
                                        IsAvailable = false,
                                        SoldOut = false,
                                        RackID = rack.RackID,
                                        CatalogID = catalog.CatalogId,
                                        ProgramID = catalog.ProgramID,

                                    };
                                    var prsd = rack.ProductRackShipDates.FirstOrDefault(sd => sd.ShipDate == shipdate && sd.Active);

                                    if (prsd != null)
                                    {
                                        var catalogShipDate = prsd.ProductCatalogRack.ProductCatalog.ProductCatalogShipDates.FirstOrDefault(x => x.ShipDate == prsd.ShipDate);
                                        if (catalogShipDate != null)
                                        {
                                            rackDate.IsAvailable = DateTime.Now < catalogShipDate.OrderDeadlineDate;
                                            rackDate.SoldOut = prsd.Available > 0;

                                            model.ShipDates.Add(rackDate);
                                        }
                                    }
                                }
                                catalogRacks.Add(model);
                            }
                        }
                        lvRacks.DataSource = catalogRacks.Where(r => !r.HasNoShipDates);
                        lvRacks.DataBind();


                        List<Customer> p_SubsTable = new List<Customer>();
                        p_SubsTable = theCustomer.GetSubCustomers().OrderBy(c => c.Company).ToList();
                        p_SubsTable.Insert(0, new Customer
                        {
                            CustomerId = theCustomer.CustomerId,
                            Company = theCustomer.Company
                        });
                        rptStores.DataSource = p_SubsTable;
                        rptStores.DataBind();

                        btnPlaceOrder.Enabled = p_SubsTable.Count == 1;

                        NumberOfStores = p_SubsTable.Count;
                    }
                }
            }
            catch (Exception ex)
            {
                //litMessage.Text = GenerateError(ex.Message);
            }
        }

        private string GenerateError(string error)
        {
            return $@"<div class='alert alert-danger d-flex justify-content-between align-items-center'>                         
                        <span>{error}</span>
                        <a class='close' data-bs-dismiss='alert' aria-label='close'>
                                <i class='far fa-times-circle' aria-hidden='true'></i>
                            </a></div>";
        }
        private string GenerateSuccess(string message)
        {
            return $@"<div class='alert alert-success d-flex justify-content-between align-items-center'>                        
                        <span>{message}</span>
                        <a class='close' data-bs-dismiss='alert' aria-label='Close'> 
                        <i class='far fa-times-circle'></i>
                        </a></div>";
        }
        private List<DateTime> GetShipDates(List<ProductCatalog> productCatalogs)
        {
            List<DateTime> dates = new List<DateTime>();
            var shipDates = productCatalogs.Select(pc => pc.ProductCatalogShipDates);
            foreach (var shipdate in shipDates)
            {
                var tempDAtes = shipdate.Where(x => x.OrderDeadlineDate.HasValue && x.OrderDeadlineDate >= DateTime.Now.Date).Select(d => d.ShipDate);
                dates.AddRange(tempDAtes);
            }
            return dates.Distinct().OrderBy(d => d).ToList();
        }
        protected void btnPlaceOrder_Click(object sender, EventArgs e)
        {
            using (MadduxEntities db = new MadduxEntities())
            {
                Customer theCustomer = AppSession.Current.CurrentCustomer;

                //setup common order details
                bool error = false;
                string errorMessage = "";
                string OrderPlacedBy = theCustomer.FirstName + " " + theCustomer.LastName; ;
                List<int> customerIDs = new List<int>();

                foreach (RepeaterItem _store in rptStores.Items)
                {
                    CheckBox chkStoreSelected = _store.FindControl("chkStoreSelected") as CheckBox;
                    HiddenField hdnCompanyId = _store.FindControl("hdnCompanyId") as HiddenField;
                    if (chkStoreSelected.Checked)
                    {
                        customerIDs.Add(int.Parse(hdnCompanyId.Value));
                    }
                }

                //only continue IF they have actually selected any stores
                if (customerIDs.Count == 0)
                {
                    error = true;
                    errorMessage += "No Store is selected for order! <br />";
                }
                else
                {
                    List<Customer> customers = db.Customers.Where(x => customerIDs.Contains(x.CustomerId)).ToList();
                    int ShippingMethodID = db.supShippingMethods.Where(x => x.ShippingMethodDesc.StartsWith("LTL")).Select(x => x.ShippingMethodID).FirstOrDefault();
                    var _state = db.States.FirstOrDefault(x => x.StateID == theCustomer.State);
                    if (string.IsNullOrWhiteSpace(theCustomer.Zip) || string.IsNullOrWhiteSpace(theCustomer.Company) || string.IsNullOrWhiteSpace(theCustomer.Address) || string.IsNullOrWhiteSpace(theCustomer.State) || string.IsNullOrWhiteSpace(theCustomer.Country) || string.IsNullOrWhiteSpace(theCustomer.City))
                    {
                        error = true;
                        errorMessage += $@"Shipping information is required for {theCustomer.Company}!";
                        //throw new Exception($@"Shipping information is required for {theCustomer.Company}!");
                    }
                    if (string.IsNullOrWhiteSpace(theCustomer.BillingZip) || string.IsNullOrWhiteSpace(theCustomer.BillingCompany) || string.IsNullOrWhiteSpace(theCustomer.BillingAddress) || string.IsNullOrWhiteSpace(theCustomer.BillingState) || string.IsNullOrWhiteSpace(theCustomer.BillingCountry) || string.IsNullOrWhiteSpace(theCustomer.BillingCity))
                    {
                        error = true;
                        errorMessage += $@"Billing information is required for {theCustomer.Company}!";
                        //throw new Exception($@"Billing information is required for {theCustomer.Company}!");
                    }
                    if (!error)
                    {
                        int tempID = 0;
                        //get loop through all the racks and get the selected dates for each rack
                        for (int rowIndex = 0; rowIndex < lvRacks.Items.Count; rowIndex++)
                        {
                            var row = lvRacks.Items[rowIndex];
                            int rackID = (int)lvRacks.DataKeys[rowIndex].Value;
                            Repeater repShipDates = (Repeater)lvRacks.Items[rowIndex].FindControl("repShipDates");
                            CatalogRackModel catalogRack = (CatalogRackModel)lvRacks.Items[rowIndex].DataItem;
                            List<ShipdateQuantity> selectedShipDates = new List<ShipdateQuantity>();


                            ProductCatalogRack productCatalogRack = db.ProductCatalogRacks.FirstOrDefault(x => x.RackID == rackID);

                            foreach (RepeaterItem item in repShipDates.Items)
                            {
                                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                                {
                                    var chkBxShipDate = (CheckBox)item.FindControl("chkBxShipDate");
                                    var txtRackQuantity = (TextBox)item.FindControl("txtRackQuantity");
                                    var hfShipDate = (HiddenField)item.FindControl("hfShipDate");
                                    if (chkBxShipDate.Checked && !string.IsNullOrEmpty(txtRackQuantity.Text.Trim()))
                                    {
                                        if (int.TryParse(txtRackQuantity.Text.Trim(), out int quantity))
                                        {
                                            selectedShipDates.Add(new ShipdateQuantity
                                            {
                                                Quantity = quantity,
                                                ShipDate = DateTime.Parse(hfShipDate.Value)
                                            });
                                        }
                                    }
                                }
                            }

                            //We have all selected shipdates for the rack in selectedShipDates
                            //We know the rack in variable catalogRack
                            //We have all the customers/stores in customers
                            //We have all the order items in orderItems

                            //only continue IF they have actually selected any shipdate for this rack
                            if (selectedShipDates.Any())
                            {
                                foreach (int customerID in customerIDs)
                                {
                                    var customer = customers.FirstOrDefault(x => x.CustomerId == customerID);
                                    foreach (ShipdateQuantity selectedShipDate in selectedShipDates)
                                    {
                                        for (int x = 0; x < selectedShipDate.Quantity; x++)
                                        {
                                            if (productCatalogRack.RackType == (int)RackType.Standard)
                                            {
                                                OrderRack orderRack = new OrderRack
                                                {
                                                    Quantity = 1,
                                                    RackId = rackID
                                                };

                                                bool isCustomized = false;
                                                if (productCatalogRack.AllowCustomization)
                                                {
                                                    HiddenField hidIsCustomized = (HiddenField)lvRacks.Items[rowIndex].FindControl("isCustomized");
                                                    isCustomized = hidIsCustomized.Value == "1";
                                                }

                                                if (isCustomized)
                                                {
                                                    Repeater rptCustomize = (Repeater)lvRacks.Items[rowIndex].FindControl("rptProductRacks");
                                                    Repeater rptCustomizeProducts = (Repeater)rptCustomize.Items[0].FindControl("rptProducts");

                                                    foreach (RepeaterItem productItem in rptCustomizeProducts.Items)
                                                    {
                                                        HiddenField hidProductId = (HiddenField)productItem.FindControl("hdnProductID");
                                                        TextBox txtQty = (TextBox)productItem.FindControl("txtCustomProductQuantity");
                                                        int productId = int.Parse(hidProductId.Value);
                                                        int qty = int.Parse(txtQty.Text);

                                                        RackProduct rackProduct = productCatalogRack.RackProducts.Where(rp => rp.ProductID == productId).FirstOrDefault();
                                                        if (rackProduct != null && qty > 0)
                                                        {
                                                            orderRack.OrderItems.Add(new OrderItem
                                                            {
                                                                OrderId = tempID,
                                                                Quantity = qty,
                                                                ProductId = rackProduct.ProductID,
                                                                DiscountPercent = 0,
                                                                LinePosition = 0,
                                                                NoGlobalDiscount = true,
                                                                ProductNotAvailable = false,
                                                                ProductNotAvailableDesc = "",
                                                                UnitPrice = rackProduct.UnitPrice
                                                            });
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    foreach (RackProduct rackProduct in productCatalogRack.RackProducts)
                                                    {
                                                        if (rackProduct.DefaultQuantity > 0)
                                                        {
                                                            orderRack.OrderItems.Add(new OrderItem
                                                            {
                                                                OrderId = tempID,
                                                                Quantity = rackProduct.DefaultQuantity,
                                                                ProductId = rackProduct.ProductID,
                                                                DiscountPercent = productCatalogRack.Discount.HasValue ? productCatalogRack.Discount.Value : 0,
                                                                LinePosition = 0,
                                                                NoGlobalDiscount = true,
                                                                ProductNotAvailable = false,
                                                                ProductNotAvailableDesc = "",
                                                                UnitPrice = rackProduct.UnitPrice
                                                            });
                                                        }
                                                    }

                                                }
                                                double freightCharge = FreightCalculator.CalculateFreighCharge(orderRack.GetRackTotal, _state.StateID, theCustomer.Zip);
                                                Order order = new Order
                                                {
                                                    OrderID = tempID,
                                                    OrderStatus = 0,
                                                    CustomerID = customer.CustomerId,
                                                    RequestedShipDate = selectedShipDate.ShipDate,
                                                    BillingAddress = customer.BillingAddress,
                                                    BillingCity = customer.BillingCity,
                                                    BillingCountry = customer.BillingCountry,
                                                    BillingName = customer.BillingCompany,
                                                    BillingState = customer.BillingState,
                                                    BillingZip = customer.BillingZip,
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
                                                    GSTAmount = Convert.ToDecimal(TaxUtilities.GetTaxPercentage(_state.StateID, null) * (orderRack.GetRackTotal + freightCharge)),
                                                    HST = _state.HST,
                                                    OfficeNotes = null,
                                                    OrderNotes = null,
                                                    //PONumber = OrderPONumber,
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
                                                db.Orders.Add(order);
                                                order.OrderRacks.Add(orderRack);
                                                var subTotal = order.SubTotal;
                                                tempID++;
                                            }
                                            else
                                            {
                                                var bulkOrderKey = Guid.NewGuid();
                                                foreach (var productRack in productCatalogRack.RackRacks)
                                                {
                                                    bool isCustomized = false;
                                                    if (productCatalogRack.AllowCustomization)
                                                    {
                                                        HiddenField hidIsCustomized = (HiddenField)lvRacks.Items[rowIndex].FindControl("isCustomized");
                                                        isCustomized = hidIsCustomized.Value == "1";
                                                    }

                                                    var quantityOrdered = productRack.DefaultQuantity;
                                                    if (isCustomized)
                                                    {
                                                        Repeater rptCustomize = (Repeater)lvRacks.Items[rowIndex].FindControl("rptProductRacks");

                                                        foreach (RepeaterItem item in rptCustomize.Items)
                                                        {
                                                            var rackId = (item.FindControl("hdnProductRackID") as HiddenField).Value;

                                                            if (rackId == productRack.ProductRackID.ToString())
                                                            {
                                                                quantityOrdered = Int32.Parse((item.FindControl("txtCustomRackQuantity") as TextBox).Text);
                                                            }
                                                        }

                                                    }

                                                    // We need to create a seperate order for each rack within the bulk rack. 
                                                    // For example if they order a bulk rack that contains 3 rack A's and 2 rack B's. 
                                                    // We need 3 orders of rack A and 2 orders of rack B. 
                                                    for (var j = 1; j <= quantityOrdered; j++)
                                                    {
                                                        OrderRack orderRack = new OrderRack
                                                        {
                                                            Quantity = 1,
                                                            RackId = productRack.ProductRackID
                                                        };

                                                        foreach (var product in productRack.ProductRack.RackProducts.Where(p => p.DefaultQuantity > 0))
                                                        {
                                                            orderRack.OrderItems.Add(new OrderItem
                                                            {
                                                                OrderId = tempID,
                                                                Quantity = product.DefaultQuantity,
                                                                ProductId = product.ProductID,
                                                                DiscountPercent = productCatalogRack.Discount.HasValue ? productCatalogRack.Discount.Value : 0,
                                                                LinePosition = 0,
                                                                NoGlobalDiscount = true,
                                                                ProductNotAvailable = false,
                                                                ProductNotAvailableDesc = "",
                                                                UnitPrice = product.UnitPrice,
                                                            });
                                                        }

                                                        double freightCharge = FreightCalculator.CalculateFreighCharge(orderRack.GetRackTotal, _state.StateID, theCustomer.Zip);
                                                        Order order = new Order
                                                        {
                                                            OrderID = tempID,
                                                            OrderStatus = 0,
                                                            CustomerID = customer.CustomerId,
                                                            RequestedShipDate = selectedShipDate.ShipDate,
                                                            BillingAddress = customer.BillingAddress,
                                                            BillingCity = customer.BillingCity,
                                                            BillingCountry = customer.BillingCountry,
                                                            BillingName = customer.BillingCompany,
                                                            BillingState = customer.BillingState,
                                                            BillingZip = customer.BillingZip,
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
                                                            GSTAmount = Convert.ToDecimal(TaxUtilities.GetTaxPercentage(_state.StateID, null) * (orderRack.GetRackTotal + freightCharge)),
                                                            HST = _state.HST,
                                                            OfficeNotes = null,
                                                            OrderNotes = null,
                                                            //PONumber = OrderPONumber,
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
                                                            ReceivedVia = 1,
                                                            BulkOrderKey = bulkOrderKey,
                                                            BulkRackID = productCatalogRack.RackID
                                                        };
                                                        db.Orders.Add(order);
                                                        order.OrderRacks.Add(orderRack);
                                                        var subTotal = order.SubTotal;
                                                        tempID++;
                                                    }
                                                }
                                            }

                                        }
                                    }
                                }
                                db.SaveChanges();
                            }
                        }
                    }
                }

                if (error == false)
                {
                    litError.InnerHtml = StringTools.GenerateSuccess("Your order(s) has been added to your cart.  Please visit your cart to complete the checkout process, or continue shopping to add additional items to your cart.");
                    if (!error)
                    {
                        Response.Redirect("/cart.aspx?oSuccess=true", false);
                    }
                }
                else
                {
                    litError.InnerHtml = StringTools.GenerateError(errorMessage);
                }
            }
        }

        protected void lvRacks_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                using (MadduxEntities db = new MadduxEntities())
                {
                    if (e.Item.ItemType == ListViewItemType.DataItem)
                    {
                        Repeater rptProductRacks = e.Item.FindControl("rptProductRacks") as Repeater;
                        Button CustomizeBtn = e.Item.FindControl("CustomizeBtn") as Button;
                        Panel pnlCustomize = e.Item.FindControl("pnlCustomize") as Panel;

                        var itemRow = (CatalogRackModel)e.Item.DataItem;

                        if (itemRow.AllowCustomization)
                        {
                            Customer theCustomer = AppSession.Current.CurrentCustomer;
                            ProductCatalogRack theRack = db.ProductCatalogRacks.Include(x => x.RackProducts.Select(z => z.Product.Photos)).FirstOrDefault(r => r.RackID == itemRow.RackID);

                            CustomizeBtn.Visible = true;
                            CustomizeBtn.CssClass = $@"btn btn-customize w-100 customize-btn C{theRack.RackID} ms-300 fs-15 my-2 btn-dark";

                            List<RackProductRackResponse> customizeData = new List<RackProductRackResponse>();

                            if (theRack.RackType == (int)RackType.Standard)
                            {
                                //standard rack
                                customizeData.Add(new RackProductRackResponse
                                {
                                    RackId = theRack.RackID,
                                    RackName = theRack.RackName,
                                    DefaultQuantity = 1,
                                    MinQuantity = theRack.MinimumItems,
                                    MaxQuantity = theRack.MaximumItems,
                                    Discount = theRack.Discount.HasValue ? theRack.Discount.Value : 0,
                                    RackType = theRack.RackType,
                                    Products = theRack.RackProducts.Select(rp => new RackProductResponse
                                    {
                                        ProductID = rp.ProductID,
                                        RackID = theRack.RackID,
                                        ParentRackType = theRack.RackType,
                                        DefaultQuantity = rp.DefaultQuantity,
                                        ItemNumber = rp.Product.ItemNumber,
                                        ProductName = rp.ProductName,
                                        Size = rp.Product.Size,
                                        ItemsPerPackage = rp.Product.ItemsPerPackage,
                                        PackagesPerUnit = rp.Product.PackagesPerUnit,
                                        UnitPrice = rp.UnitPrice,
                                        PhotoCount = rp.Product.Photos.Count,
                                        RackDiscount = theRack.Discount.HasValue ? theRack.Discount.Value : 0,
                                        ItemPhoto = rp.Product.Photos.Select(x => x.PhotoPath).FirstOrDefault(),
                                        Photos = rp.Product.Photos.Select(p => new RackProductPhotoResponse
                                        {
                                            PhotoId = p.PhotoID,
                                            PhotoPath = p.PhotoPath,
                                            ProductId = rp.ProductID,
                                            ProductName = rp.ProductName
                                        }).Skip(1).ToList()
                                    }).OrderBy(rp => rp.Size).ToList(),
                                });
                            }
                            else
                            {
                                customizeData = theRack.AllProductRacks(false);
                            }
                            rptProductRacks.DataSource = customizeData;
                            rptProductRacks.DataBind();
                        }
                        else
                        {
                            pnlCustomize.Visible = false;
                            CustomizeBtn.Visible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        public string CalculateRackQty(int caseCount, double defaultQty)
        {
            return ((int)Math.Round(caseCount * defaultQty)).ToString();
        }
    }
}
