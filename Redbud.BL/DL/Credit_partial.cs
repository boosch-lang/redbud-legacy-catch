using System.Linq;

namespace Redbud.BL.DL
{
    public partial class Credit
    {
        public string SubTotal
        {
            get
            {
                var total = this.CreditItems.Sum(r => (double)r.EachPrice * r.Units);
                return total.ToString("C");
            }
        }
        public double SubTotalAmount
        {
            get
            {
                return this.CreditItems.Sum(r => (double)r.EachPrice * r.Units);
            }
        }
        public double Total
        {
            get
            {
                var total = (this.CreditItems.Sum(r => (double)r.EachPrice * r.Units)) + (double)this.GSTAmount + (double)this.FreightCredit;
                if (!this.PSTExempt)
                {
                    total = total + (double)this.PSTAmount;
                }
                return total;
            }
        }
    }
}
