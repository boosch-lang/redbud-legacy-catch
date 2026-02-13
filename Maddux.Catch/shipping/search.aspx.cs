using Maddux.Catch.LocalClasses;
using Redbud.BL.DL;
using Redbud.BL.Utils;
using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Maddux.Catch.shipping
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
                    AppSession.Current.LastSearchType = "Shipments";
                    AppSession.Current.LastSearchString = SearchCriteria;
                    LoadGrid();

                    this.Title = "Maddux.Catch | Orders";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadGrid()
        {
            try
            {

                using (var db = new MadduxEntities())
                {
                    var user = AppSession.Current.CurrentUser;
                    var shipments = from s in db.vwAllShipments
                                    select s;

                    if (FCSAppUtils.IsNumeric(SearchCriteria))
                    {
                        int searchNum = int.Parse(SearchCriteria);
                        shipments = shipments.Where(r => r.ShipmentID == searchNum || r.OrderId == searchNum);
                    }
                    else
                    {
                        shipments = shipments.Where(r => r.ShippingName.Contains(SearchCriteria));
                    }

                    if (user.ShowOtherMyShipments == false)
                    {
                        shipments = shipments.Where(r => r.SalesPersonID == user.UserID);
                    }

                    dgvShipments.DataSource = shipments.OrderByDescending(x => x.Province).ToList();
                    dgvShipments.DataBind();
                }


                Literal litPageHeader = (Literal)Master.FindControl("litPageHeader");
                Literal litPageSubHeader = (Literal)Master.FindControl("litPageSubHeader");
                litPageHeader.Text = "Shipments";

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
                litTotal.Text = dgvShipments.Rows.Count.ToString() + " record(s) found";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void dgvShipments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
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