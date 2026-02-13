namespace Redbud.BL.DL
{
    public partial class Customer
    {
        public class QueryResult
        {
            public int CustomerID { get; set; }
            public string Company { get; set; }
            public int OrderID { get; set; }
            public string FirstName { get; internal set; }
        }
    }
}
