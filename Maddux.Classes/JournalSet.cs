using FCS;
using System;
using System.Data;

namespace Maddux.Classes
{
    public class JournalSet
    {
        public JournalSet()
        {

        }

        public DataTable GetJournals(int userID)
        {
            return GetJournals(userID, "00", -1);
        }

        public DataTable GetJournals(int userID, string provinceID, int associationID)
        {
            string sql;
            string where = "";

            try
            {
                sql = "SELECT ImgHighlight, 0 as Checkbox, JournalID, FollowUpDate, Company, State, CustomerId, AssignedToID, AssignedToName, \n" +
                        "HighlightBackgroundColour, HighlightForegroundColour, JournalNotes \n" +
                        "FROM vwMyFollowups";

                if (userID != -1)
                {
                    if (where.Length > 0)
                    {
                        where += " AND";
                    }
                    where += " AssignedToID = " + userID.ToString();
                }

                if (provinceID != "00")
                {
                    if (where.Length > 0)
                    {
                        where += " AND";
                    }
                    where += " State = '" + provinceID + "' ";
                }

                if (associationID != -1)
                {
                    if (where.Length > 0)
                    {
                        where += " AND";
                    }
                    where += " CustomerId IN(SELECT CustomerID FROM vwCustomersByAssociation WHERE AssociationID = " + associationID.ToString() + ")";
                }

                if (where.Length > 0)
                {
                    sql += " WHERE " + where;
                }

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