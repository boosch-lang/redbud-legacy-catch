using Maddux.Catch.LocalClasses;
using Redbud.BL.DL;
using Redbud.BL.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Maddux.Catch.users
{
    public partial class userlist : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            if (!IsPostBack)
            {
                Title = "Maddux.Catch | Users";
                LoadGrid();
            }


        }
        /// <summary>
        /// Load users
        /// </summary>
        private void LoadGrid()
        {
            User currentUser = AppSession.Current.CurrentUser;
            try
            {
                List<User> usersList; //To store users

                using (MadduxEntities db = new MadduxEntities())
                {
                    usersList = !chkShowAll.Checked
                        ? db.Users.Where(x => x.Active == true)
                        .OrderBy(x => x.FirstName)
                        .ToList()
                        : db.Users
                        .OrderBy(x => x.FirstName)
                        .ToList();

                    dgvUsers.DataSource = usersList;
                    dgvUsers.DataBind();
                }
                Literal litPageHeader = (Literal)Master.FindControl("litPageHeader");
                litPageHeader.Text = "Users";
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }

        protected void chkShowAll_CheckedChanged(object sender, EventArgs e)
        {
            LoadGrid();
        }

        protected void GridUsers_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
        /// <summary>
        /// Pagin handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitUsersGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvUsers.PageIndex = e.NewPageIndex;
            dgvUsers.DataBind();
        }
        protected void btnAddUser_Click(object sender, EventArgs e)
        {
            Response.Redirect("/users/userdetail.aspx");
        }
    }
}