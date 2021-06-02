namespace Invoice.Models.ProductType
{
    public class FourthSpecificProductTypeModel
    {
        public int IdByInvoiceNumber { get; set; }
        public int IdByInvoiceNumberYearCreation { get; set; }
        public string FourthProductType { get; set; }
        public double? FourthProductTypeQuantity { get; set; }
        public double? FourthProductTypePrice { get; set; }
    }
}
