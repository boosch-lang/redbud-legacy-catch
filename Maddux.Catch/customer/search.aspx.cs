using Maddux.Catch.LocalClasses;
using Redbud.BL.DL;
using Redbud.BL.Utils;
using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Maddux.Catch.customer
{

    public partial class search : System.Web.UI.Page
    {
        private string SearchCriteria
        {
            get
            {
                if (ViewState["SearchCriteria"] == null)
                {
                    ViewState["SearchCriteria"] = Request.QueryString["q"] == null || Request.QueryString["q"] == "" ? "" : Request.QueryString["q"];
                }
                return HttpUtility.UrlDecode(ViewState["SearchCriteria"].ToString());
            }

            set
            {
                ViewState["SearchCriteria"] = value;
            }
        }

        private int AssociationID
        {
            get
            {
                if (ViewState["AssociationID"] == null)
                {
                    ViewState["AssociationID"] = Request.QueryString["asscid"] == null || Request.QueryString["asscid"] == "" ? -1 : (object)Request.QueryString["asscid"];
                }
                return Convert.ToInt32(ViewState["AssociationID"].ToString());
            }

            set
            {
                ViewState["AssociationID"] = value;
            }
        }

        private string ProvinceID
        {
            get
            {
                if (ViewState["ProvinceID"] == null)
                {
                    ViewState["ProvinceID"] = Request.QueryString["province"] == null || Request.QueryString["q"] == "province" ? "00" : Request.QueryString["province"];
                }
                return ViewState["ProvinceID"].ToString();
            }

            set
            {
                ViewState["ProvinceID"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Response.Cache.SetCacheability(HttpCacheability.NoCache);

                if (!Page.IsPostBack)
                {
                    AppSession.Current.LastSearchType = "Customers";
                    AppSession.Current.LastSearchString = SearchCriteria;
                    LoadGrid();

                    this.Title = "Maddux.Catch | Customers";
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }

        private void LoadGrid()
        {
            string filterDesc = "";

            try
            {
                using (var db = new MadduxEntities())
                {
                    var user = AppSession.Current.CurrentUser;

                    var customers = from c in db.Customers
                                    select c;

                    if (SearchCriteria.Length > 0)
                    {
                        customers = customers.Where(
                            r =>
                                (r.Company.Contains(SearchCriteria) ||
                                 r.FirstName.Contains(SearchCriteria) ||
                                 r.LastName.Contains(SearchCriteria) ||
                                 (r.FirstName + " " + r.LastName).Contains(SearchCriteria) ||
                                 r.Address.Contains(SearchCriteria) ||
                                 r.City.Contains(SearchCriteria) ||
                                 r.State.Contains(SearchCriteria) ||
                                 r.Zip.Replace(" ", "").Contains(SearchCriteria.Replace("", "")) ||
                                 r.Country.Contains(SearchCriteria) ||
                                 r.Phone.Contains(SearchCriteria) ||
                                 r.CellPhone.Contains(SearchCriteria) ||
                                 r.Email.Contains(SearchCriteria) ||
                                 r.AlternateEmail.Contains(SearchCriteria) ||
                                 r.InvoiceEmail.Contains(SearchCriteria)));
                    }

                    if (user.CanOnlyViewOwnCustomers)
                    {
                        customers = customers.Where(r => r.SalesPersonID == user.UserID || r.SalesPersonID == 0);
                    }

                    if (AssociationID != -1)
                    {
                        var associations = db.CustomerAsscs.Where(r => r.AssociationID == AssociationID).Select(r => r.CustomerID);
                        customers = customers.Where(r => associations.Contains(r.CustomerId));
                    }
                    else
                    {
                        if (user.CanOnlyViewAssignedAssociations)
                        {
                            var associations = db.UserAsscs.Where(r => r.UserID == user.UserID).Select(r => r.AssociationID);
                            var custAsscs = db.CustomerAsscs.Where(r => associations.Contains(r.AssociationID)).Select(r => r.CustomerID);
                            customers = customers.Where(r => custAsscs.Contains(r.CustomerId));
                        }
                    }

                    if (ProvinceID != "00")
                    {
                        customers = customers.Where(r => r.State == ProvinceID);
                    }
                    else
                    {
                        if (user.CanOnlyViewAssignedProvinces)
                        {
                            var states = db.UserStates.Where(r => r.UserID == user.UserID).Select(r => r.StateID);
                            customers = customers.Where(r => states.Contains(r.State));
                        }
                    }
                    customers = customers.Where(x=>x.Active).OrderBy(c => c.Company);

                    dgvCustomers.DataSource = customers.ToList();
                    dgvCustomers.DataBind();

                    Literal litPageHeader = (Literal)Master.FindControl("litPageHeader");
                    Literal litPageSubHeader = (Literal)Master.FindControl("litPageSubHeader");
                    litPageHeader.Text = "Customers";

                    Association selectedAssociation = db.Associations.FirstOrDefault(r => r.AssociationID == AssociationID);
                    if (selectedAssociation != null)
                    {
                        filterDesc = selectedAssociation.AsscDesc;
                    }

                    var selectedProvince = db.States.FirstOrDefault(r => r.StateID == ProvinceID);
                    if (selectedProvince != null)
                    {
                        if (filterDesc.Length > 0)
                        {
                            filterDesc += " (" + selectedProvince.StateName + ")";
                        }
                        else
                        {
                            filterDesc = selectedProvince.StateName;
                        }
                    }

                    litPageSubHeader.Text = filterDesc.Length > 0 ? "Current filter: " + filterDesc : "";

                    if (SearchCriteria.Length > 0)
                    {
                        if (litPageSubHeader.Text.Length > 0)
                        {
                            litPageSubHeader.Text += "; Search criteria: " + SearchCriteria;
                        }
                        else
                        {
                            litPageSubHeader.Text = "Search criteria: " + SearchCriteria;
                        }
                    }

                    Literal litTotal = (Literal)Master.FindControl("litTotal");
                    litTotal.Text = dgvCustomers.Rows.Count.ToString() + " record(s) found";
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void dgvCustomers_RowDataBound(object sender, GridViewRowEventArgs e)
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

                    HyperLink hypJournal = (HyperLink)gvr.FindControl("hypJournal");

                    if (drv.JournalID > 0)
                    {
                        hypJournal.Visible = true;
                        hypJournal.NavigateUrl = "/journal/journaldetail.aspx?id=" + drv.JournalID.ToString();
                    }
                    else
                    {
                        hypJournal.Visible = false;
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
                throw ex;
            }
        }
    }
}