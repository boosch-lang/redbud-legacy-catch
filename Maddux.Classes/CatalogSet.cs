using FCS;
using System;
using System.Data;

namespace Maddux.Classes
{
    public class CatalogSet
    {
        public CatalogSet()
        {

        }

        public DataTable GetCustomerCatalogs(int customerID, bool activeOnly)
        {
            string sql;

            try
            {
                sql = "SELECT pc.CatalogId, CASE WHEN LEN(pc.CustomerCatalogName) = 0 THEN pc.CatalogName ELSE pc.CustomerCatalogName END AS CatalogName, \n" +
                        "pc.CatalogYear, pc.PDFUrl, pc.OrderFormUrl, pcg.CatalogGroupDesc, \n" +
                        "pcg.CatalogGroupCustomerDesc, pcg.CatalogURL AS GroupCatalogURL, pcg.OrderFormURL AS GroupOrderFormURL, \n" +
                        "CASE WHEN LEN(LTRIM(RTRIM(pc.PhotoPath))) = 0 THEN '/img/program-not-available.jpg' ELSE pc.PhotoPath END AS PhotoPath \n" +
                        "FROM dbo.ProductCatalog pc \n" +
                        "INNER JOIN dbo.AssociationCatalogs ac ON pc.CatalogID = ac.CatalogID \n" +
                        "INNER JOIN dbo.ProductCatalogGroup pcg ON pc.CatalogGroupID = pcg.CatalogGroupID \n" +
                        "WHERE pc.ShowOnMyAccount <> 0 ";

                if (activeOnly)
                {
                    sql += "AND pc.Active <> 0 ";
                }

                sql += "AND ac.AssociationID IN \n" +
                        "(SELECT ca.AssociationID FROM CustomerAssc ca WHERE ca.CustomerID = " + customerID + ") \n" +
                        "GROUP BY pc.CatalogId, pc.CatalogName, pc.CustomerCatalogName, \n" +
                        "pc.CatalogYear, pc.PDFUrl, pc.OrderFormUrl, pcg.CatalogGroupDesc, \n" +
                        "pcg.CatalogGroupCustomerDesc, pcg.CatalogURL, pcg.OrderFormURL, pc.PhotoPath \n" +
                        "ORDER BY pcg.CatalogGroupCustomerDesc, pc.CatalogYear DESC, pc.CustomerCatalogName, pc.CatalogName ";

                DataHelper dh = new DataHelper();
                return dh.GetDataTableSQL(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetCustomerCatalogsWithRacks(int customerID, bool activeOnly)
        {
            string sql;

            try
            {
                sql = "SELECT pc.CatalogId, CASE WHEN LEN(pc.CustomerCatalogName) = 0 THEN pc.CatalogName ELSE pc.CustomerCatalogName END AS CatalogName, \n" +
                        "pc.CatalogYear, pc.PDFUrl, pc.OrderFormUrl, pcg.CatalogGroupDesc, \n" +
                        "pcg.CatalogGroupCustomerDesc, pcg.CatalogURL AS GroupCatalogURL, pcg.OrderFormURL AS GroupOrderFormURL, \n" +
                        "CASE WHEN LEN(LTRIM(RTRIM(pc.PhotoPath))) = 0 THEN '/img/program-not-available.jpg' ELSE pc.PhotoPath END AS PhotoPath \n" +
                        "FROM dbo.ProductCatalog pc \n" +
                        "INNER JOIN dbo.AssociationCatalogs ac ON pc.CatalogID = ac.CatalogID \n" +
                        "INNER JOIN dbo.ProductCatalogGroup pcg ON pc.CatalogGroupID = pcg.CatalogGroupID \n" +
                        "INNER JOIN dbo.ProductCatalogRack pcr ON pc.CatalogID = pcr.CatalogID \n" +
                        "WHERE pc.ShowOnMyAccount <> 0 ";

                if (activeOnly)
                {
                    sql += "AND pc.Active <> 0 ";
                }

                sql += "AND ac.AssociationID IN \n" +
                        "(SELECT ca.AssociationID FROM CustomerAssc ca WHERE ca.CustomerID = " + customerID + ") \n" +
                        "GROUP BY pc.CatalogId, pc.CatalogName, pc.CustomerCatalogName, \n" +
                        "pc.CatalogYear, pc.PDFUrl, pc.OrderFormUrl, pcg.CatalogGroupDesc, \n" +
                        "pcg.CatalogGroupCustomerDesc, pcg.CatalogURL, pcg.OrderFormURL, pc.PhotoPath \n" +
                        "ORDER BY pcg.CatalogGroupCustomerDesc, pc.CatalogYear DESC, pc.CustomerCatalogName, pc.CatalogName ";

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
