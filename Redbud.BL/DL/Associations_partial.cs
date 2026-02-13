using System.Linq;

namespace Redbud.BL.DL
{
    public partial class vwCustomersByAssociation
    {
        public int CountCustomerID
        {
            get
            {
                using (var db = new MadduxEntities())
                {
                    return db.vwCustomersByAssociations.Count(r => r.CustomerID == this.CustomerID);
                }

            }
        }
        public string BlankCol
        {
            get
            {
                return "";
            }
        }
    }
}
