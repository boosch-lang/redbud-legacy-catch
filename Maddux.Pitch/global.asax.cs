using log4net;
using Maddux.Pitch.LocalClasses;
using Redbud.BL.DL;
using System;
using System.Net;

namespace Maddux.Pitch
{
    public class global : System.Web.HttpApplication
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected void Application_Start(object sender, EventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            try
            {
                if (Request.IsAuthenticated && AppSession.Current != null)
                {
                    Customer theCustomer = null;
                    try
                    {
                        theCustomer = AppSession.Current.CurrentCustomer;
                    }
                    catch { }

                    if (theCustomer != null)
                    {
                        var request = Request.Path;

                        if (theCustomer.IsTemporaryPassword && request != "/temppassword.aspx")
                        {
                            Response.Redirect("/temppassword.aspx", false);
                        }
                    }
                }
            }
            catch { }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            // Get the exception object.
            string errorMessage = "";
            Exception exc = Server.GetLastError();
            log.Error(exc.Message, exc);

            //Write out the error details
            Response.Write("<h2>An Application Error Occurred</h2>\n");

            // Submit the error
            if (!Request.IsLocal)
            {
                FCS.FCSErrorHandler.SubmitError(exc, "default application error handler");
                errorMessage = "<p>It looks like an error has occurred in the application.  An email has been sent to report the problem.<br /><br />\n\n" +
                        "In the meantime, please try the operation again.  If the issue persists, please contact <a href=\"mailto:sales@redbud.com\">Redbud</a>.</p>";
            }
            else
            {
                if (exc.InnerException != null)
                {
                    errorMessage += "<p>Inner Error Message:<br />" + exc.InnerException.Message + "</p>";
                    errorMessage += "<pre>" + exc.InnerException.StackTrace + "</pre>";
                }
                errorMessage += "<p>Error Message:<br />" + exc.Message + "<!-- " + exc.StackTrace + "--></p>";
                errorMessage += "<pre>" + exc.StackTrace + "</pre>";

                if (exc.Data.Count > 0)
                {
                    errorMessage += "<p>Extra data:<br />";

                    foreach (System.Collections.DictionaryEntry de in exc.Data)
                    {
                        errorMessage += de.Key.ToString() + ": " + de.Value.ToString() + "\n";
                    }
                    errorMessage += "</p>";
                }
            }

            Response.Write(errorMessage);
            Response.Write("Return to your <a href='Default.aspx'>default page</a>.\n");

            // Clear the error from the server
            Server.ClearError();
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}