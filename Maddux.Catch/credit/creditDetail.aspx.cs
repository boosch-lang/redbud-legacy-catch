using Newtonsoft.Json;
using Redbud.BL.DL;
using Redbud.BL.Resources;
using Redbud.BL.Utils;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;

namespace Maddux.Catch.credit
{
    public partial class creditDetail : System.Web.UI.Page
    {
        private int CreditID
        {
            get
            {
                if (ViewState["CreditID"] == null)
                {
                    ViewState["CreditID"] = Request.QueryString["CreditID"] == null || Request.QueryString["CreditID"] == "" ? 0 : (object)Request.QueryString["CreditID"];
                }
                return Convert.ToInt32(ViewState["CreditID"].ToString());
            }

            set
            {
                ViewState["CreditID"] = value;
            }
        }

        private int CustomerID
        {
            get
            {
                if (ViewState["CustomerId"] == null)
                {
                    ViewState["CustomerId"] = Request.QueryString["CustomerId"] == null || Request.QueryString["CustomerId"] == "" ? 0 : (object)Request.QueryString["CustomerId"];
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
            Literal litPageHeader = (Literal)Master.FindControl("litPageHeader");
            if (!IsPostBack)
            {

                try
                {
                    using (var db = new MadduxEntities())
                    {
                        var credit = new Credit();
                        Customer customer = db.Customers.AsNoTracking().FirstOrDefault(c => c.CustomerId == CustomerID);
                        if (CreditID != 0)
                        {

                            credit = db.Credits.AsNoTracking().Include(c => c.CreditItems).FirstOrDefault(r => r.CreditID == CreditID);
                            if (credit != null)
                            {

                                litPageHeader.Text = $"Credit Memo # {credit.CreditID} <small>({customer.Company})</small>";
                                txtDate.Text = credit.CreateDate.ToString("MMMM dd, yyyy");
                                txtNotes.Text = credit.CreditNotes;
                                txtOfficeNotes.Text = credit.OfficeNotes;

                                lblFreight.Text = credit.FreightCredit.ToString("C");

                                lblSubtotal.Text = credit.SubTotalAmount.ToString("C");
                                lblGST.Text = (credit.GSTAmount + credit.PSTAmount).ToString("C");
                                lblTotal.Text = credit.Total.ToString("C");
                                TaxTypeText.InnerText = TaxUtilities.GetTaxTypeText(customer.State);

                                string crediAmount = credit.FreightCredit.ToString();
                                if (credit.OverrideFreightCredit)
                                {
                                    FreightLabelDiv.Attributes.Add("style", "display:none");
                                    txtFreight.Text = crediAmount.Remove(crediAmount.Length - 2);
                                    chkOverrideFreightCredit.Checked = true;
                                }
                                else
                                {
                                    FreightTextDiv.Attributes.Add("style", "display:none");
                                }

                                txtFreight.Text = crediAmount.Remove(crediAmount.Length - 2);

                                emailTo.Text = string.IsNullOrWhiteSpace(customer.Email) ? string.Empty : customer.Email;
                                emailSubject.Text = string.Format(EmailerResources.CreditMemoEmailSubject, CreditID);
                                emailBody.Text = string.Format(EmailerResources.CreditMemoEmailBody, customer.Company, CreditID);


                                dgvItems.DataSource = credit.CreditItems.ToList();
                                dgvItems.DataBind();
                            }
                        }
                        else
                        {
                            litPageHeader.Text = $"New Credit Memo <small>({customer.Company})</small>";
                            //pnlItems.Visible = false;
                            PrintMemo.Visible = false;
                            EmailConfirmBtn.Visible = false;
                        }



                    }
                    PopulateDropdown();
                }
                catch (Exception ex)
                {
                    litMessage.Text = StringTools.GenerateError(ex.Message);
                }

            }

        }

        private void PopulateDropdown()
        {
            using (var db = new MadduxEntities())
            {
                var orders = db.Orders.Where(r => r.CustomerID == CustomerID);
                List<int> orderItemIds = new List<int>();
                foreach (var item in orders)
                {
                    var q = item.OrderItems.Select(r => r.OrderItemId);
                    orderItemIds.AddRange(q);
                }

                var shipments = db.Shipments.Where(r => r.ShipmentItems.Any(q => orderItemIds.Contains(q.OrderItemId))).ToList();
                var items = new List<ListItem>();
                //+ "(" + r.DateShipped.Value.ToString("d MMM yyyy") + ")"
                items = shipments.Select(r => new ListItem
                {
                    Text = $"#{r.ShipmentID} - {r.DateShipped?.ToString("MMMM dd, yyyy")}",
                    Value = r.ShipmentID.ToString()
                }).ToList();

                items.Insert(0, new ListItem
                {
                    Text = "Select a shipment",
                    Value = "0"
                });
                ddShipmentList.DataSource = items;
                ddShipmentList.DataBind();
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                using (var db = new MadduxEntities())
                {
                    var credit = new Credit();

                    if (CreditID == 0)
                    {
                        credit.CustomerID = CustomerID;
                        db.Credits.Add(credit);

                    }
                    else
                    {
                        credit = db.Credits.FirstOrDefault(c => c.CreditID == CreditID);
                    }
                    credit.CreditNotes = txtNotes.Text;
                    credit.CreateDate = DateTime.Parse(txtDate.Text);
                    credit.OfficeNotes = txtOfficeNotes.Text;
                    // var credits = db.Credits.Include(c => c.CreditItems).FirstOrDefault(r => r.CreditID == CreditID);
                    foreach (GridViewRow row in dgvItems.Rows)
                    {

                        TextBox priceTextBox = row.FindControl("txtPrice") as TextBox;
                        var creditItem = credit.CreditItems.ToArray()[row.RowIndex];
                        creditItem.EachPrice = Convert.ToDecimal(priceTextBox.Text);

                    }
                    if (chkOverrideFreightCredit.Checked)
                    {
                        credit.OverrideFreightCredit = true;
                        credit.FreightCredit = Convert.ToDecimal(txtFreight.Text);
                    }
                    else
                    {
                        credit.OverrideFreightCredit = false;
                        credit.FreightCredit = 0;
                    }

                    var customer = db.Customers.Include(c => c.State).Where(c => c.CustomerId == CustomerID).Select(c => new { c.CustomerId, c.State, PostalCode = c.Zip }).FirstOrDefault();
                    State _state = db.States.FirstOrDefault(s => s.StateID == customer.State);
                    credit.GSTAmount = Convert.ToDecimal(TaxUtilities.GetTaxPercentage(_state.StateID, null) * (credit.SubTotalAmount + Convert.ToDouble(credit.FreightCredit)));

                    db.SaveChanges();

                    if (CreditID == 0)
                    {
                        CreditID = credit.CreditID;
                    }
                }

                Response.Redirect("/credit/creditdetail.aspx?creditid=" + CreditID + "&customerid=" + CustomerID);
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }

        }

        protected void ddShipmentList_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (var db = new MadduxEntities())
            {
                var shipmentId = int.Parse(ddShipmentList.SelectedValue);
                var shipment = db.Shipments.FirstOrDefault(r => r.ShipmentID == shipmentId);
                if (shipment != null)
                {
                    grdOrderItems.DataSource = shipment.ShipmentItems.Select(r => new
                    {
                        ProductID = r.OrderItem.ProductId,
                        Quantity = r.Quantity * r.OrderItem.Product.PackagesPerUnit * r.OrderItem.Product.ItemsPerPackage,
                        EachPrice = (r.OrderItem.UnitPrice / r.OrderItem.Product.PackagesPerUnit),
                        PacksPerUnit = r.OrderItem.Product.PackagesPerUnit,
                        ItemsPerPack = r.OrderItem.Product.ItemsPerPackage,
                        r.OrderItem.Product.ItemNumber,
                        Description = r.OrderItem.Product.ProductName,
                        r.OrderItem.Product.Size,
                        UnitPrice = r.OrderItem.UnitPrice.ToString("C"),
                        Discount = r.OrderItem.DiscountPercent
                    }).ToList();
                    grdOrderItems.DataBind();
                }
                ScriptDIV.InnerHtml = "";
                ScriptDIV.InnerHtml = $@"<script> $(document).ready(function(){{ 
                                                $('#modalView').modal('show');
                                             }}) 
                                        </script>";

            }
        }

        protected void saveAndClose_Click(object sender, EventArgs e)
        {
            try
            {
                using (var db = new MadduxEntities())
                {
                    int creditID = CreditID;
                    var customer = db.Customers.Include(c => c.State).Where(c => c.CustomerId == CustomerID).Select(c => new { c.CustomerId, c.State, PostalCode = c.Zip }).FirstOrDefault();
                    Credit credit = db.Credits.Create();
                    if (CreditID == 0)
                    {
                        credit.CustomerID = CustomerID;
                        credit.CreateDate = string.IsNullOrWhiteSpace(txtDate.Text) ? DateTime.Now : DateTime.Parse(txtDate.Text);
                        db.Credits.Add(credit);
                        db.SaveChanges();
                        CreditID = credit.CreditID;
                        creditID = credit.CreditID;
                    }

                    if (CreditID > 0)
                    {
                        credit = db.Credits.FirstOrDefault(r => r.CreditID == CreditID);
                    }
                    foreach (GridViewRow row in grdOrderItems.Rows)
                    {
                        var productId = row.Cells[0].FindControl("hdnId") as HiddenField;
                        var units = row.Cells[0].FindControl("txtUnits") as TextBox;
                        var unitPrice = row.Cells[0].FindControl("hdnPrice") as HiddenField;
                        var packsPerUnit = row.Cells[0].FindControl("hdnPacksPerUnit") as HiddenField;
                        if ((!string.IsNullOrEmpty(units.Text) && units.Text != "0") && int.TryParse(units.Text, out int qty))
                        {
                            if (int.TryParse(productId.Value, out int prodId) && int.TryParse(packsPerUnit.Value, out int packs) && decimal.TryParse(unitPrice.Value, out decimal price))
                            {
                                credit.CreditItems.Add(new CreditItem
                                {
                                    ProductId = prodId,
                                    EachPrice = price,
                                    Units = qty,
                                    PackagesPerUnit = packs

                                });

                            }

                        }
                    }
                    State _state = db.States.FirstOrDefault(s => s.StateID == customer.State);
                    credit.FreightCredit = chkOverrideFreightCredit.Checked == false
                        ? Convert.ToDecimal(FreightCalculator.CalculateFreighCharge(credit.SubTotalAmount, customer.State, customer.PostalCode))
                        : Convert.ToDecimal(txtFreight.Text);

                    credit.GSTAmount = Convert.ToDecimal(TaxUtilities.GetTaxPercentage(_state.StateID, null) * (credit.SubTotalAmount + Convert.ToDouble(credit.FreightCredit)));
                    credit.PSTExempt = false;
                    credit.PSTAmount = 0;
                    db.SaveChanges();
                    Response.Redirect("/credit/creditdetail.aspx?creditid=" + creditID + "&customerid=" + CustomerID);
                }
            }
            catch (Exception ex)
            {
                ScriptDIV.InnerHtml = "";
                ScriptDIV.InnerHtml = $@"<script> $(document).ready(function(){{ 
                                                $('#modalView').modal('show');
                                             }}) 
                                        </script>";
                litModalMessage.Text = StringTools.GenerateError(ex.Message);
            }


        }

        protected void RemoveProduct_Click(object sender, EventArgs e)
        {
            try
            {
                string argument = ((Button)sender).CommandArgument;
                int creditItemId = 0;
                if (!string.IsNullOrWhiteSpace(argument))
                {
                    creditItemId = Convert.ToInt32(argument);
                }
                using (MadduxEntities madduxEntities = new MadduxEntities())
                {
                    CreditItem creditItem = madduxEntities.CreditItems.FirstOrDefault(ci => ci.CreditItemId == creditItemId);
                    if (creditItem != null)
                    {
                        madduxEntities.CreditItems.Remove(creditItem);
                        madduxEntities.SaveChanges();
                    }
                    Response.Redirect("/credit/creditdetail.aspx?creditid=" + CreditID + "&customerid=" + CustomerID);
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }

        }

        protected void PrintMemo_Click(object sender, EventArgs e)
        {
            try
            {
                using (var writer = new StringWriter())
                {
                    Server.Execute($"/credit/creditMemoPrint.aspx?customerID={CustomerID}&creditID={CreditID}&view=print", writer);
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
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", $"attachment; filename=creditmemo#{CreditID}.pdf");
                    Response.OutputStream.Write(pdfResponse.RawBytes, 0, pdfResponse.RawBytes.Length);
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                using (var db = new MadduxEntities())
                {
                    Credit credit = db.Credits.Include(c => c.CreditItems).FirstOrDefault(r => r.CreditID == CreditID);
                    if (credit != null)
                    {
                        var removeCreditItems = db.CreditItems.RemoveRange(credit.CreditItems);
                        db.Credits.Remove(credit);
                        db.SaveChanges();
                        Response.Redirect($"~/customer/customerdetail.aspx?CustomerID={CustomerID}&message=Credit memo removed!&type=success");
                    }
                    else
                    {
                        throw new Exception("Error while retrieving credit memo!");
                    }
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }

        protected void btnEmailCreditMemo_Click(object sender, EventArgs e)
        {
            try
            {
                Emailer emailer = new Emailer();
                using (var writer = new StringWriter())
                using (var db = new MadduxEntities())
                {
                    var credit = db.Credits.FirstOrDefault(x => x.CreditID == CreditID);


                    var customer = db.Customers.Select(x => new
                    {
                        x.CustomerId,
                        x.Email,
                        x.InvoiceEmail,
                        CanSendInvoice = x.EmailInvoice,
                        CustomerName = x.Company
                    }).FirstOrDefault(x => x.CustomerId == credit.CustomerID);

                    //if (!customer.CanSendInvoice || string.IsNullOrWhiteSpace(customer.InvoiceEmail))
                    //{
                    //    litMessage.Text = StringTools.GenerateError($"{customer.CustomerName} doesn't have any invoice email assigned.");
                    //    return;
                    //}

                    Server.Execute($"/credit/creditMemoPrint.aspx?creditID={CreditID}&customerid={customer.CustomerId}&view=print", writer);
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

                    if (!string.IsNullOrWhiteSpace(emailTo.Text))
                    {
                        Dictionary<int, string> recipients = new Dictionary<int, string>
                        {
                            { credit.Customer.CustomerId, emailTo.Text }
                        };

                        bool emailSent = emailer.SendEmailWithAttachment(
                                                       recipients,
                                                       string.Format(emailSubject.Text),
                                                       $@"{emailBody.Text} {EmailerResources.EmailFooter}",
                                                       pdfResponse.RawBytes,
                                                       $"credit-memo-{CreditID}.pdf",
                                                       EmailerResources.PDFContentType,
                                                       new List<string> { emailCC.Text }
                                                    );

                        if (emailSent)
                        {
                            litMessage.Text = StringTools.GenerateSuccess($"Credit memo sent successfully!");
                        }
                        else
                            throw new Exception($"Error while sending credit memo to {customer.CustomerName} please try again!");
                    }
                    else
                    {
                        throw new Exception("Please insert recepients email address!");
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