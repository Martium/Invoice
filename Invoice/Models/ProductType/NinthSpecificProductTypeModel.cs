namespace Invoice.Models.ProductType
{
    public class NinthSpecificProductTypeModel
    {
        public int IdByInvoiceNumber { get; set; }
        public int IdByInvoiceNumberYearCreation { get; set; }
        public string NinthProductType { get; set; }
        public double? NinthProductTypeQuantity { get; set; }
        public double? NinthProductTypePrice { get; set; }
    }
}
