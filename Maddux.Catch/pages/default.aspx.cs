using Redbud.BL.DL;
using Redbud.BL.Utils;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace Maddux.Catch.pages
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //Not in use anymore
                Response.Redirect("/", true);

                Literal litPageHeader = (Literal)Master.FindControl("litPageHeader");
                litPageHeader.Text = $@"Pages";
                using (MadduxEntities db = new MadduxEntities())
                {
                    dgvPages.DataSource = db.StaticPages.Where(x => x.PageStatus != Redbud.BL.PageStatus.Deleted).ToList();
                    dgvPages.DataBind();
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }

        }

        protected void dgvPages_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
    }
}