using Redbud.BL.DL;
using Redbud.BL.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Maddux.Catch.racks
{
    public partial class racklist : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    Title = "Maddux | Racks";
                    Literal litPageHeader = (Literal)Master.FindControl("litPageHeader");
                    litPageHeader.Text = "Racks";
                    LoadDropDown();
                    LoadRacks();
                }
                catch (Exception ex)
                {

                    litMessage.Text = StringTools.GenerateError(ex.Message);
                }

            }
        }
        private void LoadDropDown()
        {
            using (MadduxEntities db = new MadduxEntities())
            {
                List<ListItem> catalogs = db.ProductCatalogs.OrderByDescending(pc => pc.CatalogYear).ThenBy(x => x.CatalogName).Select(pc => new ListItem { Text = pc.CatalogName, Value = pc.CatalogId.ToString() }).ToList();
                catalogs.Insert(0, new ListItem { Text = "--- Select All Catalogs ---", Value = "0" });
                ddlCatalogs.DataTextField = "Text";
                ddlCatalogs.DataValueField = "Value";
                ddlCatalogs.DataSource = catalogs;
                ddlCatalogs.DataBind();
            }
        }
        /// <summary>
        /// Populates Gridview
        /// </summary>
        private void LoadRacks()
        {
            try
            {
                using (MadduxEntities db = new MadduxEntities())
                {
                    int catalogID = int.Parse(ddlCatalogs.SelectedValue);
                    IQueryable<ProductCatalogRack> racks = catalogID == 0 ? db.ProductCatalogRacks : db.ProductCatalogRacks.Where(r => r.CatalogID == catalogID);
                    if(!chkShowAll.Checked)
                    {
                        racks = racks.Where(r => r.Active);
                    }

                    dgvRacks.DataSource = racks.OrderByDescending(x => x.RackID).ToList();
                    dgvRacks.DataBind();

                    lblRackRecordCount.Text = racks.Count() > 0 ? racks.Count() + " record(s) found" : "No records found";
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
        protected void SubmitRacksGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvRacks.PageIndex = e.NewPageIndex;
            LoadRacks();
            dgvRacks.DataBind();
        }

        /// <summary>
        /// CSV export handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExportRack_Click(object sender, EventArgs e)
        {
            try
            {
                using (MadduxEntities db = new MadduxEntities())
                {
                    var racks = db.ProductCatalogRacks
                        .Select(x => new
                        {
                            x.RackID,
                            x.RackName,
                            x.RackDesc,
                            x.RackSize,
                            x.MinimumItems,
                            x.MaximumItems,
                            x.AllowCustomization,
                            Photos = x.Photos.Select(p => p.PhotoPath)
                        })
                        .ToList();
                        

                    ExportCsv(racks.Select(x => new
                    {
                        x.RackID,
                        x.RackName,
                        x.RackDesc,
                        x.RackSize,
                        x.MinimumItems,
                        x.MaximumItems,
                        x.AllowCustomization,
                        Photos = string.Join(", ", x.Photos)
                    }).ToList());
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }
        /// <summary>
        /// Generates a CSV file.
        /// </summary>
        /// <typeparam name="T">Type of the objects in the list.</typeparam>
        /// <param name="genericList">List of objects to export.</param>
        private void ExportCsv<T>(List<T> genericList)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                PropertyInfo[] properties = typeof(T).GetProperties();

                // Build header
                for (int i = 0; i < properties.Length; i++)
                {
                    stringBuilder.Append($"\"{properties[i].Name}\"");
                    if (i < properties.Length - 1)
                    {
                        stringBuilder.Append(",");
                    }
                }
                stringBuilder.Append(Environment.NewLine);

                // Generate rows
                foreach (T item in genericList)
                {
                    for (int i = 0; i < properties.Length; i++)
                    {
                        object value = properties[i].GetValue(item, null);
                        stringBuilder.Append($"\"{value?.ToString().Replace("\"", "\"\"")}\"");
                        if (i < properties.Length - 1)
                        {
                            stringBuilder.Append(",");
                        }
                    }
                    stringBuilder.Append(Environment.NewLine);
                }

                Response.Clear();
                Response.ContentType = "text/csv";
                Response.AddHeader("Content-Disposition", "attachment; filename=\"Racks.csv\"");
                Response.AddHeader("Content-Length", stringBuilder.Length.ToString());
                Response.Write(stringBuilder.ToString());
                Response.Flush();
                Response.End();
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }


        protected void ddlCatalogs_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadRacks();
        }

        /// <summary>
        /// Show all racks handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkShowAll_CheckedChanged(object sender, EventArgs e)
        {
            LoadRacks();
        }

    }
}