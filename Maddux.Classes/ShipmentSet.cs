using FCS;
using System;
using System.Data;

namespace Maddux.Classes
{
    public class ShipmentSet
    {
        public ShipmentSet()
        {

        }

        public DataTable GetShipments(int salesPersonID)
        {
            return GetShipments(salesPersonID, "00", -1);
        }

        public DataTable GetShipments(int salesPersonID, string provinceID, int catalogID)
        {
            string sql;
            string where = "";

            try
            {
                sql = "SELECT * FROM vwMyShipments ";

                if (salesPersonID != -1)
                {
                    if (where.Length > 0)
                    {
                        where += "AND ";
                    }
                    where += "SalesPersonID = " + salesPersonID.ToString();
                }

                if (provinceID != "00")
                {
                    if (where.Length > 0)
                    {
                        where += "AND ";
                    }
                    where += "State = '" + provinceID + "'";
                }

                if (catalogID != -1)
                {
                    if (where.Length > 0)
                    {
                        where += "AND ";
                    }
                    where += "ShipmentID IN (SELECT ShipmentID FROM dbo.vwShipmentCatalogs WHERE CatalogID = " + catalogID + ") ";
                }


                if (where.Length > 0)
                {
                    sql += "WHERE " + where;
                }

                DataHelper dh = new DataHelper();
                return dh.GetDataTableSQL(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable FindShipments(string criteria, int userID)
        {
            string sql = "";
            string where = "";

            try
            {
                User currentUser = new User(userID);

                criteria = criteria.Replace("'", "''");
                criteria = criteria.Replace("*", "%");

                sql = "SELECT * FROM dbo.vwAllShipments ";
                where = "";

                if (AppUtils.IsNumeric(criteria))
                {
                    where = "(ShipmentID = " + criteria + " OR OrderID = " + criteria + ") ";
                }
                else
                {
                    where = "ShippingName LIKE '%" + criteria + "%' ";
                }

                if (!currentUser.ShowOtherMyShipments)
                {
                    if (where.Length > 0)
                    {
                        where += "AND ";
                    }
                    where += "SalesPersonID = " + currentUser.UserID.ToString() + " ";
                }

                if (where.Length > 0)
                {
                    sql += "WHERE " + where;
                }


                sql += " ORDER BY ShipmentID";

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