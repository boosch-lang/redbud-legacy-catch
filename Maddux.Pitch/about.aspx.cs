using Redbud.BL.DL;
using Redbud.BL.Utils;
using System;
using System.Linq;

namespace Maddux.Pitch
{
    public partial class about : System.Web.UI.Page
    {
        private string Slug
        {
            get
            {
                if (ViewState["Slug"] == null)
                {
                    if (Request.QueryString["slug"] == null || Request.QueryString["slug"] == "")
                        ViewState["Slug"] = string.Empty;
                    else
                        ViewState["Slug"] = Request.QueryString["slug"];
                }
                return ViewState["Slug"].ToString();
            }

            set
            {
                ViewState["Slug"] = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //not being used anymore
                Response.Redirect("/", true);

                using (MadduxEntities db = new MadduxEntities())
                {
                    var page = db.StaticPages.FirstOrDefault(x => x.Slug.Contains(Slug));
                    if (page != null)
                    {
                        pageHTML.Text = page.HTML;
                        if (!string.IsNullOrWhiteSpace(page.BannerImagePath))
                        {
                            BannerImagePath.ImageUrl = page.BannerImagePath;
                        }
                        else
                        {
                            bannerDiv.Visible = false;
                            contentDiv.Attributes.Add("class", "col-xs-12");
                        }
                    }
                    else
                        pageHTML.Text = $@"<div class='text-center'>
                                        <h1>Coming Soon...</h1>
                                    </div>";
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }


        }
    }
}