namespace Redbud.BL.DL
{
    public partial class ProductProgramShipDate
    {
        public string FormattedShipDate
        {
            get
            {
                return this.ShipDate.ToString("yyyy-MM-dd");
            }
        }

        public string FormattedShipDateLong
        {
            get
            {
                return this.ShipDate.ToString("MMMM d, yyyy");
            }
        }
    }
}
