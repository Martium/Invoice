namespace Invoice.Models.ProductType
{
    public class ThirdSpecificProductTypeModel
    {
        public int IdByInvoiceNumber { get; set; }
        public int IdByInvoiceNumberYearCreation { get; set; }
        public string ThirdProductType { get; set; }
        public double? ThirdProductTypeQuantity { get; set; }
        public double? ThirdProductTypePrice { get; set; }
    }
}
