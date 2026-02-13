using log4net;
using Maddux.Pitch.LocalClasses;
//using FCS;
using Redbud.BL.DL;
using Redbud.BL.Resources;
using Redbud.BL.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using static Redbud.BL.DL.Customer;

namespace Maddux.Pitch
{
    public class ShipDateAndAvalibility
    {
        public int OrderID { get; set; }
        public int Avalibilty { get; set; }
        public int ShipDateID { get; set; }
    }

    public class ShipDatesWithRackID
    {
        public List<ProductCatalogShipDate> productCatalogShipDates { get; set; }
        public int RackID { get; set; }
    }



    public partial class cart : System.Web.UI.Page
    {
        private ILog _logger = LogManager.GetLogger(typeof(cart));

        private decimal grandTotal;
        private int totalOrders;
        private Dictionary<int, ShipDatesWithRackID> shipDatesWithRackID;
        private List<Guid> OrderKeys = new List<Guid>();

        protected void Page_Load(object sender, EventArgs e)
        {
            shipDatesWithRackID = new Dictionary<int, ShipDatesWithRackID>();
            try
            {
                if (!Page.IsPostBack)
                {
                    LoadCart(false);
                    if (Request.QueryString["oSuccess"] != null && string.Equals(Request.QueryString["oSuccess"], "true"))
                    {
                        litMessage.Text = GenerateSuccess("The rack(s) have been successfully added to your cart. Please review and submit to finalize your order.");
                        //
                    }
                    if (Request.QueryString["del"] != null && string.Equals(Request.QueryString["del"], "1"))
                    {
                        litMessage.Text = GenerateSuccess("All orders have been successfully deleted from your cart.");
                    }
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = GenerateError(ex.Message);
            }
        }

        private void LoadCart(bool updateCartItemCount)
        {
            try
            {
                totalOrders = 0;
                Customer theCustomer = AppSession.Current.CurrentCustomer;

                var draftOrdersTable = theCustomer.DraftOrders().OrderByDescending(x => x.RequestedShipDate).ToList();
                var draftSubOrdersTable = theCustomer.GetSubCustomersWithDraftOrders();

                int totalDraftOrders = 0;
                List<int> subCartTable = new List<int>();
                try
                {
                    subCartTable = theCustomer.GetSubCustomersWithDraftOrders(false).Select(x => x.CustomerID).Distinct().ToList();
                }
                catch (NullReferenceException ex)
                {
                }
                foreach (var query in subCartTable)
                {
                    using (MadduxEntities db = new MadduxEntities())
                    {
                        var cust = db.Customers.FirstOrDefault(x => x.CustomerId == query);
                        totalDraftOrders += cust.DraftOrders().Count;
                    }

                }

                rptOtherCart.DataSource = draftSubOrdersTable.GroupBy(qr => qr.CustomerID).Select(qr => qr.First()).ToList();
                rptOtherCart.DataBind();


                if (draftOrdersTable.Count + draftSubOrdersTable.Count == 0)
                {
                    divOtherCartOrders.Visible = false;
                    yesButton.Visible = false;
                    cmdSubmit.Visible = false;
                    cmdDelete.Visible = false;
                }
                else
                {

                    if (draftSubOrdersTable.Count == 0)
                    {
                        divOtherCartOrders.Visible = false;
                    }

                    if (draftOrdersTable.Count + draftSubOrdersTable.Count > 1)
                    {
                        cmdSubmit.Text = "Submit Order(s)";
                    }
                    else
                    {
                        cmdSubmit.Text = "Submit Order";
                    }

                    cmdSubmit.Visible = true;
                    cmdDelete.Visible = true;
                    yesButton.Visible = true;
                }


                if (updateCartItemCount)
                {
                    //Update the cart counter
                    System.Web.UI.HtmlControls.HtmlGenericControl supCartTotal = Master.FindControl("supCartTotal") as System.Web.UI.HtmlControls.HtmlGenericControl;
                    if (supCartTotal != null)
                    {
                        supCartTotal.InnerText = totalOrders.ToString();
                        if (totalOrders == 0)
                        {
                            supCartTotal.Visible = false;
                        }
                        else
                        {
                            supCartTotal.Visible = true;
                        }
                    }
                }
                if (totalDraftOrders == 0)
                {
                    cmdSubmit.Visible = false;
                    cmdDelete.Visible = false;
                    yesButton.Visible = false;
                    PONumber.Text = String.Empty;
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = GenerateError(ex.Message);
            }
        }

        protected void dgvCart_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    decimal rowTotal = 0;
                    if (decimal.TryParse(e.Row.Cells[4].Text.Replace("$", ""), out rowTotal))
                    {
                        totalOrders++;
                    }

                }
                else
                {
                    if (e.Row.RowType == DataControlRowType.Footer)
                    {
                        e.Row.TableSection = TableRowSection.TableFooter;
                    }
                    else
                    {
                        if (e.Row.RowType == DataControlRowType.Header)
                        {
                            e.Row.TableSection = TableRowSection.TableHeader;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = GenerateError(ex.Message);
            }
        }

        protected void dgvSubCart_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    decimal rowTotal = 0;
                    if (decimal.TryParse(e.Row.Cells[4].Text.Replace("$", ""), out rowTotal))
                    {
                        totalOrders++;
                    }

                    // Find the delete button in the current row
                    LinkButton deleteButton = (LinkButton)e.Row.FindControl("deleteButton");
                    deleteButton.Visible = true;
                    // Get the unique key value from the data item
                    var key = DataBinder.Eval(e.Row.DataItem, "BulkOrderKey");
                    if (key != null)
                    {
                        if (CheckIfKeyIsUnique((Guid)key))
                        {
                            e.Row.CssClass = "cart-grouped-item-first";
                        }
                        else
                        {
                            deleteButton.Visible = false;
                            e.Row.CssClass = "cart-grouped-item";
                        }
                    }

                }
                else
                {
                    if (e.Row.RowType == DataControlRowType.Footer)
                    {
                        e.Row.TableSection = TableRowSection.TableFooter;
                    }
                    else
                    {
                        if (e.Row.RowType == DataControlRowType.Header)
                        {
                            e.Row.TableSection = TableRowSection.TableHeader;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = GenerateError(ex.Message);
            }
        }

        protected void rptOtherCart_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                GridView dgvSubCart = e.Item.FindControl("dgvSubCart") as GridView;
                QueryResult itemRow = (QueryResult)e.Item.DataItem;
                using (MadduxEntities db = new MadduxEntities())
                {
                    Customer subCustomer = db.Customers.FirstOrDefault(r => r.CustomerId == itemRow.CustomerID);
                    var ordered = subCustomer.DraftOrders()
                        .OrderBy(x => x.OrderID)
                        .ThenBy(x => x.BulkOrderKey)
                        .ToList();
                    dgvSubCart.DataSource = ordered;
                    dgvSubCart.DataBind();
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = GenerateError(ex.Message);
            }
        }

        private bool CheckIfKeyIsUnique(Guid key)
        {
            if (OrderKeys.Contains(key))
            {
                return false;
            }

            OrderKeys.Add(key);
            return true;
        }

        protected void rptShipDates_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                int orderID = ((KeyValuePair<int, ShipDatesWithRackID>)e.Item.DataItem).Key;
                ShipDatesWithRackID shipDatesWithRackID = ((KeyValuePair<int, ShipDatesWithRackID>)e.Item.DataItem).Value;
                int rackID = shipDatesWithRackID.RackID;
                List<ProductCatalogShipDate> shipDates = shipDatesWithRackID.productCatalogShipDates;

                RadioButtonList lstShipDateSelector = e.Item.FindControl("lstShipDateSelector") as RadioButtonList;

                using (var db = new MadduxEntities())
                {
                    //Product rack available ship dates
                    var productRackShipDates = db.ProductRackShipDates.Where(prsd => prsd.RackID == rackID && prsd.ShipDate > DateTime.Now && prsd.Active)
                            .Select(prsd => new RackShipDate
                            {
                                ShipDate = prsd.ShipDate,
                                OrderDeadlineDate = prsd.ProductCatalogRack.ProductCatalog.ProductCatalogShipDates.FirstOrDefault(x => x.ShipDate == prsd.ShipDate).OrderDeadlineDate,
                                Available = prsd.Available
                            })
                            .OrderBy(prsd => prsd.ShipDate)
                            .ToList();
                    //.Select(prsd => new { ShipDate = prsd.ShipDate.ToString("MMMM d, yyyy") })
                    List<ShipdateObject> shipadates = new List<ShipdateObject>();
                    foreach (var date in productRackShipDates)
                    {
                        if (date.OrderDeadlineDate > DateTime.Now)
                        {
                            shipadates.Add(new ShipdateObject { ShipDate = date.ShipDate.ToString("MMMM d, yyyy") });
                        }
                        else
                        {
                            if (date.Available > 0)
                            {
                                shipadates.Add(new ShipdateObject { ShipDate = date.ShipDate.ToString("MMMM d, yyyy") });
                            }
                        }
                    }

                    lstShipDateSelector.DataValueField = "ShipDate";
                    lstShipDateSelector.DataTextField = "ShipDate";
                    lstShipDateSelector.DataSource = shipadates;
                    lstShipDateSelector.DataBind();

                    lstShipDateSelector.Attributes.Add("tag", "sd" + orderID.ToString());
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = GenerateError(ex.Message);
            }
        }


        protected void cmdReOrder_Click(object sender, EventArgs e)
        {
            try
            {
                //DateTime selectedShipDates = DateTime.MinValue;
                //foreach (RepeaterItem shipDate in rptShipDates.Items)
                //{
                //    RadioButtonList lstShipDateSelector = shipDate.FindControl("lstShipDateSelector") as RadioButtonList;

                //    foreach (ListItem item in lstShipDateSelector.Items)
                //    {
                //        if (item.Selected)
                //        {
                //            selectedShipDates = DateTime.Parse(item.Value);
                //            break;
                //        }
                //    }
                //}

                Button btn = (Button)sender;
                int orderID = Convert.ToInt32(btn.CommandArgument);

                int rackID;
                using (var db = new MadduxEntities())
                {
                    rackID = db.OrderRacks.Where(or => or.OrderId == orderID).Select(or => or.RackId).FirstOrDefault();
                    var order = db.OrderItems.Where(r => r.OrderId == orderID);
                    foreach (var item in order)
                    {
                        int productID = item.ProductId;
                        double quantity = item.Quantity;
                    }
                    Response.Redirect($"overview.aspx?orderid={orderID}");
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = GenerateError(ex.Message);
            }
        }


        protected void cmdSubmit_Click(object sender, EventArgs e)
        {

            try
            {
                Customer theCustomer = AppSession.Current.CurrentCustomer;

                using (var db = new MadduxEntities())
                {
                    IQueryable<Order> orders = db.Orders.Where(r => r.OrderStatus == 0 && r.CustomerID == theCustomer.CustomerId);
                    List<int> orderIDs = new List<int>();
                    Dictionary<int, int> shipdateAvalibiltyChecker = new Dictionary<int, int>();
                    foreach (var order in orders)
                    {
                        bool isBulkOrder = order.BulkRackID.HasValue && order.BulkRackID.HasValue;

                        foreach (var orderedRack in order.OrderRacks.ToList())
                        {
                            ProductRackShipDate productRackShipDates = isBulkOrder
                                ? db.ProductRackShipDates.Where(r => order.RequestedShipDate == r.ShipDate && r.RackID == order.BulkRackID && r.Active).FirstOrDefault()
                                : db.ProductRackShipDates.Where(r => order.RequestedShipDate == r.ShipDate && r.RackID == orderedRack.RackId && r.Active).FirstOrDefault();

                            if (productRackShipDates != null)
                            {
                                order.OrderStatus = 1;
                                order.OrderDate = DateTime.UtcNow;
                                orderIDs.Add(order.OrderID);
                                order.PONumber = PONumber.Text.Trim();
                                if (productRackShipDates.ProperOrderDeadlineDate < DateTime.Now)
                                {

                                    int availability = productRackShipDates.Available;
                                    bool exist = shipdateAvalibiltyChecker.Any(sac => sac.Key == productRackShipDates.ProductRackShipDateID);
                                    if (availability > 0)
                                    {
                                        if (exist)
                                        {
                                            int value = shipdateAvalibiltyChecker[productRackShipDates.ProductRackShipDateID];
                                            shipdateAvalibiltyChecker[productRackShipDates.ProductRackShipDateID] = value - 1;
                                        }
                                        else
                                        {
                                            shipdateAvalibiltyChecker.Add(productRackShipDates.ProductRackShipDateID, availability - 1);
                                        }
                                    }
                                    else
                                    {

                                        if (exist)
                                        {
                                            shipdateAvalibiltyChecker[productRackShipDates.ProductRackShipDateID] = 0;
                                        }
                                        else
                                        {
                                            shipdateAvalibiltyChecker.Add(productRackShipDates.ProductRackShipDateID, 0);
                                        }
                                    }
                                }

                            }
                        }
                    }
                    var subCustomers = db.CustomersSubs.Where(r => r.MasterCustomerID == theCustomer.CustomerId).Select(r => r.ChildCustomerID);

                    var subOrders = db.Orders.Where(r => r.OrderStatus == 0 && subCustomers.Contains(r.CustomerID));
                    foreach (var order in subOrders)
                    {
                        foreach (var orderedRack in order.OrderRacks.ToList())
                        {
                            ProductRackShipDate productRackShipDates = db.ProductRackShipDates
                                                        .Where(r => order.RequestedShipDate == r.ShipDate && r.RackID == orderedRack.RackId && r.Active)
                                                        .FirstOrDefault();
                            if (productRackShipDates != null)
                            {
                                order.OrderStatus = 1;
                                order.OrderDate = DateTime.UtcNow;
                                order.PONumber = PONumber.Text.Trim();
                                orderIDs.Add(order.OrderID);
                                if (productRackShipDates.ProperOrderDeadlineDate < DateTime.Now)
                                {

                                    int availability = productRackShipDates.Available;
                                    bool exist = shipdateAvalibiltyChecker.Any(sac => sac.Key == productRackShipDates.ProductRackShipDateID);
                                    if (availability > 0)
                                    {
                                        if (exist)
                                        {
                                            int value = shipdateAvalibiltyChecker[productRackShipDates.ProductRackShipDateID];
                                            shipdateAvalibiltyChecker[productRackShipDates.ProductRackShipDateID] = value - 1;
                                        }
                                        else
                                        {
                                            shipdateAvalibiltyChecker.Add(productRackShipDates.ProductRackShipDateID, availability - 1);
                                        }
                                    }
                                    else
                                    {
                                        if (exist)
                                        {
                                            shipdateAvalibiltyChecker[productRackShipDates.ProductRackShipDateID] = 0;
                                        }
                                        else
                                        {
                                            shipdateAvalibiltyChecker.Add(productRackShipDates.ProductRackShipDateID, 0);
                                        }
                                    }
                                }

                            }
                        }
                    }
                    List<int> shidateIDs = new List<int>();
                    if (shipdateAvalibiltyChecker.Count > 0)
                    {
                        shidateIDs = shipdateAvalibiltyChecker.Select(x => x.Key).ToList();
                    }
                    foreach (ProductRackShipDate shipdate in db.ProductRackShipDates.Where(sd => shidateIDs.Contains(sd.ProductRackShipDateID)))
                    {
                        if (shipdateAvalibiltyChecker[shipdate.ProductRackShipDateID] <= 0)
                        {
                            int rackID = shipdate.RackID;
                            foreach (var order in orders)
                            {
                                if (order.OrderRacks.FirstOrDefault().RackId == rackID)
                                {
                                    order.OfficeNotes = "NOTE: The order was placed after availability reached to 0!";
                                }
                            }
                            foreach (var order in subOrders)
                            {
                                if (order.OrderRacks.FirstOrDefault().RackId == rackID)
                                {
                                    order.OfficeNotes = "NOTE: The order was placed after availability reached to 0!";
                                }
                            }
                        }

                        shipdate.Available = shipdateAvalibiltyChecker[shipdate.ProductRackShipDateID];
                    }
                    int saved = db.SaveChanges();
                    if (saved > 0)
                    {
                        _logger.Debug("Calculate star rating");
                        // Calculate the star rating for each customer
                        DateTime cutoffDate = DateTime.Today.AddYears(-1);
                        DateTime threeYearCutOffDate = DateTime.Today.AddYears(-3);

                        Customer customer = db.Customers.Single(x => x.CustomerId == theCustomer.CustomerId);
                        _logger.Debug($"Customer found for id: {theCustomer.CustomerId}");

                        int? rating = customer.Rating;
                        //get all orders with status == 1 for this customer
                        List<Order> theCustomerOrders = db.Orders.Where(x => x.OrderStatus == 1 && x.CustomerID == customer.CustomerId).ToList();
                        _logger.Debug($"{theCustomerOrders.Count} order found.");

                        if (theCustomerOrders.Any(x => x.OrderDate >= threeYearCutOffDate))
                        {
                            _logger.Debug($"Order in past 3 years.");

                            decimal totalOrder = theCustomerOrders.Where(o => o.OrderDate >= cutoffDate).Sum(o => o.SubTotal);

                            if (totalOrder < 1500)
                                rating = 1;
                            else if (totalOrder < 4000)
                                rating = 2;
                            else if (totalOrder < 8000)
                                rating = 3;
                            else if (totalOrder < 15000)
                                rating = 4;
                            else
                                rating = 5;
                        }
                        else if (theCustomerOrders.Any())
                        {
                            _logger.Debug($"No Orders in past 3 years,  but customer has ordered in past.");

                            rating = 0;
                        }
                        else
                        {
                            _logger.Debug($"No orders found");
                            rating = null;
                        }

                        customer.StarRating = rating;
                        db.SaveChanges();

                        _logger.Debug("Calculating star rating for SubCustomers");

                        List<int> subIDs = customer.CustomersSubs.Select(x => x.ChildCustomerID).ToList();

                        if (subIDs.Any())
                        {
                            List<Order> subIDOrders = db.Orders.Where(x => subIDs.Contains(x.CustomerID)).ToList();
                            foreach (var customerSub in customer.CustomersSubs)
                            {
                                _logger.Debug($"Calculating star rating for id: {customerSub.Customer.CustomerId} name: {customerSub.Customer.Company}");
                                var subCustomer = db.Customers.Single(x => x.CustomerId == customerSub.ChildCustomerID);
                                var subCustomerOrders = subIDOrders.Where(x => x.CustomerID == subCustomer.CustomerId && x.OrderStatus == 1);
                                int? subRating = subCustomer.Rating;

                                _logger.Debug($"{subCustomerOrders.Count()} order found.");

                                if (subCustomerOrders.Any(x => x.OrderDate >= threeYearCutOffDate))
                                {
                                    _logger.Debug($"Order in past 3 years.");

                                    decimal totalOrder = subCustomerOrders.Where(o => o.OrderDate >= cutoffDate).Sum(o => o.SubTotal);

                                    if (totalOrder < 1500)
                                        subRating = 1;
                                    else if (totalOrder < 3999)
                                        subRating = 2;
                                    else if (totalOrder < 7999)
                                        subRating = 3;
                                    else if (totalOrder < 14999)
                                        subRating = 4;
                                    else
                                        subRating = 5;
                                }
                                else if (subCustomerOrders.Any())
                                {
                                    _logger.Debug($"No Orders in past 3 years,  but customer has ordered in past.");

                                    subRating = 0;
                                }
                                else
                                {
                                    _logger.Debug($"No orders found");

                                    subRating = null;
                                }
                                subCustomer.StarRating = subRating;
                            }
                            db.SaveChanges();
                        }
                        else
                        {
                            _logger.Debug("No SubCustomers found");
                        }
                        Emailer emailer = new Emailer();
                        string ids = string.Empty;
                        foreach (int id in orderIDs)
                        {
                            ids += id + ", ";
                        }
                        ids = ids.Substring(0, ids.Length - 2);
                        string body = string.Format(EmailerResources.OnlineOrderSubmissionBody, theCustomer.Company, ids) + "<br>" + EmailerResources.EmailFooter;
                        var cc = new List<string> { EmailerResources.OnlineOrderSubmissionCc };
                        if (theCustomer.AlternateEmailReceivesConfirmations && !string.IsNullOrEmpty(theCustomer.AlternateEmail))
                        {
                            cc.Add(theCustomer.AlternateEmail);
                        }
                        bool emailSent = true;
                        emailSent = emailer.SendEmailWithoutAttachment(
                                                           new List<string>() { theCustomer.Email },
                                                           string.Format(EmailerResources.OnlineOrderSubmissionSubject),
                                                           body,
                                                           cc
                                                        );
                        if (emailSent)
                        {
                            spnSuccessMessage.InnerText = "Your order(s) have been successfully submitted.";
                            pnlActivitySuccess.Visible = true;
                        }

                        Response.Redirect("/order.aspx?oSuccess=true");
                    }

                }
                LoadCart(true);
            }
            catch (Exception ex)
            {
                _logger.Error("Error submitting cart", ex);
                litMessage.Text = GenerateError(ex.Message);
            }
        }

        protected void cmdDelete_Click(object sender, EventArgs e)
        {
            try
            {
                Customer theCustomer = AppSession.Current.CurrentCustomer;

                using (var db = new MadduxEntities())
                {
                    // get primary customer orders
                    foreach (var item in db.Orders.Where(r => r.OrderStatus == 0 && r.CustomerID == theCustomer.CustomerId).ToList())
                    {
                        if (item.OrderRacks.Any())
                        {
                            foreach (var rack in item.OrderRacks.ToList())
                            {
                                foreach (var orderitem in rack.OrderItems.ToList())
                                {
                                    db.OrderItems.Remove(orderitem);
                                }
                                db.OrderRacks.Remove(rack);
                            }
                        }
                        else
                        {
                            foreach (var orderItem in item.OrderItems.ToList())
                            {
                                db.OrderItems.Remove(orderItem);
                            }
                        }

                        db.Orders.Remove(item);
                    }
                    var subCustomers = db.CustomersSubs.Where(r => r.MasterCustomerID == theCustomer.CustomerId).Select(r => r.ChildCustomerID);

                    foreach (var item in db.Orders.Where(r => r.OrderStatus == 0 && subCustomers.Contains(r.CustomerID)))
                    {
                        if (item.OrderRacks.Any())
                        {
                            foreach (var rack in item.OrderRacks.ToList())
                            {
                                foreach (var orderitem in rack.OrderItems.ToList())
                                {
                                    db.OrderItems.Remove(orderitem);
                                }
                                db.OrderRacks.Remove(rack);
                            }
                        }
                        else
                        {
                            foreach (var orderItem in item.OrderItems.ToList())
                            {
                                db.OrderItems.Remove(orderItem);
                            }
                        }

                        db.Orders.Remove(item);
                    }

                    db.SaveChanges();
                    spnSuccessMessage.InnerText = "All orders have been successfully deleted from your cart.";
                    pnlActivitySuccess.Visible = true;
                }

                Response.Redirect("cart.aspx?del=1", false);
            }
            catch (Exception ex)
            {
                litMessage.Text = GenerateError(ex.Message);
            }
        }

        private string GetSelectedOrders()
        {
            string orderIDs = "";

            try
            {
                if (rptOtherCart.Items.Count > 0)
                {
                    foreach (RepeaterItem rptItem in rptOtherCart.Items)
                    {
                        GridView dgvOtherCart = rptItem.FindControl("dgvSubCart") as GridView;

                        foreach (GridViewRow gridRow in dgvOtherCart.Rows)
                        {
                            CheckBox chkBox = (CheckBox)gridRow.FindControl("chkOrderSelect");
                            if (chkBox.Checked)
                            {
                                if (orderIDs.Length > 0)
                                {
                                    orderIDs += ",";
                                }
                                orderIDs += gridRow.Cells[1].Text.ToString();
                            }
                        }
                    }
                }

                return orderIDs;
            }
            catch (Exception ex)
            {
                litMessage.Text = GenerateError(ex.Message);
                return "";
            }
        }

        protected void dgvCart_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                var orderID = Convert.ToInt32(e.CommandArgument);

                try
                {
                    using (var db = new MadduxEntities())
                    {
                        var ord = db.Orders.FirstOrDefault(r => orderID == r.OrderID);
                        var ordersToRemove = new List<Order>();

                        if (ord.BulkOrderKey.HasValue)
                        {
                            ordersToRemove.AddRange(db.Orders.Where(r => r.BulkOrderKey.Value == ord.BulkOrderKey.Value).ToList());
                        }
                        else
                        {
                            ordersToRemove.Add(ord);
                        }

                        foreach (var order in ordersToRemove)
                        {

                            if (order.OrderRacks.Any())
                            {
                                foreach (var rack in order.OrderRacks.ToList())
                                {
                                    foreach (var orderitem in rack.OrderItems.ToList())
                                    {
                                        db.OrderItems.Remove(orderitem);
                                    }
                                    db.OrderRacks.Remove(rack);
                                }
                            }
                            else
                            {
                                foreach (var orderItem in order.OrderItems.ToList())
                                {
                                    db.OrderItems.Remove(orderItem);
                                }
                            }

                            db.Orders.Remove(order);
                        }

                        db.SaveChanges();
                        spnSuccessMessage.InnerText = "The selected order(s) have been successfully deleted from your cart.";
                        pnlActivitySuccess.Visible = true;

                    }
                    Response.Redirect("cart.aspx?del=1", false);
                    LoadCart(true);
                }
                catch (Exception ex)
                {
                    litMessage.Text = GenerateError(ex.Message);
                }

            }
        }

        private string GenerateError(string error)
        {
            return $@"<div class='alert alert-danger d-flex justify-content-between align-items-center'> 
                        <span >{error}</span>
                        <a class='close' data-bs-dismiss='alert' aria-label='Close'> 
                        <i class='far fa-times-circle fs-24 text-white'></i>
                        </a> 
                        </div>";
        }
        private string GenerateSuccess(string message)
        {
            return $@"<div class='alert alert-success d-flex justify-content-between align-items-center'>                        
                        <span>{message}</span>
                        <a  class='close' data-bs-dismiss='alert' aria-label='Close'> 
                            <i class='far fa-times-circle' aria-hidden='true'></i>
                        </a> 
                      </div>";
        }
    }
}