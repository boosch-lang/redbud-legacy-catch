using System;

namespace Redbud.BL.Helpers
{
    public static class OrderHelper
    {
        public static double CalculateDiscountPrice(double unitPrice, double discountPercent)
        {
            var discount = Math.Round(unitPrice - (unitPrice * discountPercent), 2, MidpointRounding.AwayFromZero);

            return discount < 0 ? 0 : discount;
        }

        /// <summary>
        /// Gets the total base (without discount) price
        /// </summary>
        public static double GetTotalBasePrice(double unitPrice, double quantity)
        {
            var price = unitPrice < 0 ? 0 : unitPrice;
            return price * quantity;
        }

        public static double GetTotalPrice(double unitPrice, double discountPercent, double quantity)
        {
            var price = CalculateDiscountPrice(unitPrice, discountPercent);
            return price * quantity;
        }

        /// <summary>
        /// Gets the base (without discount) price for each item
        /// </summary>
        public static double CalculateEachBasePrice(double unitPrice, int itemsPerUnit)
        {
            return unitPrice < 0 ? 0 : Math.Round(unitPrice / itemsPerUnit, 2, MidpointRounding.AwayFromZero);
        }

        public static double CalculateEachPrice(double unitPrice, double discountPercent, int itemsPerUnit)
        {
            var discountedPrice = CalculateDiscountPrice(unitPrice, discountPercent);
            return discountedPrice == 0 ? 0 : Math.Round(discountedPrice / itemsPerUnit, 2, MidpointRounding.AwayFromZero);
        }

    }
}