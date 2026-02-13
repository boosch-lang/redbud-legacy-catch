using ICSharpCode.SharpZipLib.Zip;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using Redbud.BL.DL;
using Redbud.BL.Utils;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Maddux.Catch.purchaseorder
{
    public partial class purchaseorderdetail : Page
    {
        protected int PurchaseOrderID
        {
            get
            {
                if (ViewState["PurchaseOrderID"] == null)
                {
                    ViewState["PurchaseOrderID"] = Request.QueryString["id"] == null || Request.QueryString["id"] == "" ? 0 : (object)Request.QueryString["id"];
                }
                return Convert.ToInt32(ViewState["PurchaseOrderID"].ToString());
            }

            set
            {
                ViewState["PurchaseOrderID"] = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            try
            {
                tabDetails.Visible = true;
                if (!Page.IsPostBack)
                {
                    Literal litPageHeader = (Literal)Master.FindControl("litPageHeader");

                    successAlert.Visible = false;
                    LoadDropDowns();

                    if (PurchaseOrderID == 0)
                    {
                        btnDeletePurchaseOrder.Visible = false;
                        pnlOrders.Visible = false;
                        this.Title = "Maddux | New Purchase Order";
                        litPageHeader.Text = "New Purchase Order";
                    }
                    else
                    {
                        btnDeletePurchaseOrder.Visible = true;
                        pnlOrders.Visible = true;
                        this.Title = "Maddux | Purchase Order";
                        litPageHeader.Text = "Purchase Order";
                    }
                    LoadPurchaseOrder();

                }

            }
            catch
            {
                Response.Redirect("/purchaseorder/purchaseorders.aspx");
            }
        }

        private void LoadPurchaseOrder()
        {
            if (PurchaseOrderID > 0)
            {
                using (var db = new MadduxEntities())
                {

                    PurchaseOrder purchaseOrder = db.PurchaseOrders.Find(PurchaseOrderID);

                    if (purchaseOrder != null)
                    {
                        //details tab
                        txtName.Text = purchaseOrder.Name;
                        ddlDeliveryHub.SelectedValue = purchaseOrder.DeliveryHubID.ToString();
                        txtPickupDate.Text = purchaseOrder.PickupDate.HasValue ? purchaseOrder.PickupDate.Value.ToString("MMMM dd, yyyy") : string.Empty;
                        ddlShipDate.SelectedValue = purchaseOrder.ShipDate.HasValue ? purchaseOrder.ShipDate.Value.ToString() : DateTime.MinValue.ToString();
                        totalFeet.InnerText = purchaseOrder.TotalFeet.ToString();
                        fullCount.InnerText = purchaseOrder.FullRackCount.ToString();
                        halfCount.InnerText = purchaseOrder.HalfRackCount.ToString();
                        quarterCount.InnerText = purchaseOrder.QuarterRackCount.ToString();
                        txtBolShipperAddress.Text = purchaseOrder.DeliveryHub.Address;
                        txtBolShipperCity.Text = purchaseOrder.DeliveryHub.City;
                        txtBolShipperName.Text = purchaseOrder.DeliveryHub.ShippingName;
                        txtBolShipperPostal.Text = purchaseOrder.DeliveryHub.Zip;
                        txtBolShipperCountry.Text = "Canada";
                        ddlProvince.SelectedValue = purchaseOrder.DeliveryHub.State;
                        dgvOrders.DataSource = purchaseOrder.Orders;
                        dgvOrders.DataBind();
                    }
                    else
                    {
                        throw new Exception("Purchase Order not found");
                    }
                }
            }
            else
            {
                totalFeet.InnerText = "0";
                halfCount.InnerText = "0";
                quarterCount.InnerText = "0";
            }
        }

        private void LoadDropDowns()
        {

            using (var db = new MadduxEntities())
            {
                var hubs = db.DeliveryHubs.ToList();
                hubs.Insert(0, new DeliveryHub
                {
                    HubID = -1,
                    Name = "-- Select a Delivery Hub --",

                });

                ddlDeliveryHub.DataTextField = "Name";
                ddlDeliveryHub.DataValueField = "HubID";
                ddlDeliveryHub.DataSource = hubs;
                ddlDeliveryHub.DataBind();

                ddlProvince.DataSource = db.States.Where(x => x.Country.ToLower() == "canada").Select(r => new ListItem
                {
                    Text = r.StateName,
                    Value = r.StateID
                }).ToList();
                ddlProvince.DataBind();

                var shipDates = db.ProductCatalogShipDates
                  .AsEnumerable()
                  .Select(r => new
                  {
                      Text = r.ShipDate.ToString("dd-MMM-yyyy"),
                      r.ShipDate,
                  });

                if (PurchaseOrderID == 0)
                {
                    shipDates = shipDates.Where(r => r.ShipDate >= DateTime.Today);
                }

                shipDates = shipDates
                  .Distinct()
                  .OrderBy(x => x.ShipDate);

                ddlShipDate.DataTextField = "Text";
                ddlShipDate.DataValueField = "ShipDate";
                ddlShipDate.DataSource = shipDates.ToList();
                ddlShipDate.DataBind();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    if (ddlShipDate.SelectedIndex == 0)
                    {
                        litMessage.Text = StringTools.GenerateError("Please select the ship date.");
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(txtPickupDate.Text))
                    {
                        litMessage.Text = StringTools.GenerateError("Please select the pick up date.");
                        return;
                    }
                    Redbud.BL.DL.PurchaseOrder purchaseOrder;

                    using (var db = new MadduxEntities())
                    {
                        if (PurchaseOrderID == 0)
                        {
                            purchaseOrder = new Redbud.BL.DL.PurchaseOrder();
                            db.PurchaseOrders.Add(purchaseOrder);
                        }
                        else
                        {
                            purchaseOrder = db.PurchaseOrders.Find(PurchaseOrderID);

                        }


                        if (purchaseOrder != null)
                        {
                            purchaseOrder.Name = txtName.Text.Trim();
                            purchaseOrder.DeliveryHubID = Convert.ToInt32(ddlDeliveryHub.SelectedValue);
                            purchaseOrder.ShipDate = DateTime.Parse(ddlShipDate.SelectedValue);
                            purchaseOrder.PickupDate = DateTime.Parse(txtPickupDate.Text);
                        }

                        db.SaveChanges();
                    }

                    litMessage.Text = StringTools.GenerateSuccess("Purchase order saved successfully");
                    Response.Redirect($@"/purchaseorder/purchaseorderdetail.aspx?id={purchaseOrder.PurchaseOrderID}");
                }
                catch
                {

                    throw;
                }

            }

        }

        protected void btnDeletePurchaseOrder_Click(object sender, EventArgs e)
        {
            using (var db = new MadduxEntities())
            {
                var purchaseOrder = db.PurchaseOrders.Find(PurchaseOrderID);
                try
                {
                    if (purchaseOrder != null)
                    {
                        purchaseOrder.Orders.Clear();
                        db.PurchaseOrders.Remove(purchaseOrder);
                        db.SaveChanges();
                    }
                }
                catch
                {

                }
                finally
                {
                    Response.Redirect("/purchaseorder/purchaseorders.aspx");
                }
            }

        }

        public string GetCount(object quantity, object rackSize, string rackType)
        {
            if (rackSize.ToString() == rackType)
            {
                return quantity.ToString();
            }

            return string.Empty;
        }

        protected void btnRemoveSelectedOrders_Click(object sender, EventArgs e)
        {
            try
            {
                using (var db = new MadduxEntities())
                {
                    var purchaseOrder = db.PurchaseOrders.FirstOrDefault(o => o.PurchaseOrderID == PurchaseOrderID);
                    foreach (GridViewRow row in dgvOrders.Rows)
                    {
                        var selectedCell = row.Cells[0];
                        var checkbox = (CheckBox)selectedCell.FindControl("OrderSelector");

                        if (checkbox.Checked)
                        {
                            //todo: This line has a lot of potential for problems.
                            var id = int.Parse(((HiddenField)selectedCell.FindControl("OrderID")).Value);
                            var order = purchaseOrder.Orders.Where(o => o.OrderID == id).FirstOrDefault();

                            if (order != null)
                            {
                                purchaseOrder.Orders.Remove(order);
                            }
                        }
                    }
                    db.SaveChanges();
                }

                Response.Redirect($@"/purchaseorder/purchaseorderdetail.aspx?id={PurchaseOrderID}", true);
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }


        }

        protected void btnPurchaseOrder_Click(object sender, EventArgs e)
        {
            try
            {
                const string sheetName = "Order Form";
                const int orderInfoRowIndex = 15;
                const int shippingAddressIndex = 11;

                int productRowIndex = 20;

                string filePath = Server.MapPath($@"~/App_Data/po/POTemplate.xlsx");

                using (MadduxEntities db = new MadduxEntities())
                {
                    var purchaseOrder = db.PurchaseOrders.FirstOrDefault(o => o.PurchaseOrderID == PurchaseOrderID);

                    if (purchaseOrder == null)
                    {
                        litMessage.Text = StringTools.GenerateError("There was an error retrieving purchase order.");
                        return;
                    }
                    using (MemoryStream ms = new MemoryStream())
                    {
                        IWorkbook workbook;

                        using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
                        {
                            workbook = new XSSFWorkbook(fileStream);
                            //workbook = NPOI.SS.UserModel.WorkbookFactory.Create(fileStream);
                        }

                        ISheet sheet = workbook.GetSheetAt(0) ?? workbook.CreateSheet(sheetName);

                        //shipping address name
                        IRow shippingNameRow = sheet.GetRow(shippingAddressIndex - 1);
                        IRow shippingAddressRow = sheet.GetRow(shippingAddressIndex);
                        IRow shippingCityRow = sheet.GetRow(shippingAddressIndex + 1);

                        shippingNameRow.GetCell(0).SetCellValue(purchaseOrder.DeliveryHub.ShippingName.ToUpper());
                        shippingAddressRow.GetCell(0).SetCellValue(purchaseOrder.DeliveryHub.Address);
                        shippingCityRow.GetCell(0).SetCellValue(string.Format("{0}, {1} {2}", purchaseOrder.DeliveryHub.City.ToUpper(), purchaseOrder.DeliveryHub.State.ToUpper(), purchaseOrder.DeliveryHub.Zip.ToUpper()));

                        int currentOrderIndex = 7;

                        IRow provinceRow = sheet.CreateRow(orderInfoRowIndex - 1);
                        IRow rackSizeRow = sheet.CreateRow(orderInfoRowIndex);
                        IRow custInfoNameRow = sheet.CreateRow(orderInfoRowIndex + 1);
                        IRow poNumberRow = sheet.CreateRow(orderInfoRowIndex + 2);

                        ICell rackSizeTextCell = rackSizeRow.CreateCell(currentOrderIndex - 1);
                        rackSizeTextCell.SetCellValue("Rack Size:");

                        ICell custTextCell = custInfoNameRow.CreateCell(currentOrderIndex - 1);
                        custTextCell.SetCellValue("Customer:");

                        ICell poNumberTextCell = poNumberRow.CreateCell(currentOrderIndex - 1);
                        poNumberTextCell.SetCellValue("PO# >>>");

                        Dictionary<int, int> orderColumnTracker = new Dictionary<int, int>();
                        List<int> orderProducts = new List<int>();

                        // Checking if need to update Purchase Order Date.
                        string updatePoSentDate = hdnUpdatePOSentDate.Value;

                        ICellStyle iCellStyle = workbook.CreateCellStyle();
                        iCellStyle.Alignment = HorizontalAlignment.Center;

                        string lastColumdID = "";
                        foreach (Order order in purchaseOrder.Orders)
                        {
                            ICell provinceCell = provinceRow.CreateCell(currentOrderIndex);
                            provinceCell.CellStyle = iCellStyle;
                            ICell rackSizeCell = rackSizeRow.CreateCell(currentOrderIndex, CellType.Numeric);
                            rackSizeCell.CellStyle = iCellStyle;
                            ICell customerCell = custInfoNameRow.CreateCell(currentOrderIndex);
                            customerCell.CellStyle = iCellStyle;
                            ICell poNumberCell = poNumberRow.CreateCell(currentOrderIndex);
                            poNumberCell.CellStyle = iCellStyle;

                            CellReference cellReference = new CellReference(provinceCell);
                            lastColumdID = cellReference.CellRefParts[2];
                            OrderRack orderRack = order.OrderRacks.FirstOrDefault();
                            double size = 1;

                            if (orderRack != null)
                            {
                                ProductCatalogRack rack = orderRack.ProductCatalogRack;

                                if (rack != null)
                                {
                                    orderProducts.AddRange(order.OrderItems.Select(x => x.ProductId).ToList());

                                    if (string.IsNullOrWhiteSpace(rack.RackSize))
                                    {
                                        size = 1;
                                    }
                                    else if (string.Equals("1/4", rack.RackSize))
                                    {
                                        size = 0.5;
                                    }
                                    else if (string.Equals("1/2", rack.RackSize))
                                    {
                                        size = 1;
                                    }
                                }
                            }
                            else
                            {
                                orderProducts.AddRange(order.OrderItems.Select(x => x.ProductId).ToList());

                                if (order.OrderItems.Count == 8)
                                {
                                    size = 0.5;
                                }
                                else if (order.OrderItems.Count >= 16)
                                {
                                    size = 1;
                                }
                            }
                            rackSizeCell.SetCellValue(size);
                            customerCell.SetCellValue(order.Customer.Company);
                            poNumberCell.SetCellValue(order.OrderID.ToString());

                            try
                            {
                                provinceCell.SetCellValue(string.IsNullOrWhiteSpace(order.Customer.State) ? "" : order.Customer.State);
                            }
                            catch
                            {
                                provinceCell.SetCellValue("");
                            }

                            orderColumnTracker.Add(order.OrderID, currentOrderIndex);

                            currentOrderIndex++;

                            //updating PO sent date if selected
                            if (!string.IsNullOrWhiteSpace(updatePoSentDate) && string.Equals("True", updatePoSentDate))
                            {
                                order.PurchaseOrdersSentDate = DateTime.Now;
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(updatePoSentDate) && string.Equals("True", updatePoSentDate))
                        {
                            db.SaveChanges();
                        }

                        List<int> productIDs = orderProducts.Distinct().OrderBy(x => x).ToList();
                        List<Product> products = db.Products.Where(x => productIDs.Contains(x.ProductId)).OrderBy(x => x.ProductName).ToList();

                        var groupedProducts = products.GroupBy(x => (x.ItemNumberInternal.Trim(), x.ProductName.Trim(), x.UnitCost));

                        foreach (var groupedProduct in groupedProducts)
                        {
                            var firstProduct = groupedProduct.First();

                            IRow productRow = sheet.CreateRow(productRowIndex);

                            ICell packagePerUnitCell = productRow.CreateCell(2, CellType.Numeric);
                            packagePerUnitCell.SetCellValue(firstProduct.PackagesPerUnit);

                            ICell eachPriceCell = productRow.CreateCell(3, CellType.Numeric);
                            eachPriceCell.SetCellValue(firstProduct.UnitCost);

                            ICell itemNumberCell = productRow.CreateCell(5, CellType.Numeric);
                            itemNumberCell.SetCellValue(firstProduct.ItemNumberInternal);

                            ICell descriptionCell = productRow.CreateCell(6, CellType.String);
                            descriptionCell.SetCellValue(firstProduct.ProductName);

                            foreach (Product product in groupedProduct)
                            {
                                double totalItems = 0;

                                foreach (KeyValuePair<int, int> orderColumn in orderColumnTracker)
                                {
                                    Order order = purchaseOrder.Orders.FirstOrDefault(x => x.OrderID == orderColumn.Key);

                                    if (order != null)
                                    {
                                        OrderItem orderItem = order.OrderItems.FirstOrDefault(x => x.ProductId == product.ProductId);

                                        if (orderItem != null)
                                        {
                                            totalItems += orderItem.Quantity;
                                            ICell qtyCell = productRow.CreateCell(orderColumn.Value, CellType.Numeric);

                                            qtyCell.SetCellValue(orderItem.Quantity);
                                            qtyCell.CellStyle = iCellStyle;


                                        }
                                    }
                                }

                                ICell qtyFlatCell = productRow.CreateCell(1, CellType.Formula);
                                qtyFlatCell.SetCellFormula($"SUM(H{productRowIndex + 1}:{lastColumdID}{productRowIndex + 1})");
                                qtyFlatCell.SetCellValue(totalItems);

                                ICell totalQtyCell = productRow.CreateCell(0, CellType.Formula);
                                totalQtyCell.SetCellFormula($"SUM(H{productRowIndex + 1}:{lastColumdID}{productRowIndex + 1})*C{productRowIndex + 1}");

                                ICell totalPriceCell = productRow.CreateCell(4, CellType.Formula);
                                totalPriceCell.SetCellFormula($"PRODUCT(A{productRowIndex + 1},D{productRowIndex + 1})");

                            }
                            productRowIndex++;
                        }

                        IRow totalsRow = sheet.CreateRow(productRowIndex + 1);

                        ICell totalPriceOfPoDescCell = totalsRow.CreateCell(3);
                        totalPriceOfPoDescCell.SetCellValue("TOTAL>>");

                        ICell totalPriceOfPoCell = totalsRow.CreateCell(4, CellType.Formula);
                        totalPriceOfPoCell.SetCellFormula($"SUM(E20:E{productRowIndex})");

                        foreach (KeyValuePair<int, int> orderColumn in orderColumnTracker)
                        {
                            ICell orderTotalQtyCell = totalsRow.CreateCell(orderColumn.Value, CellType.Formula);
                            CellReference cellReference = new CellReference(orderTotalQtyCell);

                            orderTotalQtyCell.SetCellFormula($"SUM({cellReference.CellRefParts[2]}20:{cellReference.CellRefParts[2]}{productRowIndex})");
                        }

                        //sheet.
                        ICell rowCountTextCell = provinceRow.CreateCell(5);
                        rowCountTextCell.SetCellValue("Rack Count: ");

                        ICell rowCountCell = provinceRow.CreateCell(6, CellType.Formula);
                        rowCountCell.SetCellFormula($"SUM(H16:{lastColumdID}16)");


                        XSSFFormulaEvaluator formula = new XSSFFormulaEvaluator(workbook);
                        formula.EvaluateAll();

                        workbook.Write(ms);

                        byte[] data = ms.ToArray();

                        Response.Clear();
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("Content-Disposition", $"attachment; filename=PurchaseOrder-{DateTime.Now:dd-MMM-yy-hh-mm}.xlsx");
                        Response.OutputStream.Write(data, 0, data.Length);
                        Response.End();
                    }
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }

        protected void btnPrintBOLs_Click(object sender, EventArgs e)
        {
            try
            {
                string shipperInfo = string.Empty;
                shipperInfo += txtBolShipperAccountNo.Text + "#";
                shipperInfo += txtBolShipperPhone.Text + "#";
                shipperInfo += txtBolShipperName.Text + "#";
                shipperInfo += txtBolShipperAddress.Text + "#";
                shipperInfo += txtBolShipperCity.Text + "#";
                shipperInfo += ddlProvince.SelectedValue + "#";
                shipperInfo += txtBolShipperPostal.Text + "#";
                shipperInfo += txtBolShipperCountry.Text + "#";

                shipperInfo = HttpUtility.HtmlEncode(shipperInfo);
                PurchaseOrder purchaseOrder;

                using (MadduxEntities db = new MadduxEntities())
                {
                    purchaseOrder = db.PurchaseOrders.FirstOrDefault(o => o.PurchaseOrderID == PurchaseOrderID);
                    if (purchaseOrder == null)
                    {
                        litMessage.Text = StringTools.GenerateError("There was an error retrieving purchase order.");
                        return;
                    }

                    Response.Clear();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + "Bills-of-lading-" + PurchaseOrderID.ToString() + ".zip");
                    Response.ContentType = "application/zip";
                    using (var zipStream = new ZipOutputStream(Response.OutputStream))
                    {
                        foreach (int _id in purchaseOrder.Orders.Select(o => o.OrderID).ToList())
                        {
                            using (var writer = new StringWriter())
                            {
                                Server.Execute($"/order/bill-of-lading.aspx?id={_id}&shipperInfo={shipperInfo}&view=print", writer);
                                string html = writer.GetStringBuilder().ToString();
                                var stream = new MemoryStream();

                                HtmlToPdf htmlToPdf = new HtmlToPdf()
                                {
                                    Html = html,
                                    Orientation = PdfOrientation.Portrait
                                };
                                RestSharp.RestClient client = new RestClient("https://html2pdf.webilitynetwork.ca/api/Page/GetByHtml")
                                {
                                    Timeout = -1
                                };
                                RestRequest request = new RestRequest(Method.POST);
                                request.AddHeader("Content-Type", "application/json");

                                request.AddParameter(
                                    "application/json",
                                    JsonConvert.SerializeObject(htmlToPdf),
                                    ParameterType.RequestBody);

                                IRestResponse pdfResponse = client.Execute(request);

                                var fileEntry = new ZipEntry(string.Format("order_{0}.pdf", _id))
                                {
                                    Size = pdfResponse.RawBytes.Length
                                };
                                zipStream.PutNextEntry(fileEntry);
                                zipStream.Write(pdfResponse.RawBytes, 0, pdfResponse.RawBytes.Length);
                            }
                        }
                        zipStream.Flush();
                        zipStream.Close();
                    }
                }
                Response.End();
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }

        protected void ddlDeliveryHub_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedHubId = Convert.ToInt32(ddlDeliveryHub.SelectedValue);


            if (selectedHubId > 0)
            {
                using (MadduxEntities db = new MadduxEntities())
                {
                    var hub = db.DeliveryHubs.FirstOrDefault(h => h.HubID == selectedHubId);
                    if (hub != null)
                    {
                        txtBolShipperAddress.Text = hub.Address;
                        txtBolShipperName.Text = hub.ShippingName;
                        txtBolShipperCity.Text = hub.City;
                        txtBolShipperCountry.Text = "Canada";
                        ddlProvince.SelectedValue = hub.State;
                        txtBolShipperPostal.Text = hub.Zip;
                    }
                }
            }
            else
            {
                txtBolShipperAddress.Text = string.Empty;
                txtBolShipperName.Text = string.Empty;
                txtBolShipperCity.Text = string.Empty;
                txtBolShipperCountry.Text = "Canada";
                ddlProvince.SelectedIndex = 0;
                txtBolShipperPostal.Text = string.Empty;
            }
        }

        protected void btnPickSheets_Click(object sender, EventArgs e)
        {
            try
            {
                PurchaseOrder purchaseOrder;
                using (MadduxEntities db = new MadduxEntities())
                {
                    purchaseOrder = db.PurchaseOrders.FirstOrDefault(o => o.PurchaseOrderID == PurchaseOrderID);
                    if (purchaseOrder == null)
                    {
                        litMessage.Text = StringTools.GenerateError("There was an error retrieving purchase order.");
                        return;
                    }

                    Response.Clear();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + "picksheets_" + PurchaseOrderID.ToString() + ".zip");
                    Response.ContentType = "application/zip";
                    using (var zipStream = new ZipOutputStream(Response.OutputStream))
                    {
                        foreach (int _id in purchaseOrder.Orders.Select(o => o.OrderID).ToList())
                        {
                            using (var writer = new StringWriter())
                            {
                                Server.Execute($"/order/picksheet.aspx?id={_id}&view=print", writer);
                                string html = writer.GetStringBuilder().ToString();

                                HtmlToPdf htmlToPdf = new HtmlToPdf()
                                {
                                    Html = html,
                                    Orientation = PdfOrientation.Portrait
                                };

                                RestSharp.RestClient client = new RestClient("https://html2pdf.webilitynetwork.ca/api/Page/GetByHtml")
                                {
                                    Timeout = -1
                                };

                                RestRequest request = new RestRequest(Method.POST);
                                request.AddHeader("Content-Type", "application/json");

                                request.AddParameter(
                                    "application/json",
                                    JsonConvert.SerializeObject(htmlToPdf),
                                    ParameterType.RequestBody);

                                IRestResponse pdfResponse = client.Execute(request);

                                var fileEntry = new ZipEntry(string.Format("order_{0}.pdf", _id))
                                {
                                    Size = pdfResponse.RawBytes.Length
                                };
                                zipStream.PutNextEntry(fileEntry);
                                zipStream.Write(pdfResponse.RawBytes, 0, pdfResponse.RawBytes.Length);
                            }
                        }
                        zipStream.Flush();
                        zipStream.Close();
                    }
                }
                Response.End();
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }
    }
}