using System.Linq;

namespace Redbud.BL.DL
{
    public partial class RackProduct
    {
        public string ProductName
        {
            get

            {
                return this.Product.ProductName;
            }
        }

        public bool HasProductPhoto
        {
            get
            {
                return !string.IsNullOrWhiteSpace(this.ProductPhotoPath);
            }
        }

        public string ProductPhotoPath
        {
            get
            {
                var firstPhoto = this.Product.Photos.FirstOrDefault();
                if (firstPhoto != null)
                    return firstPhoto.PhotoPath;
                else
                    return null;
            }
        }
        public double UnitPrice
        {
            get
            {
                return this.Product.UnitPrice;
            }
        }
        public int CaseQuantity
        {
            get
            {
                return this.Product.ItemsPerPackage;
            }
        }
        public int UnitPerCase
        {
            get
            {
                return this.Product.PackagesPerUnit;
            }
        }
        public string UnitPriceFormatted
        {
            get
            {
                return this.Product.UnitPrice.ToString("C2");
            }
        }
    }
}
