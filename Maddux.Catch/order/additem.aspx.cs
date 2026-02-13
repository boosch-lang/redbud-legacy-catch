using Maddux.Catch.LocalClasses;
using Redbud.BL;
using Redbud.BL.DL;
using Redbud.BL.Helpers;
using Redbud.BL.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Maddux.Catch.order
{

    public class CatalogRackItem
    {
        public string RackContainerMargin { get; set; }
        public bool IsFirstInRack { get; set; }
        public int RackID { get; set; }
        public int RackRackID { get; set; }
        public int ProductID { get; set; }
        public double UnitPrice { get; set; }
        public bool HasProductPhoto { get; set; }
        public string ProductPhotoPath { get; set; }
        public string ProductName { get; set; }
        public string UnitPriceFormatted { get; set; }
        public int UnitPerCase { get; set; }
        public double DefaultQuantity { get; set; }
        public string ProductUPCCode { get; set; }
        public bool IsBulkRack { get; set; }
    }

    public class CatalogRackImage
    {
        public string PhotoPath { get; set; }
        public string RackName { get; set; }
        public int RackID { get; set; }
        public int PhotoId { get; set; }
    }

    public class CatalogRackModel
    {
        public int RackID { get; set; }
        public int CatalogID { get; set; }
        public int? ProgramID { get; set; }
        public string RackDescription { get; set; }
        public string ProgramName { get; set; }
        public string RackImage { get; set; }
        public List<CatalogRackImage> RackImages { get; set; }
        public bool HasMultipleImages => RackImages.Any();
        public string TrayCount { get; set; }
        public bool AllowCustomization { get; set; }
        public List<ProductRackShipDate> ShipDates { get; set; }
        public Double Price { get; set; }
        public string ProductCatalogDescription { get; set; }
        public int? MinimumItems { get; set; }
        public double? UnitCount { get; set; }
        public string ItemType { get; set; }
        public double Discount { get; set; }
        public int RackType { get; set; }
    }

    public partial class additem : System.Web.UI.Page
    {
        private Dictionary<int, List<ProductCatalogShipDate>> ProductCatalogShipDates;
        private List<Customer> p_SubsTable;
        private User currentUser;
        private int OrderID { get; set; }
        private Customer currentCustomer
        {
            get
            {
                using (var db = new MadduxEntities())
                {
                    return db.Customers.FirstOrDefault(x => x.CustomerId == CustomerID);
                }
            }
        }
        public int CustomerID
        {
            get
            {
                if (ViewState["CustomerID"] == null)
                {
                    ViewState["CustomerID"] = Request.QueryString["CustomerID"];
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
            currentUser = AppSession.Current.CurrentUser;
            ProductCatalogShipDates = new Dictionary<int, List<ProductCatalogShipDate>>();
            Literal litPageHeader = (Literal)Master.FindControl("litPageHeader");
            litPageHeader.Text = "New Order";
            if (Request.QueryString["CustomerID"] == null || Request.QueryString["CustomerID"] == "")
                return;
            if (!Page.IsPostBack)
            {
                PopulateDropdowns();
                LoadCustomers();
                rptStores.DataSource = p_SubsTable;
                rptStores.DataBind();
            }
        }
        private void LoadCustomers()
        {
            try
            {
                Customer theCustomer = null;
                using (var db = new MadduxEntities())
                {
                    theCustomer = db.Customers.FirstOrDefault(x => x.CustomerId == CustomerID);
                }
                if (theCustomer == null)
                {
                    lbError.Text = StringTools.GenerateError("Requested customer not fount");
                    return;
                }

                p_SubsTable = theCustomer.GetSubCustomers();

                p_SubsTable.Insert(0, new Customer
                {
                    CustomerId = theCustomer.CustomerId,
                    Company = theCustomer.Company
                });
                if (p_SubsTable.Count == 1)
                {
                    SelectAllDiv.Attributes.Add("style", "display:none");
                }
            }
            catch (Exception ex)
            {
                lbError.Text = StringTools.GenerateError(ex.Message);
            }
        }
        protected void ddCatalogList_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (var db = new MadduxEntities())
            {
                List<CatalogRackModel> catalogRacks = new List<CatalogRackModel>();

                if (ddCatalogList.SelectedValue != "0")
                {
                    int catalogId = int.Parse(ddCatalogList.SelectedValue);
                    ProductCatalog theCatalog = db.ProductCatalogs.FirstOrDefault(r => r.CatalogId == catalogId);


                    foreach (ProductCatalogRack rack in theCatalog.ProductCatalogRacks.Where(x => x.Active).OrderBy(x => x.DisplayOrder))
                    {
                        ProductCatalogRack theRack = db.ProductCatalogRacks.FirstOrDefault(r => r.RackID == rack.RackID);
                        var itemType = theRack.RackType == (int)RackType.Standard ? "tray" : "Smart Stack";

                        CatalogRackModel model = new CatalogRackModel
                        {
                            RackID = theRack.RackID,
                            CatalogID = theCatalog.CatalogId,
                            ProgramID = theCatalog.ProgramID,
                            ProgramName = theCatalog.ProductProgram.ProgramName,
                            RackDescription = rack.RackName,
                            RackType = rack.RackType,
                            RackImage = theRack.Photos.Any() ? theRack.Photos.Select(p => p.PhotoPath).First() : "/img/rack-not-available.jpg",
                            RackImages = theRack.Photos.Select(p => new CatalogRackImage
                            {
                                PhotoId = p.PhotoID,
                                PhotoPath = p.PhotoPath,
                                RackName = theRack.RackName,
                                RackID = theRack.RackID
                            }).Skip(1).ToList(),
                            TrayCount = $"{rack.MaximumItems} tray(s)",
                            AllowCustomization = theRack.AllowCustomization,
                            ShipDates = theRack.ProductRackShipDates.ToList(),
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

                        catalogRacks.Add(model);
                    }

                    rptRacks.DataSource = catalogRacks;
                    rptRacks.DataBind();
                    ScriptDIV.InnerHtml = $@"<script>
                         $('.reset-btn').hide();
                         $('.update-price-btn').hide();
                    </script>";
                }
            }
        }
        /// <summary>
        /// Populates Catalog and Order Received Via dropdowns
        /// </summary>
        private void PopulateDropdowns()
        {
            using (MadduxEntities db = new MadduxEntities())
            {
                IQueryable<int> associationIds = db.CustomerAsscs
                    .Where(ca => ca.CustomerID == CustomerID)
                    .Select(ca => ca.AssociationID);

                IQueryable<int> catalogIds = db.AssociationCatalogs
                    .Where(ac => associationIds.Contains(ac.AssociationID))
                    .Select(ac => ac.CatalogID);

                IQueryable<ProductCatalog> productCatalogs = db.ProductCatalogs
                    .Where(pc => catalogIds.Contains(pc.CatalogId) && pc.Active)
                    .Select(pc => pc);

                if (currentUser.CanOnlyViewAssignedCatalogs)
                {
                    productCatalogs = productCatalogs.Where(pc => db.UserCatalogs.Where(uc => uc.UserID == currentUser.UserID)
                    .Select(uc => uc.CatalogID)
                    .Contains(pc.CatalogId));
                }

                ddCatalogList.DataSource = productCatalogs.ToList();
                ddCatalogList.DataTextField = "CatalogName";
                ddCatalogList.DataValueField = "CatalogId";
                ddCatalogList.DataBind();
                ddCatalogList.Items.Insert(0, new ListItem { Text = "Select a catalog", Value = "0" });
            }

            string message = "Select order received via option"; //Select message for dropdowns
                                                                 //Populates drop down list OrderReceivedVia
            PopulateDropDownFromEnum(ddlOrderReceivedVia, Enum.GetNames(typeof(Redbud.BL.OrderReceivedVia)), message, "0");

            message = "Select Order Approved status";
            //Populates drop down list ddlOrderApproved
            PopulateDropDownFromEnum(ddlOrderApproved, Enum.GetNames(typeof(Redbud.BL.OrderApproved)), message, "-1");
        }

        protected void btnUpdatePrice_Click(object sender, EventArgs e)
        {
            double reckPrice = 0;
            int trays = 0;
            Repeater repeater = ((Control)sender).Parent.FindControl("rptProducts") as Repeater;
            HtmlGenericControl divQtyWarning = ((Control)sender).Parent.FindControl("DivRackQtyWarning") as HtmlGenericControl;
            Button btnChooseStores = ((Control)sender).FindControl("btnChooseStores") as Button;
            HiddenField rID = ((Control)sender).FindControl("RackID") as HiddenField;

            var count = 0;
            foreach (object item in repeater.Items)
            {
                count++;
                int qty = 0;
                double uPrice = 0;
                TextBox textBox = ((Control)item).FindControl("txtProductQuantity") as TextBox;
                HiddenField hdnUPCCode = ((Control)item).FindControl("hdnUPCCode") as HiddenField;
                HiddenField isBulkRack = ((Control)item).FindControl("hdnIsBulkRack") as HiddenField;
                HiddenField isFirstRackItem = ((Control)item).FindControl("hdnIsFirstTime") as HiddenField;
                HiddenField itemRackID = ((Control)item).FindControl("RackID") as HiddenField;

                if (textBox != null)
                {
                    if (isBulkRack.Value == true.ToString())
                    {
                        // If it's a bulk rack, we only calculate off of the first item in the repeater
                        if (isFirstRackItem.Value != true.ToString())
                        {
                            continue;
                        }
                        using (var db = new MadduxEntities())
                        {
                            var rack = db.ProductCatalogRacks.Where(r => r.RackID.ToString() == rID.Value).FirstOrDefault();
                            var rackQty = Convert.ToInt32(textBox.Text);
                            trays += rackQty;
                            var rackRacks = rack.RackRacks.Where(x => x.ProductRackID.ToString() == itemRackID.Value).ToList().Select(r =>
                            new
                            {
                                RackProducts = r.ProductRack.RackProducts.ToList(),
                                Price = r.ProductRack.RackProducts
                                                       .Where(x => x.DefaultQuantity > 0)
                                                       .Sum(x => OrderHelper.GetTotalPrice(x.UnitPrice, rack.Discount ?? 0, rackQty * x.DefaultQuantity)),
                            }).ToList();

                            foreach (var rackRack in rackRacks)
                            {
                                reckPrice += rackRack.Price;
                            }
                        }
                    }
                    else
                    {
                        qty = Convert.ToInt32(textBox.Text);

                        if (qty > 0)
                        {
                            if (!string.IsNullOrWhiteSpace(hdnUPCCode.Value))
                            {
                                trays += qty;
                            }
                            HiddenField unitprice = ((Control)item).FindControl("hdnUnitPrice") as HiddenField;
                            uPrice = Convert.ToDouble(unitprice.Value);
                            reckPrice += qty * uPrice;
                        }
                    }
                }
            }
            ProductCatalogRack theRack = null;
            int rackID = int.Parse(rID.Value);
            using (MadduxEntities db = new MadduxEntities())
            {
                theRack = db.ProductCatalogRacks.FirstOrDefault(r => r.RackID == rackID);
            }

            int _min = Convert.ToInt32(theRack.MinimumItems);
            int _max = Convert.ToInt32(theRack.MaximumItems);

            if (theRack.RackType == (int)RackType.Bulk)
            {

                if (theRack.MinimumItems > trays)
                {
                    btnChooseStores.Enabled = false;
                    divQtyWarning.InnerHtml = $"<div class='alert alert-warning alert-dismissible fade in'  role='alert'> <button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button> You still need to add <strong> minimum {theRack.MinimumItems - trays} pallets </strong> into rack.</div>";
                }
                else if (theRack.MaximumItems < trays)
                {
                    btnChooseStores.Enabled = false;
                    divQtyWarning.InnerHtml = $"<div class='alert alert-warning alert-dismissible fade in'  role='alert'> <button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button> You need to remove <strong> {trays - theRack.MaximumItems} pallets </strong> from rack.</div>";
                }
                else if (trays < theRack.MaximumItems)
                {
                    btnChooseStores.Enabled = true;
                    divQtyWarning.InnerHtml = $"<div class='alert alert-info alert-dismissible fade in'  role='alert'> <button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button>You have selected {trays} racks worth {reckPrice:C}.<br /> You can still add <strong>{_max - trays} more pallets </strong> into the rack.</div>";
                }
                else
                {
                    btnChooseStores.Enabled = true;
                    divQtyWarning.InnerHtml = $"<div class='alert alert-info alert-dismissible fade in'  role='alert'> <button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button> " +
                        $"You have selected {trays} pallets worth {reckPrice:C}. </div>";
                }

            }
            else
            {
                //get the number of remaining products after creating racks with it's maximum capacity
                int remainingItems = trays % _max;

                //get number of rack that can be ordered with it's maximum capacity
                int numberOfRacks = (trays - remainingItems) / _max;
                int racks = numberOfRacks;

                if (theRack.MinimumItems > trays)
                {
                    btnChooseStores.Enabled = false;
                    divQtyWarning.InnerHtml = $"<div class='alert alert-warning alert-dismissible fade in'  role='alert'> <button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button> You still need to add <strong> minimum {theRack.MinimumItems - trays} trays </strong> into rack.</div>";
                }
                else if (theRack.MaximumItems < trays)
                {

                    //check if number of remaining items is enough for rack with minimum number of items
                    if (remainingItems > 0)
                    {
                        if (_min <= remainingItems)
                        {
                            btnChooseStores.Enabled = true;
                            racks++;
                            divQtyWarning.InnerHtml = $"<div class='alert alert-info alert-dismissible fade in'  role='alert'> <button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button>You have selected {racks} racks worth {reckPrice:C}.<br /> You can still add <strong>{_max - remainingItems} more trays </strong> into additional racks.</div>";
                        }
                        else
                        {
                            //if not check if by removing one one rack with it's maximum capacity,
                            //by adding those items in remining items and devide it by 2 and check if result is greater than minimum number of items
                            remainingItems += _max;
                            double rItems = remainingItems / 2;
                            if (rItems >= _min)
                            {
                                //if yes than create two rack with result
                                btnChooseStores.Enabled = true;
                                racks++;
                                divQtyWarning.InnerHtml = $"<div class='alert alert-info alert-dismissible fade in'  role='alert'> <button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button>You have selected {racks} racks worth {reckPrice:C}.<br /> You can still add <strong>{(2 * _max) - remainingItems} more trays </strong> into additional racks.</div>";
                            }
                            else
                            {
                                //throw error
                                btnChooseStores.Enabled = false;
                                racks++;
                                divQtyWarning.InnerHtml = $"<div class='alert alert-warning alert-dismissible fade in'  role='alert'> <button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button>You have selected {numberOfRacks} racks worth {reckPrice:C}.<br /> You still need to add minimum <strong>{(2 * _min) - remainingItems} trays </strong>into rack for additional racks.</div>";
                            }
                        }
                    }
                    else
                    {
                        btnChooseStores.Enabled = true;
                        //racks++;
                        divQtyWarning.InnerHtml = $"<div class='alert alert-info alert-dismissible fade in'  role='alert'> <button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button> " +
                            $"You have selected {numberOfRacks} racks worth {reckPrice:C}. </div>";
                    }
                }
                else if (theRack.MinimumItems < trays && theRack.MaximumItems > trays)
                {
                    btnChooseStores.Enabled = true;
                    racks++;
                    divQtyWarning.InnerHtml = $"<div class='alert alert-info alert-dismissible fade in'  role='alert'> <button type='button' class='close' data-dismiss='alert' aria-label='Close'>" +
                        $"<span aria-hidden='true'>&times;</span></button>You have selected {racks} racks worth {reckPrice:C}.<br /> You can still add <strong>{theRack.MaximumItems - trays} more trays </strong> into this racks.</div>";
                }
                else if (theRack.MaximumItems == trays)
                {
                    btnChooseStores.Enabled = true;
                    racks++;
                    divQtyWarning.InnerHtml = $"<div class='alert alert-info alert-dismissible fade in'  role='alert'> <button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button> " +
                        $"You have selected {racks} racks worth {reckPrice:C}. </div>";
                }
                else
                {
                    btnChooseStores.Enabled = true;
                    racks++;
                    divQtyWarning.InnerHtml = $"<div class='alert alert-info alert-dismissible fade in'  role='alert'> <button type='button' class='close' data-dismiss='alert' aria-label='Close'>" +
                        $"<span aria-hidden='true'>&times;</span></button>You have selected {racks} racks worth {reckPrice:C}.<br /> You can still add <strong>{theRack.MaximumItems - trays} more trays </strong> into this racks.</div>";
                }
            }
            ScriptDIV.InnerHtml = "";
            ScriptDIV.InnerHtml = $@"<script> $(document).ready(function(){{ 
                                                var updatePrice=$('.R{rID.Value}').parent().find('.update-price-btn');
                                                updatePrice.show();
                                                $('.{rID.Value}').collapse('show');
                                                $('.S{rID.Value}').text('{reckPrice:C}');
                                                $('.R{rID.Value}').show();
                                                $('.C{rID.Value}').hide();
                                                $('.clear-{rID.Value}').show();
                                                var resetButton=$('.R{rID.Value}');
                                                $('html, body').animate({{
                                                    scrollTop: $('.{rID.Value}').find('table').height() + $('.S{rID.Value}').offset().top
                                                }})
                                             }}) </script>";
            currentReckPrice.Value = reckPrice.ToString("#.##");
        }

        protected void rptRacks_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                using (MadduxEntities db = new MadduxEntities())
                {
                    CatalogRackModel itemRow = (CatalogRackModel)e.Item.DataItem;

                    int rackID = Convert.ToInt32(itemRow.RackID);
                    bool allowCustomization = Convert.ToBoolean(itemRow.AllowCustomization);
                    ProductCatalogRack theRack = db.ProductCatalogRacks.Include(x => x.RackProducts.Select(z => z.Product.Photos)).FirstOrDefault(r => r.RackID == rackID);

                    HtmlInputHidden hfRackName = e.Item.FindControl("hfRackName") as HtmlInputHidden;

                    Label lblShipDates = e.Item.FindControl("lblShipDates") as Label;
                    Panel pnlNotAvailable = e.Item.FindControl("pnlNotAvailable") as Panel;
                    Panel pnlCustomize = e.Item.FindControl("pnlCustomize") as Panel;
                    Repeater rptProducts = e.Item.FindControl("rptProducts") as Repeater;
                    Button btnChooseStores = e.Item.FindControl("btnChooseStores") as Button;
                    HiddenField hiddenField = e.Item.FindControl("KeepThisExpanded") as HiddenField;
                    hiddenField.Value = "R" + rackID;
                    Button CustomizeBtn = e.Item.FindControl("CustomizeBtn") as Button;
                    Button btnReset = e.Item.FindControl("btnReset") as Button;
                    btnReset.CssClass = btnReset.CssClass + " R" + rackID;
                    Label lblReckSizeInfo = e.Item.FindControl("lblReckSizeInfo") as Label;
                    Label lblRackPrice = e.Item.FindControl("lblRackPrice") as Label;
                    double totalQty = 0;
                    foreach (RackProduct product in theRack.RackProducts)
                    {
                        if (!string.IsNullOrWhiteSpace(product.Product.UPCCode))
                        {
                            totalQty += product.DefaultQuantity;
                        }
                    }
                    foreach (RackRack product in theRack.RackRacks)
                    {
                        totalQty += product.DefaultQuantity;
                    }

                    if ((totalQty > theRack.MinimumItems.GetValueOrDefault(0) && totalQty < theRack.MaximumItems) || totalQty == theRack.MaximumItems || totalQty == theRack.MinimumItems)
                    {
                        btnChooseStores.Enabled = true;
                    }
                    else if (theRack.MaximumItems > totalQty)
                    {
                        double remainder = (int)totalQty % theRack.MaximumItems.GetValueOrDefault(1);
                        btnChooseStores.Enabled = true;
                        if (remainder < theRack.MinimumItems)
                        {
                            btnChooseStores.Enabled = false;
                            if (theRack.AllowCustomization == false)
                            {
                                lblReckSizeInfo.Text = $"* Rack doesn't have enough product assigned to the Rack. Please Contact Administrator.";
                                lblReckSizeInfo.CssClass = "text-danger";
                            }
                        }
                    }
                    else
                    {
                        btnChooseStores.Enabled = false;
                        if (theRack.AllowCustomization == false)
                        {
                            lblReckSizeInfo.Text = $"* Rack doesn't have enough product assigned to the Rack. Please Contact Administrator.";
                            lblReckSizeInfo.CssClass = "text-danger";
                        }
                    }
                    if (theRack.RackSize != null)
                    {

                        if (theRack.MinimumItems == theRack.MaximumItems)
                        {
                            lblReckSizeInfo.Text = $"* You need minimum {theRack.MinimumItems} trays for a rack.";
                        }

                        if (theRack.MinimumItems < theRack.MaximumItems)
                        {
                            lblReckSizeInfo.Text = $"* You can add minimum {theRack.MinimumItems} trays and maximum {theRack.MaximumItems} trays in a rack.";
                        }
                    }
                    lblRackPrice.Attributes.Add("class", "input-form-label text-right S" + theRack.RackID);

                    lblRackPrice.Text = Convert.ToDecimal(itemRow.Price).ToString("C2");

                    List<ProductCatalogShipDate> shipDatesTable = theRack.ProductCatalog.FutureShipDatesForCatch;
                    ProductCatalogShipDates.Add(rackID, shipDatesTable);
                    rptShipDates.DataSource = ProductCatalogShipDates;
                    rptShipDates.DataBind();
                    lblShipDates.Text = string.Join(", ", shipDatesTable.Select(r => r.FormattedShipDateLong));

                    if (shipDatesTable.Count > 0)
                    {
                        pnlNotAvailable.Visible = false;
                    }
                    else
                    {
                        btnChooseStores.Enabled = false;
                        pnlNotAvailable.Visible = true;
                    }

                    if (!allowCustomization)
                    {
                        pnlCustomize.Visible = false;
                        CustomizeBtn.Visible = false;
                    }
                    else
                    {
                        CustomizeBtn.Visible = true;
                        CustomizeBtn.CssClass = $@"btn btn-success customize-btn C{theRack.RackID}";
                    }


                    var items = new List<CatalogRackItem>();

                    if (theRack.RackType == (int)RackType.Standard)
                    {
                        items.AddRange(theRack.RackProducts.Select(rp => new CatalogRackItem
                        {
                            IsFirstInRack = true,
                            RackID = rp.RackID,
                            ProductID = rp.ProductID,
                            UnitPrice = rp.UnitPrice,
                            HasProductPhoto = rp.HasProductPhoto,
                            ProductPhotoPath = rp.ProductPhotoPath,
                            ProductName = rp.ProductName,
                            //UnitPriceFormatted =  (rp.UnitPrice / rp.UnitPerCase).ToString("C"),
                            UnitPriceFormatted = OrderHelper.CalculateEachPrice(rp.UnitPrice, theRack.Discount ?? 0, rp.UnitPerCase).ToString("C"),
                            UnitPerCase = rp.UnitPerCase,
                            DefaultQuantity = rp.DefaultQuantity,
                            ProductUPCCode = rp.Product.UPCCode,
                            IsBulkRack = false
                        }).OrderBy(rp => rp.ProductName).ToList());
                    }
                    else
                    {
                        var allRacks = theRack.RackRacks.ToList();

                        var rackCount = 0;
                        foreach (var productRack in allRacks)
                        {
                            rackCount++;
                            int count = 0;
                            foreach (var rp in productRack.ProductRack.RackProducts)
                            {
                                count++;
                                var model = new CatalogRackItem
                                {
                                    IsFirstInRack = count == 1,
                                    RackContainerMargin = (count == 1 && rackCount > 1) ? "margin-top: 50px" : "",
                                    RackID = rp.RackID,
                                    RackRackID = productRack.RackID,
                                    ProductID = rp.ProductID,
                                    UnitPrice = rp.UnitPrice,
                                    HasProductPhoto = rp.HasProductPhoto,
                                    ProductPhotoPath = rp.ProductPhotoPath,
                                    ProductName = rp.ProductName,
                                    UnitPriceFormatted = OrderHelper.CalculateEachPrice(rp.UnitPrice, theRack.Discount ?? 0, rp.UnitPerCase).ToString("C"),
                                    UnitPerCase = rp.UnitPerCase,
                                    DefaultQuantity = productRack.DefaultQuantity, // NOTE: For bulk racks, we will set this to the rack default quantity instead of product quantity
                                    ProductUPCCode = rp.Product.UPCCode,
                                    IsBulkRack = true
                                };
                                items.Add(model);
                            }
                        }
                    }

                    rptProducts.DataSource = items;
                    rptProducts.DataBind();

                    hfRackName.Value = theRack.RackName.Replace("\"", " inch");
                    hfRackName.Attributes.Add("tag", "h" + rackID.ToString());
                    if (theRack.AllowCustomization == false)
                    {
                        HideScripts.InnerHtml += $@"<script>
                            $('.C{theRack.RackID}').hide();
                        </script>";
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void rptShipDates_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            int rackID = ((KeyValuePair<int, List<ProductCatalogShipDate>>)e.Item.DataItem).Key;
            List<ProductCatalogShipDate> shipDates = ((KeyValuePair<int, List<ProductCatalogShipDate>>)e.Item.DataItem).Value;
            CheckBoxList lstShipDateSelector = e.Item.FindControl("lstShipDateSelector") as CheckBoxList;
            lstShipDateSelector.DataValueField = "ShipDate";
            lstShipDateSelector.DataTextField = "FormattedShipDateLong";
            lstShipDateSelector.DataSource = shipDates;
            lstShipDateSelector.DataBind();


            lstShipDateSelector.Attributes.Add("tag", "sd" + rackID.ToString());
        }

        protected void rptStores_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

        }
        protected void btnPlaceOrder_Click(object sender, EventArgs e)
        {
            try
            {
                bool error = false;
                string errorMessage = "";
                Button cmdPlaceOrder = (Button)sender;
                int rackID = Convert.ToInt32(hdnRackID.Value);
                Customer theCustomer = currentCustomer;
                Control ee = cmdPlaceOrder.Parent.FindControl("pnlPrograms");
                RepeaterItem rptRack = null;
                foreach (RepeaterItem item in rptRacks.Items)
                {
                    HiddenField rackId = item.FindControl("RackID") as HiddenField;
                    if (int.Parse(rackId.Value) == rackID)
                    {
                        rptRack = item;
                    }
                }



                List<DateTime> selectedShipDates = new List<DateTime>();

                foreach (RepeaterItem shipDate in rptShipDates.Items)
                {
                    CheckBoxList lstShipDateSelector = shipDate.FindControl("lstShipDateSelector") as CheckBoxList;

                    foreach (ListItem item in lstShipDateSelector.Items)
                    {
                        if (item.Selected)
                        {
                            selectedShipDates.Add(DateTime.Parse(item.Value));
                        }
                    }
                }
                errorMessage = string.Empty;
                if (selectedShipDates.Count == 0)
                {
                    error = true;
                    errorMessage += "No Ship date is selected for order! <br />";
                }
                bool IsStoreSelected = false;
                foreach (RepeaterItem _store in rptStores.Items)
                {
                    CheckBox selected = _store.FindControl("chkStoreSelected") as CheckBox;
                    TextBox txtRackQuantity = _store.FindControl("txtRackQuantity") as TextBox;
                    int rackQuantity = int.Parse(txtRackQuantity.Text);
                    if (rackQuantity > 0)
                    {
                        IsStoreSelected = true;
                        Label StoreName = _store.FindControl("StoreName") as Label;
                        int quantity = int.Parse(txtRackQuantity.Text);
                        if (quantity <= 0)
                        {
                            error = true;
                            errorMessage += $@"Rack quantity can not be 0 for selected store ({StoreName.Text}) <br />";
                        }
                    }
                }
                if (!IsStoreSelected)
                {
                    error = true;
                    errorMessage += "No Store is selected for order! <br />";
                }
                if (!IsStoreSelected && selectedShipDates.Count == 0)
                {
                    error = true;
                    errorMessage += "No Store and ShipDate is selected for order! <br />";

                }

                Control pnlCustom = rptRack.FindControl("pnlCustomize");
                Repeater rptProducts = pnlCustom.FindControl("rptProducts") as Repeater;
                if (rptProducts.Items.Count == 0)
                {
                    error = true;
                    errorMessage += "There is no products in the rack! Please contact Administrator for details. <br />";
                }
                Repeater stores = rptStores;
                if (error == false)
                {
                    using (MadduxEntities db = new MadduxEntities())
                    {

                        var parentRack = db.ProductCatalogRacks.Where(r => r.RackID == rackID).FirstOrDefault();
                        RackType rackType = (RackType)parentRack.RackType;

                        foreach (RepeaterItem store in rptStores.Items)
                        {
                            CheckBox selected = store.FindControl("chkStoreSelected") as CheckBox;
                            TextBox rackQuantityText = store.FindControl("txtRackQuantity") as TextBox;
                            int? bulkRackId = null;
                            Guid? bulkOrderKey = null;

                            int rackQuantity = int.Parse(rackQuantityText.Text);
                            if (rackQuantity > 0)
                            {
                                HiddenField hdnCompanyId = store.FindControl("hdnCompanyId") as HiddenField;
                                TextBox txtRackQuantity = store.FindControl("txtRackQuantity") as TextBox;
                                int quantity = int.Parse(txtRackQuantity.Text);
                                if (int.TryParse(hdnCompanyId.Value, out int companyId))
                                {
                                    Customer customer = db.Customers.FirstOrDefault(x => x.CustomerId == companyId);
                                    if (string.IsNullOrWhiteSpace(customer.Zip) || string.IsNullOrWhiteSpace(customer.Company) || string.IsNullOrWhiteSpace(customer.Address) || string.IsNullOrWhiteSpace(customer.State) || string.IsNullOrWhiteSpace(customer.Country) || string.IsNullOrWhiteSpace(customer.City))
                                    {
                                        throw new Exception($@"Shipping information is required for {customer.Company}!");
                                    }
                                    if (string.IsNullOrWhiteSpace(customer.BillingZip) || string.IsNullOrWhiteSpace(customer.BillingCompany) || string.IsNullOrWhiteSpace(customer.BillingAddress) || string.IsNullOrWhiteSpace(customer.BillingState) || string.IsNullOrWhiteSpace(customer.BillingCountry) || string.IsNullOrWhiteSpace(customer.BillingCity))
                                    {
                                        throw new Exception($@"Billing information is required for {customer.Company}!");
                                    }

                                    foreach (DateTime selectedShipDate in selectedShipDates)
                                    {
                                        for (int i = 0; i < quantity; i++)
                                        {

                                            List<OrderRack> racks = new List<OrderRack>();

                                            if (rackType == RackType.Standard)
                                            {
                                                OrderRack orderRack = new OrderRack
                                                {
                                                    Quantity = 1,
                                                    RackId = rackID,
                                                };

                                                List<OrderItem> items = new List<OrderItem>();
                                                int itemCount = 0;
                                                foreach (RepeaterItem product in rptProducts.Items)
                                                {
                                                    TextBox txtQuantity = product.FindControl("txtProductQuantity") as TextBox;
                                                    HiddenField productID = product.FindControl("hdnProductID") as HiddenField;
                                                    HiddenField productPrice = product.FindControl("hdnUnitPrice") as HiddenField;
                                                    HiddenField hdnUPCCode = product.FindControl("hdnUPCCode") as HiddenField;
                                                    if (int.Parse(txtQuantity.Text) > 0)
                                                    {
                                                        if (!string.IsNullOrWhiteSpace(hdnUPCCode.Value))
                                                        {
                                                            itemCount += int.Parse(txtQuantity.Text);
                                                        }
                                                        items.Add(new OrderItem
                                                        {
                                                            Quantity = int.Parse(txtQuantity.Text),
                                                            ProductId = int.Parse(productID.Value),
                                                            DiscountPercent = 0,
                                                            LinePosition = 0,
                                                            NoGlobalDiscount = true,
                                                            ProductNotAvailable = false,
                                                            ProductNotAvailableDesc = "",
                                                            UnitPrice = double.Parse(productPrice.Value)

                                                        });
                                                    }
                                                }
                                                List<OrderItem> expandedItems = new List<OrderItem>();
                                                foreach (OrderItem item in items)
                                                {
                                                    for (int x = 0; x < item.Quantity; x++)
                                                    {
                                                        expandedItems.Add(new OrderItem
                                                        {
                                                            Quantity = 1,
                                                            ProductId = item.ProductId,
                                                            DiscountPercent = 0,
                                                            LinePosition = 0,
                                                            NoGlobalDiscount = true,
                                                            ProductNotAvailable = false,
                                                            ProductNotAvailableDesc = "",
                                                            UnitPrice = item.UnitPrice
                                                        });
                                                    }
                                                }

                                                ProductCatalogRack rackModel = db.ProductCatalogRacks.FirstOrDefault(r => r.RackID == orderRack.RackId);
                                                if (rackModel.RackSize != null && (itemCount < rackModel.MinimumItems || itemCount > rackModel.MaximumItems))
                                                {
                                                    if (itemCount < rackModel.MinimumItems)
                                                    {
                                                        error = true;
                                                        errorMessage = "The number of trays is less than the minimum number of trays (" + rackModel.MinimumItems + ") for this rack.";
                                                    }
                                                    if (itemCount > rackModel.MaximumItems)
                                                    {

                                                        int _min = Convert.ToInt32(rackModel.MinimumItems);
                                                        int _max = Convert.ToInt32(rackModel.MaximumItems);
                                                        Dictionary<int, int> _racks = new Dictionary<int, int>();

                                                        //get the number of remaining products after creating racks with it's maximum capacity
                                                        int remainingItems = itemCount % _max;
                                                        //get number of rack that can be ordered with it's maximum capacity
                                                        int numberOfRacks = (itemCount - remainingItems) / _max;

                                                        for (int _rack = 1; _rack < numberOfRacks + 1; _rack++)
                                                        {
                                                            _racks.Add(_rack, _max);
                                                        }


                                                        //check if number of remaining items is enough for rack with minimum number of items
                                                        if (_min < remainingItems)
                                                        {
                                                            _racks.Add(numberOfRacks + 1, remainingItems);
                                                        }
                                                        else
                                                        {
                                                            //if not check if by removing one one rack with it's maximum capacity,
                                                            //by adding those items in remining items and devide it by 2 and check if result is greater than minimum number of items
                                                            remainingItems += _max;
                                                            double rItems = remainingItems / 2;
                                                            if (rItems >= _min)
                                                            {
                                                                //if yes than create two rack with result
                                                                if (remainingItems % 2 == 0)
                                                                {
                                                                    _racks[numberOfRacks] = remainingItems / 2;
                                                                    _racks.Add(numberOfRacks + 1, remainingItems / 2);
                                                                }
                                                                else
                                                                {
                                                                    _racks[numberOfRacks] = remainingItems / 2;
                                                                    _racks.Add(numberOfRacks + 1, (remainingItems / 2) + 1);
                                                                }
                                                                //_racks[numberOfRacks]=

                                                            }
                                                            else
                                                            {
                                                                //throw error
                                                                error = true;
                                                                errorMessage = "The number of trays ordered is more than the maximum for a single rack, but less than the minimum for an additional rack.";
                                                                break;
                                                            }
                                                        }
                                                        if (!error)
                                                        {
                                                            for (int _r = 1; _r < _racks.Count + 1; _r++)
                                                            {
                                                                OrderRack _orderRack = new OrderRack
                                                                {
                                                                    Quantity = 1,
                                                                    RackId = rackID
                                                                };
                                                                int rackSize = _racks[_r];
                                                                for (int _rackItem = 0; _rackItem < rackSize; _rackItem++)
                                                                {
                                                                    OrderItem existingProduct = _orderRack.OrderItems.FirstOrDefault(r => r.ProductId == expandedItems[_rackItem].ProductId);
                                                                    if (existingProduct != null)
                                                                    {
                                                                        existingProduct.Quantity += 1;
                                                                    }
                                                                    else
                                                                    {
                                                                        _orderRack.OrderItems.Add(expandedItems[_rackItem]);
                                                                    }
                                                                }
                                                                racks.Add(_orderRack);
                                                                expandedItems.RemoveRange(0, rackSize);
                                                            }
                                                        }

                                                    }
                                                }
                                                else
                                                {
                                                    foreach (OrderItem item in expandedItems)
                                                    {
                                                        OrderItem existingProduct = orderRack.OrderItems.FirstOrDefault(r => r.ProductId == item.ProductId);
                                                        if (existingProduct != null)
                                                        {
                                                            existingProduct.Quantity += 1;
                                                        }
                                                        else
                                                        {
                                                            orderRack.OrderItems.Add(item);
                                                        }
                                                    }

                                                    racks.Add(orderRack);
                                                }
                                            }
                                            else // BULK RACK
                                            {
                                                bulkOrderKey = Guid.NewGuid();
                                                bulkRackId = parentRack.RackID;
                                                foreach (RepeaterItem rack in rptProducts.Items)
                                                {
                                                    HiddenField isFirstRackItem = ((Control)rack).FindControl("hdnIsFirstTime") as HiddenField;

                                                    // For bulk rack, we only grab the first item of each rack as it has the quantity we want
                                                    if (isFirstRackItem.Value != true.ToString()) continue;

                                                    TextBox txtQuantity = rack.FindControl("txtProductQuantity") as TextBox;

                                                    // This is the rack within the rack
                                                    HiddenField rackRackID = rack.FindControl("RackID") as HiddenField;

                                                    var theRack = db.ProductCatalogRacks.Where(x => x.RackID.ToString() == rackRackID.Value).FirstOrDefault();

                                                    for (var j = 1; j <= Int32.Parse(txtQuantity.Text); j++)
                                                    {
                                                        OrderRack orderRack = new OrderRack
                                                        {
                                                            Quantity = 1,
                                                            RackId = theRack.RackID
                                                        };

                                                        List<OrderItem> items = new List<OrderItem>();

                                                        foreach (RackProduct product in theRack.RackProducts)
                                                        {
                                                            items.Add(new OrderItem
                                                            {
                                                                Quantity = product.DefaultQuantity,
                                                                ProductId = product.ProductID,
                                                                DiscountPercent = parentRack.Discount ?? 0,
                                                                LinePosition = 0,
                                                                NoGlobalDiscount = true,
                                                                ProductNotAvailable = false,
                                                                ProductNotAvailableDesc = "",
                                                                UnitPrice = product.UnitPrice

                                                            });
                                                        }

                                                        foreach (OrderItem item in items)
                                                        {
                                                            OrderItem existingProduct = orderRack.OrderItems.FirstOrDefault(r => r.ProductId == item.ProductId);
                                                            if (existingProduct != null)
                                                            {
                                                                existingProduct.Quantity += 1;
                                                            }
                                                            else
                                                            {
                                                                orderRack.OrderItems.Add(item);
                                                            }
                                                        }

                                                        List<OrderItem> expandedItems = new List<OrderItem>();
                                                        foreach (OrderItem item in items)
                                                        {
                                                            for (int x = 0; x < item.Quantity; x++)
                                                            {
                                                                expandedItems.Add(new OrderItem
                                                                {
                                                                    Quantity = 1,
                                                                    ProductId = item.ProductId,
                                                                    DiscountPercent = 0,
                                                                    LinePosition = 0,
                                                                    NoGlobalDiscount = true,
                                                                    ProductNotAvailable = false,
                                                                    ProductNotAvailableDesc = "",
                                                                    UnitPrice = item.UnitPrice
                                                                });
                                                            }
                                                        }

                                                        racks.Add(orderRack);
                                                    }
                                                }
                                            }


                                            if (error == true)
                                            {
                                                lbError.Text = StringTools.GenerateError(errorMessage);
                                            }
                                            else
                                            {
                                                //Order _oldOrder = db.Orders.Include(x => x.OrderRacks).FirstOrDefault(x => x.OrderID == OrderID);


                                                string _CreatedBy = AppSession.Current.CurrentUser.EmailAddress.Trim();

                                                int ShippingMethodID = db.supShippingMethods.Where(x => x.ShippingMethodDesc.StartsWith("LTL")).Select(x => x.ShippingMethodID).FirstOrDefault();

                                                var _state = db.States.FirstOrDefault(x => x.StateID == theCustomer.State);

                                                foreach (OrderRack item in racks)
                                                {
                                                    double freightCharge = FreightCalculator.CalculateFreighCharge(item.GetRackTotal, _state.StateID, customer.Zip);
                                                    ProductRackShipDate productRackShipDates = db.ProductRackShipDates
                                                       .Where(r => selectedShipDate == r.ShipDate && r.RackID == item.RackId && r.Active)
                                                       .FirstOrDefault();
                                                    if (productRackShipDates != null)
                                                    {
                                                        if (productRackShipDates.ProperOrderDeadlineDate < DateTime.Now)
                                                        {
                                                            int availability = productRackShipDates.Available;
                                                            productRackShipDates.Available = availability > 0 ? availability - 1 : 0;
                                                        }
                                                    }
                                                    Order order = new Order
                                                    {
                                                        BulkOrderKey = bulkOrderKey,
                                                        BulkRackID = bulkRackId,
                                                        OrderStatus = int.Parse(ddOrderStatus.SelectedValue),
                                                        CustomerID = companyId,
                                                        RequestedShipDate = selectedShipDate,
                                                        BillingAddress = customer.BillingAddress,
                                                        BillingCity = customer.BillingCity,
                                                        BillingCountry = customer.BillingCountry,
                                                        BillingName = customer.BillingCompany,
                                                        BillingState = customer.BillingState,
                                                        BillingZip = customer.BillingZip,
                                                        DateCreated = DateTime.Now,
                                                        DateUpdated = DateTime.Now,
                                                        CreatedBy = _CreatedBy,
                                                        UpdatedBy = "",
                                                        ShippingAddress = customer.Address,
                                                        ConfirmationSentDate = null,
                                                        CustomShippingCharge = false,
                                                        GlobalDiscountDesc = txtDiscount1Desc.Text.Trim(),
                                                        GlobalDiscountPercent = (double)(Convert.ToDouble(FCSAppUtils.GetNumberString(txtDiscount1Pct.Text)) / 100),
                                                        GlobalDiscount2Desc = txtDiscount2Desc.Text.Trim(),
                                                        GlobalDiscount2Percent = (double)(Convert.ToDouble(FCSAppUtils.GetNumberString(txtDiscount2Pct.Text)) / 100),
                                                        GlobalDiscount3Desc = txtDiscount3Desc.Text.Trim(),
                                                        GlobalDiscount3Percent = (double)(Convert.ToDouble(FCSAppUtils.GetNumberString(txtDiscount3Pct.Text)) / 100),
                                                        GlobalDiscount4Desc = txtDiscount4Desc.Text.Trim(),
                                                        GlobalDiscount4Percent = (double)(Convert.ToDouble(FCSAppUtils.GetNumberString(txtDiscount4Pct.Text)) / 100),
                                                        GlobalDiscount5Desc = txtDiscount5Desc.Text.Trim(),
                                                        GlobalDiscount5Percent = (double)(Convert.ToDouble(FCSAppUtils.GetNumberString(txtDiscount5Pct.Text)) / 100),
                                                        GSTAmount = Convert.ToDecimal(TaxUtilities.GetTaxPercentage(_state.StateID, DateTime.Now) * (item.GetRackTotal + freightCharge)),
                                                        HST = _state.HST,
                                                        OfficeNotes = null,
                                                        OrderNotes = null,
                                                        PONumber = txtPO.Text.Trim(),
                                                        OrderDate = DateTime.Now,
                                                        QuoteDate = null,
                                                        PSTAmount = 0,
                                                        PSTExempt = false,
                                                        PurchaseOrdersSentDate = null,
                                                        QuoteExpiryDate = null,
                                                        ShipDate = null,
                                                        SalesPersonID = customer.SalesPersonID.GetValueOrDefault(0),
                                                        PaymentTermsID = customer.DefaultTermsId,
                                                        ShippingMethodID = ShippingMethodID,
                                                        PaymentTypeID = 1,
                                                        ShippingCharge = Convert.ToDecimal(freightCharge),
                                                        ShippingCity = customer.City,
                                                        ShippingCountry = customer.Country,
                                                        ShippingEmail = customer.Email,
                                                        ShippingName = customer.Company,
                                                        ShippingState = customer.State,
                                                        ShippingZip = customer.Zip,
                                                        Approved = ddlOrderApproved.SelectedValue == "2" ? true : false,
                                                        ReceivedVia = Convert.ToInt32(ddlOrderReceivedVia.SelectedValue)
                                                    };
                                                    order.OrderRacks.Add(item);
                                                    db.Orders.Add(order);
                                                    db.SaveChanges();
                                                    OrderID = order.OrderID;
                                                    order.CalculateFreightAndTaxes();
                                                }
                                                db.SaveChanges();

                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (error == false)
                {
                    Response.Redirect($@"/order/orderdetail.aspx?id={OrderID}&oSuccess=true");

                }
                else
                {
                    lbError.Text = StringTools.GenerateError(errorMessage);
                }
            }
            catch (Exception ex)
            {

                lbError.Text = StringTools.GenerateError(ex.Message);
            }
        }
        /// <summary>
        /// Populates a dropdown list from enum values
        /// </summary>
        /// <param name="dropDownList">The <see cref="DropDownList"/> to populate</param>
        /// <param name="options">The <see cref="string"/>to populate dropdown with</param>
        /// <param name="message">The <see cref="string"/>message for the select option</param>
        /// <param name="selectValue">The <see cref="string"/>value of the select from dropdown option</param>
        private void PopulateDropDownFromEnum(DropDownList dropDownList, string[] options, string message, string selectValue)
        {
            for (int i = 0; i <= options.Length - 1; i++)
            {
                ListItem item = new ListItem { Text = options[i], Value = (i + 1).ToString() };
                dropDownList.Items.Add(item);
            }
            dropDownList.Items.Insert(0, new ListItem { Text = message, Value = selectValue });
        }
        /// <summary>
        /// Reset  order values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnReset_Click(object sender, EventArgs e)
        {
            double reckPrice = 0;
            int trays = 0;
            Repeater repeater = ((Control)sender).Parent.FindControl("rptProducts") as Repeater;
            HtmlGenericControl divQtyWarning = ((Control)sender).Parent.FindControl("DivRackQtyWarning") as HtmlGenericControl;

            HiddenField rID = ((Control)sender).FindControl("RackID") as HiddenField;
            Button btnChooseStores = ((Control)sender).FindControl("btnChooseStores") as Button;

            ProductCatalogRack theRack = null;
            int rackID = int.Parse(rID.Value);
            using (MadduxEntities db = new MadduxEntities())
            {
                theRack = db.ProductCatalogRacks.Include(pc => pc.RackProducts).FirstOrDefault(r => r.RackID == rackID);
            }
            foreach (object item in repeater.Items)
            {
                HiddenField hdnProductID = ((Control)item).FindControl("hdnProductID") as HiddenField;
                TextBox textBox = ((Control)item).FindControl("txtProductQuantity") as TextBox;
                int ProductID = Convert.ToInt16(hdnProductID.Value);
                var product = theRack.RackProducts.FirstOrDefault(rp => rp.ProductID == ProductID);
                HiddenField hdnUPCCode = ((Control)item).FindControl("hdnUPCCode") as HiddenField;
                if (product != null)
                {
                    int qty = (int)product.DefaultQuantity;
                    double uPrice = 0;

                    textBox.Text = qty.ToString();
                    if (textBox != null)
                    {
                        qty = Convert.ToInt32(textBox.Text);
                        if (qty > 0)
                        {
                            if (!string.IsNullOrWhiteSpace(hdnUPCCode.Value))
                            {
                                trays += qty;
                            }
                            HiddenField unitprice = ((Control)item).FindControl("hdnUnitPrice") as HiddenField;
                            uPrice = Convert.ToDouble(unitprice.Value);
                            reckPrice += qty * uPrice;
                        }
                    }
                }
                else
                {
                    textBox.Text = "0";
                }
            }

            int racks = 0;
            if (theRack.MinimumItems > trays)
            {
                btnChooseStores.Enabled = false;
                divQtyWarning.InnerHtml = $"<div class='alert alert-warning alert-dismissible fade in'  role='alert'> <button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button> You still need to add <strong> minimum {theRack.MinimumItems - trays} trays </strong> into rack.</div>";
            }
            else if (theRack.MaximumItems < trays)
            {
                int _min = Convert.ToInt32(theRack.MinimumItems);
                int _max = Convert.ToInt32(theRack.MaximumItems);
                //get number of rack that can be ordered with it's maximum capacity
                int numberOfRacks = trays / _max;

                //get the number of remaining products after creating racks with it's maximum capacity
                int remainingItems = trays % _max;

                //check if number of remaining items is enough for rack with minimum number of items
                if (remainingItems > 0)
                {
                    if (_min < remainingItems)
                    {
                        btnChooseStores.Enabled = true;
                        racks++;
                        divQtyWarning.InnerHtml = $"<div class='alert alert-info alert-dismissible fade in'  role='alert'> <button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button> You can still add <strong>{_max - remainingItems} more trays </strong> into additional racks.</div>";
                    }
                    else
                    {
                        //if not check if by removing one one rack with it's maximum capacity,
                        //by adding those items in remining items and devide it by 2 and check if result is greater than minimum number of items
                        remainingItems += _max;
                        double rItems = remainingItems / 2;
                        if (rItems >= _min)
                        {
                            //if yes than create two rack with result
                            btnChooseStores.Enabled = true;
                            racks++;
                            divQtyWarning.InnerHtml = $"<div class='alert alert-info alert-dismissible fade in'  role='alert'> <button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button> You can still add <strong>{(2 * _max) - remainingItems} more trays </strong> into additional racks.</div>";

                        }
                        else
                        {
                            //throw error
                            btnChooseStores.Enabled = false;
                            divQtyWarning.InnerHtml = $"<div class='alert alert-warning alert-dismissible fade in'  role='alert'> <button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button> You still need to add minimum <strong>{(2 * _min) - remainingItems} trays </strong>into rack for additional racks.</div>";
                        }
                    }
                }
                else
                {
                    btnChooseStores.Enabled = true;
                    racks++;
                    divQtyWarning.InnerHtml = $"<div class='alert alert-info alert-dismissible fade in'  role='alert'> <button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button> " +
                        $"You have selected {numberOfRacks} racks worth {reckPrice:C}. </div>";
                }
            }
            else if (theRack.MinimumItems < trays && theRack.MaximumItems > trays)
            {
                btnChooseStores.Enabled = true;
                divQtyWarning.InnerHtml = $"<div class='alert alert-info alert-dismissible fade in'  role='alert'> <button type='button' class='close' data-dismiss='alert' aria-label='Close'>" +
                    $"<span aria-hidden='true'>&times;</span></button> You can still add <strong>{theRack.MaximumItems - trays} more trays </strong> into additional racks.</div>";
            }
            else
            {
                btnChooseStores.Enabled = true;
                racks++;
                divQtyWarning.InnerHtml = $"<div class='alert alert-info alert-dismissible fade in'  role='alert'> <button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button> " +
                    $"You have selected {racks} racks worth {reckPrice:C}. </div>";
            }
            ScriptDIV.InnerHtml = "";
            ScriptDIV.InnerHtml = $@"<script> $(document).ready(function(){{ 
                                                var updatePrice=$('.R{rID.Value}').parent().find('.update-price-btn');
                                                updatePrice.show();
                                                $('.{rID.Value}').collapse('show');
                                                $('.S{rID.Value}').text('{reckPrice:C}');
                                                $('.R{rID.Value}').show();
                                                $('.C{rID.Value}').hide();
                                                $('.reset-btn').show();
                                                $('.detail-btn').hide();
                                                $('.clear-{rID.Value}').show();
                                                var resetButton=$('.R{rID.Value}');
                                                $('html, body').animate({{
                                                    scrollTop: $('.{rID.Value}').find('table').height() + $('.S{rID.Value}').offset().top
                                                }})
                                             }}) </script>";
            currentReckPrice.Value = reckPrice.ToString("#.##");
        }
    }
}