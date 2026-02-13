using Maddux.Catch.Helpers;
using Maddux.Catch.LocalClasses;
using Redbud.BL.DL;
using Redbud.BL.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Web;
using System.Web.UI.WebControls;

namespace Maddux.Catch.customer
{
    public partial class customerlist : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            try
            {
                if (!Page.IsPostBack)
                {
                    Literal litPageHeader = (Literal)Master.FindControl("litPageHeader");
                    litPageHeader.Text = "Customer List";
                    LoadFilters();

                    //we don't load the grid until the user clicks search
                    
                    this.Title = "Maddux | Active Customers";

                }
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }

        private void LoadGrid()
        {
            using (MadduxEntities db = new MadduxEntities())
            {
                //load customers
                var user = AppSession.Current.CurrentUser;
                var query = db.Customers.Where(x => x.Active);

                //handle user permissions
                if (user.CanOnlyViewAssignedProvinces)
                {
                    var states = db.UserStates.Where(r => r.UserID == user.UserID).Select(r => r.StateID);
                    query = query.Where(r => states.Contains(r.State));
                }
                if (user.CanOnlyViewAssignedAssociations)
                {
                    var asscs = db.UserAsscs.Where(r => r.UserID == user.UserID).Select(r => r.AssociationID);
                    query = query.Where(x => x.CustomerAsscs.Any(a => asscs.Contains(a.AssociationID)));
                }
                if (user.CanOnlyViewOwnCustomers)
                {
                    query = query.Where(r => r.SalesPersonID == user.UserID);

                }

                //apply filters

                //associations
                var selectedAssociations = new List<int>();
                foreach (var i in ddlFilterMembership.GetSelectedIndices())
                {
                    selectedAssociations.Add(int.Parse(ddlFilterMembership.Items[i].Value));
                }
                if (selectedAssociations.Any())
                {
                    query = query.Where(x => x.CustomerAsscs.Any(a => selectedAssociations.Contains(a.AssociationID)));
                }

                //regions
                var selectedRegions = new List<string>();
                foreach (var i in ddlFilterRegion.GetSelectedIndices())
                {
                    selectedRegions.Add(ddlFilterRegion.Items[i].Value.ToString());
                }
                if (selectedRegions.Any())
                {
                    var fsa = db.FreightCharges.Where(x => selectedRegions.Contains(x.Region)).Select(x => x.AreaID).ToList();
                    query = query.Where(x => !string.IsNullOrEmpty(x.Zip) && fsa.Contains(x.Zip.Substring(0, 3)));
                }

                //star rating
                var selectedRatings = new List<int>();
                foreach (var i in ddlFilterStarRating.GetSelectedIndices())
                {
                    selectedRatings.Add(int.Parse(ddlFilterStarRating.Items[i].Value));
                }
                if (selectedRatings.Any())
                {
                    var filterStars = selectedRatings.Where(x => x != 999).ToList();
                    var includeNull = selectedRatings.Any(x => x == 999);

                    if (includeNull)
                    {
                        query = query.Where(x => !x.StarRating.HasValue || filterStars.Contains(x.StarRating.Value));
                    }
                    else
                    {
                        query = query.Where(x => x.StarRating.HasValue && filterStars.Contains(x.StarRating.Value));
                    }
                }

                var result = query.OrderBy(x => x.Company).ToList();

                dgvCustomers.DataSource = result;
                dgvCustomers.DataBind();

                Literal litTotal = (Literal)Master.FindControl("litTotal");
                litTotal.Text = dgvCustomers.Rows.Count.ToString() + " customer(s) found";

            }
        }

        private void LoadFilters()
        {
            try
            {
                LoadAssociations();
                LoadRegions();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadRegions()
        {
            var regions = RegionHelper.GetRegionsForSelectList(false);
            ddlFilterRegion.DataTextField = "Text";
            ddlFilterRegion.DataValueField = "Value";
            ddlFilterRegion.DataSource = regions;
            ddlFilterRegion.DataBind();

        }

        private void LoadAssociations()
        {
            using (var db = new MadduxEntities())
            {
                var user = AppSession.Current.CurrentUser;

                var query = db.vwCustomersByAssociations.AsQueryable<vwCustomersByAssociation>();

                if (user.CanOnlyViewAssignedProvinces)
                {
                    var states = db.UserStates.Where(r => r.UserID == user.UserID).Select(r => r.StateID);
                    var userCustomers = db.Customers.Where(r => states.Contains(r.State)).Select(r => r.CustomerId);
                    query = query.Where(r => userCustomers.Contains(r.CustomerID));
                }

                if (user.CanOnlyViewAssignedAssociations)
                {
                    var asscs = db.UserAsscs.Where(r => r.UserID == user.UserID).Select(r => r.AssociationID);
                    query = query.Where(x => asscs.Contains(x.AssociationID));
                }

                if (user.CanOnlyViewOwnCustomers)
                {
                    var customers = db.Customers.Where(r => r.SalesPersonID == user.UserID).Select(r => r.CustomerId);
                    query = query.Where(r => customers.Contains(r.CustomerID));
                }

                var associations = query
                    .OrderBy(a => a.Class)
                    .ThenBy(a => a.AsscDesc)
                    .GroupBy(a => a.AsscDesc)
                    .Select(x => new
                    {
                        Name = x.Key,
                        AssociationId = x.Select(c => c.AssociationID).FirstOrDefault(),
                    })
                    .OrderBy(x => x.Name)
                    .ToList();

                ddlFilterMembership.DataSource = associations;
                ddlFilterMembership.DataTextField = "Name";
                ddlFilterMembership.DataValueField = "AssociationId";
                ddlFilterMembership.DataBind();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGrid();
        }
    }
}