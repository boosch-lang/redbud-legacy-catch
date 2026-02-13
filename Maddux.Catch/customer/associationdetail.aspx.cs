using Maddux.Catch.Helpers;
using Maddux.Catch.LocalClasses;
using Redbud.BL.DL;
using Redbud.BL.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace Maddux.Catch.customer
{
    public partial class associationdetail : System.Web.UI.Page
    {
        private int AssociationID
        {
            get
            {
                if (ViewState["AssociationID"] == null)
                {
                    ViewState["AssociationID"] = Request.QueryString["aID"] == null || Request.QueryString["aID"] == "" ? 0 : (object)Request.QueryString["aID"];
                }
                return Convert.ToInt32(ViewState["AssociationID"].ToString());
            }

            set
            {
                ViewState["AssociationID"] = value;
            }
        }
        private string Region
        {
            get
            {
                if (ViewState["Region"] == null)
                {
                    ViewState["Region"] = Request.QueryString["region"] == null || Request.QueryString["region"] == "" ? "All" : Request.QueryString["region"];
                }
                return ViewState["Region"].ToString();
            }

            set
            {
                ViewState["Region"] = value;
            }
        }
        private string StarRating
        {
            get
            {
                if (ViewState["StarRating"] == null)
                {
                    ViewState["StarRating"] = Request.QueryString["starRating"] == null || Request.QueryString["starRating"] == "" ? "-1" : Request.QueryString["starRating"];
                }
                return ViewState["StarRating"].ToString();
            }

            set
            {
                ViewState["StarRating"] = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (AssociationID == 0)
                {
                    Response.Redirect("./associations.aspx", true);
                }

                LoadGrid();
                var user = AppSession.Current.CurrentUser;
                if (!user.CanDeleteCustomers || !user.CanDeleteOrders)
                {
                    DivDeleteBtn.Visible = false;
                    deleteAssociationModal.Visible = false;
                }
            }
            catch (Exception ex)
            {
                litAssociationDetails.Text = StringTools.GenerateError(ex.Message);
            }



        }
        private void LoadGrid()
        {
            if (!IsPostBack)
            {
                using (MadduxEntities db = new MadduxEntities())
                {
                    Association association = db.Associations.Where(x => x.AssociationID == AssociationID).FirstOrDefault();

                    BannerMessage.Text = association.BannerMessage;
                    bannerStartDate.Text = association.BannerStartDate.HasValue ? association.BannerStartDate.Value.ToString("MMMM dd, yyyy") : "";
                    bannerEndDate.Text = association.BannerEndDate.HasValue ? association.BannerEndDate.Value.ToString("MMMM dd, yyyy") : "";

                    SalesBannerMessage.Text = association.SalesBannerMessage;
                    SalesStartDate.Text = association.SalesBannerStartDate.HasValue ? association.SalesBannerStartDate.Value.ToString("MMMM dd, yyyy") : "";
                    SalesEndDate.Text = association.SalesBannerEndDate.HasValue ? association.SalesBannerEndDate.Value.ToString("MMMM dd, yyyy") : "";
                    //TagLine.Text = association.TagLine;

                    var regions = RegionHelper.GetRegionsForSelectList(false);

                    ddlFilterRegion.DataTextField = "Text";
                    ddlFilterRegion.DataValueField = "Value";
                    ddlFilterRegion.DataSource = regions;
                    ddlFilterRegion.DataBind();

                    if (Region != "All")
                    {
                        var selectedRegions = Region.Split(',');
                        foreach (var s in selectedRegions)
                        {
                            ddlFilterRegion.Items.FindByValue(s).Selected = true;
                        }
                    }

                    if (StarRating != "-1")
                    {
                        var selectedRatings = StarRating.Split(',');
                        foreach (var s in selectedRatings)
                        {
                            ddlFilterStarRating.Items.FindByValue(s).Selected = true;
                        }
                    }


                    //association.CustomerAsscs
                    Literal litPageHeader = (Literal)Master.FindControl("litPageHeader");
                    litPageHeader.Text = $@"Association-<small>({association.AsscDesc})</small>";

                }
                LoadCustomers();
            }
        }

        private void LoadCustomers()
        {
            using (MadduxEntities db = new MadduxEntities())
            {
                var user = AppSession.Current.CurrentUser;
                List<AssociationDetails> list = new List<AssociationDetails>();

                var query = db.vmCustomerDetails.Where(x => x.AssociationID == AssociationID);

                if (user.CanOnlyViewAssignedProvinces)
                {
                    var states = db.UserStates.Where(x => x.UserID == user.UserID).Select(x => x.StateID).ToList();
                    query = query.Where(x => states.Contains(x.State));
                }

                if (StarRating != "-1")
                {
                    var stars = StarRating.Split(',').Select(x => int.Parse(x)).ToList();
                    var filterStars = stars.Where(x => x != 999).ToList();
                    var includeNull = stars.Any(x => x == 999);
                    if (includeNull)
                    {
                        query = query.Where(x => !x.StarRating.HasValue || filterStars.Contains(x.StarRating.Value));
                    }
                    else
                    {
                        query = query.Where(x => x.StarRating.HasValue && filterStars.Contains(x.StarRating.Value));
                    }
                }

                list = query
                    .OrderBy(x => x.Company)
                    .Select(x => new AssociationDetails
                    {
                        CustomerID = x.CustomerId,
                        Company = x.Company,
                        Contact = x.Contact,
                        City = x.City,
                        Phone = x.Phone,
                        LastOrderDate = x.LastOrderDate,
                        State = x.State,
                        FSA = !string.IsNullOrEmpty(x.Zip) ? x.Zip.Substring(0, 3) : string.Empty,
                        StarRating = x.StarRating
                    })
                    .ToList();


                if (!string.Equals("All", Region))
                {
                    var rgs = Region.Split(',');

                    var fsa = db.FreightCharges.Where(x => rgs.Contains(x.Region)).Select(x => x.AreaID).ToList();
                    list = list.Where(x => fsa.Contains(x.FSA)).ToList();
                    dgvAssociation.DataBind();
                }

                dgvAssociation.DataSource = list.ToList();
                dgvAssociation.DataBind();
            }
        }
        protected void dgvAssociation_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvAssociation.PageIndex = e.NewPageIndex;
            LoadGrid();
            dgvAssociation.DataBind();
        }
        protected void dgvAssociation_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
        public string GenerateStarRatingGraphic(int? starRating)
        {

            string stars = string.Empty;
            if (starRating.HasValue)
            {
                for (int x = 0; x < starRating.Value; x++)
                {
                    stars += "<i class='fa fa-star'></i> ";
                }
                for (int x = 0; x < (5 - starRating.Value); x++)
                {
                    stars += "<i class='fa fa-star-o'></i> ";
                }
            }
            else
            {
                stars += "N/A";
            }
            return stars.Trim();
        }
        protected void Save_Click(object sender, EventArgs e)
        {
            using (MadduxEntities madduxEntities = new MadduxEntities())
            {
                try
                {
                    Association association = madduxEntities.Associations.FirstOrDefault(x => x.AssociationID == AssociationID);
                    if (association != null)
                    {
                        int messageLength = BannerMessage.Text.Length;
                        if (messageLength <= 500)
                        {
                            association.BannerMessage = BannerMessage.Text;
                        }
                        else
                        {
                            throw new Exception("Banner message can not be more than 500 characters!");
                        }
                        if (!string.IsNullOrEmpty(bannerStartDate.Text))
                            association.BannerStartDate = DateTime.Parse(bannerStartDate.Text);
                        if (!string.IsNullOrEmpty(bannerEndDate.Text))
                            association.BannerEndDate = DateTime.Parse(bannerEndDate.Text);


                        messageLength = SalesBannerMessage.Text.Length;
                        if (messageLength <= 500)
                        {
                            association.SalesBannerMessage = SalesBannerMessage.Text;
                        }
                        else
                        {
                            throw new Exception("Sales banner message can not be more than 500 characters!");
                        }
                        if (!string.IsNullOrEmpty(SalesStartDate.Text))
                            association.SalesBannerStartDate = DateTime.Parse(SalesStartDate.Text);
                        if (!string.IsNullOrEmpty(SalesEndDate.Text))
                            association.SalesBannerEndDate = DateTime.Parse(SalesEndDate.Text);

                        //Tag line functionality removed on clients request
                        /*
                        int tagLineLength = TagLine.Text.Length;
                        if (tagLineLength <= 100)
                        {
                            association.TagLine = TagLine.Text;
                        }
                        else
                        {
                            throw new Exception("Tag line can not be more than 100 characters!");
                        }
                        */


                        int saved = madduxEntities.SaveChanges();
                        if (saved > 0)
                        {
                            litAssociationDetails.Text = StringTools.GenerateSuccess("Saved!");
                        }
                    }
                }
                catch
                {
                    litAssociationDetails.Text = StringTools.GenerateError("Error while saving association!");
                }

            }
        }

        protected void Delete_Click(object sender, EventArgs e)
        {
            using (MadduxEntities madduxEntities = new MadduxEntities())
            {
                try
                {
                    Association association = madduxEntities.Associations.FirstOrDefault(x => x.AssociationID == AssociationID);
                    if (association != null)
                    {

                        IQueryable<AssociationCatalog> catalogAssociations = madduxEntities.AssociationCatalogs.Where(a => a.AssociationID == association.AssociationID);
                        madduxEntities.AssociationCatalogs.RemoveRange(catalogAssociations);

                        IQueryable<CustomerAssc> customerAssociations = madduxEntities.CustomerAsscs.Where(ca => ca.AssociationID == association.AssociationID);
                        madduxEntities.CustomerAsscs.RemoveRange(customerAssociations);

                        IQueryable<UserAssc> userAssociations = madduxEntities.UserAsscs.Where(uc => uc.AssociationID == association.AssociationID);
                        madduxEntities.UserAsscs.RemoveRange(userAssociations);

                        madduxEntities.Associations.Remove(association);

                        int changes = madduxEntities.SaveChanges();
                        if (changes > 0)
                            Response.Redirect("/customer/associations.aspx", true);
                    }
                }
                catch
                {
                    litAssociationDetails.Text = StringTools.GenerateError("Error while removing association!");
                }

            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string query = $@"?aID={AssociationID}";

            //add regions
            foreach (var i in ddlFilterRegion.GetSelectedIndices())
            {
                var region = ddlFilterRegion.Items[i];
                query += $@"&region={region.Value}";
            }

            //add ratings
            foreach (var i in ddlFilterStarRating.GetSelectedIndices())
            {
                var rating = ddlFilterStarRating.Items[i];
                query += $@"&starrating={rating.Value}";
            }

            query += "#associationCustomers";

            Response.Redirect($"/customer/associationdetail.aspx{query}");

        }
    }
    class AssociationDetails
    {
        public int CustomerID { get; set; }
        public string Company { get; set; }
        public string Contact { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string State { get; set; }
        public string FSA { get; set; }
        public DateTime? LastOrderDate { get; set; }
        public int? StarRating { get; set; }
    }
}