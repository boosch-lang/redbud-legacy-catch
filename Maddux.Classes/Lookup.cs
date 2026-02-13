using FCS;
using System;
using System.Data;
using System.Web.UI.WebControls;

namespace Maddux.Classes
{
    public static class Lookup
    {
        public static void LoadAssociationDropDown(ref DropDownList combo, bool assignedOnly, bool addAllOption, int userID)
        {
            try
            {
                string sql;
                DataHelper dh = new DataHelper();

                sql = "SELECT AssociationID, AsscDesc \n" +
                        "FROM vwCustomersByAssociation  \n";

                if (assignedOnly)
                {
                    sql += "WHERE AssociationID IN (SELECT AssociationID FROM UserAssc WHERE UserID = " + userID.ToString() + ") \n";
                }

                sql += "GROUP BY AssociationID, AsscDesc ORDER BY AsscDesc";

                DataTable associationTable = dh.GetDataTableSQL(sql);

                if (addAllOption)
                {
                    DataRow blankRow = associationTable.NewRow();
                    blankRow["AssociationID"] = -1;
                    blankRow["AsscDesc"] = "-- All Associations --";
                    associationTable.Rows.InsertAt(blankRow, 0);
                }

                combo.DataValueField = "AssociationID";
                combo.DataTextField = "AsscDesc";
                combo.DataSource = associationTable;
                combo.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void LoadCatalogDropDown(ref DropDownList combo, bool assignedOnly, bool addAllOption, int userID)
        {
            try
            {
                string sql;
                DataHelper dh = new DataHelper();

                sql = "SELECT CatalogID, CatalogName \n" +
                        "FROM vwOrderCatalogs \n" +
                        "WHERE OrderID IN ";

                if (assignedOnly)
                {
                    sql += "(SELECT OrderID FROM vwMyOrders WHERE SalesPersonID = " + userID.ToString() + ") \n";
                }
                else
                {
                    sql += "(SELECT OrderID FROM vwMyOrders) \n";
                }

                sql += "GROUP BY CatalogID, CatalogName ORDER BY CatalogName";

                DataTable catalogTable = dh.GetDataTableSQL(sql);

                if (addAllOption)
                {
                    DataRow blankRow = catalogTable.NewRow();
                    blankRow["CatalogID"] = -1;
                    blankRow["CatalogName"] = "-- All Catalogs --";
                    catalogTable.Rows.InsertAt(blankRow, 0);
                }

                combo.DataValueField = "CatalogID";
                combo.DataTextField = "CatalogName";
                combo.DataSource = catalogTable;
                combo.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void LoadContactTitlesDropDown(ref DropDownList combo)
        {
            try
            {
                string sql;
                DataHelper dh = new DataHelper();

                sql = "SELECT * FROM supContactTitles \n" +
                        "ORDER BY TitleDesc";

                DataTable userTable = dh.GetDataTableSQL(sql);

                combo.DataValueField = "TitleDesc";
                combo.DataTextField = "TitleDesc";
                combo.DataSource = userTable;
                combo.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void LoadCountryDropDown(ref DropDownList combo, bool addAllOption)
        {
            try
            {
                string sql;
                DataHelper dh = new DataHelper();

                sql = "SELECT * FROM Countries \n" +
                        "ORDER BY CASE WHEN CountryCode = 'OTHER' OR CountryCode = 'UNKNOWN' THEN 1 ELSE 0 END, CountryName";

                DataTable countryTable = dh.GetDataTableSQL(sql);

                if (addAllOption)
                {
                    DataRow blankRow = countryTable.NewRow();
                    blankRow["CountryCode"] = "000";
                    blankRow["CountryName"] = "-- All countries --";
                    countryTable.Rows.InsertAt(blankRow, 0);
                }

                combo.DataValueField = "CountryCode";
                combo.DataTextField = "CountryName";
                combo.DataSource = countryTable;
                combo.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void LoadProvinceDropDown(ref DropDownList combo, bool assignedOnly, bool addAllOption, bool addSelectOption, int userID)
        {
            try
            {
                string sql;
                DataHelper dh = new DataHelper();

                sql = "SELECT StateId, StateName \n" +
                        "FROM States \n";

                if (assignedOnly)
                {
                    sql += "WHERE StateID IN (SELECT StateID FROM UserState WHERE UserID = " + userID.ToString() + ") \n";
                }

                sql += "ORDER BY Country, StateName";

                DataTable stateTable = dh.GetDataTableSQL(sql);

                if (addAllOption)
                {
                    DataRow blankRow = stateTable.NewRow();
                    blankRow["StateId"] = "00";
                    blankRow["StateName"] = "-- All Provinces --";
                    stateTable.Rows.InsertAt(blankRow, 0);
                }

                if (addSelectOption)
                {
                    DataRow blankRow = stateTable.NewRow();
                    blankRow["StateId"] = "--";
                    blankRow["StateName"] = "-- Select Province --";
                    stateTable.Rows.InsertAt(blankRow, 0);
                }

                combo.DataValueField = "StateId";
                combo.DataTextField = "StateName";
                combo.DataSource = stateTable;
                combo.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void LoadUserDropdown(ref DropDownList combo, bool addAllOption)
        {
            try
            {
                string sql;
                DataHelper dh = new DataHelper();

                sql = "SELECT *, FirstName + N' ' + LastName AS FullName \n" +
                        "FROM Users \n" +
                        "WHERE Active <> 0 OR UserID = 0 \n" +
                        "ORDER BY FirstName, LastName";

                DataTable userTable = dh.GetDataTableSQL(sql);

                if (addAllOption)
                {
                    DataRow blankRow = userTable.NewRow();
                    blankRow["UserID"] = -1;
                    blankRow["FullName"] = "-- All users --";
                    userTable.Rows.InsertAt(blankRow, 0);
                }

                combo.DataValueField = "UserID";
                combo.DataTextField = "FullName";
                combo.DataSource = userTable;
                combo.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}