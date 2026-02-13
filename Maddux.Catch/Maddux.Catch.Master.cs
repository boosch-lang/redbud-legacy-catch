using Maddux.Catch.LocalClasses;
using Redbud.BL;
using Redbud.BL.DL;
using System;
using System.Web.UI;

namespace Maddux.Catch
{
    public partial class Catch : MasterPage
    {
        public string PageName;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    User currentUser = AppSession.Current.CurrentUser;

                    lblUserName.Text = currentUser.FullName;
                    litUserName.Text = currentUser.FullName;
                    litSidebarUserName.Text = currentUser.FullName;

                    //imgUserNavBar.ImageUrl = currentUser.ProfileImage;
                    //imgUserProfile.ImageUrl = currentUser.ProfileImage;
                    //imgUserSideBar.ImageUrl = currentUser.ProfileImage;

                    //Build the sidebar menu
                    //Add the Sales Header
                    litSidebarMenu.Text += "<li class=\"header\">SALES</li>";

                    if (currentUser.ShowCampaigns)
                    {
                        //Campaigns Sidebar Item
                        litSidebarMenu.Text += "<li ";
                        if (hfPageName.Value == "campaigns")
                        {
                            litSidebarMenu.Text += "class=\"active\"";
                        }
                        litSidebarMenu.Text += "><a href=\"" + ResolveUrl("~/campaign/campaigns.aspx")
                            + "\"><i class=\"fa fa-dashboard\"></i> <span>Campaigns</span>";
                        litSidebarMenu.Text += "<span class=\"pull-right-container\"><span class=\"label pull-right bg-red\">";
                        litSidebarMenu.Text += currentUser.TotalCampaigns.ToString(); ;
                        litSidebarMenu.Text += "</span></span>";
                        litSidebarMenu.Text += "</a></li>";
                    }

                    //Call list Sidebar Item
                    litSidebarMenu.Text += "<li ";
                    if (hfPageName.Value == "customerlist")
                    {
                        litSidebarMenu.Text += "class=\"active\"";
                    }
                    litSidebarMenu.Text += "><a href=\"" + ResolveUrl("~/customer/customerlist.aspx") + "\"><i class=\"fa fa-id-badge\"></i> <span>Customer List</span></a></li>";

                    //Followups Sidebar Item
                    litSidebarMenu.Text += "<li ";
                    if (hfPageName.Value == "followups")
                    {
                        litSidebarMenu.Text += "class=\"active\"";
                    }
                    litSidebarMenu.Text += "><a href=\"" + ResolveUrl("~/journal/myfollowups.aspx")
                        + "\"><i class=\"fa fa-calendar-check-o\"></i> <span>My Follow Ups</span>";
                    litSidebarMenu.Text += "<span class=\"pull-right-container\"><span class=\"label pull-right bg-red\">";
                    litSidebarMenu.Text += currentUser.TotalFollowups.ToString(); ;
                    litSidebarMenu.Text += "</span></span>";
                    litSidebarMenu.Text += "</a></li>";


                    //Draft Orders Sidebar Item
                    litSidebarMenu.Text += "<li ";
                    if (hfPageName.Value == "draftorders")
                    {
                        litSidebarMenu.Text += "class=\"active\"";
                    }
                    litSidebarMenu.Text += "><a href=\"" + ResolveUrl("~/order/myorders.aspx?page=1&type="
                        + Convert.ToInt32(OrderStatus.DraftOrders).ToString()) + "\"><i class=\"fa fa-ellipsis-h\"></i> <span>Draft Orders</span>";
                    litSidebarMenu.Text += "<span class=\"pull-right-container\"><span class=\"label pull-right bg-red\">";
                    litSidebarMenu.Text += currentUser.TotalDraftOrders.ToString();
                    litSidebarMenu.Text += "</span></span>";
                    litSidebarMenu.Text += "</a></li>";

                    //My Orders Sidebar Item
                    litSidebarMenu.Text += "<li ";
                    if (hfPageName.Value == "orders")
                    {
                        litSidebarMenu.Text += "class=\"active\"";
                    }
                    litSidebarMenu.Text += "><a href=\"" + ResolveUrl("~/order/myorders.aspx?page=1&type="
                        + Convert.ToInt32(OrderStatus.Orders).ToString()) + "\"><i class=\"fa fa-shopping-cart\"></i> <span>Orders</span>";
                    litSidebarMenu.Text += "<span class=\"pull-right-container\"><span class=\"label pull-right bg-red\">";
                    litSidebarMenu.Text += currentUser.TotalOrders.ToString();
                    litSidebarMenu.Text += "</span></span>";
                    litSidebarMenu.Text += "</a></li>";

                    if (currentUser.CanEditShipments)
                    {
                        //Purchase Orders Sidebar Item
                        litSidebarMenu.Text += "<li ";
                    if (hfPageName.Value == "purchaseorders")
                    {
                        litSidebarMenu.Text += "class=\"active\"";
                    }
                    litSidebarMenu.Text += "><a href=\"" + ResolveUrl("~/purchaseorder/purchaseorders.aspx")
                        + "\"><i class=\"fa fa-archive\"></i> <span>Purchase Orders</span>";
                    litSidebarMenu.Text += "<span class=\"pull-right-container\"><span class=\"label pull-right bg-red\">";
                    litSidebarMenu.Text += currentUser.TotalPurchaseOrders.ToString();
                    litSidebarMenu.Text += "</span></span>";
                    litSidebarMenu.Text += "</a></li>";

                        //Shipments Sidebar Item
                        litSidebarMenu.Text += "<li ";
                        if (hfPageName.Value == "shipments")
                        {
                            litSidebarMenu.Text += "class=\"active\"";
                        }
                        litSidebarMenu.Text += "><a href=\"" + ResolveUrl("~/shipping/myshipments.aspx")
                            + "\"><i class=\"fa fa-truck\"></i> <span>Shipments</span>";
                        litSidebarMenu.Text += "<span class=\"pull-right-container\"><span class=\"label pull-right bg-red\">";
                        litSidebarMenu.Text += currentUser.TotalShipments.ToString();
                        litSidebarMenu.Text += "</span></span>";
                        litSidebarMenu.Text += "</a></li>";
                    }
                    //Customer section
                    if (currentUser.CanViewCustomerAssociations || currentUser.CanViewMyAccountActivity)
                    {
                        //Add the Customers Header
                        litSidebarMenu.Text += "<li class=\"header\">CUSTOMERS</li>";


                        //My Account Activity Sidebar Item
                        litSidebarMenu.Text += "<li ";
                        if (hfPageName.Value == "customerdetail")
                        {
                            litSidebarMenu.Text += "class=\"active\"";
                        }
                        litSidebarMenu.Text += "><a href=\"" + ResolveUrl("~/customer/customerdetail.aspx?customerid=0")
                            + "\"><i class=\"fa fa-pencil\"></i> <span>Create New Customer</span></a></li>";


                        if (currentUser.CanViewCustomerAssociations)
                        {
                            //Associations Sidebar Item
                            litSidebarMenu.Text += "<li ";
                            if (hfPageName.Value == "associations")
                            {
                                litSidebarMenu.Text += "class=\"active\"";
                            }
                            litSidebarMenu.Text += "><a href=\"" + ResolveUrl("~/customer/associations.aspx")
                                + "\"><i class=\"fa fa-address-book\"></i> <span>Associations</span>";
                            //litSidebarMenu.Text += "<span class=\"pull-right-container\"><span class=\"label pull-right bg-red\">";
                            //litSidebarMenu.Text += currentUser.TotalAssociations.ToString();
                            //litSidebarMenu.Text += "</span></span>";
                            litSidebarMenu.Text += "</a></li>";
                        }

                        if (currentUser.CanViewMyAccountActivity)
                        {
                            //My Account Activity Sidebar Item
                            litSidebarMenu.Text += "<li ";
                            if (hfPageName.Value == "myaccountactivity")
                            {
                                litSidebarMenu.Text += "class=\"active\"";
                            }
                            litSidebarMenu.Text += "><a href=\"" + ResolveUrl("~/customer/myaccountactivity.aspx")
                                + "\"><i class=\"fa fa-link\"></i> <span>My Account Activity</span></a></li>";
                        }
                    }

                    //Mailing section
                    if (currentUser.CanSendNewsletters || currentUser.CanCreateLabels)
                    {
                        //Add the Mailing Header
                        litSidebarMenu.Text += "<li class=\"header\">MAILING</li>";

                        if (currentUser.CanSendNewsletters)
                        {
                            //Newsletters Sidebar Item
                            litSidebarMenu.Text += "<li ";
                            if (hfPageName.Value == "newsletter")
                            {
                                litSidebarMenu.Text += "class=\"active\"";
                            }
                            litSidebarMenu.Text += "><a href=\"" + ResolveUrl("~/mailing/newsletters.aspx")
                                + "\"><i class=\"fa fa-envelope\"></i> <span>Newsletters</span></a></li>";
                        }

                        if (currentUser.CanCreateLabels)
                        {
                            //Labels Sidebar Item
                            litSidebarMenu.Text += "<li ";
                            if (hfPageName.Value == "mailinglabels")
                            {
                                litSidebarMenu.Text += "class=\"active\"";
                            }
                            litSidebarMenu.Text += "><a href=\"" + ResolveUrl("~/mailing/mailinglabels.aspx")
                                + "\"><i class=\"fa fa-table\"></i> <span>Mailing Labels</span></a></li>";
                        }
                    }

                    //Settings section
                    if (currentUser.ShowSettings)
                    {
                        //Add the Settings Header
                        litSidebarMenu.Text += "<li class=\"header\">SETTINGS</li>";

                        //Programs Sidebar Item
                        litSidebarMenu.Text += "<li ";
                        if (hfPageName.Value == "programs")
                        {
                            litSidebarMenu.Text += "class=\"active\"";
                        }
                        litSidebarMenu.Text += "><a href=\"" + ResolveUrl("~/products/programlist.aspx")
                            + "\"><i class=\"fa fa-book\"></i> <span>Programs</span>";
                        //litSidebarMenu.Text += "<span class=\"pull-right-container\"><span class=\"label pull-right bg-red\">";
                        //litSidebarMenu.Text += currentUser.TotalPrograms.ToString();
                        //litSidebarMenu.Text += "</span></span>";
                        litSidebarMenu.Text += "</a></li>";


                        //Catalogs Sidebar Item
                        litSidebarMenu.Text += "<li ";
                        if (hfPageName.Value == "catalogs")
                        {
                            litSidebarMenu.Text += "class=\"active\"";
                        }
                        litSidebarMenu.Text += "><a href=\"" + ResolveUrl("~/catalogs/cataloglist.aspx")
                            + "\"><i class=\"fa fa-folder-open\"></i> <span>Catalogs</span>";
                        //litSidebarMenu.Text += "<span class=\"pull-right-container\"><span class=\"label pull-right bg-red\">";
                        //litSidebarMenu.Text += currentUser.TotalCatalogs.ToString();
                        //litSidebarMenu.Text += "</span></span>";
                        litSidebarMenu.Text += "</a></li>";

                        //Racks Sidebar Item
                        litSidebarMenu.Text += "<li ";
                        if (hfPageName.Value == "racks")
                        {
                            litSidebarMenu.Text += "class=\"active\"";
                        }
                        litSidebarMenu.Text += "><a href=\"" + ResolveUrl("~/racks/racklist.aspx")
                            + "\"><i class=\"fa fa-cart-arrow-down\"></i> <span>Racks</span>";
                        //litSidebarMenu.Text += "<span class=\"pull-right-container\"><span class=\"label pull-right bg-red\">";
                        //litSidebarMenu.Text += currentUser.TotalRacks.ToString();
                        //litSidebarMenu.Text += "</span></span>";
                        litSidebarMenu.Text += "</a></li>";

                        //Products Sidebar Item
                        litSidebarMenu.Text += "<li ";
                        if (hfPageName.Value == "products")
                        {
                            litSidebarMenu.Text += "class=\"active\"";
                        }
                        litSidebarMenu.Text += "><a href=\"" + ResolveUrl("~/products/productlist.aspx")
                            + "\"><i class=\"fa fa-suitcase\"></i> <span>Products</span>";
                        //litSidebarMenu.Text += "<span class=\"pull-right-container\"><span class=\"label pull-right bg-red\">";
                        //litSidebarMenu.Text += currentUser.TotalProducts.ToString();
                        //litSidebarMenu.Text += "</span></span>";
                        litSidebarMenu.Text += "</a></li>";

                        //Users Sidebar Item
                        litSidebarMenu.Text += "<li ";
                        if (hfPageName.Value == "users")
                        {
                            litSidebarMenu.Text += "class=\"active\"";
                        }
                        litSidebarMenu.Text += "><a href=\"" + ResolveUrl("~/users/userlist.aspx")
                            + "\"><i class=\"fa fa-user\"></i> <span>Users</span>";
                        //litSidebarMenu.Text += "<span class=\"pull-right-container\"><span class=\"label pull-right bg-red\">";
                        //litSidebarMenu.Text += currentUser.TotalUsers.ToString();
                        //litSidebarMenu.Text += "</span></span>";
                        litSidebarMenu.Text += "</a></li>";

                        //Freight charges
                        //Users Sidebar Item
                        litSidebarMenu.Text += "<li ";
                        if (hfPageName.Value == "freight")
                        {
                            litSidebarMenu.Text += "class=\"active\"";
                        }
                        litSidebarMenu.Text += "><a href=\"" + ResolveUrl("~/freight/default.aspx")
                            + "\"><i class=\"fa fa-dollar\"></i> <span>Freight Charges</span>";
                        //litSidebarMenu.Text += "<span class=\"pull-right-container\"><span class=\"label pull-right bg-red\">";
                        //litSidebarMenu.Text += currentUser.TotalFreightCharges.ToString();
                        //litSidebarMenu.Text += "</span></span>";
                        litSidebarMenu.Text += "</a></li>";
                        //Users Sidebar Item
                        litSidebarMenu.Text += "<li ";
                        if (hfPageName.Value == "pages")
                        {
                            litSidebarMenu.Text += "class=\"active\"";
                        }
                        //Removed on client's request
                        //litSidebarMenu.Text += "><a href=\"" + ResolveUrl("~/pages/default.aspx")
                        //    + "\"><i class=\"fa fa-file\"></i> <span>Pages</span>";
                        ////litSidebarMenu.Text += "<span class=\"pull-right-container\"><span class=\"label pull-right bg-red\">";
                        ////litSidebarMenu.Text += currentUser.TotalStaticPages.ToString();
                        ////litSidebarMenu.Text += "</span></span>";
                        //litSidebarMenu.Text += "</a></li>";
                    }

                }

                txtSearch.Text = AppSession.Current.LastSearchString;

                if (AppSession.Current.LastSearchType != "")
                {
                    ddlSearchType.SelectedValue = AppSession.Current.LastSearchType;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddMessage(string message)
        {
            MasterErrorMessages.Text = message;
        }
    }
}