using Maddux.Catch.LocalClasses;
using Newtonsoft.Json;
using Redbud.BL.DL;
using Redbud.BL.Helpers;
using Redbud.BL.Resources;
using Redbud.BL.Utils;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Maddux.Catch.shipping
{
    class EmailResponse
    {
        public bool Sent { get; set; }
        public byte[] data { get; set; }
    }
    public partial class test : System.Web.UI.Page
    {
        private User currentUser;

        private int ShipmentID
        {
            get
            {
                if (ViewState["ShipmentID"] == null)
                {
                    ViewState["ShipmentID"] = Request.QueryString["id"] == null || Request.QueryString["id"] == "" ? 0 : (object)Request.QueryString["id"];
                }
                return Convert.ToInt32(ViewState["ShipmentID"].ToString());
            }

            set
            {
                ViewState["ShipmentID"] = value;
            }
        }

        private int CustomerID
        {
            get
            {
                if (ViewState["CustomerId"] == null)
                {
                    if (Request.QueryString["CustomerId"] == null || Request.QueryString["CustomerId"] == "")
                    {
                        using (MadduxEntities db = new MadduxEntities())
                        {
                            var customerID = db.vwMyShipments.Where(s => s.ShipmentID == ShipmentID).Select(s => new { s.CustomerID }).FirstOrDefault();
                            ViewState["CustomerId"] = customerID != null ? customerID.CustomerID : (object)0;
                        }

                    }
                    else
                        ViewState["CustomerId"] = Request.QueryString["CustomerId"];
                }
                return Convert.ToInt32(ViewState["CustomerId"].ToString());
            }

            set
            {
                ViewState["CustomerId"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (ShipmentID == 0)
            {
                Response.Redirect("/shipping/myshipments.aspx", true);
            }

            SetButtonVisibility();

            if (!IsPostBack)
            {
                checkForMessage();
                using (var db = new MadduxEntities())
                {
                    var shipment = db.Shipments.FirstOrDefault(r => r.ShipmentID == ShipmentID);

                    shipment.CalculateFreightAndTaxes();
                    db.SaveChanges();


                    Literal litPageHeader = (Literal)Master.FindControl("litPageHeader");
                    this.Title = "Maddux | Shipment";
                    litPageHeader.Text = string.Format("Shipment {0}", shipment.ShipmentID);

                    ddShipVia.DataSource = db.supShippingMethods.Select(r => new ListItem
                    {
                        Text = r.ShippingMethodDesc,
                        Value = r.ShippingMethodID.ToString(),

                    }).ToList();
                    ddShipVia.DataBind();

                    ddShipVia.SelectedValue = shipment.ShippingMethodId.ToString();
                    var order = db.Orders.FirstOrDefault(r => r.OrderID == shipment.OrderID);

                    lblAddress.Text = order.ShippingAddress;
                    lblCity.Text = order.ShippingCity + ", " + order.ShippingState;
                    lblCountry.Text = order.ShippingCountry;
                    lblPostalCode.Text = order.ShippingZip;
                    hlCustomer.Text = order.ShippingName;
                    hlCustomer.NavigateUrl = $"/customer/customerdetail.aspx?CustomerID={order.CustomerID}";
                    litStarRating.Text = order.Customer.StarRatingGraphic;

                    txtDateCreated.Text = shipment.CreateDate.ToString("MMMM dd, yyyy");
                    txtInvoice.Text = shipment.InvoiceSentDate.HasValue ? shipment.InvoiceSentDate.Value.Date.ToString("MMMM dd, yyyy") : "";
                    txtShipDate.Text = shipment.DateShipped.HasValue ? shipment.DateShipped.Value.Date.ToString("MMMM dd, yyyy") : "";
                    txtOfficeNotes.Text = shipment.OfficeNotes;
                    txtShipmentNotes.Text = shipment.ShipmentNotes;
                    txtTracking.Text = shipment.TrackingNumber;

                    txtDiscount1Desc.Text = shipment.GlobalDiscountDesc;
                    txtDiscount2Desc.Text = shipment.GlobalDiscount2Desc;
                    txtDiscount3Desc.Text = shipment.GlobalDiscount3Desc;
                    txtDiscount4Desc.Text = shipment.GlobalDiscount4Desc;
                    txtDiscount5Desc.Text = shipment.GlobalDiscount5Desc;

                    txtGlobalDiscount1Pct.Text = shipment.GlobalDiscountPercent.ToString("P", CultureInfo.InvariantCulture);
                    txtDiscount2Pct.Text = shipment.GlobalDiscount2Percent.ToString("P", CultureInfo.InvariantCulture);
                    txtDiscount3Pct.Text = shipment.GlobalDiscount3Percent.ToString("P", CultureInfo.InvariantCulture);
                    txtDiscount4Pct.Text = shipment.GlobalDiscount4Percent.ToString("P", CultureInfo.InvariantCulture);
                    txtDiscount5Pct.Text = shipment.GlobalDiscount5Percent.ToString("P", CultureInfo.InvariantCulture);

                    ShowTotals(shipment);
                    LoadItems(shipment);
                    emailTo.Text = string.IsNullOrWhiteSpace(order.Customer.InvoiceEmail) ? "" : order.Customer.InvoiceEmail;
                    emailBody.Text = string.Format(EmailerResources.SendInvoiceEmailBody, order.Customer.Company, shipment.ShipmentID);
                    emailSubject.Text = string.Format(EmailerResources.SendInvoiceEmailSubject, shipment.ShipmentID);

                    emailPackingSlipTo.Text = order.Customer.Email;
                    emailPackingSlipCc.Text = (order.Customer.AlternateEmailReceivesConfirmations && !string.IsNullOrWhiteSpace(order.Customer.AlternateEmail)) ? order.Customer.AlternateEmail : "";
                    emailPackingSlipBody.Text = string.Format(EmailerResources.PackingSlipEmailBody, order.Customer.Company, shipment.ShipmentID);
                    emailPackingSlipSubject.Text = string.Format(EmailerResources.PackingSlipEmailSubject, shipment.ShipmentID);
                }
            }
        }

        private void LoadItems(Shipment shipment)
        {
            grdShipmentItems.DataSource = shipment.ShipmentItems.Select(r => new
            {
                r.OrderItemId,
                r.ShipmentItemId,
                QuantityShipped = r.Quantity,
                QuantityOrdered = r.OrderItem.Quantity,
                r.OrderItem.ProductNotAvailable,
                Category = r.OrderItem.Product.supProductSubCategory.supProductCategory.CategoryDesc,
                ItemNo = r.OrderItem.Product.ItemNumber,
                Description = r.OrderItem.Product.ProductName,
                ProductID = r.OrderItem.Product.ProductId,
                r.OrderItem.Product.Size,
                ItemsPerPack = r.OrderItem.Product.ItemsPerPackage,
                PacksPerUnit = r.OrderItem.Product.PackagesPerUnit,
                Discount = r.OrderItem.DiscountPercent,
                Total = OrderHelper.GetTotalPrice(r.OrderItem.UnitPrice, r.OrderItem.DiscountPercent, r.Quantity).ToString("C")
            }).ToList();
            grdShipmentItems.DataBind();
        }

        private void SetButtonVisibility()
        {
            currentUser = AppSession.Current.CurrentUser;
            if (currentUser.CanEditShipments)
            {

                btnSave.Visible = true;
                btnRemoveSelectedItems.Visible = true;
            }
            else
            {

                btnSave.Visible = false;
                btnRemoveSelectedItems.Visible = false;
            }
            btnDelete.Visible = currentUser.CanDeleteShipments;
            if (currentUser.CanEmailPrintExportShipments)
            {
                btnEmailInvoice.Visible = true;
                btnPrintPackingSlip.Visible = true;
            }
            else
            {
                btnPrintPackingSlip.Visible = false;
                btnEmailInvoice.Visible = false;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (Page.IsValid)
            {
                using (var db = new MadduxEntities())
                {
                    var shipment = db.Shipments.Find(ShipmentID);
                    if (shipment != null)
                    {
                        shipment.ShippingMethodId = Convert.ToInt32(ddShipVia.SelectedValue.ToString().Trim());
                        shipment.TrackingNumber = txtTracking.Text.Trim();
                        shipment.ShipmentNotes = txtShipmentNotes.Text.Trim();
                        shipment.OfficeNotes = txtOfficeNotes.Text.Trim();

                        shipment.CreateDate = !string.IsNullOrWhiteSpace(txtDateCreated.Text) ? DateTime.Parse(txtDateCreated.Text.Trim()) : DateTime.MaxValue;

                        shipment.InvoiceSentDate = !string.IsNullOrWhiteSpace(txtInvoice.Text) ? (DateTime?)DateTime.Parse(txtInvoice.Text.Trim()) : null;

                        shipment.DateShipped = !string.IsNullOrWhiteSpace(txtShipDate.Text) ? (DateTime?)DateTime.Parse(txtShipDate.Text.Trim()) : null;

                        shipment.GlobalDiscountDesc = txtDiscount1Desc.Text.Trim();
                        shipment.GlobalDiscount2Desc = txtDiscount2Desc.Text.Trim();
                        shipment.GlobalDiscount3Desc = txtDiscount3Desc.Text.Trim();
                        shipment.GlobalDiscount4Desc = txtDiscount4Desc.Text.Trim();
                        shipment.GlobalDiscount5Desc = txtDiscount5Desc.Text.Trim();

                        shipment.GlobalDiscountPercent = (double)(Convert.ToDouble(FCSAppUtils.GetNumberString(txtGlobalDiscount1Pct.Text)) / 100);
                        shipment.GlobalDiscount2Percent = (double)(Convert.ToDouble(FCSAppUtils.GetNumberString(txtDiscount2Pct.Text)) / 100);
                        shipment.GlobalDiscount3Percent = (double)(Convert.ToDouble(FCSAppUtils.GetNumberString(txtDiscount3Pct.Text)) / 100);
                        shipment.GlobalDiscount4Percent = (double)(Convert.ToDouble(FCSAppUtils.GetNumberString(txtDiscount4Pct.Text)) / 100);
                        shipment.GlobalDiscount5Percent = (double)(Convert.ToDouble(FCSAppUtils.GetNumberString(txtDiscount5Pct.Text)) / 100);

                        if (chkCustomFreightCharge.Checked)
                        {
                            shipment.CustomShippingCharge = true;
                            shipment.ShippingCharge = Convert.ToDecimal(FCSAppUtils.GetNumberString(txtFreightCharge.Text));
                        }
                        else
                        {
                            shipment.CustomShippingCharge = false;
                        }
                        shipment.PSTExempt = false;

                        //save Shipment Items
                        foreach (GridViewRow row in grdShipmentItems.Rows)
                        {
                            var shipmentItemId = Convert.ToInt32(grdShipmentItems.DataKeys[row.RowIndex].Values[0]);
                            ShipmentItem item = shipment.ShipmentItems.Where(i => i.ShipmentItemId == shipmentItemId).FirstOrDefault();
                            if (item != null)
                            {
                                var qty = row.FindControl("txtQtyShipped") as TextBox;
                                var itemId = Convert.ToInt32(grdShipmentItems.DataKeys[row.RowIndex].Values[1]);
                                item.Quantity = Convert.ToDouble(qty.Text);
                                item.OrderItemId = itemId;

                                row.Cells[row.Cells.Count - 1].Text = (item.Quantity * item.OrderItem.UnitPrice).ToString("C");
                            }
                        }

                        //calculate freight and taxes
                        shipment.CalculateFreightAndTaxes();

                        db.SaveChanges();

                        ShowTotals(shipment);
                    }
                }
            }
        }

        private void ShowTotals(Shipment shipment)
        {
            lblDiscountedSubTotal.Text = shipment.DiscountedSubTotal.ToString("C");

            lblGlobalDiscount1Discount.Text = shipment.GlobalDiscountAmount1.ToString("C");
            lblDiscount2.Text = shipment.GlobalDiscountAmount2.ToString("C");
            lblDiscount3.Text = shipment.GlobalDiscountAmount3.ToString("C");
            lblDiscount4.Text = shipment.GlobalDiscountAmount4.ToString("C");
            lblDiscount5.Text = shipment.GlobalDiscountAmount5.ToString("C");
            lblDiscountedSubTotal2.Text = shipment.GlobalDiscountedSubTotal.ToString("C");

            chkCustomFreightCharge.Checked = shipment.CustomShippingCharge;
            txtFreightCharge.Enabled = shipment.CustomShippingCharge;
            txtFreightCharge.Text = shipment.ShippingCharge.ToString("C");

            lblGST.Text = (shipment.GSTAmount + shipment.PSTAmount).ToString("C");
            lblHST.Text = String.Format("{0:C}", shipment.GSTAmount + shipment.PSTAmount);

            if (shipment.HST == true)
            {
                lblHSTCaption.Visible = true;
                lblHST.Visible = true;
                lblGST.Visible = false;
                lblGSTCaption.Visible = false;
            }

            lblTotal.Text = shipment.ShipmentGrandTotal.ToString("C");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            using (var db = new MadduxEntities())
            {
                var shipment = db.Shipments.Find(ShipmentID);
                if (shipment != null)
                {
                    db.Shipments.Remove(shipment);
                    db.SaveChanges();
                    Response.Redirect("/shipping/myshipments.aspx");
                }
            }
        }

        protected void grdShipmentItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HyperLink lnk = e.Row.FindControl("lnkEditProduct") as HyperLink;
                Label lbl = e.Row.FindControl("lblProductName") as Label;
                lnk.Visible = currentUser.ShowSettings;
                lbl.Visible = !currentUser.ShowSettings;
            }
        }

        protected void chkCustomFreightCharge_CheckedChanged(object sender, EventArgs e)
        {
            txtFreightCharge.Enabled = chkCustomFreightCharge.Checked == true;
        }

        protected void btnRemoveSelectedItems_Click(object sender, EventArgs e)
        {
            using (var db = new MadduxEntities())
            {
                Shipment shipment = db.Shipments.Find(ShipmentID);
                foreach (GridViewRow row in grdShipmentItems.Rows)
                {
                    var selectedCell = row.Cells[0];

                    var checkbox = (CheckBox)selectedCell.FindControl("chkSelected");

                    if (checkbox.Checked)
                    {
                        var id = Convert.ToInt32(grdShipmentItems.DataKeys[row.RowIndex].Values[0]);
                        var shipmentItem = shipment.ShipmentItems.FirstOrDefault(s => s.ShipmentItemId == id);
                        if (shipmentItem != null)
                        {
                            db.ShipmentItems.Remove(shipmentItem);
                        }
                    }
                }
                db.SaveChanges();

                LoadItems(shipment);
                ShowTotals(shipment);
            }
        }

        private bool PrintPackingSlip(Shipment shipment)
        {
            try
            {

                using (var writer = new StringWriter())
                {
                    Server.Execute($"~/order/packing-slip.aspx?id={shipment.OrderID}&sId={shipment.ShipmentID}&view=print", writer);
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

                    Response.Clear();
                    Response.ContentType = EmailerResources.PDFContentType;
                    Response.AddHeader("Content-Disposition", $"attachment; filename=packingslip#{shipment.OrderID}-{DateTime.Now:yyyy-MM-dd-hh-mm-tt}.pdf");
                    Response.OutputStream.Write(pdfResponse.RawBytes, 0, pdfResponse.RawBytes.Length);
                    Response.End();
                    return true;
                }
            }
            catch
            {
                return false;
            }

        }


        protected void btnPrintPackingSlip_Click(object sender, EventArgs e)
        {
            try
            {
                using (var db = new MadduxEntities())
                {
                    var ship = db.Shipments.FirstOrDefault(x => x.ShipmentID == ShipmentID);
                    PrintPackingSlip(ship);
                }

            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }
        private EmailResponse EmailInvoice()
        {
            EmailResponse response = new EmailResponse
            {
                Sent = false
            };
            try
            {
                Emailer emailer = new Emailer();
                using (var writer = new StringWriter())
                using (var db = new MadduxEntities())
                {
                    var ship = db.Shipments.FirstOrDefault(x => x.ShipmentID == ShipmentID);

                    Server.Execute($"/shipping/invoice.aspx?id={ship.OrderID}&sID={ship.ShipmentID}&view=print", writer);
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

                    response.Sent = false;
                    if (!string.IsNullOrWhiteSpace(emailTo.Text))
                    {
                        Dictionary<int, string> recipients = new Dictionary<int, string>
                        {
                            { CustomerID, emailTo.Text }
                        };
                        response.Sent = emailer.SendEmailWithAttachment(
                                                   recipients,
                                                   emailSubject.Text,
                                                   $@"{emailBody.Text} <br /> {EmailerResources.EmailFooter}",
                                                   pdfResponse.RawBytes,
                                                   $"invoice-{ship.OrderID}.pdf",
                                                   EmailerResources.PDFContentType,
                                                   new List<string> { emailCC.Text }
                                                );
                    }

                    response.data = pdfResponse.RawBytes;

                    return response;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void btnEmailInvoice_Click(object sender, EventArgs e)
        {
            try
            {
                using (var db = new MadduxEntities())
                {
                    Shipment ship = db.Shipments.Where(x => x.ShipmentID == ShipmentID).FirstOrDefault();
                    if (ship == null)
                        return;
                    InvoicePostBatchItem invoicePostBatchItem = db.InvoicePostBatchItems.FirstOrDefault(x => x.ShipmentID == ShipmentID);
                    var customer = db.Customers.Select(x => new
                    {
                        x.CustomerId,
                        x.Email,
                        x.InvoiceEmail,
                        CanSendInvoice = x.EmailInvoice,
                        CustomerName = x.Company
                    }).FirstOrDefault(x => x.CustomerId == CustomerID);
                    if (invoicePostBatchItem == null)
                    {

                        InvoicePostBatch invoicePostBatch = db.InvoicePostBatches.Create();
                        invoicePostBatch.BatchDate = DateTime.Now;

                        invoicePostBatchItem = db.InvoicePostBatchItems.Create();
                        invoicePostBatchItem.ShipmentID = ShipmentID;
                        invoicePostBatchItem.InvoicePrinted = false;
                        invoicePostBatchItem.InvoiceEmailed = false;
                        invoicePostBatchItem.InvoicePosted = false;
                        invoicePostBatchItem.InvoicePostedMessage = string.Empty;
                        invoicePostBatchItem.EmailAddress = customer.Email;
                        invoicePostBatchItem.InvoiceCopies = 1;
                        invoicePostBatchItem.InvoiceTotal = Convert.ToDecimal(ship.ShipmentGrandTotal);

                        invoicePostBatch.InvoicePostBatchItems.Add(invoicePostBatchItem);


                        db.InvoicePostBatches.Add(invoicePostBatch);
                        db.SaveChanges();


                    }

                    EmailResponse result = EmailInvoice();

                    if (result.data.Length > 0)
                    {
                        //Setting shipdate for a shipment
                        //shipment.DateShipped = (DateTime)DateTime.Now.Date;
                        //shipment.InvoiceSentDate = (DateTime)DateTime.Now.Date;

                        invoicePostBatchItem.InvoiceEmailed = true;

                        db.SaveChanges();
                        if (result.Sent == false)
                        {
                            invoicePostBatchItem.InvoiceEmailed = false;
                            litMessage.Text = !customer.CanSendInvoice || string.IsNullOrWhiteSpace(customer.InvoiceEmail)
                                ? StringTools.GenerateError($"{customer.CustomerName} doesn't have any invoice email assigned")
                                : StringTools.GenerateError($"Error while sending invoice to {customer.CustomerName} please try again!");
                        }
                        else
                        {
                            litMessage.Text = StringTools.GenerateSuccess("Invoice Sent!");
                            if (chkUpdateInvoiceSentDate.Checked)
                            {
                                if (!string.IsNullOrWhiteSpace(invoiceDate.Text))
                                {
                                    Shipment shipment = db.Shipments.FirstOrDefault(sh => sh.ShipmentID == ShipmentID);
                                    shipment.InvoiceSentDate = DateTime.Parse(invoiceDate.Text);
                                    db.SaveChanges();
                                    Response.Redirect($"~/shipping/shipmentdetail.aspx?id={shipment.ShipmentID}&CustomerId={CustomerID}&success=true&message=Invoice Sent!");
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }
        private void checkForMessage()
        {

            if (Request.QueryString["success"] != null && Request.QueryString["success"] != "")
            {
                string success = Request.QueryString["success"].ToString();
                string message = Request.QueryString["message"].ToString();
                if (string.Equals(success, "false"))
                {
                    litMessage.Text = StringTools.GenerateError(message);
                }
                if (string.Equals(success, "true"))
                {
                    litMessage.Text = StringTools.GenerateSuccess(message);
                }
            }
        }
        private bool EmailPackingSlip(Shipment shipment)
        {
            try
            {
                Emailer emailer = new Emailer();
                using (var writer = new StringWriter())
                using (var db = new MadduxEntities())
                {
                    Server.Execute($"~/order/packing-slip.aspx?id={shipment.OrderID}&sId={shipment.ShipmentID}&view=print", writer);
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

                    Dictionary<int, string> recipients = new Dictionary<int, string>
                    {
                        { CustomerID, emailPackingSlipTo.Text }
                    };

                    bool emailSent = emailer.SendEmailWithAttachment(
                                                   recipients,
                                                   emailPackingSlipSubject.Text,
                                                   emailPackingSlipBody.Text + "<br>" + EmailerResources.EmailFooter,
                                                   pdfResponse.RawBytes,
                                                   $"packing-slip-{shipment.OrderID}.pdf",
                                                   EmailerResources.PDFContentType,
                                                   new List<string> { emailPackingSlipCc.Text }
                                                );
                    return true;
                }
            }
            catch
            {

                return false;
            }
        }
        protected void btnEmailPackingSlip_Click(object sender, EventArgs e)
        {
            try
            {
                using (var db = new MadduxEntities())
                {
                    var ship = db.Shipments.FirstOrDefault(x => x.ShipmentID == ShipmentID);
                    //var _ship = db.vwMyShipments.Where(x => x.ShipmentID == ShipmentID).FirstOrDefault();
                    var customer = db.Customers.Select(x => new
                    {
                        x.CustomerId,
                        x.Email,
                        CustomerName = x.Company
                    }).FirstOrDefault(x => x.CustomerId == CustomerID);

                    bool result = EmailPackingSlip(ship);

                    litMessage.Text = result
                        ? StringTools.GenerateSuccess("Packing slip sent!")
                        : StringTools.GenerateError($"Error while sending packing slip to {customer.CustomerName} please try again!");
                }

            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }
        protected void btnPrintInvoice_Click(object sender, EventArgs e)
        {
            try
            {
                using (var writer = new StringWriter())
                using (var db = new MadduxEntities())
                {
                    var ship = db.Shipments.Where(s => s.ShipmentID == ShipmentID).FirstOrDefault();
                    //var ship = db.Shipments.Where(x => x.ShipmentID == ShipmentID).FirstOrDefault();
                    if (ship == null)
                        return;
                    var invoicePostBatchItem = db.InvoicePostBatchItems.FirstOrDefault(x => x.ShipmentID == ShipmentID);
                    var customer = db.Customers.Select(x => new
                    {
                        x.CustomerId,
                        x.Email,
                        x.InvoiceEmail,
                        CanSendInvoice = x.EmailInvoice,
                        CustomerName = x.Company
                    }).FirstOrDefault(x => x.CustomerId == CustomerID);
                    if (invoicePostBatchItem == null)
                    {

                        var invoicePostBatch = db.InvoicePostBatches.Create();
                        invoicePostBatch.BatchDate = DateTime.Now;

                        invoicePostBatchItem = db.InvoicePostBatchItems.Create();
                        invoicePostBatchItem.ShipmentID = ShipmentID;
                        invoicePostBatchItem.InvoicePrinted = false;
                        invoicePostBatchItem.InvoiceEmailed = false;
                        invoicePostBatchItem.InvoicePosted = false;
                        invoicePostBatchItem.InvoicePostedMessage = string.Empty;
                        invoicePostBatchItem.EmailAddress = customer.Email;
                        invoicePostBatchItem.InvoiceCopies = 1;
                        invoicePostBatchItem.InvoiceTotal = Convert.ToDecimal(ship.ShipmentGrandTotal);

                        invoicePostBatch.InvoicePostBatchItems.Add(invoicePostBatchItem);


                        db.InvoicePostBatches.Add(invoicePostBatch);
                        db.SaveChanges();


                    }
                    Shipment shipment = db.Shipments.FirstOrDefault(sh => sh.ShipmentID == ShipmentID);
                    var url = $"/shipping/invoice.aspx?id={shipment.OrderID}&sID={ShipmentID}&view=print";
                    Server.Execute($"/shipping/invoice.aspx?id={shipment.OrderID}&sID={ShipmentID}&view=print", writer);
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

                    if (pdfResponse.RawBytes.Length > 0)
                    {

                        invoicePostBatchItem.InvoicePrinted = true;

                        db.SaveChanges();
                        //Print Invoice
                        Response.Clear();
                        Response.ContentType = EmailerResources.PDFContentType;
                        Response.AddHeader("Content-Disposition", $"attachment; filename=invoice-{shipment.ShipmentID}-{DateTime.Now:yyyy-MM-dd-hh-mm-tt}.pdf");
                        Response.OutputStream.Write(pdfResponse.RawBytes, 0, pdfResponse.RawBytes.Length);
                        Response.Flush();
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