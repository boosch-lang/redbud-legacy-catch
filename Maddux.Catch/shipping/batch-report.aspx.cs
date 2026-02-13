using Redbud.BL.DL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.UI.WebControls;

namespace Maddux.Catch.shipping
{
    public class BatchReportItem
    {
        public int InvNo { get; set; }
        public bool Emailed { get; set; }
        public int Copies { get; set; }
        public string Email { get; set; }
        public string CustomerName { get; set; }
        public DateTime? InvDate { get; set; }
        public bool Printed { get; set; }
        public decimal Total { get; set; }
    }
    public partial class batch_report : System.Web.UI.Page
    {

        private int BatchID
        {
            get
            {
                if (ViewState["BatchID"] == null)
                {
                    ViewState["BatchID"] = Request.QueryString["id"] == null || Request.QueryString["id"] == "" ? -1 : (object)Request.QueryString["id"];
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
            using (var db = new MadduxEntities())
            {
                var batch = db.InvoicePostBatches.Include(x => x.InvoicePostBatchItems).FirstOrDefault(r => r.BatchID == BatchID);
                if (batch != null)
                {
                    List<BatchReportItem> reportItems = new List<BatchReportItem>();
                    lblBatchDate.InnerText = batch.BatchDate.ToString("MMM dd, yyy hh:mm tt");
                    lblBatchNumber.InnerText = batch.BatchID.ToString();
                    foreach (var batchItem in batch.InvoicePostBatchItems)
                    {
                        var shipment = db.Shipments.FirstOrDefault(x => x.ShipmentID == batchItem.ShipmentID);

                        BatchReportItem item = new BatchReportItem
                        {
                            InvNo = shipment.ShipmentID,
                            InvDate = shipment.DateShipped,
                            Printed = batchItem.InvoicePrinted,
                            Emailed = batchItem.InvoiceEmailed,
                            Copies = batchItem.InvoiceCopies,
                            Email = batchItem.EmailAddress,
                            Total = batchItem.InvoiceTotal
                        };


                        var order = db.Orders.Include(x => x.Customer).Select(x => new { x.OrderID, CustomerName = x.Customer.Company }).FirstOrDefault(x => x.OrderID == shipment.OrderID);

                        item.CustomerName = order.CustomerName;
                        reportItems.Add(item);
                    }
                    dgvBatchReport.DataSource = reportItems;
                    dgvBatchReport.DataBind();
                }
            }
        }

        protected void dgvBatchReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
    }
}