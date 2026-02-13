using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace FCS
{
    public class DataHelper
    {
        #region "Member Variables"

        private SqlConnection m_Conn;
        private string m_ConnStr;

        #endregion

        #region "Constructors"

        public DataHelper()
        {
            m_ConnStr = WebConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
        }

        #endregion

        #region "Methods"

        #region "Properties"

        public bool IsValid()
        {
            if (m_ConnStr.Trim().Length == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion

        private void Connect()
        {
            try
            {
                if (m_Conn == null)
                {
                    m_Conn = new SqlConnection();
                }

                m_Conn.ConnectionString = m_ConnStr;
                if (m_Conn.State == ConnectionState.Closed)
                {
                    m_Conn.Open();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Disconnect()
        {
            try
            {
                if (m_Conn.State == ConnectionState.Open)
                {
                    m_Conn.Close();
                    m_Conn.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet GetDataSetSQL(string sqlString, string dataTableName)
        {
            SqlDataAdapter daAdapter;
            DataSet dsDataSet;

            try
            {
                Connect();

                daAdapter = new SqlDataAdapter(sqlString, m_Conn);
                dsDataSet = new DataSet();

                daAdapter.Fill(dsDataSet, dataTableName);

                return dsDataSet;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                Disconnect();
            }
        }

        public DataTable GetDataTableSQL(string sqlString)
        {
            try
            {
                return GetDataSetSQL(sqlString, "dataset").Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetDataTableCmd(SqlCommand cmd)
        {
            SqlDataAdapter adapter;
            DataTable dt;

            try
            {
                Connect();

                cmd.CommandType = CommandType.Text;
                cmd.Connection = m_Conn;
                adapter = new SqlDataAdapter
                {
                    SelectCommand = cmd
                };

                dt = new DataTable();
                adapter.Fill(dt);

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Disconnect();
            }
        }

        public int RunSQL(string sqlString)
        {
            SqlCommand comSQL;

            try
            {
                Connect();
                comSQL = new SqlCommand(sqlString, m_Conn);
                return comSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Disconnect();
            }
        }

        public int RunStoredProcedure(SqlCommand command)
        {
            int recordsReturned;

            try
            {
                recordsReturned = 0;
                Connect();

                command.Connection = m_Conn;
                command.CommandType = CommandType.StoredProcedure;

                recordsReturned = command.ExecuteNonQuery();

                return recordsReturned;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Disconnect();
            }
        }

        public int RunQueryCmd(SqlCommand cmd)
        {
            int retval;

            try
            {
                Connect();
                cmd.Connection = m_Conn;
                cmd.CommandType = CommandType.Text;
                retval = cmd.ExecuteNonQuery();

                return retval;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Disconnect();
            }
        }

        public int RunSQLInsertCommand(SqlCommand command)
        {
            int retval;

            try
            {
                Connect();

                command.Connection = m_Conn;
                command.CommandType = CommandType.Text;

                retval = command.ExecuteNonQuery();

                return retval;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Disconnect();
            }
        }
        #endregion
    }
}
