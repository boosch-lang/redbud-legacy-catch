using FCS;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Maddux.Classes
{
    public class ProductPhoto
    {
        public ProductPhoto()
        {

        }

        public bool AddPhoto(int productID, string photoPath)
        {
            try
            {
                string sql;
                SqlCommand cmd;
                SqlParameter param;
                int photoID;

                DataHelper dh = new DataHelper();

                sql = "INSERT INTO dbo.Photos (PhotoDescription, PhotoPath, Notes) VALUES (@PhotoDescription, @PhotoPath, @Notes) SET @PhotoID = SCOPE_IDENTITY();";
                cmd = new SqlCommand(sql);

                param = new SqlParameter("@PhotoID", SqlDbType.Int, 4)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(param);

                cmd.Parameters.AddWithValue("@PhotoDescription", "");
                cmd.Parameters.AddWithValue("@PhotoPath", photoPath);
                cmd.Parameters.AddWithValue("@Notes", "");

                dh.RunSQLInsertCommand(cmd);
                photoID = Convert.ToInt32(cmd.Parameters["@PhotoID"].Value.ToString());

                sql = "INSERT INTO dbo.ProductPhotos (ProductID, PhotoID) VALUES (@ProductID, @PhotoID)";
                cmd = new SqlCommand(sql);
                cmd.Parameters.AddWithValue("@ProductID", productID);
                cmd.Parameters.AddWithValue("@PhotoID", photoID);
                dh.RunSQLInsertCommand(cmd);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeletePhoto(int photoID)
        {
            try
            {
                DataHelper dh = new DataHelper();

                dh.RunSQL("DELETE FROM dbo.Photos WHERE PhotoID = " + photoID.ToString());
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetProductPhotos(int productID)
        {
            try
            {
                DataHelper dh = new DataHelper();

                return dh.GetDataSetSQL("SELECT * FROM dbo.Photos INNER JOIN dbo.ProductPhotos ON dbo.Photos.PhotoID = dbo.ProductPhotos.PhotoID WHERE ProductID = " + productID, "Photos").Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
