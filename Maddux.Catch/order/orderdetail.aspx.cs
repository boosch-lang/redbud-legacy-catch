using Maddux.Catch.LocalClasses;
using Newtonsoft.Json;
using Redbud.BL.DL;
using Redbud.BL.Helpers;
using Redbud.BL.Resources;
using Redbud.BL.Utils;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Maddux.Catch.order
{
    public partial class orderdetail : Page
    {

        public string pID
        {
            get
            {
                return ddlPrograms.SelectedValue;
            }

        }

        private int OrderID
        {
            get
            {
                if (ViewState["OrderID"] == null)
                {
                    ViewState["OrderID"] = Request.QueryString["id"] == null || Request.QueryString["id"] == "" ? 0 : (object)Request.QueryString["id"];
                }
                return Convert.ToInt32(ViewState["OrderID"].ToString());
            }

            set
            {
                ViewState["OrderID"] = value;
            }
        }

        protected int CustomerID
        {
            get
            {
                if (ViewState["CustomerId"] == null)
                {
                    ViewState["CustomerId"] = Request.QueryString["CustomerId"] == null || Request.QueryString["CustomerId"] == "" ? 0 : (object)Request.QueryString["CustomerId"];
                }
                return Convert.ToInt32(ViewState["CustomerId"].ToString());
            }

            set
            {
                ViewState["CustomerId"] = value;
            }
        }

        private User currentUser;

        private void PopulateDropdowns()
        {
            try
            {
                using (var db = new MadduxEntities())
                {
                    var countries = db.Countries.Select(r => new ListItem
                    {
                        Text = r.CountryName,
                        Value = r.CountryCode
                    }).ToList();

                    var purchaseOrders = db.PurchaseOrders.OrderByDescending(x => x.PickupDate).Select(po => new ListItem
                    {
                        Text = po.Name,
                        Value = po.PurchaseOrderID.ToString()
                    }).ToList();
                    purchaseOrders.Insert(0, new ListItem("None", ""));

                    var states = db.States.Where(x => x.Country.ToLower() == "canada").Select(r => new ListItem
                    {
                        Text = r.StateName,
                        Value = r.StateID
                    }).ToList();

                    var terms = db.supPaymentTerms.Select(r => new ListItem
                    {
                        Text = r.PaymentTermsDesc,
                        Value = r.PaymentTermsId.ToString()
                    }).ToList();

                    var types = db.supPaymentTypes.Select(r => new ListItem
                    {
                        Text = r.PaymentTypeDesc,
                        Value = r.PaymentTypeID.ToString()
                    }).ToList();

                    var salesReps = db.Users.Select(r => new ListItem
                    {
                        Text = r.FirstName + " " + r.LastName,
                        Value = r.UserID.ToString()
                    }).ToList();

                    var shippingMethods = db.supShippingMethods.Select(r => new ListItem
                    {
                        Value = r.ShippingMethodID.ToString(),
                        Text = r.ShippingMethodDesc
                    }).ToList();

                    var orderStatuses = new List<ListItem>
                {
                    new ListItem
                    {
                        Text = "Quote",
                        Value = "-1"
                    },
                    new ListItem
                    {
                        Text = "Draft",
                        Value="0"
                    },
                    new ListItem
                    {
                        Text = "Order",
                        Value = "1"
                    }
                };

                    var programs = db.ProductPrograms.Where(r => r.Active).OrderBy(r => r.ProgramName).Select(r => new ListItem
                    {
                        Text = r.ProgramName,
                        Value = r.ProgramID.ToString()
                    }).ToList();

                    string[] orderApprovedOptions = Enum.GetNames(typeof(Redbud.BL.OrderApproved));

                    for (int i = 0; i <= orderApprovedOptions.Length - 1; i++)
                    {
                        ListItem item = new ListItem(orderApprovedOptions[i]);
                        ddlOrderApproved.Items.Add(item);
                    }

                    ddOrderStatus.DataSource = orderStatuses;
                    ddOrderStatus.DataBind();

                    ddBillingCountry.DataSource = countries;
                    ddBillingCountry.DataBind();

                    ddShippingProvince.DataSource = states;
                    ddShippingProvince.DataBind();

                    ddCountry.DataSource = countries;
                    ddCountry.DataBind();

                    ddBillingProvince.DataSource = states;
                    ddBillingProvince.DataBind();

                    ddDefaultTerms.DataSource = terms;
                    ddDefaultTerms.DataBind();

                    ddDefaultPaymentType.DataSource = types;
                    ddDefaultPaymentType.DataBind();

                    ddSalesRep.DataSource = salesReps;
                    ddSalesRep.DataBind();

                    ddShippingMethod.DataSource = shippingMethods;
                    ddShippingMethod.DataBind();

                    ddlPrograms.DataSource = programs;
                    ddlPrograms.DataBind();

                    ddlProvince.DataSource = states;
                    ddlProvince.DataBind();

                    ddlPurchaseOrders.DataSource = purchaseOrders;
                    ddlPurchaseOrders.DataBind();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        private void PopulateCatalogDropdown()
        {
            try
            {
                int selectedProgram = int.Parse(ddlPrograms.SelectedValue);
                using (var db = new MadduxEntities())
                {
                    var associationIds = db.CustomerAsscs.Where(r => r.CustomerID == CustomerID).Select(c => c.AssociationID);
                    var catalogIds = db.AssociationCatalogs.Where(r => associationIds.Contains(r.AssociationID)).Select(r => r.CatalogID);
                    var qry = db.ProductCatalogs.Where(pc => catalogIds.Contains(pc.CatalogId) && pc.Active && pc.ProgramID == selectedProgram).Select(c => c);

                    if (currentUser.CanOnlyViewAssignedCatalogs)
                    {
                        qry = qry.Where(c => db.UserCatalogs.Where(uc => uc.UserID == currentUser.UserID).Select(uc => uc.CatalogID).Contains(c.CatalogId));
                    }

                    ddCatalogList.DataSource = qry.ToList();
                    ddCatalogList.DataTextField = "CatalogName";
                    ddCatalogList.DataValueField = "CatalogId";
                    ddCatalogList.DataBind();
                    ddCatalogList.Items.Insert(0, new ListItem { Text = "Select a catalog", Value = "0" });
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private void PopulateShipDateDropdown()
        {
            try
            {
                DateTime beginningLastYear = new DateTime((DateTime.Today.Year), 1, 1);
                int selectedProgram = int.Parse(ddlPrograms.SelectedValue);
                using (var db = new MadduxEntities())
                {
                    ddlShipDate.DataSource = db.ProductPrograms
                        .Where(s => s.ProgramID == selectedProgram)
                            .SelectMany(x => x.ProductCatalogs)
                                .SelectMany(x => x.ProductCatalogShipDates)
                        .AsEnumerable()
                        .OrderByDescending(s => s.ShipDate)
                        .ToList()
                        .Where(x => x.ShipDate.Date >= beginningLastYear)
                        .Select(r => new ListItem
                        {
                            Text = r.ShipDate.ToString("MMMM dd, yyyy"),
                            Value = r.ShipDate.ToString("MMMM dd, yyyy")
                        })
                        .Distinct()
                        .ToList();

                    ddlShipDate.DataTextField = "Text";
                    ddlShipDate.DataValueField = "Value";
                    ddlShipDate.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            currentUser = AppSession.Current.CurrentUser;
            if (!IsPostBack)
            {
                checkForMessage();
                if (Request.QueryString["tab"] != null)
                {
                    CurrentTab.Value = Request.QueryString["tab"].ToString();
                }
                if (Request.QueryString["oSuccess"] != null)
                {
                    litMessage.Text = StringTools.GenerateSuccess("The new order has been created successfully!");
                }
                try
                {

                    using (var db = new MadduxEntities())
                    {

                        Literal litPageHeader = (Literal)Master.FindControl("litPageHeader");

                        string message = "Select order received via option"; //Select message for dropdowns
                        //Populates drop down list OrderReceivedVia
                        PopulateDropDownFromEnum(ddlOrderReceivedVia, Enum.GetNames(typeof(Redbud.BL.OrderReceivedVia)), message, "0");
                        //new Order
                        if (OrderID == 0 && CustomerID != 0)
                        {
                            PopulateDropdowns();
                            PopulateShipDateDropdown();
                            this.Title = "Maddux | New Order";
                            Customer customer = db.Customers.FirstOrDefault(c => c.CustomerId == CustomerID);

                            litPageHeader.Text = "New Order";

                            if (customer == null)
                            {
                                throw new Exception("No Customer found");
                            }


                            //Shipping Details
                            txtShippingCompany.Text = customer.Company;
                            txtShippingAddress.Text = customer.Address;
                            txtShippingCity.Text = customer.City;
                            ddShippingProvince.SelectedValue = customer.State;
                            ddCountry.SelectedValue = customer.Country;
                            txtShippingPostal.Text = customer.Zip;
                            txtShippingEmail.Text = customer.Email;

                            //Billing Details
                            txtBillingCompany.Text = customer.BillingCompany;
                            txtBillingAddress.Text = customer.BillingAddress;
                            txtBillingCity.Text = customer.BillingCity;
                            ddBillingProvince.SelectedValue = customer.BillingState;
                            ddBillingCountry.SelectedValue = customer.BillingCountry;
                            txtBillingPostal.Text = customer.BillingZip;

                            ddDefaultTerms.SelectedValue = customer.DefaultTermsId.ToString();
                            ddDefaultPaymentType.SelectedValue = customer.DefaultPaymentTypeId.ToString();
                            ddShippingMethod.SelectedValue = customer.DefaultShippingMethodID.ToString();

                            ddSalesRep.SelectedValue = currentUser.UserID.ToString();
                            LoadItems(null);
                            SetButtonVisiblity(null);
                        }
                        else
                        {
                            Order order = db.Orders.Find(OrderID);

                            order.CalculateFreightAndTaxes();
                            db.SaveChanges();


                            if (order == null)
                            {
                                throw new Exception("No order found");
                            }
                            Customer customer = db.Customers.FirstOrDefault(c => c.CustomerId == order.CustomerID);

                            this.Title = "Maddux | Order";
                            litPageHeader.Text = string.Format("{1} #{0} -- {2} {4}<span style='float:right;'>{3}</span>", order.OrderID, getOrderStatus(order.OrderStatus), order.Customer.Company, order.Customer.StarRatingGraphic, GetBulkRackHeading(order));

                            if (CustomerID == 0)
                            {
                                CustomerID = order.CustomerID;
                            }
                            PopulateDropdowns();
                            ddlPrograms.SelectedValue = order.OrderItems.Select(r => r.Product.ProductCatalog.ProgramID).FirstOrDefault().ToString();

                            ddlPrograms.Enabled = false;
                            PopulateShipDateDropdown();
                            PopulateCatalogDropdown();
                            if (order.ReceivedVia != null)
                            {
                                ddlOrderReceivedVia.SelectedValue = order.ReceivedVia.ToString();
                            }


                            // Shipping Details
                            txtShippingCompany.Text = order.ShippingName;
                            txtShippingAddress.Text = order.ShippingAddress;
                            txtShippingCity.Text = order.ShippingCity;
                            ddShippingProvince.SelectedValue = order.ShippingState;
                            ddCountry.SelectedValue = order.ShippingCountry;
                            txtShippingPostal.Text = order.ShippingZip;
                            txtShippingEmail.Text = order.ShippingEmail;

                            //Billing Details
                            txtBillingCompany.Text = order.BillingName;
                            txtBillingAddress.Text = order.BillingAddress;
                            txtBillingCity.Text = order.BillingCity;
                            ddBillingProvince.SelectedValue = order.BillingState;
                            ddBillingCountry.SelectedValue = order.BillingCountry;
                            txtBillingPostal.Text = order.BillingZip;

                            ddDefaultTerms.SelectedValue = order.PaymentTermsID.ToString();
                            ddDefaultPaymentType.SelectedValue = order.PaymentTypeID.ToString();
                            txtPONumber.Text = order.PONumber;
                            txtOrderPlacedBy.Text = order.CreatedBy;
                            ddSalesRep.SelectedValue = order.SalesPersonID.ToString();
                            ddShippingMethod.SelectedValue = order.ShippingMethodID.ToString();
                            ddlOrderApproved.SelectedValue = order.Approved ? "Yes" : "No";
                            ddOrderStatus.SelectedValue = order.OrderStatus.ToString();
                            if (!ddlShipDate.Items.Contains(new ListItem(order.RequestedShipDate.Value.ToString("MMMM dd, yyyy"), order.RequestedShipDate.Value.ToString("MMMM dd, yyyy"))))
                            {
                                ddlShipDate.Items.Add(new ListItem(order.RequestedShipDate.Value.ToString("MMMM dd, yyyy"), order.RequestedShipDate.Value.ToString("MMMM dd, yyyy")));
                            }
                            ddlShipDate.SelectedValue = order.RequestedShipDate.HasValue ? order.RequestedShipDate.Value.ToString("MMMM dd, yyyy") : "";
                            txtPOSentDate.Text = order.PurchaseOrdersSentDate.HasValue ? order.PurchaseOrdersSentDate.Value.ToString("MMMM dd, yyyy") : "";
                            txtConfirmationSent.Text = order.ConfirmationSentDate.HasValue ? order.ConfirmationSentDate.Value.ToString("MMMM dd, yyyy") : "";
                            txtOrderDate.Text = order.OrderDate.HasValue ? order.OrderDate.Value.ToString("MMMM dd, yyyy") : "";

                            txtNotes.Text = order.OrderNotes;
                            txtOfficeNotes.Text = order.OfficeNotes;

                            txtDiscount1Desc.Text = order.GlobalDiscountDesc;
                            txtDiscount2Desc.Text = order.GlobalDiscount2Desc;
                            txtDiscount3Desc.Text = order.GlobalDiscount3Desc;
                            txtDiscount4Desc.Text = order.GlobalDiscount4Desc;
                            txtDiscount5Desc.Text = order.GlobalDiscount5Desc;

                            txtGlobalDiscount1Pct.Text = order.GlobalDiscountPercent.ToString("P", CultureInfo.InvariantCulture);
                            txtDiscount2Pct.Text = order.GlobalDiscount2Percent.ToString("P", CultureInfo.InvariantCulture);
                            txtDiscount3Pct.Text = order.GlobalDiscount3Percent.ToString("P", CultureInfo.InvariantCulture);
                            txtDiscount4Pct.Text = order.GlobalDiscount4Percent.ToString("P", CultureInfo.InvariantCulture);
                            txtDiscount5Pct.Text = order.GlobalDiscount5Percent.ToString("P", CultureInfo.InvariantCulture);

                            var shipments = db.Shipments.Where(r => r.ShipmentItems.Any(q => q.OrderItem.OrderId == OrderID)).ToList();
                            LoadItems(order);
                            SetButtonVisiblity(order);
                            ShowTotals(order);

                            dgvShipments.DataSource = shipments;
                            dgvShipments.DataBind();

                            pnlPO.Visible = order.PurchaseOrderID.HasValue;

                            if (order.PurchaseOrderID.HasValue)
                            {
                                if (ddlPurchaseOrders.Items.FindByValue(order.PurchaseOrderID.Value.ToString()) == null)
                                {
                                    ddlPurchaseOrders.Items.Add(new ListItem(order.PurchaseOrder.Name.ToString(), order.PurchaseOrderID.ToString()));
                                }

                                ddlPurchaseOrders.SelectedValue = order.PurchaseOrderID.ToString();
                                litFullRacks.Text = order.PurchaseOrder.FullRackCount.ToString();
                                litHalfRacks.Text = order.PurchaseOrder.HalfRackCount.ToString();
                                litQuarterRacks.Text = order.PurchaseOrder.QuarterRackCount.ToString();
                                litAllocatedSapce.Text = order.PurchaseOrder.TotalFeet.ToString();
                                litPickupDate.Text = order.PurchaseOrder.PickupDate.HasValue ? order.PurchaseOrder.PickupDate.Value.ToString("MMMM dd, yyyy") : string.Empty;
                                litDeliveryHub.Text = order.PurchaseOrder.DeliveryHub.Name;
                                litShipDate.Text = order.PurchaseOrder.ShipDate.HasValue ? order.PurchaseOrder.ShipDate.Value.ToString("MMMM dd, yyyy") : string.Empty;

                                txtBolShipperAddress.Text = order.PurchaseOrder.DeliveryHub.Address;
                                txtBolShipperName.Text = order.PurchaseOrder.DeliveryHub.ShippingName;
                                txtBolShipperCity.Text = order.PurchaseOrder.DeliveryHub.City;
                                txtBolShipperCountry.Text = "Canada";
                                txtBolShipperPostal.Text = order.PurchaseOrder.DeliveryHub.Zip;
                                ddlProvince.SelectedValue = order.PurchaseOrder.DeliveryHub.State;
                            }

                            emailTo.Text = string.IsNullOrWhiteSpace(customer.Email) ? string.Empty : customer.Email;
                            if (customer.AlternateEmailReceivesConfirmations && !string.IsNullOrWhiteSpace(customer.AlternateEmail))
                                emailCC.Text = customer.AlternateEmail;
                            emailSubject.Text = string.Format(EmailerResources.OrderConfirmationEmailSubject, OrderID);
                            emailBody.Text = string.Format(EmailerResources.OrderConfirmationEmailBody, customer.Company, OrderID);

                        }
                    }
                }
                catch (Exception ex)
                {
                    litMessage.Text = StringTools.GenerateError(ex.Message);
                }
            }

        }

        private string GetBulkRackHeading(Order order)
        {
            if (order.BulkOrderKey.HasValue)
            {
                return "(Bulk Rack Order)";
            }

            return string.Empty;
        }

        private string getOrderStatus(int orderStatus)
        {
            switch (orderStatus)
            {
                case 0:
                    return "Draft Order";
                case -1:
                    return "Quote";
                case 1:
                default:
                    return "Order";

            }
        }

        private void SetButtonVisiblity(Order order)
        {
            if (order == null)
            {
                btnApprove.Visible = false;
                btnDeleteOrder.Visible = false;
                btnRenew.Visible = false;
            }
            else
            {
                btnApprove.Visible = order.OrderStatus == -1 || order.OrderStatus == 0;
                btnDeleteOrder.Visible = true;
                var confirmMessage = "Are you sure you want to delete this order?";

                if (order.BulkOrderKey.HasValue)
                    confirmMessage = "Note: This order is part of a Bulk Rack order.  \\n\\nAre you sure you want to delete this order?";
                btnDeleteOrder.OnClientClick = $"return confirm('{confirmMessage}');";
                btnRenew.Visible = order.IsRenewable;
            }

            if (!currentUser.CanEditOrders)
            {
                btnSave.Visible = false;
                btnCopyBilling.Visible = false;
                btnCopyShipping.Visible = false;
                btnRemoveSelectedItems.Visible = false;
                lnkAddItems.Visible = false;

            }
            if (!currentUser.CanDeleteOrders)
            {
                btnDeleteOrder.Visible = false;
            }

            if (!currentUser.CanEditShipments)
            {
                btnShipSelected.Visible = false;
            }

            if (!currentUser.CanApproveOrders)
            {
                btnApprove.Visible = false;
            }
            if (!currentUser.CanEmailPrintExportOrders)
            {
                btnPrintBOL.Visible = false;
                btnPrintPickSheet.Visible = false;
                btnPrintConfirmation.Visible = false;
                PrintAndEmailDiv.Visible = false;
            }
        }

        private void ShowTotals(Order order)
        {
            try
            {
                if (order != null)
                {
                    divTotals.Visible = order.OrderItems.Count > 0;
                    lblDiscountedSubTotal.Text = order.DiscountedSubTotal.ToString("C");
                    lblGlobalDiscount1Discount.Text = order.GlobalDiscountAmount1.ToString("C");
                    lblDiscount2.Text = order.GlobalDiscountAmount2.ToString("C");
                    lblDiscount3.Text = order.GlobalDiscountAmount3.ToString("C");
                    lblDiscount4.Text = order.GlobalDiscountAmount4.ToString("C");
                    lblDiscount5.Text = order.GlobalDiscountAmount5.ToString("C");
                    lblDiscountedSubTotal2.Text = order.GlobalDiscountedSubTotal.ToString("C");

                    chkCustomFreightCharge.Checked = order.CustomShippingCharge;
                    txtFreightCharge.Enabled = order.CustomShippingCharge;
                    txtFreightCharge.Text = order.ShippingCharge.ToString("C");

                    lblGST.Text = (order.GSTAmount + order.PSTAmount).ToString("C");
                    lblHST.Text = String.Format("{0:C}", order.GSTAmount + order.PSTAmount);

                    if (order.HST == true)
                    {
                        lblHSTCaption.Visible = true;
                        lblHST.Visible = true;
                        lblGST.Visible = false;
                        lblGSTCaption.Visible = false;
                    }

                    lblTotal.Text = order.GrandTotal.ToString("C");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private void LoadItems(Order order)
        {
            try
            {
                if (order != null)
                {
                    tabItems.Visible = true;
                    tabShipments.Visible = true;
                    if (order.OrderItems.Any())
                    {
                        dgvItems.DataSource = order.OrderItems.OrderBy(x => x.OrderItemId).Select(r => new
                        {
                            OrderItemID = r.OrderItemId,
                            Qty = r.Quantity,
                            Category = r.Product.supProductSubCategory.SubCategoryDesc,
                            r.Product.ItemNumber,
                            Description = r.Product.ProductName,
                            ProductID = r.ProductId,
                            r.Product.Size,
                            ItemsPerPack = r.Product.ItemsPerPackage,
                            PacksPerUnit = r.Product.PackagesPerUnit,
                            r.UnitPrice,
                            Total = OrderHelper.GetTotalPrice(r.UnitPrice, r.DiscountPercent, r.Quantity),
                            Discount = r.DiscountPercent,
                            DiscountedCasePrice = OrderHelper.CalculateDiscountPrice(r.UnitPrice, r.DiscountPercent),
                            r.ProductNotAvailable,
                            r.UnshippedQuantity
                        }).ToList();
                        dgvItems.DataBind();
                    }
                }
                else
                {
                    tabItems.Visible = false;
                    tabShipments.Visible = false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        protected void btnRemoveSelectedItems_Click(object sender, EventArgs e)
        {
            try
            {
                using (var db = new MadduxEntities())
                {
                    var order = db.Orders.FirstOrDefault(o => o.OrderID == OrderID);
                    foreach (GridViewRow row in dgvItems.Rows)
                    {
                        var selectedCell = row.Cells[0];
                        var checkbox = (CheckBox)selectedCell.FindControl("chkSelected");

                        if (checkbox.Checked)
                        {
                            //todo: This line has a lot of potential for problems.
                            var id = int.Parse(((HiddenField)selectedCell.FindControl("hdnOrderItemId")).Value);
                            var orderItem = order.OrderItems.FirstOrDefault(r => r.OrderItemId == id);
                            if (orderItem != null && orderItem.ShipmentItems.Any() == false)
                            {
                                db.OrderItems.Remove(orderItem);
                            }
                        }
                    }
                    db.SaveChanges();
                    order.CalculateFreightAndTaxes();
                    db.SaveChanges();
                    Response.Redirect($@"/order/orderdetail.aspx?id={OrderID}&tab=items");
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }

        }

        protected void btnShipSelected_Click(object sender, EventArgs e)
        {
            try
            {
                using (var db = new MadduxEntities())
                {
                    var save = false;
                    var order = db.Orders.FirstOrDefault(o => o.OrderID == OrderID);

                    var shipment = new Shipment
                    {
                        CreateDate = DateTime.Now,

                        CustomShippingCharge = order.CustomShippingCharge,
                        GlobalDiscountDesc = order.GlobalDiscountDesc,
                        GlobalDiscountPercent = order.GlobalDiscountPercent,
                        GlobalDiscount2Desc = order.GlobalDiscount2Desc,
                        GlobalDiscount2Percent = order.GlobalDiscount2Percent,
                        GlobalDiscount3Desc = order.GlobalDiscount3Desc,
                        GlobalDiscount3Percent = order.GlobalDiscount3Percent,
                        GlobalDiscount4Desc = order.GlobalDiscount4Desc,
                        GlobalDiscount4Percent = order.GlobalDiscount4Percent,
                        GlobalDiscount5Desc = order.GlobalDiscount5Desc,
                        GlobalDiscount5Percent = order.GlobalDiscount5Percent,
                        GSTAmount = order.GSTAmount + order.PSTAmount,
                        HST = order.HST,
                        OfficeNotes = order.OfficeNotes,
                        PSTAmount = 0,
                        PSTExempt = false,
                        ShippingMethodId = order.ShippingMethodID,
                        ShippingCharge = order.ShippingCharge,


                    };
                    foreach (GridViewRow row in dgvItems.Rows)
                    {
                        var selectedCell = row.Cells[0];

                        var checkbox = (CheckBox)selectedCell.FindControl("chkSelected");

                        if (checkbox.Checked)
                        {
                            var id = Convert.ToInt32(dgvItems.DataKeys[row.RowIndex].Values[0]);
                            var orderItem = order.OrderItems.FirstOrDefault(r => r.OrderItemId == id);
                            if (orderItem != null)
                            {

                                //TODO: do we need to refactor this logic? (BULK RACK UPDATES 2024)
                                //      we removed the ShipQty text box from the screen,  that used to specify the 
                                //      number of items to ship.

                                //      Can we just assume that we will always ship the 
                                //      quantity ordered?
                                shipment.ShipmentItems.Add(new ShipmentItem
                                {
                                    OrderItemId = orderItem.OrderItemId,
                                    Quantity = orderItem.Quantity,
                                });
                                save = true;
                            }
                        }
                    }
                    if (save)
                    {
                        db.Shipments.Add(shipment);
                        db.SaveChanges();
                        //email packing-slip to the customer
                        int shipmentId = shipment.ShipmentID;

                        Response.Redirect($@"/order/download.aspx?id={shipmentId}&CustomerId={order.CustomerID}", true);
                    }

                }
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }

        }

        private bool PrintPackingSlip(Shipment shipment, int customerID)
        {
            try
            {
                using (var writer = new StringWriter())
                using (var db = new MadduxEntities())
                {

                    if (shipment == null)
                        return false;
                    Server.Execute($"~/order/packing-slip.aspx?id={shipment.OrderID}&sId={shipment.ShipmentID}&view=print", writer);
                    string html = writer.GetStringBuilder().ToString();


                    HtmlToPdf htmlToPdf = new HtmlToPdf()
                    {
                        Html = html,
                        Orientation = PdfOrientation.Portrait
                    };
                    RestSharp.RestClient client = new RestClient("https://html2pdf.webilitynetwork.ca/api/Page/GetByHtml")
                    {
                        Timeout = -1
                    };
                    RestRequest request = new RestRequest(Method.POST);
                    request.AddHeader("Content-Type", "application/json");

                    request.AddParameter(
                        "application/json",
                        JsonConvert.SerializeObject(htmlToPdf),
                        ParameterType.RequestBody);

                    IRestResponse pdfResponse = client.Execute(request);

                    Response.Clear();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Refresh", $@"5; url=/shipping/shipmentdetail.aspx?id={shipment.ShipmentID}&CustomerId={customerID}");
                    Response.AddHeader("Content-Disposition", $"attachment; filename=packingslip#{shipment.ShipmentID}-{DateTime.Now:yyyy-MM-dd-hh-mm-tt}.pdf");
                    Response.OutputStream.Write(pdfResponse.RawBytes, 0, pdfResponse.RawBytes.Length);
                    Response.Flush();
                    Response.End();
                    return true;
                }
            }
            catch
            {
                return false;
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    string _email = currentUser.EmailAddress.Trim();
                    using (var db = new MadduxEntities())
                    {
                        var order = new Order();
                        if (OrderID == 0)
                        {
                            order.CustomerID = CustomerID;
                            order.CreatedBy = _email;
                            order.DateCreated = DateTime.Now;
                            db.Orders.Add(order);
                        }
                        else
                        {
                            order = db.Orders.Include(o => o.OrderRacks).FirstOrDefault(o => o.OrderID == OrderID);

                        }
                        order.ShippingName = txtShippingCompany.Text;
                        order.ShippingAddress = txtShippingAddress.Text;
                        order.ShippingCity = txtShippingCity.Text;
                        order.ShippingState = ddShippingProvince.SelectedValue;
                        order.ShippingCountry = ddCountry.SelectedValue;
                        order.ShippingEmail = txtShippingEmail.Text;
                        order.ShippingZip = txtShippingPostal.Text;

                        order.BillingName = txtBillingCompany.Text;
                        order.BillingAddress = txtBillingAddress.Text;
                        order.BillingCity = txtBillingCity.Text;
                        order.BillingState = ddBillingProvince.SelectedValue;
                        order.BillingCountry = ddBillingCountry.SelectedValue;
                        order.BillingZip = txtBillingPostal.Text;


                        order.PaymentTermsID = int.Parse(ddDefaultTerms.SelectedValue);
                        order.PONumber = txtPONumber.Text;
                        order.CreatedBy = txtOrderPlacedBy.Text;
                        order.PaymentTypeID = int.Parse(ddDefaultPaymentType.SelectedValue);
                        order.SalesPersonID = int.Parse(ddSalesRep.SelectedValue);
                        order.ShippingMethodID = int.Parse(ddShippingMethod.SelectedValue);
                        order.ReceivedVia = int.Parse(ddlOrderReceivedVia.SelectedValue);

                        order.OrderStatus = int.Parse(ddOrderStatus.SelectedValue);

                        order.Approved = ddlOrderApproved.SelectedValue == "Yes" ? true : false;

                        order.RequestedShipDate = !string.IsNullOrWhiteSpace(ddlShipDate.SelectedValue) ? (DateTime?)DateTime.Parse(ddlShipDate.SelectedValue) : null;

                        order.PurchaseOrderID = !string.IsNullOrWhiteSpace(ddlPurchaseOrders.SelectedItem.Value) ? int.Parse(ddlPurchaseOrders.SelectedItem.Value) : (int?)null;

                        order.PurchaseOrdersSentDate = !string.IsNullOrWhiteSpace(txtPOSentDate.Text) ? (DateTime?)DateTime.Parse(txtPOSentDate.Text) : null;

                        order.ConfirmationSentDate = !string.IsNullOrWhiteSpace(txtConfirmationSent.Text) ? (DateTime?)DateTime.Parse(txtConfirmationSent.Text) : null;

                        order.OrderDate = !string.IsNullOrWhiteSpace(txtOrderDate.Text) ? (DateTime?)DateTime.Parse(txtOrderDate.Text) : null;
                        order.OrderNotes = txtNotes.Text;
                        order.OfficeNotes = txtOfficeNotes.Text;


                        //save Shipment Items
                        int x = 1;
                        foreach (GridViewRow row in dgvItems.Rows)
                        {
                            var orderItemId = Convert.ToInt32(dgvItems.DataKeys[row.RowIndex].Values[0]);
                            OrderItem orderItem = order.OrderItems.Where(i => i.OrderItemId == orderItemId).FirstOrDefault();
                            if (orderItem != null)
                            {
                                var qty = row.FindControl("txtQty") as TextBox;
                                var price = row.FindControl("txtPrice") as TextBox;
                                var discount = row.FindControl("txtDiscount") as TextBox;
                                var available = row.FindControl("chkProductAvailable") as CheckBox;
                                orderItem.Quantity = Convert.ToDouble(qty.Text);
                                orderItem.ProductNotAvailable = available.Checked;
                                orderItem.UnitPrice = Convert.ToDouble(Double.Parse(price.Text, NumberStyles.AllowCurrencySymbol | NumberStyles.Number));
                                orderItem.DiscountPercent = Convert.ToDouble(Double.Parse(discount.Text.Replace("%", string.Empty)) / 100);
                                orderItem.LinePosition = x;
                            }
                            x++;
                        }

                        if (order.OrderItems.Count > 0)
                        {
                            //save Global Discounts
                            order.GlobalDiscountDesc = txtDiscount1Desc.Text.Trim();
                            order.GlobalDiscount2Desc = txtDiscount2Desc.Text.Trim();
                            order.GlobalDiscount3Desc = txtDiscount3Desc.Text.Trim();
                            order.GlobalDiscount4Desc = txtDiscount4Desc.Text.Trim();
                            order.GlobalDiscount5Desc = txtDiscount5Desc.Text.Trim();

                            order.GlobalDiscountPercent = (double)(Convert.ToDouble(FCSAppUtils.GetNumberString(txtGlobalDiscount1Pct.Text)) / 100);
                            order.GlobalDiscount2Percent = (double)(Convert.ToDouble(FCSAppUtils.GetNumberString(txtDiscount2Pct.Text)) / 100);
                            order.GlobalDiscount3Percent = (double)(Convert.ToDouble(FCSAppUtils.GetNumberString(txtDiscount3Pct.Text)) / 100);
                            order.GlobalDiscount4Percent = (double)(Convert.ToDouble(FCSAppUtils.GetNumberString(txtDiscount4Pct.Text)) / 100);
                            order.GlobalDiscount5Percent = (double)(Convert.ToDouble(FCSAppUtils.GetNumberString(txtDiscount5Pct.Text)) / 100);

                            if (chkCustomFreightCharge.Checked)
                            {
                                order.CustomShippingCharge = true;
                                order.ShippingCharge = Convert.ToDecimal(FCSAppUtils.GetNumberString(txtFreightCharge.Text));
                            }
                            else
                            {
                                order.CustomShippingCharge = false;

                            }
                            order.PSTExempt = false;
                            db.SaveChanges();
                            order.CalculateFreightAndTaxes();
                        }
                        order.UpdatedBy = _email;
                        order.DateUpdated = DateTime.Now;

                        db.SaveChanges();
                        OrderID = order.OrderID;
                        Response.Redirect($@"/order/orderdetail.aspx?id={OrderID}");
                    }
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }

        /// <summary>
        /// Populates Racks to order
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddCatalogList_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (var db = new MadduxEntities())
            {

                if (ddCatalogList.SelectedValue != "0")
                {
                    int catalogId = int.Parse(ddCatalogList.SelectedValue);

                    grdOrderItems.DataSource = db.Products.Where(p => p.CatalogId == catalogId).Select(r => new
                    {
                        ProductID = r.ProductId,
                        Page = r.CatalogPageStart.HasValue ? r.CatalogPageStart.Value + " - " + r.CatalogPageEnd.Value : "",
                        Category = r.supProductSubCategory.supProductCategory.CategoryDesc,
                        ItemNo = r.ItemNumber,
                        Description = r.ProductName,
                        r.Size,
                        ItemsPerPack = r.ItemsPerPackage,
                        PacksPerUnit = r.PackagesPerUnit,
                        r.UnitPrice,
                        New = r.NewItem
                    }).OrderBy(x => x.Description).ToList();
                    grdOrderItems.DataBind();
                }
            }
        }

        protected void saveAndClose_Click(object sender, EventArgs e)
        {
            using (var db = new MadduxEntities())
            {
                var order = db.Orders.FirstOrDefault(f => f.OrderID == OrderID);
                foreach (GridViewRow row in grdOrderItems.Rows)
                {
                    var quantityCell = row.Cells[0];
                    var priceCell = row.Cells[7];

                    var quantity = ((TextBox)quantityCell.FindControl("txtUnits")).Text;
                    var id = ((HiddenField)quantityCell.FindControl("hdnId")).Value;
                    var price = ((TextBox)priceCell.FindControl("UnitPrice")).Text;

                    if (int.TryParse(quantity, out int qty) && qty > 0)
                    {
                        var productId = int.Parse(id);
                        if (double.TryParse(price, NumberStyles.AllowCurrencySymbol | NumberStyles.Number, CultureInfo.CurrentCulture, out double unitPrice))
                        {
                            var item = new OrderItem
                            {
                                ProductId = productId,
                                Quantity = qty,
                                UnitPrice = unitPrice,
                                DiscountPercent = 0,
                                ProductNotAvailable = false,
                                LinePosition = 0,
                                NoGlobalDiscount = false,
                                OrderRackId = order.OrderRacks.FirstOrDefault()?.OrderRackId

                            };
                            order.OrderItems.Add(item);
                        }

                    }

                }
                db.SaveChanges();
                order.CalculateFreightAndTaxes();
                db.SaveChanges();

                Response.Redirect($@"/order/orderdetail.aspx?id={OrderID}&tab=items");
            }
        }

        protected void btnDeleteOrder_Click(object sender, EventArgs e)
        {
            using (var db = new MadduxEntities())
            {
                var order = db.Orders.FirstOrDefault(o => o.OrderID == OrderID);
                int oID = order.OrderID;
                var status = order.OrderStatus;
                try
                {
                    order.PurchaseOrderID = null;
                    var shipped = db.Shipments.Where(x => x.ShipmentItems.Any(si => si.OrderItem.OrderId == oID)).FirstOrDefault();
                    if (shipped == null)
                    {
                        var racks = order.OrderRacks;
                        var orderItems = order.OrderItems;

                        if (orderItems != null)
                            db.OrderItems.RemoveRange(orderItems);
                        if (racks != null)
                            db.OrderRacks.RemoveRange(racks);
                        db.Orders.Remove(order);
                        int changes = db.SaveChanges();
                        if (changes > 0)
                            Response.Redirect($@"/order/myorders.aspx?type={status}");
                        else
                            throw new Exception("Error while removing order please try again");

                    }
                    else
                    {
                        throw new Exception("Can not delete order because some of the items from the order already have been shipped.");
                    }


                }
                catch (Exception ex)
                {
                    Master.AddMessage(StringTools.GenerateError(ex.Message));
                }
            }
        }

        protected void btnCopyBilling_Click(object sender, EventArgs e)
        {
            txtShippingCompany.Text = txtBillingCompany.Text.ToString();
            txtShippingAddress.Text = txtBillingAddress.Text.ToString();
            txtShippingCity.Text = txtBillingCity.Text.ToString();
            txtShippingPostal.Text = txtBillingPostal.Text.ToString();
            ddShippingProvince.SelectedValue = ddBillingProvince.SelectedValue;
            ddCountry.SelectedValue = ddBillingCountry.SelectedValue;
        }

        protected void btnCopyShipping_Click(object sender, EventArgs e)
        {
            txtBillingCompany.Text = txtShippingCompany.Text.ToString();
            txtBillingAddress.Text = txtShippingAddress.Text.ToString();
            txtBillingCity.Text = txtShippingCity.Text.ToString();
            txtBillingPostal.Text = txtShippingPostal.Text.ToString();
            ddBillingProvince.SelectedValue = ddShippingProvince.SelectedValue;
            ddBillingCountry.SelectedValue = ddCountry.SelectedValue;
        }

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                using (var db = new MadduxEntities())
                {
                    Order order = db.Orders.FirstOrDefault(o => o.OrderID == OrderID);
                    order.OrderStatus = 1;
                    order.OrderDate = DateTime.UtcNow;
                    db.SaveChanges();
                    btnApprove.Visible = false;
                    ddOrderStatus.SelectedValue = order.OrderStatus.ToString();
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }

        protected void dgvItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                System.Web.UI.WebControls.HyperLink lnk = e.Row.FindControl("lnkEditProduct") as System.Web.UI.WebControls.HyperLink;
                Label lbl = e.Row.FindControl("lblProductName") as Label;
                lnk.Visible = currentUser.ShowSettings;
                lbl.Visible = !currentUser.ShowSettings;
            }
        }

        protected void chkCustomFreightCharge_CheckedChanged(object sender, EventArgs e)
        {
            txtFreightCharge.Enabled = chkCustomFreightCharge.Checked == true;
        }

        protected void btnRenew_Click(object sender, EventArgs e)
        {
            try
            {
                using (var db = new MadduxEntities())
                {
                    Order order = db.Orders.Find(OrderID);
                    if (order != null)
                    {
                        int newCatalogId = order.GetNextActiveCatalog();
                        if (newCatalogId > 0)
                        {
                            Order newOrder = new Order
                            {
                                BillingAddress = order.BillingAddress,
                                BillingCity = order.BillingCity,
                                BillingCountry = order.BillingCountry,
                                BillingName = order.BillingName,
                                BillingState = order.BillingState,
                                BillingZip = order.BillingZip,
                                Customer = order.Customer,
                                OrderDate = DateTime.Now,
                                OrderStatus = 0,
                                PaymentTermsID = order.PaymentTermsID,
                                PaymentTypeID = order.PaymentTypeID,
                                SalesPersonID = order.SalesPersonID,
                                ShippingAddress = order.ShippingAddress,
                                ShippingCity = order.ShippingCity,
                                ShippingCountry = order.ShippingCountry,
                                ShippingEmail = order.ShippingEmail,
                                ShippingName = order.ShippingName,
                                ShippingState = order.ShippingState,
                                ShippingZip = order.ShippingZip,
                                CreatedBy = currentUser.EmailAddress.Trim(),
                                UpdatedBy = currentUser.EmailAddress.Trim(),
                                DateCreated = DateTime.Now,
                                DateUpdated = DateTime.Now
                            };
                            newOrder.SalesPersonID = currentUser.UserID;
                            newOrder.ShippingMethodID = 1;

                            db.Orders.Add(newOrder);

                            foreach (var item in order.OrderItems)
                            {
                                if (item.ShipmentItems.Select(i => i.Quantity).FirstOrDefault() != 0)
                                {
                                    var newProduct = db.Products.Where(p => p.ItemNumberInternal == item.Product.ItemNumberInternal && p.CatalogId == newCatalogId && !p.ProductNotAvailable).FirstOrDefault();
                                    if (newProduct != null)
                                    {
                                        OrderItem newItem = new OrderItem
                                        {
                                            Quantity = item.Quantity,
                                            ProductId = newProduct.ProductId,
                                            UnitPrice = newProduct.UnitPrice,
                                            DiscountPercent = 0,
                                            LinePosition = 0
                                        };
                                        newOrder.OrderItems.Add(newItem);
                                    }
                                }
                            }

                            if (newOrder.OrderItems.Count > 0)
                            {
                                db.SaveChanges();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }

        }

        protected void ddlPrograms_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateShipDateDropdown();
            PopulateCatalogDropdown();
        }

        protected void btnPrintConfirmation_Click(object sender, EventArgs e)
        {
            try
            {
                using (var writer = new StringWriter())
                {
                    Server.Execute($"/order/confirmation.aspx?id={OrderID}&view=print", writer);
                    string html = writer.GetStringBuilder().ToString();



                    HtmlToPdf htmlToPdf = new HtmlToPdf()
                    {
                        Html = html,
                        Orientation = PdfOrientation.Portrait
                    };
                    RestSharp.RestClient client = new RestClient("https://html2pdf.webilitynetwork.ca/api/Page/GetByHtml")
                    {
                        Timeout = -1
                    };
                    RestRequest request = new RestRequest(Method.POST);
                    request.AddHeader("Content-Type", "application/json");

                    request.AddParameter(
                        "application/json",
                        JsonConvert.SerializeObject(htmlToPdf),
                        ParameterType.RequestBody);

                    IRestResponse pdfResponse = client.Execute(request);

                    Response.Clear();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", $"attachment; filename=orderconfirmation-{OrderID}-{DateTime.Now:yyyy-MM-dd-hh-mm-tt}.pdf");
                    Response.OutputStream.Write(pdfResponse.RawBytes, 0, pdfResponse.RawBytes.Length);
                    Response.End();
                }
            }
            catch (Exception ex)
            {

                litMessage.Text = StringTools.GenerateError(ex.Message);
            }


        }

        protected void btnPrintPickSheet_Click(object sender, EventArgs e)
        {
            try
            {
                Emailer emailer = new Emailer();
                using (var writer = new StringWriter())
                using (var db = new MadduxEntities())
                {
                    Server.Execute($"/order/picksheet.aspx?id={OrderID}&view=print", writer);
                    string html = writer.GetStringBuilder().ToString();

                    HtmlToPdf htmlToPdf = new HtmlToPdf()
                    {
                        Html = html,
                        Orientation = PdfOrientation.Portrait
                    };
                    RestSharp.RestClient client = new RestClient("https://html2pdf.webilitynetwork.ca/api/Page/GetByHtml")
                    {
                        Timeout = -1
                    };
                    RestRequest request = new RestRequest(Method.POST);
                    request.AddHeader("Content-Type", "application/json");

                    request.AddParameter(
                        "application/json",
                        JsonConvert.SerializeObject(htmlToPdf),
                        ParameterType.RequestBody);

                    IRestResponse pdfResponse = client.Execute(request);

                    Response.Clear();
                    Response.ContentType = EmailerResources.PDFContentType;
                    Response.AddHeader("Content-Disposition", $"attachment; filename=picksheet-{OrderID}-{DateTime.Now:yyyy-MM-dd-hh-mm-tt}.pdf");
                    Response.OutputStream.Write(pdfResponse.RawBytes, 0, pdfResponse.RawBytes.Length);
                    Response.Flush();
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }

        protected void btnPrintBOL_Click(object sender, EventArgs e)
        {
            try
            {
                using (var writer = new StringWriter())
                using (var db = new MadduxEntities())
                {
                    string shipperInfo = string.Empty;
                    shipperInfo += txtBolShipperAccountNo.Text + "#";
                    shipperInfo += txtBolShipperPhone.Text + "#";
                    shipperInfo += txtBolShipperName.Text + "#";
                    shipperInfo += txtBolShipperAddress.Text + "#";
                    shipperInfo += txtBolShipperCity.Text + "#";
                    shipperInfo += ddlProvince.SelectedValue + "#";
                    shipperInfo += txtBolShipperPostal.Text + "#";
                    shipperInfo += txtBolShipperCountry.Text + "#";

                    shipperInfo = HttpUtility.HtmlEncode(shipperInfo);


                    Server.Execute($"/order/bill-of-lading.aspx?id={OrderID}&shipperInfo={shipperInfo}&view=print", writer);
                    string html = writer.GetStringBuilder().ToString();

                    HtmlToPdf htmlToPdf = new HtmlToPdf()
                    {
                        Html = html,
                        Orientation = PdfOrientation.Portrait
                    };
                    RestSharp.RestClient client = new RestClient("https://html2pdf.webilitynetwork.ca/api/Page/GetByHtml")
                    {
                        Timeout = -1
                    };
                    RestRequest request = new RestRequest(Method.POST);
                    request.AddHeader("Content-Type", "application/json");

                    request.AddParameter(
                        "application/json",
                        JsonConvert.SerializeObject(htmlToPdf),
                        ParameterType.RequestBody);

                    IRestResponse pdfResponse = client.Execute(request);

                    Response.Clear();
                    Response.ContentType = EmailerResources.PDFContentType;
                    Response.AddHeader("Content-Disposition", $"attachment; filename=BOL#{OrderID}.pdf");
                    Response.OutputStream.Write(pdfResponse.RawBytes, 0, pdfResponse.RawBytes.Length);
                    Response.Flush();
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }


        }

        protected void btnEmailOrderConfirmation_Click(object sender, EventArgs e)
        {
            try
            {
                Emailer emailer = new Emailer();
                using (var writer = new StringWriter())
                using (var db = new MadduxEntities())
                {
                    var order = db.Orders.FirstOrDefault(x => x.OrderID == OrderID);


                    var customer = db.Customers.Select(x => new
                    {
                        x.CustomerId,
                        x.Email,
                        x.InvoiceEmail,
                        CanSendInvoice = x.EmailInvoice,
                        CustomerName = x.Company
                    }).FirstOrDefault(x => x.CustomerId == order.CustomerID);


                    Server.Execute($"/order/confirmation.aspx?id={OrderID}&view=print", writer);
                    string html = writer.GetStringBuilder().ToString();

                    HtmlToPdf htmlToPdf = new HtmlToPdf()
                    {
                        Html = html,
                        Orientation = PdfOrientation.Portrait
                    };
                    RestSharp.RestClient client = new RestClient("https://html2pdf.webilitynetwork.ca/api/Page/GetByHtml")
                    {
                        Timeout = -1
                    };
                    RestRequest request = new RestRequest(Method.POST);
                    request.AddHeader("Content-Type", "application/json");

                    request.AddParameter(
                        "application/json",
                        JsonConvert.SerializeObject(htmlToPdf),
                        ParameterType.RequestBody);

                    IRestResponse pdfResponse = client.Execute(request);

                    if (!string.IsNullOrWhiteSpace(emailTo.Text))
                    {
                        Dictionary<int, string> recipients = new Dictionary<int, string>
                        {
                            { order.Customer.CustomerId, emailTo.Text }
                        };

                        var cc = new List<string> { emailCC.Text };

                        bool emailSent = emailer.SendEmailWithAttachment(
                                                       recipients,
                                                       string.Format(emailSubject.Text),
                                                       $@"{emailBody.Text} {EmailerResources.EmailFooter}",
                                                       pdfResponse.RawBytes,
                                                       $"order-confirmation-{OrderID}.pdf",
                                                       EmailerResources.PDFContentType,
                                                       cc
                                                    );

                        if (emailSent)
                        {
                            litMessage.Text = StringTools.GenerateSuccess($"Order confirmation sent successfully!");
                            if (chkUpdateConfirmationDate.Checked)
                            {
                                order.ConfirmationSentDate = DateTime.Now;
                                db.SaveChanges();
                                Response.Redirect($"~/order/orderdetail.aspx?id={order.OrderID}&success=true&message=Order confirmation sent successfully!");
                            }
                        }
                        else
                            throw new Exception($"Error while sending order confirmation to {customer.CustomerName} please try again!");
                    }
                    else
                    {
                        throw new Exception("Please insert recepients email address!");
                    }
                }
            }
            catch (Exception ex)
            {

                litMessage.Text = StringTools.GenerateError(ex.Message);
            }

        }
        private void checkForMessage()
        {

            if (Request.QueryString["success"] != null && Request.QueryString["success"] != "")
            {
                string success = Request.QueryString["success"].ToString();
                string message = Request.QueryString["message"].ToString();
                if (string.Equals(success, "false"))
                {
                    litMessage.Text = StringTools.GenerateError(message);
                }
                if (string.Equals(success, "true"))
                {
                    litMessage.Text = StringTools.GenerateSuccess(message);
                }
            }
        }
        protected void btnEmailPickSheet_Click(object sender, EventArgs e)
        {
            try
            {
                Emailer emailer = new Emailer();
                using (var writer = new StringWriter())
                using (var db = new MadduxEntities())
                {
                    Server.Execute($"/order/picksheet.aspx?id={OrderID}&view=print", writer);
                    string html = writer.GetStringBuilder().ToString();

                    HtmlToPdf htmlToPdf = new HtmlToPdf()
                    {
                        Html = html,
                        Orientation = PdfOrientation.Portrait
                    };
                    RestSharp.RestClient client = new RestClient("https://html2pdf.webilitynetwork.ca/api/Page/GetByHtml")
                    {
                        Timeout = -1
                    };
                    RestRequest request = new RestRequest(Method.POST);
                    request.AddHeader("Content-Type", "application/json");

                    request.AddParameter(
                        "application/json",
                        JsonConvert.SerializeObject(htmlToPdf),
                        ParameterType.RequestBody);

                    IRestResponse pdfResponse = client.Execute(request);

                    var order = db.Orders.FirstOrDefault(x => x.OrderID == OrderID);

                    Dictionary<int, string> recipients = new Dictionary<int, string>
                    {
                        { order.Customer.CustomerId, order.Customer.Email }
                    };
                    var sent = emailer.SendEmailWithAttachment(recipients,
                                                    string.Format(EmailerResources.PickSheetEmailSubject, OrderID),
                                                    EmailerResources.PickSheetEmailBody,
                                                    pdfResponse.RawBytes,
                                                    $"picksheet-{OrderID}.pdf",
                                                    EmailerResources.PDFContentType
                                                    );

                    litMessage.Text = sent
                        ? StringTools.GenerateSuccess($"Pick sheet sent successfully!")
                        : StringTools.GenerateError($"Error while sending pick sheet to {order.Customer.Company} please try again!");
                }
            }
            catch (Exception ex)
            {

                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }

        /// <summary>
        /// Populates a dropdown list from enum values
        /// </summary>
        /// <param name="dropDownList">The <see cref="DropDownList"/> to populate</param>
        /// <param name="options">The <see cref="string"/>to populate dropdown with</param>
        /// <param name="message">The <see cref="string"/>message for the select option</param>
        /// <param name="selectValue">The <see cref="string"/>value of the select from dropdown option</param>
        private void PopulateDropDownFromEnum(DropDownList dropDownList, string[] options, string message, string selectValue)
        {
            for (int i = 0; i <= options.Length - 1; i++)
            {
                ListItem item = new ListItem { Text = options[i], Value = (i + 1).ToString() };
                dropDownList.Items.Add(item);
            }
            dropDownList.Items.Insert(0, new ListItem { Text = message, Value = selectValue });
        }
    }
}
