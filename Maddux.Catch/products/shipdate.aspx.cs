using Maddux.Catch.LocalClasses;
using Redbud.BL.DL;
using Redbud.BL.Utils;
using System;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Maddux.Catch.products
{
    public partial class shipdate : System.Web.UI.Page
    {
        private int ProgramID
        {
            get
            {
                if (ViewState["ProgramId"] == null)
                {
                    ViewState["ProgramId"] = Request.QueryString["ProgramId"] == null || Request.QueryString["ProgramId"] == "" ? 0 : (object)Request.QueryString["ProgramId"];
                }
                return Convert.ToInt32(ViewState["ProgramId"].ToString());
            }

            set
            {
                ViewState["ProgramId"] = value;
            }
        }
        private int CatalogID
        {
            get
            {
                if (ViewState["CatalogId"] == null)
                {
                    ViewState["CatalogId"] = Request.QueryString["CatalogId"] == null || Request.QueryString["CatalogId"] == "" ? 0 : (object)Request.QueryString["CatalogId"];
                }
                return Convert.ToInt32(ViewState["CatalogId"].ToString());
            }
        }
        private int ShipDateID
        {
            get
            {
                if (ViewState["ShipDateId"] == null)
                {
                    ViewState["ShipDateId"] = Request.QueryString["id"] == null || Request.QueryString["id"] == "" ? 0 : (object)Request.QueryString["id"];
                }
                return Convert.ToInt32(ViewState["ShipDateId"].ToString());
            }

            set
            {
                ViewState["ShipDateId"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            var shipdate = new Redbud.BL.DL.ProductCatalogShipDate();
            if (!IsPostBack)
            {
                using (var db = new MadduxEntities())
                {
                    if (ShipDateID != 0)
                    {
                        shipdate = db.ProductCatalogShipDates.FirstOrDefault(r => r.ShipDateID == ShipDateID);
                        txtShipDate.Text = shipdate.ShipDate.ToString("MMMM dd, yyyy");
                        if (shipdate.OrderDeadlineDate.HasValue)
                            txtOrderDeadline.Text = shipdate.OrderDeadlineDate.Value.ToString("MMMM dd, yyyy");
                        delete.Visible = true;
                    }
                    else
                    {
                        delete.Visible = false;
                        shipdate.CatalogID = CatalogID;
                    }
                }
            }

        }

        protected void saveAndClose_Click(object sender, EventArgs e)
        {
            Save();
            CloseWindow(true);

        }

        private bool Save()
        {
            if (Page.IsValid)
            {
                using (var db = new MadduxEntities())
                {
                    try
                    {
                        Redbud.BL.DL.ProductCatalogShipDate shipDate;
                        if (ShipDateID == 0)
                        {
                            shipDate = new ProductCatalogShipDate();
                            shipDate.CatalogID = CatalogID;
                            db.ProductCatalogShipDates.Add(shipDate);

                        }
                        else
                        {
                            shipDate = db.ProductCatalogShipDates.Find(ShipDateID);
                        }

                        if (shipDate != null)
                        {
                            var dt = DateTime.Parse(txtShipDate.Text);
                            var deadline = DateTime.Parse(txtOrderDeadline.Text);

                            //get all the racks for the catalog id
                            var racks = db.ProductCatalogRacks
                                            .Where(x => x.CatalogID == CatalogID)
                                            .ToList();

                            foreach(var rack in racks)
                            {
                                var rackShipDate = rack.ProductRackShipDates.Where(x => x.ShipDate == shipDate.ShipDate).FirstOrDefault();
                                if(rackShipDate != null)
                                {
                                    rackShipDate.ShipDate = dt;
                                    rackShipDate.OrderDeadlineDate = deadline;
                                }
                            }

                            shipDate.ShipDate = dt;
                            shipDate.OrderDeadlineDate = deadline;
                            var results = db.SaveChanges() == 1;

                            ShipDateID = shipDate.ShipDateID;
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        litMessage.Text = StringTools.GenerateError(ex.Message);
                        return false;
                    }
                }
            }
            return false;
        }

        private void CloseWindow(Boolean RefreshParent)
        {
            Utils util = new Utils();

            if (RefreshParent)
            {
                util.RegisterStartupScriptBlock("CloseWindow", "window.parent.location.reload();", Page);
            }
            else
            {
                util.RegisterStartupScriptBlock("CloseWindow", "Close();", Page);
            }
        }

        protected void delete_Click(object sender, EventArgs e)
        {
            using (var db = new MadduxEntities())
            {
                var shipdate = db.ProductCatalogShipDates.Find(ShipDateID);
                db.ProductCatalogShipDates.Remove(shipdate);
                db.SaveChanges();
            }
            CloseWindow(true);
        }
    }
}