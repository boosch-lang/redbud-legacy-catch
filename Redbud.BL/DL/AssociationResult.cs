using System;

namespace Redbud.BL.DL
{
    public partial class Customer
    {
        public class AssociationResult
        {
            public int AssociationID { get; set; }
            public string AsscDesc { get; set; }
            public string BannerMessage { get; set; }
            public DateTime? BannerStartDate { get; set; }
            public DateTime? BannerEndDate { get; set; }
            public string SalesBannerMessage { get; set; }
            public DateTime? SalesBannerStartDate { get; set; }
            public DateTime? SalesBannerEndDate { get; set; }
            public string TagLine { get; set; }
        }
    }
}
