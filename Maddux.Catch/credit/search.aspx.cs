using Maddux.Catch.LocalClasses;
using Redbud.BL.DL;
using Redbud.BL.Utils;
using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Maddux.Catch.credit
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
                    AppSession.Current.LastSearchType = "Credits";
                    AppSession.Current.LastSearchString = SearchCriteria;
                    LoadGrid();

                    this.Title = "Maddux.Catch | Credit Memos";
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
                    var credits = from c in db.vwAllCredits
                                  select c;
                    if (FCSAppUtils.IsNumeric(SearchCriteria))
                    {
                        int searchNum = int.Parse(SearchCriteria);
                        credits = credits.Where(r => r.CreditID == searchNum);
                    }
                    else
                    {
                        credits = credits.Where(r => r.CustomerName.Contains(SearchCriteria));
                    }
                    if (user.ShowOtherMyOrders == false)
                    {
                        credits = credits.Where(r => r.SalesPersonID == user.UserID);
                    }
                    dgvCredits.DataSource = credits.ToList();
                    dgvCredits.DataBind();
                }



                Literal litPageHeader = (Literal)Master.FindControl("litPageHeader");
                Literal litPageSubHeader = (Literal)Master.FindControl("litPageSubHeader");
                litPageHeader.Text = "Credit Memos";

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
                litTotal.Text = dgvCredits.Rows.Count.ToString() + " record(s) found";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void dgvCredits_RowDataBound(object sender, GridViewRowEventArgs e)
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