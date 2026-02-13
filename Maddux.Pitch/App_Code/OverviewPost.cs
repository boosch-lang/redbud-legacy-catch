using System;
using System.Collections.Generic;


namespace Maddux.Pitch
{
    [Serializable]
    public class OverviewPost
    {
        public int RackID { get; set; }
        public List<OverviewProduct> Products { get; set; }
    }
    [Serializable]
    public class OverviewProduct
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
    }
}