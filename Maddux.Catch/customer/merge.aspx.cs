using Redbud.BL.DL;
using Redbud.BL.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Maddux.Catch.customer
{
    public partial class merge : Page
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

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            if (!Page.IsPostBack)
            {
                if (CustomerID == 0)
                {
                }
                else
                {
                    using (var db = new MadduxEntities())
                    {
                        Customer customer = db.Customers.Find(CustomerID);
                        lnkCompany.Text = customer.Company;
                        lnkCompany.NavigateUrl = "/customer/customerdetail.aspx?id=" + CustomerID;

                        var contactName = string.Format("{0} {1}", customer.FirstName, customer.LastName);
                        if (string.IsNullOrWhiteSpace(customer.Email))
                        {
                            lnkContact.Visible = false;
                            lblContact.Text = "(no email)";
                        }
                        else
                        {
                            lnkContact.Visible = false;
                            lnkContact.Text = contactName;
                            lnkContact.NavigateUrl = "mailto:" + customer.Email;
                            lnkContact.Visible = true;
                        }

                        lblAddress.Text = customer.ShippingAddressComplete;
                        lblCustomerPhone.Text = customer.Phone;
                        txtSearch.Text = customer.Company;
                    }
                    LoadGrid();
                }
            }
        }

        private void LoadGrid()
        {

            if (!string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                string searchCriteria = txtSearch.Text.Trim();

                using (var db = new MadduxEntities())
                {
                    var qry = db.Customers.Where(r => r.CustomerId != CustomerID && (r.Company.Contains(searchCriteria)
                        || r.FirstName.Contains(searchCriteria)
                        || r.LastName.Contains(searchCriteria)
                        || (r.FirstName + " " + r.LastName).Contains(searchCriteria)
                        || r.Address.Contains(searchCriteria)
                        || r.City.Contains(searchCriteria)
                        || r.State.Contains(searchCriteria)
                        || r.Zip.Replace(" ", "").Contains(searchCriteria.Replace("", ""))
                        || r.Country.Contains(searchCriteria)
                        || r.Phone.Contains(searchCriteria)
                        || r.CellPhone.Contains(searchCriteria)));

                    qry = qry.OrderBy(r => r.Company).ThenBy(r => r.FirstName).ThenBy(r => r.LastName);
                    var results = qry.ToList();
                    grdMergeItems.DataSource = results;
                    btnMerge.Visible = results.Count > 0;
                    lblRecordCount.Text = string.Format("{0} record(s) found", results.Count);
                    lblRecordCount.Visible = results.Count > 0;
                }
            }
            else
            {
                grdMergeItems.DataSource = null;
                btnMerge.Visible = false;
                lblRecordCount.Visible = false;
            }
            grdMergeItems.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGrid();
        }

        protected void btnMerge_Click(object sender, EventArgs e)
        {
            using (var db = new MadduxEntities())
            {
                Customer primaryCustomer = db.Customers.Find(CustomerID);
                foreach (GridViewRow row in grdMergeItems.Rows)
                {
                    var selectedCell = row.Cells[0];
                    var checkbox = (CheckBox)selectedCell.FindControl("chkCompanySelect");

                    if (checkbox.Checked)
                    {
                        var customerIdToMerge = Convert.ToInt32(grdMergeItems.DataKeys[row.RowIndex].Values[0]);
                        Customer custToMerge = db.Customers.Find(customerIdToMerge);
                        if (custToMerge != null)
                        {
                            foreach (var contact in custToMerge.Contacts)
                            {
                                contact.CustomerID = primaryCustomer.CustomerId;
                            }
                            foreach (var credit in custToMerge.Credits)
                            {
                                credit.CustomerID = primaryCustomer.CustomerId;
                            }
                            foreach (var a in custToMerge.CustomerAsscs)
                            {
                                primaryCustomer.CustomerAsscs.Add(new CustomerAssc
                                {
                                    CustomerID = primaryCustomer.CustomerId,
                                    AssociationID = a.AssociationID
                                });
                            }
                            foreach (var journal in custToMerge.Journals)
                            {
                                journal.CustomerID = primaryCustomer.CustomerId;
                            }
                            List<int> orderIds = new List<int>();
                            foreach (var o in custToMerge.Orders)
                            {
                                o.Customer = primaryCustomer;
                                orderIds.Add(o.OrderID);
                            }

                            db.Customers.Remove(custToMerge);
                        }
                    }
                }
                db.SaveChanges();
            }
        }

        protected void grdMergeItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    GridViewRow gvr = e.Row;
                    Customer drv = (Customer)gvr.DataItem;

                    HyperLink hypContact = (HyperLink)gvr.FindControl("hypContact");
                    Label lblContact = (Label)gvr.FindControl("lblContact");

                    if (!string.IsNullOrWhiteSpace(drv.Email))
                    {
                        hypContact.Visible = true;
                        lblContact.Visible = false;
                        hypContact.Text = drv.Contact;
                        hypContact.NavigateUrl = "mailto:" + drv.Email.Trim();
                    }
                    else
                    {
                        lblContact.Visible = true;
                        lblContact.Text = drv.Contact;
                        hypContact.Visible = false;
                        hypContact.NavigateUrl = "";
                    }
                }
                else
                {
                    if (e.Row.RowType == DataControlRowType.Footer)
                    {
                        e.Row.TableSection = TableRowSection.TableFooter;
                    }
                    else
                    {
                        if (e.Row.RowType == DataControlRowType.Header)
                        {
                            e.Row.TableSection = TableRowSection.TableHeader;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }
    }
}