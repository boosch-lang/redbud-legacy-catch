namespace Redbud.BL
{
    public enum ProgramFilter
    {
        Spring = 1,
        Christmas = 2,
        Easter = 3,
        MothersDay = 4,
        LateSpring = 5,
        LabourDay = 6,
        Thanksgiving = 7,
        Christmas2020 = 8,
        Custom = 9,
        EarlySummer = 10,
        Summer = 11
    }

    public enum OrderStatus
    {
        Quotes = -1,
        DraftOrders = 0,
        Orders = 1
    }
    public enum OrderReceivedVia
    {
        Online = 1,
        Phone = 2,
        Email = 3,
        Fax = 4,
        TradeShow = 5
    }
    public enum PageStatus
    {
        Draft = 0,
        Published = 1,
        Deleted = 2
    }

    public enum OrderApproved
    {
        No = 0,
        Yes = 1
    }

    public enum RackType
    {
        Standard = 0,
        Bulk = 1
    }
}

