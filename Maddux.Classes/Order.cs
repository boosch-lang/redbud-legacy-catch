using FCS;
using System;
using System.Data;
using System.Data.SqlClient;


namespace Maddux.Classes
{
    public class Order
    {
        private int p_OrderID;
        private int p_CustomerID;
        private int p_SalesPersonID;
        private string p_PONumber;
        private int p_OrderStatus;
        private DateTime p_QuoteDate;
        private DateTime p_QuoteExpiryDate;
        private DateTime p_OrderDate;
        private DateTime p_RequestedShipDate;
        private DateTime p_ShippingDate;
        private int p_ShippingMethodID;
        private string p_GlobalDiscountDesc;
        private double p_GlobalDiscountPercent;
        private string p_GlobalDiscount2Desc;
        private double p_GlobalDiscount2Percent;
        private string p_GlobalDiscount3Desc;
        private double p_GlobalDiscount3Percent;
        private string p_GlobalDiscount4Desc;
        private double p_GlobalDiscount4Percent;
        private string p_GlobalDiscount5Desc;
        private double p_GlobalDiscount5Percent;
        private bool p_CustomShippingCharge;
        private double p_ShippingCharge;
        private double p_GSTAmount;
        private double p_PSTAmount;
        private bool p_HST;
        private bool p_PSTExempt;
        private int p_PaymentTypeID;
        private int p_PaymentTermsID;
        private string p_BillingName;
        private string p_BillingAddress;
        private string p_BillingCity;
        private string p_BillingState;
        private string p_BillingZip;
        private string p_BillingCountry;
        private string p_ShippingName;
        private string p_ShippingAddress;
        private string p_ShippingCity;
        private string p_ShippingState;
        private string p_ShippingZip;
        private string p_ShippingCountry;
        private string p_ShippingEmail;
        private string p_OrderNotes;
        private string p_OfficeNotes;
        private DateTime p_PurchaseOrdersSentDate;
        private DateTime p_ConfirmationSentDate;
        private string p_CreatedBy;
        private DateTime p_CreatedDate;
        private string p_UpdatedBy;
        private DateTime p_UpdatedDate;

        public Order()
            : this(0)
        { }

        public Order(int orderID)
        {
            LoadOrder("OrderID = " + orderID.ToString());
        }

        public bool Save()
        {
            SqlCommand cmd;
            SqlParameter param;

            try
            {
                DataHelper dh = new DataHelper();

                cmd = new SqlCommand("dbo.spOrderSave");

                param = new SqlParameter("@OrderID", p_OrderID)
                {
                    Direction = ParameterDirection.InputOutput
                };
                cmd.Parameters.Add(param);

                cmd.Parameters.AddWithValue("@CustomerID", p_CustomerID);
                cmd.Parameters.AddWithValue("@SalesPersonID", p_SalesPersonID);
                cmd.Parameters.AddWithValue("@PONumber", p_PONumber.ToString().Trim());
                cmd.Parameters.AddWithValue("@OrderStatus", p_OrderStatus);

                if (p_QuoteDate == DateTime.MaxValue)
                {
                    cmd.Parameters.AddWithValue("@QuoteDate", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@QuoteDate", p_QuoteDate);
                }

                if (p_QuoteExpiryDate == DateTime.MaxValue)
                {
                    cmd.Parameters.AddWithValue("@QuoteExpiryDate", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@QuoteExpiryDate", p_QuoteExpiryDate);
                }

                if (p_OrderDate == DateTime.MaxValue)
                {
                    cmd.Parameters.AddWithValue("@OrderDate", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@OrderDate", p_OrderDate);
                }

                if (p_RequestedShipDate == DateTime.MaxValue)
                {
                    cmd.Parameters.AddWithValue("@RequestedShipDate", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@RequestedShipDate", p_RequestedShipDate);
                }

                if (p_ShippingDate == DateTime.MaxValue)
                {
                    cmd.Parameters.AddWithValue("@ShipDate", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ShipDate", p_ShippingDate);
                }

                cmd.Parameters.AddWithValue("@ShippingMethodID", p_ShippingMethodID);
                cmd.Parameters.AddWithValue("@GlobalDiscountDesc", p_GlobalDiscountDesc);
                cmd.Parameters.AddWithValue("@GlobalDiscountPercent", p_GlobalDiscountPercent);
                cmd.Parameters.AddWithValue("@GlobalDiscount2Desc", p_GlobalDiscount2Desc);
                cmd.Parameters.AddWithValue("@GlobalDiscount2Percent", p_GlobalDiscount2Percent);
                cmd.Parameters.AddWithValue("@GlobalDiscount3Desc", p_GlobalDiscount3Desc);
                cmd.Parameters.AddWithValue("@GlobalDiscount3Percent", p_GlobalDiscount3Percent);
                cmd.Parameters.AddWithValue("@GlobalDiscount4Desc", p_GlobalDiscount4Desc);
                cmd.Parameters.AddWithValue("@GlobalDiscount4Percent", p_GlobalDiscount4Percent);
                cmd.Parameters.AddWithValue("@GlobalDiscount5Desc", p_GlobalDiscount5Desc);
                cmd.Parameters.AddWithValue("@GlobalDiscount5Percent", p_GlobalDiscount5Percent);
                cmd.Parameters.AddWithValue("@CustomShippingCharge", p_CustomShippingCharge);
                cmd.Parameters.AddWithValue("@ShippingCharge", p_ShippingCharge);
                cmd.Parameters.AddWithValue("@GSTAmount", p_GSTAmount);
                cmd.Parameters.AddWithValue("@PSTAmount", p_PSTAmount);

                if (p_OrderID == 0)
                {
                    p_HST = ShowHSTOnly;
                }
                cmd.Parameters.AddWithValue("@HST", p_HST);
                cmd.Parameters.AddWithValue("@PSTExempt", p_PSTExempt);
                cmd.Parameters.AddWithValue("@PaymentTypeID", p_PaymentTypeID);
                cmd.Parameters.AddWithValue("@PaymentTermsID", p_PaymentTermsID);
                cmd.Parameters.AddWithValue("@BillingName", p_BillingName.ToString().Trim());
                cmd.Parameters.AddWithValue("@BillingAddress", p_BillingAddress.ToString().Trim());
                cmd.Parameters.AddWithValue("@BillingCity", p_BillingCity.ToString().Trim());
                cmd.Parameters.AddWithValue("@BillingState", p_BillingState.ToString().Trim());
                cmd.Parameters.AddWithValue("@BillingZip", p_BillingZip.ToString().Trim());
                cmd.Parameters.AddWithValue("@BillingCountry", p_BillingCountry.ToString().Trim());
                cmd.Parameters.AddWithValue("@ShippingName", p_ShippingName.ToString().Trim());
                cmd.Parameters.AddWithValue("@ShippingAddress", p_ShippingAddress.ToString().Trim());
                cmd.Parameters.AddWithValue("@ShippingCity", p_ShippingCity.ToString().Trim());
                cmd.Parameters.AddWithValue("@ShippingState", p_ShippingState.ToString().Trim());
                cmd.Parameters.AddWithValue("@ShippingZip", p_ShippingZip.ToString().Trim());
                cmd.Parameters.AddWithValue("@ShippingCountry", p_ShippingCountry.ToString().Trim());
                cmd.Parameters.AddWithValue("@ShippingEmail", p_ShippingEmail.ToString().Trim());
                cmd.Parameters.AddWithValue("@OrderNotes", p_OrderNotes.ToString().Trim());
                cmd.Parameters.AddWithValue("@OfficeNotes", p_OfficeNotes.ToString().Trim());

                if (p_PurchaseOrdersSentDate == DateTime.MaxValue)
                {
                    cmd.Parameters.AddWithValue("@PurchaseOrdersSentDate", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@PurchaseOrdersSentDate", p_PurchaseOrdersSentDate);
                }

                if (p_ConfirmationSentDate == DateTime.MaxValue)
                {
                    cmd.Parameters.AddWithValue("@ConfirmationSentDate", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ConfirmationSentDate", p_ConfirmationSentDate);
                }

                if (p_OrderID == 0)
                {
                    cmd.Parameters.AddWithValue("@CreatedBy", System.Web.HttpContext.Current.User.Identity.Name);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@CreatedBy", p_CreatedBy.ToString());
                }

                cmd.Parameters.AddWithValue("@UpdatedBy", System.Web.HttpContext.Current.User.Identity.Name);

                dh.RunStoredProcedure(cmd);
                p_OrderID = Convert.ToInt32(cmd.Parameters["@OrderId"].Value.ToString());

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SaveFreightAndTaxes()
        {
            double subTotal, shippingRate, gstRate, pstRate;
            bool isPercent, calcGST, calcPST;

            try
            {
                DataHelper dh = new DataHelper();

                subTotal = SubTotal;

                //Calculate the freight
                if (p_CustomShippingCharge == false)
                {
                    DataTable shipRatesTable = dh.GetDataTableSQL("SELECT * FROM dbo.supShippingRates WHERE MinAmount <= " + subTotal + " AND MaxAmount >= " + subTotal);

                    if (shipRatesTable.Rows.Count != 0)
                    {
                        DataRow shipRateRow = shipRatesTable.Rows[0];
                        shippingRate = Convert.ToDouble(shipRateRow["ShippingRate"].ToString());
                        isPercent = Convert.ToBoolean(shipRateRow["IsPercent"].ToString());

                        if (isPercent == false)
                        {
                            p_ShippingCharge = shippingRate;
                        }
                        else
                        {
                            p_ShippingCharge = Math.Round(subTotal * shippingRate, 2);
                        }
                    }
                    else
                    {
                        p_ShippingCharge = 0;
                    }

                }

                subTotal = GlobalDiscountedSubTotal;

                //Calculate the taxes

                DataTable taxRateTable = dh.GetDataTableSQL("SELECT * FROM dbo.States WHERE StateID = '" + p_ShippingState + "'");

                if (taxRateTable.Rows.Count != 0)
                {
                    DataRow taxRateRow = taxRateTable.Rows[0];
                    calcGST = Convert.ToBoolean(taxRateRow["GST"].ToString());
                    gstRate = Convert.ToDouble(taxRateRow["GSTRate"].ToString());
                    calcPST = Convert.ToBoolean(taxRateRow["PST"].ToString());
                    pstRate = Convert.ToDouble(taxRateRow["PSTRate"].ToString());
                    //p_HST = Convert.ToBoolean(_drTaxRates["HST"].ToString());

                    if (calcGST == true)
                    {

                        p_GSTAmount = Math.Round((subTotal + p_ShippingCharge) * gstRate, 2);
                    }
                    else
                    {
                        p_GSTAmount = 0;
                    }

                    if (calcPST == true)
                    {
                        if (!p_PSTExempt)
                        {
                            p_PSTAmount = Math.Round((subTotal + p_ShippingCharge) * pstRate, 2);
                        }
                        else
                        {
                            p_PSTAmount = 0;
                        }
                    }
                    else
                    {
                        p_PSTAmount = 0;
                    }
                }
                else
                {
                    p_GSTAmount = 0;
                    p_PSTAmount = 0;
                }

                return Save();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete()
        {
            try
            {
                DataHelper dh = new DataHelper();
                dh.RunSQL("DELETE FROM Orders WHERE OrderID = " + p_OrderID);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadOrder(string criteria)
        {
            try
            {
                DataHelper dh = new DataHelper();
                DataTable orderTable = dh.GetDataTableSQL("SELECT * FROM dbo.Orders WHERE " + criteria);

                if (orderTable.Rows.Count != 0)
                {
                    DataRow dr = orderTable.Rows[0];

                    p_OrderID = Convert.ToInt32(dr["OrderID"]);
                    p_CustomerID = Convert.ToInt32(dr["CustomerId"]);
                    p_SalesPersonID = Convert.ToInt32(dr["SalesPersonID"]);
                    p_PONumber = dr["PONumber"].ToString().Trim();
                    p_OrderStatus = Convert.ToInt32(dr["OrderStatus"]);

                    if (dr["QuoteDate"] != null)
                    {
                        p_QuoteDate = Convert.ToDateTime(dr["QuoteDate"]);
                    }
                    else
                    {
                        p_QuoteDate = DateTime.MaxValue;
                    }

                    if (dr["QuoteExpiryDate"] != null)
                    {
                        p_QuoteExpiryDate = Convert.ToDateTime(dr["QuoteExpiryDate"]);
                    }
                    else
                    {
                        p_QuoteExpiryDate = DateTime.MaxValue;
                    }

                    if (dr["OrderDate"] != null)
                    {
                        p_OrderDate = Convert.ToDateTime(dr["OrderDate"]);
                    }
                    else
                    {
                        p_OrderDate = DateTime.MaxValue;
                    }

                    if (dr["RequestedShipDate"] != null)
                    {
                        p_RequestedShipDate = Convert.ToDateTime(dr["RequestedShipDate"]);
                    }
                    else
                    {
                        p_RequestedShipDate = DateTime.MaxValue;
                    }

                    if (dr["ShipDate"] != null)
                    {
                        p_ShippingDate = Convert.ToDateTime(dr["ShipDate"]);
                    }
                    else
                    {
                        p_ShippingDate = DateTime.MaxValue;
                    }

                    p_ShippingMethodID = Convert.ToInt32(dr["ShippingMethodID"]);
                    p_GlobalDiscountDesc = dr["GlobalDiscountDesc"].ToString();
                    p_GlobalDiscountPercent = Convert.ToDouble(dr["GlobalDiscountPercent"]);
                    p_GlobalDiscount2Desc = dr["GlobalDiscount2Desc"].ToString();
                    p_GlobalDiscount2Percent = Convert.ToDouble(dr["GlobalDiscount2Percent"]);
                    p_GlobalDiscount3Desc = dr["GlobalDiscount3Desc"].ToString();
                    p_GlobalDiscount3Percent = Convert.ToDouble(dr["GlobalDiscount3Percent"]);
                    p_GlobalDiscount4Desc = dr["GlobalDiscount4Desc"].ToString();
                    p_GlobalDiscount4Percent = Convert.ToDouble(dr["GlobalDiscount4Percent"]);
                    p_GlobalDiscount5Desc = dr["GlobalDiscount5Desc"].ToString();
                    p_GlobalDiscount5Percent = Convert.ToDouble(dr["GlobalDiscount5Percent"]);
                    p_CustomShippingCharge = Convert.ToBoolean(dr["CustomShippingCharge"]);
                    p_ShippingCharge = Convert.ToDouble(dr["ShippingCharge"]);
                    p_GSTAmount = Convert.ToDouble(dr["GSTAmount"]);
                    p_PSTAmount = Convert.ToDouble(dr["PSTAmount"]);
                    p_HST = Convert.ToBoolean(dr["HST"]);
                    p_PSTExempt = Convert.ToBoolean(dr["PSTExempt"]);
                    p_PaymentTypeID = Convert.ToInt32(dr["PaymentTypeID"]);
                    p_PaymentTermsID = Convert.ToInt32(dr["PaymentTermsID"]);
                    p_BillingName = dr["BillingName"].ToString().Trim();
                    p_BillingAddress = dr["BillingAddress"].ToString().Trim();
                    p_BillingCity = dr["BillingCity"].ToString().Trim();
                    p_BillingState = dr["BillingState"].ToString().Trim();
                    p_BillingZip = dr["BillingZip"].ToString().Trim();
                    p_BillingCountry = dr["BillingCountry"].ToString().Trim();
                    p_ShippingName = dr["ShippingName"].ToString().Trim();
                    p_ShippingAddress = dr["ShippingAddress"].ToString().Trim();
                    p_ShippingCity = dr["ShippingCity"].ToString().Trim();
                    p_ShippingState = dr["ShippingState"].ToString().Trim();
                    p_ShippingZip = dr["ShippingZip"].ToString().Trim();
                    p_ShippingCountry = dr["ShippingCountry"].ToString().Trim();
                    p_ShippingEmail = dr["ShippingEmail"].ToString().Trim();
                    p_OrderNotes = dr["OrderNotes"].ToString().Trim();
                    p_OfficeNotes = dr["OfficeNotes"].ToString().Trim();

                    if (dr["PurchaseOrdersSentDate"] != null)
                    {
                        p_PurchaseOrdersSentDate = Convert.ToDateTime(dr["PurchaseOrdersSentDate"]);
                    }
                    else
                    {
                        p_PurchaseOrdersSentDate = DateTime.MaxValue;
                    }

                    if (dr["ConfirmationSentDate"] != null)
                    {
                        p_ConfirmationSentDate = Convert.ToDateTime(dr["ConfirmationSentDate"]);
                    }
                    else
                    {
                        p_ConfirmationSentDate = DateTime.MaxValue;
                    }

                    p_CreatedBy = dr["CreatedBy"].ToString().Trim();

                    if (dr["DateCreated"] != null)
                    {
                        p_CreatedDate = Convert.ToDateTime(dr["DateCreated"]);
                    }
                    else
                    {
                        p_CreatedDate = System.DateTime.Now;
                    }

                    p_UpdatedBy = dr["UpdatedBy"].ToString().Trim();

                    if (dr["DateUpdated"] != null)
                    {
                        p_UpdatedDate = Convert.ToDateTime(dr["DateUpdated"]);
                    }
                    else
                    {
                        p_UpdatedDate = System.DateTime.Now;
                    }
                }
                else
                {
                    p_OrderID = 0;

                    p_CustomerID = 0;
                    p_SalesPersonID = 1;
                    p_PONumber = "";
                    p_OrderStatus = 0;
                    p_QuoteDate = DateTime.MaxValue;
                    p_QuoteExpiryDate = DateTime.MaxValue;
                    p_OrderDate = DateTime.MaxValue;
                    p_RequestedShipDate = DateTime.MaxValue;
                    p_ShippingDate = DateTime.MaxValue;
                    p_ShippingMethodID = 1;
                    p_GlobalDiscountDesc = "";
                    p_GlobalDiscountPercent = 0;
                    p_GlobalDiscount2Desc = "";
                    p_GlobalDiscount2Percent = 0;
                    p_GlobalDiscount3Desc = "";
                    p_GlobalDiscount3Percent = 0;
                    p_GlobalDiscount4Desc = "";
                    p_GlobalDiscount4Percent = 0;
                    p_GlobalDiscount5Desc = "";
                    p_GlobalDiscount5Percent = 0;
                    p_CustomShippingCharge = false;
                    p_ShippingCharge = 0;
                    p_GSTAmount = 0;
                    p_PSTAmount = 0;
                    p_HST = false;
                    p_PSTExempt = false;
                    p_PaymentTermsID = 0;
                    p_PaymentTypeID = 1;
                    p_BillingName = "";
                    p_BillingAddress = "";
                    p_BillingCity = "";
                    p_BillingState = "";
                    p_BillingZip = "";
                    p_BillingCountry = "";
                    p_ShippingName = "";
                    p_ShippingAddress = "";
                    p_ShippingCity = "";
                    p_ShippingState = "";
                    p_ShippingZip = "";
                    p_ShippingCountry = "";
                    p_ShippingEmail = "";
                    p_OrderNotes = "";
                    p_OfficeNotes = "";
                    p_PurchaseOrdersSentDate = DateTime.MaxValue;
                    p_ConfirmationSentDate = DateTime.MaxValue;
                    p_CreatedBy = "";
                    p_CreatedDate = DateTime.Now;
                    p_UpdatedBy = "";
                    p_UpdatedDate = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int OrderID
        {
            get { return p_OrderID; }
        }

        public int CustomerID
        {
            get { return p_CustomerID; }
            set { p_CustomerID = value; }
        }

        public int SalesPersonID
        {
            get { return p_SalesPersonID; }
            set { p_SalesPersonID = value; }
        }

        public string PONumber
        {
            get { return p_PONumber; }
            set { p_PONumber = value; }
        }

        public int OrderStatus
        {
            get { return p_OrderStatus; }
            set { p_OrderStatus = value; }
        }

        public DateTime QuoteDate
        {
            get { return p_QuoteDate; }
            set { p_QuoteDate = value; }
        }

        public DateTime QuoteExpiryDate
        {
            get { return p_QuoteExpiryDate; }
            set { p_QuoteExpiryDate = value; }
        }

        public DateTime OrderDate
        {
            get { return p_OrderDate; }
            set { p_OrderDate = value; }
        }

        public DateTime RequestedShipDate
        {
            get { return p_RequestedShipDate; }
            set { p_RequestedShipDate = value; }
        }

        public DateTime ShippingDate
        {
            get { return p_ShippingDate; }
            set { p_ShippingDate = value; }
        }

        public int ShippingMethodID
        {
            get { return p_ShippingMethodID; }
            set { p_ShippingMethodID = value; }
        }

        public string GlobalDiscount1Desc
        {
            get { return p_GlobalDiscountDesc; }
            set { p_GlobalDiscountDesc = value; }
        }

        public double GlobalDiscount1Percent
        {
            get { return p_GlobalDiscountPercent; }
            set { p_GlobalDiscountPercent = value; }
        }

        public string GlobalDiscount2Desc
        {
            get { return p_GlobalDiscount2Desc; }
            set { p_GlobalDiscount2Desc = value; }
        }

        public double GlobalDiscount2Percent
        {
            get { return p_GlobalDiscount2Percent; }
            set { p_GlobalDiscount2Percent = value; }
        }
        public string GlobalDiscount3Desc
        {
            get { return p_GlobalDiscount3Desc; }
            set { p_GlobalDiscount3Desc = value; }
        }

        public double GlobalDiscount3Percent
        {
            get { return p_GlobalDiscount3Percent; }
            set { p_GlobalDiscount3Percent = value; }
        }
        public string GlobalDiscount4Desc
        {
            get { return p_GlobalDiscount4Desc; }
            set { p_GlobalDiscount4Desc = value; }
        }

        public double GlobalDiscount4Percent
        {
            get { return p_GlobalDiscount4Percent; }
            set { p_GlobalDiscount4Percent = value; }
        }
        public string GlobalDiscount5Desc
        {
            get { return p_GlobalDiscount5Desc; }
            set { p_GlobalDiscount5Desc = value; }
        }

        public double GlobalDiscount5Percent
        {
            get { return p_GlobalDiscount5Percent; }
            set { p_GlobalDiscount5Percent = value; }
        }

        public bool CustomShippingCharge
        {
            get { return p_CustomShippingCharge; }
            set { p_CustomShippingCharge = value; }
        }

        public double ShippingCharge
        {
            get { return p_ShippingCharge; }
            set { p_ShippingCharge = value; }
        }

        public double GSTAmount
        {
            get { return p_GSTAmount; }
            set { p_GSTAmount = value; }
        }

        public double PSTAmount
        {
            get { return p_PSTAmount; }
            set { p_PSTAmount = value; }
        }

        public bool HST
        {
            get { return p_HST; }
            set { p_HST = value; }
        }

        public bool PSTExempt
        {
            get { return p_PSTExempt; }
            set { p_PSTExempt = value; }
        }

        public int PaymentTypeID
        {
            get { return p_PaymentTypeID; }
            set { p_PaymentTypeID = value; }
        }

        public int PaymentTermsID
        {
            get { return p_PaymentTermsID; }
            set { p_PaymentTermsID = value; }
        }

        public string BillingName
        {
            get { return p_BillingName; }
            set { p_BillingName = value; }
        }

        public string BillingAddress
        {
            get { return p_BillingAddress; }
            set { p_BillingAddress = value; }
        }

        public string BillingCity
        {
            get { return p_BillingCity; }
            set { p_BillingCity = value; }
        }

        public string BillingState
        {
            get { return p_BillingState; }
            set { p_BillingState = value; }
        }

        public string BillingZip
        {
            get { return p_BillingZip; }
            set { p_BillingZip = value; }
        }

        public string BillingCountry
        {
            get { return p_BillingCountry; }
            set { p_BillingCountry = value; }
        }

        public string ShippingName
        {
            get { return p_ShippingName; }
            set { p_ShippingName = value; }
        }

        public string ShippingAddress
        {
            get { return p_ShippingAddress; }
            set { p_ShippingAddress = value; }
        }

        public string ShippingCity
        {
            get { return p_ShippingCity; }
            set { p_ShippingCity = value; }
        }

        public string ShippingState
        {
            get { return p_ShippingState; }
            set { p_ShippingState = value; }
        }

        public string ShippingZip
        {
            get { return p_ShippingZip; }
            set { p_ShippingZip = value; }
        }

        public string ShippingCountry
        {
            get { return p_ShippingCountry; }
            set { p_ShippingCountry = value; }
        }

        public string ShippingEmail
        {
            get { return p_ShippingEmail; }
            set { p_ShippingEmail = value; }
        }

        public string OrderNotes
        {
            get { return p_OrderNotes; }
            set { p_OrderNotes = value; }
        }

        public string OfficeNotes
        {
            get { return p_OfficeNotes; }
            set { p_OfficeNotes = value; }
        }

        public DateTime PurchaseOrdersSentDate
        {
            get { return p_PurchaseOrdersSentDate; }
            set { p_PurchaseOrdersSentDate = value; }
        }

        public DateTime ConfirmationSentDate
        {
            get { return p_ConfirmationSentDate; }
            set { p_ConfirmationSentDate = value; }
        }

        public string CreatedBy
        {
            get { return p_CreatedBy; }
            set { p_CreatedBy = value; }
        }

        public DateTime CreatedDate
        {
            get { return p_CreatedDate; }
            set { p_CreatedDate = value; }
        }

        public string UpdatedBy
        {
            get { return p_UpdatedBy; }
            set { p_UpdatedBy = value; }
        }

        public DateTime UpdatedDate
        {
            get { return p_UpdatedDate; }
            set { p_UpdatedDate = value; }
        }

        private bool ShowHSTOnly
        {
            get
            {
                try
                {
                    DataHelper dh = new DataHelper();
                    DataTable taxTable = dh.GetDataTableSQL("SELECT HST FROM dbo.States WHERE StateID = '" + p_ShippingState + "'");

                    if (taxTable.Rows.Count != 0)
                    {
                        DataRow dr = taxTable.Rows[0];
                        return Convert.ToBoolean(dr["HST"]);
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public double TotalFlats
        {
            get
            {
                try
                {
                    DataHelper dh = new DataHelper();
                    DataTable dt = dh.GetDataTableSQL("SELECT COALESCE(SUM(Quantity),0) AS TotalQuantity FROM dbo.OrderItems WHERE dbo.OrderItems.ProductNotAvailable = 0 AND dbo.OrderItems.OrderID = " + p_OrderID);

                    if (dt.Rows.Count != 0)
                    {
                        DataRow dr = dt.Rows[0];

                        return Convert.ToDouble(dr["TotalQuantity"]);
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public double TotalWeight
        {
            get
            {
                try
                {
                    DataHelper dh = new DataHelper();
                    DataTable dt = dh.GetDataTableSQL("SELECT COALESCE(SUM(Quantity * UnitWeight),0) AS TotalWeight FROM dbo.OrderItems INNER JOIN dbo.Products ON dbo.OrderItems.ProductID = dbo.Products.ProductID WHERE dbo.OrderItems.ProductNotAvailable = 0 AND dbo.OrderItems.OrderID = " + p_OrderID);

                    if (dt.Rows.Count != 0)
                    {
                        DataRow dr = dt.Rows[0];

                        return Convert.ToDouble(dr["TotalWeight"]);
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public double TotalSize
        {
            get
            {
                try
                {
                    DataHelper dh = new DataHelper();
                    DataTable dt = dh.GetDataTableSQL("SELECT COALESCE(SUM(Quantity * UnitSize),0) AS TotalSize FROM dbo.OrderItems INNER JOIN dbo.Products ON dbo.OrderItems.ProductID = dbo.Products.ProductID WHERE dbo.OrderItems.ProductNotAvailable = 0 AND dbo.OrderItems.OrderID = " + p_OrderID);

                    if (dt.Rows.Count != 0)
                    {
                        DataRow dr = dt.Rows[0];

                        return Convert.ToDouble(dr["TotalSize"]);
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public double SubTotal
        {
            get
            {
                try
                {
                    DataHelper dh = new DataHelper();
                    DataTable dt = dh.GetDataTableSQL("SELECT COALESCE(dbo.OrderSubTotal(" + p_OrderID + "),0) AS OrderSubTotal");

                    if (dt.Rows.Count != 0)
                    {
                        DataRow dr = dt.Rows[0];

                        return Convert.ToDouble(dr["OrderSubTotal"]);
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public double DiscountTotal
        {
            get
            {
                try
                {
                    DataHelper dh = new DataHelper();
                    DataTable dt = dh.GetDataTableSQL("SELECT COALESCE(dbo.OrderDiscountTotal(" + p_OrderID + "),0) AS OrderDiscountTotal");

                    if (dt.Rows.Count != 0)
                    {
                        DataRow dr = dt.Rows[0];

                        return Convert.ToDouble(dr["OrderDiscountTotal"]);
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public double DiscountedSubTotal
        {
            get
            {
                try
                {
                    DataHelper dh = new DataHelper();
                    DataTable dt = dh.GetDataTableSQL("SELECT COALESCE(dbo.OrderDiscountedSubTotal(" + p_OrderID + "),0) AS DiscountedSubTotal");

                    if (dt.Rows.Count != 0)
                    {
                        DataRow dr = dt.Rows[0];

                        return Convert.ToDouble(dr["DiscountedSubTotal"]);
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public double GlobalDiscountAmount1
        {
            get
            {
                try
                {
                    return DiscountedSubTotal * p_GlobalDiscountPercent;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public double GlobalDiscountAmount2
        {
            get
            {
                try
                {
                    return (DiscountedSubTotal - GlobalDiscountAmount1) * p_GlobalDiscount2Percent;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public double GlobalDiscountAmount3
        {
            get
            {
                try
                {
                    return (DiscountedSubTotal - GlobalDiscountAmount1 - GlobalDiscountAmount2) * p_GlobalDiscount3Percent;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public double GlobalDiscountAmount4
        {
            get
            {
                try
                {
                    return (DiscountedSubTotal - GlobalDiscountAmount1 - GlobalDiscountAmount2 - GlobalDiscountAmount3) * p_GlobalDiscount4Percent;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public double GlobalDiscountAmount5
        {
            get
            {
                try
                {
                    return (DiscountedSubTotal - GlobalDiscountAmount1 - GlobalDiscountAmount2 - GlobalDiscountAmount3 - GlobalDiscountAmount4) * p_GlobalDiscount5Percent;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public double GlobalDiscountedSubTotal
        {
            get
            {
                try
                {
                    return DiscountedSubTotal - GlobalDiscountAmount1 - GlobalDiscountAmount2 - GlobalDiscountAmount3 - GlobalDiscountAmount4 - GlobalDiscountAmount5;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public double OrderGrandTotal
        {
            get { return 0; }
        }

        public DataTable OrderItems
        {
            get
            {
                try
                {
                    DataHelper dh = new DataHelper();
                    return dh.GetDataTableSQL("SELECT * FROM dbo.vwOrderItems WHERE OrderID = " + p_OrderID + " ORDER BY OrderID, CatalogPageStart, SubCategoryDesc, ProductName");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public DataTable OrderCatalogs
        {
            get
            {
                try
                {
                    DataHelper dh = new DataHelper();
                    return dh.GetDataTableSQL("SELECT * FROM dbo.vwOrderCatalogs WHERE OrderID = " + p_OrderID + " ORDER BY OrderID, CatalogName");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public DataTable Shipments
        {
            get
            {
                try
                {
                    DataHelper dh = new DataHelper();
                    return dh.GetDataTableSQL("SELECT * FROM dbo.vwOrderShipments WHERE OrderID = " + p_OrderID + " ORDER BY ShipmentID, CreateDate, DateShipped");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public Customer TheCustomer
        {
            get
            {
                return new Customer(p_CustomerID);
            }
        }

    }
}
