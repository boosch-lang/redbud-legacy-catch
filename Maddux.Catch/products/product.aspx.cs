using Redbud.BL.DL;
using Redbud.BL.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Maddux.Catch.products
{
    public partial class product : System.Web.UI.Page
    {
        //TEST: ?ItemNumberInternal=010396
        private string ItemNumberInternal
        {
            get
            {
                if (ViewState["ItemNumberInternal"] == null)
                {
                    ViewState["ItemNumberInternal"] = Request.QueryString["ItemNumberInternal"] == null || Request.QueryString["ItemNumberInternal"] == "" ? string.Empty : (object)Request.QueryString["ItemNumberInternal"];
                }
                return ViewState["ItemNumberInternal"].ToString();
            }

            set
            {
                ViewState["ItemNumberInternal"] = value;
            }
        }
        private void LoadDropdowns()
        {
            using (var db = new MadduxEntities())
            {
                ddlNewCatalog.DataSource = db.ProductCatalogs.Where(r => r.Active).Select(r => new ListItem
                {
                    Text = r.CatalogName,
                    Value = r.CatalogId.ToString()
                }).ToList();
                ddlNewCatalog.DataBind();

                ddCategory.DataSource = db.supProductSubCategories.Select(r => new ListItem
                {
                    Text = r.SubCategoryDesc,
                    Value = r.SubCategoryID.ToString()
                }).ToList();
                ddCategory.DataBind();

                ddSupplier.DataSource = db.Suppliers.Select(r => new ListItem
                {
                    Text = r.SupplierName,
                    Value = r.SupplierID.ToString()
                }).ToList();
                ddSupplier.DataBind();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Title = "Maddux.Catch | Product Details";
                LoadDropdowns();
                if (!string.IsNullOrEmpty(ItemNumberInternal))
                {
                    using (var db = new MadduxEntities())
                    {
                        List<Product> products = db.Products.Where(x => x.ItemNumberInternal == ItemNumberInternal).OrderByDescending(x => x.ProductId).ToList();
                        Product product = products.FirstOrDefault();
                        Literal litPageHeader = (Literal)Master.FindControl("litPageHeader");

                        litPageHeader.Text = product.ProductName;
                        txtInternalNumber.Text = product.ItemNumberInternal;
                        txtName.Text = product.ProductName;
                        txtDescription.Text = product.ProductDesc;
                        ddCategory.SelectedValue = product.ProductSubCategoryId.ToString();
                        txtSuggestedRetail.Text = product.SuggestedPackageRetail;
                        txtSize.Text = product.Size;
                        ddSupplier.SelectedValue = product.SupplierID.ToString();
                        txtItemsPerPAckage.Text = product.ItemsPerPackage.ToString();
                        txtUPCCode.Text = product.UPCCode;
                        txtPacksPerUnit.Text = product.PackagesPerUnit.ToString();
                        chkNewProd.Checked = product.NewItem;
                        chkNotAvailable.Checked = product.ProductNotAvailable;
                        txtNotAvailableMsg.Text = product.ProductNotAvailableDesc;
                        txtProductWeight.Text = product.UnitWeight.ToString();

                        LoadOrders(products);
                        LoadShipments(products);
                        LoadPhotos(product);
                        LoadCatalogs(products);
                        phNewProduct.Visible = false;
                    }
                }
                else
                {
                    Title = "Maddux.Catch | Product";
                    Literal litPageHeader = (Literal)Master.FindControl("litPageHeader");
                    litPageHeader.Text = "New Product";
                    btnDelete.Visible = false;
                    nav.Visible = false;

                    using (var db = new MadduxEntities())
                    {
                        gvNewCatalogs.DataSource = db.ProductCatalogs.Where(r => r.Active).ToList();
                        gvNewCatalogs.DataBind();
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                using (var db = new MadduxEntities())
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(ItemNumberInternal))
                        {
                            if (db.Products.Any(x => x.ItemNumberInternal == txtInternalNumber.Text))
                            {
                                throw new Exception("A product with that internal item number already exists.");
                            }
                            var hasChecked = false;

                            foreach (GridViewRow row in gvNewCatalogs.Rows)
                            {
                                CheckBox cbSelected = row.FindControl("cbSelected") as CheckBox;
                                TextBox txtItemNumber = row.FindControl("txtItemNumber") as TextBox;
                                TextBox txtUnitCost = row.FindControl("txtUnitCost") as TextBox;
                                TextBox txtUnitPrice = row.FindControl("txtUnitPrice") as TextBox;

                                if (!cbSelected.Checked)
                                    continue;

                                hasChecked = true;

                                Product product = new Product();
                                db.Products.Add(product);

                                product.CatalogId = (int)gvNewCatalogs.DataKeys[row.RowIndex].Value;
                                product.ItemNumber = txtItemNumber.Text;
                                product.ItemNumberInternal = txtInternalNumber.Text;
                                if (int.TryParse(txtItemsPerPAckage.Text, out int itemspackage))
                                {
                                    product.ItemsPerPackage = itemspackage;
                                }
                                product.NewItem = chkNewProd.Checked;
                                if (int.TryParse(txtPacksPerUnit.Text, out int packsunit))
                                {
                                    product.PackagesPerUnit = packsunit;
                                }
                                product.ProductCode = txtUPCCode.Text;
                                product.ProductDesc = txtDescription.Text;
                                product.ProductName = txtName.Text;
                                product.ProductNotAvailable = chkNotAvailable.Checked;
                                product.ProductNotAvailableDesc = txtNotAvailableMsg.Text;
                                product.ProductSubCategoryId = int.Parse(ddCategory.SelectedValue);
                                product.Size = txtSize.Text;
                                product.SuggestedPackageRetail = txtSuggestedRetail.Text;
                                product.UnitWeight = !string.IsNullOrWhiteSpace(txtProductWeight.Text) ? Convert.ToDouble(txtProductWeight.Text) : 0;
                                product.SupplierID = int.Parse(ddSupplier.SelectedValue);
                                if (double.TryParse(txtUnitCost.Text, out double unitcost))
                                {
                                    product.UnitCost = unitcost;
                                }

                                if (double.TryParse(txtUnitPrice.Text, out double unitprice))
                                {
                                    product.UnitPrice = unitprice;
                                }
                                product.UPCCode = txtUPCCode.Text;
                                product.UnitSize = 0;
                                product.WarehouseLocation = 0;
                            }

                            if (!hasChecked)
                                throw new Exception("You must select at least one catalog.");

                            int changes = db.SaveChanges();
                            if (changes > 0)
                            {
                                Response.Redirect("/products/product.aspx?ItemNumberInternal=" + txtInternalNumber.Text.Trim(), false);
                            }
                        }
                        else
                        {
                            Photo photo = null;
                            if (fupPhoto.HasFile)
                            {

                                string fileExtension = Path.GetExtension(fupPhoto.FileName).ToLower();
                                Guid fileNameGuid = Guid.NewGuid();
                                string fileName = fileNameGuid.ToString() + fileExtension;
                                fupPhoto.SaveAs(Server.MapPath("~/uploads/files/products/") + fileName);

                                photo = new Photo
                                {
                                    PhotoPath = "//" + Request.Url.Host + "/uploads/files/products/" + fileName,
                                    PhotoDescription = txtPhotoDescription.Text,
                                    Notes = txtPhotoNotes.Text
                                };
                                db.Photos.Add(photo);
                                db.SaveChanges();
                            }

                            List<Product> products = db.Products.Where(x => x.ItemNumberInternal == ItemNumberInternal).OrderByDescending(x => x.ProductId).ToList();
                            foreach (Product product in products)
                            {
                                product.ItemNumberInternal = txtInternalNumber.Text;
                                if (int.TryParse(txtItemsPerPAckage.Text, out int itemspackage))
                                {
                                    product.ItemsPerPackage = itemspackage;
                                }
                                if (int.TryParse(txtPacksPerUnit.Text, out int packsunit))
                                {
                                    product.PackagesPerUnit = packsunit;
                                }
                                product.NewItem = chkNewProd.Checked;
                                product.ProductCode = txtUPCCode.Text;
                                product.ProductDesc = txtDescription.Text;
                                product.ProductName = txtName.Text;
                                product.ProductNotAvailable = chkNotAvailable.Checked;
                                product.ProductNotAvailableDesc = txtNotAvailableMsg.Text;
                                product.ProductSubCategoryId = int.Parse(ddCategory.SelectedValue);
                                product.Size = txtSize.Text;
                                product.SuggestedPackageRetail = txtSuggestedRetail.Text;
                                product.UnitWeight = !string.IsNullOrWhiteSpace(txtProductWeight.Text) ? Convert.ToDouble(txtProductWeight.Text) : 0;
                                product.SupplierID = int.Parse(ddSupplier.SelectedValue);
                                product.UPCCode = txtUPCCode.Text;
                                product.UnitSize = 0;
                                product.WarehouseLocation = 0;

                                if (photo != null)
                                {
                                    product.Photos.Add(photo);
                                }

                            }
                            int changes = db.SaveChanges();
                            if (changes > 0)
                            {
                                litMessage.Text = StringTools.GenerateSuccess("Your changes have been saved.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        litMessage.Text = StringTools.GenerateError(ex.Message);
                    }
                }
            }
        }
        /// <summary>
        /// Deletes a product
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                using (MadduxEntities db = new MadduxEntities())
                {
                    List<Product> products = db.Products.Where(x => x.ItemNumberInternal == ItemNumberInternal).OrderByDescending(x => x.ProductId).ToList();
                    db.Products.RemoveRange(products);
                    db.SaveChanges();
                    Response.Redirect("/Products/ProductList.aspx");
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }
        /// <summary>
        /// Populates orders tab
        /// </summary>
        /// <param name="product"></param>
        private void LoadOrders(List<Product> products)
        {
            try
            {
                using (MadduxEntities db = new MadduxEntities())
                {
                    var productIDs = products.Select(p => p.ProductId).ToList();
                    List<vwProductOrder> orders = db.vwProductOrders
                        .Where(x => productIDs.Contains(x.ProductId))
                        .OrderByDescending(x => x.OrderID)
                        .ToList();

                    if (orders.Count == 0)
                    {
                        gridOrders.Visible = false;
                        lblQtyOrdered.Visible = false;
                        lblNoOrders.Visible = true;
                    }
                    else
                    {
                        gridOrders.Visible = true;
                        lblQtyOrdered.Visible = true;
                        lblNoOrders.Visible = false;
                        gridOrders.DataSource = orders;
                        gridOrders.DataBind();

                        lblQtyOrdered.Text = "Total ordered:  " + (from order in orders select order.Quantity).Sum();
                    }
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }
        /// <summary>
        /// Populates shipments tab
        /// </summary>
        /// <param name="product"></param>
        private void LoadShipments(List<Product> products)
        {
            try
            {
                using (MadduxEntities db = new MadduxEntities())
                {
                    var productIDs = products.Select(p => p.ProductId).ToList();
                    List<vwProductShipment> shipments = db.vwProductShipments.Where(x => productIDs.Contains(x.ProductId)).OrderByDescending(x => x.ShipmentId).ToList();

                    if (shipments.Count == 0)
                    {
                        uwGridShipments.Visible = false;
                        lblQtyShipped.Visible = false;
                        lblNoShipments.Visible = true;
                    }
                    else
                    {
                        uwGridShipments.Visible = true;
                        lblQtyShipped.Visible = true;
                        lblNoShipments.Visible = false;

                        uwGridShipments.DataSource = shipments;
                        uwGridShipments.DataBind();

                        lblQtyShipped.Text = "Total shipped:  " + (from shipment in shipments select shipment.Quantity).Sum();
                    }
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }
        /// <summary>
        /// Load product photos
        /// </summary>
        /// <param name="theProduct"></param>
        private void LoadPhotos(Product theProduct)
        {
            try
            {
                ICollection<Photo> photos = theProduct.Photos;

                if (photos.Count() > 0)
                {
                    rptPhotos.DataSource = photos;
                    rptPhotos.DataBind();
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }
        /// <summary>
        /// Load product categories
        /// </summary>
        /// <param name="theProduct"></param>
        private void LoadCatalogs(List<Product> products)
        {
            try
            {
                gvCatalogs.DataSource = products;
                gvCatalogs.DataBind();
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }

        protected void SubmitOrdersGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                using (MadduxEntities db = new MadduxEntities())
                {
                    Product product = db.Products.OrderByDescending(x => x.ProductId).FirstOrDefault(r => r.ItemNumberInternal == ItemNumberInternal);
                    List<Product> products = db.Products.Where(x => x.ItemNumberInternal == product.ItemNumberInternal).OrderByDescending(x => x.ProductId).ToList();
                    gridOrders.PageIndex = e.NewPageIndex;
                    LoadOrders(products);
                    gridOrders.DataBind();
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }
        /// <summary>
        /// Cancel create/edit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Products/ProductList.aspx");
        }

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
                    Product product = db.Products.OrderByDescending(x => x.ProductId).FirstOrDefault(r => r.ItemNumberInternal == ItemNumberInternal);
                    LoadPhotos(product);
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }

        protected void btnAddCatalog_Click(object sender, EventArgs e)
        {
            try
            {
                var catalogID = int.Parse(ddlNewCatalog.SelectedValue);
                var productID = 0;
                using (var db = new MadduxEntities())
                {
                    List<Product> products = db.Products.Where(x => x.ItemNumberInternal == ItemNumberInternal).OrderByDescending(x => x.ProductId).ToList();
                    Product product = products.FirstOrDefault();

                    if (products.Any(x => x.CatalogId == catalogID))
                    {
                        throw new Exception("This offering already exists");
                    }

                    Product newProduct = new Product
                    {
                        CatalogId = catalogID,
                        ItemNumber = product.ItemNumber,
                        ItemNumberInternal = product.ItemNumberInternal,
                        ItemsPerPackage = product.ItemsPerPackage,
                        NewItem = product.NewItem,
                        PackagesPerUnit = product.PackagesPerUnit,
                        PotDescription = product.PotDescription,
                        ProductCode = product.ProductCode,
                        ProductDesc = product.ProductDesc,
                        ProductName = product.ProductName,
                        ProductNotAvailable = product.ProductNotAvailable,
                        ProductNotAvailableDesc = product.ProductNotAvailableDesc,
                        ProductSubCategoryId = product.ProductSubCategoryId,
                        Size = product.Size,
                        SuggestedPackageRetail = product.SuggestedPackageRetail,
                        SupplierID = product.SupplierID,
                        TagsIncluded = product.TagsIncluded,
                        UnitCost = product.UnitCost,
                        UnitPrice = product.UnitPrice,
                        UnitSize = product.UnitSize,
                        UnitWeight = product.UnitWeight,
                        UPCCode = product.UPCCode,
                        WarehouseLocation = product.WarehouseLocation
                    };

                    db.Products.Add(newProduct);
                    db.SaveChanges();

                    foreach (Photo photo in product.Photos)
                    {
                        newProduct.Photos.Add(photo);
                    }

                    db.SaveChanges();
                    productID = product.ProductId;
                }

                //Get row index based off of id
                int index = 0;
                foreach (GridViewRow row in gvCatalogs.Rows)
                {
                    var dataKey = gvCatalogs.DataKeys[row.DataItemIndex];

                    if (dataKey == null || (int)dataKey.Value != productID)
                        continue;

                    index = row.DataItemIndex;
                }
                gvCatalogs.EditIndex = index;
                refreshCatalogs();
                TabName.Value = "catalogs";
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }

        public void refreshCatalogs()
        {
            using (var db = new MadduxEntities())
            {
                List<Product> products = db.Products.Where(x => x.ItemNumberInternal == ItemNumberInternal).OrderByDescending(x => x.ProductId).ToList();
                LoadCatalogs(products);
            }
        }

        protected void gvCatalogs_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvCatalogs.EditIndex = -1;
            refreshCatalogs();
            TabName.Value = "catalogs";
        }

        protected void gvCatalogs_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int productID = (int)gvCatalogs.DataKeys[e.RowIndex].Value;

            using (MadduxEntities db = new MadduxEntities())
            {
                Product product = db.Products.FirstOrDefault(x => x.ProductId == productID);
                if (product != null)
                {
                    db.Products.Remove(product);
                    db.SaveChanges();
                }
            }

            refreshCatalogs();
            TabName.Value = "catalogs";
            if (gvCatalogs.Rows.Count == 0)
                Response.Redirect("/products/productlist.aspx");
        }

        protected void gvCatalogs_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvCatalogs.EditIndex = e.NewEditIndex;
            refreshCatalogs();
            TabName.Value = "catalogs";
        }

        protected void gvCatalogs_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Page.Validate("edit");
            int productID = (int)gvCatalogs.DataKeys[e.RowIndex].Value;
            GridViewRow row = (GridViewRow)gvCatalogs.Rows[e.RowIndex];

            TextBox txtItemNumber = row.Cells[1].FindControl("txtItemNumber") as TextBox;
            TextBox txtUnitCost = row.Cells[2].FindControl("txtUnitCost") as TextBox;
            TextBox txtUnitPrice = row.Cells[3].FindControl("txtUnitPrice") as TextBox;

            if (Page.IsValid)
            {
                using (MadduxEntities db = new MadduxEntities())
                {
                    Product product = db.Products.FirstOrDefault(x => x.ProductId == productID);
                    if (product != null)
                    {

                        product.ItemNumber = txtItemNumber.Text.Trim();
                        if (double.TryParse(txtUnitCost.Text, out double unitcost))
                        {
                            product.UnitCost = unitcost;
                        }

                        if (double.TryParse(txtUnitPrice.Text, out double unitprice))
                        {
                            product.UnitPrice = unitprice;
                        }
                    }
                    db.SaveChanges();
                }

                gvCatalogs.EditIndex = -1;
                refreshCatalogs();
                TabName.Value = "catalogs";
            }
        }
    }
}