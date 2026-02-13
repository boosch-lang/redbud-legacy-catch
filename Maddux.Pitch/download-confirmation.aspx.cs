using Newtonsoft.Json;
using Redbud.BL.Utils;
using RestSharp;
using System;
using System.IO;

namespace Maddux.Pitch
{
    public partial class download_confirmation : System.Web.UI.Page
    {
        private int OrderID
        {
            get
            {
                if (ViewState["OrderID"] == null)
                {
                    if (Request.QueryString["id"] == null || Request.QueryString["id"] == "")
                        ViewState["OrderID"] = -1;
                    else
                        ViewState["OrderID"] = Request.QueryString["id"];
                }
                return Convert.ToInt32(ViewState["OrderID"].ToString());
            }

            set
            {
                ViewState["OrderID"] = value;
            }
        }
        private int ShipmentID
        {
            get
            {
                if (ViewState["ShipmentID"] == null)
                {
                    if (Request.QueryString["sID"] == null || Request.QueryString["sID"] == "")
                        ViewState["ShipmentID"] = -1;
                    else
                        ViewState["ShipmentID"] = Request.QueryString["sID"];
                }
                return Convert.ToInt32(ViewState["ShipmentID"].ToString());
            }

            set
            {
                ViewState["ShipmentID"] = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                using (var writer = new StringWriter())
                {
                    if (ShipmentID == -1)
                    {
                        Server.Execute($"/OrderConfirmation.aspx?id={OrderID}&view=print", writer);
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
                        Response.AddHeader("Content-Disposition", $"attachment; filename=confirmation-{OrderID}.pdf");
                        Response.OutputStream.Write(pdfResponse.RawBytes, 0, pdfResponse.RawBytes.Length);
                        Response.End();
                    }
                    else
                    {
                        Server.Execute($"/invoice.aspx?id={OrderID}&sID={ShipmentID}&view=print", writer);
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
                        Response.AddHeader("Content-Disposition", $"attachment; filename=invoice-{OrderID}.pdf");
                        Response.OutputStream.Write(pdfResponse.RawBytes, 0, pdfResponse.RawBytes.Length);
                        Response.End();
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