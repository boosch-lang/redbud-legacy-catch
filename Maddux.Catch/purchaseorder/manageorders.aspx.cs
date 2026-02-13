using Maddux.Catch.LocalClasses;
using Redbud.BL;
using Redbud.BL.DL;
using Redbud.BL.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Maddux.Catch.purchaseorder
{
    public partial class manageorders : System.Web.UI.Page
    {
        protected int PurchaseOrderID
        {
            get
            {
                if (ViewState["PurchaseOrderID"] == null)
                {
                    ViewState["PurchaseOrderID"] = Request.QueryString["id"] == null || Request.QueryString["id"] == "" ? 0 : (object)Request.QueryString["id"];
                }
                return Convert.ToInt32(ViewState["PurchaseOrderID"].ToString());
            }

            set
            {
                ViewState["PurchaseOrderID"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            if (!IsPostBack)
            {

                LoadFilterDropDowns();
                if (!string.IsNullOrEmpty(Request.QueryString["sd"]) && Request.QueryString["sd"] != "0001-01-01 12:00:00 AM")
                {
                    ddlFilterShipDate.SelectedValue = Request.QueryString["sd"];
                }
                LoadOrders();
            }
        }

        private void LoadFilterDropDowns()
        {
            try
            {
                User currentUser = AppSession.Current.CurrentUser;

                Lookup.LoadProvinceMultiSelect(ref ddlFilterProvince, currentUser.CanOnlyViewAssignedProvinces, false, false, currentUser.UserID);

                //Lookup.LoadCustomerDropDown(ref ddlFilterCustomer);
                Lookup.LoadShipDatesDropDown(ref ddlFilterShipDate);

                using (MadduxEntities madduxEntities = new MadduxEntities())
                {
                    var catalogs = madduxEntities.ProductCatalogs
                        .Where(pc => pc.Active)
                        .Select(pc => new
                        {
                            Program = pc.ProductProgram.ProgramName,
                            CatalogID = pc.CatalogId,
                            pc.CatalogYear,
                            pc.CatalogName
                        })
                        .OrderByDescending(pc => pc.CatalogYear)
                        .ThenBy(pc => pc.Program)
                        .ThenBy(pc => pc.CatalogName)
                        .ToList();

                    ddlFilterCatalog.DataValueField = "CatalogID";
                    ddlFilterCatalog.DataTextField = "CatalogName";
                    ddlFilterCatalog.DataSource = catalogs;
                    ddlFilterCatalog.DataBind();
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }
        private void LoadOrders()
        {
            using (var db = new MadduxEntities())
            {
                User currentUser = AppSession.Current.CurrentUser;
                IQueryable<Order> orders = !currentUser.ShowOtherMyOrders
                    ? db.Orders.Where(r => r.OrderStatus == (int)OrderStatus.Orders && r.SalesPersonID == currentUser.UserID)
                    : db.Orders.Where(r => r.OrderStatus == (int)OrderStatus.Orders);
                orders = orders.Where(r => r.PurchaseOrderID == null && !r.PurchaseOrdersSentDate.HasValue);
                if (orders != null)
                {
                    if (!string.IsNullOrEmpty(ddlFilterShipDate.SelectedValue) && ddlFilterShipDate.SelectedValue != DateTime.MinValue.ToString())
                    {
                        var shipDate = DateTime.Parse(ddlFilterShipDate.SelectedValue);
                        orders = orders.Where(r => r.RequestedShipDate.HasValue && r.RequestedShipDate.Value == shipDate);
                    }

                    List<string> provinces = new List<string>();
                    foreach (var s in ddlFilterProvince.Items.Cast<ListItem>())
                    {
                        if (s.Selected)
                        {
                            provinces.Add(s.Value);
                        }
                    }
                    if (provinces.Count > 0)
                    {
                        orders = orders.Where(r => provinces.Contains(r.Customer.State));
                    }

                    List<int> catalogIds = new List<int>();
                    foreach (var s in ddlFilterCatalog.Items.Cast<ListItem>())
                    {
                        if (s.Selected)
                        {
                            catalogIds.Add(Convert.ToInt32(s.Value));
                        }
                    }
                    if (catalogIds.Count > 0)
                    {
                        orders = orders.Where(r => r.OrderRacks.Any(or => catalogIds.Contains(or.ProductCatalogRack.CatalogID)));
                    }
                    dgvOrders.DataSource = orders.ToList();
                    dgvOrders.DataBind();
                }
            }
        }
        public string GetCount(object quantity, object rackSize, string rackType)
        {
            if (rackSize.ToString() == rackType)
            {
                return quantity.ToString();
            }

            return string.Empty;
        }

        protected void btnSearchOrder_Click(object sender, EventArgs e)
        {
            LoadOrders();
        }

        protected void saveAndClose_Click(object sender, EventArgs e)
        {
            using (var db = new MadduxEntities())
            {
                var purchaseOrder = db.PurchaseOrders.FirstOrDefault(f => f.PurchaseOrderID == PurchaseOrderID);
                foreach (GridViewRow row in dgvOrders.Rows)
                {
                    var selectedCell = row.Cells[0];
                    var checkbox = (CheckBox)selectedCell.FindControl("OrderSelector");
                    if (checkbox.Checked)
                    {
                        var id = int.Parse(((HiddenField)selectedCell.FindControl("OrderID")).Value);
                        var order = db.Orders
                            .Where(o => !o.PurchaseOrderID.HasValue && !o.PurchaseOrdersSentDate.HasValue)
                            .FirstOrDefault(o => o.OrderID == id);

                        if (order != null)
                        {
                            purchaseOrder.Orders.Add(order);
                        }
                    }
                }

                db.SaveChanges();
            }

            CloseWindow(true);
        }
        private void CloseWindow(bool refreshParent)
        {
            Utils util = new Utils();

            util.RegisterStartupScriptBlock(
                "CloseWindow",
                refreshParent ? "window.parent.location.reload();" : "Close();",
                Page);
        }

    }
}