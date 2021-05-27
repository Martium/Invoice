namespace Invoice.Models.ProductType
{
    public class EighthSpecificProductTypeModel
    {
        public int IdByInvoiceNumber { get; set; }
        public int IdByInvoiceNumberYearCreation { get; set; }
        public string EighthProductType { get; set; }
        public double? EighthProductTypeQuantity { get; set; }
        public double? EighthProductTypePrice { get; set; }
    }
}
