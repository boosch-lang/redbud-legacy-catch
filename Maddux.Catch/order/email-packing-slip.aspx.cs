using Newtonsoft.Json;
using Redbud.BL.DL;
using Redbud.BL.Resources;
using Redbud.BL.Utils;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Maddux.Catch.order
{
    public partial class email_packing_slip : System.Web.UI.Page
    {
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
                if (ViewState["CustomerID"] == null)
                {
                    ViewState["CustomerID"] = Request.QueryString["CustomerId"] == null || Request.QueryString["CustomerId"] == "" ? 0 : (object)Request.QueryString["CustomerId"];
                }
                return Convert.ToInt32(ViewState["CustomerID"].ToString());
            }

            set
            {
                ViewState["CustomerID"] = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            using (var db = new MadduxEntities())
            {
                Shipment shipment = db.Shipments.FirstOrDefault(s => s.ShipmentID == ShipmentID);
                Customer customer = db.Customers.FirstOrDefault(c => c.CustomerId == CustomerID);
                PrintPackingSlip(shipment, customer);
            }
        }
        private bool PrintPackingSlip(Shipment shipment, Customer customer)
        {
            try
            {
                using (var writer = new StringWriter())
                {

                    if (shipment == null)
                        return false;
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

                    Emailer emailer = new Emailer();
                    if (!string.IsNullOrWhiteSpace(customer.Email))
                    {
                        string body = string.Format(EmailerResources.PackingSlipEmailBody, customer.Company, ShipmentID) + "<br>" + EmailerResources.EmailFooter;
                        Dictionary<int, string> recipients = new Dictionary<int, string>
                        {
                            { CustomerID, customer.Email }
                        };
                        var cc = new List<string>();
                        if (customer.AlternateEmailReceivesConfirmations && !string.IsNullOrEmpty(customer.AlternateEmail)) {
                            cc.Add(customer.AlternateEmail);
                        }
                        bool emailSent = emailer.SendEmailWithAttachment(
                                                           recipients,
                                                           string.Format(EmailerResources.PackingSlipEmailSubject, ShipmentID),
                                                           body,
                                                           pdfResponse.RawBytes,
                                                           $"packing-slip-{ShipmentID}.pdf",
                                                           EmailerResources.PDFContentType,
                                                           cc
                                                        );
                    }

                    Response.Clear();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", $"attachment; filename=packingslip#{shipment.ShipmentID}-{DateTime.Now:yyyy-MM-dd}.pdf");
                    Response.OutputStream.Write(pdfResponse.RawBytes, 0, pdfResponse.RawBytes.Length);
                    Response.Flush();
                    Response.End();
                    return true;
                }
            }
            catch
            {
                return false;
            }

        }
    }
}