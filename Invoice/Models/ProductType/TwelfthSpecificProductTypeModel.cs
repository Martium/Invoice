namespace Invoice.Models.ProductType
{
    public class TwelfthSpecificProductTypeModel
    {
        public int IdByInvoiceNumber { get; set; }
        public int IdByInvoiceNumberYearCreation { get; set; }
        public string TwelfthProductType { get; set; }
        public double? TwelfthProductTypeQuantity { get; set; }
        public double? TwelfthProductTypePrice { get; set; }
    }
}
