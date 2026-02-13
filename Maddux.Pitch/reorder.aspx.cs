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
    public partial class reorder : System.Web.UI.Page
    {

        private Dictionary<int, List<ProductCatalogShipDate>> ProductCatalogShipDates;
        private int OrderID
        {
            get
            {
                if (ViewState["id"] == null)
                {
                    if (Request.QueryString["id"] == null || Request.QueryString["id"] == "")
                        ViewState["id"] = -1;
                    else
                        ViewState["id"] = Request.QueryString["id"];
                }
                return Convert.ToInt32(ViewState["id"].ToString());
            }

            set
            {
                ViewState["id"] = value;
            }
        }
        protected void rptShipDates_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                int rackID = ((KeyValuePair<int, List<ProductCatalogShipDate>>)e.Item.DataItem).Key;
                List<ProductCatalogShipDate> shipDates = ((KeyValuePair<int, List<ProductCatalogShipDate>>)e.Item.DataItem).Value;
                CheckBoxList lstShipDateSelector = e.Item.FindControl("lstShipDateSelector") as CheckBoxList;

                using (var db = new MadduxEntities())
                {
                    //Product rack available ship dates
                    var productRackShipDates = db.ProductRackShipDates.Where(prsd => prsd.RackID == rackID && prsd.ShipDate > DateTime.Now && prsd.Active)
                            .Select(prsd => new RackShipDate
                            {
                                ShipDate = prsd.ShipDate,
                                OrderDeadlineDate = prsd.ProductCatalogRack.ProductCatalog.ProductCatalogShipDates.FirstOrDefault(x => x.ShipDate == prsd.ShipDate).OrderDeadlineDate,
                                Available = prsd.Available
                            })
                            .OrderBy(prsd => prsd.ShipDate)
                            .ToList();
                    //.Select(prsd => new { ShipDate = prsd.ShipDate.ToString("MMMM d, yyyy") })
                    List<ShipdateObject> shipadates = new List<ShipdateObject>();
                    foreach (var date in productRackShipDates)
                    {
                        if (date.OrderDeadlineDate > DateTime.Now)
                        {
                            shipadates.Add(new ShipdateObject { ShipDate = date.ShipDate.ToString("MMMM d, yyyy") });
                        }
                        else
                        {
                            if (date.Available > 0)
                            {
                                shipadates.Add(new ShipdateObject { ShipDate = date.ShipDate.ToString("MMMM d, yyyy") });
                            }
                        }
                    }

                    lstShipDateSelector.DataValueField = "ShipDate";
                    lstShipDateSelector.DataTextField = "ShipDate";
                    lstShipDateSelector.DataSource = shipadates;
                    lstShipDateSelector.DataBind();

                    lstShipDateSelector.Attributes.Add("tag", "sd" + rackID.ToString());
                }
            }
            catch (Exception ex)
            {
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            using (var db = new MadduxEntities())
            {
                var order = db.Orders.Include(x => x.OrderRacks).FirstOrDefault(r => r.OrderID == OrderID);
                var program = order.OrderRacks.Select(x => x.ProductCatalogRack.ProductCatalog).FirstOrDefault();

                ProductCatalogShipDates = new Dictionary<int, List<ProductCatalogShipDate>>();

                int rackID = order.OrderRacks.Select(x => x.RackId).Distinct().FirstOrDefault();
                ProductCatalogRack theRack = db.ProductCatalogRacks.FirstOrDefault(r => r.RackID == rackID);
                List<ProductCatalogShipDate> shipDatesTable = theRack.ProductCatalog.FutureShipDates;
                ProductCatalogShipDates.Add(rackID, shipDatesTable);

                Customer theCustomer = AppSession.Current.CurrentCustomer;
                var p_SubsTable = theCustomer.GetSubCustomers().OrderBy(x => x.Company).ToList();

                p_SubsTable.Insert(0, new Customer
                {
                    CustomerId = theCustomer.CustomerId,
                    Company = theCustomer.Company
                });
                if (!Page.IsPostBack)
                {
                    rptStores.DataSource = p_SubsTable;
                    rptStores.DataBind();
                    rptShipDates.DataSource = ProductCatalogShipDates;
                    rptShipDates.DataBind();

                    OrderByField.Text = theCustomer.FirstName + " " + theCustomer.LastName;
                }

            }
        }

        protected void rptStores_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            CheckBox rptRacks = e.Item.FindControl("chkStoreSelected") as CheckBox;
            rptRacks.Checked = true;
        }
        private string GenerateError(string error)
        {
            return $@"<div class='alert alert-danger d-flex justify-content-between align-items-center'>                         
                        <span >{error}</ span >
                        <a class='close' data-dismiss='alert' aria-label='Close'> 
                        <i class='far fa-times-circle'></i>
                        </a > </div >";
        }
        private string GenerateSuccess(string message)
        {
            return $@"<div class='alert alert-success d-flex justify-content-between align-items-center'> 
                        <span >{message}</ span >                        
                        <a class='close' data-dismiss='alert' aria-label='Close'> 
                        <i class='far fa-times-circle'></i>
                        </a > 
                    </div >";
        }

        protected void btnPlaceOrder_Click(object sender, EventArgs e)
        {
            try
            {
                using (var db = new MadduxEntities())
                {

                    var order = db.Orders.Include(x => x.OrderRacks).FirstOrDefault(r => r.OrderID == OrderID);
                    if (order == null)
                        return;
                    bool error = false;
                    string errorMessage = "";
                    string OrderPONumber = PONumber.Text.TrimEnd();
                    string OrderPlacedBy = OrderByField.Text.TrimEnd();
                    Button cmdPlaceOrder = (Button)sender;
                    int rackID = order.OrderRacks.Select(x => x.RackId).Distinct().FirstOrDefault();
                    Customer theCustomer = AppSession.Current.CurrentCustomer;

                    List<DateTime> selectedShipDates = new List<DateTime>();

                    foreach (RepeaterItem shipDate in rptShipDates.Items)
                    {
                        CheckBoxList lstShipDateSelector = shipDate.FindControl("lstShipDateSelector") as CheckBoxList;

                        foreach (ListItem item in lstShipDateSelector.Items)
                        {
                            if (item.Selected)
                            {
                                selectedShipDates.Add(DateTime.Parse(item.Value));
                            }
                        }
                    }
                    errorMessage = string.Empty;
                    if (selectedShipDates.Count == 0)
                    {
                        error = true;
                        errorMessage += "No Ship date is selected for order! <br />";
                    }
                    bool IsStoreSelected = false;
                    foreach (RepeaterItem _store in rptStores.Items)
                    {
                        TextBox txtRackQuantity = _store.FindControl("txtRackQuantity") as TextBox;

                        int rackQuantity = int.Parse(txtRackQuantity.Text);
                        CheckBox selected = _store.FindControl("chkStoreSelected") as CheckBox;
                        if (rackQuantity > 0)
                        {
                            IsStoreSelected = true;
                            Label StoreName = _store.FindControl("StoreName") as Label;
                        }
                    }
                    if (!IsStoreSelected)
                    {
                        error = true;
                        errorMessage += "No Store is selected for order! <br />";
                    }
                    if (!IsStoreSelected && selectedShipDates.Count == 0)
                    {
                        error = true;
                        errorMessage += "No Store and ShipDate is selected for order! <br />";

                    }
                    if (error)
                    {
                        litErrorModal.Text = GenerateError(errorMessage);
                        return;
                    }
                    foreach (RepeaterItem store in rptStores.Items)
                    {
                        CheckBox selected = store.FindControl("chkStoreSelected") as CheckBox;
                        if (selected.Checked)
                        {
                            HiddenField hdnCompanyId = store.FindControl("hdnCompanyId") as HiddenField;
                            TextBox txtRackQuantity = store.FindControl("txtRackQuantity") as TextBox;
                            Label StoreName = store.FindControl("StoreName") as Label;
                            int quantity = int.Parse(txtRackQuantity.Text);
                            if (int.TryParse(hdnCompanyId.Value, out int companyId))
                            {
                                var _state = db.States.FirstOrDefault(x => x.StateID == theCustomer.State);
                                foreach (var shipDate in selectedShipDates)
                                {
                                    for (int i = 0; i < quantity; i++)
                                    {
                                        List<OrderRack> racks = new List<OrderRack>();
                                        OrderRack orderRack = new OrderRack
                                        {
                                            Quantity = 1,
                                            RackId = rackID
                                        };
                                        orderRack.OrderItems = order.OrderItems.Select(r => new OrderItem
                                        {
                                            Quantity = r.Quantity,
                                            DiscountPercent = 0,
                                            NoGlobalDiscount = true,
                                            LinePosition = r.LinePosition,
                                            ProductId = r.ProductId,
                                            ProductNotAvailable = r.ProductNotAvailable,
                                            ProductNotAvailableDesc = r.ProductNotAvailableDesc,
                                            UnitPrice = r.GetLatestUnitPrize
                                        }).ToList();
                                        double freightCharge = FreightCalculator.CalculateFreighCharge(orderRack.GetRackTotal, _state.StateID, theCustomer.Zip);

                                        var newOrder = new Order
                                        {
                                            BillingAddress = order.BillingAddress,
                                            BillingCity = order.BillingCity,
                                            ShippingCharge = Convert.ToDecimal(freightCharge),
                                            BillingCountry = order.BillingCountry,
                                            supPaymentTerm = order.supPaymentTerm,
                                            OfficeNotes = order.OfficeNotes,
                                            BillingName = order.BillingName,
                                            OrderDate = DateTime.Now,
                                            BillingState = order.BillingState,
                                            OrderNotes = order.OrderNotes,
                                            BillingZip = order.BillingZip,
                                            CreatedBy = OrderPlacedBy,
                                            CustomerID = order.CustomerID,
                                            DateCreated = DateTime.Now,
                                            DateUpdated = DateTime.Now,
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
                                            GSTAmount = Convert.ToDecimal(TaxUtilities.GetTaxPercentage(_state.StateID, DateTime.Now) * (orderRack.GetRackTotal + freightCharge)),
                                            HST = _state.HST,
                                            OrderStatus = 0,
                                            PaymentTermsID = order.PaymentTermsID,
                                            PONumber = string.IsNullOrWhiteSpace(OrderPONumber) ? "" : OrderPONumber,
                                            PSTAmount = 0,
                                            PSTExempt = false,
                                            RequestedShipDate = shipDate,
                                            SalesPersonID = order.SalesPersonID,
                                            ShippingAddress = order.ShippingAddress,
                                            ShippingCity = order.ShippingCity,
                                            ShippingCountry = order.ShippingCountry,
                                            ShippingMethodID = order.ShippingMethodID,
                                            ShippingEmail = order.ShippingEmail,
                                            PaymentTypeID = order.PaymentTypeID,
                                            ShippingName = order.ShippingName,
                                            ShippingState = order.ShippingState,
                                            ShippingZip = order.ShippingZip,
                                            UpdatedBy = order.UpdatedBy,
                                            PurchaseOrdersSentDate = null,
                                            ReceivedVia = 1

                                        };
                                        newOrder.OrderRacks.Add(orderRack);
                                        db.Orders.Add(newOrder);
                                        db.SaveChanges();
                                    }
                                }
                            }
                        }

                    }
                    Response.Redirect("cart.aspx?oSuccess=true");
                }

            }
            catch (Exception ex)
            {
                litErrorModal.Text = GenerateError(ex.Message);
            }





        }
    }
}