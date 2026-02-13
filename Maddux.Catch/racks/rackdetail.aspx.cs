using Maddux.Catch.Helpers;
using Redbud.BL;
using Redbud.BL.DL;
using Redbud.BL.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Maddux.Catch.racks
{
    public partial class rackdetail : Page
    {

        private int RackID
        {
            get
            {
                if (ViewState["RackID"] == null)
                {
                    ViewState["RackID"] = Request.QueryString["RackID"] == null || Request.QueryString["RackID"] == "" ? 0 : (object)Request.QueryString["RackID"];
                }
                return Convert.ToInt32(ViewState["RackID"].ToString());
            }

            set
            {
                ViewState["RackID"] = value;
            }
        }

        private int RackType
        {
            get
            {
                if (ViewState["RackType"] == null)
                {
                    ViewState["RackType"] = (int)Redbud.BL.RackType.Standard;
                }
                return Convert.ToInt32(ViewState["RackType"].ToString());
            }

            set
            {
                ViewState["RackType"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["tab"] != null)
                {
                    CurrentTab.Value = Request.QueryString["tab"].ToString();
                }

                Title = "Maddux | Rack Details";
                Literal litPageHeader = (Literal)Master.FindControl("litPageHeader");
                successAlert.Visible = false;
                errorAlert.Visible = false;

                LoadCatalog();
                LoadRackSize();
                LoadDiscountOptions();
                EnumHelper.PopulateDropDownList<RackType>(ddlRackType);

                if (RackID != 0) //Existing rack - load details
                {
                    using (MadduxEntities db = new MadduxEntities())
                    {
                        ProductCatalogRack rack = db.ProductCatalogRacks
                            .Include(x => x.RackProducts)
                            .Include(x => x.Photos)
                            .First(x => x.RackID == RackID);

                        RackType = rack.RackType;

                        tabAssociations.Visible = rack.RackType == (int)Redbud.BL.RackType.Standard;
                        tabRackAssociations.Visible = rack.RackType == (int)Redbud.BL.RackType.Bulk;

                        txtRackName.Text = rack.RackName;
                        txtRackDescription.Text = rack.RackDesc;
                        txtDisplayOrder.Text = rack.DisplayOrder.ToString();
                        txtRackMinimumItems.Text = rack.MinimumItems.ToString();
                        txtRackMaximumItems.Text = rack.MaximumItems.ToString();
                        chkAllowCustomization.Checked = rack.AllowCustomization;
                        chkBoxIsActive.Checked = rack.Active;
                        ddlRackCatalog.SelectedValue = rack.CatalogID.ToString();
                        txtForm.Text = rack.FilePath;
                        ddlDiscount.SelectedValue = rack.Discount.HasValue ? rack.Discount.Value.ToString("F2") : "0";
                        ddlRackType.SelectedValue = ((int)rack.RackType).ToString();

                        rptAssigned.DataSource = rack.RackProducts;
                        rptAssigned.DataBind();

                        //There is a console app that creates Racks from catalogs and so if the size is not assigned when
                        //programmatically creating we must check for null
                        if (rack.RackSize != null)
                        {
                            ddlRackSize.SelectedValue = rack.RackSize.ToString();
                        }

                        //Load rack dimensions
                        if (!string.IsNullOrWhiteSpace(rack.Dimensions))
                        {
                            string[] RackDimensions = rack.Dimensions.Split('x');
                            RackLength.Text = RackDimensions[0];
                            RackWidth.Text = RackDimensions[1];
                            RackHeight.Text = RackDimensions[2];
                        }
                        RackWeight.Text = rack.Weight;

                        //Sync the rack ship dates with it's catalog
                        //get all ship dates for the catalog
                        List<ProductCatalogShipDate> productCatalogShipdates = rack.ProductCatalog.ProductCatalogShipDates.ToList();

                        //delete any RackShipDates that are no longer on the catalog
                        var shipDatesToDelete = rack.ProductRackShipDates.Where(x => !productCatalogShipdates.Any(d => d.ShipDate == x.ShipDate)).ToList();
                        foreach (var shipDate in shipDatesToDelete)
                        {
                            db.ProductRackShipDates.Remove(shipDate);
                            rack.ProductRackShipDates.Remove(shipDate);
                        }

                        //now loop through the catalog ship dates and create any
                        //that aren't already configured for the rack
                        foreach (ProductCatalogShipDate shipdate in productCatalogShipdates)
                        {
                            if (rack.ProductRackShipDates.Any(x => x.ShipDate == shipdate.ShipDate))
                            {
                                continue;
                            }

                            var pRackShipDate = new ProductRackShipDate
                            {
                                RackID = rack.RackID,
                                ShipDate = shipdate.ShipDate,
                                OrderDeadlineDate = shipdate.OrderDeadlineDate
                            };

                            rack.ProductRackShipDates.Add(pRackShipDate);
                        }
                        db.SaveChanges();

                        if (rack.ProductRackShipDates.Any())
                        {
                            //Loads ship dates check box list
                            rpAvailableShipDates.DataSource = rack.ProductRackShipDates;
                            rpAvailableShipDates.DataBind();
                        }
                        else
                        {
                            spAvailableShipDates.InnerText = "There are no ship dates for this rack";
                        }

                        litPageHeader.Text = "Rack: " + rack.RackName;
                        LoadPhotos(rack);
                        LoadOrders();
                    }
                    LoadAssigned();

                    txtActiveTab.Text = "tab-item-details";
                    ddlRackType.Enabled = false;
                }
                else
                {
                    litPageHeader.Text = "New Rack";
                    txtActiveTab.Text = "tab-item-details";
                    txtDisplayOrder.Text = "0"; //default displayOrder to 0
                    chkBoxIsActive.Checked = true;

                    //Hide until we have successfully created a rack
                    tabAssociations.Visible = false;
                    tabRackAssociations.Visible = false;
                    tabOrders.Visible = false;
                    tabPictures.Visible = false;
                    btnDeleteRack.Visible = false;
                    ddlRackType.Enabled = true;
                }
            }
        }

        /// <summary>
        /// Load catalogs
        /// </summary>
        private void LoadCatalog()
        {
            try
            {
                using (MadduxEntities db = new MadduxEntities())
                {
                    var items = db.ProductCatalogs
                        .OrderByDescending(x => x.CatalogYear)
                        .Select(x => new ListItem
                        {
                            Text = x.CatalogName,
                            Value = x.CatalogId.ToString()
                        })
                        .ToList();

                    ddlRackCatalog.DataSource = items;
                    ddlRackCatalog.DataTextField = "Text";
                    ddlRackCatalog.DataValueField = "Value";
                    ddlRackCatalog.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Populates Rack size dropdown
        /// </summary>
        private void LoadRackSize()
        {
            //Apparently they only offer this two sizes so they asked for the sizes table to be remove
            //and to hardcode this values
            List<string> rackSizes = new List<string>
            {
                    "1/4",
                    "1/2",
                    "Full"
            };

            List<ListItem> items = rackSizes.Select(x => new ListItem
            {
                Text = x,
                Value = x
            })
            .ToList();

            ddlRackSize.DataSource = items;
            ddlRackSize.DataTextField = "Text";
            ddlRackSize.DataValueField = "Value";
            ddlRackSize.DataBind();
        }

        /// <summary>
        /// Populates the Discount Options dropdown
        /// </summary>
        private void LoadDiscountOptions()
        {
            List<ListItem> items = new List<ListItem>();

            items.Add(new ListItem("None", "0"));
            items.Add(new ListItem("5%", "0.05"));
            items.Add(new ListItem("10%", "0.10"));
            items.Add(new ListItem("15%", "0.15"));
            ddlDiscount.DataSource = items;
            ddlDiscount.DataTextField = "Text";
            ddlDiscount.DataValueField = "Value";
            ddlDiscount.DataBind();
        }

        /// <summary>
        /// Populates orders table
        /// </summary>
        private void LoadOrders()
        {
            using (MadduxEntities db = new MadduxEntities())
            {
                if (RackType == (int)Redbud.BL.RackType.Standard)
                {
                    gridRackOrders.DataSource = db.OrderRacks
                        .Where(x => x.RackId == RackID)
                        .OrderByDescending(x => x.Quantity)
                        .ThenByDescending(x => x.Order.OrderDate)
                        .ThenBy(x => x.Order.Customer.Company)
                        .ToList();
                } else
                {
                    gridRackOrders.DataSource = db.OrderRacks
                        .Where(x => x.Order.BulkRackID == RackID)
                        .OrderByDescending(x => x.Quantity)
                        .ThenByDescending(x => x.Order.OrderDate)
                        .ThenBy(x => x.Order.Customer.Company)
                        .ToList();
                }

                gridRackOrders.DataBind();
            }
        }

        /// <summary>
        /// Binds the Photos 
        /// </summary>
        /// <param name="theRack"></param>
        private void LoadPhotos(ProductCatalogRack theRack)
        {
            if (theRack.Photos.Any())
            {
                rptPhotos.DataSource = theRack.Photos;
                rptPhotos.DataBind();
            }
        }

        /// <summary>
        /// Cancels edits
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("/racks/racklist.aspx");
        }

        /// <summary>
        /// Handles deleting a rack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDeleteRack_Click(object sender, EventArgs e)
        {
            using (MadduxEntities db = new MadduxEntities())
            {
                List<RackProduct> rackProducts =
                    db.RackProducts
                      .Where(x => x.RackID == RackID)
                      .ToList();

                foreach (RackProduct rackProduct in rackProducts)
                {
                    db.RackProducts.Remove(rackProduct);
                }

                db.SaveChanges();

                ProductCatalogRack productCatalogRack =
                    db.ProductCatalogRacks
                      .FirstOrDefault(x => x.RackID == RackID);

                if (productCatalogRack != null)
                {
                    db.ProductCatalogRacks.Remove(productCatalogRack);
                    db.SaveChanges();
                }

                successAlert.Visible = true;
                spSuccessMessage.InnerText = "Rack  deleted successfully.";

                Response.AddHeader("REFRESH", "1;URL=/racks/racklist.aspx");
            }
        }

        /// <summary>
        /// Handles saving details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Save())
            {
                successAlert.Visible = true;
                spSuccessMessage.InnerText = "Rack  saved successfully.";
                Response.AddHeader("REFRESH", "1;URL=/racks/racklist.aspx");
            }
        }

        public string GetDimensions()
        {
            string Dimensions = string.Empty;
            if (!string.IsNullOrWhiteSpace(RackLength.Text))
            {
                Dimensions += $"{RackLength.Text}x";
                if (!string.IsNullOrWhiteSpace(RackWidth.Text))
                {
                    Dimensions += $"{RackWidth.Text}x";
                }
                else
                {
                    Dimensions += $"1x";
                }
                if (!string.IsNullOrWhiteSpace(RackHeight.Text))
                {
                    Dimensions += $"{RackHeight.Text}";
                }
                else
                {
                    Dimensions += $"1";
                }
            }
            return Dimensions;
        }

        /// <summary>
        /// Handles updating / creating rack
        /// </summary>
        /// <returns>bool</returns>
        private bool Save()
        {
            bool isSuccess;

            using (MadduxEntities db = new MadduxEntities())
            {
                string Dimensions = GetDimensions();
                var catalogID = Convert.ToInt32(ddlRackCatalog.SelectedValue);
                var catalog = db.ProductCatalogs
                    .AsNoTracking()
                    .Include(r => r.ProductCatalogShipDates)
                    .FirstOrDefault(r => r.CatalogId == catalogID && r.Active);

                double discount = 0;
                if (!double.TryParse(ddlDiscount.SelectedValue, out discount))
                {
                    discount = 0;
                }

                if (RackID == 0) //New Rack
                {
                    RackType rackType = Redbud.BL.RackType.Standard;

                    if (!Enum.TryParse(ddlRackType.SelectedValue, out rackType))
                    {
                        rackType = Redbud.BL.RackType.Standard;
                    }
                    var rack =
                        new ProductCatalogRack
                        {
                            RackName = txtRackName.Text,
                            RackDesc = txtRackDescription.Text,
                            DisplayOrder = Convert.ToInt32(txtDisplayOrder.Text),
                            MinimumItems = Convert.ToInt32(txtRackMinimumItems.Text),
                            MaximumItems = Convert.ToInt32(txtRackMaximumItems.Text),
                            RackSize = ddlRackSize.SelectedValue,
                            AllowCustomization = chkAllowCustomization.Checked,
                            CatalogID = Convert.ToInt32(ddlRackCatalog.SelectedValue),
                            Dimensions = Dimensions,
                            Weight = RackWeight.Text.Trim(),
                            FilePath = txtForm.Text.Trim(),
                            Active = chkBoxIsActive.Checked,
                            RackType = (int)rackType,
                            Discount = discount
                        };

                    db.ProductCatalogRacks.Add(rack);

                    //Add the catalog ship dates to the rack
                    foreach (var shipdate in catalog.ProductCatalogShipDates)
                    {
                        var pRackShipDate = new ProductRackShipDate
                        {
                            RackID = rack.RackID,
                            ShipDate = shipdate.ShipDate,
                            OrderDeadlineDate = shipdate.OrderDeadlineDate,
                            Active = true
                        };

                        db.ProductRackShipDates.Add(pRackShipDate);
                    }

                    isSuccess = db.SaveChanges() > 0;
                    tabAssociations.Visible = rack.RackType == (int)Redbud.BL.RackType.Standard;
                    tabRackAssociations.Visible = rack.RackType == (int)Redbud.BL.RackType.Bulk;
                    tabOrders.Visible = true;
                    btnDeleteRack.Visible = true;

                    return isSuccess;
                }
                else //Existing Rack - update
                {
                    //do not allow update of rack type
                    ProductCatalogRack rack = db.ProductCatalogRacks.FirstOrDefault(x => x.RackID == RackID);
                    rack.RackName = txtRackName.Text.ToString();
                    rack.RackDesc = txtRackDescription.Text.ToString();
                    rack.DisplayOrder = Convert.ToInt32(txtDisplayOrder.Text);
                    rack.MinimumItems = Convert.ToInt32(txtRackMinimumItems.Text);
                    rack.MaximumItems = Convert.ToInt32(txtRackMaximumItems.Text);
                    rack.AllowCustomization = chkAllowCustomization.Checked;
                    rack.CatalogID = Convert.ToInt32(ddlRackCatalog.SelectedValue);
                    rack.RackSize = ddlRackSize.SelectedValue.ToString();
                    rack.Dimensions = Dimensions;
                    rack.Weight = RackWeight.Text.Trim();
                    rack.FilePath = txtForm.Text.Trim();
                    rack.Active = chkBoxIsActive.Checked;
                    rack.Discount = discount;

                    //Set active ship dates
                    foreach (RepeaterItem item in rpAvailableShipDates.Items)
                    {
                        CheckBox shipDate = item.FindControl("cbxshipDate") as CheckBox;
                        TextBox quantity = item.FindControl("txtShipDateRacksvailable") as TextBox;
                        HiddenField productRackShipDate = item.FindControl("hfproductRackShipDateID") as HiddenField;

                        //Enable/disable ship date
                        if (shipDate != null)
                        {
                            if (DateTime.TryParse(shipDate.Text, out DateTime date))
                            {
                                if (shipDate.Checked && catalog.ProductCatalogShipDates.Any(pcsd => pcsd.ShipDate == date))
                                {
                                    rack.ProductRackShipDates.Where(x => x.ShipDate == date).FirstOrDefault().Active = true;
                                }
                                else if (!catalog.ProductCatalogShipDates.Any(pcsd => pcsd.ShipDate == date))
                                {
                                    //remove any ship dates that are no longer set on the catalog
                                    var rackShipDate = rack.ProductRackShipDates.Where(x => x.ShipDate == date).FirstOrDefault();
                                    rack.ProductRackShipDates.Remove(rackShipDate);
                                }
                                else
                                {
                                    rack.ProductRackShipDates.Where(x => x.ShipDate == date).FirstOrDefault().Active = false;
                                }
                            }
                        }

                        //Set quantity
                        if (productRackShipDate != null)
                        {
                            int productRackShipDateID = int.Parse(productRackShipDate.Value);
                            if (quantity != null)
                            {
                                ProductRackShipDate rackShipDate = rack.ProductRackShipDates.Where(x => x.ProductRackShipDateID == productRackShipDateID).FirstOrDefault();
                                if (rackShipDate != null)
                                    rackShipDate.Available = int.Parse(quantity.Text);
                            }
                        }
                    }

                    isSuccess = db.SaveChanges() > 0;
                    return isSuccess;
                }
            }
        }

        private string GenerateError(string error)
        {
            return $@"<div class='alert alert-danger'> 
                        <button type='button' class='close' data-dismiss='alert' aria-label='Close'> 
                        <i class='fa fa-times'></i>
                        </button > 
                        <span >{error}</ span ></div >";
        }

        /// <summary>
        /// Load the assigned and available Products/Racks 
        /// </summary>
        private void LoadAssigned()
        {
            try
            {
                using (MadduxEntities db = new MadduxEntities())
                {
                    var rack = db.ProductCatalogRacks
                        .FirstOrDefault(x => x.RackID == RackID);

                    if (rack == null)
                        throw new Exception("rack not found");

                    int totalItems = 0;

                    if (rack.RackType == (int)Redbud.BL.RackType.Standard)
                    {
                        //Standard Rack -  load the Rack Products
                        //load the products
                        db.Entry(rack).Collection(x => x.RackProducts).Load();
                        totalItems = rack.RackProducts.Sum(x => Convert.ToInt32(x.DefaultQuantity));

                        rptAssigned.DataSource = rack.RackProducts.OrderBy(x => x.ProductName);
                        rptAssigned.DataBind();

                        List<int> assignedProductIds = rack.RackProducts.Select(x => x.ProductID).ToList();

                        //Available products (not assigned)
                        rptRackProducts.DataSource = db.Products
                            .Where(x => x.CatalogId == rack.CatalogID && !assignedProductIds.Contains(x.ProductId))
                            .OrderBy(x => x.ProductName)
                            .ToList();
                        rptRackProducts.DataBind();
                        totalQty.Text = totalItems.ToString();

                    }
                    else
                    {
                        //Bulk Rack - Load the RackRacks

                        //load the racks
                        db.Entry(rack).Collection(x => x.RackRacks).Load();

                        totalItems = rack.RackRacks.Sum(x => Convert.ToInt32(x.DefaultQuantity));

                        rptAssignedRacks.DataSource = rack.RackRacks;
                        rptAssignedRacks.DataBind();

                        List<int> assignedRacks = rack.RackRacks.Select(x => x.ProductRackID).ToList();
                        rptRackRacks.DataSource = db.ProductCatalogRacks
                            .Where(x =>
                                x.CatalogID == rack.CatalogID
                                && x.RackType == (int)Redbud.BL.RackType.Standard
                                && x.Active
                                && !assignedRacks.Contains(x.RackID))
                            .OrderBy(x => x.RackID)
                            .ToList();
                        rptRackRacks.DataBind();

                        totalRackQty.Text = totalItems.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                litError.Text = GenerateError(ex.Message);
            }

        }

        /// <summary>
        /// Add Selected rack products handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AddSelected_Click(object sender, EventArgs e)
        {
            try
            {
                Repeater rptProducts = rptRackProducts;
                foreach (RepeaterItem product in rptProducts.Items)
                {
                    CheckBox selected = product.FindControl("chkProductSelected") as CheckBox;
                    if (selected.Checked == true)
                    {
                        HiddenField hiddenField = product.FindControl("hdnProductID") as HiddenField;
                        int productID = Convert.ToInt32(hiddenField.Value);
                        int qty = Convert.ToInt32((product.FindControl("txtProductQuantity") as TextBox).Text);
                        using (MadduxEntities db = new MadduxEntities())
                        {
                            RackProduct rackProduct = db.RackProducts.Create();
                            rackProduct.RackID = RackID;
                            rackProduct.ProductID = productID;
                            rackProduct.DefaultQuantity = qty;
                            db.RackProducts.Add(rackProduct);
                            db.SaveChanges();
                        }
                    }
                }
                LoadAssigned();
                CurrentTab.Value = "associations";
            }
            catch (Exception ex)
            {
                litError.Text = GenerateError(ex.Message);
            }

        }

        /// <summary>
        /// Remove Selected products handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RemoveSelected_Click(object sender, EventArgs e)
        {
            try
            {
                Repeater rptAssignedProducts = rptAssigned;
                foreach (RepeaterItem product in rptAssignedProducts.Items)
                {
                    CheckBox selected = product.FindControl("chkProductSelected") as CheckBox;
                    if (selected.Checked == true)
                    {
                        HiddenField hdnProductID = product.FindControl("hdnProductID") as HiddenField;
                        int pID = Convert.ToInt32(hdnProductID.Value);
                        using (MadduxEntities db = new MadduxEntities())
                        {
                            RackProduct rackProduct = db.RackProducts.FirstOrDefault(x => x.ProductID == pID && x.RackID == RackID);
                            if (rackProduct != null)
                            {
                                db.RackProducts.Remove(rackProduct);
                                db.SaveChanges();
                            }
                        }
                    }
                }
                LoadAssigned();
                CurrentTab.Value = "associations";
            }
            catch (Exception ex)
            {
                litError.Text = GenerateError(ex.Message);
            }
        }

        /// <summary>
        /// Pagination handler on Orders tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridRackOrders_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridRackOrders.PageIndex = e.NewPageIndex;
            LoadOrders();
            gridRackOrders.DataBind();
            CurrentTab.Value = "orders";
        }

        /// <summary>
        /// Updates/Saves the default Quantity for each product in the rack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpdateQty_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (RepeaterItem item in rptAssigned.Items)
                {
                    TextBox qty = item.FindControl("txtProductQuantity") as TextBox;
                    HiddenField productID = item.FindControl("hdnID") as HiddenField;
                    int pID = Convert.ToInt32(productID.Value);
                    using (MadduxEntities db = new MadduxEntities())
                    {
                        RackProduct product = db.RackProducts.FirstOrDefault(x => x.ProductID == pID && x.RackID == RackID);
                        if (product != null)
                        {
                            product.DefaultQuantity = Convert.ToDouble(qty.Text);
                            db.SaveChanges();
                        }
                    }
                }
                LoadAssigned();
                CurrentTab.Value = "associations";
            }
            catch (Exception ex)
            {
                litError.Text = GenerateError(ex.Message);
            }
        }

        /// <summary>
        /// Removes a photo from the rack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DeleteButton_Click1(object sender, EventArgs e)
        {
            try
            {
                int photoID = 0;
                Button deleteButton = (Button)sender;
                using (MadduxEntities db = new MadduxEntities())
                {
                    if (deleteButton != null)
                    {
                        photoID = int.Parse(deleteButton.CommandArgument);
                        Photo photo = db.Photos.FirstOrDefault(x => x.PhotoID == photoID);
                        db.Photos.Remove(photo);
                        db.SaveChanges();
                    }
                    ProductCatalogRack rack = db.ProductCatalogRacks
                        .Include(x => x.Photos)
                        .FirstOrDefault(x => x.RackID == RackID);

                    LoadPhotos(rack);
                }
                CurrentTab.Value = "pictures";
            }
            catch (Exception ex)
            {
                litError.Text = StringTools.GenerateError(ex.Message);
            }
        }

        /// <summary>
        /// Adds a photo to the rack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddPhoto_Click(object sender, EventArgs e)
        {

            Photo photo = null;
            if (string.IsNullOrEmpty(ImageTextBox.Value))
                return;

            using (var db = new MadduxEntities())
            {
                ProductCatalogRack rack = db.ProductCatalogRacks
                    .Include(x => x.Photos)
                    .FirstOrDefault(x => x.RackID == RackID);

                photo = new Photo
                {
                    PhotoPath = ImageTextBox.Value,
                    PhotoDescription = RackID.ToString(),
                    Notes = string.Empty
                };

                db.Photos.Add(photo);
                rack.Photos.Add(photo);

                db.SaveChanges();

                LoadPhotos(rack);
            }

            CurrentTab.Value = "pictures";
            ImageTextBox.Value = null;
        }

        /// <summary>
        /// Updates/Saves the default rack quantity (Bulk Racks) 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpdateRackQty_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (RepeaterItem item in rptAssignedRacks.Items)
                {
                    TextBox qty = item.FindControl("txtRackQuantity") as TextBox;
                    HiddenField rackID = item.FindControl("hdnRID") as HiddenField;
                    int pID = Convert.ToInt32(rackID.Value);
                    using (MadduxEntities db = new MadduxEntities())
                    {
                        var productRack = db.RackRacks
                           .FirstOrDefault(x =>
                               x.ProductRackID == pID
                               && x.RackID == RackID);
                        if (productRack != null)
                        {
                            productRack.DefaultQuantity = Convert.ToDouble(qty.Text);
                            db.SaveChanges();
                        }
                    }
                }
                LoadAssigned();
                CurrentTab.Value = "rackAssociations";
            }
            catch (Exception ex)
            {
                litError.Text = GenerateError(ex.Message);
            }
        }

        /// <summary>
        /// Add selected racks handler (Bulk Racks)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AddSelectedRack_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (RepeaterItem productRack in rptRackRacks.Items)
                {
                    CheckBox selected = productRack.FindControl("chkRackSelected") as CheckBox;
                    if (selected.Checked == true)
                    {
                        HiddenField hiddenField = productRack.FindControl("hdnRackID") as HiddenField;
                        int productRackID = Convert.ToInt32(hiddenField.Value);
                        int qty = Convert.ToInt32((productRack.FindControl("txtRackQuantity") as TextBox).Text);
                        using (MadduxEntities db = new MadduxEntities())
                        {
                            var rackProduct = db.RackRacks.Create();
                            rackProduct.RackID = RackID;
                            rackProduct.ProductRackID = productRackID;
                            rackProduct.DefaultQuantity = qty;
                            db.RackRacks.Add(rackProduct);
                            db.SaveChanges();
                        }
                    }
                }
                LoadAssigned();
                CurrentTab.Value = "rackAssociations";
            }
            catch (Exception ex)
            {
                litError.Text = GenerateError(ex.Message);
            }
        }

        /// <summary>
        /// Remove selected racks handler (Bulk Racks)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RemoveSelectedRack_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (RepeaterItem productRack in rptAssignedRacks.Items)
                {
                    CheckBox selected = productRack.FindControl("chkRackSelected") as CheckBox;
                    if (selected.Checked == true)
                    {
                        HiddenField hdnProductID = productRack.FindControl("hdnRackID") as HiddenField;
                        int pID = Convert.ToInt32(hdnProductID.Value);
                        using (MadduxEntities db = new MadduxEntities())
                        {
                            var rackProduct = db.RackRacks.FirstOrDefault(x => x.ProductRackID == pID && x.RackID == RackID);
                            if (rackProduct != null)
                            {
                                db.RackRacks.Remove(rackProduct);
                                db.SaveChanges();
                            }
                        }
                    }
                }
                LoadAssigned();
                CurrentTab.Value = "rackAssociations";
            }
            catch (Exception ex)
            {
                litError.Text = GenerateError(ex.Message);
            }
        }
    }
}