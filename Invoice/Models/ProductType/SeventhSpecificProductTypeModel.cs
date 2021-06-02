namespace Invoice.Models.ProductType
{
    public class SeventhSpecificProductTypeModel
    {
        public int IdByInvoiceNumber { get; set; }
        public int IdByInvoiceNumberYearCreation { get; set; }
        public string SeventhProductType { get; set; }
        public double? SeventhProductTypeQuantity { get; set; }
        public double? SeventhProductTypePrice { get; set; }
    }
}
