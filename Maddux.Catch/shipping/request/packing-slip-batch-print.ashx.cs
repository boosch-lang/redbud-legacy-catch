using Newtonsoft.Json;
using Redbud.BL.DL;
using Redbud.BL.Resources;
using RestSharp;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Maddux.Catch.shipping.request
{
    /// <summary>
    /// Summary description for packing_slip_batch_print
    /// </summary>
    public class packing_slip_batch_print : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            List<int> shipmentIDs = context.Session["shipmentIDs"] as List<int>;
            if (shipmentIDs.Count > 0)
            {
                using (MadduxEntities madduxEntities = new MadduxEntities())
                {
                    var batchItems = madduxEntities.Shipments.Where(x => shipmentIDs.Contains(x.ShipmentID)).ToList();
                    string html = string.Empty;
                    foreach (var item in batchItems)
                    {
                        using (var writer = new StringWriter())
                        {
                            context.Server.Execute($"~/order/packing-slip.aspx?id={item.OrderID}&sId={item.ShipmentID}&view=print", writer);
                            html += writer.GetStringBuilder().ToString();
                        }
                    }
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
                    context.Response.AddHeader("Content-Disposition", $"attachment; filename=Packing%20Slips.pdf");
                    context.Response.OutputStream.Write(pdfResponse.RawBytes, 0, pdfResponse.RawBytes.Length);
                    context.Session["shipmentIDs"] = null;
                }
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