using System;

namespace Maddux.Pitch
{
    public partial class Login : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblCopyrightYear.Text = DateTime.Today.Year.ToString();
        }
    }
}