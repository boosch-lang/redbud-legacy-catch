using Redbud.BL.DL;
using System;
using System.Linq;

namespace Redbud.BL.Utils
{
    public class FreightCalculator
    {
        public static double CalculateFreighCharge(double subtotal, string province, string postalcode)
        {
            if (!string.IsNullOrWhiteSpace(postalcode) && !string.IsNullOrWhiteSpace(province))
            {
                string AreaID = postalcode.Substring(0, 3);
                using (MadduxEntities db = new MadduxEntities())
                {
                    var charge = db.FreightCharges.Where(x => x.Province == province && x.AreaID == AreaID).Select(x => x.Charge).FirstOrDefault();
                    double _charge = Convert.ToDouble(charge);
                    return Math.Round(((_charge * subtotal) / 100), 2, MidpointRounding.AwayFromZero);
                }
            }
            else
            {
                return 0;
            }
        }
    }
}
