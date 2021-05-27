namespace Invoice.Models
{
    public class FirstSpecificProductTypeModel
    {
        public int IdByInvoiceNumber { get; set; }
        public int IdByInvoiceNumberYearCreation { get; set; }
        public string FirstProductType { get; set; }
        public double? FirstProductTypeQuantity { get; set; }
        public double? FirstProductTypePrice { get; set; }
    }
}
