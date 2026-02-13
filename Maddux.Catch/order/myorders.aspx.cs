using Maddux.Catch.LocalClasses;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using Redbud.BL;
using Redbud.BL.DL;
using Redbud.BL.Utils;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Maddux.Catch.order
{
    public partial class myorders : Page
    {
        public class RackDetails
        {
            public int? RackID { get; set; }
            public string RackName { get; set; }
        }

        private int OrderType
        {
            get
            {
                if (ViewState["type"] == null)
                {
                    ViewState["type"] = Request.QueryString["type"] != null && Redbud.BL.Utils.FCSAppUtils.IsInteger(Request.QueryString["type"])
                        ? Request.QueryString["type"]
                        : (object)Convert.ToInt32(OrderStatus.Orders);
                }

                return (Convert.ToInt32(ViewState["type"]));
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string pageType;

            try
            {
                Response.Cache.SetCacheability(HttpCacheability.NoCache);

                if (!Page.IsPostBack)
                {
                    LoadFilterDropDowns();
                    LoadOrders();
                    switch (OrderType)
                    {
                        case (int)OrderStatus.DraftOrders:
                            pageType = "Draft Orders";
                            dgvOrders.EmptyDataText = "There are no draft orders to display.";
                            foreach (DataControlField col in dgvOrders.Columns)
                            {
                                if (col.HeaderText == "Order Date")
                                {
                                    col.Visible = false;
                                }

                                if (col.HeaderText == "Conf. Sent Date")
                                {
                                    col.Visible = false;
                                }

                                if (col.HeaderText == "P.O. Sent Date")
                                {
                                    col.Visible = false;
                                }
                            }
                            btnDelete.Visible = true;
                            break;
                        case (int)OrderStatus.Quotes:
                            pageType = "Quotes";
                            dgvOrders.EmptyDataText = "There are no quotes to display.";
                            break;
                        default:
                            pageType = "Orders";
                            dgvOrders.EmptyDataText = "There are no orders to display.";
                            break;
                    }
                    this.Title = "Maddux | " + pageType;
                    SetButtonVisiblity();
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }

        /// <summary>
        /// Populates filters drop downs
        /// </summary>
        private void LoadFilterDropDowns()
        {
            try
            {
                User currentUser = AppSession.Current.CurrentUser;

                Lookup.LoadProvinceDropDown(ref ddlFilterProvince, currentUser.CanOnlyViewAssignedProvinces, true, false, currentUser.UserID);

                //Lookup.LoadCustomerDropDown(ref ddlFilterCustomer);
                //Lookup.LoadShipDatesDropDown(ref ddlFilterShipDate);

                using (MadduxEntities madduxEntities = new MadduxEntities())
                {
                    ddlProvince.DataSource = madduxEntities.States.Where(x => x.Country.ToLower() == "canada").Select(r => new ListItem
                    {
                        Text = r.StateName,
                        Value = r.StateID
                    }).ToList();
                    ddlProvince.DataBind();

                    var catalogs = madduxEntities.ProductCatalogs
                        .Where(pc => pc.Active)
                        .Select(pc => new
                        {
                            CatalogID = pc.CatalogId,
                            pc.CatalogYear,
                            pc.CatalogName
                        })
                        .OrderByDescending(pc => pc.CatalogYear)
                        .ThenBy(x => x.CatalogName)
                        .ToList();

                    ddlFilterCatalog.DataValueField = "CatalogID";
                    ddlFilterCatalog.DataTextField = "CatalogName";
                    ddlFilterCatalog.DataSource = catalogs;
                    ddlFilterCatalog.DataBind();
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }

        private void LoadOrders(int page = 1)
        {
            using (MadduxEntities db = new MadduxEntities())
            {
                User currentUser = AppSession.Current.CurrentUser;
                IQueryable<vwMyOrder> orders = !currentUser.ShowOtherMyOrders
                    ? db.vwMyOrders.Where(r => r.SalesPersonID == currentUser.UserID)
                    : db.vwMyOrders;
                if (orders != null)
                {
                    string query = $@"&type={OrderType}";

                    if (!string.IsNullOrWhiteSpace(Request.QueryString["province"]))
                    {
                        ddlFilterProvince.SelectedValue = Request.QueryString["province"];
                        string province = Request.QueryString["province"];
                        query += string.Format("&province={0}", Server.UrlEncode(Request.QueryString["province"]));
                        orders = orders.Where(r => r.OrderStatus == OrderType && r.State == province);
                    }
                    //filter by catalog
                    if (Request.QueryString["catalogId"] != null)
                    {
                        //array of catalogs to filter by
                        string[] catalogsToFilterBy = Request.QueryString["catalogId"].Split(',');

                        //to store the catalogs select by the user
                        List<int> catalogIds = new List<int>();

                        foreach (string catalogId in catalogsToFilterBy)
                        {
                            ddlFilterCatalog.Items.FindByValue(catalogId).Selected = true;
                            //add the catalog IDs to the list
                            catalogIds.Add(Convert.ToInt32(catalogId));
                            //build the url
                            query += string.Format("&catalogId={0}", catalogId);
                        }

                        var catalogs = db.vwOrderCatalogs.Where(r => catalogIds.Contains(r.CatalogId)).Select(r => r.OrderId);
                        orders = orders.Where(r => r.OrderStatus == OrderType && catalogs.Contains(r.OrderID));
                    }

                    if (!string.IsNullOrWhiteSpace(Request.QueryString["customer"]) && Convert.ToInt32(Request.QueryString["customer"]) > 0)
                    {
                        int customerId = Convert.ToInt32(Request.QueryString["customer"]);
                        orders = orders.Where(r => r.CustomerID == customerId);
                    }
                    else if (!string.IsNullOrWhiteSpace(Request.QueryString["customer"]) && Convert.ToInt32(Request.QueryString["customer"]) == 0)
                    {
                        orders = orders.Where(r => r.OrderStatus == OrderType);
                    }

                    if (!string.IsNullOrWhiteSpace(Request.QueryString["reqShipDate"]) && Request.QueryString["reqShipDate"] == "0001-01-01")
                    {
                        ddlFilterShipDate.SelectedValue = Request.QueryString["reqShipDate"];
                        orders = orders.Where(r => r.OrderStatus == OrderType);
                    }
                    else if (!string.IsNullOrWhiteSpace(Request.QueryString["reqShipDate"]) && Request.QueryString["reqShipDate"] != "0001-01-01")
                    {

                        DateTime shipDate = DateTime.Parse(Request.QueryString["reqShipDate"]);
                        ddlFilterShipDate.SelectedValue = shipDate.ToString();

                        orders = orders.Where(r => r.OrderStatus == OrderType && r.RequestedShipDate == shipDate);
                    }

                    if (Request.QueryString["approved"] != null)
                    {
                        ddlFilterApproved.SelectedValue = Request.QueryString["approved"];
                        bool approved = Convert.ToBoolean(int.Parse(Request.QueryString["approved"]));
                        orders = orders.Where(x => x.Approved == approved);
                    }
                    // Query the results and store them locally so we don't need to make multiple requests to the DB. 
                    var orderResult = orders.ToList();
                    orderResult = orderResult.Where(x => x.OrderStatus == OrderType).ToList();

                    var orderShipDates = orderResult.OrderBy(x => x.RequestedShipDate).Select(x => x.RequestedShipDate.Value).ToList().Distinct()
                       .Select(r => new
                       {
                           Text = r.ToString("dd-MMM-yyyy"),
                           ShipDate = r,
                       }).Distinct().ToList();

                    orderShipDates.Insert(0, new
                    {
                        Text = "-- All Req. Ship Dates --",
                        ShipDate = DateTime.MinValue
                    });

                    ddlFilterShipDate.DataValueField = "ShipDate";
                    ddlFilterShipDate.DataTextField = "Text";
                    ddlFilterShipDate.DataSource = orderShipDates;
                    ddlFilterShipDate.DataBind();


                    // Potentially add CatalogName to the view to remove the second select in this:
                    dgvOrders.DataSource =
                    orderResult
                       .Select(
                            x => new
                            {
                                x.OrderID,
                                CustomerId = x.CustomerID,
                                x.OrderDate,
                                x.RequestedShipDate,
                                x.State,
                                x.SalesPerson,
                                x.ConfirmationSentDate,
                                x.PurchaseOrdersSentDate,
                                x.OrderTotal,
                                x.Company,
                                x.StarRating,
                                x.CatalogId,
                                x.CatalogName,
                                x.RackID,
                                x.RackName,
                                x.BulkRackID,
                                x.BulkRackName,
                                x.BulkRackKey,
                                x.Approved
                            }).OrderByDescending(o => o.OrderID)
                       .ToList();

                    dgvOrders.DataBind();

                    Literal litPageHeader = (Literal)Master.FindControl("litPageHeader");
                    Literal litTotal = (Literal)Master.FindControl("litTotal");
                    litTotal.Text = dgvOrders.Rows.Count.ToString();

                    switch (OrderType)
                    {
                        case (int)OrderStatus.Quotes:
                            litPageHeader.Text = "Quotes";
                            litTotal.Text += " Quotes";
                            break;
                        case (int)OrderStatus.DraftOrders:
                            litPageHeader.Text = "Draft Orders";
                            litTotal.Text += " Draft Orders";
                            break;
                        default:
                            litPageHeader.Text = "Orders";
                            litTotal.Text += " Orders";
                            break;
                    }

                    var orderTotal = orderResult.Sum(r => Convert.ToInt32(r.OrderTotal));
                    litTotal.Text += " (Total value: " + orderTotal.ToString("C2") + ")";
                }
            }
        }

        /// <summary>
        /// Builds url parameters to filter orders
        /// </summary>
        private void FilterQuery()
        {
            string query = $@"?page=1&type={OrderType}";

            var province = ddlFilterProvince.SelectedValue.ToString();

            var approved = ddlFilterApproved.SelectedValue.ToString();

            List<int> selecteds = ddlFilterCatalog.GetSelectedIndices().ToList();

            if (province != "00")
            {
                query += $@"&province={province}";
            }

            if (!string.IsNullOrEmpty(approved))
            {
                query += $@"&approved={approved}";
            }

            for (int i = 0; i < selecteds.Count; i++)
            {
                ListItem l = ddlFilterCatalog.Items[selecteds[i]];

                if (int.Parse(l.Value) != -1)
                {
                    query += $@"&catalogId={int.Parse(l.Value)}";
                }
            }

            if (DateTime.TryParse(ddlFilterShipDate.SelectedValue, out DateTime shipDate))
            {
                if (shipDate != DateTime.MinValue)
                    query += $@"&reqShipDate={shipDate.ToString("yyyy-MM-dd")}";
            }

            Response.Redirect($@"myorders.aspx{query}");
        }

        public string GenerateStarRatingGraphic(int? starRating)
        {

            string stars = string.Empty;
            if (starRating.HasValue)
            {
                for (int x = 0; x < starRating.Value; x++)
                {
                    stars += "<i class='fa fa-star'></i> ";
                }
                for (int x = 0; x < (5 - starRating.Value); x++)
                {
                    stars += "<i class='fa fa-star-o'></i> ";
                }
            }
            else
            {
                stars += "N/A";
            }
            return stars.Trim();
        }

        private void SetButtonVisiblity()
        {

            var currentUser = AppSession.Current.CurrentUser;

            //if (!currentUser.CanEmailPrintExportOrders)
            //{
            //    btnPickSheets.Visible = false;
            //    btnPurchaseOrder.Visible = false;
            //    btnPrintConfirmation.Visible = false;
            //}
        }

        protected void btnPrintConfirmation_Click(object sender, EventArgs e)
        {
            try
            {
                List<int> IDs = getSelectedOrders();
                string pages = string.Empty;

                if (IDs.Count == 0)
                {
                    litMessage.Text = StringTools.GenerateError("Please select any order to print confirmation");
                    return;
                }
                foreach (int _id in IDs)
                {
                    using (var writer = new StringWriter())
                    {
                        Server.Execute($"/order/confirmation.aspx?id={_id}&view=print", writer);
                        string html = writer.GetStringBuilder().ToString();
                        pages += html;
                    }

                }


                HtmlToPdf htmlToPdf = new HtmlToPdf()
                {
                    Html = pages,
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

                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", "attachment; filename=orderconfirmation.pdf");
                Response.OutputStream.Write(pdfResponse.RawBytes, 0, pdfResponse.RawBytes.Length);
                Response.End();
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }

        }

        protected void btnPickSheets_Click(object sender, EventArgs e)
        {
            try
            {
                List<int> IDs = getSelectedOrders();
                string pages = string.Empty;

                if (IDs.Count == 0)
                {
                    litMessage.Text = StringTools.GenerateError("Please select any order to print confirmation");
                    return;
                }
                foreach (int _id in IDs)
                {
                    using (var writer = new StringWriter())
                    {
                        Server.Execute($"/order/picksheet.aspx?id={_id}&view=print", writer);
                        string html = writer.GetStringBuilder().ToString();
                        pages += html;
                    }

                }


                HtmlToPdf htmlToPdf = new HtmlToPdf()
                {
                    Html = pages,
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

                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", $"attachment; filename=picksheets-{DateTime.Now:yyyy-MM-dd-hh-mm-tt}.pdf");
                Response.BinaryWrite(pdfResponse.RawBytes);
                Response.Flush();
                Response.End();
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }

        private List<int> getSelectedOrders()
        {
            List<int> IDs = new List<int>();

            string pages = string.Empty;
            foreach (GridViewRow row in dgvOrders.Rows)
            {
                CheckBox checkBox = row.FindControl("OrderSelector") as CheckBox;
                if (checkBox.Checked)
                {
                    HiddenField hdnOrderID = row.FindControl("OrderID") as HiddenField;
                    int OrderID = int.Parse(hdnOrderID.Value);
                    IDs.Add(OrderID);
                }
            }
            return IDs;
        }

        protected void btnPurchaseOrder_Click(object sender, EventArgs e)
        {
            try
            {
                const string sheetName = "Order Form";
                const int orderInfoRowIndex = 15;

                int productRowIndex = 20;

                string filePath = Server.MapPath($@"~/App_Data/po/POTemplate.xls");

                List<int> selectedOrderIds = getSelectedOrders();

                if (selectedOrderIds.Count == 0)
                {
                    litMessage.Text = StringTools.GenerateError("Please select any order to print confirmation");
                    return;
                }

                using (MadduxEntities madduxEntities = new MadduxEntities())
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        IWorkbook workbook;
                        List<Order> orders =
                            madduxEntities
                               .Orders
                               .Include(x => x.Customer)
                               .Include(x => x.OrderRacks)
                               .Where(x => selectedOrderIds.Contains(x.OrderID))
                               .ToList();

                        using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
                        {
                            workbook = new HSSFWorkbook(fileStream);
                        }

                        ISheet sheet = workbook.GetSheet(sheetName) ?? workbook.CreateSheet(sheetName);

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

                        foreach (Order order in orders)
                        {
                            ICell provinceCell = provinceRow.CreateCell(currentOrderIndex);
                            ICell rackSizeCell = rackSizeRow.CreateCell(currentOrderIndex);
                            ICell customerCell = custInfoNameRow.CreateCell(currentOrderIndex);
                            ICell poNumberCell = poNumberRow.CreateCell(currentOrderIndex);

                            OrderRack orderRack = order.OrderRacks.FirstOrDefault();
                            double size = 1;

                            if (orderRack != null)
                            {
                                ProductCatalogRack rack = madduxEntities.ProductCatalogRacks.FirstOrDefault(x => x.RackID == orderRack.ProductCatalogRack.RackID);

                                if (rack != null)
                                {
                                    orderProducts.AddRange(rack.RackProducts.Select(x => x.ProductID).ToList());

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
                            madduxEntities.SaveChanges();
                        }

                        List<int> productIDs = orderProducts.Distinct().OrderBy(x => x).ToList();
                        foreach (Product product in madduxEntities.Products.Where(x => productIDs.Contains(x.ProductId)).ToList())
                        {
                            IRow productRow = sheet.CreateRow(productRowIndex);

                            ICell packagePerUnitCell = productRow.CreateCell(2);
                            packagePerUnitCell.SetCellValue(product.PackagesPerUnit);

                            ICell eachPriceCell = productRow.CreateCell(3);
                            eachPriceCell.SetCellValue(product.UnitPrice / product.PackagesPerUnit);

                            ICell itemNumberCell = productRow.CreateCell(5);
                            itemNumberCell.SetCellValue(string.IsNullOrWhiteSpace(product.ItemNumberInternal) ? string.Empty : product.ItemNumberInternal);

                            ICell descriptionCell = productRow.CreateCell(6);
                            descriptionCell.SetCellValue(product.ProductName);

                            double totalItems = 0;

                            foreach (KeyValuePair<int, int> orderColumn in orderColumnTracker)
                            {
                                Order order = orders.FirstOrDefault(x => x.OrderID == orderColumn.Key);

                                if (order != null)
                                {
                                    if (order.OrderRacks.FirstOrDefault() == null)
                                    {
                                        OrderItem orderItem = order.OrderItems.FirstOrDefault(x => x.ProductId == product.ProductId);

                                        if (orderItem != null)
                                        {
                                            totalItems += orderItem.Quantity;
                                            ICell qtyCell = productRow.CreateCell(orderColumn.Value);
                                            qtyCell.SetCellValue(orderItem.Quantity);
                                        }
                                    }
                                    else
                                    {
                                        OrderRack orderRack = order.OrderRacks.FirstOrDefault();
                                        OrderItem orderItem = orderRack?.OrderItems.FirstOrDefault(x => x.ProductId == product.ProductId);

                                        if (orderItem != null)
                                        {
                                            totalItems += orderItem.Quantity;
                                            ICell qtyCell = productRow.CreateCell(orderColumn.Value);
                                            qtyCell.SetCellValue(orderItem.Quantity);
                                        }
                                    }
                                }
                            }

                            ICell qtyFlatCell = productRow.CreateCell(1);
                            qtyFlatCell.SetCellFormula($"SUM(H{productRowIndex + 1}:HF{productRowIndex + 1})");
                            qtyFlatCell.SetCellValue(totalItems);

                            ICell totalQtyCell = productRow.CreateCell(0);
                            totalQtyCell.SetCellFormula($"SUM(H{productRowIndex + 1}:HF{productRowIndex + 1})*C{productRowIndex + 1}");

                            ICell totalPriceCell = productRow.CreateCell(4);
                            totalPriceCell.SetCellFormula($"PRODUCT(A{productRowIndex + 1}*D{productRowIndex + 1})");

                            productRowIndex++;
                        }

                        IRow totalsRow = sheet.CreateRow(productRowIndex + 1);

                        ICell totalPriceOfPoDescCell = totalsRow.CreateCell(3);
                        totalPriceOfPoDescCell.SetCellValue("TOTAL>>");

                        ICell totalPriceOfPoCell = totalsRow.CreateCell(4);
                        totalPriceOfPoCell.SetCellFormula($"SUM(E20:E{productRowIndex})");

                        foreach (KeyValuePair<int, int> orderColumn in orderColumnTracker)
                        {
                            ICell orderTotalQtyCell = totalsRow.CreateCell(orderColumn.Value);
                            CellReference cellReference = new CellReference(orderTotalQtyCell);

                            orderTotalQtyCell.SetCellFormula($"SUM({cellReference.CellRefParts[2]}20:{cellReference.CellRefParts[2]}{productRowIndex})");
                        }

                        //sheet.
                        ICell rowCountTextCell = provinceRow.CreateCell(5);
                        rowCountTextCell.SetCellValue("Rack Count: ");

                        ICell rowCountCell = provinceRow.CreateCell(6);
                        rowCountCell.SetCellFormula("SUM(H16:II15)");

                        HSSFFormulaEvaluator formula = new HSSFFormulaEvaluator(workbook);

                        formula.EvaluateAll();

                        workbook.Write(ms);

                        byte[] data = ms.ToArray();

                        Response.Clear();
                        Response.ContentType = "application/vnd.xls";
                        Response.AddHeader("Content-Disposition", $"attachment; filename=PurchaseOrder-{DateTime.Now:dd-MMM-yy-hh-mm}.xls");
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

                List<int> IDs = getSelectedOrders();
                string pages = string.Empty;

                if (IDs.Count == 0)
                {
                    litMessage.Text = StringTools.GenerateError("Please select any order to print BOLs");
                    return;
                }
                foreach (int _id in IDs)
                {
                    using (var writer = new StringWriter())
                    {
                        Server.Execute($"/order/bill-of-lading.aspx?id={_id}&shipperInfo={shipperInfo}&view=print", writer);
                        string html = writer.GetStringBuilder().ToString();
                        pages += html;
                    }

                }


                HtmlToPdf htmlToPdf = new HtmlToPdf()
                {
                    Html = pages,
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

                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", "attachment; filename=Bill-of-ladings.pdf");
                Response.OutputStream.Write(pdfResponse.RawBytes, 0, pdfResponse.RawBytes.Length);
                Response.End();
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }

        protected void btnSearchOrder_Click(object sender, EventArgs e)
        {
            FilterQuery();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                List<int> IDs = getSelectedOrders();
                foreach (int id in IDs)
                {
                    using (var db = new MadduxEntities())
                    {
                        var order = db.Orders.FirstOrDefault(o => o.OrderID == id);
                        List<int> orderRackIds = new List<int>();

                        if (order.BulkRackID.HasValue)
                        {
                            //bulk rack order - get all associated orderRackIds
                            orderRackIds.AddRange(db.Orders.Where(x => x.BulkOrderKey == order.BulkOrderKey).Select(o => o.OrderID).ToList());

                        } else
                        {
                            orderRackIds.Add(order.OrderID);

                        }

                        var isShipped  = db.Shipments.Any(x => x.ShipmentItems.Any(si => orderRackIds.Contains(si.OrderItem.OrderId)));

                        if (isShipped)
                        {
                            throw new Exception("Can not delete order(s) because some of the items from the order already have been shipped.");
                        } else {
                            var racks = db.OrderRacks.Where(x => orderRackIds.Contains(x.OrderId)).ToList();
                            var orderItems = db.OrderItems.Where(x => orderRackIds.Contains(x.OrderId)).ToList();
                            var orders = db.Orders.Where(x => orderRackIds.Contains(x.OrderID)).ToList();

                            if (orderItems != null)
                                db.OrderItems.RemoveRange(orderItems);
                            if (racks != null)
                                db.OrderRacks.RemoveRange(racks);
                            db.Orders.RemoveRange(orders);
                            int changes = db.SaveChanges();
                            if (changes == 0)
                                throw new Exception("Error while removing order please try again");
                        }
                    }
                }
                LoadOrders();
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }

        }

        private List<Guid> OrderKeys = new List<Guid>();

        private bool CheckIfKeyIsUnique(Guid key)
        {
            if (OrderKeys.Contains(key))
            {
                return false;
            }

            OrderKeys.Add(key);
            return true;
        }

        protected void dgvOrders_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    var key = DataBinder.Eval(e.Row.DataItem, "BulkRackKey");
                    // Find the delete button in the current row
                    CheckBox deleteButton = (CheckBox)e.Row.FindControl("OrderSelector");// Adjust based on your data source
                    if (key != null)
                    {
                        deleteButton.Visible = CheckIfKeyIsUnique((Guid)key);
                    }
                    else
                    {
                        deleteButton.Visible = true;
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