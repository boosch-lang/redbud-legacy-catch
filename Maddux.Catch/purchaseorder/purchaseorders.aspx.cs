using Redbud.BL.DL;
using Redbud.BL.Utils;
using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Maddux.Catch.purchaseorders
{
    public partial class purchaseorders : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            if (!Page.IsPostBack)
            {
                LoadFilterDropDowns();
                LoadGrid();
            }

            Title = "Maddux | Purchase Orders";
        }

        private void LoadFilterDropDowns()
        {
            try
            {
                Lookup.LoadDeliveryHubDropdown(ref ddlFilterHub);
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
                    int selectedHub = int.Parse(ddlFilterHub.SelectedValue.ToString());

                    var qry = from po in db.PurchaseOrders
                              select po;

                    if (selectedHub > 0)
                    {
                        qry = qry.Where(po => po.DeliveryHubID == selectedHub);
                    }

                    dgvPurchaseOrders.DataSource = qry
                        .OrderByDescending(p => p.ShipDate).ThenByDescending(p => p.PurchaseOrderID)
                        .ToList();

                    dgvPurchaseOrders.DataBind();

                }
                Literal litPageHeader = (Literal)Master.FindControl("litPageHeader");
                litPageHeader.Text = "Purchase Orders";

            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }

        protected void dgvPurchaseOrders_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvPurchaseOrders.PageIndex = e.NewPageIndex;
            LoadGrid();
            dgvPurchaseOrders.DataBind();
        }

        protected void ddlFilterHub_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGrid();
        }
    }
}