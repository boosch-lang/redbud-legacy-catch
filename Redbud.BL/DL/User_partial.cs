using Redbud.BL.Utils;
using System;
using System.Data.Entity;
using System.Linq;

namespace Redbud.BL.DL
{
    public partial class User
    {
        public string PasswordUnEncrypted
        {
            get
            {
                return FCSEncryption.Decrypt(this.Password);

            }

        }
        public string ProfileImage
        {
            get
            {
                if (Gender == "F")
                {
                    return "img/userf-160x160.png";
                }
                else
                {
                    return "img/userm-160x160.png";
                }
            }
        }
        public string FullName
        {
            get
            {
                return this.FirstName + " " + this.LastName;
            }
        }
        public int TotalMyFollowups
        {
            get
            {
                try
                {
                    using (var db = new MadduxEntities())
                    {
                        var qry = from fu in db.vwMyFollowups
                                  where fu.AssignedToId == this.UserID
                                  select fu;

                        return qry.ToList().Count;
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public int TotalFollowups
        {
            get
            {
                try
                {
                    using (var db = new MadduxEntities())
                    {
                        var qry = from fu in db.vwMyFollowups
                                  select fu;

                        return qry.ToList().Count;
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public int TotalQuotes
        {
            get
            {
                try
                {
                    using (var db = new MadduxEntities())
                    {
                        var orders = db.vwMyOrders.Where(r => r.OrderStatus == -1);
                        if (!ShowOtherMyOrders)
                        {
                            orders = orders.Where(r => r.SalesPersonID == UserID);
                        }

                        return orders.ToList().Count;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public int TotalDraftOrders
        {
            get
            {
                try
                {
                    using (var db = new MadduxEntities())
                    {
                        var orders = db.vwMyOrders.Where(r => r.OrderStatus == 0);
                        if (!ShowOtherMyOrders)
                        {
                            orders = orders.Where(r => r.SalesPersonID == UserID);
                        }

                        return orders.ToList().Count;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public int TotalOrders
        {
            get
            {
                try
                {
                    using (var db = new MadduxEntities())
                    {
                        var orders = db.vwMyOrders.Where(r => r.OrderStatus == 1);
                        if (!ShowOtherMyOrders)
                        {
                            orders = orders.Where(r => r.SalesPersonID == UserID);
                        }

                        return orders.ToList().Count;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public int TotalShipments
        {
            get
            {
                try
                {
                    using (var db = new MadduxEntities())
                    {
                        var orders = from s in db.vwMyShipments
                                     select s;
                        if (!ShowOtherMyShipments)
                        {
                            orders = orders.Where(r => r.SalesPersonID == UserID);
                        }

                        return orders.ToList().Count;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public int TotalPurchaseOrders
        {
            get
            {
                try
                {
                    using (var db = new MadduxEntities())
                    {
                        var purchaseOrders = from po in db.PurchaseOrders
                                             select po;

                        return purchaseOrders.Count();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public int TotalUsers
        {

            get
            {
                try
                {
                    using (var db = new MadduxEntities())
                    {
                        return db.Users.Count();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public int TotalRacks
        {
            get
            {
                try
                {
                    using (MadduxEntities db = new MadduxEntities())
                    {
                        return db.ProductCatalogRacks.Count();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public int TotalPrograms
        {
            get
            {
                try
                {
                    using (MadduxEntities db = new MadduxEntities())
                    {
                        return db.ProductPrograms
                         .Count();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public int TotalProducts
        {
            get
            {
                try
                {
                    using (MadduxEntities db = new MadduxEntities())
                    {
                        return db.Products.Count();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public object TotalCatalogs
        {
            get
            {
                try
                {
                    using (MadduxEntities db = new MadduxEntities())
                    {
                        return db.ProductCatalogs
                            .Count();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public object TotalCategories
        {
            get
            {
                try
                {
                    using (MadduxEntities db = new MadduxEntities())
                    {
                        return db.supProductSubCategories.Count();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public object TotalCampaigns
        {
            get
            {
                try
                {
                    using (MadduxEntities db = new MadduxEntities())
                    {
                        return db.Campaigns
                            .Include(c => c.CampaignShipdates)
                            .Include(c => c.CampaignShipdates.Select(x => x.ProductCatalogShipDate))
                            .Count(c => c.CampaignShipdates.Any(cs => cs.ProductCatalogShipDate.ShipDate > DateTime.Today) || (c.Shipdate.HasValue && c.Shipdate.Value > DateTime.Today));
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public object TotalAssociations
        {
            get
            {
                try
                {
                    using (MadduxEntities db = new MadduxEntities())
                    {
                        return db.Associations.Count();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public object TotalFreightCharges
        {
            get
            {
                try
                {
                    using (MadduxEntities db = new MadduxEntities())
                    {
                        return db.FreightCharges.Count();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public object TotalStaticPages
        {
            get
            {
                try
                {
                    using (MadduxEntities db = new MadduxEntities())
                    {
                        return db.StaticPages.Count();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public bool LogActivity(ActivityLog log)
        {
            bool result = false;
            using (var db = new MadduxEntities())
            {
                db.ActivityLogs.Add(log);
                int count = db.SaveChanges();
                if (count > 0) result = true;
                return result;
            }
        }
        public bool LogActivity(string sessionID, string email, string page, string logDesc, string platForm, string browser, string userAgent, string bVersion, string hostAdd)
        {
            bool result = false;
            using (var db = new MadduxEntities())
            {
                if (!EmailAddress.ToLower().Contains("redbud"))
                {
                    //Log login activity
                    var log = db.ActivityLogs.Create();
                    log.SessionId = sessionID;
                    log.CustomerEmail = email;
                    log.CustomerId = this.UserID;
                    log.ActivityDesc = logDesc;
                    log.BrowserVersion = bVersion;
                    log.IpAddress = hostAdd;
                    log.PageName = page;
                    log.Platform = platForm;
                    log.BrowserType = browser;
                    log.ActivityDate = DateTime.UtcNow;

                    db.ActivityLogs.Add(log);

                    //Log login activity as journal
                    //var journal = db.Journals.Create();
                    //journal.CustomerID = 3; //Redbud Supply Inc.
                    //journal.DateStamp = DateTime.UtcNow;
                    //journal.IsResolved = true;
                    //journal.CreateDate = DateTime.UtcNow;
                    //journal.AssignedToId = UserID;
                    //journal.Notes = logDesc + " " + bVersion + " "+ hostAdd + " "+ page + " "+ platForm + " "+ browser;

                    //db.Journals.Add(journal);

                    int count = db.SaveChanges();
                    if (count > 0)
                    {
                        result = true;
                    }
                }

                return result;
            }
        }
    }
}
