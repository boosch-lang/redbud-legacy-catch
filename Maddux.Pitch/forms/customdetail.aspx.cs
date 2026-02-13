using Newtonsoft.Json;
using Redbud.BL;
using Redbud.BL.DL;
using Redbud.BL.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Maddux.Pitch.forms
{
    public partial class customdetail : System.Web.UI.Page
    {
        private int RackID
        {
            get
            {
                if (ViewState["RackID"] == null)
                {
                    if (Request.QueryString["rackid"] == null || Request.QueryString["rackid"] == "")
                        ViewState["RackID"] = -1;
                    else
                        ViewState["RackID"] = Request.QueryString["rackid"];
                }
                return Convert.ToInt32(ViewState["RackID"].ToString());
            }

            set
            {
                ViewState["RackID"] = value;
            }
        }

        private List<RackProductRackResponse> RackProductRackResponses { get; set; } = new List<RackProductRackResponse>();

        public class SelectedProduct
        {
            [JsonProperty("id", Required = Required.Always)]
            public int ProductId { get; set; }

            [JsonProperty("q", Required = Required.Always)]
            public int Quantity { get; set; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (RackID > 0)
                {
                    using (MadduxEntities db = new MadduxEntities())
                    {
                        ProductCatalogRack productCatalogRack = db.ProductCatalogRacks.FirstOrDefault(r => r.RackID == RackID);
                        ProductCatalog productCatalog = productCatalogRack.ProductCatalog;

                        //If we can't find the rack in the database, then diplay a message
                        if (productCatalogRack != null)
                        {
                            if (!Page.IsPostBack)
                            {
                                var json = Request.QueryString["json"];
                                var selectedProducts = JsonConvert.DeserializeObject<SelectedProduct[]>(json);


                                //Standard Rack
                                if (productCatalogRack.RackType == (int)RackType.Standard)
                                {

                                    var allRackProducts = productCatalogRack.AllProducts(false);

                                    var products = new List<RackProductResponse>();

                                    foreach (var p in selectedProducts)
                                    {
                                        var t = allRackProducts.Where(x => x.ProductID == p.ProductId).FirstOrDefault();
                                        if (t != null)
                                        {
                                            //this is just for display purposes, set the default quantity to the quantity selected
                                            //to display on the customize details.
                                            t.DefaultQuantity = p.Quantity;
                                            products.Add(t);
                                        }
                                    }

                                    RackProductRackResponses = new List<RackProductRackResponse>() {
                                        new RackProductRackResponse
                                        {
                                            RackId = productCatalogRack.RackID,
                                            RackName = productCatalogRack.RackName,
                                            DefaultQuantity = 1,
                                            MinQuantity = productCatalogRack.MinimumItems,
                                            MaxQuantity = productCatalogRack.MaximumItems,
                                            Discount = productCatalogRack.Discount.HasValue ? productCatalogRack.Discount.Value : 0,
                                            RackType = (int)RackType.Standard,
                                            Products = products.OrderBy(x => x.Size).ToList()
                                        }
                                    };
                                }
                                else
                                {
                                    //Bulk Rack
                                    var rackProducts = productCatalogRack.AllProductRacks(false);
                                    var products = new List<RackProductRackResponse>();

                                    foreach (var r in selectedProducts)
                                    {

                                        var t = rackProducts.Where(x => x.RackId == r.ProductId).FirstOrDefault();
                                        if (t != null)
                                        {
                                            t.DefaultQuantity = r.Quantity;
                                            products.Add(t);
                                        }
                                    }

                                    RackProductRackResponses = products;

                                }

                                var itemType = productCatalogRack.RackType == (int)RackType.Standard ? "tray" : "Smart Stack";
                                CatalogRackModel model = new CatalogRackModel
                                {
                                    RackType = productCatalogRack.RackType,
                                    RackID = productCatalogRack.RackID,
                                    CatalogID = productCatalog.CatalogId,
                                    ProgramID = productCatalog.ProgramID,
                                    ProgramName = productCatalog.ProductProgram.ProgramName,
                                    RackDescription = productCatalogRack.RackName,
                                    RackImage = productCatalogRack.Photos.Any() ? productCatalogRack.Photos.Select(p => p.PhotoPath).First() : "/img/rack-not-available.jpg",
                                    RackImages = productCatalogRack.Photos.Select(p => new CatalogRackImage
                                    {
                                        PhotoId = p.PhotoID,
                                        PhotoPath = p.PhotoPath,
                                        RackID = productCatalogRack.RackID,
                                        RackName = productCatalogRack.RackName
                                    }).Skip(1).ToList(),
                                    TrayCount = $"{productCatalogRack.MaximumItems} tray(s)",
                                    AllowCustomization = productCatalogRack.AllowCustomization,
                                    ShipDates = new List<RackDate>(),
                                    ProductCatalogDescription = productCatalogRack.RackDesc,
                                    MinimumItems = productCatalogRack.MinimumItems,
                                    ItemType = productCatalogRack.MinimumItems > 1 ? $"{itemType}s" : itemType,
                                    Discount = productCatalogRack.Discount != null ? ((productCatalogRack.Discount ?? 0) * 100) : 0
                                };

                                lvRacks.DataSource = new List<CatalogRackModel>() { model };
                                lvRacks.DataBind();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                litError.InnerText = StringTools.GenerateError(ex.Message);
            }
        }

        protected void lvRacks_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                using (MadduxEntities db = new MadduxEntities())
                {
                    if (e.Item.ItemType == ListViewItemType.DataItem)
                    {
                        Repeater rptProductRacks = e.Item.FindControl("rptProductRacks") as Repeater;

                        rptProductRacks.DataSource = RackProductRackResponses;
                        rptProductRacks.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        public string CalculateRackQty(int caseCount, double defaultQuantity)
        {
            return ((int)Math.Round(caseCount * defaultQuantity)).ToString();
        }

    }
}