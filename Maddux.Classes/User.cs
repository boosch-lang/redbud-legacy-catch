using FCS;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Maddux.Classes
{
    [Serializable]
    public class User
    {
        private int p_UserID;
        private string p_FirstName;
        private string p_LastName;
        private string p_Gender;
        private string p_EmailAddress;
        private string p_PasswordEncrypted;
        private string p_PasswordUnEncrypted;
        private bool p_Active;
        private bool p_DefaultToPersonalJournals;
        private bool p_DefaultToPersonalOrders;
        private bool p_DefaultToPersonalShipments;
        private string p_HighlightBackgroundColour;
        private string p_HighlightForegroundColour;
        private int p_SortPosition;
        private bool p_ShowCampaigns;
        private bool p_ShowSettings;
        private bool p_ShowOtherMyJournals;
        private bool p_ShowOtherMyShipments;
        private bool p_ShowOtherMyOrders;
        private bool p_CanSendNewsletters;
        private bool p_CanCreateLabels;
        private bool p_CanViewMyAccountActivity;
        private bool p_CanDeleteCustomers;
        private bool p_CanViewCustomerAssociations;
        private bool p_CanMergeCustomers;
        private bool p_CanEditOrders;
        private bool p_CanDeleteOrders;
        private bool p_CanEmailPrintExportOrders;
        private bool p_CanApproveOrders;
        private bool p_CanEditShipments;
        private bool p_CanDeleteShipments;
        private bool p_CanEmailPrintExportShipments;
        private bool p_CanEditCreditMemos;
        private bool p_CanDeleteCreditMemos;
        private bool p_CanEmailPrintExportCreditMemos;
        private bool p_CanOnlyViewAssignedAssociations;
        private bool p_CanOnlyViewAssignedProvinces;
        private bool p_CanOnlyViewAssignedCatalogs;
        private bool p_CanOnlyViewOwnCustomers;

        private bool flgFound;

        public User()
            : this(-1)
        {
        }

        public User(int userID)
        {
            try
            {
                LoadUser(0, userID.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public User(string emailAddress)
        {
            try
            {
                LoadUser(1, emailAddress);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadUser(int type, string criteria)
        {
            string sql;
            SqlCommand cmd;

            try
            {
                flgFound = false;
                cmd = new SqlCommand();

                if (type == 0)
                {
                    sql = "SELECT * FROM Users WHERE UserID = @UserID";
                    cmd.Parameters.AddWithValue("@UserID", criteria);
                }
                else
                {
                    sql = "SELECT * FROM Users WHERE EmailAddress = @EmailAddress";
                    cmd.Parameters.AddWithValue("@EmailAddress", criteria);
                }
                cmd.CommandText = sql;

                DataHelper dh = new DataHelper();
                DataTable dt = dh.GetDataTableCmd(cmd);

                if (dt.Rows.Count != 0)
                {
                    DataRow dr = dt.Rows[0];

                    p_UserID = Convert.ToInt32(dr["UserId"]);
                    p_FirstName = dr["FirstName"].ToString();
                    p_LastName = dr["LastName"].ToString();
                    p_Gender = dr["Gender"].ToString();
                    p_EmailAddress = dr["EmailAddress"].ToString();
                    p_PasswordEncrypted = dr["Password"].ToString();
                    p_PasswordUnEncrypted = FCSEncryption.Decrypt(p_PasswordEncrypted);
                    p_Active = Convert.ToBoolean(dr["Active"]);
                    p_DefaultToPersonalJournals = Convert.ToBoolean(dr["DefaultToPersonalJournals"]);
                    p_DefaultToPersonalOrders = Convert.ToBoolean(dr["DefaultToPersonalOrders"]);
                    p_DefaultToPersonalShipments = Convert.ToBoolean(dr["DefaultToPersonalShipments"]);
                    p_HighlightBackgroundColour = dr["HighlightBackgroundColour"].ToString();
                    p_HighlightForegroundColour = dr["HighlightForegroundColour"].ToString();
                    p_ShowCampaigns = Convert.ToBoolean(dr["ShowCampaigns"]);
                    p_ShowSettings = Convert.ToBoolean(dr["ShowSettings"]);
                    p_ShowOtherMyJournals = Convert.ToBoolean(dr["ShowOtherMyJournals"]);
                    p_ShowOtherMyOrders = Convert.ToBoolean(dr["ShowOtherMyOrders"]);
                    p_ShowOtherMyShipments = Convert.ToBoolean(dr["ShowOtherMyShipments"]);
                    p_CanSendNewsletters = Convert.ToBoolean(dr["CanSendNewsletters"]);
                    p_CanCreateLabels = Convert.ToBoolean(dr["CanCreateLabels"]);
                    p_CanViewMyAccountActivity = Convert.ToBoolean(dr["CanViewMyAccountActivity"]);
                    p_CanDeleteCustomers = Convert.ToBoolean(dr["CanDeleteCustomers"]);
                    p_CanViewCustomerAssociations = Convert.ToBoolean(dr["CanViewCustomerAssociations"]);
                    p_CanMergeCustomers = Convert.ToBoolean(dr["CanMergeCustomers"]);
                    p_CanEditOrders = Convert.ToBoolean(dr["CanEditOrders"]);
                    p_CanDeleteOrders = Convert.ToBoolean(dr["CanDeleteOrders"]);
                    p_CanEmailPrintExportOrders = Convert.ToBoolean(dr["CanEmailPrintExportOrders"]);
                    p_CanApproveOrders = Convert.ToBoolean(dr["CanApproveOrders"]);
                    p_CanEditShipments = Convert.ToBoolean(dr["CanEditShipments"]);
                    p_CanDeleteShipments = Convert.ToBoolean(dr["CanDeleteShipments"]);
                    p_CanEmailPrintExportShipments = Convert.ToBoolean(dr["CanEmailPrintExportShipments"]);
                    p_CanEditCreditMemos = Convert.ToBoolean(dr["CanEditCreditMemos"]);
                    p_CanDeleteCreditMemos = Convert.ToBoolean(dr["CanDeleteCreditMemos"]);
                    p_CanEmailPrintExportCreditMemos = Convert.ToBoolean(dr["CanEmailPrintExportCreditMemos"]);
                    p_CanOnlyViewAssignedAssociations = Convert.ToBoolean(dr["CanOnlyViewAssignedAssociations"]);
                    p_CanOnlyViewAssignedProvinces = Convert.ToBoolean(dr["CanOnlyViewAssignedProvinces"]);
                    p_CanOnlyViewAssignedCatalogs = Convert.ToBoolean(dr["CanOnlyViewAssignedCatalogs"]);
                    p_CanOnlyViewOwnCustomers = Convert.ToBoolean(dr["CanOnlyViewOwnCustomers"]);

                    flgFound = true;
                }
                else
                {
                    p_UserID = -1;
                    p_FirstName = "";
                    p_LastName = "";
                    p_Gender = "M";
                    p_EmailAddress = "";
                    p_PasswordUnEncrypted = System.Web.Security.Membership.GeneratePassword(6, 1);
                    p_PasswordEncrypted = FCSEncryption.Encrypt(p_PasswordUnEncrypted);
                    p_Active = false;
                    p_DefaultToPersonalJournals = true;
                    p_DefaultToPersonalOrders = true;
                    p_DefaultToPersonalShipments = true;
                    p_ShowCampaigns = false;
                    p_ShowSettings = false;
                    p_ShowOtherMyJournals = false;
                    p_ShowOtherMyOrders = false;
                    p_ShowOtherMyShipments = false;
                    p_HighlightBackgroundColour = "#ffffff";
                    p_HighlightForegroundColour = "#000000";
                    p_SortPosition = 0;
                    p_CanCreateLabels = false;
                    p_CanSendNewsletters = false;
                    p_CanViewMyAccountActivity = false;
                    p_CanDeleteCustomers = false;
                    p_CanViewCustomerAssociations = false;
                    p_CanMergeCustomers = false;
                    p_CanEditOrders = false;
                    p_CanDeleteOrders = false;
                    p_CanEmailPrintExportOrders = false;
                    p_CanApproveOrders = false;
                    p_CanEditShipments = false;
                    p_CanDeleteShipments = false;
                    p_CanEmailPrintExportShipments = false;
                    p_CanEditCreditMemos = false;
                    p_CanDeleteCreditMemos = false;
                    p_CanEmailPrintExportCreditMemos = false;
                    p_CanOnlyViewAssignedAssociations = true;
                    p_CanOnlyViewAssignedProvinces = true;
                    p_CanOnlyViewAssignedCatalogs = true;
                    p_CanOnlyViewOwnCustomers = true;
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
            SqlParameter param;

            try
            {
                DataHelper dh = new DataHelper();

                command = new SqlCommand("spUserSave");

                param = new SqlParameter("@UserID", p_UserID)
                {
                    Direction = ParameterDirection.InputOutput
                };
                command.Parameters.Add(param);

                command.Parameters.AddWithValue("@FirstName", p_FirstName);
                command.Parameters.AddWithValue("@LastName", p_LastName.Trim());
                command.Parameters.AddWithValue("@EmailAddress", p_EmailAddress.Trim());
                command.Parameters.AddWithValue("@Password", FCSEncryption.Encrypt(p_PasswordUnEncrypted));
                command.Parameters.AddWithValue("@Active", p_Active);
                command.Parameters.AddWithValue("@DefaultToPersonalJournals", p_DefaultToPersonalJournals);
                command.Parameters.AddWithValue("@DefaultToPersonalOrders", p_DefaultToPersonalOrders);
                command.Parameters.AddWithValue("@DefaultToPersonalShipments", p_DefaultToPersonalShipments);
                command.Parameters.AddWithValue("@HighlightBackgroundColour", p_HighlightBackgroundColour);
                command.Parameters.AddWithValue("@HighlightForegroundColour", p_HighlightForegroundColour);
                command.Parameters.AddWithValue("@SortPosition", p_SortPosition);
                command.Parameters.AddWithValue("@ShowSettings", p_ShowSettings);
                command.Parameters.AddWithValue("@ShowOtherMyJournals", p_ShowOtherMyJournals);
                command.Parameters.AddWithValue("@ShowOtherMyOrders", p_ShowOtherMyOrders);
                command.Parameters.AddWithValue("@ShowOtherMyShipments", p_ShowOtherMyShipments);
                command.Parameters.AddWithValue("@CanSendNewsletters", p_CanSendNewsletters);
                command.Parameters.AddWithValue("@CanCreateLabels", p_CanCreateLabels);
                command.Parameters.AddWithValue("@CanViewMyAccountActivity", p_CanViewMyAccountActivity);
                command.Parameters.AddWithValue("@CanDeleteCustomers", p_CanDeleteCustomers);
                command.Parameters.AddWithValue("@CanViewCustomerAssociations", p_CanViewCustomerAssociations);
                command.Parameters.AddWithValue("@CanMergeCustomers", p_CanMergeCustomers);
                command.Parameters.AddWithValue("@CanEditOrders", p_CanEditOrders);
                command.Parameters.AddWithValue("@CanDeleteOrders", p_CanDeleteOrders);
                command.Parameters.AddWithValue("@CanEmailPrintExportOrders", p_CanEmailPrintExportOrders);
                command.Parameters.AddWithValue("@CanApproveOrders", p_CanApproveOrders);
                command.Parameters.AddWithValue("@CanEditShipments", p_CanEditShipments);
                command.Parameters.AddWithValue("@CanDeleteShipments", p_CanDeleteShipments);
                command.Parameters.AddWithValue("@CanEmailPrintExportShipments", p_CanEmailPrintExportShipments);
                command.Parameters.AddWithValue("@CanEditCreditMemos", p_CanEditCreditMemos);
                command.Parameters.AddWithValue("@CanDeleteCreditMemos", p_CanDeleteCreditMemos);
                command.Parameters.AddWithValue("@CanEmailPrintExportCreditMemos", p_CanEmailPrintExportCreditMemos);
                command.Parameters.AddWithValue("@CanOnlyViewAssignedAssociations", p_CanOnlyViewAssignedAssociations);
                command.Parameters.AddWithValue("@CanOnlyViewAssignedProvinces", p_CanOnlyViewAssignedProvinces);
                command.Parameters.AddWithValue("@CanOnlyViewAssignedCatalogs", p_CanOnlyViewAssignedCatalogs);
                command.Parameters.AddWithValue("@CanOnlyViewOwnCustomers", p_CanOnlyViewOwnCustomers);

                dh.RunStoredProcedure(command);

                p_UserID = Convert.ToInt32(command.Parameters["@UserID"].Value.ToString());

                return true;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public bool Delete()
        {
            try
            {
                DataHelper dh = new DataHelper();
                dh.RunSQL("DELETE FROM dbo.Users WHERE UserID = " + p_UserID);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ClearAssociations()
        {
            SqlCommand command;

            try
            {
                DataHelper dh = new DataHelper();

                command = new SqlCommand("spUserClearAssociations");

                command.Parameters.AddWithValue("@UserId", p_UserID);
                dh.RunStoredProcedure(command);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SaveAssociation(long associationID)
        {
            SqlCommand command;

            try
            {
                DataHelper dh = new DataHelper();

                command = new SqlCommand("spUserSaveAssociation");

                command.Parameters.AddWithValue("@UserAsscID", 0);
                command.Parameters.AddWithValue("@UserID", p_UserID);
                command.Parameters.AddWithValue("@AssociationID", associationID);
                dh.RunStoredProcedure(command);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ClearProvinces()
        {
            SqlCommand command;

            try
            {
                DataHelper dh = new DataHelper();

                command = new SqlCommand("spUserClearStates");

                command.Parameters.Clear();
                command.Parameters.AddWithValue("@UserId", p_UserID);
                dh.RunStoredProcedure(command);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SaveProvince(string provinceID)
        {
            SqlCommand command;

            try
            {
                DataHelper dh = new DataHelper();

                command = new SqlCommand("spUserSaveState");

                command.Parameters.AddWithValue("@UserStateID", 0);
                command.Parameters.AddWithValue("@UserID", p_UserID);
                command.Parameters.AddWithValue("@StateID", provinceID);
                dh.RunStoredProcedure(command);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ClearCatalogs()
        {
            SqlCommand command;

            try
            {
                DataHelper dh = new DataHelper();

                command = new SqlCommand("spUserClearCatalogs");

                command.Parameters.AddWithValue("@UserId", p_UserID);
                dh.RunStoredProcedure(command);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SaveCatalog(string provinceID)
        {
            SqlCommand command;

            try
            {
                DataHelper dh = new DataHelper();

                command = new SqlCommand("spUserSaveCatalog");

                command.Parameters.AddWithValue("@UserCatalogID", 0);
                command.Parameters.AddWithValue("@UserID", p_UserID);
                command.Parameters.AddWithValue("@CatalogID", provinceID);
                dh.RunStoredProcedure(command);

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
                emailer.SendLostPwdEmail(p_FirstName, p_EmailAddress, p_PasswordUnEncrypted, true);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Exists
        {
            get { return flgFound; }
        }

        public int UserID
        {
            get { return p_UserID; }
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

        public string FullName
        {
            get { return p_FirstName + " " + p_LastName; }
        }

        public string Gender
        {
            get { return p_Gender; }
            set { p_Gender = value; }
        }

        public string ProfileImage
        {
            get
            {
                if (p_Gender == "F")
                {
                    return "img/userf-160x160.png";
                }
                else
                {
                    return "img/userm-160x160.png";
                }
            }
        }

        public string EmailAddress
        {
            get { return p_EmailAddress; }
            set { p_EmailAddress = value; }
        }

        public string PasswordEncrypted
        {
            get { return p_PasswordEncrypted; }
        }

        public string PasswordUnEncrypted
        {
            get { return p_PasswordUnEncrypted; }
            set { p_PasswordUnEncrypted = value; }
        }

        public bool Active
        {
            get { return p_Active; }
            set { p_Active = value; }
        }

        public bool DefaultToPersonalJournals
        {
            get { return p_DefaultToPersonalJournals; }
            set { p_DefaultToPersonalJournals = value; }
        }

        public bool DefaultToPersonalOrders
        {
            get { return p_DefaultToPersonalOrders; }
            set { p_DefaultToPersonalOrders = value; }
        }

        public bool DefaultToPersonalShipments
        {
            get { return p_DefaultToPersonalShipments; }
            set { p_DefaultToPersonalShipments = value; }
        }

        public string HighlightBackgroundColour
        {
            get { return p_HighlightBackgroundColour; }
            set { p_HighlightBackgroundColour = value; }
        }

        public string HighlightForegroundColour
        {
            get { return p_HighlightForegroundColour; }
            set { p_HighlightForegroundColour = value; }
        }

        public bool ShowCampaigns
        {
            get { return p_ShowCampaigns; }
            set { p_ShowCampaigns = value; }
        }

        public bool ShowSettings
        {
            get { return p_ShowSettings; }
            set { p_ShowSettings = value; }
        }

        public bool CanSendNewsletters
        {
            get { return p_CanSendNewsletters; }
            set { p_CanSendNewsletters = value; }
        }

        public bool CanCreateLabels
        {
            get { return p_CanCreateLabels; }
            set { p_CanCreateLabels = value; }
        }

        public bool CanViewMyAccountActivity
        {
            get { return p_CanViewMyAccountActivity; }
            set { p_CanViewMyAccountActivity = value; }
        }

        public bool ShowOtherMyJournals
        {
            get { return p_ShowOtherMyJournals; }
            set { p_ShowOtherMyJournals = value; }
        }

        public bool ShowOtherMyOrders
        {
            get { return p_ShowOtherMyOrders; }
            set { p_ShowOtherMyOrders = value; }
        }

        public bool ShowOtherMyShipments
        {
            get { return p_ShowOtherMyShipments; }
            set { p_ShowOtherMyShipments = value; }
        }

        public bool CanDeleteCustomers
        {
            get { return p_CanDeleteCustomers; }
            set { p_CanDeleteCustomers = value; }
        }

        public bool CanViewCustomerAssociations
        {
            get { return p_CanViewCustomerAssociations; }
            set { p_CanViewCustomerAssociations = value; }
        }

        public bool CanMergeCustomers
        {
            get { return p_CanMergeCustomers; }
            set { p_CanMergeCustomers = value; }
        }

        public bool CanEditOrders
        {
            get { return p_CanEditOrders; }
            set { p_CanEditOrders = value; }
        }

        public bool CanDeleteOrders
        {
            get { return p_CanDeleteOrders; }
            set { p_CanDeleteOrders = value; }
        }

        public bool CanEmailPrintExportOrders
        {
            get { return p_CanEmailPrintExportOrders; }
            set { p_CanEmailPrintExportOrders = value; }
        }

        public bool CanApproveOrders
        {
            get { return p_CanApproveOrders; }
            set { p_CanApproveOrders = value; }
        }

        public bool CanEditShipments
        {
            get { return p_CanEditShipments; }
            set { p_CanEditShipments = value; }
        }

        public bool CanDeleteShipments
        {
            get { return p_CanDeleteShipments; }
            set { p_CanDeleteShipments = value; }
        }

        public bool CanEmailPrintExportShipments
        {
            get { return p_CanEmailPrintExportShipments; }
            set { p_CanEmailPrintExportShipments = value; }
        }

        public bool CanEditCreditMemos
        {
            get { return p_CanEditCreditMemos; }
            set { p_CanEditCreditMemos = value; }
        }

        public bool CanDeleteCreditMemos
        {
            get { return p_CanDeleteCreditMemos; }
            set { p_CanDeleteCreditMemos = value; }
        }

        public bool CanEmailPrintExportCreditMemos
        {
            get { return p_CanEmailPrintExportCreditMemos; }
            set { p_CanEmailPrintExportCreditMemos = value; }
        }

        public bool CanOnlyViewAssignedAssociations
        {
            get { return p_CanOnlyViewAssignedAssociations; }
            set { p_CanOnlyViewAssignedAssociations = value; }
        }

        public bool CanOnlyViewAssignedProvinces
        {
            get { return p_CanOnlyViewAssignedProvinces; }
            set { p_CanOnlyViewAssignedProvinces = value; }
        }

        public bool CanOnlyViewAssignedCatalogs
        {
            get { return p_CanOnlyViewAssignedCatalogs; }
            set { p_CanOnlyViewAssignedCatalogs = value; }
        }

        public bool CanOnlyViewOwnCustomers
        {
            get { return p_CanOnlyViewOwnCustomers; }
            set { p_CanOnlyViewOwnCustomers = value; }
        }

        public DataTable Associations
        {
            get
            {
                try
                {
                    DataHelper dh = new DataHelper();
                    return dh.GetDataTableSQL("SELECT dbo.Associations.AssociationID, dbo.Associations.AsscDesc FROM dbo.UserAssc INNER JOIN dbo.Associations ON dbo.UserAssc.AssociationID = dbo.Associations.AssociationID WHERE (dbo.UserAssc.UserID = " + p_UserID + ") GROUP BY dbo.Associations.AssociationID, dbo.Associations.AsscDesc ORDER BY dbo.Associations.AsscDesc");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public DataTable AvailableAssociations
        {
            get
            {
                try
                {
                    DataHelper dh = new DataHelper();
                    return dh.GetDataTableSQL("SELECT dbo.Associations.* FROM Associations WHERE Active <> 0 AND AssociationID NOT IN (SELECT AssociationID FROM UserAssc WHERE UserID = " + p_UserID + ") ORDER BY AsscDesc");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public DataTable States
        {
            get
            {
                try
                {
                    DataHelper dh = new DataHelper();
                    return dh.GetDataTableSQL("SELECT dbo.States.StateID, dbo.States.StateName FROM dbo.UserState INNER JOIN dbo.States ON dbo.UserState.StateID = dbo.States.StateID WHERE (dbo.UserState.UserID = " + p_UserID + ") GROUP BY dbo.States.StateID, dbo.States.StateName ORDER BY dbo.States.StateName");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public DataTable AvailableStates
        {
            get
            {
                try
                {
                    DataHelper dh = new DataHelper();
                    return dh.GetDataTableSQL("SELECT dbo.States.* FROM States WHERE StateID NOT IN (SELECT StateID FROM UserState WHERE UserID = " + p_UserID + ") ORDER BY StateName");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public DataTable Catalogs
        {
            get
            {
                try
                {
                    DataHelper dh = new DataHelper();
                    return dh.GetDataTableSQL("SELECT dbo.ProductCatalog.CatalogID, dbo.ProductCatalog.CatalogName FROM dbo.UserCatalog INNER JOIN dbo.ProductCatalog ON dbo.UserCatalog.CatalogID = dbo.ProductCatalog.CatalogID WHERE (dbo.UserCatalog.UserID = " + p_UserID + ") GROUP BY dbo.ProductCatalog.CatalogID, dbo.ProductCatalog.CatalogName ORDER BY dbo.ProductCatalog.CatalogName");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public DataTable AvailableCatalogs
        {
            get
            {
                try
                {
                    DataHelper dh = new DataHelper();
                    return dh.GetDataTableSQL("SELECT dbo.ProductCatalog.* FROM ProductCatalog WHERE Active <> 0 AND CatalogID NOT IN (SELECT CatalogID FROM UserCatalog WHERE UserID = " + p_UserID + ") ORDER BY CatalogName");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public int TotalFollowups
        {
            get
            {
                try
                {
                    JournalSet journals = new JournalSet();
                    return journals.GetJournals(p_UserID).Rows.Count;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public int TotalQuotes
        {
            get
            {
                try
                {
                    OrderSet orders = new OrderSet();
                    return orders.GetOrders((int)OrderSet.OrderStatus.Quotes, p_ShowOtherMyOrders ? -1 : p_UserID).Rows.Count;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public int TotalDraftOrders
        {
            get
            {
                try
                {
                    OrderSet orders = new OrderSet();
                    return orders.GetOrders((int)OrderSet.OrderStatus.DraftOrders, p_ShowOtherMyOrders ? -1 : p_UserID).Rows.Count;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public int TotalOrders
        {
            get
            {
                try
                {
                    OrderSet orders = new OrderSet();
                    return orders.GetOrders((int)OrderSet.OrderStatus.Orders, p_ShowOtherMyOrders ? -1 : p_UserID).Rows.Count;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public int TotalShipments
        {
            get
            {
                try
                {
                    ShipmentSet shipments = new ShipmentSet();
                    return shipments.GetShipments(p_ShowOtherMyShipments ? -1 : p_UserID).Rows.Count;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

    }
}