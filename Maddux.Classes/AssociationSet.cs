using FCS;
using System;
using System.Data;

namespace Maddux.Classes
{
    public class AssociationSet
    {
        public AssociationSet()
        {

        }

        public DataTable GetAssociationMembershipCounts(string provinceID, int userID)
        {
            string sql;
            string where = "";

            try
            {
                User currentUser = new User(userID);

                sql = "SELECT AssociationID, Class, AsscDesc, COUNT(CustomerID) AS CountCustomerID, N'' AS BlankCol \n" +
                        "FROM vwCustomersByAssociation ";

                if (provinceID != "00")
                {
                    if (where.Length > 0)
                    {
                        where += "AND ";
                    }
                    where += "CustomerID IN (SELECT CustomerID FROM dbo.Customers WHERE State = '" + provinceID + "') ";
                }
                else
                {
                    if (currentUser.CanOnlyViewAssignedProvinces)
                    {
                        if (where.Length > 0)
                        {
                            where += "AND ";
                        }
                        where += "CustomerID IN (SELECT CustomerID FROM dbo.Customers WHERE State IN (SELECT StateID FROM UserState WHERE UserID = " + currentUser.UserID + ")) ";
                    }
                }

                if (currentUser.CanOnlyViewAssignedAssociations)
                {
                    if (where.Length > 0)
                    {
                        where += "AND ";
                    }
                    where += "AssociationID IN (SELECT AssociationID FROM UserAssc WHERE UserID = " + currentUser.UserID + ") ";
                }

                if (currentUser.CanOnlyViewOwnCustomers)
                {
                    if (where.Length > 0)
                    {
                        where += "AND ";
                    }
                    where += "CustomerID IN (SELECT CustomerID FROM dbo.Customers WHERE SalesPersonID = " + currentUser.UserID + " OR SalesPersonID = 0) ";
                }

                if (where.Length > 0)
                {
                    sql += "WHERE " + where;
                }

                sql += "GROUP BY AssociationID, Class, AsscDesc ORDER BY Class, AsscDesc";

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