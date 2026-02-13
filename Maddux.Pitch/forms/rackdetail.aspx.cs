using Maddux.Pitch.LocalClasses;
using Redbud.BL;
using Redbud.BL.DL;
using Redbud.BL.Helpers;
using Redbud.BL.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Maddux.Pitch.forms
{
    public partial class rackdetail : System.Web.UI.Page
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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                using (MadduxEntities db = new MadduxEntities())
                {
                    ProductCatalogRack theRack = db.ProductCatalogRacks.FirstOrDefault(r => r.RackID == RackID);

                    //If we can't find the rack in the database, then diplay a message
                    if (RackID == -1 || theRack?.ProductCatalog == null)
                    {
                        //show a message
                        litError.InnerHtml = StringTools.GenerateError("Rack not found");
                    }
                    else
                    {
                        ProductCatalog catalog = theRack.ProductCatalog;

                        if (!Page.IsPostBack)
                        {
                            var itemType = theRack.RackType == (int)RackType.Standard ? "tray" : "Smart Stack";
                            CatalogRackModel model = new CatalogRackModel
                            {
                                RackType = theRack.RackType,
                                RackID = theRack.RackID,
                                CatalogID = catalog.CatalogId,
                                ProgramID = catalog.ProgramID,
                                ProgramName = catalog.ProductProgram.ProgramName,
                                RackDescription = theRack.RackName,
                                RackImage = theRack.Photos.Any() ? theRack.Photos.Select(p => p.PhotoPath).First() : "/img/rack-not-available.jpg",
                                RackImages = theRack.Photos.Select(p => new CatalogRackImage
                                {
                                    PhotoId = p.PhotoID,
                                    PhotoPath = p.PhotoPath,
                                    RackID = theRack.RackID,
                                    RackName = theRack.RackName
                                }).Skip(1).ToList(),
                                TrayCount = $"{theRack.MaximumItems} tray(s)",
                                AllowCustomization = theRack.AllowCustomization,
                                ShipDates = new List<RackDate>(),
                                ProductCatalogDescription = theRack.RackDesc,
                                MinimumItems = theRack.MinimumItems,
                                ItemType = theRack.MinimumItems > 1 ? $"{itemType}s" : itemType,
                                Discount = theRack.Discount != null ? ((theRack.Discount ?? 0) * 100) : 0
                            };

                            var rackDiscount = theRack.Discount.HasValue ? theRack.Discount.Value : 0;

                            if (theRack.RackType == (int)RackType.Standard)
                            {
                                //add unit price
                                model.Price = theRack.RackProducts
                                    .Where(p => p.DefaultQuantity > 0)
                                    .Sum(p => OrderHelper.GetTotalPrice(p.UnitPrice, rackDiscount, p.DefaultQuantity));

                                model.UnitCount = theRack.RackProducts
                                    .Where(x => x.DefaultQuantity > 0 && !string.IsNullOrEmpty(x.Product.ProductCode))
                                    .Sum(x => x.DefaultQuantity * x.UnitPerCase);
                            }
                            else
                            {
                                var products = theRack.RackRacks
                                    .Where(r => r.DefaultQuantity > 0)
                                    .Select(r => new
                                    {
                                        Price = r.ProductRack.RackProducts
                                                    .Where(x => x.DefaultQuantity > 0)
                                                    .Sum(x => OrderHelper.GetTotalPrice(x.UnitPrice, rackDiscount, r.DefaultQuantity * x.DefaultQuantity)),
                                        UnitCount = r.ProductRack.RackProducts
                                                    .Where(x => x.DefaultQuantity > 0)
                                                    .Sum(x => r.DefaultQuantity * x.DefaultQuantity * x.UnitPerCase)

                                    });

                                model.Price = products.Sum(x => x.Price);
                                model.UnitCount = products.Sum(x => x.UnitCount);
                            }

                            lvRacks.DataSource = new List<CatalogRackModel>() { model };
                            lvRacks.DataBind();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                litError.InnerHtml = StringTools.GenerateError(ex.Message);
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

                        var itemRow = (CatalogRackModel)e.Item.DataItem;


                        Customer theCustomer = AppSession.Current.CurrentCustomer;
                        ProductCatalogRack theRack = db.ProductCatalogRacks.Include(x => x.RackProducts.Select(z => z.Product.Photos)).FirstOrDefault(r => r.RackID == itemRow.RackID);

                        List<RackProductRackResponse> customizeData = new List<RackProductRackResponse>();

                        if (theRack.RackType == (int)RackType.Standard)
                        {
                            //standard rack
                            customizeData.Add(new RackProductRackResponse
                            {
                                RackId = theRack.RackID,
                                RackName = theRack.RackName,
                                DefaultQuantity = 1,
                                MinQuantity = theRack.MinimumItems,
                                MaxQuantity = theRack.MaximumItems,
                                Discount = theRack.Discount.HasValue ? theRack.Discount.Value : 0,
                                RackType = theRack.RackType,
                                Products = theRack.RackProducts.Where(rp => rp.DefaultQuantity > 0).Select(rp => new RackProductResponse
                                {
                                    ProductID = rp.ProductID,
                                    RackID = theRack.RackID,
                                    ParentRackType = theRack.RackType,
                                    DefaultQuantity = rp.DefaultQuantity,
                                    ItemNumber = rp.Product.ItemNumber,
                                    ProductName = rp.ProductName,
                                    Size = rp.Product.Size,
                                    ItemsPerPackage = rp.Product.ItemsPerPackage,
                                    PackagesPerUnit = rp.Product.PackagesPerUnit,
                                    UnitPrice = rp.UnitPrice,
                                    PhotoCount = rp.Product.Photos.Count,
                                    RackDiscount = theRack.Discount.HasValue ? theRack.Discount.Value : 0,
                                    ItemPhoto = rp.Product.Photos.Select(x => x.PhotoPath).FirstOrDefault(),
                                    Photos = rp.Product.Photos.Select(p => new RackProductPhotoResponse
                                    {
                                        PhotoId = p.PhotoID,
                                        PhotoPath = p.PhotoPath,
                                        ProductId = rp.ProductID,
                                        ProductName = rp.ProductName
                                    }).Skip(1).ToList()
                                }).OrderBy(rp => rp.Size).ToList(),

                            });
                        }
                        else
                        {
                            customizeData = theRack.AllProductRacks(true);
                        }
                        rptProductRacks.DataSource = customizeData;
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