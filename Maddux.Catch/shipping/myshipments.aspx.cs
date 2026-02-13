using Maddux.Catch.LocalClasses;
using Newtonsoft.Json;
using Redbud.BL.DL;
using Redbud.BL.Resources;
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

namespace Maddux.Catch.shipping
{
    public partial class myshipments : System.Web.UI.Page
    {
        //decimal p_ShipmentTotal = 0;
        private int BatchID
        {
            get
            {
                if (ViewState["BatchID"] == null)
                {
                    ViewState["BatchID"] = Request.QueryString["batchID"] == null || Request.QueryString["batchID"] == "" ? 0 : (object)Request.QueryString["batchID"];
                }
                return Convert.ToInt32(ViewState["BatchID"].ToString());
            }

            set
            {
                ViewState["BatchID"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            if (!Page.IsPostBack)
            {
                LoadFilterDropDowns();
                LoadGrid();
                if (BatchID != 0)
                {
                    printBatchReport.Attributes.Add("src", $@"/shipping/request/print-batch-report.ashx?batchID={BatchID}");
                }
            }

            Title = "Maddux | Shipments";
        }

        private void LoadFilterDropDowns()
        {
            try
            {
                User currentUser = AppSession.Current.CurrentUser;

                Lookup.LoadProvinceDropDown(ref ddlFilterProvince, currentUser.CanOnlyViewAssignedProvinces, true, false, currentUser.UserID);
                Lookup.LoadCatalogDropDown(ref ddlFilterCatalog, currentUser.CanOnlyViewAssignedCatalogs, true, currentUser.UserID);
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }

        private void LoadGrid()
        {
            try
            {
                using (var db = new MadduxEntities())
                {
                    var province = ddlFilterProvince.SelectedValue.ToString();
                    var catalogId = Convert.ToInt32(ddlFilterCatalog.SelectedValue);
                    var shipments = from s in db.vwMyShipments
                                    select s;

                    if (province != "00")
                    {
                        shipments = shipments.Where(r => r.Province == province);
                    }

                    if (catalogId != -1)
                    {
                        var shipCatalogs = db.vwShipmentCatalogs.Where(r => r.CatalogId == catalogId).Select(r => r.ShipmentId);
                        shipments = shipments.Where(r => shipCatalogs.Contains(r.ShipmentID));
                    }

                    var user = AppSession.Current.CurrentUser;

                    if (user.CanOnlyViewAssignedAssociations)
                    {
                        var asscs = db.UserAsscs.Where(r => r.UserID == user.UserID).Select(r => r.AssociationID);
                        var associations = db.vwCustomersByAssociations.Where(r => asscs.Contains(r.AssociationID)).Select(r => r.CustomerID).ToList();
                        shipments = shipments.Where(r => associations.Contains(r.CustomerID));
                    }
                    else if (user.CanOnlyViewAssignedProvinces)
                    {
                        var states = db.UserStates.Where(r => r.UserID == user.UserID).Select(r => r.StateID);
                        var customers = db.Customers.Where(r => states.Contains(r.State)).Select(r => r.CustomerId);
                        var associations = db.vwCustomersByAssociations.Where(r => customers.Contains(r.CustomerID)).Select(r => r.CustomerID).ToList();
                        shipments = shipments.Where(r => associations.Contains(r.CustomerID));
                    }

                    var shipmentResults = shipments.ToList().Select(x => new
                    {
                        x.ShipmentID,
                        OrderID = x.OrderId,
                        RackName = db.OrderRacks.FirstOrDefault(or => or.OrderId == x.OrderId).RackName,
                        x.CreateDate,
                        x.InvoiceSentDate,
                        Catalog = db.vwShipmentCatalogs.Where(sc => sc.ShipmentId == x.ShipmentID).Select(sc => sc.CatalogName).FirstOrDefault(),
                        x.ShippingMethodDesc,
                        x.ShippingName,
                        CustomerId = x.CustomerID,
                        StarRatingGraphic = GenerateStarGraphic(x.StarRating),
                        x.SalesPerson,
                        x.ShipmentTotal,
                        x.Province
                    }).OrderByDescending(s => s.ShipmentID).ToList();

                    dgvShipments.DataSource = shipmentResults;
                    dgvShipments.DataBind();

                    Literal litPageHeader = (Literal)Master.FindControl("litPageHeader");
                    litPageHeader.Text = "Shipments";

                    var shipmentTotal = shipmentResults.Sum(x => Convert.ToInt32(x.ShipmentTotal));
                    Literal litTotal = (Literal)Master.FindControl("litTotal");
                    litTotal.Text = dgvShipments.Rows.Count.ToString() + " Shipments (Total value: " + shipmentTotal.ToString("C2") + ")";
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }

        private string GenerateStarGraphic(int? starRating)
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

        protected void ddlFilterSalesRep_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGrid();
        }

        protected void ddlFilterCatalog_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGrid();
        }

        protected void ddlFilterProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGrid();
        }
        private List<int> getSelecteShipments()
        {
            List<int> shipmentIDs = new List<int>();
            foreach (GridViewRow gridRow in dgvShipments.Rows)
            {
                CheckBox chkBox = (CheckBox)gridRow.FindControl("chkShipmentSelect");
                if (chkBox.Checked)
                {
                    int id = Convert.ToInt32(dgvShipments.DataKeys[gridRow.RowIndex].Values[0]);
                    shipmentIDs.Add(id);
                }
            }
            return shipmentIDs;
        }
        protected void btnShipSelected_ServerClick(object sender, EventArgs e)
        {
            User currentUser = AppSession.Current.CurrentUser;
            try
            {
                var batchItems = new List<InvoicePostBatchItem>();
                List<int> shipmentIDs = getSelecteShipments();
                shipmentIDs.Reverse();
                string InvoiceHtml = string.Empty;
                Emailer emailer = new Emailer();
                using (var db = new MadduxEntities())
                {
                    InvoicePostBatch batch = new InvoicePostBatch
                    {
                        BatchDate = DateTime.Now
                    };
                    db.InvoicePostBatches.Add(batch);
                    db.SaveChanges();
                    foreach (int _id in shipmentIDs)
                    {
                        using (var writer = new StringWriter())
                        {
                            var shipment = db.Shipments.Find(_id);

                            var batchItem = new InvoicePostBatchItem
                            {
                                ShipmentID = shipment.ShipmentID,
                                InvoicePrinted = false,
                                InvoiceCopies = 0,
                                InvoiceEmailed = false,
                                InvoicePosted = false,
                                InvoiceTotal = Convert.ToDecimal(shipment.ShipmentGrandTotal),
                                EmailAddress = string.Empty,
                                InvoicePostedMessage = string.Empty
                            };
                            batch.InvoicePostBatchItems.Add(batchItem);
                            db.SaveChanges();

                            //Setting shipdate for a shipment
                            shipment.DateShipped = (DateTime)DateTime.Now.Date;
                            if (chkUpdateShippedDate.Checked && !string.IsNullOrWhiteSpace(shippedDate.Text))
                            {
                                shipment.DateShipped = DateTime.Parse(shippedDate.Text);
                            }
                            if (shipment.InvoiceSentDate == null)
                            {
                                shipment.InvoiceSentDate = DateTime.Now.Date;
                            }
                            db.SaveChanges();

                            bool printed = false;
                            bool emailed = false;
                            bool posted = false;
                            int copiesPrinted = 1;
                            string emailAddress = "";
                            var order = db.Orders.Find(shipment.OrderID);

                            Server.Execute($"/shipping/invoice.aspx?id={shipment.OrderID}&sID={shipment.ShipmentID}&view=print", writer);
                            string html = writer.GetStringBuilder().ToString();
                            InvoiceHtml += html;

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

                            if (!string.IsNullOrWhiteSpace(order.Customer.InvoiceEmail))
                            {
                                //try to send invoice by email
                                bool emailSent = false;

                                Dictionary<int, string> recipients = new Dictionary<int, string>
                                {
                                    { order.Customer.CustomerId, order.Customer.InvoiceEmail }
                                };

                                emailSent = emailer.SendEmailWithAttachment(
                                                recipients,
                                                string.Format(EmailerResources.SendInvoiceEmailSubject, shipment.ShipmentID),
                                                string.Format(EmailerResources.SendInvoiceEmailBody, order.Customer.Company, shipment.ShipmentID) + "<br>" + EmailerResources.EmailFooter,
                                                pdfResponse.RawBytes,
                                                $"invoice-{shipment.ShipmentID}.pdf",
                                                EmailerResources.PDFContentType
                                             );
                                var success = emailSent;
                                if (success)
                                {
                                    emailed = true;
                                    emailAddress = order.Customer.InvoiceEmail;
                                }
                            }


                            batchItem.ShipmentID = shipment.ShipmentID;
                            batchItem.OrderID = shipment.OrderID;
                            batchItem.InvoicePrinted = printed;
                            batchItem.InvoiceCopies = copiesPrinted;
                            batchItem.InvoiceEmailed = emailed;
                            batchItem.InvoicePosted = posted;
                            batchItem.InvoiceTotal = Convert.ToDecimal(shipment.ShipmentGrandTotal);
                            batchItem.EmailAddress = emailAddress;
                            batchItem.InvoicePostedMessage = string.Empty;
                            db.SaveChanges();
                        }
                    }
                    Response.Redirect($"/shipping/myshipments.aspx?batchID={batch.BatchID}");
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }
        protected void btnPrintPackingSlips_ServerClick(object sender, EventArgs e)
        {
            User currentUser = AppSession.Current.CurrentUser;
            try
            {
                List<int> shipmentIDs = getSelecteShipments();
                Session["shipmentIDs"] = shipmentIDs;

                printBatchReport.Attributes.Add("src", $@"/shipping/request/packing-slip-batch-print.ashx");
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }
        protected void dgvShipments_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvShipments.PageIndex = e.NewPageIndex;
            LoadGrid();
            dgvShipments.DataBind();
        }
    }
}