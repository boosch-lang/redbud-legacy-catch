using Redbud.BL.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Maddux.Catch.catalogs
{
    public partial class cataloglist : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Literal litPageHeader = (Literal)Master.FindControl("litPageHeader");
            litPageHeader.Text = "Catalogs";

            if (!Page.IsPostBack)
            {
                Title = "Maddux.Catch | Catalogs";
                LoadGrid();
            }
        }
        /// <summary>
        /// Populates catalogs grid
        /// </summary>
        private void LoadGrid()
        {
            try
            {
                List<ProductCatalog> catalogs;  //To store the catalogs

                using (MadduxEntities db = new MadduxEntities())
                {
                    catalogs = !chkShowAll.Checked
                        ? db.ProductCatalogs
                           .Where(x => x.Active)
                           .OrderByDescending(x => x.CatalogYear)
                           .ToList()
                        : db.ProductCatalogs
                           .OrderByDescending(x => x.CatalogYear)
                           .ToList();
                    gridCatalogs.DataSource = catalogs;
                    gridCatalogs.DataBind();

                    lblCatalogRecordCount.Text = catalogs.Count() > 0 ? catalogs.Count() + " record(s) found" : "No records found";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Show all catalogs handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkShowAll_CheckedChanged(object sender, EventArgs e)
        {
            LoadGrid();
        }
        /// <summary>
        /// Pagination handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitCatalogsGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridCatalogs.PageIndex = e.NewPageIndex;
            LoadGrid();
            gridCatalogs.DataBind();
        }

        protected void gridCatalogs_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }
        protected void gridCatalogs_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView gv = (GridView)sender;
            // Change the row state
            gv.Rows[e.NewEditIndex].RowState = DataControlRowState.Edit;
        }

        protected void BtnExportCatalogs_Click(object sender, EventArgs e)
        {
            //This is not used as client asked to remove button from front-end
            //TODO: Remove this code once we know client does not want this functionality back at all
            try
            {
                using (MadduxEntities db = new MadduxEntities())
                {
                    //Only export necessary columns

                    var catalog = db.ProductCatalogs
                         .Select(x => new
                         {
                             x.CatalogId,
                             x.CatalogName,
                             x.CatalogGroupId,
                             x.CatalogClassId,
                             x.CatalogSeason,
                             x.CatalogYear,
                             x.Active,
                             x.ShowBothItemNumbers,
                             x.ShowOnMyAccount,
                             x.PhotoPath,
                             x.PDFUrl,
                             x.OrderFormUrl,
                             x.ProgramID,
                             x.Notes
                         })
                          .OrderBy(x => x.CatalogName)
                          .ToList();

                    ExportCsv(catalog);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Generates csv file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="genericList"></param>
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
                        var property = rowValues[i];
                        stringBuilder.Append(property.GetValue(item, null) + ",");
                    }
                    stringBuilder.Append(rowValues[rowValues.Length - 1].GetValue(item, null) + Environment.NewLine);
                }

                Response.Clear();
                //Ouput the file
                Response.AddHeader("Content-Disposition", "attachment; filename=\"catalogs.csv");
                Response.AddHeader("Content-Length", stringBuilder.Length.ToString());
                Response.ContentType = "text/plain";
                Response.Write(stringBuilder);
                Response.Flush();
                Response.End();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}