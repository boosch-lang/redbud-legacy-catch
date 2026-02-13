using Redbud.BL.DL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;

namespace Maddux.Pitch.requests
{
    public class SwipeboxPhoto
    {
        public string href { get; set; }
        public string title { get; set; }
    }
    /// <summary>
    /// Summary description for GetProductPhotos
    /// </summary>
    ///
    public class GetProductPhotos : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                int ProductID = int.Parse(context.Request.Form["productID"]);
                List<Photo> photos = new List<Photo>();
                List<SwipeboxPhoto> swipeboxPhotos = new List<SwipeboxPhoto>();
                using (MadduxEntities db = new MadduxEntities())
                {
                    Product product = db.Products.Include(p => p.Photos).FirstOrDefault(p => p.ProductId == ProductID);
                    photos = product.Photos.ToList();
                    foreach (Photo photo in photos)
                    {
                        SwipeboxPhoto swipeboxPhoto = new SwipeboxPhoto();
                        string href = photo.PhotoPath;
                        if (photo.PhotoPath.StartsWith("~/Photos"))
                        {
                            href = photo.PhotoPath.Replace("~/Photos", "//catch.redbud.com/uploads/files/products");
                        }
                        swipeboxPhoto.href = href;
                        swipeboxPhoto.title = photo.PhotoID.ToString();
                        swipeboxPhotos.Add(swipeboxPhoto);
                    }
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
                                    photos = swipeboxPhotos
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