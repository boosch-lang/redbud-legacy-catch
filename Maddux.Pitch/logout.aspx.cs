using Maddux.Pitch.LocalClasses;
using System;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;

namespace Maddux.Pitch
{
    public partial class logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                AppSession.Current.Clear();
                FormsAuthentication.SignOut();
                Session.Abandon();

                // clear authentication cookie
                HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, "")
                {
                    Expires = DateTime.Now.AddYears(-1)
                };
                Response.Cookies.Add(authCookie);

                // clear session cookie
                SessionStateSection sessionStateSection = (SessionStateSection)WebConfigurationManager.GetSection("system.web/sessionState");
                HttpCookie sessionCookie = new HttpCookie(sessionStateSection.CookieName, "")
                {
                    Expires = DateTime.Now.AddYears(-1)
                };
                Response.Cookies.Add(sessionCookie);

                Response.Redirect("login.aspx?logout=true", false);
            }
            catch (Exception ex)
            {
                Response.Write("An error occurred: " + ex.Message);
                return;
            }
        }
    }
}