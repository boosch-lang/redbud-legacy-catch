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
    public partial class productDetail : Page
    {
        private int ProductID
        {
            get
            {
                if (ViewState["ProductID"] == null)
                {
                    ViewState["ProductID"] = Request.QueryString["ProductID"] == null || Request.QueryString["ProductID"] == "" ? 0 : (object)Request.QueryString["ProductID"];
                }
                return Convert.ToInt32(ViewState["ProductID"].ToString());
            }

            set
            {
                ViewState["ProductID"] = value;
            }
        }
        private int CatalogID
        {
            get
            {
                if (ViewState["CatalogID"] == null)
                {
                    ViewState["CatalogID"] = Request.QueryString["catalogId"] == null || Request.QueryString["catalogId"] == "" ? 0 : (object)Request.QueryString["catalogId"];
                }
                return Convert.ToInt32(ViewState["CatalogID"].ToString());
            }

            set
            {
                ViewState["CatalogID"] = value;
            }
        }
        private void LoadDropdowns()
        {
            using (var db = new MadduxEntities())
            {
                ddCatalog.DataSource = db.ProductCatalogs.Where(r => r.Active).Select(r => new ListItem
                {
                    Text = r.CatalogName,
                    Value = r.CatalogId.ToString()
                }).ToList();
                ddCatalog.DataBind();

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
                if (ProductID != 0)
                {

                    using (var db = new MadduxEntities())
                    {
                        Product product = db.Products.FirstOrDefault(r => r.ProductId == ProductID);
                        Literal litPageHeader = (Literal)Master.FindControl("litPageHeader");
                        litPageHeader.Text = product.ProductName;
                        ddCatalog.SelectedValue = product.CatalogId.ToString();
                        txtItemNumber.Text = product.ItemNumber;
                        txtInternalNumber.Text = product.ItemNumberInternal;
                        txtName.Text = product.ProductName;
                        txtUnitCost.Text = product.UnitCost.ToString();
                        txtDescription.Text = product.ProductDesc;
                        txtUnitPrice.Text = product.UnitPrice.ToString();
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
                        //Client asked to remove the subproduct tab - code left in place in case they want this back in the near future
                        //LoadSubProducts(product);
                        LoadOrders(product);
                        LoadShipments(product);
                        LoadPhotos(product);

                        var orders = db.Orders.Where(r => r.OrderItems.Any(i => i.ProductId == ProductID)
                        || r.OrderRacks.Any(q => q.OrderItems.Any(oi => oi.ProductId == ProductID))).Select(r => new
                        {
                            r.OrderID,
                            r.OrderDate,
                            r.Customer.Company,
                            r.CustomerID,
                            Province = r.Customer.State,
                            r.Customer.Phone,
                            r.Customer.Email,
                            Qty = r.OrderItems.Where(f => f.ProductId == ProductID).Sum(f => f.Quantity) +
                            r.OrderItems.Where(f => f.OrderRackId.HasValue && f.OrderRack.OrderItems.Any(y => y.ProductId == ProductID)).Sum(f => f.Quantity)
                        });
                    }
                }
                else
                {
                    Title = "Maddux.Catch | Product";
                    Literal litPageHeader = (Literal)Master.FindControl("litPageHeader");
                    litPageHeader.Text = "New Product";
                    if (CatalogID != 0)
                    {
                        ddCatalog.SelectedValue = CatalogID.ToString();
                    }
                    btnDelete.Visible = false;
                    nav.Visible = false;
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
                        if (fupPhoto.HasFile)
                        {
                            if (string.IsNullOrWhiteSpace(txtPhotoDescription.Text) || string.IsNullOrWhiteSpace(txtPhotoNotes.Text))
                            {
                                throw new Exception("Photo Description / Photo Note is required.");
                            }
                        }
                        Product product = new Product();
                        if (ProductID != 0)
                        {
                            product = db.Products.FirstOrDefault(r => r.ProductId == ProductID);
                        }
                        else
                        {
                            db.Products.Add(product);
                        }

                        product.CatalogId = int.Parse(ddCatalog.SelectedValue);

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

                        if (fupPhoto.HasFile)
                        {

                            string fileExtension = Path.GetExtension(fupPhoto.FileName).ToLower();
                            Guid fileNameGuid = Guid.NewGuid();
                            string fileName = fileNameGuid.ToString() + fileExtension;
                            fupPhoto.SaveAs(Server.MapPath("~/uploads/files/products/") + fileName);

                            Photo thePhoto = new Photo
                            {
                                PhotoID = Convert.ToInt32(product.ProductId),
                                PhotoPath = "//" + Request.Url.Host + "/uploads/files/products/" + fileName,
                                PhotoDescription = txtPhotoDescription.Text,
                                Notes = txtPhotoNotes.Text
                            };

                            product.Photos.Add(thePhoto);
                            db.SaveChanges();

                            LoadPhotos(product);
                        }

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

                        //Client asked for this fields to be remove from front-end, set to 0 for now 
                        //TODO: remove field from db
                        product.UnitSize = 0;
                        //product.UnitWeight = 0;
                        product.WarehouseLocation = 0;


                        int changes = db.SaveChanges();
                        if (changes > 0)
                        {
                            Response.Redirect("/products/productdetail.aspx?productid=" + ProductID, false);
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
                    Product product = db.Products.FirstOrDefault(r => r.ProductId == ProductID);
                    db.Products.Remove(product);

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
        /// Loads sub products
        /// </summary>
        /// <param name="_oProduct">Product</param>
        private void LoadSubProducts(Product _oProduct)
        {
            try
            {
                using (MadduxEntities db = new MadduxEntities())
                {
                    List<vwProductSubProduct> _dsSubProducts = db.vwProductSubProducts
                          .Where(x => x.MainProductId == _oProduct.ProductId)
                          .OrderBy(x => x.CatalogPageStart)
                          .ThenBy(x => x.SubCategoryDesc)
                          .ThenBy(x => x.ProductName).ToList();

                    if (_dsSubProducts.Count == 0)
                    {
                        uwGridSubProducts.Visible = false;
                        lblNoSubProducts.Visible = true;
                    }
                    else
                    {
                        uwGridSubProducts.Visible = true;
                        lblNoSubProducts.Visible = false;

                        uwGridSubProducts.DataSource = _dsSubProducts;
                        uwGridSubProducts.DataBind();
                    }
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
        /// <param name="_oProduct"></param>
        private void LoadOrders(Product _oProduct)
        {
            try
            {
                using (MadduxEntities db = new MadduxEntities())
                {
                    List<vwProductOrder> orders = db.vwProductOrders
                        .Where(x => x.ProductId == _oProduct.ProductId)
                        .OrderByDescending(x => x.OrderID).ToList();

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
                        //Binds data to orders grid
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
        /// <param name="_oProduct"></param>
        private void LoadShipments(Product _oProduct)
        {
            try
            {
                using (MadduxEntities db = new MadduxEntities())
                {
                    List<vwProductShipment> shipments = db.vwProductShipments
                        .Where(x => x.ProductId == _oProduct.ProductId)
                        .OrderByDescending(x => x.ShipmentId).ToList();

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
        /// Deletes photo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteButton_Click(object sender, EventArgs e)
        {

        }

        protected void gridProducts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }

        protected void SubmitOrdersGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                using (MadduxEntities db = new MadduxEntities())
                {
                    Product product = db.Products.FirstOrDefault(r => r.ProductId == ProductID);
                    gridOrders.PageIndex = e.NewPageIndex;
                    LoadOrders(product);
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
                    Product product = db.Products.FirstOrDefault(x => x.ProductId == ProductID);
                    LoadPhotos(product);
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }

    }
}