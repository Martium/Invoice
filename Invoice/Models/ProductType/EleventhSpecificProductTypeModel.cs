namespace Invoice.Models.ProductType
{
    public class EleventhSpecificProductTypeModel
    {
        public int IdByInvoiceNumber { get; set; }
        public int IdByInvoiceNumberYearCreation { get; set; }
        public string EleventhProductType { get; set; }
        public double? EleventhProductTypeQuantity { get; set; }
        public double? EleventhProductTypePrice { get; set; }
    }
}
