using FCS;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Maddux.Classes
{
    public class Catalog
    {
        private long p_CatalogID;
        private string p_CatalogName;
        private string p_CustomerCatalogName;
        private long p_CatalogGroupID;
        private long p_CatalogClassID;
        private string p_CatalogSeason;
        private long p_CatalogYear;
        private bool p_Active;
        private bool p_ShowOnMyAccount;
        private bool p_ShowBothItemNumbers;
        private string p_PhotoPath;
        private string p_PDFUrl;
        private string p_OrderFormURL;
        private string p_Notes;

        private bool flgExists;

        public Catalog()
            : this(0)
        {
        }

        public Catalog(long catalogID)
        {
            try
            {
                flgExists = false;
                DataHelper dh = new DataHelper();

                DataTable dt = dh.GetDataTableSQL("SELECT * FROM dbo.ProductCatalog WHERE CatalogID = " + catalogID);

                if (dt.Rows.Count != 0)
                {
                    DataRow dr = dt.Rows[0];

                    p_CatalogID = catalogID;
                    p_CatalogName = dr["CatalogName"].ToString();
                    p_CustomerCatalogName = dr["CustomerCatalogName"].ToString();
                    p_CatalogGroupID = Convert.ToInt32(dr["CatalogGroupID"]);
                    p_CatalogClassID = Convert.ToInt32(dr["CatalogClassID"]);
                    p_CatalogSeason = dr["CatalogSeason"].ToString();
                    p_CatalogYear = Convert.ToInt32(dr["CatalogYear"]);
                    p_Active = Convert.ToBoolean(dr["Active"]);
                    p_ShowOnMyAccount = Convert.ToBoolean(dr["ShowOnMyAccount"]);
                    p_ShowBothItemNumbers = Convert.ToBoolean(dr["ShowBothItemNumbers"]);
                    p_PhotoPath = dr["PhotoPath"].ToString();
                    p_PDFUrl = dr["PDFUrl"].ToString();
                    p_OrderFormURL = dr["OrderFormURL"].ToString();
                    p_Notes = dr["Notes"].ToString();

                    flgExists = true;
                }
                else
                {
                    p_CatalogID = 0;
                    p_CatalogName = "";
                    p_CustomerCatalogName = "";
                    p_CatalogGroupID = 0;
                    p_CatalogClassID = 1;
                    p_CatalogSeason = "All Year";
                    p_CatalogYear = DateTime.Today.Year + 1;
                    p_Active = false;
                    p_ShowOnMyAccount = false;
                    p_ShowBothItemNumbers = false;
                    p_PhotoPath = "";
                    p_PDFUrl = "";
                    p_OrderFormURL = "";
                    p_Notes = "";
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public bool Exists
        {
            get { return flgExists; }
        }

        public long CatalogID
        {
            get { return p_CatalogID; }
        }

        public string CatalogName
        {
            get { return p_CatalogName; }
            set { p_CatalogName = value; }
        }

        public string CustomerCatalogName
        {
            get { return p_CustomerCatalogName; }
            set { p_CustomerCatalogName = value; }
        }

        public string DisplayCatalogName
        {
            get
            {
                if (p_CustomerCatalogName.Length == 0)
                {
                    return p_CatalogName;
                }
                else
                {
                    return p_CustomerCatalogName;
                }
            }
        }

        public long GroupId
        {
            get { return p_CatalogGroupID; }
            set { p_CatalogGroupID = value; }
        }

        public long ClassId
        {
            get { return p_CatalogClassID; }
            set { p_CatalogClassID = value; }
        }

        public string Season
        {
            get { return p_CatalogSeason; }
            set { p_CatalogSeason = value; }
        }

        public long Year
        {
            get { return p_CatalogYear; }
            set { p_CatalogYear = value; }
        }

        public bool Active
        {
            get { return p_Active; }
            set { p_Active = value; }
        }

        public bool ShowOnMyAccount
        {
            get { return p_ShowOnMyAccount; }
            set { p_ShowOnMyAccount = value; }
        }

        public bool ShowBothItemNumbers
        {
            get { return p_ShowBothItemNumbers; }
            set { p_ShowBothItemNumbers = value; }
        }

        public string PhotoPath
        {
            get { return p_PhotoPath; }
            set { p_PhotoPath = value; }
        }

        public string PDFUrl
        {
            get { return p_PDFUrl; }
            set { p_PDFUrl = value; }
        }

        public string OrderFormUrl
        {
            get { return p_OrderFormURL; }
            set { p_OrderFormURL = value; }
        }

        public string Notes
        {
            get { return p_Notes; }
            set { p_Notes = value; }
        }

        public DataTable Associations
        {
            get
            {
                try
                {
                    DataHelper dh = new DataHelper();
                    return dh.GetDataTableSQL("SELECT dbo.Associations.AssociationID, dbo.Associations.AsscDesc FROM dbo.AssociationCatalogs INNER JOIN dbo.Associations ON dbo.AssociationCatalogs.AssociationID = dbo.Associations.AssociationID WHERE (dbo.AssociationCatalogs.CatalogID = " + p_CatalogID + ") GROUP BY dbo.Associations.AssociationID, dbo.Associations.AsscDesc ORDER BY dbo.Associations.AsscDesc");
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
                    return dh.GetDataTableSQL("SELECT dbo.Associations.* FROM Associations WHERE Active <> 0 AND AssociationID NOT IN (SELECT AssociationID FROM AssociationCatalogs WHERE CatalogID = " + p_CatalogID + ") ORDER BY AsscDesc");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public bool Save()
        {
            SqlCommand cmd;
            SqlParameter param;

            try
            {
                DataHelper dh = new DataHelper();

                cmd = new SqlCommand("spCustomerSave");

                param = new SqlParameter("@CatalogID", p_CatalogID)
                {
                    Direction = ParameterDirection.InputOutput
                };
                cmd.Parameters.Add(param);

                cmd.Parameters.AddWithValue("@CatalogName", p_CatalogName.Trim());
                cmd.Parameters.AddWithValue("@CatalogGroupID", p_CatalogGroupID);
                cmd.Parameters.AddWithValue("@CatalogClassID", p_CatalogClassID);
                cmd.Parameters.AddWithValue("@CatalogSeason", p_CatalogSeason.Trim());
                cmd.Parameters.AddWithValue("@CatalogYear", p_CatalogYear);
                cmd.Parameters.AddWithValue("@Active", p_Active);
                cmd.Parameters.AddWithValue("@ShowOnMyAccount", p_ShowOnMyAccount);
                cmd.Parameters.AddWithValue("@ShowBothItemNumbers", p_ShowBothItemNumbers);
                cmd.Parameters.AddWithValue("@PDFUrl", p_PDFUrl.Trim());
                cmd.Parameters.AddWithValue("@OrderFormUrl", p_OrderFormURL.Trim());
                cmd.Parameters.AddWithValue("@Notes", p_Notes.Trim());

                dh.RunStoredProcedure(cmd);

                p_CatalogID = Convert.ToInt32(cmd.Parameters["@CatalogID"].Value.ToString());

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
                dh.RunSQL("DELETE FROM dbo.ProductCatalog WHERE CatalogID = " + p_CatalogID.ToString());

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ClearAssociations()
        {
            try
            {
                SqlCommand cmd;

                DataHelper dh = new DataHelper();

                cmd = new SqlCommand("dbo.spCatalogClearAssociations");
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@CatalogId", p_CatalogID);
                dh.RunStoredProcedure(cmd);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SaveAssociation(long _lAssociationID)
        {
            try
            {
                SqlCommand cmd;

                DataHelper dh = new DataHelper();

                cmd = new SqlCommand("dbo.spCatalogSaveAssociation");
                cmd.Parameters.AddWithValue("@CatalogID", p_CatalogID);
                cmd.Parameters.AddWithValue("@AssociationID", _lAssociationID);
                dh.RunStoredProcedure(cmd);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable FutureShipDates
        {
            get
            {
                string sql;

                try
                {
                    sql = "SELECT sd.*, CONVERT(VARCHAR(12), ShipDate, 107) AS FormattedShipDate \n" +
                            "FROM dbo.ProductCatalogShipDates sd " +
                            "WHERE sd.OrderDeadlineDate >= GETDATE() \n" +
                            "AND sd.CatalogID = " + p_CatalogID.ToString() + " \n" +
                            "ORDER BY ShipDate";

                    DataHelper dh = new DataHelper();
                    return dh.GetDataTableSQL(sql);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public DataTable Racks
        {
            get
            {
                string sql;

                try
                {
                    sql = "SELECT pcr.RackID, pcr.CatalogID, CASE WHEN LEN(pc.CustomerCatalogName) = 0 THEN pc.CatalogName ELSE pc.CustomerCatalogName END AS CatalogName, \n" +
                            "pcr.RackName, REPLACE(pcr.RackName, '\"', '&quot;') AS RackNameHTML, pcr.RackDesc, \n" +
                            "COALESCE((SELECT SUM(rp.DefaultQuantity * p.UnitPrice) AS Total FROM RackProducts rp INNER JOIN Products p ON rp.ProductID = p.ProductID WHERE rp.RackID = pcr.RackID),0) AS RackTotal, \n" +
                            "CASE WHEN LEN(LTRIM(RTRIM(pcr.PhotoPath))) = 0 THEN './img/rack-not-available.jpg' ELSE pcr.PhotoPath END AS PhotoPath, \n" +
                            "pcr.AllowCustomization \n" +
                            "FROM ProductCatalogRack pcr \n" +
                            "INNER JOIN ProductCatalog pc ON pcr.CatalogID = pc.CatalogID \n" +
                            "WHERE pcr.CatalogID = " + p_CatalogID.ToString();

                    DataHelper dh = new DataHelper();
                    return dh.GetDataTableSQL(sql);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
