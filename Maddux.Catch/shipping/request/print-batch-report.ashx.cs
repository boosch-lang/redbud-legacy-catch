using Newtonsoft.Json;
using Redbud.BL.DL;
using Redbud.BL.Resources;
using RestSharp;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;

namespace Maddux.Catch.shipping.request
{
    /// <summary>
    /// Summary description for print_batch_report
    /// </summary>
    public class print_batch_report : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {

                int BatchID = int.Parse(context.Request.QueryString["batchID"]);

                using (MadduxEntities madduxEntities = new MadduxEntities())
                {
                    InvoicePostBatch invoicePostBatch = madduxEntities.InvoicePostBatches.Include(pb => pb.InvoicePostBatchItems).FirstOrDefault(pb => pb.BatchID == BatchID);
                    if (invoicePostBatch != null)
                    {
                        List<InvoicePostBatchItem> batchItems = invoicePostBatch.InvoicePostBatchItems.Where(bi => bi.BatchID == BatchID && bi.InvoicePrinted == false).ToList();
                        if (batchItems != null && batchItems.Count > 0)
                        {
                            string html = string.Empty;
                            foreach (var item in batchItems)
                            {
                                using (var writer = new StringWriter())
                                {
                                    context.Server.Execute($"/shipping/invoice.aspx?id={item.OrderID}&sID={item.ShipmentID}&view=print", writer);
                                    html += writer.GetStringBuilder().ToString();
                                }
                                item.InvoicePrinted = true;

                            }
                            madduxEntities.SaveChanges();
                            //print report
                            using (var writer = new StringWriter())
                            {
                                context.Server.Execute($"~/shipping/batch-report.aspx?id={BatchID}&view=print", writer);
                                html += writer.GetStringBuilder().ToString();

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

                                context.Response.ContentType = EmailerResources.PDFContentType;
                                context.Response.AddHeader("Content-Disposition", $"attachment; filename=invoices-with-batch-report-{BatchID}-{invoicePostBatch.BatchDate:dd-MMM-hh:mm tt}.pdf");
                                context.Response.OutputStream.Write(pdfResponse.RawBytes, 0, pdfResponse.RawBytes.Length);
                            }
                        }
                    }

                }

            }
            catch
            {

                throw;
            }

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}