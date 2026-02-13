using Maddux.Catch.Helpers;
using NPOI.OpenXml4Net.OPC;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Redbud.BL.DL;
using Redbud.BL.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Maddux.Catch.freight
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Literal litPageHeader = (Literal)Master.FindControl("litPageHeader");
            litPageHeader.Text = $@"Freight Charges";
            try
            {
                if (!Page.IsPostBack)
                {
                    LoadDropdown();
                    ddlRegion.SelectedValue = "GTHA Ontario";
                    ddlProvince.SelectedValue = "All";
                    LoadGrid("All", "GTHA Ontario");
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }

        }
        private void LoadGrid(string Province = "All", string Region = "All")
        {
            using (MadduxEntities db = new MadduxEntities())
            {
                List<FreightCharge> charges = db.FreightCharges
                    .Where(x =>
                        (string.Equals(Province, "All") || x.Province == Province)
                        && (string.Equals(Region, "All") || x.Region == Region)
                    )
                    .OrderBy(x => x.Province)
                    .ThenByDescending(x => x.Charge)
                    .ToList();
                
                dgvFreightCharges.DataSource = charges;
                dgvFreightCharges.DataBind();
            }
        }
        protected void btnImportFreightCharges_Click(object sender, EventArgs e)
        {
            if (!ImportFile.HasFile)
            {
                litMessage.Text = StringTools.GenerateError("Please select excel file you want to import.");
                return;
            }
            try
            {
                OPCPackage pkg = OPCPackage.Open(ImportFile.PostedFile.InputStream);
                DataFormatter formatter = new DataFormatter();
                XSSFWorkbook workbook = new XSSFWorkbook(pkg);

                ISheet sheet = workbook.GetSheetAt(0); //Sheet1 on the excel file
                int itemsAddedCount = 0;
                int itemsUpdatedCount = 0;
                using (MadduxEntities db = new MadduxEntities())
                {
                    List<string> productsWithErrorList = new List<string>();
                    List<FreightCharge> charges = db.FreightCharges.ToList();
                    List<FreightCharge> newCharges = new List<FreightCharge>();
                    List<string> regionNames = RegionHelper.GetRegionNames();

                    //Loop through the sheet rows and retrieve the products data
                    for (int row = 1; row <= sheet.LastRowNum; row++)
                    {
                        FreightCharge freightCharge;
                        //null is when the row only contains empty cells 
                        if (sheet.GetRow(row) != null)
                        {
                            bool IsNew = false;
                            decimal charge = 0;
                            try
                            {
                                var areaId = sheet.GetRow(row).GetCell(0).StringCellValue.Trim();
                                var placeName = sheet.GetRow(row).GetCell(1).StringCellValue.Trim();
                                var province = sheet.GetRow(row).GetCell(2).StringCellValue.Trim();
                                var region = sheet.GetRow(row).GetCell(3).StringCellValue.Trim();
                                var rate = sheet.GetRow(row).GetCell(4).NumericCellValue;

                                //skip if we didn't get any of the values
                                if (string.IsNullOrWhiteSpace(areaId) || string.IsNullOrWhiteSpace(province) || string.IsNullOrWhiteSpace(region) || !regionNames.Contains(region))
                                {
                                    continue;
                                }

                                freightCharge = charges.FirstOrDefault(x => x.AreaID == areaId);
                                if (freightCharge == null)
                                {
                                    freightCharge = db.FreightCharges.Create();
                                    freightCharge.AreaID = areaId;
                                    IsNew = true;
                                }

                                freightCharge.PlaceName = placeName != null ? placeName : string.Empty;
                                freightCharge.Province = province;
                                freightCharge.Region = region;
                                freightCharge.Charge = Convert.ToDecimal(rate) * 100;

                                if (IsNew)
                                {
                                    //check if record is repeating in file
                                    bool check = newCharges.Any(x => x.AreaID == freightCharge.AreaID);
                                    if (!check)
                                    {
                                        newCharges.Add(freightCharge);
                                        itemsAddedCount++;
                                    }
                                }
                                else
                                {
                                    db.SaveChanges();
                                    itemsUpdatedCount++;
                                }
                            }
                            catch { }
                        }
                    }
                    if (newCharges.Count > 0)
                    {
                        db.FreightCharges.AddRange(newCharges);
                        db.SaveChanges();
                    }
                    if (itemsAddedCount > 0 && itemsUpdatedCount > 0)
                    {
                        litMessage.Text = StringTools.GenerateSuccess($"{itemsAddedCount} new records added and {itemsUpdatedCount} records were updated.");
                    }
                    else
                    {
                        litMessage.Text = itemsUpdatedCount == 0 && itemsAddedCount > 0
                            ? StringTools.GenerateSuccess($"{itemsAddedCount} new records added.")
                            : StringTools.GenerateSuccess($"{itemsUpdatedCount} records updated.");
                    }
                    LoadGrid();
                }
            }
            catch (Exception ex)
            {

                litMessage.Text = StringTools.GenerateError(ex.Message);
            }

        }

        protected void dgvFreightCharges_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
        private void LoadDropdown()
        {
            using (MadduxEntities db = new MadduxEntities())
            {
                var provinces = db.States.Where(x => x.Country.TrimEnd() == "Canada").Select(x => new ListItem
                {
                    Text = x.StateName,
                    Value = x.StateID
                }).ToList();
                var item = new ListItem { Text = "All Provinces", Value = "All" };
                provinces.Insert(0, item);
                ddlProvince.DataSource = provinces;
                ddlProvince.DataBind();
            }

            //Regions
            var regions = RegionHelper.GetRegionsForSelectList(true);
            ddlRegion.DataSource = regions;
            ddlRegion.DataBind();
        }
        protected void Search_ServerClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(SearctCriteria.Text))
            {
                string str = SearctCriteria.Text;
                try
                {
                    using (MadduxEntities db = new MadduxEntities())
                    {
                        var charge = db.FreightCharges.Where(x => x.AreaID.Contains(str) || x.PlaceName.Contains(str));
                        dgvFreightCharges.DataSource = charge.OrderBy(x => x.Province).ThenBy(x => x.Charge).ToList();
                        dgvFreightCharges.DataBind();
                    }
                }
                catch (Exception ex)
                {

                    litMessage.Text = StringTools.GenerateError(ex.Message);
                }
            }
            else
            {
                return;
            }
        }

        protected void ddlProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadGrid(ddlProvince.SelectedValue, ddlRegion.SelectedValue);
            }
            catch (Exception ex)
            {

                litMessage.Text = StringTools.GenerateError(ex.Message);
            }

        }

        protected void ddlRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadGrid(ddlProvince.SelectedValue, ddlRegion.SelectedValue);
            }
            catch (Exception ex)
            {

                litMessage.Text = StringTools.GenerateError(ex.Message);
            }

        }

    }
}