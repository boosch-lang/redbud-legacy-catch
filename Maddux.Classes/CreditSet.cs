using FCS;
using System;
using System.Data;

namespace Maddux.Classes
{
    public class CreditSet
    {
        public CreditSet()
        {

        }

        public DataTable FindCreditMemos(string criteria, int userID)
        {
            string sql = "";
            string where = "";

            try
            {
                User currentUser = new User(userID);

                criteria = criteria.Replace("'", "''");
                criteria = criteria.Replace("*", "%");

                sql = "SELECT * FROM dbo.vwAllCredits ";
                where = "";

                if (AppUtils.IsNumeric(criteria))
                {
                    where = "CreditID = " + criteria + " ";
                }
                else
                {
                    where = "CustomerName LIKE '%" + criteria + "%' ";
                }

                if (!currentUser.ShowOtherMyOrders)
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

                sql += " ORDER BY CreditID";

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