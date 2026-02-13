using Maddux.Pitch.LocalClasses;
using Redbud.BL.DL;
using System;

namespace FCS
{
    public sealed class FCSErrorHandler
    {
        private FCSErrorHandler()
        { }

        public static void SubmitError(Exception ex, string source)
        {
            try
            {
                string userName = "Unknown";
                string emailAddress = "";
                string errorDetails;
                errorDetails = "An error was trapped in " + source + ".\n\nException detail: " + ex.ToString();

                if (ex.InnerException != null)
                {
                    errorDetails += "\n\nInner error message: " + ex.InnerException.Message + "\n";
                    errorDetails += ex.InnerException.StackTrace;
                }

                if (ex.Data.Count > 0)
                {
                    errorDetails += "\n\nExtra data:\n";

                    foreach (System.Collections.DictionaryEntry de in ex.Data)
                    {
                        errorDetails += de.Key.ToString() + ": " + de.Value.ToString() + "\n";
                    }
                }

                if (AppSession.Current != null)
                {
                    Customer currentUser = AppSession.Current.CurrentCustomer;
                    if (currentUser != null)
                    {
                        userName = currentUser.Company;
                        emailAddress = currentUser.Email;
                    }
                }

                Maddux.Pitch.wsFCSBugTracker.IssueReportingServices issueReporter = new Maddux.Pitch.wsFCSBugTracker.IssueReportingServices();
                //issueReporter.SubmitError(14, userName, emailAddress, "", errorDetails, false);
            }
            catch { }
        }
    }
}