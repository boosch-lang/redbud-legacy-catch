using System;

namespace Redbud.BL.Utils
{
    public class TaxUtilities
    {
        public static string GetTaxTypeText(string state)
        {
            switch (state)
            {
                case "ON":
                case "NL":
                case "NB":
                case "NS":
                case "PE":
                    return "HST";

                default:
                    return "GST";
            }
        }

        public static double GetTaxPercentage(string state, DateTime? orderDate)
        {

            switch (state)
            {
                case "NL": // Newfoundland and Labrador
                case "NB": // New Brunswick
                case "PE": // Prince Edward Island
                    return 0.15;

                case "NS":
                    {
                        // tax rate has changed for NS as of Apr 1, 2025
                        var cutOff = new DateTime(2025, 04, 01);
                        if (orderDate == null)
                            orderDate = DateTime.Today;

                        if (orderDate.Value.Date < cutOff.Date)
                            return 0.15;

                        return 0.14;
                    }

                case "ON": // Ontario
                    return 0.13;

                default:
                    return 0.05;
                    //Alberta
                    //British Columbia
                    //Manitoba
                    //Quebec
                    //Saskatchewan

            }
        }
    }
}
