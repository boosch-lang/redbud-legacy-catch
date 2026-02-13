namespace Redbud.BL.DL
{
    public partial class CreditItem
    {
        public string Category
        {
            get
            {
                return this.Product.supProductSubCategory.SubCategoryDesc;
            }
        }

        public string ItemNumber
        {
            get
            {
                return this.Product.ItemNumber;

            }
        }

        public string ItemNumberInternal
        {
            get
            {
                return this.Product.ItemNumberInternal;

            }
        }
        public string Description
        {
            get
            {
                return Product.ProductName;
            }
        }
        public string Size
        {
            get
            {
                return this.Product.Size;
            }
        }
        public string ItemsPerPack
        {
            get
            {
                return this.Product.ItemsPerPackage.ToString();
            }
        }
        public string Total
        {
            get
            {
                return (EachPrice * (decimal)Units).ToString("C");
            }
        }

    }
}
