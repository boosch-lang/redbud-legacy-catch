using Maddux.Catch.LocalClasses;
using Redbud.BL.DL;
using Redbud.BL.Utils;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Maddux.Catch.pages
{
    public partial class edit : System.Web.UI.Page
    {
        private int PageID
        {
            get
            {
                if (ViewState["PageID"] == null)
                {
                    ViewState["PageID"] = Request.QueryString["id"] == null || Request.QueryString["id"] == "" ? 0 : (object)Request.QueryString["id"];
                }
                return Convert.ToInt32(ViewState["PageID"].ToString());
            }

            set
            {
                ViewState["PageID"] = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //Not in use anymore
            Response.Redirect("/", true);

            Literal litPageHeader = (Literal)Master.FindControl("litPageHeader");
            try
            {
                using (MadduxEntities db = new MadduxEntities())
                {
                    StaticPage page;
                    if (PageID == 0)
                    {
                        litPageHeader.Text = $@"New Page";
                    }
                    else
                    {
                        if (!Page.IsPostBack)
                        {
                            page = db.StaticPages.Where(x => x.PageID == PageID).FirstOrDefault();
                            if (page != null)
                            {
                                litPageHeader.Text = $@"Page <small>({page.Title})<small>";

                                TitleText.Text = page.Title;
                                HTML.Text = page.HTML;
                                Description.Text = page.Description;
                                BannerImagePath.Value = page.BannerImagePath;
                            }
                            else
                            {
                                litPageHeader.Text = $@"New Page";
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }

        }

        protected void Save_Click(object sender, EventArgs e)
        {
            try
            {
                using (MadduxEntities db = new MadduxEntities())
                {
                    var currentUser = AppSession.Current.CurrentUser;
                    StaticPage page;
                    bool NewPage = false;
                    if (PageID == 0)
                    {
                        page = db.StaticPages.Create();
                        NewPage = true;
                    }
                    else
                    {
                        page = db.StaticPages.Where(x => x.PageID == PageID).FirstOrDefault();
                        if (page == null)
                        {
                            page = db.StaticPages.Create();
                            NewPage = true;
                        }
                    }
                    page.Title = TitleText.Text;
                    page.HTML = HTML.Text;
                    page.Description = Description.Text;
                    page.Slug = StringTools.GenerateSlug(TitleText.Text);
                    page.PageStatus = Redbud.BL.PageStatus.Published;
                    page.ModifiedOn = DateTime.Now;
                    page.ModifiedBy = currentUser.FullName;
                    page.BannerImagePath = BannerImagePath.Value;
                    if (NewPage)
                    {
                        page.CreatedOn = DateTime.Now;
                        page.CreatedBy = currentUser.FullName;
                        db.StaticPages.Add(page);
                    }
                    int saved = db.SaveChanges();
                    if (saved > 0)
                    {
                        litMessage.Text = StringTools.GenerateSuccess("Saved!");
                        if (NewPage)
                            Response.Redirect($"/pages/edit.aspx?id={page.PageID}", true);
                    }

                }
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }
    }
}