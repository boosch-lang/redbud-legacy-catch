using Redbud.BL.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Maddux.Catch.categories
{
    public partial class categorylist : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Literal litPageHeader = (Literal)Master.FindControl("litPageHeader");
            litPageHeader.Text = "Categories";

            if (!Page.IsPostBack)
            {
                Title = "Maddux.Catch | Categories";
                LoadGrid();
            }
        }
        /// <summary>
        /// Populates categories table
        /// </summary>
        private void LoadGrid()
        {
            try
            {
                using (MadduxEntities db = new MadduxEntities())
                {
                    List<supProductSubCategory> categories = db.supProductSubCategories
                        .OrderBy(x => x.SubCategoryDesc)
                        .ToList();
                    gridCategories.DataSource = categories;
                    gridCategories.DataBind();

                    lblRecordCount.Text = categories.Count() > 0 ? categories.Count() + " record(s) found" : "No records found";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void gridCategories_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }
        protected void gridCategories_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView gv = (GridView)sender;
            // Change the row state
            gv.Rows[e.NewEditIndex].RowState = DataControlRowState.Edit;
        }
        protected void SubmitCategoriesGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridCategories.PageIndex = e.NewPageIndex;
            LoadGrid();
            gridCategories.DataBind();
        }
        /// <summary>
        /// Csv export handlers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExportCategories_Click(object sender, EventArgs e)
        {
            try
            {
                using (MadduxEntities db = new MadduxEntities())
                {
                    var vategories = db.supProductSubCategories
                        .Select(x => new
                        {
                            x.SubCategoryID,
                            x.SubCategoryDesc,
                            x.ProductCategoryID
                        })
                        .ToList();
                    ExportCsv(vategories);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Export records to csv file
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
                        PropertyInfo property = rowValues[i];
                        stringBuilder.Append(property.GetValue(item, null) + ",");
                    }
                    stringBuilder.Append(rowValues[rowValues.Length - 1].GetValue(item, null) + Environment.NewLine);
                }

                Response.Clear();
                //Ouput the file
                Response.AddHeader("Content-Disposition", "attachment; filename=\"Categories.csv");
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