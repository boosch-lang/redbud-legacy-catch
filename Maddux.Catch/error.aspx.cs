using System;

namespace Maddux.Catch
{
    public partial class error : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Get the last error from the server
            Exception ex = Server.GetLastError();
            litError.Text = ex.ToString();
            // Log the exception and notify system operators
            FCS.FCSErrorHandler.SubmitError(ex, "error handler");

            // Create a safe message
            string safeMsg = "It looks like an error has occurred in the application.  An email has been sent to report the problem.<br /><br />\n\n" +
                    "In the meantime, please try the operation again.  If the issue persists, please contact the System Administrator.";

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
                lblExceptionTrace.Visible = true;
            }
            else
            {
                ex = new Exception(safeMsg, ex);
            }

            // Fill the page fields
            lblExceptionMessage.Text = ex.Message;
            lblExceptionTrace.Text = ex.StackTrace;

            // Clear the error from the server
            Server.ClearError();
        }
    }
}