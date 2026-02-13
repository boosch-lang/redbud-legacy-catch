using FCS;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Maddux.Classes
{
    public class Configuration
    {
        private string p_AppName;
        private string p_CompanyName;
        private string p_CompanyAddress;
        private string p_CompanyCity;
        private string p_CompanyState;
        private string p_CompanyZip;
        private string p_CompanyCountry;
        private string p_CompanyPhone1;
        private string p_CompanyPhone2;
        private string p_CompanyFax;
        private string p_CompanyEmail;
        private string p_CompanyWebsite;
        private string p_ShipToAddress;
        private string p_ShipToCity;
        private string p_ShipToState;
        private string p_ShipToZip;
        private string p_ShipToCountry;
        private long p_JournalEntryDays;
        private string p_NewsletterURL;
        private string p_MemoURL;
        private string p_NewsletterTemplatesFolder;
        private bool p_NewsletterUseExternalSMTP;
        private string p_NewsletterSMTPServer;
        private string p_NewsletterSMTPLogin;
        private string p_NewsletterSMTPPwd;
        private int p_NewsletterSMTPPort;
        private bool p_NewsletterUseSSL;
        private string p_LostPwdFromAddress;
        private string p_EmailerFromAddress;
        private bool p_EmailUseExternalSMTP;
        private string p_EmailSMTPServer;
        private string p_EmailSMTPLogin;
        private string p_EmailSMTPPwd;
        private int p_EmailSMTPPort;
        private bool p_EmailUseSSL;
        private string p_LastNewsletterFromAddress;
        private string p_LastNewsletterSubject;
        private bool p_LastNewsletterWasPreformed;
        private string p_LastNewsletterPreformedName;
        private string p_LastNewsletterBody;
        private string p_SimplyDBPath;
        private string p_SimplyUserName;
        private string p_SimplyPwd;
        private string p_InvoiceEmailBody;

        public Configuration()
        {
            try
            {
                DataHelper dh = new DataHelper();

                DataTable dt = dh.GetDataTableSQL("SELECT * FROM dbo.Configuration");

                if (dt.Rows.Count != 0)
                {
                    DataRow dr = dt.Rows[0];

                    p_AppName = dr["AppName"].ToString();
                    p_CompanyName = dr["CompanyName"].ToString();
                    p_CompanyAddress = dr["CompanyAddress"].ToString();
                    p_CompanyCity = dr["CompanyCity"].ToString();
                    p_CompanyState = dr["CompanyState"].ToString();
                    p_CompanyZip = dr["CompanyZip"].ToString();
                    p_CompanyCountry = dr["CompanyCountry"].ToString();
                    p_CompanyPhone1 = dr["CompanyPhone1"].ToString();
                    p_CompanyPhone2 = dr["CompanyPhone2"].ToString();
                    p_CompanyFax = dr["CompanyFax"].ToString();
                    p_CompanyEmail = dr["CompanyEmail"].ToString();
                    p_CompanyWebsite = dr["CompanyWebsite"].ToString();
                    p_ShipToAddress = dr["ShipToAddress"].ToString();
                    p_ShipToCity = dr["ShipToCity"].ToString();
                    p_ShipToState = dr["ShipToState"].ToString();
                    p_ShipToZip = dr["ShipToZip"].ToString();
                    p_ShipToCountry = dr["ShipToCountry"].ToString();
                    p_JournalEntryDays = Convert.ToInt32(dr["JournalEntryDays"]);
                    p_NewsletterURL = dr["NewsletterURL"].ToString();
                    p_MemoURL = dr["MemoURL"].ToString();
                    p_NewsletterTemplatesFolder = dr["NewsletterTemplatesFolder"].ToString();
                    p_NewsletterUseExternalSMTP = Convert.ToBoolean(dr["NewsletterUseExternalSMTP"]);
                    p_NewsletterSMTPServer = dr["NewsletterSMTPServer"].ToString();
                    p_NewsletterSMTPLogin = dr["NewsletterSMTPLogin"].ToString();
                    p_NewsletterSMTPPwd = dr["NewsletterSMTPPwd"].ToString();
                    p_NewsletterSMTPPort = Convert.ToInt32(dr["NewsletterSMTPPort"]);
                    p_NewsletterUseSSL = Convert.ToBoolean(dr["NewsletterUseSSL"]);
                    p_LostPwdFromAddress = dr["LostPwdFromAddress"].ToString();
                    p_EmailerFromAddress = dr["EmailerFromAddress"].ToString();
                    p_EmailUseExternalSMTP = Convert.ToBoolean(dr["EmailUseExternalSMTP"]);
                    p_EmailSMTPServer = dr["EmailSMTPServer"].ToString();
                    p_EmailSMTPLogin = dr["EmailSMTPLogin"].ToString();
                    p_EmailSMTPPwd = dr["EmailSMTPPwd"].ToString();
                    p_EmailSMTPPort = Convert.ToInt32(dr["EmailSMTPPort"]);
                    p_EmailUseSSL = Convert.ToBoolean(dr["EmailUseSSL"]);
                    p_LastNewsletterFromAddress = dr["LastNewsletterFromAddress"].ToString();
                    p_LastNewsletterSubject = dr["LastNewsletterSubject"].ToString();
                    p_LastNewsletterWasPreformed = Convert.ToBoolean(dr["LastNewsletterWasPreformed"]);
                    p_LastNewsletterPreformedName = dr["LastNewsletterPreformedName"].ToString();
                    p_LastNewsletterBody = dr["LastNewsletterBody"].ToString();
                    p_SimplyDBPath = dr["SimplyDBPath"].ToString();
                    p_SimplyUserName = dr["SimplyUserName"].ToString();
                    p_SimplyPwd = dr["SimplyPwd"].ToString();
                    p_InvoiceEmailBody = dr["InvoiceEmailBody"].ToString();
                }
                else
                {
                    p_CompanyName = "";
                    p_CompanyAddress = "";
                    p_CompanyCity = "";
                    p_CompanyState = "";
                    p_CompanyZip = "";
                    p_CompanyCountry = "";
                    p_CompanyPhone1 = "";
                    p_CompanyPhone2 = "";
                    p_CompanyFax = "";
                    p_CompanyEmail = "";
                    p_CompanyWebsite = "";
                    p_ShipToAddress = "";
                    p_ShipToCity = "";
                    p_ShipToState = "";
                    p_ShipToZip = "";
                    p_ShipToCountry = "";
                    p_JournalEntryDays = 0;
                    p_NewsletterURL = "";
                    p_MemoURL = "";
                    p_NewsletterTemplatesFolder = "";
                    p_NewsletterUseExternalSMTP = false;
                    p_NewsletterSMTPServer = "";
                    p_NewsletterSMTPLogin = "";
                    p_NewsletterSMTPPwd = "";
                    p_NewsletterSMTPPort = 25;
                    p_NewsletterUseSSL = false;
                    p_LostPwdFromAddress = "";
                    p_EmailerFromAddress = "";
                    p_EmailUseExternalSMTP = false;
                    p_EmailSMTPServer = "";
                    p_EmailSMTPLogin = "";
                    p_EmailSMTPPwd = "";
                    p_EmailSMTPPort = 25;
                    p_EmailUseSSL = false;
                    p_LastNewsletterFromAddress = "";
                    p_LastNewsletterBody = "";
                    p_LastNewsletterWasPreformed = false;
                    p_LastNewsletterPreformedName = "";
                    p_LastNewsletterSubject = "";
                    p_SimplyDBPath = "";
                    p_SimplyUserName = "";
                    p_SimplyPwd = "";
                    p_InvoiceEmailBody = "";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Save()
        {
            SqlCommand command;

            try
            {
                DataHelper dh = new DataHelper();

                command = new SqlCommand("spConfigurationSave");

                command.Parameters.AddWithValue("@LastNewsletterFromAddress", p_LastNewsletterFromAddress.ToString());
                command.Parameters.AddWithValue("@LastNewsletterSubject", p_LastNewsletterSubject.ToString());
                command.Parameters.AddWithValue("@LastNewsletterWasPreformed", p_LastNewsletterWasPreformed.ToString());
                command.Parameters.AddWithValue("@LastNewsletterPreformedName", p_LastNewsletterPreformedName.ToString());
                command.Parameters.AddWithValue("@LastNewsletterBody", p_LastNewsletterBody.ToString());

                dh.RunStoredProcedure(command);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string AppName
        {
            get { return p_AppName; }
            set { p_AppName = value; }
        }

        public string CompanyName
        {
            get { return p_CompanyName; }
            set { p_CompanyName = value; }
        }

        public string CompanyEmail
        {
            get { return p_CompanyEmail; }
            set { p_CompanyEmail = value; }
        }

        public string CompanyPhone1
        {
            get { return p_CompanyPhone1; }
            set { p_CompanyPhone1 = value; }
        }

        public string NewsletterTemplatesFolder
        {
            get { return p_NewsletterTemplatesFolder; }
            set { p_NewsletterTemplatesFolder = value; }
        }

        public bool NewsletterUseExternalSMTP
        {
            get { return p_NewsletterUseExternalSMTP; }
            set { p_NewsletterUseExternalSMTP = value; }
        }

        public string NewsletterSMTPServer
        {
            get { return p_NewsletterSMTPServer; }
            set { p_NewsletterSMTPServer = value; }
        }

        public string NewsletterSMTPLogin
        {
            get { return p_NewsletterSMTPLogin; }
            set { p_NewsletterSMTPLogin = value; }
        }

        public string NewsletterSMTPPwd
        {
            get { return p_NewsletterSMTPPwd; }
            set { p_NewsletterSMTPPwd = value; }
        }

        public int NewsletterSMTPPort
        {
            get { return p_NewsletterSMTPPort; }
            set { p_NewsletterSMTPPort = value; }
        }

        public bool NewsletterUseSSL
        {
            get { return p_NewsletterUseSSL; }
            set { p_NewsletterUseSSL = value; }
        }

        public string LostPwdFromAddress
        {
            get { return p_LostPwdFromAddress; }
            set { p_LostPwdFromAddress = value; }
        }

        public string EmailerFromAddress
        {
            get { return p_EmailerFromAddress; }
            set { p_EmailerFromAddress = value; }
        }

        public bool EmailUseExternalSMTP
        {
            get { return p_EmailUseExternalSMTP; }
            set { p_EmailUseExternalSMTP = value; }
        }

        public string EmailSMTPServer
        {
            get { return p_EmailSMTPServer; }
            set { p_EmailSMTPServer = value; }
        }

        public string EmailSMTPLogin
        {
            get { return p_EmailSMTPLogin; }
            set { p_EmailSMTPLogin = value; }
        }

        public string EmailSMTPPwd
        {
            get { return p_EmailSMTPPwd; }
            set { p_EmailSMTPPwd = value; }
        }

        public int EmailSMTPPort
        {
            get { return p_EmailSMTPPort; }
            set { p_EmailSMTPPort = value; }
        }

        public bool EmailUseSSL
        {
            get { return p_EmailUseSSL; }
            set { p_EmailUseSSL = value; }
        }

        public string LastNewsletterFromAddress
        {
            get { return p_LastNewsletterFromAddress; }
            set { p_LastNewsletterFromAddress = value; }
        }

        public string LastNewsletterSubject
        {
            get { return p_LastNewsletterSubject; }
            set { p_LastNewsletterSubject = value; }
        }

        public bool LastNewsletterWasPreformed
        {
            get { return p_LastNewsletterWasPreformed; }
            set { p_LastNewsletterWasPreformed = value; }
        }

        public string LastNewsletterPreformedName
        {
            get { return p_LastNewsletterPreformedName; }
            set { p_LastNewsletterPreformedName = value; }
        }

        public string LastNewsletterBody
        {
            get { return p_LastNewsletterBody; }
            set { p_LastNewsletterBody = value; }
        }

        public string SimplyDBPath
        {
            get { return p_SimplyDBPath; }
            set { p_SimplyDBPath = value; }
        }

        public string SimplyUserName
        {
            get { return p_SimplyUserName; }
            set { p_SimplyUserName = value; }
        }

        public string SimplyPwd
        {
            get { return p_SimplyPwd; }
            set { p_SimplyPwd = value; }
        }

        public string InvoiceEmailBody
        {
            get { return p_InvoiceEmailBody; }
            set { p_InvoiceEmailBody = value; }
        }
    }
}