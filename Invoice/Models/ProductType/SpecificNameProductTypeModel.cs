namespace Invoice.Models.ProductType
{
    public class SpecificNameProductTypeModel
    {
        public int IdByInvoiceNumber { get; set; }
        public int IdByInvoiceNumberYearCreation { get; set; }
        public string ProductTypeName { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
    }
}
