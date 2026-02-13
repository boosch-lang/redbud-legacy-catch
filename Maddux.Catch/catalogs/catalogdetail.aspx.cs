using Redbud.BL.DL;
using Redbud.BL.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.UI.WebControls;

namespace Maddux.Catch.catalogs
{
    public partial class catalogdetail : System.Web.UI.Page
    {
        /// <summary>
        /// Sets catalog ID
        /// </summary>
        public int CatalogID
        {
            get
            {
                if (ViewState["CatalogID"] == null)
                {
                    ViewState["CatalogID"] = Request.QueryString["CatalogID"] == null || Request.QueryString["CatalogID"] == "" ? 0 : (object)Request.QueryString["CatalogID"];
                }
                return Convert.ToInt32(ViewState["CatalogID"].ToString());
            }

            set
            {
                ViewState["CatalogID"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Title = "Maddux.Catch | Catalog Details";
                Literal litPageHeader = (Literal)Master.FindControl("litPageHeader");
                successAlert.Visible = false;
                ProductCatalog productCatalog;
                LoadAssociations();
                LoadDropDowns();
                if (CatalogID != 0)
                {
                    NewCatalog.Attributes.Add("style", "display:none");
                    using (MadduxEntities db = new MadduxEntities())
                    {
                        productCatalog = db.ProductCatalogs.Include(c => c.Products)
                            .FirstOrDefault(x => x.CatalogId == CatalogID);
                        txtCatalogName.Text = productCatalog.CatalogName;
                        txtCustomerCatalogName.Text = productCatalog.CustomerCatalogName;

                        ddlYear.SelectedValue = productCatalog.CatalogYear.ToString();
                        chkActive.Checked = productCatalog.Active;
                        chkShowOnMyAccount.Checked = productCatalog.ShowOnMyAccount;
                        txtNotes.Text = productCatalog.Notes;
                        litPageHeader.Text = productCatalog.CatalogName.ToString();
                        ddlProductProgram.SelectedValue = productCatalog.ProgramID.ToString();
                        txtDisplayOrder.Text = productCatalog.DisplayOrder.ToString();

                        gridCatalogProducts.DataSource = productCatalog.Products.Select(p => new
                        {
                            p.ProductId,
                            p.CatalogId,
                            p.ItemNumber,
                            p.ItemNumberInternal,
                            p.ProductName,
                            p.ProductDesc,
                            p.Size,
                            p.Supplier.SupplierName,
                            p.UnitPrice
                        }).ToList();
                        gridCatalogProducts.DataBind();
                        //Set selected states
                        foreach (ListItem item in ddlCatalogStates.Items)
                        {
                            if (productCatalog.States.Any(s => s.StateID == item.Value))
                            {
                                item.Selected = true;
                            }
                        }
                        LoadShipDates();
                    }
                }
                else
                {
                    litPageHeader.Text = "New Product Catalog";
                    tabAssociations.Visible = false;
                    tabProducts.Visible = false;
                    btnDeleteProgramCatalog.Visible = false;
                    txtDisplayOrder.Text = "0"; //default order to 0

                }
                txtActiveTab.Text = "tab-item-details";

            }
        }
        /// <summary>
        /// Load associations dropdowns
        /// </summary>
        private void LoadShipDates()
        {
            try
            {
                using (MadduxEntities db = new MadduxEntities())
                {
                    dgvShipdates.DataSource = db.ProductCatalogShipDates
                        .Where(s => s.CatalogID == CatalogID)
                        .OrderBy(s => s.ShipDate)
                        .ThenBy(s => s.OrderDeadlineDate)
                        .ToList();
                    dgvShipdates.DataBind();
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }
        /// <summary>
        /// Load associations dropdowns
        /// </summary>
        private void LoadAssociations()
        {
            ListItem listItem;
            List<int> associations;

            try
            {
                using (MadduxEntities db = new MadduxEntities())
                {
                    associations = db.AssociationCatalogs
                    .Where(x => x.CatalogID == CatalogID)
                    .Select(x => x.AssociationID)
                    .ToList();

                    if (CatalogID != 0)
                    {
                        List<ListItem> _listItems = new List<ListItem>();
                        //Assigned associations
                        foreach (int associationID in associations)
                        {
                            Association assoc = db.Associations.First(x => x.AssociationID == associationID);
                            listItem = new ListItem
                            {
                                Text = assoc.AsscDesc.ToString().Trim(),
                                Value = assoc.AssociationID.ToString()
                            };
                            _listItems.Add(listItem);
                            lbAssignedAssociations.Items.Add(listItem);
                        }
                        _listItems = _listItems.OrderBy(x => x.Text).ToList();
                        lbAssignedAssociations.Items.Clear();
                        lbAssignedAssociations.DataSource = _listItems;
                        lbAssignedAssociations.DataBind();

                    }
                    //Not assigned associations
                    foreach (Association assoc in db.Associations.OrderBy(x => x.AsscDesc).ToList())
                    {
                        if (!associations.Contains(assoc.AssociationID))
                        {
                            listItem = new ListItem(assoc.AsscDesc.ToString().Trim(), assoc.AssociationID.ToString());
                            lbAvailableAssociations.Items.Add(listItem);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Populates product programs, product catalogs and product catalogs states drop downs
        /// </summary>
        private void LoadDropDowns()
        {
            ListItem listItem;

            try
            {
                using (MadduxEntities db = new MadduxEntities())
                {
                    //Load product programs drop downs
                    foreach (ProductProgram productProgram in db.ProductPrograms.ToList())
                    {
                        listItem = new ListItem(productProgram.ProgramName, productProgram.ProgramID.ToString());
                        ddlProductProgram.Items.Add(listItem);
                    }

                    //Load product catalog drop down
                    var catalogs = db.ProductCatalogs
                       .OrderByDescending(x => x.CatalogYear)
                       .ThenBy(x => x.CatalogName)
                       .Select(x => new ListItem()
                       {
                           Text = x.CatalogName,
                           Value = x.CatalogId.ToString()
                       })
                       .ToList();
                    catalogs.Insert(0, new ListItem()
                    {
                        Text = "------Select Catalog-------",
                        Value = "0"
                    });
                    ddlCatalogs.DataSource = catalogs;
                    ddlCatalogs.DataBind();

                    var states = db.States.Where(s => s.Country != "USA").Select(r => new
                    {
                        r.StateID,
                        r.StateName
                    }).ToList();

                    ddlCatalogStates.DataValueField = "StateID";
                    ddlCatalogStates.DataTextField = "StateName";
                    ddlCatalogStates.DataSource = states;
                    ddlCatalogStates.DataBind();

                }
                //Load years drop down
                //2000 - current + 2
                for (int i = DateTime.Today.Year + 2; i >= 2000; i--)
                {
                    ListItem _li = new ListItem(i.ToString(), i.ToString());
                    ddlYear.Items.Add(_li);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnDeleteProgramCatalog_Click(object sender, EventArgs e)
        {
            try
            {
                using (MadduxEntities db = new MadduxEntities())
                {
                    var productCatalog = db.ProductCatalogs.FirstOrDefault(x => x.CatalogId == CatalogID);
                    db.ProductCatalogs.Remove(productCatalog);
                    bool succcess = db.SaveChanges() > 0;

                    if (succcess)
                    {
                        successAlert.Visible = true;
                        spSuccessMessage.InnerText = "Program  deleted successfully.";
                        Response.AddHeader("REFRESH", "1;URL=/products/programlist.aspx");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("/products/programlist.aspx");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Save())
            {
                successAlert.Visible = true;
                spSuccessMessage.InnerText = "Program catalog saved successfully.";
                txtActiveTab.Text = "tab-item-details";
            }
        }
        /// <summary>
        /// Add to assigned list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdAddAssoc_Click(object sender, EventArgs e)
        {
            foreach (ListItem _liItem in lbAvailableAssociations.Items)
            {
                if (_liItem.Selected == true)
                {
                    if (lbAssignedAssociations.Items.Count > 0)
                    {
                        int _iIndex = 0;
                        bool _bInserted = false;
                        foreach (ListItem _liExistingItem in lbAssignedAssociations.Items)
                        {
                            if (string.Compare(_liExistingItem.Text.ToString(), _liItem.Text.ToString()) >= 0)
                            {
                                lbAssignedAssociations.Items.Insert(_iIndex, _liItem);
                                _bInserted = true;
                                break;
                            }
                            _iIndex++;
                        }

                        if (_bInserted == false)
                        {
                            lbAssignedAssociations.Items.Add(_liItem);
                        }
                    }
                    else
                    {
                        lbAssignedAssociations.Items.Add(_liItem);
                    }
                    _liItem.Selected = false;
                    lbAvailableAssociations.Items.Remove(_liItem);
                    break;
                }
            }
            txtActiveTab.Text = "tab-item-associations";
        }
        /// <summary>
        /// Remove from assigned list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdRemAssoc_Click(object sender, EventArgs e)
        {
            foreach (ListItem _liItem in lbAssignedAssociations.Items)
            {
                if (_liItem.Selected == true)
                {
                    if (lbAvailableAssociations.Items.Count > 0)
                    {
                        int _iIndex = 0;
                        bool _bInserted = false;
                        foreach (ListItem _liExistingItem in lbAvailableAssociations.Items)
                        {
                            if (string.Compare(_liExistingItem.Text.ToString(), _liItem.Text.ToString()) >= 0)
                            {
                                lbAvailableAssociations.Items.Insert(_iIndex, _liItem);
                                _bInserted = true;
                                break;
                            }
                            _iIndex++;
                        }

                        if (_bInserted == false)
                        {
                            lbAvailableAssociations.Items.Add(_liItem);
                        }
                    }
                    else
                    {
                        lbAvailableAssociations.Items.Add(_liItem);
                    }
                    _liItem.Selected = false;
                    lbAssignedAssociations.Items.Remove(_liItem);
                    break;
                }
            }

            txtActiveTab.Text = "tab-item-associations";

        }
        /// <summary>
        /// Saves association
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdSaveAssoc_Click(object sender, EventArgs e)
        {
            try
            {
                using (MadduxEntities db = new MadduxEntities())
                {
                    List<AssociationCatalog> associationCatalogs = db.AssociationCatalogs.Where(x => x.CatalogID == CatalogID)
                        .ToList();
                    //Remove association
                    if (associationCatalogs.Count > 0)
                    {
                        foreach (AssociationCatalog associationCatalog in associationCatalogs)
                        {
                            db.AssociationCatalogs.Remove(associationCatalog);
                        }
                        db.SaveChanges();
                    }
                    //Add association
                    foreach (ListItem _liItem in lbAssignedAssociations.Items)
                    {
                        int associationID = Convert.ToInt32(_liItem.Value);
                        AssociationCatalog association = new AssociationCatalog
                        {
                            AssociationID = associationID,
                            CatalogID = CatalogID
                        };
                        db.AssociationCatalogs.Add(association);
                    }
                    db.SaveChanges();
                }

                successAlert.Visible = true;
                txtActiveTab.Text = "tab-item-associations";
                spSuccessMessage.InnerText = "Association(s)  assigned/removed successfully.";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Saves catalog
        /// </summary>
        /// <returns></returns>
        private bool Save()
        {
            bool isSuccess;
            try
            {
                using (MadduxEntities db = new MadduxEntities())
                {
                    ProductCatalog productCatalog = null;
                    if (CatalogID == 0) //New catalog
                    {
                        productCatalog = new ProductCatalog();

                        db.ProductCatalogs.Add(productCatalog);

                        tabAssociations.Visible = true;
                        btnDeleteProgramCatalog.Visible = true;

                        LoadAssociations();
                    }
                    else //Existing catalog
                    {
                        productCatalog = db.ProductCatalogs.FirstOrDefault(x => x.CatalogId == CatalogID);
                        productCatalog.States.Clear();
                    }

                    productCatalog.CatalogName = txtCatalogName.Text;
                    productCatalog.CustomerCatalogName = txtCustomerCatalogName.Text;
                    productCatalog.CatalogYear = Convert.ToInt32(ddlYear.SelectedValue);
                    productCatalog.ShowOnMyAccount = chkShowOnMyAccount.Checked;
                    productCatalog.Notes = txtNotes.Text;
                    productCatalog.ProgramID = Convert.ToInt32(ddlProductProgram.SelectedValue);
                    productCatalog.Active = chkActive.Checked;
                    productCatalog.DisplayOrder = Convert.ToInt32(txtDisplayOrder.Text);

                    //Client ask to remove below fields from front-end but are still required on db
                    //TODO: Clean up db once old project is end of life
                    productCatalog.CatalogSeason = "N/A"; // Removed from front-end but still present on the db and required
                    productCatalog.PDFUrl = "N/A";
                    productCatalog.PhotoPath = "N/A";
                    productCatalog.CatalogGroupId = 0;
                    productCatalog.CatalogClassId = 24;

                    //Add select states
                    foreach (ListItem listItem in ddlCatalogStates.Items)
                    {
                        if (listItem.Selected == true)
                        {
                            string StateID = listItem.Value;
                            State state = db.States.Where(s => s.StateID == StateID).FirstOrDefault();
                            if (state != null)
                            {
                                productCatalog.States.Add(state);
                            }
                        }
                    }

                    isSuccess = db.SaveChanges() > 0;

                    return isSuccess;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnCopyData_Click(object sender, EventArgs e)
        {
            try
            {

                if (ddlCatalogs.SelectedIndex == 0)
                {
                    litCatalogError.Text = "<span class='text-danger'>Please select the catalog!</span>";
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtNewCatalogName.Text))
                {
                    litCatalogError.Text = "<span class='text-danger'>New catalog name is required!</span>";
                    return;
                }
                string _selectedValue = ddlCatalogs.SelectedValue;
                int _catalogID = Convert.ToInt32(ddlCatalogs.SelectedValue);
                using (MadduxEntities db = new MadduxEntities())
                {
                    var productCatalog = db.ProductCatalogs
                                            .Include(p => p.Products)
                                            .First(x => x.CatalogId == _catalogID);
                    if (productCatalog != null)
                    {
                        ProductCatalog newCatalog = db.ProductCatalogs.Create();
                        newCatalog.CatalogName = txtNewCatalogName.Text;
                        newCatalog.CustomerCatalogName = txtNewCatalogName.Text;
                        newCatalog.CatalogYear = productCatalog.CatalogYear;
                        newCatalog.Active = productCatalog.Active;
                        newCatalog.ShowOnMyAccount = productCatalog.ShowOnMyAccount;
                        newCatalog.Notes = productCatalog.Notes;
                        newCatalog.ProgramID = productCatalog.ProgramID;
                        newCatalog.DisplayOrder = productCatalog.DisplayOrder;

                        newCatalog.CatalogSeason = "N/A"; // Removed from front-end but still present on the db and required
                        newCatalog.PDFUrl = "N/A";
                        newCatalog.PhotoPath = "N/A";
                        newCatalog.CatalogGroupId = 0;
                        newCatalog.CatalogClassId = 24;

                        foreach (Product p in productCatalog.Products.ToList())
                        {
                            var newProduct = new Product
                            {
                                ItemNumber = p.ItemNumber,
                                ItemNumberInternal = p.ItemNumberInternal,
                                ProductName = p.ProductName,
                                ProductDesc = p.ProductDesc,
                                ProductSubCategoryId = p.ProductSubCategoryId,
                                ItemsPerPackage = p.ItemsPerPackage,
                                PackagesPerUnit = p.PackagesPerUnit,
                                UnitCost = p.UnitCost,
                                UnitPrice = p.UnitPrice,
                                ProductCode = p.ProductCode,
                                Size = p.Size,
                                PotDescription = p.PotDescription,
                                UPCCode = p.UPCCode,
                                SuggestedPackageRetail = p.SuggestedPackageRetail,
                                UnitWeight = p.UnitWeight,
                                UnitSize = p.UnitSize,
                                CatalogPageStart = p.CatalogPageStart,
                                CatalogPageEnd = p.CatalogPageEnd,
                                TagsIncluded = p.TagsIncluded,
                                SupplierID = p.SupplierID,
                                WarehouseLocation = p.WarehouseLocation,
                                ProductNotAvailable = p.ProductNotAvailable,
                                ProductNotAvailableDesc = p.ProductNotAvailableDesc,
                                NewItem = p.NewItem,
                                PreviousProductID = p.PreviousProductID
                            };

                            p.Photos.ToList().ForEach(ph =>
                            newProduct.Photos.Add(new Photo()
                            {
                                PhotoDescription = ph.PhotoDescription,
                                PhotoPath = ph.PhotoPath,
                                Notes = ph.Notes
                            }));
                            newCatalog.Products.Add(newProduct);
                        }

                        db.ProductCatalogs.Add(newCatalog);
                        db.SaveChanges();

                        Response.Redirect($"~/catalogs/catalogdetail.aspx?CatalogID={newCatalog.CatalogId}");


                        //txtCatalogName.Text = txtNewCatalogName.Text;
                        //txtCustomerCatalogName.Text = txtNewCatalogName.Text;
                        //ddlYear.SelectedValue = productCatalog.CatalogYear.ToString();
                        //chkActive.Checked = productCatalog.Active;
                        //chkShowOnMyAccount.Checked = productCatalog.ShowOnMyAccount;
                        //txtNotes.Text = productCatalog.Notes;
                        //ddlProductProgram.SelectedValue = productCatalog.ProgramID.ToString();
                        //ddlCatalogs.SelectedIndex = 0;
                        //litCatalogError.Text = StringTools.GenerateSuccess("Copied successfully!");
                    }
                }
            }
            catch (Exception ex)
            {
                litError.Text = StringTools.GenerateError(ex.Message);
            }

        }

        protected void gridCatalogProducts_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
    }
}