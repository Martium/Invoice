namespace Invoice.Models
{
    public class MoneyReceiptModel
    {
        public string SellerInfo { get; set; }
        public string SellerFirmCode { get; set; }
        public string SerialNumber { get; set; }
        public string MoneyReceiptOfferNumber { get; set; }
        public string InvoiceDate { get; set; }
        public string AllProducts { get; set; }
        public string PriceInWords { get; set; }
        public string InvoiceMaker { get; set; }
    }
}
