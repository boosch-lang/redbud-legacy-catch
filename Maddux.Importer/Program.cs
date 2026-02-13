using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Maddux.Importer
{
    class Program
    {
        static void Main(string[] args)
        {
            //Uncomment to execute
            //RacksImporter racksImporter = new RacksImporter();
            //racksImporter.ConvertCatalogsToRacks();

            //CustomerWorker customer = new CustomerWorker();
            //customer.EncryptCustomerPassword();
        }
    }
    #region CustomerWorker
    class CustomerWorker
    {
        /// <summary>
        /// Password were store as plain text by old system (oh yeah)
        /// </summary>
        public void EncryptCustomerPassword()
        {
            try
            {

                IDictionary<int, string> customerIds = new Dictionary<int, string>();
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
                {
                    DataSource = @"DELL-XPS-I7-1\SQL2016",
                    UserID = "sa",
                    Password = "webility",
                    InitialCatalog = "redbudcrm"
                };

                // Connect to SQL
                Console.WriteLine("===========================");
                Console.Write("Connecting to SQL Server ... ");
                Console.WriteLine("===========================");

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {

                    connection.Open();
                    Console.WriteLine("Done.");

                    string sql = "SELECT CustomerId, WebPassword  FROM Customers";


                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            Console.WriteLine("Reading customers.");
                            while (reader.Read())
                            {
                                Console.WriteLine("{0}", reader.GetInt32(0));
                                customerIds[reader.GetInt32(0)] = reader.GetString(1);
                            }
                            Console.WriteLine("Done.");
                        }
                    }
                    connection.Close();

                    connection.Open();

                    StringBuilder sb = new StringBuilder();
                    sb.Append("UPDATE Customers SET WebPassword =@WebPassword ");
                    sb.Append("WHERE CustomerId =@CustomerId");
                    sql = sb.ToString();

                    //Create rack from catalog
                    foreach (KeyValuePair<int, string> customer in customerIds)
                    {
                        Console.WriteLine("{0}", customer.Key);
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            Console.WriteLine("Encrypting Password for user ID: {0}", customer.Key);
                            command.Parameters.AddWithValue("@WebPassword", Redbud.BL.Utils.FCSEncryption.Encrypt(customer.Value));
                            command.Parameters.AddWithValue("@CustomerId", customer.Key);
                            int rowsAffected = command.ExecuteNonQuery();
                            Console.WriteLine("Rows affected {0}", rowsAffected);
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
    #endregion CustomerWorker
    #region RacksImporter
    class RacksImporter
    {
        /// <summary>
        /// Reads catalogs and creates racks
        /// </summary>
        public void ConvertCatalogsToRacks()
        {
            try
            {
                // Build connection string
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
                {
                    DataSource = @"DELL-XPS-I7-1\SQL2016",
                    UserID = "sa",
                    Password = "webility",
                    InitialCatalog = "redbudcrm"
                };

                IDictionary<int, string> catalogs = new Dictionary<int, string>(); //To store the catalogs we must create racks from

                // Connect to SQL
                Console.WriteLine("===========================");
                Console.Write("Connecting to SQL Server ... ");
                Console.WriteLine("===========================");
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    Console.WriteLine("Done.");

                    string sql = "SELECT * FROM ProductCatalog";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            Console.WriteLine("Reading catalogs.");
                            while (reader.Read())
                            {
                                Console.WriteLine("{0} {1}", reader.GetInt32(0), reader.GetString(1));
                                catalogs[reader.GetInt32(0)] = reader.GetString(1);
                            }
                            Console.WriteLine("Done.");
                        }
                    }
                    connection.Close();

                    Console.WriteLine("==========================");
                    Console.WriteLine("Catalog list to covert.");
                    Console.WriteLine("==========================");

                    connection.Open();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("INSERT ProductCatalogRack (CatalogID, RackName) ");
                    sb.Append("VALUES (@CatalogID, @RackName);");
                    sql = sb.ToString();

                    //Create rack from catalog
                    foreach (KeyValuePair<int, string> cat in catalogs)
                    {
                        Console.WriteLine("{0} {1}", cat.Key, cat.Value);
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            Console.WriteLine("Creating rack from catalog, Name: {0}", cat.Value);
                            command.Parameters.AddWithValue("@CatalogID", cat.Key);
                            command.Parameters.AddWithValue("@RackName", cat.Value);
                            int rowsAffected = command.ExecuteNonQuery();
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
    #endregion RacksImporter
}
