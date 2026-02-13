using FCS;
using System;
using System.Data;

namespace Maddux.Classes
{
    public class OrderSet
    {
        public enum OrderStatus
        {
            Quotes = -1,
            DraftOrders = 0,
            Orders = 1
        }

        public OrderSet()
        {

        }

        public bool SubmitDraftOrders(string orderIDs)
        {
            string sql;

            try
            {

                sql = "UPDATE Orders SET OrderStatus = 1 WHERE OrderID IN (" + orderIDs + ");";

                DataHelper dh = new DataHelper();
                dh.RunSQL(sql);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteDraftOrders(string orderIDs)
        {
            string sql;

            try
            {

                sql = "DELETE FROM Orders WHERE OrderStatus = 0 AND OrderID IN (" + orderIDs + ");";

                DataHelper dh = new DataHelper();
                dh.RunSQL(sql);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetOrders(int status, int salesPersonID)
        {
            return GetOrders(status, salesPersonID, "00", -1);
        }

        public DataTable GetOrders(int status, int salesPersonID, string provinceID, int catalogID)
        {
            string sql;

            try
            {
                sql = "SELECT * FROM vwMyOrders WHERE OrderStatus = " + status.ToString();

                if (salesPersonID != -1)
                {
                    sql += " AND SalesPersonID = " + salesPersonID.ToString();
                }

                if (provinceID != "00")
                {
                    sql += " AND State = '" + provinceID + "'";
                }

                if (catalogID != -1)
                {
                    sql += " AND OrderID IN (SELECT OrderId FROM dbo.vwOrderCatalogs WHERE CatalogID = " + catalogID + ") ";
                }

                sql += " ORDER BY RequestedShipDate, OrderID";

                DataHelper dh = new DataHelper();
                return dh.GetDataTableSQL(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable FindOrders(string criteria, int userID)
        {
            string sql = "";
            string where = "";

            try
            {
                User currentUser = new User(userID);

                criteria = criteria.Replace("'", "''");
                criteria = criteria.Replace("*", "%");

                sql = "SELECT OrderID, OrderDate, Company, State, CustomerID, CustomerPhone, Email, OrderTotal FROM dbo.vwAllOrders ";

                if (AppUtils.IsNumeric(criteria))
                {
                    where = "OrderID = " + criteria + " ";
                }
                else
                {
                    where = "Company LIKE '%" + criteria + "%' ";
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

                sql += "ORDER BY OrderID";


                DataHelper dh = new DataHelper();
                return dh.GetDataTableSQL(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetCustomerSubCustomersWithDraftOrders(int customerID)
        {
            string sql;

            try
            {
                sql = "SELECT c.CustomerID, c.Company \n" +
                        "FROM vwCustomerDraftOrders do \n" +
                        "INNER JOIN CustomersSub cs ON cs.ChildCustomerID = do.CustomerID \n" +
                        "INNER JOIN Customers c ON cs.ChildCustomerID = c.CustomerId \n" +
                        "WHERE cs.MasterCustomerID = " + customerID + " \n" +
                        "GROUP BY c.CustomerID, c.Company \n" +
                        "ORDER BY c.Company";

                DataHelper dh = new DataHelper();
                return dh.GetDataTableSQL(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetCustomerSubCustomerDraftOrders(int customerID)
        {
            string sql;

            try
            {
                sql = "SELECT do.*, \n" +
                        "STUFF((SELECT '<br />' + CASE WHEN LEN(pc.PDFUrl) > 0 THEN '<a href=\"' + pc.PDFUrl + '\" target=\"_blank\">' + CASE WHEN LEN(pc.CustomerCatalogName) > 0 THEN pc.CustomerCatalogName ELSE pc.CatalogName END + '</a>' ELSE CASE WHEN LEN(pc.CustomerCatalogName) > 0 THEN pc.CustomerCatalogName ELSE pc.CatalogName END END \n" +
                        "        FROM dbo.OrderItems oi \n" +
                        "        INNER JOIN dbo.Products p ON oi.ProductID = p.ProductID \n" +
                        "        INNER JOIN dbo.ProductCatalog pc ON p.CatalogId = pc.CatalogId \n" +
                        "        WHERE oi.OrderID = do.OrderID \n" +
                        "        GROUP BY pc.CatalogName, pc.CustomerCatalogName, pc.PDFUrl \n" +
                        "    FOR XML PATH, TYPE).value(N'.[1]', \n" +
                        " N'nvarchar(max)'),1,6, N'') AS ProgramHTML \n" +
                        "FROM vwCustomerDraftOrders do \n" +
                        "INNER JOIN CustomersSub cs ON cs.ChildCustomerID = do.CustomerID \n" +
                        "INNER JOIN Customers c ON cs.ChildCustomerID = c.CustomerId \n" +
                        "WHERE cs.MasterCustomerID = " + customerID + " \n" +
                        "ORDER BY c.Company, OrderID";

                DataHelper dh = new DataHelper();
                return dh.GetDataTableSQL(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetCustomerDraftOrders(int customerID)
        {
            string sql;

            try
            {
                sql = "SELECT do.*, \n" +
                        "STUFF((SELECT '<br />' + CASE WHEN LEN(pc.PDFUrl) > 0 THEN '<a href=\"' + pc.PDFUrl + '\" target=\"_blank\">' + CASE WHEN LEN(pc.CustomerCatalogName) > 0 THEN pc.CustomerCatalogName ELSE pc.CatalogName END + '</a>' ELSE CASE WHEN LEN(pc.CustomerCatalogName) > 0 THEN pc.CustomerCatalogName ELSE pc.CatalogName END END \n" +
                        "        FROM dbo.OrderItems oi \n" +
                        "        INNER JOIN dbo.Products p ON oi.ProductID = p.ProductID \n" +
                        "        INNER JOIN dbo.ProductCatalog pc ON p.CatalogId = pc.CatalogId \n" +
                        "        WHERE oi.OrderID = do.OrderID \n" +
                        "        GROUP BY pc.CatalogName, pc.CustomerCatalogName, pc.PDFUrl \n" +
                        "    FOR XML PATH, TYPE).value(N'.[1]', \n" +
                        " N'nvarchar(max)'),1,6, N'') AS ProgramHTML \n" +
                        "FROM vwCustomerDraftOrders do \n" +
                        "WHERE do.CustomerID = " + customerID + " \n" +
                        "ORDER BY do.OrderID";

                DataHelper dh = new DataHelper();
                return dh.GetDataTableSQL(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable GetCustomerSubCustomerWithUnshippedOrders(int customerID)
        {
            string sql;

            try
            {
                sql = "SELECT c.CustomerID, c.Company \n" +
                        "FROM vwCustomerUnshippedOrders uo \n" +
                        "INNER JOIN CustomersSub cs ON cs.ChildCustomerID = uo.CustomerID \n" +
                        "INNER JOIN Customers c ON cs.ChildCustomerID = c.CustomerId \n" +
                        "WHERE cs.MasterCustomerID = " + customerID + " \n" +
                        "GROUP BY c.CustomerID, c.Company \n" +
                        "ORDER BY c.Company";

                DataHelper dh = new DataHelper();
                return dh.GetDataTableSQL(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetCustomerUnshippedOrders(int customerID)
        {
            string sql;

            try
            {
                sql = "SELECT uo.*, \n" +
                        "STUFF((SELECT '<br />' + CASE WHEN LEN(pc.PDFUrl) > 0 THEN '<a href=\"' + pc.PDFUrl + '\" target=\"_blank\">' + CASE WHEN LEN(pc.CustomerCatalogName) > 0 THEN pc.CustomerCatalogName ELSE pc.CatalogName END + '</a>' ELSE CASE WHEN LEN(pc.CustomerCatalogName) > 0 THEN pc.CustomerCatalogName ELSE pc.CatalogName END END \n" +
                        "        FROM dbo.OrderItems oi \n" +
                        "        INNER JOIN dbo.Products p ON oi.ProductID = p.ProductID \n" +
                        "        INNER JOIN dbo.ProductCatalog pc ON p.CatalogId = pc.CatalogId \n" +
                        "        WHERE oi.OrderID = uo.OrderID \n" +
                        "        GROUP BY pc.CatalogName, pc.CustomerCatalogName, pc.PDFUrl \n" +
                        "    FOR XML PATH, TYPE).value(N'.[1]', \n" +
                        " N'nvarchar(max)'),1,6, N'') AS ProgramHTML \n" +
                        "FROM vwCustomerUnshippedOrders uo \n" +
                        "WHERE uo.CustomerID = " + customerID + " \n" +
                        "ORDER BY uo.RequestedShipDate, uo.OrderID";

                DataHelper dh = new DataHelper();
                return dh.GetDataTableSQL(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetCustomerOrderYears(int customerID, bool addRecent)
        {
            string sql;

            try
            {
                sql = "SELECT YearID, YearDesc FROM \n" +
                        "(SELECT YEAR(OrderDate) AS YearID, CAST(YEAR(OrderDate) AS varchar(5)) AS YearDesc \n" +
                        "FROM vwCustomerShippedOrders \n" +
                        "WHERE CustomerID = " + customerID + " \n" +
                        "UNION ALL \n" +
                        "SELECT YEAR(OrderDate) AS YearID, CAST(YEAR(OrderDate) AS varchar(5)) AS YearDesc \n" +
                        "FROM vwCustomerShippedOrders so \n" +
                        "INNER JOIN CustomersSub cs ON cs.ChildCustomerID = so.CustomerID \n" +
                        "WHERE cs.MasterCustomerID = " + customerID + ") vwSO \n" +
                        "GROUP BY YearID, YearDesc \n" +
                        "ORDER BY YearID DESC";

                DataHelper dh = new DataHelper();
                DataTable yearsTable = dh.GetDataTableSQL(sql);

                if (addRecent)
                {
                    DataRow newRow = yearsTable.NewRow();
                    newRow["YearID"] = "1";
                    newRow["YearDesc"] = "the past six months";
                    yearsTable.Rows.InsertAt(newRow, 0);

                    newRow = yearsTable.NewRow();
                    newRow["YearID"] = "0";
                    newRow["YearDesc"] = "the past 30 days";
                    yearsTable.Rows.InsertAt(newRow, 0);
                }

                return yearsTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetCustomerPrograms(int customerID, DateTime startDate, DateTime endDate, bool addAllOption)
        {
            string sql;

            try
            {
                sql = "SELECT vwCP.CatalogID, vwCP.CatalogName FROM \n" +
                        "(SELECT pc.CatalogID, CASE WHEN LEN(pc.CustomerCatalogName) > 0 THEN pc.CustomerCatalogName + N' (' + CONVERT(nvarchar(10), pc.CatalogYear) + N')' ELSE pc.CatalogName END AS CatalogName \n" +
                        "  FROM vwCustomerShippedOrders so \n" +
                        "  INNER JOIN dbo.OrderItems oi ON so.OrderID = oi.OrderID \n" +
                        "  INNER JOIN dbo.Products p ON oi.ProductID = p.ProductID \n" +
                        "  INNER JOIN dbo.ProductCatalog pc ON p.CatalogID = pc.CatalogID \n" +
                        "  WHERE so.CustomerID = " + customerID + " \n" +
                        "  AND RequestedShipDate >= CONVERT(datetime, '" + startDate.ToString("dd-MMM-yyyy") + "') \n" +
                        "  AND RequestedShipDate <= CONVERT(datetime, '" + endDate.ToString("dd-MMM-yyyy") + "') \n" +
                        "UNION ALL \n" +
                        "SELECT pc.CatalogID, CASE WHEN LEN(pc.CustomerCatalogName) > 0 THEN pc.CustomerCatalogName + N' (' + CONVERT(nvarchar(10), pc.CatalogYear) + N')' ELSE pc.CatalogName END AS CatalogName \n" +
                        "  FROM vwCustomerShippedOrders so \n" +
                        "  INNER JOIN CustomersSub cs ON cs.ChildCustomerID = so.CustomerID \n" +
                        "  INNER JOIN dbo.OrderItems oi ON so.OrderID = oi.OrderID \n" +
                        "  INNER JOIN dbo.Products p ON oi.ProductID = p.ProductID \n" +
                        "  INNER JOIN dbo.ProductCatalog pc ON p.CatalogID = pc.CatalogID \n" +
                        "  WHERE cs.MasterCustomerID = " + customerID + " \n" +
                        "  AND RequestedShipDate >= CONVERT(datetime, '" + startDate.ToString("dd-MMM-yyyy") + "') \n" +
                        "  AND RequestedShipDate <= CONVERT(datetime, '" + endDate.ToString("dd-MMM-yyyy") + "') \n" +
                        ") vwCP \n" +
                        "GROUP BY vwCP.CatalogID, vwCP.CatalogName \n" +
                        "ORDER BY vwCP.CatalogName";

                DataHelper dh = new DataHelper();
                DataTable programsTable = dh.GetDataTableSQL(sql);

                if (addAllOption)
                {
                    DataRow newRow = programsTable.NewRow();
                    newRow["CatalogID"] = "1";
                    newRow["CatalogName"] = "-- All Programs --";
                    programsTable.Rows.InsertAt(newRow, 0);
                }

                return programsTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetCustomerShippedOrders(int customerID, DateTime startDate, DateTime endDate, int programID)
        {
            string sql;

            try
            {
                sql = "SELECT so.*, \n" +
                        "STUFF((SELECT '<br />' + CASE WHEN LEN(pc.PDFUrl) > 0 THEN '<a href=\"' + pc.PDFUrl + '\" target=\"_blank\">' + CASE WHEN LEN(pc.CustomerCatalogName) > 0 THEN pc.CustomerCatalogName ELSE pc.CatalogName END + '</a>' ELSE CASE WHEN LEN(pc.CustomerCatalogName) > 0 THEN pc.CustomerCatalogName ELSE pc.CatalogName END END \n" +
                        "        FROM dbo.OrderItems oi \n" +
                        "        INNER JOIN dbo.Products p ON oi.ProductID = p.ProductID \n" +
                        "        INNER JOIN dbo.ProductCatalog pc ON p.CatalogId = pc.CatalogId \n" +
                        "        WHERE oi.OrderID = so.OrderID \n" +
                        "        GROUP BY pc.CatalogName, pc.CustomerCatalogName, pc.PDFUrl \n" +
                        "    FOR XML PATH, TYPE).value(N'.[1]', \n" +
                        " N'nvarchar(max)'),1,6, N'') AS ProgramHTML \n" +
                        "FROM vwCustomerShippedOrders so \n" +
                        "WHERE so.CustomerID = " + customerID + " \n" +
                        "  AND so.RequestedShipDate >= CONVERT(datetime, '" + startDate.ToString("dd-MMM-yyyy") + "') \n" +
                        "  AND so.RequestedShipDate <= CONVERT(datetime, '" + endDate.ToString("dd-MMM-yyyy") + "') \n";

                if (programID != 1)
                {
                    sql += "AND OrderID IN \n" +
                                "(SELECT oi.OrderID FROM dbo.Orders o \n" +
                                " INNER JOIN dbo.OrderItems oi ON o.OrderID = oi.OrderID \n" +
                                " INNER JOIN dbo.Products p ON oi.ProductID = p.ProductID \n" +
                                " WHERE o.CustomerID = " + customerID + " AND p.CatalogID = " + programID + ") \n";
                }

                sql += "ORDER BY RequestedShipDate, OrderID";

                DataHelper dh = new DataHelper();
                return dh.GetDataTableSQL(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetCustomerSubCustomerWithShippedOrders(int customerID, DateTime startDate, DateTime endDate, int programID)
        {
            string sql;

            try
            {
                sql = "SELECT c.CustomerID, c.Company \n" +
                        "FROM vwCustomerShippedOrders so \n" +
                        "INNER JOIN CustomersSub cs ON cs.ChildCustomerID = so.CustomerID \n" +
                        "INNER JOIN Customers c ON cs.ChildCustomerID = c.CustomerId \n" +
                        "WHERE cs.MasterCustomerID = " + customerID + " \n" +
                        "  AND so.RequestedShipDate >= CONVERT(datetime, '" + startDate.ToString("dd-MMM-yyyy") + "') \n" +
                        "  AND so.RequestedShipDate <= CONVERT(datetime, '" + endDate.ToString("dd-MMM-yyyy") + "') \n";

                if (programID != 1)
                {
                    sql += "AND so.OrderID IN \n" +
                                "(SELECT oi.OrderID FROM dbo.Orders o \n" +
                                " INNER JOIN CustomersSub cs ON cs.ChildCustomerID = o.CustomerID \n" +
                                " INNER JOIN dbo.OrderItems oi ON o.OrderID = oi.OrderID \n" +
                                " INNER JOIN dbo.Products p ON oi.ProductID = p.ProductID \n" +
                                " WHERE cs.MasterCustomerID = " + customerID + " AND p.CatalogID = " + programID + ") \n";
                }

                sql += "GROUP BY c.CustomerID, c.Company \n" +
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
}