using Redbud.BL.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Redbud.BL.DL
{
    public partial class Customer
    {
        public List<Customer> GetSubCustomers()
        {
            using (var db = new MadduxEntities())
            {
                var subs = db.CustomersSubs.Where(r => r.MasterCustomerID == CustomerId).Select(r => r.ChildCustomerID);

                var subCustomers = db.Customers.Where(r => subs.Contains(r.CustomerId)).OrderBy(x => x.Company);

                return subCustomers.ToList();
            }
        }

        public string Contact
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        public string CityState
        {
            get
            {
                return City + ", " + State;
            }
        }

        public int JournalID
        {
            get
            {
                if (this.Journals.Any())
                {
                    return this.Journals.OrderByDescending(r => r.CreateDate).First().JournalID;
                }
                else
                {
                    return 0;
                }
            }
        }
        public DateTime? LastOrderDate
        {
            get
            {
                if (this.Orders.Any())
                {
                    return this.Orders.OrderByDescending(r => r.OrderDate).First().OrderDate;
                }
                else
                {
                    return null;
                }
            }
        }
        public int LastOrderID
        {
            get
            {
                if (this.Orders.Any())
                {
                    return this.Orders.OrderByDescending(r => r.OrderDate).First().OrderID;
                }
                else
                {
                    return 0;
                }
            }
        }

        public int? Rating
        {
            get
            {
                //int rating = 0;
                //DateTime cutoffDate = DateTime.Today.AddYears(-1);
                //DateTime threeYearCutOffDate = DateTime.Today.AddYears(-3);
                //if (this.Orders.Any(x => x.OrderDate >= threeYearCutOffDate))
                //{
                //    double totalOrder = this.Orders.Where(o => o.OrderDate >= cutoffDate).Sum(o => o.SubTotal);
                //    if (totalOrder < 1500)
                //        rating = 1;
                //    else if (totalOrder < 3999)
                //        rating = 2;
                //    else if (totalOrder < 7999)
                //        rating = 3;
                //    else if (totalOrder < 14999)
                //        rating = 4;
                //    else
                //        rating = 5;
                //}
                //else
                //{
                //    rating = 0;
                //}
                //return rating;
                return (int?)this.StarRating;
            }
        }
        public string StarRatingGraphic
        {
            get
            {
                string stars = string.Empty;
                if (StarRating.HasValue)
                {
                    for (int x = 0; x < StarRating.Value; x++)
                    {
                        stars += "<i class='fa fa-star'></i> ";
                    }
                    for (int x = 0; x < (5 - StarRating.Value); x++)
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
        }
        public bool LogActivity(string sessionID, string email, string page, string logDesc, string platForm, string browser, string userAgent, string bVersion, string hostAdd)
        {
            bool result = false;
            try
            {
                using (var db = new MadduxEntities())
                {


                    if (!Company.ToLower().Contains("redbud") && !Email.ToLower().Contains("redbud"))
                    {
                        var log = db.ActivityLogs.Create();
                        log.SessionId = sessionID;
                        log.CustomerEmail = email;
                        log.CustomerId = this.CustomerId;
                        log.ActivityDesc = logDesc;
                        log.BrowserVersion = bVersion;
                        log.IpAddress = hostAdd;
                        log.PageName = page;
                        log.Platform = platForm;
                        log.BrowserType = browser;
                        log.ActivityDate = DateTime.UtcNow;


                        db.ActivityLogs.Add(log);

                        //Log login activity as journal
                        var journal = db.Journals.Create();
                        journal.CustomerID = CustomerId; //Redbud Supply Inc.
                        journal.DateStamp = DateTime.Now;
                        journal.IsResolved = true;
                        journal.ResolvedDate = DateTime.Now;
                        journal.CreateDate = DateTime.Now;
                        journal.AssignedToId = 1;
                        journal.Notes = logDesc + " " + bVersion + " " + hostAdd + " " + page + " " + platForm + " " + browser;

                        db.Journals.Add(journal);
                    }

                    int count = db.SaveChanges();

                    if (count > 0)
                    {
                        result = true;
                    }
                    return result;
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }
        /// <summary>
        /// Retrieves list of product catalogs that the current user is associated with
        /// </summary>
        /// <returns> List of ProductCatalog</returns>
        public List<ProductCatalog> GetActiveProgramWithRacks(bool showAll = false)
        {
            using (MadduxEntities db = new MadduxEntities())
            {
                //Product catalogs that can be display to the user (only ones with ship dates)
                IQueryable<ProductCatalog> catalogs;

                var qry = db.ProductCatalogs
                    .AsNoTracking()
                    .Include(pc => pc.ProductProgram)
                    .Include(pc => pc.ProductCatalogShipDates)
                    .Include(pc => pc.ProductCatalogRacks.Select(r => r.ProductRackShipDates))
                    .Where(r => r.ProductCatalogShipDates.Any(d => DateTime.Now < d.OrderDeadlineDate));

                if (!showAll)
                {
                    qry = qry.Where(r => r.ShowOnMyAccount);
                }
                catalogs = qry;

                //Customer's associations
                IQueryable<int> associations = db.CustomerAsscs
                    .Where(r => r.CustomerID == this.CustomerId)
                    .Select(r => r.AssociationID);

                //Catalogs associated with the user
                IQueryable<int> assocCats = db.AssociationCatalogs
                    .Where(r => associations.Contains(r.AssociationID))
                    .Select(r => r.CatalogID);

                catalogs = catalogs.Where(r => assocCats.Contains(r.CatalogId));

                //To store the product catalogs after filtering
                List<ProductCatalog> productCatalogs = new List<ProductCatalog>();

                if (catalogs.Count() > 0)
                {
                    foreach (ProductCatalog catalog in catalogs)
                    {
                        if (catalog.States.Any())
                        {
                            foreach (var state in catalog.States)
                            {
                                //If the catalog state is equal to the user state add it to the list
                                if (state.StateID == this.State && catalog.ProductCatalogRacks.Count > 0)
                                {
                                    productCatalogs.Add(catalog);
                                }
                            }
                        }
                        else
                        {
                            //If the catalog does not have states assigned, display it to the user anyway
                            //productCatalogs.Add(catalog);
                        }
                    }
                }

                return productCatalogs;
            }
        }

        public List<CatalogResult> GetOrderPrograms()
        {
            using (var db = new MadduxEntities())
            {

                var qry = from so in db.vwCustomerShippedOrders
                          join oi in db.OrderItems on so.OrderID equals oi.OrderId
                          join p in db.Products on oi.ProductId equals p.ProductId
                          join pc in db.ProductCatalogs on p.CatalogId equals pc.CatalogId
                          where so.CustomerID == this.CustomerId
                          orderby pc.CatalogYear descending, pc.CatalogName
                          select new CatalogResult
                          {
                              CatalogID = pc.CatalogId,
                              CatalogName = pc.CustomerCatalogName.Length > 0 ? pc.CustomerCatalogName + " (" + pc.CatalogYear + ")" : pc.CatalogName,

                          };
                var unionQry = from so in db.vwCustomerShippedOrders
                               join cs in db.CustomersSubs on so.CustomerID equals cs.ChildCustomerID
                               join oi in db.OrderItems on so.OrderID equals oi.OrderId
                               join p in db.Products on oi.ProductId equals p.ProductId
                               join pc in db.ProductCatalogs on p.CatalogId equals pc.CatalogId
                               where so.CustomerID == this.CustomerId
                               orderby pc.CatalogYear descending, pc.CatalogName
                               select new CatalogResult
                               {
                                   CatalogID = pc.CatalogId,
                                   CatalogName = pc.CustomerCatalogName.Length > 0 ? pc.CustomerCatalogName + " (" + pc.CatalogYear + ")" : pc.CatalogName,

                               };
                var results = qry.ToList().Union(unionQry.ToList());
                return results.ToList();
            }
        }
        /// <summary>
        /// Order history for customer id
        /// </summary>
        /// <param name="startDate">The <see cref="DateTime"/> start date to filter by</param>
        /// <param name="endDate">The <see cref="DateTime"/> end date to filter by</param>
        /// <param name="programID">The program ID to filter records by </param>
        /// <returns>List of shipped orders</returns>
        public List<OrderHistory> GetOrderHistory(int programId)
        {
            using (MadduxEntities db = new MadduxEntities())
            {
                // Fetch orders and related data
                var orders = db.Orders
                    .Where(o =>
                        o.CustomerID == CustomerId &&
                        (programId == 1 || o.OrderItems.Any(oi => oi.Product.CatalogId == programId))
                    )
                    .Select(o => new
                    {
                        o.OrderID,
                        o.OrderDate,
                        o.QuoteDate,
                        OrderItems = o.OrderItems.Select(oi => new
                        {
                            oi.Product.ProductCatalog.CatalogName,
                            ShipmentItem = oi.ShipmentItems.FirstOrDefault()
                        }),
                        OrderRacks = o.OrderRacks.Select(or => new
                        {
                            or.RackId,
                            or.ProductCatalogRack.RackName,
                            PhotoPath = or.ProductCatalogRack.Photos.FirstOrDefault().PhotoPath
                        })
                    })
                    .ToList();

                // Fetch the order history
                var orderIds = orders.Select(o => o.OrderID).ToList();
                var orderHistory = db.vwCustomerShippedOrders
                    .Where(so => so.CustomerID == CustomerId && orderIds.Contains(so.OrderID))
                    .ToList();

                // Create the final result
                var result = orderHistory.Select(so => new OrderHistory
                {
                    OrderID = so.OrderID,
                    OrderDate = so.OrderDate,
                    QuoteDate = so.QuoteDate,
                    CatalogueName = orders.FirstOrDefault(o => o.OrderID == so.OrderID)
                                     ?.OrderItems.FirstOrDefault()?.CatalogName,
                    RequestedShipDate = so.RequestedShipDate,
                    ShipDate = orders.FirstOrDefault(o => o.OrderID == so.OrderID)
                                     ?.OrderItems.FirstOrDefault()?.ShipmentItem?.Shipment.DateShipped,
                    ShipmentID = orders.FirstOrDefault(o => o.OrderID == so.OrderID)
                                     ?.OrderItems.FirstOrDefault()?.ShipmentItem?.Shipment.ShipmentID,
                    ShippedTo = so.ShippedTo,
                    ShippingName = so.ShippingName,
                    Total = so.Total,
                    CustomerID = so.CustomerID,
                    RackID = orders.FirstOrDefault(o => o.OrderID == so.OrderID)
                                     ?.OrderRacks.FirstOrDefault()?.RackId ?? 0,
                    RackName = orders.FirstOrDefault(o => o.OrderID == so.OrderID)
                                     ?.OrderRacks.FirstOrDefault()?.RackName ?? "",
                    PhotoPath = orders.FirstOrDefault(o => o.OrderID == so.OrderID)
                                     ?.OrderRacks.FirstOrDefault()?.PhotoPath ?? string.Empty
                }).ToList();

                return result;
            }
        }
        public List<QueryResult> GetAllCustomerWithOrderHistory(int programID)
        {
            try
            {
                using (var db = new MadduxEntities())
                {
                    //get list of sub customer for the customer
                    List<int> _customers = db.CustomersSubs.Where(x => x.MasterCustomerID == CustomerId).Select(x => x.ChildCustomerID).ToList();
                    _customers.Insert(0, CustomerId);

                    var qrywithprimarystore = db.vwCustomerShippedOrders
                        .Where(x => _customers.Contains(x.CustomerID))
                        .OrderBy(x => x.ShipDate)
                        .Select(x => new QueryResult
                        {
                            Company = db.Customers.Where(cust => cust.CustomerId == x.CustomerID).Select(cust => cust.Company).FirstOrDefault(),
                            CustomerID = x.CustomerID,
                            OrderID = x.OrderID
                        }).ToList();

                    if (programID != 1)
                    {
                        List<int> _orderIDs = db.Orders.Include(x => x.OrderItems).Where(x => _customers.Contains(x.CustomerID) && x.OrderItems.Where(item => item.Product.CatalogId == programID).Any()).Select(x => x.OrderID).ToList();

                        qrywithprimarystore = qrywithprimarystore.Where(r => _orderIDs.Contains(r.OrderID)).ToList();
                    }
                    var list = new List<QueryResult>();

                    foreach (var item in qrywithprimarystore)
                    {
                        if (item.CustomerID != CustomerId)
                        {
                            var result = new QueryResult
                            {
                                Company = item.Company,
                                CustomerID = item.CustomerID
                            };
                            if (list.Any(r => r.CustomerID == item.CustomerID) == false)
                            {
                                list.Add(result);
                            }
                        }
                    }
                    list = list.OrderBy(qr => qr.Company).ToList();
                    QueryResult primaryCustomer = qrywithprimarystore.FirstOrDefault(qr => qr.CustomerID == CustomerId);
                    list.Insert(0, primaryCustomer);

                    return list;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        /// <summary>
        /// Subcustomers with order history between two dates
        /// </summary>
        /// <param name="startDate">The <see cref="DateTime"/> start date to filter by</param>
        /// <param name="endDate">The <see cref="DateTime"/> end date to filter by</param>
        /// <param name="programID">The program ID to filter records by </param>
        /// <returns>List of customers</returns>
        public List<QueryResult> GetSubCustomersWithOrderHistory(int programID)
        {
            using (MadduxEntities db = new MadduxEntities())
            {
                List<int> _customers = db.CustomersSubs
                    .Where(x => x.MasterCustomerID == CustomerId)
                    .Select(x => x.ChildCustomerID)
                    .ToList();

                _customers.Add(CustomerId);

                List<QueryResult> qrywithprimarystore = db.vwCustomerShippedOrders
                    .Where(x => _customers.Contains(x.CustomerID))
                    .OrderBy(x => x.ShipDate)
                    .Select(x => new QueryResult
                    {
                        Company = db.Customers.Where(cust => cust.CustomerId == x.CustomerID).Select(cust => cust.Company).FirstOrDefault(),
                        CustomerID = x.CustomerID,
                        OrderID = x.OrderID
                    }).ToList();

                if (programID != 1)
                {
                    List<int> _orderIDs = db.Orders
                        .Include(x => x.OrderItems)
                        .Where(x => _customers.Contains(x.CustomerID) && x.OrderItems.Where(item => item.Product.CatalogId == programID).Any())
                        .Select(x => x.OrderID)
                        .ToList();

                    qrywithprimarystore = qrywithprimarystore
                        .Where(r => _orderIDs.Contains(r.OrderID))
                        .ToList();
                }

                return qrywithprimarystore
                .GroupBy(q => q.CustomerID)
                .Select(q => new QueryResult
                {
                    Company = q.First().Company,
                    CustomerID = q.First().CustomerID

                }).ToList();
            }
        }

        public List<OrderHistory> DraftOrders()
        {
            using (var db = new MadduxEntities())
            {
                var orderData = db.Orders
                    .Where(r => r.OrderStatus == 0 && r.CustomerID == this.CustomerId)
                    .Select(r => new
                    {
                        CustomerID = r.CustomerID,
                        OrderDate = r.OrderDate,
                        OrderID = r.OrderID,
                        BulkOrderKey = r.BulkOrderKey,
                        RackID = r.OrderRacks.Select(or => or.RackId).FirstOrDefault(),
                        RackName = r.OrderRacks.Any() ? r.OrderRacks.FirstOrDefault().ProductCatalogRack.RackName : "",
                        CatalogueName = db.Orders.FirstOrDefault(o => o.OrderID == r.OrderID).OrderItems.FirstOrDefault().Product.ProductCatalog.CatalogName,
                        QuoteDate = r.QuoteDate,
                        RequestedShipDate = r.RequestedShipDate,
                        ShipDate = r.ShipDate,
                        ShippedTo = r.ShippingAddress,
                        ShippingName = r.ShippingName,
                        ShippingCharge = r.ShippingCharge,
                        GSTAmount = r.GSTAmount,
                        PSTAmount = r.PSTAmount,
                        Items = r.OrderItems.ToList()
                    }).ToList();


                var orders = orderData.Select(r => new OrderHistory
                {
                    CustomerID = r.CustomerID,
                    OrderDate = r.OrderDate,
                    OrderID = r.OrderID,
                    BulkOrderKey = r.BulkOrderKey,
                    RackID = r.RackID,
                    RackName = r.RackName,
                    CatalogueName = r.CatalogueName,
                    QuoteDate = r.QuoteDate,
                    RequestedShipDate = r.RequestedShipDate,
                    ShipDate = r.ShipDate,
                    ShippedTo = r.ShippedTo,
                    ShippingName = r.ShippingName,
                    Subtotal = r.Items.Sum(s => OrderHelper.GetTotalPrice(s.UnitPrice, s.DiscountPercent, s.Quantity)),
                    Total = r.Items.Sum(s => OrderHelper.GetTotalPrice(s.UnitPrice, s.DiscountPercent, s.Quantity)) + (double)r.ShippingCharge + (double)r.GSTAmount + (double)r.PSTAmount
                })
                .ToList();

                return orders.ToList();
            }
        }

        public List<AssociationResult> MembershipAssociations()
        {
            using (var db = new MadduxEntities())
            {
                var qry = from ca in db.CustomerAsscs
                          join a in db.Associations on ca.AssociationID equals a.AssociationID
                          where ca.CustomerID == CustomerId && a.Class == "Membership"
                          select new AssociationResult
                          {
                              AssociationID = a.AssociationID,
                              AsscDesc = a.AsscDesc,
                              TagLine = a.TagLine,
                              BannerMessage = a.BannerMessage,
                              BannerStartDate = a.BannerStartDate,
                              BannerEndDate = a.BannerEndDate,
                              SalesBannerMessage = a.SalesBannerMessage,
                              SalesBannerStartDate = a.SalesBannerStartDate,
                              SalesBannerEndDate = a.SalesBannerEndDate
                          };

                return qry.ToList();
            }

        }
        public List<OrderHistory> UnshippedOrders()
        {
            using (var db = new MadduxEntities())
            {
                var query = new List<OrderHistory>();
                foreach (var item in db.vwCustomerUnshippedOrders.Where(r => r.CustomerID == this.CustomerId).ToList())
                {
                    var r = db.Orders.FirstOrDefault(o => o.OrderID == item.OrderID);

                    ProductCatalogRack productCatalogRack = null;
                    if (r.OrderRacks.Any())
                    {
                        productCatalogRack = r.OrderRacks.FirstOrDefault().ProductCatalogRack;
                    }

                    var orderHistory = new OrderHistory
                    {
                        Approved = r.Approved,
                        CustomerID = r.CustomerID,
                        OrderDate = r.OrderDate,
                        OrderID = r.OrderID,
                        BulkOrderKey = r.BulkOrderKey,
                        QuoteDate = r.QuoteDate,
                        PONumber = r.PONumber,
                        RackID = productCatalogRack != null ? productCatalogRack.RackID : 0,
                        RackName = productCatalogRack != null ? productCatalogRack.RackName : string.Empty,
                        PhotoPath = productCatalogRack.DisplayPhotoPath,
                        CatalogueName = r.OrderItems.FirstOrDefault().Product.ProductCatalog.CatalogName,
                        RequestedShipDate = r.RequestedShipDate,
                        ShipDate = r.ShipDate,
                        ShippedTo = r.ShippingAddress + "," + r.ShippingCity,
                        ShippingName = r.ShippingName,
                        Total = r.OrderRacks.Any() ? r.OrderRacks.Sum(s => s.OrderItems.Sum(i => OrderHelper.GetTotalPrice(i.UnitPrice, i.DiscountPercent, s.Quantity * i.Quantity))) :
                            r.OrderItems.Sum(i => OrderHelper.GetTotalPrice(i.UnitPrice, i.DiscountPercent, i.Quantity))
                    };
                    query.Add(orderHistory);
                }
                return query;
            }
        }
        /// <summary>
        /// Retrieves sub customers/stores linked to the current customer/store with Unshipped Orders
        /// </summary>
        /// <returns></returns>
        public List<QueryResult> GetSubCustomersWithUnshippedOrders()
        {
            using (MadduxEntities db = new MadduxEntities())
            {
                //Customer = main store
                QueryResult customer = db.Customers
                    .Select(x => new QueryResult
                    {
                        CustomerID = x.CustomerId,
                        Company = x.Company
                    })
                    .FirstOrDefault(x => x.CustomerID == CustomerId);

                //Customer sub store with Unshipped Orders
                IQueryable<QueryResult> query = from uo in db.vwCustomerUnshippedOrders
                                                join cs in db.CustomersSubs on uo.CustomerID equals cs.ChildCustomerID
                                                join c in db.Customers on cs.ChildCustomerID equals c.CustomerId
                                                where cs.MasterCustomerID == CustomerId || cs.ChildCustomerID == CustomerId
                                                orderby customer.Company, uo.ShipDate descending
                                                select new QueryResult { CustomerID = c.CustomerId, Company = c.Company, FirstName = c.FirstName };

                //Customer's sub stores
                List<QueryResult> customers = query.Distinct().ToList();

                //Insert main customer/store at index 0 to always have it as first store
                if (!customers.Any(x => x.CustomerID == customer.CustomerID))
                    customers.Insert(0, customer);

                return customers;
            }
        }
        public List<QueryResult> GetSubCustomersWithDraftOrders(bool distinct = true)
        {

            using (var db = new MadduxEntities())
            {
                QueryResult _customer = db.Customers.Select(x => new QueryResult
                {
                    CustomerID = x.CustomerId,
                    Company = x.Company
                }).FirstOrDefault(x => x.CustomerID == CustomerId);

                var query = from order in db.vwCustomerDraftOrders
                            join customer in db.Customers on order.CustomerID equals customer.CustomerId
                            join cs in db.CustomersSubs on customer.CustomerId equals cs.ChildCustomerID
                            where cs.MasterCustomerID == CustomerId
                            select new QueryResult { CustomerID = customer.CustomerId, Company = customer.Company };

                List<QueryResult> results = new List<QueryResult>();
                if (distinct)
                {
                    results = query.Distinct().ToList();
                }
                else
                {
                    results = query.ToList();
                }
                results = query.OrderBy(qr => qr.Company).ToList();
                results.Insert(0, _customer);

                return results;
            }

        }

        public string ShippingAddressComplete
        {
            get
            {
                string address;
                address = string.Format("{0} \n{1}, {2} {3}", this.Address, this.City, this.State, this.Zip);

                return address.Trim();
            }
        }
    }
    public partial class Contact
    {
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
    }
}
