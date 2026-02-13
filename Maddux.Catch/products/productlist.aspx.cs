using NPOI.OpenXml4Net.OPC;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Redbud.BL.DL;
using Redbud.BL.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Maddux.Catch.products
{
    public partial class productlist : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Literal litPageHeader = (Literal)Master.FindControl("litPageHeader");
            litPageHeader.Text = "Products";

            Title = "Maddux.Catch | Products";

            if (!Page.IsPostBack)
            {
                LoadCatalogDropDown();
                LoadGrid();
            }
        }

        /// <summary>
        /// Populates catalog dropdown
        /// </summary>
        private void LoadCatalogDropDown()
        {
            try
            {
                using (MadduxEntities db = new MadduxEntities())
                {
                    //All catalogs
                    var catalogs =
                        db.ProductCatalogs
                          .Select(x => new { x.CatalogId, x.CatalogYear, x.CatalogName, x.Active })
                          .OrderByDescending(x => x.CatalogYear)
                          .ThenBy(x => x.CatalogName)
                          .ToList();

                    ddlCatalog.Items.Clear();

                    if (!chkShowAll.Checked)
                    {
                        catalogs = catalogs.Where(c => c.Active).ToList();
                    }

                    foreach (var catalog in catalogs)
                    {
                        ListItem listItem = new ListItem(catalog.CatalogName.Trim(), catalog.CatalogId.ToString().Trim());
                        ddlCatalog.Items.Add(listItem);
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }

        /// <summary>
        /// Populates product grid
        /// </summary>
        private void LoadGrid()
        {
            try
            {
                using (MadduxEntities db = new MadduxEntities())
                {
                    int catalogID = Convert.ToInt32(ddlCatalog.SelectedValue.ToString());

                    var products = db.Products.Where(c => c.CatalogId == catalogID)
                        .Where(c => c.Supplier.Products.Any(i => i.SupplierID == c.SupplierID))
                        .Select(x => new
                        {
                            x.ItemNumber,
                            x.ItemNumberInternal,
                            x.ProductId,
                            x.ProductName,
                            x.ProductDesc,
                            x.Size,
                            x.UnitPrice,
                            x.CatalogId,
                            x.Supplier.SupplierName
                        }).ToList();

                    gridProducts.DataSource = products;
                    gridProducts.DataBind();

                    lblRecordCount.Text = products.Count() > 0 ? products.Count() + " record(s) found" : "No records found";
                }

            }
            catch (DbEntityValidationException ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }

        /// <summary>
        /// Catalog dropdown handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlCatalog_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGrid();
        }

        protected void gridProducts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }

        /// <summary>
        /// Show all checkbox handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkShowAll_CheckedChanged(object sender, EventArgs e)
        {
            LoadCatalogDropDown();
        }

        protected void gridProducts_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView gv = (GridView)sender;
            // Change the row state
            gv.Rows[e.NewEditIndex].RowState = DataControlRowState.Edit;
        }

        /// <summary>
        /// Paging handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitProductsGrid_PageIndexChanging(Object sender, GridViewPageEventArgs e)
        {
            gridProducts.PageIndex = e.NewPageIndex;
            LoadGrid();
            gridProducts.DataBind();
        }

        /// <summary>
        /// Export catalog click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExportCatalog_Click(Object sender, EventArgs e)
        {
            try
            {
                using (MadduxEntities db = new MadduxEntities())
                {
                    int catalogID = Convert.ToInt32(ddlCatalog.SelectedValue.ToString());

                    List<vwCatalogExport> catalog = db.vwCatalogExports.Where(x => x.CatalogId == catalogID)
                         .OrderBy(x => x.CatalogPageStart)
                         .ThenBy(x => x.SubCategoryDesc)
                         .ThenBy(x => x.ProductName).ToList();

                    ExportCsv(catalog);
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }

        /// <summary>
        /// Generates csv file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="genericList">List of catalogs</param>
        private void ExportCsv<T>(List<T> genericList)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                PropertyInfo[] headerProperties = typeof(T).GetProperties();

                //Build header
                for (int i = 0; i < headerProperties.Length - 1; i++)
                {
                    stringBuilder.Append(headerProperties[i].Name + ",");
                }
                string lastProperty = headerProperties[headerProperties.Length - 1].Name;
                stringBuilder.Append(lastProperty + Environment.NewLine);
                //Generates rows
                foreach (var item in genericList)
                {
                    PropertyInfo[] rowValues = typeof(T).GetProperties();
                    for (int i = 0; i < rowValues.Length - 1; i++)
                    {
                        PropertyInfo property = rowValues[i];
                        stringBuilder.Append(property.GetValue(item, null) + ",");
                    }
                    stringBuilder.Append(rowValues[rowValues.Length - 1].GetValue(item, null) + Environment.NewLine);
                }

                Response.Clear();
                //Ouput the file
                Response.AddHeader("Content-Disposition", "attachment; filename=\"" + ddlCatalog.SelectedItem.ToString().TrimEnd() + ".csv");
                Response.AddHeader("Content-Length", stringBuilder.Length.ToString());
                Response.ContentType = "text/plain";
                Response.Write(stringBuilder);
                Response.Flush();
                Response.End();
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }

        protected void BtnImportProducts_Click(object sender, EventArgs e)
        {
            try
            {
                if (fuProducts.HasFile)
                {
                    OPCPackage pkg = OPCPackage.Open(fuProducts.PostedFile.InputStream);
                    DataFormatter formatter = new DataFormatter();
                    XSSFWorkbook workbook = new XSSFWorkbook(pkg);

                    ISheet sheet = workbook.GetSheetAt(0); //Sheet1 on the excel file
                    bool hasError = false; //Flag to set if a particular product on the file is wrongly formatted / has required data missing
                    int productsAddedCount = 0;
                    MadduxEntities db = new MadduxEntities();
                    List<string> productsWithErrorList = new List<string>();
                    List<Product> products = db.Products.ToList();
                    List<string> warnings = new List<string>();
                    //Loop through the sheet rows and retrieve the products data
                    for (int row = 1; row <= sheet.LastRowNum; row++)
                    {
                        Product product = new Product();
                        if (sheet.GetRow(row) != null) //null is when the row only contains empty cells
                        {
                            //Product name is cell #0 and required on the file.
                            if (sheet.GetRow(row).GetCell(0) != null && !string.IsNullOrWhiteSpace(getCellValue(sheet.GetRow(row).GetCell(0))))
                            {
                                product.ProductName = getCellValue(sheet.GetRow(row).GetCell(0));
                            }
                            else
                            {
                                hasError = true;
                            }

                            //Unit Cost is cell #1 and required
                            if (sheet.GetRow(row).GetCell(1) != null && sheet.GetRow(row).GetCell(1).NumericCellValue > 0)
                            {
                                product.UnitCost = sheet.GetRow(row).GetCell(1).NumericCellValue;
                            }
                            else
                            {
                                hasError = true;
                            }

                            //Case Price cell is #2 and required
                            if (sheet.GetRow(row).GetCell(2) != null && sheet.GetRow(row).GetCell(2).NumericCellValue > 0)
                            {
                                product.UnitPrice = sheet.GetRow(row).GetCell(2).NumericCellValue;
                            }
                            else
                            {
                                hasError = true;
                            }

                            //Items Per Package is cell #3 and required
                            if (sheet.GetRow(row).GetCell(3) != null && sheet.GetRow(row).GetCell(3).NumericCellValue > 0)
                            {
                                product.ItemsPerPackage = (int)sheet.GetRow(row).GetCell(3).NumericCellValue;
                            }
                            else
                            {
                                hasError = true;
                            }

                            //Units/Case cell is #4 and required
                            if (sheet.GetRow(row).GetCell(4) != null && sheet.GetRow(row).GetCell(4).NumericCellValue > 0)
                            {
                                product.PackagesPerUnit = (int)sheet.GetRow(row).GetCell(4).NumericCellValue;
                            }
                            else
                            {
                                hasError = true;
                            }

                            //New Product cell is #5
                            product.NewItem = sheet.GetRow(row).GetCell(5) != null && Convert.ToInt32(getCellValue(sheet.GetRow(row).GetCell(5))) == 1;

                            //Item Number (Internal) cell is #6
                            if (sheet.GetRow(row).GetCell(6) != null && !string.IsNullOrWhiteSpace(getCellValue(sheet.GetRow(row).GetCell(6))))
                            {
                                product.ItemNumberInternal = getCellValue(sheet.GetRow(row).GetCell(6));
                            }

                            //Item Number (Customer) cell is #7
                            if (sheet.GetRow(row).GetCell(7) != null && !string.IsNullOrWhiteSpace(getCellValue(sheet.GetRow(row).GetCell(7))))
                            {
                                product.ItemNumber = getCellValue(sheet.GetRow(row).GetCell(7));
                            }

                            //Description cell is #8
                            if (sheet.GetRow(row).GetCell(8) != null && !string.IsNullOrWhiteSpace(getCellValue(sheet.GetRow(row).GetCell(8))))
                            {
                                product.ProductDesc = getCellValue(sheet.GetRow(row).GetCell(8));
                            }

                            //Size cell is #9
                            if (sheet.GetRow(row).GetCell(9) != null && !string.IsNullOrWhiteSpace(getCellValue(sheet.GetRow(row).GetCell(9))))
                            {
                                product.Size = getCellValue(sheet.GetRow(row).GetCell(9));
                            }

                            //UPC cell is #10
                            if (sheet.GetRow(row).GetCell(10) != null && !string.IsNullOrWhiteSpace(getCellValue(sheet.GetRow(row).GetCell(10))))
                            {
                                product.UPCCode = getCellValue(sheet.GetRow(row).GetCell(10));
                            }

                            //Suggested Retail cell is #11
                            if (sheet.GetRow(row).GetCell(11) != null && !string.IsNullOrWhiteSpace(getCellValue(sheet.GetRow(row).GetCell(11))))
                            {
                                product.SuggestedPackageRetail = getCellValue(sheet.GetRow(row).GetCell(11));
                            }


                            //Supplier Name cell is #12
                            if (sheet.GetRow(row).GetCell(12) != null && !string.IsNullOrWhiteSpace(getCellValue(sheet.GetRow(row).GetCell(12))))
                            {
                                string name = getCellValue(sheet.GetRow(row).GetCell(12));
                                Supplier supplier = db.Suppliers.Where(s => s.SupplierName == name).FirstOrDefault();
                                if (supplier != null)
                                {
                                    product.SupplierID = supplier.SupplierID;
                                }
                                else
                                {
                                    //Supplier ID set to 76 = N/A as it is a required field
                                    product.SupplierID = 76;
                                    warnings.Add($"Supplier name {name} doesn't exist in row {sheet.GetRow(row).GetCell(0).StringCellValue}!");
                                }

                            }
                            else
                            {
                                //Supplier ID set to 76 = N/A as it is a required field
                                product.SupplierID = 76;
                            }

                            //Client asked for this fields to be removed from front-end, set to 0 for now
                            product.UnitSize = 0;
                            product.UnitWeight = 0;
                            product.WarehouseLocation = 0;
                            //Catalog ID set to 322 = Uncategorized as it is a required field
                            product.CatalogId = 322;

                            //ProductSubCategoryId set to 1 = N/A as it is a required field
                            product.ProductSubCategoryId = 1;

                            if (!hasError)
                            {
                                //Add the product if we did not encounter an error
                                db.Products.Add(product);
                                productsAddedCount++;

                            }
                            else
                            {
                                productsWithErrorList.Add(sheet.GetRow(row).GetCell(0).StringCellValue);
                            }

                        }

                    }
                    if (warnings.Count > 0)
                    {
                        string wMessage = "";
                        foreach (var warning in warnings)
                        {
                            wMessage += warning + "<br/>";
                        }
                        litWarnings.Text = StringTools.GenerateWarning(wMessage);
                    }
                    db.SaveChanges();

                    if (productsAddedCount > 0)
                    {
                        litMessage.Text = StringTools.GenerateSuccess($"{productsAddedCount} Products Added!");
                    }

                    string csv = "The product/s below had wrongly formatted data. All other products were imported correctly." + Environment.NewLine;
                    if (productsWithErrorList.Count > 0)
                    {
                        csv += string.Join(Environment.NewLine, productsWithErrorList.Select(x => x.ToString()).ToArray());
                        Response.AddHeader("Content-Disposition", "download; filename=\"ProductsWithErrors.csv");
                        Response.AddHeader("Content-Length", csv.Length.ToString());
                        Response.ContentType = "application/octet-stream";
                        Response.BufferOutput = true;
                        Response.Write(csv);
                        Response.End();
                    }
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }

        protected void BtnDownloadProductsImportTemplate_Click(object sender, EventArgs e)
        {
            string filePath = Server.MapPath($@"~/App_Data/templates/Redbud_Product_Upload_Template.xlsx");
            byte[] Content = File.ReadAllBytes(filePath);
            string fileName = "Redbud_Product_Upload_Template";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment; filename=" + fileName + ".xlsx");
            Response.BufferOutput = true;
            Response.OutputStream.Write(Content, 0, Content.Length);
            Response.End();
        }
        private string getCellValue(ICell cell)
        {
            string value;
            switch (cell.CellType)
            {
                case CellType.String:
                    value = cell.StringCellValue;
                    break;
                case CellType.Numeric:
                    value = Convert.ToString(cell.NumericCellValue);
                    break;
                case CellType.Boolean:
                    value = cell.BooleanCellValue ? "1" : "0";
                    break;
                default:
                    value = "";
                    break;
            }
            return value;
        }
    }
}