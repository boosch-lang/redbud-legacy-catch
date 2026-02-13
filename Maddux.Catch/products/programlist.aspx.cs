using Redbud.BL.DL;
using Redbud.BL.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Maddux.Catch.products
{
    public partial class programlist : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Literal litPageHeader = (Literal)Master.FindControl("litPageHeader");
            litPageHeader.Text = "Programs";
            if (!Page.IsPostBack)
            {
                Title = "Maddux.Catch | Programs";
                LoadProgramDropDown();
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
                using (MadduxEntities db = new MadduxEntities())
                {
                    int programID = Convert.ToInt32(ddlProgram.SelectedValue.ToString());

                    var catalogs = db.ProductCatalogs
                        .Include("ProductCatalogs.ProductPrograms")
                        .Where(c => c.ProgramID == programID)
                        .Where(c => c.ProductProgram.ProductCatalogs.Any(i => i.ProgramID == programID))
                        .Where(x => x.Active)
                        .Select(x => new
                        {
                            x.ProductProgram.ProgramName,
                            x.CatalogYear,
                            x.CatalogSeason,
                            x.CatalogName,
                            x.CustomerCatalogName,
                            x.Notes,
                            x.ProgramID,
                            x.CatalogId,
                            x.CatalogClassId,
                            x.CatalogGroupId,
                            x.Active
                        })
                        .OrderByDescending(x => x.CatalogYear)
                        .ThenByDescending(x => x.CatalogSeason)
                        .ThenByDescending(x => x.CatalogName)
                        .ToList();

                    gridProgramCatalogs.DataSource = catalogs;
                    gridProgramCatalogs.DataBind();

                    lblProgramRecordCount.Text = catalogs.Count() > 0 ? catalogs.Count() + " record(s) found" : "No records found";
                }

            }
            catch (DbEntityValidationException ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }
        /// <summary>
        /// Populates programs dropdown
        /// </summary>
        private void LoadProgramDropDown()
        {
            try
            {

                using (MadduxEntities db = new MadduxEntities())
                {
                    if (!chkShowAll.Checked)
                    {
                        //Active programs
                        var programList = db.ProductPrograms
                        .Where(x => x.Active == true)
                        .Select(x => new { x.ProgramID, x.ProgramName })
                        .OrderBy(x => x.ProgramName)
                        .ToList();

                        ddlProgram.Items.Clear();

                        foreach (var program in programList)
                        {
                            ListItem listItem = new ListItem(program.ProgramName.ToString().Trim(),
                                program.ProgramID.ToString().Trim());
                            ddlProgram.Items.Add(listItem);
                        }
                    }
                    else
                    {
                        var programList = db.ProductPrograms
                        .Select(x => new { x.ProgramID, x.ProgramName })
                        .OrderBy(x => x.ProgramName)
                        .ToList();

                        ddlProgram.Items.Clear();

                        foreach (var program in programList)
                        {
                            ListItem listItem = new ListItem(program.ProgramName.ToString().Trim(),
                                program.ProgramID.ToString().Trim());
                            ddlProgram.Items.Add(listItem);
                        }
                    }

                }
            }
            catch (DbEntityValidationException ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }

        protected void chkShowAll_CheckedChanged(object sender, EventArgs e)
        {
            LoadProgramDropDown();
        }
        protected void ddlProgram_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGrid();
        }
        /// <summary>
        /// Hangles exporting programs catalogs to csv files
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExportProgram_Click(object sender, EventArgs e)
        {
            //Client asked for the export program button to be removed. Code left here in case we have to put it back
            //in the near future
            try
            {
                using (MadduxEntities db = new MadduxEntities())
                {
                    int programID = Convert.ToInt32(ddlProgram.SelectedValue.ToString());
                    //Only export necessary columns
                    var catalog = db.ProductCatalogs
                         .Where(x => x.ProgramID == programID)
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
                         }
                                 )
                          .OrderBy(x => x.CatalogName)
                          .ToList();

                    ExportCsv(catalog);
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }
        /// <summary>
        /// Handles pagination
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitCatalogsGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridProgramCatalogs.PageIndex = e.NewPageIndex;
            LoadGrid();
            gridProgramCatalogs.DataBind();
        }

        protected void gridPrograms_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridPrograms_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView gv = (GridView)sender;
            // Change the row state
            gv.Rows[e.NewEditIndex].RowState = DataControlRowState.Edit;
        }
        /// <summary>
        /// Export to csv
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="genericList"></param>
        private void ExportCsv<T>(List<T> genericList)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                var headerProperties = typeof(T).GetProperties();

                //Build header
                for (int i = 0; i < headerProperties.Length - 1; i++)
                {
                    stringBuilder.Append(headerProperties[i].Name + ",");
                }
                var lastProperty = headerProperties[headerProperties.Length - 1].Name;
                stringBuilder.Append(lastProperty + Environment.NewLine);
                //Generates rows
                foreach (var item in genericList)
                {
                    var rowValues = typeof(T).GetProperties();
                    for (int i = 0; i < rowValues.Length - 1; i++)
                    {
                        var property = rowValues[i];
                        stringBuilder.Append(property.GetValue(item, null) + ",");
                    }
                    stringBuilder.Append(rowValues[rowValues.Length - 1].GetValue(item, null) + Environment.NewLine);
                }

                Response.Clear();
                //Ouput the file
                Response.AddHeader("Content-Disposition", "attachment; filename=\"" + ddlProgram.SelectedItem.ToString().TrimEnd() + ".csv");
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
    }
}