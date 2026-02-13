namespace Redbud.BL.DL
{
    public partial class RackRack
    {
        public string RackName {
            get

            {
                return $"{this.ProductRack.RackID.ToString()} - {this.ProductRack.RackName}";
            }
        }
    }
}
