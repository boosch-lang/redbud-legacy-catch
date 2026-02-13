using Maddux.Catch.LocalClasses;
using Redbud.BL.DL;
using Redbud.BL.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Maddux.Catch.customer
{
    public class CustomerOrder
    {
        public int OrderID { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? RequestedShipDate { get; set; }
        public string Catalog { get; set; }
        public double Total { get; set; }
        public string RackName { get; set; }
    }
    public partial class customerdetail : System.Web.UI.Page
    {
        protected int CustomerID
        {
            get
            {
                if (ViewState["CustomerID"] == null)
                {
                    ViewState["CustomerID"] = Request.QueryString["CustomerID"] == null || Request.QueryString["CustomerID"] == "" ? 0 : (object)Request.QueryString["CustomerID"];
                }
                return Convert.ToInt32(ViewState["CustomerID"].ToString());
            }

            set
            {
                ViewState["CustomerID"] = value;
            }
        }
        private User currentUser;

        private Customer customer;
        private const string PasswordMask = "**********";

        private void PopulateDropdowns()
        {
            using (var db = new MadduxEntities())
            {
                var countries = db.Countries.Select(r => new ListItem
                {
                    Text = r.CountryName,
                    Value = r.CountryCode
                }).ToList();

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

                //var types = db.supPaymentTypes.Select(r => new ListItem
                //{
                //    Text = r.PaymentTypeDesc,
                //    Value = r.PaymentTypeID.ToString()
                //}).ToList();

                var salesReps = db.Users.Where(x => x.Active == true).Select(r => new ListItem
                {
                    Text = r.FirstName + " " + r.LastName,
                    Value = r.UserID.ToString()
                }).ToList();

                //var shippingMethods = db.supShippingMethods.Select(r => new ListItem
                //{
                //    Value = r.ShippingMethodID.ToString(),
                //    Text = r.ShippingMethodDesc
                //}).ToList();

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

                //ddDefaultPaymentType.DataSource = types;
                //ddDefaultPaymentType.DataBind();

                ddSalesRep.DataSource = salesReps;
                ddSalesRep.DataBind();

                //ddShippingMethod.DataSource = shippingMethods;
                //ddShippingMethod.DataBind();
            }

        }

        public void CheckForMessage()
        {
            if (!string.IsNullOrWhiteSpace(Request.QueryString["message"]) && !string.IsNullOrWhiteSpace(Request.QueryString["type"]))
            {
                string message = Request.QueryString["message"].ToString();
                string type = Request.QueryString["type"].ToString();
                if (string.Equals(type, "success"))
                {
                    litMessage.Text = StringTools.GenerateSuccess(message);
                }
                if (string.Equals(type, "failed"))
                {
                    litMessage.Text = StringTools.GenerateError(message);
                }
            }
            if (Request.QueryString["oSuccess"] != null)
            {
                litMessage.Text = StringTools.GenerateSuccess("The new order has been created successfully!");
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            currentUser = AppSession.Current.CurrentUser;

            tabAssociations.Visible = currentUser.CanViewCustomerAssociations;


            if (currentUser.CanEditOrders)
            {
                lnkNewOrder.Visible = true;
                lnkNewOrder.Attributes.Add("href", "/order/additem.aspx?CustomerID=" + CustomerID);
            }
            else
            {
                lnkNewOrder.Visible = false;
            }
            if (currentUser.CanEditCreditMemos)
            {
                lnkCreditMemo.Visible = true;
                lnkCreditMemo.NavigateUrl = string.Format("/credit/creditdetail.aspx?customerID={0}", CustomerID);
            }
            else
            {
                lnkCreditMemo.Visible = false;
            }
            if (currentUser.CanMergeCustomers)
            {
                lnkMerge.Visible = true;
                lnkMerge.NavigateUrl = "/customer/merge.aspx?CustomerID=" + CustomerID;
            }
            else
            {
                lnkMerge.Visible = false;
            }
            if (!Page.IsPostBack)
            {
                CheckForMessage();


                using (var db = new MadduxEntities())
                {
                    PopulateDropdowns();



                    if (CustomerID != 0)
                    {
                        Title = "Maddux | Customer";
                        Literal litPageHeader = (Literal)Master.FindControl("litPageHeader");
                        customer = db.Customers.FirstOrDefault(r => r.CustomerId == CustomerID);
                        lblPassword.InnerText = "Reset Customer Password";

                        btnDelete.Visible = currentUser.CanDeleteCustomers && customer.Active;
                        var starRating = customer.StarRatingGraphic == "N/A" ? "No Rating" : customer.StarRatingGraphic;

                        litPageHeader.Text = string.Format("{3} - ({0} {1} - {2}) <span style='float:right;'>{4}</span><br />{5}", customer.FirstName,
                            customer.LastName, customer.Phone, customer.Company, starRating, customer.Active?"": "<span class='label bg-red'>DELETED</span>");
                        
                        if (customer.WebPassword_Hash != null)
                        {
                            txtUserPassword.Text = PasswordMask;
                        }

                        // Shipping Details
                        txtShippingCompany.Text = customer.Company;
                        txtShippingContactPrefix.Text = customer.Salutation;
                        txtShippingContactFirst.Text = customer.FirstName;
                        txtShippingContactLast.Text = customer.LastName;
                        //txtJobTitle.Text = customer.JobTitle;
                        txtShippingAddress.Text = customer.Address;
                        txtShippingCity.Text = customer.City;
                        ddShippingProvince.SelectedValue = customer.State;
                        ddCountry.SelectedValue = customer.Country;
                        txtShippingPostal.Text = customer.Zip;

                        //get Freight Charge for Customer
                        if (!string.IsNullOrEmpty(customer.Zip) && customer.Zip.Length >= 3)
                        {
                            var areaId = customer.Zip.Substring(0, 3);
                            var freightCharge = db.FreightCharges.Where(x => x.Province == customer.State && x.AreaID == areaId).FirstOrDefault();
                            litFreightCharge.Text = freightCharge != null ? $"{freightCharge.Charge} %" : "Not Available";
                            litRegion.Text = freightCharge != null ? freightCharge.Region : "Not Available";
                        }
                        else
                        {
                            litFreightCharge.Text = "Not Available";
                            litRegion.Text = "Not Available";
                        }

                        //Formatted address for google map link

                        //View on google map link
                        hlViewShippingAddress.NavigateUrl = string.Format(@"https://www.google.ca/maps/place/{0}+{1}+{2}+{3}+{4}", customer.Address, customer.City, customer.State, customer.Zip, customer.Country);

                        //Billing Details
                        txtBillingCompany.Text = customer.BillingCompany;
                        txtBillingContactPrefix.Text = customer.BillingSalutation;
                        txtBillingContactFirst.Text = customer.BillingFirstName;
                        txtBillingContactLast.Text = customer.BillingLastName;
                        //txtBillingPhone.Text = customer.BillingPhone;
                        txtBillingAddress.Text = customer.BillingAddress;
                        txtBillingCity.Text = customer.BillingCity;
                        ddBillingProvince.SelectedValue = customer.BillingState;
                        ddBillingCountry.SelectedValue = customer.BillingCountry;
                        txtBillingPostal.Text = customer.BillingZip;

                        ddDefaultTerms.SelectedValue = customer.DefaultTermsId.ToString();
                        txtVendorNumber.Text = customer.VendorNumber;
                        //chkPstExempt.Checked = customer.PSTExempt;
                        //ddDefaultPaymentType.SelectedValue = customer.DefaultPaymentTypeId.ToString();
                        ddSalesRep.SelectedValue = customer.SalesPersonID.ToString();
                        //ddShippingMethod.SelectedValue = customer.DefaultShippingMethodID.ToString();
                        chkActive.Checked = customer.Active;

                        txtContactPhone.Text = customer.Phone;
                        txtPhoneExtension.Text = customer.Extension;
                        txtEmail.Text = customer.Email;
                        chkEmailRecievesNewsletters.Checked = customer.EmailRecievesNewsletters == true;
                        txtFax.Text = customer.Fax;
                        txtAltEmail.Text = customer.AlternateEmail;
                        chkAltEmailRecievesNewsletters.Checked = customer.AlternateEmailRecievesNewsletters == true;
                        chkAltEmailReceivesConfirmations.Checked = customer.AlternateEmailReceivesConfirmations;
                        txtMobile.Text = customer.CellPhone;
                        txtWebsite.Text = customer.WebSite;

                        List<string> emails = customer.Email?.Split(',').ToList() ?? new List<string>();
                        if (!string.IsNullOrEmpty(customer.AlternateEmail))
                            emails = emails.Concat(customer.AlternateEmail.Split(',').ToList()).ToList();
                        phEmailLinks.Visible = emails.Count > 0;
                        repMailto.DataSource = emails.Distinct();
                        repMailto.DataBind();

                        chkEmailInvoice.Checked = customer.EmailInvoice;
                        chkPrintInvoices.Checked = customer.PrintInvoice;
                        txtInvoiceEmail.Text = customer.InvoiceEmail;
                        hlInvoiceEmailMailto.NavigateUrl = string.IsNullOrEmpty(customer.InvoiceEmail) ? string.Empty : $"mailto:{customer.InvoiceEmail}";
                        chkInvoiceShipment.Checked = customer.InvoiceWithShipment;

                        txtNotes.Text = customer.Notes;

                        dgvContacts.DataSource = customer.Contacts;
                        dgvContacts.DataBind();

                        var parentIDs = db.CustomersSubs.Where(x => x.ChildCustomerID == customer.CustomerId && x.MasterCustomerID != customer.CustomerId).Select(x => x.MasterCustomerID).ToList();
                        List<Customer> parents = db.Customers.Where(x => parentIDs.Contains(x.CustomerId)).ToList();
                        gvParent.DataSource = parents.OrderBy(x => x.Company);
                        gvParent.DataBind();

                        var childIDs = db.CustomersSubs.Where(x => x.MasterCustomerID == customer.CustomerId).Select(x => x.ChildCustomerID).ToList();
                        List<Customer> children = db.Customers.Where(x => childIDs.Contains(x.CustomerId)).ToList();
                        gvChild.DataSource = children.OrderBy(x => x.Company);
                        gvChild.DataBind();

                        var custOrders = customer.Orders.ToList();

                        LoadOrders(custOrders);

                        var orders = custOrders.Select(o => new
                        {
                            o.OrderID,
                            o.OrderDate,
                            Catalog = o.Catalogues,
                            Total = o.DiscountedSubTotal,
                            o.OrderItems,
                            o.RequestedShipDate
                        }).OrderByDescending(o => o.OrderID).ToList();

                        List<int> orderItemIds = new List<int>();
                        foreach (var item in orders)
                        {
                            var q = item.OrderItems.Select(r => r.OrderItemId);
                            orderItemIds.AddRange(q);
                        }

                        dgvShipments.DataSource = db.Shipments.Where(r => r.ShipmentItems.Any(q => orderItemIds.Contains(q.OrderItemId))).OrderByDescending(r => r.ShipmentID).ToList();
                        dgvShipments.DataBind();

                        dgvCreditMemos.DataSource = customer.Credits.OrderByDescending(c => c.CreditID);
                        dgvCreditMemos.DataBind();

                        LoadJournals();

                        LoadAssociationsAndCatalogs();
                        LoadStores();
                    }
                    else
                    {
                        Title = "Maddux | New Customer";
                        Literal litPageHeader = (Literal)Master.FindControl("litPageHeader");
                        litPageHeader.Text = "New Customer";
                        lblPassword.InnerText = "Customer Password";
                        customer = new Customer();
                        nav.Visible = false;
                        lnkMerge.Visible = false;
                        btnDelete.Visible = false;
                    }
                }

            }
        }
        private void LoadOrders(List<Order> orders)
        {
            try
            {
                dgvOrders.DataSource = from order in orders.OrderByDescending(x => x.RequestedShipDate).ThenByDescending(x=>x.OrderDate).ThenByDescending(x=>x.OrderID)
                                       select new
                                       {
                                           order.OrderID,
                                           order.OrderDate,
                                           order.Catalogues,
                                           order.DiscountedSubTotal,
                                           order.RequestedShipDate,
                                           RackName = order.OrderRacks.Any() ? order.OrderRacks.FirstOrDefault().RackName : "N/A"
                                       };
                dgvOrders.DataBind();
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }

        }
        /// <summary>
        /// Populates Journals GridView
        /// </summary>
        private void LoadJournals()
        {
            try
            {
                if (CustomerID != 0)
                {
                    using (var db = new MadduxEntities())
                    {
                        customer = db.Customers.FirstOrDefault(r => r.CustomerId == CustomerID);
                        dgvJournals.DataSource = customer.Journals.OrderByDescending(j => j.JournalID).ToList();
                        dgvJournals.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }

        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (Page.IsValid)
            {
                using (var db = new MadduxEntities())
                {
                    Customer customer;
                    if (CustomerID == 0)
                    {
                        customer = new Customer();
                        db.Customers.Add(customer);
                    }
                    else
                    {
                        customer = db.Customers.FirstOrDefault(c => c.CustomerId == CustomerID);
                    }
                    customer.Active = chkActive.Checked;

                    customer.Company = txtShippingCompany.Text;
                    customer.Salutation = txtShippingContactPrefix.Text;
                    customer.FirstName = txtShippingContactFirst.Text;
                    customer.LastName = txtShippingContactLast.Text;
                    //customer.JobTitle = txtJobTitle.Text;
                    customer.Address = txtShippingAddress.Text;
                    customer.City = txtShippingCity.Text;
                    customer.State = ddShippingProvince.SelectedValue;
                    customer.Country = ddCountry.SelectedValue;
                    customer.Zip = txtShippingPostal.Text;

                    customer.BillingCompany = txtBillingCompany.Text;
                    customer.BillingSalutation = txtBillingContactPrefix.Text;
                    customer.BillingFirstName = txtBillingContactFirst.Text;
                    customer.BillingLastName = txtBillingContactLast.Text;
                    //customer.BillingPhone = txtBillingPhone.Text;
                    customer.BillingAddress = txtBillingAddress.Text;
                    customer.BillingCity = txtBillingCity.Text;
                    customer.BillingState = ddBillingProvince.SelectedValue;
                    customer.BillingCountry = ddBillingCountry.SelectedValue;
                    customer.BillingZip = txtBillingPostal.Text;

                    customer.DefaultTermsId = int.Parse(ddDefaultTerms.SelectedValue);
                    customer.PSTExempt = false;
                    customer.VendorNumber = txtVendorNumber.Text.Trim();
                    //customer.DefaultPaymentTypeId = int.Parse(ddDefaultPaymentType.SelectedValue);
                    customer.SalesPersonID = int.Parse(ddSalesRep.SelectedValue);

                    customer.Phone = txtContactPhone.Text;
                    customer.Extension = txtPhoneExtension.Text;
                    customer.Email = txtEmail.Text;
                    customer.EmailRecievesNewsletters = chkEmailRecievesNewsletters.Checked;
                    customer.Fax = txtFax.Text;
                    customer.AlternateEmail = txtAltEmail.Text;
                    customer.AlternateEmailRecievesNewsletters = chkAltEmailRecievesNewsletters.Checked;
                    customer.AlternateEmailReceivesConfirmations = chkAltEmailReceivesConfirmations.Checked;
                    customer.CellPhone = txtMobile.Text;
                    customer.WebSite = txtWebsite.Text;

                    customer.EmailInvoice = chkEmailInvoice.Checked;
                    customer.PrintInvoice = chkPrintInvoices.Checked;
                    customer.InvoiceEmail = txtInvoiceEmail.Text;
                    customer.InvoiceWithShipment = chkInvoiceShipment.Checked;

                    if(!string.IsNullOrEmpty(txtUserPassword.Text) && txtUserPassword.Text != PasswordMask)
                    {
                        customer.WebPassword_Hash = FCSEncryption.Encrypt(txtUserPassword.Text);
                        customer.IsTemporaryPassword = true;
                    }

                    customer.Notes = txtNotes.Text;

                    db.SaveChanges();
                    Response.Redirect($@"/customer/customerdetail.aspx?customerid={customer.CustomerId}", false);
                }
            }
        }

        /// <summary>
        /// Create a JWT token based ont he customerID and redirect to PITCH
        /// </summary>
        protected void btnPitch_Click(object sender, EventArgs e)
        {
            var customerId = CustomerID;
            var token = JWTHelper.GenerateToken(customerId);
            var baseUrl = ConfigurationManager.AppSettings["jwtAudience"];
            Response.Redirect($"{baseUrl}/login.aspx?token={token}");
        }

        private void LoadAssociationsAndCatalogs()
        {
            using (var db = new MadduxEntities())
            {
                if (CustomerID != 0)
                {
                    var cst = db.Customers.FirstOrDefault(c => c.CustomerId == CustomerID);
                    var customerAssocs = cst.CustomerAsscs.Select(r => r.Association);
                    var customerAssocIds = customerAssocs.Select(r => r.AssociationID).ToList();
                    var otherAssocs = db.Associations.Where(r => r.Active && customerAssocIds.Contains(r.AssociationID) == false && r.Class != "Star").ToList();
                    lstCurrentAssociations.DataSource = customerAssocs.OrderBy(a => a.Class).ThenBy(a => a.AsscDesc).Select(r => new ListItem
                    {
                        Text = r.AsscDesc,
                        Value = r.AssociationID.ToString()
                    }).ToList();
                    lstCurrentAssociations.DataBind();


                    lstAllAssociations.DataSource = otherAssocs.OrderBy(a => a.Class).ThenBy(a => a.AsscDesc).Select(r => new ListItem
                    {
                        Text = r.AsscDesc,
                        Value = r.AssociationID.ToString()
                    }).ToList();
                    lstAllAssociations.DataBind();

                    var catalogs = db.AssociationCatalogs.Where(r => customerAssocIds.Contains(r.AssociationID)).ToList();
                    var catalogIds = catalogs.Select(c => c.CatalogID).ToList();
                    var activeCatalogs = new List<ListItem>();
                    activeCatalogs = db.ProductCatalogs.Where(x => catalogIds.Contains(x.CatalogId) && x.Active).Select(c => new ListItem
                    {
                        Text = c.CatalogName,
                        Value = c.CatalogId.ToString()
                    }).ToList();
                    //foreach (var item in catalogIds)
                    //{
                    //    var c = db.ProductCatalogs.FirstOrDefault(r => r.CatalogId == item);
                    //    if (c != null)
                    //    {
                    //        activeCatalogs.Add(new ListItem
                    //        {
                    //            Text = c.CatalogName,
                    //            Value = c.CatalogId.ToString()
                    //        });
                    //    }

                    //}

                    lstPrograms.DataSource = activeCatalogs;
                    lstPrograms.DataBind();
                }

            }
        }

        /// <summary>
        /// Populates assigned and available stores list boxes
        /// </summary>
        private void LoadStores()
        {
            try
            {
                using (MadduxEntities db = new MadduxEntities())
                {
                    if (CustomerID != 0)
                    {
                        Customer customer = db.Customers.Find(CustomerID);

                        if (customer != null)
                        {
                            //Stores assinged to the customer
                            var customersSubsIDs = customer.CustomersSubs.Select(cs =>
                            cs.ChildCustomerID
                            ).ToList();

                            //Stores not assgined to the current customer
                            IQueryable<Customer> customers = db.Customers.OrderBy(x => x.Company).Where(c => c.Active && c.CustomerId != CustomerID);

                            gvCustomers.DataSource = customers.Select(c => new CustomerStub()
                            {
                                CustomerId = c.CustomerId,
                                Company = c.Company,
                                Selected = customersSubsIDs.Contains(c.CustomerId)
                            }).ToList();
                            gvCustomers.DataBind();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }

        public class CustomerStub
        {
            public int CustomerId { get; set; }
            public string Company { get; set; }
            public bool Selected { get; set; }
        }

        protected void btnAssociate_Click(object sender, EventArgs e)
        {
            if (lstAllAssociations.SelectedItem != null)
            {
                using (var db = new MadduxEntities())
                {
                    CustomerAssc assoc = new CustomerAssc
                    {
                        CustomerID = CustomerID,
                        AssociationID = int.Parse(lstAllAssociations.SelectedValue)
                    };
                    db.CustomerAsscs.Add(assoc);

                    db.SaveChanges();
                }

                LoadAssociationsAndCatalogs();
            }
        }

        protected void btnUnassociate_Click(object sender, EventArgs e)
        {
            if (lstCurrentAssociations.SelectedItem != null)
            {
                using (var db = new MadduxEntities())
                {
                    var assocId = int.Parse(lstCurrentAssociations.SelectedValue);
                    var assoc = db.CustomerAsscs.FirstOrDefault(a => a.AssociationID == assocId && a.CustomerID == CustomerID);

                    db.CustomerAsscs.Remove(assoc);

                    db.SaveChanges();
                }
                LoadAssociationsAndCatalogs();
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            using (MadduxEntities db = new MadduxEntities())
            {
                var customer = db.Customers.Find(CustomerID);

                if (customer != null)
                {
                    customer.Active = false;
                    //db.Customers.Remove(customer);
                    db.SaveChanges();
                    Response.Redirect("/");
                }
            }
        }

        protected void lnkNewOrder_Click(object sender, EventArgs e)
        {
            Response.Redirect($@"~/order/additem.aspx?CustomerID={CustomerID}");
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

        protected void btnCopyBilling_Click(object sender, EventArgs e)
        {
            txtShippingCompany.Text = txtBillingCompany.Text.ToString();
            txtShippingAddress.Text = txtBillingAddress.Text.ToString();
            txtShippingCity.Text = txtBillingCity.Text.ToString();
            txtShippingPostal.Text = txtBillingPostal.Text.ToString();
            ddShippingProvince.SelectedValue = ddBillingProvince.SelectedValue;
            ddCountry.SelectedValue = ddBillingCountry.SelectedValue;
        }

        protected void gvCustomers_DataBound(object sender, EventArgs e)
        {
            GridView grd = (GridView)sender;

            if (grd.HeaderRow != null)
                grd.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        protected void gvCustomers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int childCustomerID = (int)gvCustomers.DataKeys[e.Row.RowIndex].Value;
                int customerID = int.Parse(Request.QueryString["CustomerID"]);
                HtmlInputCheckBox cbSelected = (HtmlInputCheckBox)e.Row.FindControl("cbSelected");
                CustomerStub stubby = (CustomerStub)e.Row.DataItem;
                cbSelected.Checked = stubby.Selected;
            }
        }

        protected void btnSaveCustomers_Click(object sender, EventArgs e)
        {
            using (MadduxEntities db = new MadduxEntities())
            {
                int customerID = int.Parse(Request.QueryString["CustomerID"]);

                List<int> checkedIDs = new List<int>();
                if (!string.IsNullOrEmpty(hfCheckedIDs.Value))
                {
                    checkedIDs = hfCheckedIDs.Value.Split(',').Select(Int32.Parse).ToList();
                }

                List<int> uncheckedIDs = new List<int>();
                if (!string.IsNullOrEmpty(hfUncheckedIDs.Value))
                {
                    uncheckedIDs = hfUncheckedIDs.Value.Split(',').Select(Int32.Parse).ToList();
                }

                foreach (int childCustomerID in checkedIDs)
                {
                    CustomersSub customersSub = db.CustomersSubs.FirstOrDefault(x => x.MasterCustomerID == customerID && x.ChildCustomerID == childCustomerID);
                    if (customersSub == null)
                    {
                        CustomersSub newCustomerSub = new CustomersSub
                        {
                            MasterCustomerID = CustomerID,
                            ChildCustomerID = childCustomerID
                        };
                        db.CustomersSubs.Add(newCustomerSub);
                    }
                }

                foreach (int childCustomerID in uncheckedIDs)
                {
                    CustomersSub customersSub = db.CustomersSubs.FirstOrDefault(x => x.MasterCustomerID == customerID && x.ChildCustomerID == childCustomerID);
                    if (customersSub != null)
                    {
                        db.CustomersSubs.Remove(customersSub);
                    }
                }
                db.SaveChanges();
            }
        }
    }
}