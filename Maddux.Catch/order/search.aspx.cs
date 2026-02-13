using Maddux.Catch.LocalClasses;
using Redbud.BL.DL;
using Redbud.BL.Utils;
using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Maddux.Catch.order
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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Response.Cache.SetCacheability(HttpCacheability.NoCache);

                if (!Page.IsPostBack)
                {
                    AppSession.Current.LastSearchType = "Orders";
                    AppSession.Current.LastSearchString = SearchCriteria;
                    LoadGrid();

                    this.Title = "Maddux.Catch | Orders";
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }

        private void LoadGrid()
        {
            try
            {
                using (var db = new MadduxEntities())
                {
                    var user = AppSession.Current.CurrentUser;
                    var orders = from o in db.vwAllOrders
                                 select o;


                    if (FCSAppUtils.IsNumeric(SearchCriteria))
                    {
                        int searchNum = int.Parse(SearchCriteria);
                        orders = orders.Where(r => r.OrderID == searchNum);
                    }
                    else
                    {
                        orders = orders.Where(r => (r.Company.Contains(SearchCriteria) || r.Email.Contains(SearchCriteria)));
                    }


                    if (!user.ShowOtherMyOrders)
                    {
                        orders.Where(r => r.SalesPersonID == user.UserID);
                    }
                    dgvOrders.DataSource = orders.OrderByDescending(x => x.OrderID).ToList();
                    dgvOrders.DataBind();
                }



                Literal litPageHeader = (Literal)Master.FindControl("litPageHeader");
                Literal litPageSubHeader = (Literal)Master.FindControl("litPageSubHeader");
                litPageHeader.Text = "Orders";

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
                litTotal.Text = dgvOrders.Rows.Count.ToString() + " record(s) found";
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }

        protected void dgvOrders_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    GridViewRow gvr = e.Row;
                    vwAllOrder drv = (vwAllOrder)gvr.DataItem;

                    HyperLink hypEmail = (HyperLink)gvr.FindControl("hypEmail");

                    if (!string.IsNullOrWhiteSpace(drv.Email))
                    {
                        hypEmail.Visible = true;
                        hypEmail.Text = drv.Email;
                        hypEmail.NavigateUrl = "mailto:" + drv.Email.Trim();
                    }
                    else
                    {
                        hypEmail.Visible = false;
                        hypEmail.NavigateUrl = "";
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