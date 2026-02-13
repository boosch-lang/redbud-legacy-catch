using Maddux.Catch.Helpers;
using Maddux.Catch.LocalClasses;
using Redbud.BL.DL;
using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Maddux.Catch.customer
{
    public partial class associations : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Response.Cache.SetCacheability(HttpCacheability.NoCache);

                if (!Page.IsPostBack)
                {
                    LoadFilterDropDowns();
                    LoadGrid();

                    this.Title = "Maddux | Associations";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadFilterDropDowns()
        {
            try
            {
                var regions = RegionHelper.GetRegionsForSelectList(true);

                ddlFilterRegion.DataTextField = "Text";
                ddlFilterRegion.DataValueField = "Value";
                ddlFilterRegion.DataSource = regions;
                ddlFilterRegion.DataBind();
                
                if (AppSession.Current.LastAssociationRegionFilter != "")
                {
                    ddlFilterRegion.SelectedValue = AppSession.Current.LastAssociationRegionFilter;
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

                    var region = ddlFilterRegion.SelectedValue.ToString();
                    var user = AppSession.Current.CurrentUser;

                    if (!user.CanDeleteCustomers || !user.CanDeleteOrders)
                    {
                        btnCreateAssociation.Visible = false;
                    }

                    var associations = from a in db.vwCustomersByAssociations
                              select a;

                    if (user.CanOnlyViewAssignedProvinces)
                    {
                        var states = db.UserStates.Where(r => r.UserID == user.UserID).Select(r => r.StateID);
                        var userCustomers = db.Customers.Where(r => states.Contains(r.State)).Select(r => r.CustomerId);
                        associations = associations.Where(r => userCustomers.Contains(r.CustomerID));
                    }

                    if (region != "All")
                    {
                        var fsaList = db.FreightCharges.Where(x => x.Region == region).Select(x => x.AreaID).ToList();
                        var customers = db.Customers.Where(r => !string.IsNullOrEmpty(r.Zip) && fsaList.Contains(r.Zip.Substring(0, 3))).Select(r => r.CustomerId);
                        associations = associations.Where(r => customers.Contains(r.CustomerID));
                    }                   

                    if (user.CanOnlyViewAssignedAssociations)
                    {
                        var asscs = db.UserAsscs.Where(r => r.UserID == user.UserID).Select(r => r.AssociationID);
                        associations = associations.Where(r => asscs.Contains(r.AssociationID));
                    }

                    if (user.CanOnlyViewOwnCustomers)
                    {
                        var customers = db.Customers.Where(r => r.SalesPersonID == user.UserID).Select(r => r.CustomerId);
                        associations = associations.Where(r => customers.Contains(r.CustomerID));
                    }

                    var results = associations.ToList().OrderBy(a => a.Class).ThenBy(a => a.AsscDesc).GroupBy(a => a.AsscDesc).Select(x => new
                    {
                        AsscDesc = x.Key,
                        CountCustomerID = x.Count(),
                        AssociationId = x.Select(c => c.AssociationID).FirstOrDefault(),
                        Class = x.Select(c => c.Class).FirstOrDefault(),
                        BlankCol = "",
                        CustomerID = x.Select(c => c.CustomerID).First()
                    });

                    dgvAssociations.DataSource = results.ToList();
                    dgvAssociations.DataBind();

                    //Customers without associations
                    var customersWithoutAssociations = db.Customers.Where(c => !c.CustomerAsscs.Any());
                    dgvCustomers.DataSource = customersWithoutAssociations.ToList();
                    dgvCustomers.DataBind();
                }

                Literal litPageHeader = (Literal)Master.FindControl("litPageHeader");
                litPageHeader.Text = "Associations";
                Literal litTotal = (Literal)Master.FindControl("litTotal");
                litTotal.Text = dgvAssociations.Rows.Count.ToString() + " record(s) found";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void dgvAssociations_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            //try {
            //    if (e.Row.RowType == DataControlRowType.DataRow)
            //    {
            //        GridViewRow gvr = e.Row;
            //        var drv = (dynamic)gvr.DataItem;
            //        //    //DataRow dr = drv.Row;

            //        HyperLink hypAssociation = (HyperLink)gvr.FindControl("hypAssociation");
            //        hypAssociation.Text = drv.AsscDesc.ToString();
            //        hypAssociation.NavigateUrl = ResolveUrl("~/customer/search.aspx?asscid="  + drv.AssociationId + "&province=" + ddlFilterProvince.SelectedValue.ToString());
            //    }
            //    else
            //    {
            //        if (e.Row.RowType == DataControlRowType.Footer)
            //        {
            //            e.Row.TableSection = TableRowSection.TableFooter;
            //        }
            //        else
            //        {
            //            if (e.Row.RowType == DataControlRowType.Header)
            //            {
            //                e.Row.TableSection = TableRowSection.TableHeader;
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex) {
            //    throw ex;
            //}
        }

        protected void ddlFilterRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGrid();
            AppSession.Current.LastAssociationRegionFilter = ddlFilterRegion.SelectedValue.ToString();
        }

        protected void dgvAssociations_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvAssociations.PageIndex = e.NewPageIndex;
            LoadGrid();
            dgvAssociations.DataBind();
        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {

        }

        protected void AssociationRedirect_Click(object sender, EventArgs e)
        {
            //associationdetail.aspx?aID=
            LinkButton button = (LinkButton)sender;
            string associtionID = button.CommandArgument;
            Response.Redirect($"~/customer/associationdetail.aspx?aID={associtionID}&region={ddlFilterRegion.SelectedValue}");
        }
    }
}