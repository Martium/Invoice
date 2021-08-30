namespace Invoice.Models.ProductInfo
{
    public class FullProductInfoModel
    {
        public int Year { get; set; }
        public string ProductName { get; set; }
        public string BarCode { get; set; }
        public string ProductSees { get; set; }
        public double? ProductPrice { get; set; }

        public string ProductType { get; set; }
        public double? ProductTypePrice { get; set; }
    }
}
