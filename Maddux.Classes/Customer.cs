using FCS;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Maddux.Classes
{
    public class Customer
    {
        private int p_CustomerId;
        private int p_AccountingID;
        private string p_Company;
        private string p_Salutation;
        private string p_FirstName;
        private string p_LastName;
        private string p_JobTitle;
        private string p_Address;
        private string p_City;
        private string p_State;
        private string p_Zip;
        private string p_Country;
        private string p_Phone;
        private string p_Extension;
        private string p_CellPhone;
        private string p_Fax;
        private string p_WebSite;
        private string p_Email;
        private string p_AlternateEmail;
        private string p_Notes;
        private int p_SalesPersonID;
        private int p_ReferralID;
        private int p_ProbabilityID;
        private DateTime p_CreateDate;
        private DateTime p_ModifyDate;
        private string p_CreatedBy;
        private string p_ModifiedBy;
        private string p_BillingAddress;
        private string p_BillingCity;
        private string p_BillingState;
        private string p_BillingZip;
        private string p_BillingCountry;
        private string p_BillingPhone;
        private string p_BillingSalutation;
        private string p_BillingFirstName;
        private string p_BillingLastName;
        private string p_BillingCompany;
        private bool p_PSTExempt;
        private int p_DefaultTermsId;
        private int p_DefaultPaymentTypeId;
        private int p_DefaultShippingMethodID;
        private string p_WebPassword;
        private bool p_EmailInvoice;
        private string p_InvoiceEmail;
        private bool p_PrintInvoice;
        private bool p_InvoiceWithShipment;
        private bool p_Active;

        private bool flgExists;

        public Customer()
            : this(-1)
        {
        }

        public Customer(int customerID)
        {
            LoadCustomer("CustomerID=" + customerID.ToString());
        }

        public Customer(string emailAddress)
        {
            LoadCustomer("Email='" + emailAddress.Replace("'", "''") + "'");
        }

        public bool Save()
        {
            SqlCommand command;
            SqlParameter param;

            try
            {
                DataHelper dh = new DataHelper();

                command = new SqlCommand("spCustomerSave");

                param = new SqlParameter("@CustomerId", p_CustomerId)
                {
                    Direction = ParameterDirection.InputOutput
                };
                command.Parameters.Add(param);

                command.Parameters.AddWithValue("@Company", p_Company.Trim());
                command.Parameters.AddWithValue("@Salutation", p_Salutation.Trim());
                command.Parameters.AddWithValue("@FirstName", p_FirstName.Trim());
                command.Parameters.AddWithValue("@LastName", p_LastName.Trim());
                command.Parameters.AddWithValue("@JobTitle", p_JobTitle.Trim());
                command.Parameters.AddWithValue("@ShippingAddress", p_Address.Trim());
                command.Parameters.AddWithValue("@ShippingCity", p_City.Trim());
                command.Parameters.AddWithValue("@ShippingState", p_State.Trim());
                command.Parameters.AddWithValue("@ShippingZip", p_Zip.Trim());
                command.Parameters.AddWithValue("@ShippingCountry", p_Country.Trim());
                command.Parameters.AddWithValue("@BillingCompany", p_BillingCompany.Trim());
                command.Parameters.AddWithValue("@BillingSalutation", p_BillingSalutation.Trim());
                command.Parameters.AddWithValue("@BillingFirstName", p_BillingFirstName.Trim());
                command.Parameters.AddWithValue("@BillingLastName", p_BillingLastName.Trim());
                command.Parameters.AddWithValue("@BillingAddress", p_BillingAddress.Trim());
                command.Parameters.AddWithValue("@BillingCity", p_BillingCity.Trim());
                command.Parameters.AddWithValue("@BillingState", p_BillingState.Trim());
                command.Parameters.AddWithValue("@BillingZip", p_BillingZip.Trim());
                command.Parameters.AddWithValue("@BillingCountry", p_BillingCountry.Trim());
                command.Parameters.AddWithValue("@BillingPhone", p_BillingPhone.Trim());
                command.Parameters.AddWithValue("@Phone", p_Phone.Trim());
                command.Parameters.AddWithValue("@Extension", p_Extension.Trim());
                command.Parameters.AddWithValue("@CellPhone", p_CellPhone.Trim());
                command.Parameters.AddWithValue("@Fax", p_Fax.Trim());
                command.Parameters.AddWithValue("@WebSite", p_WebSite.Trim());
                command.Parameters.AddWithValue("@Email", p_Email.Trim());
                command.Parameters.AddWithValue("@AlternateEmail", p_AlternateEmail.Trim());
                command.Parameters.AddWithValue("@Notes", p_Notes.Trim());
                command.Parameters.AddWithValue("@SalesPersonID", p_SalesPersonID);
                command.Parameters.AddWithValue("@ReferralID", p_ReferralID);
                command.Parameters.AddWithValue("@ProbabilityID", p_ProbabilityID);
                command.Parameters.AddWithValue("@PSTExempt", p_PSTExempt);
                command.Parameters.AddWithValue("@DefaultTermsId", p_DefaultTermsId);
                command.Parameters.AddWithValue("@DefaultPaymentTypeId", p_DefaultPaymentTypeId);
                command.Parameters.AddWithValue("@DefaultShippingMethodID", p_DefaultShippingMethodID);
                command.Parameters.AddWithValue("@WebPassword", p_WebPassword.Trim());
                command.Parameters.AddWithValue("@EmailInvoice", p_EmailInvoice);
                command.Parameters.AddWithValue("@InvoiceEmail", p_InvoiceEmail.Trim());
                command.Parameters.AddWithValue("@PrintInvoice", p_PrintInvoice);
                command.Parameters.AddWithValue("@InvoiceWithShipment", p_InvoiceWithShipment);
                command.Parameters.AddWithValue("@Active", p_Active);
                command.Parameters.AddWithValue("@CreateDate", p_CreateDate.Date);
                command.Parameters.AddWithValue("@ModifyDate", p_ModifyDate.Date);
                command.Parameters.AddWithValue("@CreatedBy", p_CreatedBy.Trim());
                command.Parameters.AddWithValue("@ModifiedBy", p_ModifiedBy.Trim());

                dh.RunStoredProcedure(command);

                p_CustomerId = Convert.ToInt32(command.Parameters["@CustomerId"].Value.ToString());
                p_ModifyDate = DateTime.Now;
                //p_ModifiedBy = AppSession.Current.LoginName;

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete()
        {
            try
            {
                DataHelper dh = new DataHelper();
                dh.RunSQL("DELETE FROM dbo.Customers WHERE CustomerId = " + p_CustomerId.ToString());

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SendPassword()
        {
            Emailer emailer;

            try
            {
                emailer = new Emailer();
                return emailer.SendLostPwdEmail(p_FirstName, p_Email, p_WebPassword, false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadCustomer(string criteria)
        {
            try
            {
                flgExists = false;

                DataHelper dh = new DataHelper();
                DataTable customersTable = dh.GetDataTableSQL("SELECT * FROM Customers WHERE " + criteria);

                if (customersTable.Rows.Count > 0)
                {
                    DataRow customersRow = customersTable.Rows[0];

                    p_CustomerId = Convert.ToInt32(customersRow["CustomerID"]);
                    p_AccountingID = Convert.ToInt32(customersRow["AccountingID"]);
                    p_Company = customersRow["Company"].ToString();
                    p_Salutation = customersRow["Salutation"].ToString();
                    p_FirstName = customersRow["FirstName"].ToString();
                    p_LastName = customersRow["LastName"].ToString();
                    p_JobTitle = customersRow["JobTitle"].ToString();
                    p_Address = customersRow["Address"].ToString();
                    p_City = customersRow["City"].ToString();
                    p_State = customersRow["State"].ToString();
                    p_Zip = customersRow["Zip"].ToString();
                    p_Country = customersRow["Country"].ToString();
                    p_Phone = customersRow["Phone"].ToString();
                    p_Extension = customersRow["Extension"].ToString();
                    p_CellPhone = customersRow["CellPhone"].ToString();
                    p_Fax = customersRow["Fax"].ToString();
                    p_WebSite = customersRow["WebSite"].ToString();
                    p_Email = customersRow["Email"].ToString();
                    p_AlternateEmail = customersRow["AlternateEmail"].ToString();
                    p_Notes = customersRow["Notes"].ToString();
                    p_SalesPersonID = Convert.ToInt32(customersRow["SalesPersonID"]);
                    p_ReferralID = Convert.ToInt32(customersRow["ReferralID"]);
                    p_ProbabilityID = Convert.ToInt32(customersRow["ProbabilityID"]);
                    p_CreateDate = Convert.ToDateTime(customersRow["CreateDate"]);
                    p_ModifyDate = Convert.ToDateTime(customersRow["ModifyDate"]);
                    p_CreatedBy = customersRow["CreatedBy"].ToString();
                    p_ModifiedBy = customersRow["ModifiedBy"].ToString();
                    p_BillingAddress = customersRow["BillingAddress"].ToString();
                    p_BillingCity = customersRow["BillingCity"].ToString();
                    p_BillingState = customersRow["BillingState"].ToString();
                    p_BillingZip = customersRow["BillingZip"].ToString();
                    p_BillingCountry = customersRow["BillingCountry"].ToString();
                    p_BillingPhone = customersRow["BillingPhone"].ToString();
                    p_BillingSalutation = customersRow["BillingSalutation"].ToString();
                    p_BillingFirstName = customersRow["BillingFirstName"].ToString();
                    p_BillingLastName = customersRow["BillingLastName"].ToString();
                    p_BillingCompany = customersRow["BillingCompany"].ToString();
                    p_PSTExempt = Convert.ToBoolean(customersRow["PSTExempt"]);
                    p_DefaultTermsId = Convert.ToInt32(customersRow["DefaultTermsId"]);
                    p_DefaultPaymentTypeId = Convert.ToInt32(customersRow["DefaultPaymentTypeId"]);
                    p_DefaultShippingMethodID = Convert.ToInt32(customersRow["DefaultShippingMethodID"]);
                    p_WebPassword = customersRow["WebPassword"].ToString();
                    p_EmailInvoice = Convert.ToBoolean(customersRow["EmailInvoice"]);
                    p_InvoiceEmail = customersRow["InvoiceEmail"].ToString();
                    p_PrintInvoice = Convert.ToBoolean(customersRow["PrintInvoice"]);
                    p_InvoiceWithShipment = Convert.ToBoolean(customersRow["InvoiceWithShipment"]);
                    p_Active = Convert.ToBoolean(customersRow["Active"]);

                    flgExists = true;
                }
                else
                {
                    p_CustomerId = -1;
                    p_AccountingID = 0;
                    p_Company = "";
                    p_Salutation = "";
                    p_FirstName = "";
                    p_LastName = "";
                    p_JobTitle = "";
                    p_Address = "";
                    p_City = "";
                    p_State = "";
                    p_Zip = "";
                    p_Country = "";
                    p_Phone = "";
                    p_Extension = "";
                    p_CellPhone = "";
                    p_Fax = "";
                    p_WebSite = "";
                    p_Email = "";
                    p_AlternateEmail = "";
                    p_Notes = "";
                    p_SalesPersonID = 0;
                    p_ReferralID = 0;
                    p_ProbabilityID = 0;
                    p_CreateDate = DateTime.Today.Date;
                    p_ModifyDate = DateTime.Today.Date;
                    p_CreatedBy = "";
                    p_ModifiedBy = "";
                    p_BillingAddress = "";
                    p_BillingCity = "";
                    p_BillingState = "";
                    p_BillingZip = "";
                    p_BillingCountry = "";
                    p_BillingPhone = "";
                    p_BillingSalutation = "";
                    p_BillingFirstName = "";
                    p_BillingLastName = "";
                    p_BillingCompany = "";
                    p_PSTExempt = false;
                    p_DefaultTermsId = 0;
                    p_DefaultPaymentTypeId = 0;
                    p_DefaultShippingMethodID = 0;
                    p_WebPassword = "";
                    p_EmailInvoice = false;
                    p_InvoiceEmail = "";
                    p_PrintInvoice = false;
                    p_InvoiceWithShipment = false;
                    p_Active = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Exists
        {
            get { return flgExists; }
        }

        public int CustomerId
        {
            get { return p_CustomerId; }
        }

        public int AccountingID
        {
            get { return p_AccountingID; }
            set { p_AccountingID = value; }
        }

        public string Company
        {
            get { return p_Company; }
            set { p_Company = value; }
        }

        public string Salutation
        {
            get { return p_Salutation; }
            set { p_Salutation = value; }
        }

        public string FirstName
        {
            get { return p_FirstName; }
            set { p_FirstName = value; }
        }

        public string LastName
        {
            get { return p_LastName; }
            set { p_LastName = value; }
        }

        public string JobTitle
        {
            get { return p_JobTitle; }
            set { p_JobTitle = value; }
        }

        public string Address
        {
            get { return p_Address; }
            set { p_Address = value; }
        }

        public string City
        {
            get { return p_City; }
            set { p_City = value; }
        }

        public string State
        {
            get { return p_State; }
            set { p_State = value; }
        }

        public string Zip
        {
            get { return p_Zip; }
            set { p_Zip = value; }
        }

        public string Country
        {
            get { return p_Country; }
            set { p_Country = value; }
        }

        public string Phone
        {
            get { return p_Phone; }
            set { p_Phone = value; }
        }

        public string Extension
        {
            get { return p_Extension; }
            set { p_Extension = value; }
        }

        public string CellPhone
        {
            get { return p_CellPhone; }
            set { p_CellPhone = value; }
        }

        public string Fax
        {
            get { return p_Fax; }
            set { p_Fax = value; }
        }

        public string WebSite
        {
            get { return p_WebSite; }
            set { p_WebSite = value; }
        }

        public string Email
        {
            get { return p_Email; }
            set { p_Email = value; }
        }

        public string AlternateEmail
        {
            get { return p_AlternateEmail; }
            set { p_AlternateEmail = value; }
        }

        public string Notes
        {
            get { return p_Notes; }
            set { p_Notes = value; }
        }

        public int SalesPersonID
        {
            get { return p_SalesPersonID; }
            set { p_SalesPersonID = value; }
        }

        public int ReferralID
        {
            get { return p_ReferralID; }
            set { p_ReferralID = value; }
        }

        public int ProbabilityID
        {
            get { return p_ProbabilityID; }
            set { p_ProbabilityID = value; }
        }

        public DateTime CreateDate
        {
            get { return p_CreateDate; }
            set { p_CreateDate = value; }
        }

        public DateTime ModifyDate
        {
            get { return p_ModifyDate; }
            set { p_ModifyDate = value; }
        }

        public string CreatedBy
        {
            get { return p_CreatedBy; }
            set { p_CreatedBy = value; }
        }

        public string ModifiedBy
        {
            get { return p_ModifiedBy; }
            set { p_ModifiedBy = value; }
        }

        public string BillingAddress
        {
            get { return p_BillingAddress; }
            set { p_BillingAddress = value; }
        }

        public string BillingCity
        {
            get { return p_BillingCity; }
            set { p_BillingCity = value; }
        }

        public string BillingState
        {
            get { return p_BillingState; }
            set { p_BillingState = value; }
        }

        public string BillingZip
        {
            get { return p_BillingZip; }
            set { p_BillingZip = value; }
        }

        public string BillingCountry
        {
            get { return p_BillingCountry; }
            set { p_BillingCountry = value; }
        }

        public string BillingPhone
        {
            get { return p_BillingPhone; }
            set { p_BillingPhone = value; }
        }

        public string BillingSalutation
        {
            get { return p_BillingSalutation; }
            set { p_BillingSalutation = value; }
        }

        public string BillingFirstName
        {
            get { return p_BillingFirstName; }
            set { p_BillingFirstName = value; }
        }

        public string BillingLastName
        {
            get { return p_BillingLastName; }
            set { p_BillingLastName = value; }
        }

        public string BillingCompany
        {
            get { return p_BillingCompany; }
            set { p_BillingCompany = value; }
        }

        public bool PSTExempt
        {
            get { return p_PSTExempt; }
            set { p_PSTExempt = value; }
        }

        public int DefaultTermsId
        {
            get { return p_DefaultTermsId; }
            set { p_DefaultTermsId = value; }
        }

        public int DefaultPaymentTypeId
        {
            get { return p_DefaultPaymentTypeId; }
            set { p_DefaultPaymentTypeId = value; }
        }

        public int DefaultShippingMethodID
        {
            get { return p_DefaultShippingMethodID; }
            set { p_DefaultShippingMethodID = value; }
        }

        public string WebPassword
        {
            get { return p_WebPassword; }
            set { p_WebPassword = value; }
        }

        public bool EmailInvoice
        {
            get { return p_EmailInvoice; }
            set { p_EmailInvoice = value; }
        }

        public string InvoiceEmail
        {
            get { return p_InvoiceEmail; }
            set { p_InvoiceEmail = value; }
        }

        public bool PrintInvoice
        {
            get { return p_PrintInvoice; }
            set { p_PrintInvoice = value; }
        }

        public bool InvoiceWithShipment
        {
            get { return p_InvoiceWithShipment; }
            set { p_InvoiceWithShipment = value; }
        }

        public bool Active
        {
            get { return p_Active; }
            set { p_Active = value; }
        }

        public DataTable Associations
        {
            get
            {
                try
                {
                    string sql = "SELECT dbo.Associations.AssociationID, dbo.Associations.AsscDesc FROM dbo.CustomerAssc INNER JOIN dbo.Associations ON dbo.CustomerAssc.AssociationID = dbo.Associations.AssociationID WHERE (dbo.CustomerAssc.CustomerID = " + p_CustomerId.ToString() + ") GROUP BY dbo.Associations.AssociationID, dbo.Associations.AsscDesc ORDER BY dbo.Associations.AsscDesc";

                    DataHelper dh = new DataHelper();
                    return dh.GetDataTableSQL(sql);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public DataTable MembershipAssociations
        {
            get
            {
                try
                {
                    string sql = "SELECT dbo.Associations.AssociationID, dbo.Associations.AsscDesc FROM dbo.CustomerAssc INNER JOIN dbo.Associations ON dbo.CustomerAssc.AssociationID = dbo.Associations.AssociationID WHERE (dbo.CustomerAssc.CustomerID = " + p_CustomerId.ToString() + ") AND (dbo.Associations.Class = 'Membership') GROUP BY dbo.Associations.AssociationID, dbo.Associations.AsscDesc ORDER BY dbo.Associations.AsscDesc";

                    DataHelper dh = new DataHelper();
                    return dh.GetDataTableSQL(sql);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public DataTable SubCustomers
        {
            get
            {
                try
                {
                    string sql;

                    sql = "SELECT c.* FROM Customers c \n" +
                            "INNER JOIN CustomersSub cs ON cs.ChildCustomerID = c.CustomerID \n" +
                            "WHERE cs.MasterCustomerID = " + p_CustomerId + " \n" +
                            "ORDER BY c.Company";

                    DataHelper dh = new DataHelper();
                    return dh.GetDataTableSQL(sql);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public DataTable ActiveProgramsWithRacks
        {
            get
            {
                try
                {
                    CatalogSet catalogs = new CatalogSet();
                    return catalogs.GetCustomerCatalogsWithRacks(p_CustomerId, true);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public DataTable DraftOrders
        {
            get
            {
                try
                {
                    OrderSet orders = new OrderSet();
                    return orders.GetCustomerDraftOrders(p_CustomerId);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public DataTable SubCustomersWithDraftOrders
        {
            get
            {
                try
                {
                    OrderSet orders = new OrderSet();
                    return orders.GetCustomerSubCustomersWithDraftOrders(p_CustomerId);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public DataTable SubDraftOrders
        {
            get
            {
                try
                {
                    OrderSet orders = new OrderSet();
                    return orders.GetCustomerSubCustomerDraftOrders(p_CustomerId);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public DataTable SubCustomersWithUnshippedOrders
        {
            get
            {
                try
                {
                    OrderSet orders = new OrderSet();
                    return orders.GetCustomerSubCustomerWithUnshippedOrders(p_CustomerId);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public DataTable UnshippedOrders
        {
            get
            {
                try
                {
                    OrderSet orders = new OrderSet();
                    return orders.GetCustomerUnshippedOrders(p_CustomerId);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public DataTable GetOrderPrograms(DateTime startDate, DateTime endDate)
        {
            try
            {
                OrderSet orders = new OrderSet();
                return orders.GetCustomerPrograms(p_CustomerId, startDate, endDate, true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetOrderHistory(DateTime startDate, DateTime endDate, int programID)
        {
            try
            {
                OrderSet orders = new OrderSet();
                return orders.GetCustomerShippedOrders(p_CustomerId, startDate, endDate, programID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetSubCustomersWithOrderHistory(DateTime startDate, DateTime endDate, int programID)
        {
            try
            {
                OrderSet orders = new OrderSet();
                return orders.GetCustomerSubCustomerWithShippedOrders(p_CustomerId, startDate, endDate, programID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
