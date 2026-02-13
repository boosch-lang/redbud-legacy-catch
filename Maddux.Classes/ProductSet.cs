using FCS;
using System;
using System.Data;

namespace Maddux.Classes
{
    public class ProductSet
    {
        public ProductSet()
        {

        }

        public DataTable GetRackProducts(int rackID, bool excludeZeroQuantity)
        {
            string sql;

            try
            {
                sql = "SELECT rp.ProductID, rp.DefaultQuantity, p.ItemNumber, p.ProductName, \n" +
                        "p.Size, p.ItemsPerPackage, p.PackagesPerUnit, p.UnitPrice, rp.DefaultQuantity* p.UnitPrice AS TotalPrice, \n" +
                        "COALESCE((SELECT COUNT(pp.PhotoID) FROM ProductPhotos pp WHERE pp.ProductID = rp.ProductID),0) AS PhotoCount \n" +
                        "FROM RackProducts rp \n" +
                        "INNER JOIN Products p ON rp.ProductID = p.ProductID \n" +
                        "INNER JOIN supProductSubCategory sc ON p.ProductSubCategoryId = sc.SubCategoryID \n" +
                        "WHERE rp.RackID = " + rackID.ToString() + " \n";

                if (excludeZeroQuantity)
                {
                    sql += "AND rp.DefaultQuantity <> 0 \n";
                }

                sql += "ORDER BY p.CatalogPageStart, sc.SubCategoryDesc, p.ProductName, p.Size";

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
