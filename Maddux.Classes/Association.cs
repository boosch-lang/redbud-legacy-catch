using FCS;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Maddux.Classes
{
    public class Association
    {
        private int p_AssociationID;
        private string p_AsscShort;
        private string p_AsscDesc;
        private bool p_Calculated;
        private string p_Class;
        private bool p_Active;

        private bool flgFound;

        public Association()
            : this(-1)
        {
        }

        public Association(int associationID)
        {
            LoadAssociation(associationID);
        }

        public bool Delete()
        {
            SqlCommand cmd;

            try
            {
                cmd = new SqlCommand
                {
                    CommandText = "DELETE FROM Associations WHERE AssociationID = @AssociationID"
                };
                cmd.Parameters.AddWithValue("@AssociationID", p_AssociationID);

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
            SqlParameter param;

            try
            {
                cmd = new SqlCommand("spAssociationSave");

                param = new SqlParameter("@AssociationID", p_AssociationID)
                {
                    Direction = ParameterDirection.InputOutput
                };
                cmd.Parameters.Add(param);

                cmd.Parameters.AddWithValue("@AsscShort", p_AsscShort);
                cmd.Parameters.AddWithValue("@AsscDesc", p_AsscDesc);
                cmd.Parameters.AddWithValue("@Calculated", p_Calculated);
                cmd.Parameters.AddWithValue("@Class", p_Class);
                cmd.Parameters.AddWithValue("@Active", p_Active);

                DataHelper dh = new DataHelper();
                dh.RunStoredProcedure(cmd);

                p_AssociationID = Convert.ToInt32(cmd.Parameters["@AssociationID"].Value.ToString());

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadAssociation(int associationID)
        {
            string sql;
            SqlCommand cmd;

            try
            {
                flgFound = false;
                cmd = new SqlCommand();

                sql = "SELECT * FROM Associations WHERE AssociationID = @AssociationID";
                cmd.Parameters.AddWithValue("@AssociationID", associationID);
                cmd.CommandText = sql;

                DataHelper dh = new DataHelper();
                DataTable dt = dh.GetDataTableCmd(cmd);

                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];

                    p_AssociationID = Convert.ToInt32(dr["AssociationID"]);
                    p_AsscShort = dr["AsscShort"].ToString();
                    p_AsscDesc = dr["AsscDesc"].ToString();
                    p_Calculated = Convert.ToBoolean(dr["Calculated"]);
                    p_Class = dr["Class"].ToString();
                    p_Active = Convert.ToBoolean(dr["Active"]);

                    flgFound = true;
                }
                else
                {
                    p_AssociationID = -1;

                    p_AsscShort = "";
                    p_AsscDesc = "";
                    p_Calculated = false;
                    p_Class = "";
                    p_Active = true;
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

        public int AssociationID
        {
            get { return p_AssociationID; }
        }

        public string AsscShort
        {
            get { return p_AsscShort; }
            set { p_AsscShort = value; }
        }

        public string AsscDesc
        {
            get { return p_AsscDesc; }
            set { p_AsscDesc = value; }
        }

        public bool Calculated
        {
            get { return p_Calculated; }
            set { p_Calculated = true; }
        }

        public string Class
        {
            get { return p_Class; }
            set { p_Class = value; }
        }

        public bool Active
        {
            get { return p_Active; }
            set { p_Active = true; }
        }
    }
}