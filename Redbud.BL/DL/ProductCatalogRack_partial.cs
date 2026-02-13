using Redbud.BL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Redbud.BL.DL
{
    public class RackProductResponse
    {
        public int RackID { get; set; }
        public int ParentRackType { get; set; }
        public int ProductID { get; set; }
        public double DefaultQuantity { get; set; }
        public string ItemNumber { get; set; }
        public string ProductName { get; set; }
        public string Size { get; set; }
        public int ItemsPerPackage { get; set; }
        public int PackagesPerUnit { get; set; }
        public double UnitPrice { get; set; }
        public double TotalCost
        {
            get
            {
                return OrderHelper.GetTotalPrice(UnitPrice, RackDiscount, DefaultQuantity);
            }
        }
        public int PhotoCount { get; set; }
        public string UPCCode { get; set; }
        public double RackDiscount { get; set; }
        /// <summary>
        /// Base (without discount) price of each package
        /// </summary>
        public double EachBasePrice
        {
            get
            {
                return OrderHelper.CalculateEachBasePrice(UnitPrice, PackagesPerUnit);
            }
        }
        public double EachPrice
        {
            get
            {
                return OrderHelper.CalculateEachPrice(UnitPrice, RackDiscount, PackagesPerUnit);
            }
        }
        public DateTime? ShipDate { get; set; }
        public DateTime? OrderDeadline { get; set; }

        public string ItemPhoto { get; set; }
        public List<RackProductPhotoResponse> Photos { get; set; }
    }
    public class RackProductPhotoResponse
    {
        public string PhotoPath { get; set; }
        public string ProductName { get; set; }
        public int ProductId { get; set; }
        public int PhotoId { get; set; }
    }

    public class RackPhotoResponse
    {
        public string PhotoPath { get; set; }
        public string RackName { get; set; }
        public int RackId { get; set; }
        public int PhotoId { get; set; }
    }

    public class RackProductRackResponse
    {
        public int ParentRackId { get; set; }
        public int ParentRackType { get; set; }
        public int RackId { get; set; }
        public int RackType { get; set; }
        public double DefaultQuantity { get; set; }
        public string RackName { get; set; }
        public double? Discount { get; set; }
        public int? MinQuantity { get; set; }
        public int? MaxQuantity { get; set; }
        public int PhotoCount { get; set; }
        public string RackPhoto { get; set; }
        /// <summary>
        /// Base (without discount) price of the rack
        /// </summary>
        public double RackBasePrice
        {
            get
            {
                return Products.Sum(p => OrderHelper.GetTotalBasePrice(p.UnitPrice, p.DefaultQuantity));
            }
        }
        public double RackPrice
        {
            get
            {
                return Products.Sum(p => OrderHelper.GetTotalPrice(p.UnitPrice, p.RackDiscount, p.DefaultQuantity));
            }
        }

        public List<RackPhotoResponse> Photos { get; set; }

        public List<RackProductResponse> Products { get; set; } = new List<RackProductResponse>();
    }

    public partial class ProductCatalogRack
    {
        public string RackNameHTML
        {
            get
            {
                return RackName.Replace("\"", "&quot");
            }

        }
        public double RackTotal
        {
            get
            {
                double total = 0;
                foreach (var item in this.AllProducts())
                {

                    total += item.TotalCost;
                }
                return total;
            }
        }
        public string CatalogName
        {
            get
            {
                return this.ProductCatalog.CatalogName;
            }
        }
        public string DisplayPhotoPath
        {
            get
            {
                var photo = Photos.FirstOrDefault(p => p.PhotoPath.Length > 0);
                if (photo == null)
                {
                    return "./img/rack-not-available.jpg";
                }
                else
                {
                    return photo.PhotoPath;
                }
            }
        }

        public List<RackProductResponse> AllProducts(bool excludeZeroQuantity = false)
        {
            using (var db = new MadduxEntities())
            {
                var qry = from rp in db.RackProducts
                          join r in db.ProductCatalogRacks on rp.RackID equals r.RackID
                          join p in db.Products on rp.ProductID equals p.ProductId
                          join sc in db.supProductSubCategories on p.ProductSubCategoryId equals sc.SubCategoryID
                          where rp.RackID == RackID
                          select new RackProductResponse
                          {
                              ProductID = rp.ProductID,
                              ParentRackType = r.RackType,
                              DefaultQuantity = rp.DefaultQuantity,
                              ItemNumber = p.ItemNumber,
                              ProductName = p.ProductName,
                              Size = p.Size,
                              ItemsPerPackage = p.ItemsPerPackage,
                              PackagesPerUnit = p.PackagesPerUnit,
                              UnitPrice = p.UnitPrice,
                              PhotoCount = p.Photos.Count,
                              RackDiscount = r.Discount.HasValue ? r.Discount.Value : 0,
                              UPCCode = p.UPCCode,
                              ShipDate = rp.ProductCatalogRack.ProductRackShipDates.Any() ? rp.ProductCatalogRack.ProductRackShipDates.FirstOrDefault().ShipDate : (DateTime?)null,
                              OrderDeadline = rp.ProductCatalogRack.ProductRackShipDates.Any() ? rp.ProductCatalogRack.ProductRackShipDates.FirstOrDefault().OrderDeadlineDate : (DateTime?)null,
                              ItemPhoto = p.Photos.FirstOrDefault() != null ? p.Photos.FirstOrDefault().PhotoPath : null,
                              Photos = p.Photos.ToList().Select(ph => new RackProductPhotoResponse
                              {
                                  PhotoId = ph.PhotoID,
                                  PhotoPath = ph.PhotoPath,
                                  ProductName = p.ProductName,
                                  ProductId = p.ProductId
                              }).OrderBy(x => x.PhotoId).Skip(1).ToList()
                          };
                if (excludeZeroQuantity)
                {
                    qry = qry.Where(r => r.DefaultQuantity != 0);
                }

                return qry.ToList();
            }
        }

        public List<RackProductRackResponse> AllProductRacks(bool excludeZeroQuantity = false, bool excludeZeroQuantityProducts = false)
        {
            using (var db = new MadduxEntities())
            {
                IQueryable<RackProductRackResponse> qry = db.RackRacks
                    .Where(rr => rr.RackID == RackID)
                    .Select(rr => new RackProductRackResponse
                    {
                        ParentRackId = rr.ParentRack.RackID,
                        RackType = rr.ParentRack.RackType,
                        RackId = rr.ProductRackID,
                        DefaultQuantity = rr.DefaultQuantity,
                        RackName = rr.ProductRack.RackName,
                        Discount = rr.ParentRack.Discount,
                        MinQuantity = rr.ParentRack.MinimumItems,
                        MaxQuantity = rr.ParentRack.MaximumItems,
                        PhotoCount = rr.ProductRack.Photos.Count(),
                        RackPhoto = rr.ProductRack.Photos.Any() ? rr.ProductRack.Photos.Select(p => p.PhotoPath).FirstOrDefault() : null,
                        Photos = rr.ProductRack.Photos.ToList().Select(ph => new RackPhotoResponse
                        {
                            PhotoId = ph.PhotoID,
                            PhotoPath = ph.PhotoPath,
                            RackName = rr.ProductRack.RackName,
                            RackId = rr.ProductRack.RackID
                        }).OrderBy(x => x.PhotoId).Skip(1).ToList(),
                        Products = rr.ProductRack.RackProducts.Select(rp => new RackProductResponse
                        {
                            ProductID = rp.ProductID,
                            RackID = rr.ProductRackID,
                            ParentRackType = rr.ParentRack.RackType,
                            DefaultQuantity = rp.DefaultQuantity,
                            ItemNumber = rp.Product.ItemNumber,
                            ProductName = rp.Product.ProductName,
                            Size = rp.Product.Size,
                            ItemsPerPackage = rp.Product.ItemsPerPackage,
                            PackagesPerUnit = rp.Product.PackagesPerUnit,
                            UnitPrice = rp.Product.UnitPrice,
                            PhotoCount = rp.Product.Photos.Count,
                            RackDiscount = rr.ParentRack.Discount.HasValue ? rr.ParentRack.Discount.Value : 0,
                            UPCCode = rp.Product.UPCCode,
                            ShipDate = rp.ProductCatalogRack.ProductRackShipDates.Any() ? rp.ProductCatalogRack.ProductRackShipDates.FirstOrDefault().ShipDate : (DateTime?)null,
                            OrderDeadline = rp.ProductCatalogRack.ProductRackShipDates.Any() ? rp.ProductCatalogRack.ProductRackShipDates.FirstOrDefault().OrderDeadlineDate : (DateTime?)null,
                            ItemPhoto = rp.Product.Photos.FirstOrDefault() != null ? rp.Product.Photos.FirstOrDefault().PhotoPath : null,
                            Photos = rp.Product.Photos.ToList().Select(ph => new RackProductPhotoResponse
                            {
                                PhotoId = ph.PhotoID,
                                PhotoPath = ph.PhotoPath,
                                ProductName = rp.Product.ProductName,
                                ProductId = rp.Product.ProductId
                            }).OrderBy(x => x.PhotoId).Skip(1).ToList()
                        }).OrderBy(rp => rp.Size).ToList()
                    })
                    .OrderBy(rr => rr.RackName);

                if (excludeZeroQuantity)
                {
                    qry = qry.Where(rp => rp.DefaultQuantity != 0);
                }

                return qry.ToList();
            }
        }
    }
}
