using Redbud.BL.DL;
using Redbud.BL.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Maddux.Catch.products
{
    public partial class programdetail : Page
    {
        public int ProgramID
        {
            get
            {
                if (ViewState["ProgramID"] == null)
                {
                    ViewState["ProgramID"] = Request.QueryString["ProgramID"] == null || Request.QueryString["ProgramID"] == "" ? 0 : (object)Request.QueryString["ProgramID"];
                }
                return Convert.ToInt32(ViewState["ProgramID"].ToString());
            }

            set
            {
                ViewState["ProgramID"] = value;
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Title = "Maddux | Program Details";
                Literal litPageHeader = (Literal)Master.FindControl("litPageHeader");
                successAlert.Visible = false;

                if (ProgramID != 0)
                {
                    using (MadduxEntities db = new MadduxEntities())
                    {
                        ProductProgram program = db.ProductPrograms.FirstOrDefault(x => x.ProgramID == ProgramID);
                        litPageHeader.Text = "Program: " + program.ProgramName.ToString();
                        txtProgramName.Text = program.ProgramName;
                        txtProgramDescription.Text = program.ProgramDescription;
                        chkActive.Checked = (bool)program.Active;
                    }
                    LoadCatalogs();
                    //LoadShipDates();
                    txtActiveTab.Text = "tab-item-details";

                }
                else
                {
                    litPageHeader.Text = "New Program";
                    //tabShipdates.Visible = false;
                    tabProductCatalogs.Visible = false;

                    txtActiveTab.Text = "tab-item-details";
                    btnDeleteProgram.Visible = false;
                }
            }
        }

        private void LoadShipDates()
        {
            try
            {
                using (MadduxEntities db = new MadduxEntities())
                {
                    dgvShipdates.DataSource = db.ProductPrograms.Where(s => s.ProgramID == ProgramID).SelectMany(x => x.ProductCatalogs).SelectMany(x => x.ProductCatalogShipDates).AsEnumerable()
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
        /// Handles loading the catalogs
        /// </summary>
        private void LoadCatalogs()
        {
            ListItem listItem;

            try
            {
                using (MadduxEntities db = new MadduxEntities())
                {
                    //Catalogs assigned to the program
                    List<ProductCatalog> catalogs = db.ProductCatalogs
                        .Where(c => c.ProgramID == ProgramID)
                        .Where(x => x.Active)
                        .OrderByDescending(x => x.CatalogYear)
                        .ThenByDescending(x => x.CatalogSeason)
                        .ThenByDescending(x => x.CatalogName)
                        .ToList();

                    foreach (ProductCatalog catalog in catalogs)
                    {
                        listItem = new ListItem(catalog.CatalogName.ToString().Trim(), catalog.CatalogId.ToString());
                        lbProgramAssignedCatalogs.Items.Add(listItem);
                    }

                    //Catalogs not assigned to the program
                    List<ProductCatalog> availableCatalogs = db.ProductCatalogs
                        .Where(x => x.ProgramID != ProgramID)
                        .Where(x => x.Active)
                        .OrderByDescending(x => x.CatalogYear)
                        .ThenByDescending(x => x.CatalogSeason)
                        .ThenByDescending(x => x.CatalogName)
                        .ToList();

                    foreach (ProductCatalog catalogAvailable in availableCatalogs)
                    {
                        listItem = new ListItem(catalogAvailable.CatalogName.ToString(), catalogAvailable.CatalogId.ToString());
                        lbProgramAvailableCatalogs.Items.Add(listItem);
                    }

                    //Sets catalogs controls to visible
                    tabProductCatalogs.Visible = true;
                    btnDeleteProgram.Visible = true;
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }
        /// <summary>
        /// Add catalogs to the assigned list item 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdAddProCat_Click(object sender, EventArgs e)
        {
            foreach (ListItem _liItem in lbProgramAvailableCatalogs.Items)
            {
                if (_liItem.Selected == true)
                {
                    if (lbProgramAssignedCatalogs.Items.Count > 0)
                    {
                        int _iIndex = 0;
                        bool _bInserted = false;
                        foreach (ListItem _liExistingItem in lbProgramAssignedCatalogs.Items)
                        {
                            if (string.Compare(_liExistingItem.Text.ToString(), _liItem.Text.ToString()) >= 0)
                            {
                                lbProgramAssignedCatalogs.Items.Insert(_iIndex, _liItem);
                                _bInserted = true;
                                break;
                            }
                            _iIndex++;
                        }

                        if (_bInserted == false)
                        {
                            lbProgramAssignedCatalogs.Items.Add(_liItem);
                        }
                    }
                    else
                    {
                        lbProgramAssignedCatalogs.Items.Add(_liItem);
                    }
                    _liItem.Selected = false;
                    lbProgramAvailableCatalogs.Items.Remove(_liItem);
                    break;
                }
            }
            txtActiveTab.Text = "tab-item-associations";
        }
        /// <summary>
        /// Handles removing catalogs from the assigned list item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdRemProCat_Click(object sender, EventArgs e)
        {
            foreach (ListItem _liItem in lbProgramAssignedCatalogs.Items)
            {
                if (_liItem.Selected == true)
                {
                    if (lbProgramAvailableCatalogs.Items.Count > 0)
                    {
                        int _iIndex = 0;
                        bool _bInserted = false;
                        foreach (ListItem _liExistingItem in lbProgramAvailableCatalogs.Items)
                        {
                            if (string.Compare(_liExistingItem.Text.ToString(), _liItem.Text.ToString()) >= 0)
                            {
                                lbProgramAvailableCatalogs.Items.Insert(_iIndex, _liItem);
                                _bInserted = true;
                                break;
                            }
                            _iIndex++;
                        }

                        if (_bInserted == false)
                        {
                            lbProgramAvailableCatalogs.Items.Add(_liItem);
                        }
                    }
                    else
                    {
                        lbProgramAvailableCatalogs.Items.Add(_liItem);
                    }
                    _liItem.Selected = false;
                    lbProgramAssignedCatalogs.Items.Remove(_liItem);
                    break;
                }
            }

            txtActiveTab.Text = "tab-item-associations";
        }
        /// <summary>
        /// Saves programs catalogs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdSaveProCat_Click(object sender, EventArgs e)
        {
            try
            {
                using (MadduxEntities db = new MadduxEntities())
                {
                    List<ProductCatalog> catalogs = db.ProductCatalogs
                        .Where(c => c.ProgramID == ProgramID)
                        .ToList();

                    if (catalogs.Count() > 0)
                    {
                        foreach (ProductCatalog catalog in catalogs)
                        {
                            catalog.ProgramID = null;
                        }
                        db.SaveChanges();
                    }

                    foreach (ListItem _liItem in lbProgramAssignedCatalogs.Items)
                    {
                        int CatalogID = Convert.ToInt32(_liItem.Value);
                        ProductCatalog catalog = db.ProductCatalogs.FirstOrDefault(x => x.CatalogId == CatalogID);
                        catalog.ProgramID = ProgramID;
                    }
                    db.SaveChanges();
                }

                successAlert.Visible = true;
                txtActiveTab.Text = "tab-item-catalogs";
                spSuccessMessage.InnerText = "Catalog(s)  assigned/removed successfully.";

            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }
        /// <summary>
        /// Handles program deletion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDeleteProgram_Click(object sender, EventArgs e)
        {
            try
            {
                using (MadduxEntities db = new MadduxEntities())
                {
                    List<ProductCatalog> catalogs = db.ProductCatalogs
                        .Where(c => c.ProgramID == ProgramID)
                        .ToList();
                    //Set ProgramID = null on the ProductCatalog table as we are deleting the program
                    foreach (ProductCatalog catalog in catalogs)
                    {
                        catalog.ProgramID = null;
                    }
                    db.SaveChanges();

                    //Remove the program
                    ProductProgram program = db.ProductPrograms.FirstOrDefault(x => x.ProgramID == ProgramID);
                    db.ProductPrograms.Remove(program);
                    db.SaveChanges();

                    //Display alert message
                    successAlert.Visible = true;
                    spSuccessMessage.InnerText = "Program  deleted successfully.";
                    Response.AddHeader("REFRESH", "1;URL=/products/programlist.aspx");
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }
        /// <summary>
        /// Save program
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Save())
            {
                successAlert.Visible = true;
                spSuccessMessage.InnerText = "Program  saved successfully.";

            }
        }

        /// <summary>
        /// Cancels edits and redirect
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("/products/programlist.aspx");
        }
        /// <summary>
        /// Saves updates to program and adds new program
        /// </summary>
        /// <returns></returns>
        private bool Save()
        {
            bool isSuccess;
            try
            {
                using (MadduxEntities db = new MadduxEntities())
                {

                    if (ProgramID == 0) //New Program
                    {
                        var program = new ProductProgram
                        {
                            ProgramName = txtProgramName.Text.ToString(),
                            ProgramDescription = txtProgramDescription.Text.ToString(),
                            Active = chkActive.Checked
                        };
                        db.ProductPrograms.Add(program);
                        isSuccess = db.SaveChanges() > 0;
                        tabProductCatalogs.Visible = true;
                        btnDeleteProgram.Visible = true;
                        LoadCatalogs();
                        Response.Redirect($@"./programdetail.aspx?ProgramID={program.ProgramID}");
                        return isSuccess;
                    }
                    else //Existing program - update
                    {
                        ProductProgram program = db.ProductPrograms.FirstOrDefault(x => x.ProgramID == ProgramID);
                        program.ProgramName = txtProgramName.Text.ToString();
                        program.ProgramDescription = txtProgramDescription.Text.ToString();
                        program.Active = chkActive.Checked;
                        isSuccess = db.SaveChanges() > 0;

                        return isSuccess;
                    }
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
                return false;
            }
        }
    }
}