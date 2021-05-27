namespace Invoice.Models.ProductType
{
    public class SecondSpecificProductTypeModel
    {
        public int IdByInvoiceNumber { get; set; }
        public int IdByInvoiceNumberYearCreation { get; set; }
        public string SecondProductType { get; set; }
        public double? SecondProductTypeQuantity { get; set; }
        public double? SecondProductTypePrice { get; set; }
    }
}
