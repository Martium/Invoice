namespace Invoice.Models.ProductType
{
    public class TenSpecificProductTypeModel
    {
        public int IdByInvoiceNumber { get; set; }
        public int IdByInvoiceNumberYearCreation { get; set; }
        public string TenProductType { get; set; }
        public double? TenProductTypeQuantity { get; set; }
        public double? TenProductTypePrice { get; set; }
    }
}
