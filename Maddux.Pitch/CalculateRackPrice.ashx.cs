using Redbud.BL;
using Redbud.BL.DL;
using Redbud.BL.Helpers;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Maddux.Pitch
{
    /// <summary>
    /// Summary description for CalculateRackPrice
    /// </summary>
    public class CalculateRackPrice : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {

                //https://stackoverflow.com/questions/19809791/send-json-array-to-ashx-handler
                var data = context.Request;
                var sr = new StreamReader(data.InputStream);
                var stream = sr.ReadToEnd();
                var javaScriptSerializer = new JavaScriptSerializer();
                var overviewPost = javaScriptSerializer.Deserialize<OverviewPost>(stream);

                int RackID = overviewPost.RackID;
                double totalPrice = 0;
                double totalUnits = 0;
                double totalCount = 0;
                int percentage = 0;
                int maximumItems = 0;
                int minimumItems = 0;
                string rackType = "";
                using (var db = new MadduxEntities())
                {
                    ProductCatalogRack theRack = db.ProductCatalogRacks.FirstOrDefault(r => r.RackID == RackID);
                    maximumItems = (int)theRack.MaximumItems;
                    minimumItems = (int)theRack.MinimumItems;

                    if (theRack.RackType == (int)RackType.Standard)
                    {
                        rackType = "standard";
                        foreach (var item in overviewPost.Products)
                        {
                            int productId = item.Id;
                            int quantity = item.Quantity;
                            double productUnitPrice = theRack.RackProducts.Where(rp => rp.ProductID == item.Id).Select(x => x.UnitPrice).FirstOrDefault();
                            double productTotalPrice = productUnitPrice * item.Quantity;
                            double productUnits = theRack.RackProducts.Where(rp => rp.ProductID == item.Id && !string.IsNullOrEmpty(rp.Product.ProductCode)).Select(x => x.UnitPerCase * quantity).Sum();
                            totalPrice += productTotalPrice;
                            totalCount += item.Quantity;
                            totalUnits += productUnits;
                        }
                    } else
                    {
                        rackType = "bulk";
                        foreach(var item in overviewPost.Products)
                        {
                            int rackId = item.Id;
                            int quantity = item.Quantity;
                            double discount = theRack.Discount.HasValue ? theRack.Discount.Value : 0;


                            var productUnits = theRack.RackRacks
                                .Where(rp => rp.ProductRackID == rackId)
                                .SelectMany(rp => 
                                    rp.ProductRack.RackProducts
                                    .Where(p => p.DefaultQuantity > 0)
                                    .Select(x => x.UnitPerCase * quantity))
                                .Sum();
                            var productPrice = theRack.RackRacks.Where(rp => rp.ProductRackID == rackId).SelectMany(rp => rp.ProductRack.RackProducts.Where(p => p.DefaultQuantity > 0)).Sum(p => OrderHelper.GetTotalPrice(p.UnitPrice, discount, p.DefaultQuantity));
                            var productTotalPrice = productPrice * quantity;

                            //get the price of the rack(with discount) * quantity
                            totalPrice += productTotalPrice;
                            totalCount += item.Quantity;
                            totalUnits += productUnits;
                        }
                    }
                }
                percentage = Convert.ToInt32(totalCount / maximumItems * 100);
                var result = "{\"price\": " + totalPrice + ", \"percentage\": " + percentage + ", \"count\": " + totalCount + ", \"min\": " + minimumItems + ", \"max\": " + maximumItems + ", \"units\": " + totalUnits + ", \"rackType\": \"" + rackType + "\" }";

                context.Response.ContentType = "application/json";
                context.Response.Write(javaScriptSerializer.Serialize(result));
            }
            catch (Exception ex)
            {
                context.Response.Write(ex.Message);
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