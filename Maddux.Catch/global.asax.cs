using log4net;
using System;
using System.Net;

namespace Maddux
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
                errorMessage = exc.ToString();
            }
            else
            {
                if (exc.InnerException != null)
                {
                    errorMessage += "<p>Inner Error Message:<br />" + exc.InnerException.Message + "</p>";
                    errorMessage += "<pre>" + exc.InnerException.StackTrace + "</pre>";
                }
                errorMessage += "<p>Error Message:<br />" + exc.Message + "</p>";
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