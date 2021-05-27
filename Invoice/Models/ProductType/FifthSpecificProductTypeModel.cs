namespace Invoice.Models.ProductType
{
    public class FifthSpecificProductTypeModel
    {
        public int IdByInvoiceNumber { get; set; }
        public int IdByInvoiceNumberYearCreation { get; set; }
        public string FifthProductType { get; set; }
        public double? FifthProductTypeQuantity { get; set; }
        public double? FifthProductTypePrice { get; set; }
    }
}
