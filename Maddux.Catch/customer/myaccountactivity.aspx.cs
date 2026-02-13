using Redbud.BL.DL;
using Redbud.BL.Utils;
using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Maddux.Catch.customer
{
    public partial class myaccountactivity : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            if (!Page.IsPostBack)
            {
                LoadGrid();
            }
        }

        private void LoadGrid()
        {
            try
            {
                using (var db = new MadduxEntities())
                {
                    var activity = db.vwMyAccountActivityLogs.OrderByDescending(a => a.ActivityDate).Take(500).ToList();

                    gridActivity.DataSource = activity;
                    gridActivity.DataBind();

                    Literal litPageHeader = (Literal)Master.FindControl("litPageHeader");
                    litPageHeader.Text = "Account Activity";
                    Literal litTotal = (Literal)Master.FindControl("litTotal");
                    litTotal.Text = string.Format("{0} record(s) shown", activity.Count);
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }

        protected void gridActivity_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridActivity.PageIndex = e.NewPageIndex;
            LoadGrid();
            gridActivity.DataBind();
        }
    }
}