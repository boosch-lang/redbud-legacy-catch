using Maddux.Catch.LocalClasses;
using Redbud.BL.DL;
using Redbud.BL.Utils;
using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Maddux.Catch.Journal
{
    public partial class myfollowups : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            if (!Page.IsPostBack)
            {
                LoadFilterDropDowns();
                LoadGrid();
            }

            this.Title = "Maddux.Catch | My Followups";
        }

        private void LoadFilterDropDowns()
        {
            try
            {
                User currentUser = AppSession.Current.CurrentUser;
                Lookup.LoadUserDropDown(ref ddlFilterUser, true);
                Lookup.LoadProvinceDropDown(ref ddlFilterProvince, currentUser.CanOnlyViewAssignedProvinces, true, false, currentUser.UserID);
                Lookup.LoadAssociationDropDown(ref ddlFilterAssociation, currentUser.CanOnlyViewAssignedAssociations, true, currentUser.UserID);
                //Lookup.LoadCampaignDropDown(ref ddlCampaigns, true);
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
                    string province = ddlFilterProvince.SelectedValue.ToString();
                    int userId = Convert.ToInt32(ddlFilterUser.SelectedValue);
                    int associationId = Convert.ToInt32(ddlFilterAssociation.SelectedValue);
                    string selectedRating = ddlRating.SelectedValue.ToString();
                    //int selectedCampaign = int.Parse(ddlCampaigns.SelectedValue.ToString());

                    IQueryable<vwMyFollowup> journals = from j in db.vwMyFollowups
                                                        select j;


                    if(userId != -1)
                    {
                        journals = journals.Where(j => j.AssignedToId == userId);
                    }
                    if (province != "00")
                    {
                        journals = journals.Where(r => r.State == province);
                    }
                    if (associationId != -1)
                    {
                        var associations = db.vwCustomersByAssociations.Where(r => r.AssociationID == associationId).Select(r => r.CustomerID).ToList();
                        journals = journals.Where(r => associations.Contains(r.CustomerId));
                    }
                    if (!string.IsNullOrEmpty(selectedRating))
                    {
                        int rating = int.Parse(selectedRating);
                        var query = db.Customers.AsEnumerable();
                        if (rating == 999)
                        {
                            query = query.Where(x => !x.StarRating.HasValue);
                        } else
                        {
                            query = query.Where(x => x.StarRating.HasValue && x.StarRating == rating);
                        }
                        var customers = query.Select(c => c.CustomerId);
                        journals = journals.Where(r => customers.Contains(r.CustomerId));
                    }
                    /*
                    if (selectedCampaign > 0)
                    {
                        var camaignJournals = db.Journals.Where(j => j.IsResolved == false && j.Campaigns.Any(c => c.CampaignID == selectedCampaign)).Select(c => c.JournalID).ToList();
                        journals = journals.Where(j => camaignJournals.Contains(j.JournalID));
                    }
                    */
                    var user = AppSession.Current.CurrentUser;

                    if (user.CanOnlyViewAssignedAssociations)
                    {
                        var asscs = db.UserAsscs.Where(r => r.UserID == user.UserID).Select(r => r.AssociationID);
                        var associations = db.vwCustomersByAssociations.Where(r => asscs.Contains(r.AssociationID)).Select(r => r.CustomerID).ToList();
                        journals = journals.Where(r => associations.Contains(r.CustomerId));
                    }
                    else
                    {
                        if (user.CanOnlyViewAssignedProvinces)
                        {
                            var states = db.UserStates.Where(r => r.UserID == user.UserID).Select(r => r.StateID);
                            var customers = db.Customers.Where(r => states.Contains(r.State)).Select(r => r.CustomerId);
                            var associations = db.vwCustomersByAssociations.Where(r => customers.Contains(r.CustomerID)).Select(r => r.CustomerID).ToList();
                            journals = journals.Where(r => associations.ToList().Contains(r.CustomerId));
                        }
                    }
                    journals = AppSession.Current.CurrentUser.ShowOtherMyJournals
                        ? journals.OrderBy(r => r.FollowUpDate).ThenBy(r => r.AssignedToName).ThenBy(r => r.State)
                        : journals.Where(x => x.AssignedToId == AppSession.Current.CurrentUser.UserID).OrderBy(r => r.FollowUpDate).ThenBy(r => r.AssignedToName).ThenBy(r => r.State);

                    dgvFollowups.DataSource = journals.ToList();
                    dgvFollowups.DataBind();
                }

                Literal litPageHeader = (Literal)Master.FindControl("litPageHeader");
                Literal litTotal = (Literal)Master.FindControl("litTotal");
                litPageHeader.Text = "Followups";
                litTotal.Text = dgvFollowups.Rows.Count + " record(s) found";
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }

        protected void ddlFilterAssociation_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGrid();
        }

        protected void ddlFilterProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGrid();
        }

        protected void btnResolveSelected_Click(object sender, EventArgs e)
        {
            try
            {
                User currentUser = AppSession.Current.CurrentUser;
                string email = currentUser.EmailAddress.Trim();
                using (var db = new MadduxEntities())
                {
                    foreach (GridViewRow gridRow in dgvFollowups.Rows)
                    {
                        CheckBox chkBox = (CheckBox)gridRow.FindControl("chkJournalSelect");
                        if (chkBox.Checked)
                        {

                            int id = Convert.ToInt32(dgvFollowups.DataKeys[gridRow.RowIndex].Values[0]);
                            var journal = db.Journals.Find(id);
                            if (journal != null)
                            {
                                journal.IsResolved = true;
                                journal.WhoUpdated = email;
                                journal.DateUpdated = DateTime.Now;
                            }
                        }
                    }
                    db.SaveChanges();
                }
                LoadGrid();
            }
            catch (Exception ex)
            {

                litMessage.Text = Redbud.BL.Utils.StringTools.GenerateError(ex.Message);
            }
        }

        protected void btnReloadRating_Click(object sender, EventArgs e)
        {
            try
            {
                using (var db = new MadduxEntities())
                {
                    // Calculate the star rating for each customer
                    var customers = db.Customers;
                    DateTime cutoffDate = DateTime.Today.AddYears(-1);
                    DateTime threeYearCutOffDate = DateTime.Today.AddYears(-3);
                    foreach (var customer in customers)
                    {
                        int? rating = customer.Rating;
                        var customerOrders = customer.Orders.Where(x => x.OrderStatus == 1);
                        if (customerOrders.Any(x => x.OrderDate >= threeYearCutOffDate))
                        {
                            decimal totalOrder = customer.Orders.Where(o => o.OrderDate >= cutoffDate).Sum(o => o.SubTotal);

                            if (totalOrder < 1500)
                                rating = 1;
                            else if (totalOrder < 3999)
                                rating = 2;
                            else if (totalOrder < 7999)
                                rating = 3;
                            else if (totalOrder < 14999)
                                rating = 4;
                            else
                                rating = 5;
                        }
                        else if (customerOrders.Any())
                        {
                            rating = 0;
                        }
                        else
                        {
                            rating = null;
                        }
                        
                        customer.StarRating = rating;
                    }
                    db.SaveChanges();
                }
                LoadGrid();
            }
            catch (Exception ex)
            {

                litMessage.Text = Redbud.BL.Utils.StringTools.GenerateError(ex.Message);
            }
        }

        protected void dgvFollowups_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvFollowups.PageIndex = e.NewPageIndex;
            LoadGrid();
            dgvFollowups.DataBind();
        }

        protected void ddlRating_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGrid();
        }

        protected void ddlCampaigns_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGrid();
        }

        protected void ddlFilterUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGrid();
        }
    }
}