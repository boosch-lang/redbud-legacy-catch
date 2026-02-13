using Redbud.BL.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data.Entity;

namespace Redbud.BL.DL
{
    public static class Lookup
    {
        public static void LoadAssociationDropDown(ref DropDownList combo, bool assignedOnly, bool addAllOption, int userID)
        {
            try
            {
                using (var db = new MadduxEntities())
                {

                    var query = from a in db.vwCustomersByAssociations
                                select new
                                {
                                    a.AssociationID,
                                    a.AsscDesc
                                };

                    if (assignedOnly)
                    {
                        var associationIds = db.UserAsscs.Where(r => r.UserID == userID).Select(r => r.AssociationID);
                        query = query.Where(r => associationIds.Contains(r.AssociationID));
                    }
                    var associations = query.Distinct().ToList();

                    combo.DataValueField = "AssociationID";
                    combo.DataTextField = "AsscDesc";
                    combo.DataSource = associations.OrderBy(a => a.AsscDesc);
                    combo.DataBind();

                    if (addAllOption)
                    {
                        combo.Items.Insert(0, new ListItem
                        {
                            Value = "-1",
                            Text = "-- All Associations --"
                        });

                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void LoadMembershipDropDown(ref DropDownList combo, bool assignedOnly, bool addAllOption, int userID)
        {
            try
            {
                using (var db = new MadduxEntities())
                {

                    var query = from a in db.vwCustomersByAssociations
                                where a.AsscDesc.StartsWith("Member - ")
                                select new
                                {
                                    a.AssociationID,
                                    a.AsscDesc
                                };

                    if (assignedOnly)
                    {
                        var associationIds = db.UserAsscs.Where(r => r.UserID == userID).Select(r => r.AssociationID);
                        query = query.Where(r => associationIds.Contains(r.AssociationID));
                    }
                    var associations = query.Distinct().ToList();

                    combo.DataValueField = "AssociationID";
                    combo.DataTextField = "AsscDesc";
                    combo.DataSource = associations.OrderBy(a => a.AsscDesc);
                    combo.DataBind();

                    if (addAllOption)
                    {
                        combo.Items.Insert(0, new ListItem
                        {
                            Value = "-1",
                            Text = "-- All Memberships --"
                        });

                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void LoadCatalogDropDown(ref DropDownList combo, bool assignedOnly, bool addAllOption, int userID)
        {
            try
            {
                using (var db = new MadduxEntities())
                {

                    //                    SELECT dbo.OrderItems.OrderId, dbo.ProductCatalog.CatalogId, dbo.ProductCatalog.CatalogName, dbo.ProductCatalog.CatalogClassId, dbo.ProductCatalog.CatalogSeason, dbo.ProductCatalog.CatalogYear, dbo.ProductCatalog.Active,
                    //                         dbo.ProductCatalog.ShowOnMyAccount, dbo.ProductCatalog.ShowBothItemNumbers, dbo.ProductCatalog.PDFUrl
                    //FROM            dbo.OrderItems INNER JOIN
                    //                         dbo.Products ON dbo.OrderItems.ProductId = dbo.Products.ProductId INNER JOIN
                    //                         dbo.ProductCatalog ON dbo.Products.CatalogId = dbo.ProductCatalog.CatalogId
                    //GROUP BY dbo.OrderItems.OrderId, dbo.ProductCatalog.CatalogId, dbo.ProductCatalog.CatalogName, dbo.ProductCatalog.CatalogClassId, dbo.ProductCatalog.CatalogSeason, dbo.ProductCatalog.CatalogYear, dbo.ProductCatalog.PDFUrl,
                    //                         dbo.ProductCatalog.Active, dbo.ProductCatalog.ShowOnMyAccount, dbo.ProductCatalog.ShowBothItemNumbers

                    var qry = (from oi in db.OrderItems
                               join c in db.ProductCatalogs on oi.Product.CatalogId equals c.CatalogId
                               select new
                               {
                                   oi.Order,
                                   c.CatalogId,
                                   c.CatalogName,
                                   c.CatalogClassId,
                                   c.CatalogSeason,
                                   c.CatalogYear,
                                   c.Active
                               });


                    if (assignedOnly)
                    {
                        //warmCountries.Join(europeanCountries, warm => warm, european => european, (warm, european) => warm);
                        qry = qry.Where(r => r.Order.SalesPersonID == userID);
                    }

                    var cats = qry.Select(r => new
                    {
                        r.CatalogYear,
                        r.CatalogId,
                        r.CatalogName
                    }).Distinct();

                    var models = cats.OrderByDescending(c => c.CatalogYear).ThenBy(c => c.CatalogName).Select(r => new
                    {
                        r.CatalogId,
                        r.CatalogName
                    }).ToList();

                    if (addAllOption)
                    {
                        models.Insert(0, new
                        {
                            CatalogId = -1,
                            CatalogName = "-- All Catalogs --"
                        });

                    }

                    combo.DataValueField = "CatalogID";
                    combo.DataTextField = "CatalogName";
                    combo.DataSource = models;
                    combo.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void LoadCatalogListBox(ref ListBox combo, bool assignedOnly, bool addAllOption, int userID)
        {
            try
            {
                using (var db = new MadduxEntities())
                {
                    var qry = (from oi in db.OrderItems
                               join c in db.ProductCatalogs on oi.Product.CatalogId equals c.CatalogId
                               select new
                               {
                                   oi.Order,
                                   c.CatalogId,
                                   c.CatalogName,
                                   c.CatalogClassId,
                                   c.CatalogSeason,
                                   c.CatalogYear,
                                   c.Active
                               });


                    if (assignedOnly)
                    {
                        qry = qry.Where(r => r.Order.SalesPersonID == userID);
                    }

                    var cats = qry.Select(r => new
                    {
                        r.CatalogYear,
                        r.CatalogId,
                        r.CatalogName
                    }).Distinct();

                    var models = cats.OrderByDescending(c => c.CatalogYear).ThenBy(c => c.CatalogName).Select(r => new
                    {
                        r.CatalogId,
                        r.CatalogName
                    }).ToList();

                    combo.DataValueField = "CatalogID";
                    combo.DataTextField = "CatalogName";
                    combo.DataSource = models;
                    combo.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void LoadContactTitlesDropDown(ref DropDownList combo)
        {
            try
            {
                using (var db = new MadduxEntities())
                {
                    var userTable = db.supContactTitles.ToList();

                    combo.DataValueField = "TitleDesc";
                    combo.DataTextField = "TitleDesc";
                    combo.DataSource = userTable;
                    combo.DataBind();

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void LoadCustomerDropDown(ref DropDownList combo)
        {
            try
            {
                using (var db = new MadduxEntities())
                {
                    var customers = db.Customers.Where(x => x.Active).OrderBy(x => x.Company)
                        .Select(r => new
                        {
                            CustomerID = r.CustomerId,
                            Customer = r.Company,
                        }).ToList();

                    customers.Insert(0, new
                    {
                        CustomerID = 0,
                        Customer = "-- All Customers --"
                    });

                    combo.DataValueField = "CustomerID";
                    combo.DataTextField = "Customer";
                    combo.DataSource = customers;
                    combo.DataBind();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void LoadShipDatesDropDown(ref DropDownList combo)
        {
            try
            {
                using (var db = new MadduxEntities())
                {
                    var shipDates = db.ProductCatalogs.SelectMany(x => x.ProductCatalogShipDates).AsEnumerable()
                       .Select(r => new
                       {
                           Text = r.ShipDate.ToString("dd-MMM-yyyy"),
                           r.ShipDate,
                       }).Distinct().OrderBy(x => x.ShipDate).Where(x=>x.ShipDate >= DateTime.Today).ToList();

                    shipDates.Insert(0, new
                    {
                        Text = "-- All Req. Ship Dates --",
                        ShipDate = DateTime.MinValue
                    });

                    combo.DataValueField = "ShipDate";
                    combo.DataTextField = "Text";
                    combo.DataSource = shipDates;
                    combo.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void LoadCountryDropDown(ref DropDownList combo, bool addAllOption)
        {
            try
            {
                using (var db = new MadduxEntities())
                {
                    var countryTable = db.Countries.ToList();



                    if (addAllOption)
                    {
                        countryTable.Insert(0, new Country
                        {
                            CountryCode = "000",
                            CountryName = "-- All countries --"
                        });

                    }

                    combo.DataValueField = "CountryCode";
                    combo.DataTextField = "CountryName";
                    combo.DataSource = countryTable;
                    combo.DataBind();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void LoadProvinceDropDown(ref DropDownList combo, bool assignedOnly, bool addAllOption, bool addSelectOption, int userID)
        {
            try
            {
                using (var db = new MadduxEntities())
                {
                    var stateTable = db.States.Where(x => x.Country.ToLower() == "canada").Select(r => new
                    {
                        StateId = r.StateID,
                        r.StateName
                    }).ToList();

                    if (assignedOnly)
                    {
                        var userState = db.UserStates.Where(r => r.UserStateID == userID).Select(r => r.StateID).ToList();
                        stateTable = stateTable.Where(r => userState.Contains(r.StateId)).ToList();
                    }


                    if (addAllOption)
                    {
                        stateTable.Insert(0, new
                        {
                            StateId = "00",
                            StateName = "-- All Provinces --"
                        });
                    }

                    if (addSelectOption)
                    {
                        stateTable.Insert(0, new
                        {
                            StateId = "--",
                            StateName = "-- Select Province --"
                        });
                    }

                    combo.DataValueField = "StateId";
                    combo.DataTextField = "StateName";
                    combo.DataSource = stateTable;
                    combo.DataBind();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void LoadCanadianProvinceDropDown(ref DropDownList combo, bool assignedOnly, bool addAllOption, bool addSelectOption, int userID)
        {
            try
            {
                using (var db = new MadduxEntities())
                {
                    var stateTable = db.States.Where(x => x.Country.ToLower() == "canada").Select(r => new
                    {
                        StateId = r.StateID,
                        r.StateName
                    }).ToList();

                    if (assignedOnly)
                    {
                        var userState = db.UserStates.Where(r => r.UserStateID == userID).Select(r => r.StateID).ToList();
                        stateTable = stateTable.Where(r => userState.Contains(r.StateId)).ToList();
                    }


                    if (addAllOption)
                    {
                        stateTable.Insert(0, new
                        {
                            StateId = "00",
                            StateName = "-- All Provinces --"
                        });
                    }

                    if (addSelectOption)
                    {
                        stateTable.Insert(0, new
                        {
                            StateId = "--",
                            StateName = "-- Select Province --"
                        });
                    }

                    combo.DataValueField = "StateId";
                    combo.DataTextField = "StateName";
                    combo.DataSource = stateTable;
                    combo.DataBind();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void LoadUSProvinceDropDown(ref DropDownList combo, bool assignedOnly, bool addAllOption, bool addSelectOption, int userID)
        {
            try
            {
                using (var db = new MadduxEntities())
                {
                    var stateTable = db.States.Where(x => x.Country.ToLower() == "usa").Select(r => new
                    {
                        StateId = r.StateID,
                        r.StateName
                    }).ToList();

                    if (assignedOnly)
                    {
                        var userState = db.UserStates.Where(r => r.UserID == userID).Select(r => r.StateID).ToList();
                        stateTable = stateTable.Where(r => userState.Contains(r.StateId)).ToList();
                    }


                    if (addAllOption)
                    {
                        stateTable.Insert(0, new
                        {
                            StateId = "00",
                            StateName = "-- All Provinces --"
                        });
                    }

                    if (addSelectOption)
                    {
                        stateTable.Insert(0, new
                        {
                            StateId = "--",
                            StateName = "-- Select Province --"
                        });
                    }

                    combo.DataValueField = "StateId";
                    combo.DataTextField = "StateName";
                    combo.DataSource = stateTable;
                    combo.DataBind();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void LoadUserDropDown(ref DropDownList combo, bool addAllOption)
        {
            try
            {
                using (var db = new MadduxEntities())
                {
                    var userTable = db.Users.Where(r => r.Active || r.UserID == 0).ToList();


                    if (addAllOption)
                    {
                        userTable.Insert(0, new User
                        {
                            UserID = -1,
                            FirstName = "-- All",
                            LastName = "users --"
                        });

                    }

                    combo.DataValueField = "UserID";
                    combo.DataTextField = "FullName";
                    combo.DataSource = userTable;
                    combo.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void LoadProgramDropDown(ref DropDownList combo, bool addAllOption)
        {
            try
            {
                using (var db = new MadduxEntities())
                {
                    var programTable = db.ProductPrograms.Where(r => r.ProductCatalogs.Any(x => x.Active)).OrderBy(x => x.ProgramName).ToList();

                    if (addAllOption)
                    {
                        programTable.Insert(0, new ProductProgram
                        {
                            ProgramID = -1,
                            ProgramName = "-- All Programs --",

                        });

                    }

                    combo.DataValueField = "ProgramId";
                    combo.DataTextField = "ProgramName";
                    combo.DataSource = programTable;
                    combo.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void LoadCampaignDropDown(ref DropDownList combo, bool addAllOption, bool addSelectOption = false)
        {
            try
            {
                using (var db = new MadduxEntities())
                {
                    var campaignTable = db
                        .Campaigns
                        .Include(c => c.CampaignShipdates)
                        .Include(c => c.CampaignShipdates.Select(x => x.ProductCatalogShipDate))
                        .AsNoTracking()
                        .Where(c => c.CampaignShipdates.Any(cs => cs.ProductCatalogShipDate.ShipDate > DateTime.Today) || ((c.Shipdate.HasValue && c.Shipdate.Value > DateTime.Today)))
                        .OrderBy(c => c.SalesEnd).ToList();

                    if (addAllOption)
                    {
                        campaignTable.Insert(0, new Campaign
                        {
                            CampaignID = -1,
                            CampaignName = "-- All Campaigns --",

                        });

                    }
                    if (addSelectOption)
                    {
                        campaignTable.Insert(0, new Campaign
                        {
                            CampaignID = -1,
                            CampaignName = "-- Select a Campaign --",

                        });
                    }

                    combo.DataValueField = "CampaignId";
                    combo.DataTextField = "CampaignName";
                    combo.DataSource = campaignTable;
                    combo.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void LoadDeliveryHubDropdown(ref DropDownList combo)
        {
            try
            {
                using (var db = new MadduxEntities())
                {
                    var hubs = db.DeliveryHubs
                        .Select(r => new
                        {
                            r.HubID,
                            HubName = r.Name,
                        }).ToList();

                    hubs.Insert(0, new
                    {
                        HubID = 0,
                        HubName = "-- All Delivery Hubs --"
                    });

                    combo.DataValueField = "HubID";
                    combo.DataTextField = "HubName";
                    combo.DataSource = hubs;
                    combo.DataBind();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static void LoadProvinceMultiSelect(ref ListBox combo, bool assignedOnly, bool addAllOption, bool addSelectOption, int userID)
        {
            try
            {
                using (var db = new MadduxEntities())
                {
                    var stateTable = db.States.Where(x => x.Country.ToLower() == "canada").Select(r => new
                    {
                        StateId = r.StateID,
                        r.StateName
                    }).ToList();

                    if (assignedOnly)
                    {
                        var userState = db.UserStates.Where(r => r.UserStateID == userID).Select(r => r.StateID).ToList();
                        stateTable = stateTable.Where(r => userState.Contains(r.StateId)).ToList();
                    }


                    if (addAllOption)
                    {
                        stateTable.Insert(0, new
                        {
                            StateId = "00",
                            StateName = "-- All Provinces --"
                        });
                    }

                    if (addSelectOption)
                    {
                        stateTable.Insert(0, new
                        {
                            StateId = "--",
                            StateName = "-- Select Province --"
                        });
                    }

                    combo.DataValueField = "StateId";
                    combo.DataTextField = "StateName";
                    combo.DataSource = stateTable;
                    combo.DataBind();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Populates the campaign multiselect drowndown
        /// </summary>
        /// <param name="combo">The <see cref="ListBox"/> to load the campaign</param>
        public static void LoadCampaignDropDown(ref ListBox listBox)
        {
            try
            {
                using (MadduxEntities db = new MadduxEntities())
                {
                    //Active campaigns
                    List<Campaign> campaigns = db.Campaigns
                        .Where(c => c.SalesEnd >= DateTime.Today)
                        .OrderBy(c => c.CampaignName)
                        .ToList();

                    //Bind capaigns to listbox
                    listBox.DataValueField = "CampaignId";
                    listBox.DataTextField = "CampaignName";
                    listBox.DataSource = campaigns;
                    listBox.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
