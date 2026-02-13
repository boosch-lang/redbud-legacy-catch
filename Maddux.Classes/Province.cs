using FCS;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Maddux.Classes
{
    public class Province
    {
        private string p_ProvinceID;
        private string p_ProvinceName;
        private string p_Country;
        private bool p_GST;
        private decimal p_GSTRate;
        private bool p_PST;
        private decimal p_PSTRate;
        private bool p_HST;

        private bool flgFound;

        public Province()
            : this("")
        {
        }

        public Province(string provinceID)
        {
            LoadProvince(provinceID);
        }

        public bool Delete()
        {
            SqlCommand cmd;

            try
            {
                cmd = new SqlCommand
                {
                    CommandText = "DELETE FROM States WHERE StateID = @StateID"
                };
                cmd.Parameters.AddWithValue("@StateID", p_ProvinceID);

                DataHelper dh = new DataHelper();
                dh.RunQueryCmd(cmd);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Save()
        {
            SqlCommand cmd;

            try
            {
                cmd = new SqlCommand("spStateSave");

                cmd.Parameters.AddWithValue("@StateID", p_ProvinceID);
                cmd.Parameters.AddWithValue("@StateName", p_ProvinceName);
                cmd.Parameters.AddWithValue("@Country", p_Country);
                cmd.Parameters.AddWithValue("@GST", p_GST);
                cmd.Parameters.AddWithValue("@GSTRate", p_GSTRate);
                cmd.Parameters.AddWithValue("@PST", p_PST);
                cmd.Parameters.AddWithValue("@PSTRate", p_PSTRate);
                cmd.Parameters.AddWithValue("@HST", p_HST);

                DataHelper dh = new DataHelper();
                dh.RunStoredProcedure(cmd);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadProvince(string provinceID)
        {
            string sql;
            SqlCommand cmd;

            try
            {
                flgFound = false;
                cmd = new SqlCommand();

                sql = "SELECT * FROM States WHERE StateID = @StateID";
                cmd.Parameters.AddWithValue("@StateID", provinceID);
                cmd.CommandText = sql;

                DataHelper dh = new DataHelper();
                DataTable dt = dh.GetDataTableCmd(cmd);

                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];

                    p_ProvinceID = dr["StateID"].ToString();
                    p_ProvinceName = dr["StateName"].ToString();
                    p_Country = dr["Country"].ToString();
                    p_GST = Convert.ToBoolean(dr["GST"]);
                    p_GSTRate = Convert.ToDecimal(dr["GSTRate"]);
                    p_PST = Convert.ToBoolean(dr["PST"]);
                    p_PSTRate = Convert.ToDecimal(dr["PSTRate"]);
                    p_HST = Convert.ToBoolean(dr["HST"]);

                    flgFound = true;
                }
                else
                {
                    p_ProvinceID = "";
                    p_ProvinceName = "";
                    p_Country = "";
                    p_GST = false;
                    p_GSTRate = 0;
                    p_PST = false;
                    p_PSTRate = 0;
                    p_HST = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Exists
        {
            get { return flgFound; }
        }

        public string ProvinceID
        {
            get { return p_ProvinceID; }
            set { p_ProvinceID = value; }
        }

        public string ProvinceName
        {
            get { return p_ProvinceName; }
            set { p_ProvinceName = value; }
        }

    }
}