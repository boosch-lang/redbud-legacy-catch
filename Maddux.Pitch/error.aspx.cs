using System;

namespace Maddux.Pitch
{
    public partial class error : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Get the last error from the server
            Exception ex = Server.GetLastError();
            // Log the exception and notify system operators
            //FCS.FCSErrorHandler.SubmitError(ex, "error handler");

            // Create a safe message
            string safeMsg = "It looks like an error has occurred in the application.  An email has been sent to report the problem.<br /><br />\n\n" +
                    "In the meantime, please try the operation again.  If the issue persists, please contact <a href=\"mailto:sales@redbud.com\">Redbud</a>.";

            // Show Inner Exception fields for local access
            if (ex.InnerException != null)
            {
                lblInnerTrace.Text = ex.InnerException.StackTrace;
                pnlInnerError.Visible = Request.IsLocal;
                lblInnerMessage.Text = ex.InnerException.Message;
            }
            // Show Trace for local access
            if (Request.IsLocal)
            {
                lblExceptionTrace.Text = ex.StackTrace;
            }
            else
            {
                // show in a hidden comment
                lblExceptionTrace.Text = "<!-- " + ex.ToString() + "-->";
                ex = new ApplicationException(safeMsg, ex);
            }

            // Fill the page fields
            lblExceptionMessage.Text = ex.Message;

            // Clear the error from the server
            Server.ClearError();
        }
    }
}