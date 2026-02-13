using FCS;
using System;
using System.Data;

namespace Maddux.Classes
{
    public class Rack
    {
        private int p_RackID;
        private int p_CatalogID;
        private string p_RackName;
        private string p_RackDesc;
        private string p_PhotoPath;

        private bool flgExists;

        public Rack()
            : this(-1)
        { }

        public Rack(int rackID)
        {
            try
            {
                flgExists = false;
                DataHelper dh = new DataHelper();

                DataTable dt = dh.GetDataTableSQL("SELECT * FROM dbo.ProductCatalogRack WHERE RackID = " + rackID);

                if (dt.Rows.Count != 0)
                {
                    DataRow dr = dt.Rows[0];

                    p_RackID = rackID;
                    p_CatalogID = Convert.ToInt32(dr["CatalogID"]);
                    p_RackName = dr["RackName"].ToString();
                    p_RackDesc = dr["RackDesc"].ToString();
                    p_PhotoPath = dr["PhotoPath"].ToString();

                    flgExists = true;
                }
                else
                {
                    p_RackID = -1;
                    p_CatalogID = 0;
                    p_RackName = "";
                    p_RackDesc = "";
                    p_PhotoPath = "";
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public bool Exists
        {
            get { return flgExists; }
        }

        public int RackID
        {
            get { return p_RackID; }
        }

        public int CatalogID
        {
            get { return p_CatalogID; }
            set { p_CatalogID = value; }
        }

        public string RackName
        {
            get { return p_RackName; }
            set { p_RackName = value; }
        }

        public string RackDescription
        {
            get { return p_RackDesc; }
            set { p_RackDesc = value; }
        }

        public string PhotoPath
        {
            get { return p_PhotoPath; }
            set { p_PhotoPath = value; }
        }

        public string DisplayPhotoPath
        {
            get
            {
                if (p_PhotoPath.Length > 0)
                {
                    return p_PhotoPath;
                }
                else
                {
                    return "./img/rack-not-available.jpg";
                }
            }
        }

        public Catalog TheCatalog
        {
            get
            {
                try
                {
                    return new Catalog(p_CatalogID);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public DataTable BestMixProducts
        {
            get
            {
                try
                {
                    ProductSet products = new ProductSet();
                    return products.GetRackProducts(p_RackID, true);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public DataTable AllProducts
        {
            get
            {
                try
                {
                    ProductSet products = new ProductSet();
                    return products.GetRackProducts(p_RackID, false);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
