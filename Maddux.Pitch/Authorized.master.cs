using Maddux.Pitch.LocalClasses;
using Redbud.BL.DL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.UI;

namespace Maddux.Pitch
{
    public partial class Authorized : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SetCurrentPage();
            SetOGTags();
        }

        private void SetOGTags()
        {
            var baseUrl = ConfigurationManager.AppSettings["BaseUrl"];
            ogImage.Attributes["content"] = $"{baseUrl}/img/Redbud_Logo.png";
            ogUrl.Attributes["content"] = baseUrl;
        }

        private void SetCurrentPage()
        {

            var cartItemCount = GetCartItemsCount();
            if (cartItemCount == 0)
            {
                supCartTotal.Visible = false;
            }
            else
            {
                supCartTotal.Visible = true;
                supCartTotal.InnerText = cartItemCount.ToString();
            }
        }

        private int GetCartItemsCount()
        {
            Customer theCustomer = AppSession.Current.CurrentCustomer;

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

            return totalDraftOrders;

        }
    }
}