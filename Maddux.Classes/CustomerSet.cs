using FCS;
using System;
using System.Data;

namespace Maddux.Classes
{
    public class CustomerSet
    {
        public CustomerSet()
        {

        }

        public DataTable GetCustomers(string criteria, int associationID, string provinceID, int userID)
        {
            string sql;
            string where = "";

            try
            {
                User currentUser = new User(userID);

                sql = "SELECT CustomerID, Company, FirstName + ' ' + LastName AS Contact, Email, City + ', ' + State AS CityState, \n" +
                        "Phone, dbo.LastJournalID(Customers.CustomerID) as JournalID, \n" +
                        "dbo.LastOrderNoAndDate(Customers.CustomerID) AS LastOrderDate, \n" +
                        "dbo.LastOrderId(Customers.CustomerID) AS LastOrderID FROM dbo.Customers \n";

                if (criteria.Length > 0)
                {
                    criteria = criteria.Replace("'", "''");

                    if (where.Length > 0)
                    {
                        where += "AND ";
                    }
                    where += "(REPLACE(Company,'''','') LIKE '%" + criteria + "%' \n" +
                            "OR REPLACE(Firstname,'''','') LIKE '%" + criteria + "%' \n" +
                            "OR REPLACE(Lastname,'''','') LIKE '%" + criteria + "%' \n" +
                            "OR (REPLACE(FirstName,'''','') + ' ' + REPLACE(LastName,'''','')) LIKE '" + criteria + "' \n" +
                            "OR REPLACE(Address,'''','') LIKE '%" + criteria + "%' \n" +
                            "OR REPLACE(City,'''','') LIKE '%" + criteria + "%' \n" +
                            "OR State LIKE '%" + criteria + "%' \n" +
                            "OR REPLACE(ZIP, ' ', '') LIKE '%" + criteria.Replace(" ", "") + "%' \n" +
                            "OR Country LIKE '%" + criteria + "%' \n" +
                            "OR REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(Phone, ' ', ''),'(',''),')',''),'-',''),'.','') LIKE '%" + criteria.Replace(" ", "").Replace("(", "").Replace(")", "").Replace("-", "").Replace(".", "") + "%' \n" +
                            "OR REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(CellPhone, ' ', ''),'(',''),')',''),'-',''),'.','') LIKE '%" + criteria.Replace(" ", "").Replace("(", "").Replace(")", "").Replace("-", "").Replace(".", "") + "%' \n" +
                            "OR Email LIKE '%" + criteria + "%') \n";
                }

                if (currentUser.CanOnlyViewOwnCustomers)
                {
                    if (where.Length > 0)
                    {
                        where += "AND ";
                    }

                    where += "(SalesPersonID = " + currentUser.UserID + " OR SalesPersonID = 0) ";
                }

                if (associationID != -1)
                {
                    if (where.Length > 0)
                    {
                        where += "AND ";
                    }
                    where += "CustomerID IN (SELECT CustomerID FROM CustomerAssc WHERE AssociationID = " + associationID + ") ";
                }
                else
                {
                    if (currentUser.CanOnlyViewAssignedAssociations)
                    {
                        if (where.Length > 0)
                        {
                            where += "AND ";
                        }
                        where += "CustomerID IN (SELECT CustomerID FROM CustomerAssc WHERE AssociationID IN (SELECT AssociationID FROM UserAssc WHERE UserID = " + currentUser.UserID + ")) ";
                    }
                }

                if (provinceID != "00")
                {
                    if (where.Length > 0)
                    {
                        where += "AND ";
                    }
                    where += "State = '" + provinceID + "' ";
                }
                else
                {
                    if (currentUser.CanOnlyViewAssignedProvinces)
                    {
                        if (where.Length > 0)
                        {
                            where += "AND ";
                        }
                        where += "State IN (SELECT StateID FROM UserState WHERE UserID = " + currentUser.UserID + ") ";
                    }
                }

                if (where.Length > 0)
                {
                    sql += "WHERE " + where;
                }

                sql += "ORDER BY Company, LastName, FirstName";

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