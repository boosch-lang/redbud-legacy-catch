using Redbud.BL.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Maddux.Catch.calllist.requests
{
    public class DropdownOption
    {
        public int val { get; set; }
        public string text { get; set; }
    }
    /// <summary>
    /// Summary description for Get Membership For Selected Campaigns
    /// </summary>
    public class GetMembershipForSelectedCampaigns : IHttpHandler
    {


        public void ProcessRequest(HttpContext context)
        {
            try
            {
                List<int> campaignIds = context.Request.Form["campaignIDs"].Split(',').Select(id => Convert.ToInt32(id)).ToList();
                List<Photo> photos = new List<Photo>();
                List<DropdownOption> Memberships = new List<DropdownOption>();
                using (MadduxEntities db = new MadduxEntities())
                {
                    List<int> catalogids = db.ProductCatalogs.Where(pc => pc.ProductProgram.Campaigns.Any(c => campaignIds.Contains(c.CampaignID))).Select(pc => pc.CatalogId).ToList();

                    Memberships = (from ac in db.AssociationCatalogs
                                   join a in db.Associations on ac.AssociationID equals a.AssociationID
                                   where catalogids.Contains(ac.CatalogID) && a.AsscDesc.StartsWith("Member")
                                   select new DropdownOption
                                   {
                                       text = a.AsscShort.Substring(8),
                                       val = a.AssociationID
                                   }).Distinct().OrderBy(a => a.val).ToList();
                }
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                System.Web.Script.Serialization.JavaScriptSerializer jsonSerializer =
                            new System.Web.Script.Serialization.JavaScriptSerializer();
                context.Response.ContentType = "text/json";
                context.Response.Write(
                            jsonSerializer.Serialize(
                                new
                                {
                                    success = true,
                                    memberships = Memberships
                                }
                            )
                        );
            }
            catch (Exception ex)
            {
                System.Web.Script.Serialization.JavaScriptSerializer jsonSerializer =
                            new System.Web.Script.Serialization.JavaScriptSerializer();
                context.Response.ContentType = "text/json";
                context.Response.Write(
                            jsonSerializer.Serialize(
                                new
                                {
                                    success = false,
                                    errors = ex.Message
                                }
                            )
                        );
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}