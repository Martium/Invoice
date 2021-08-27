namespace Invoice.Models.Deposit
{
    public class FullDepositProductWithoutIdModel
    {
        public int InvoiceYear { get; set; }
        public string ProductName { get; set; }
        public string BarCode { get; set; }
        public double? ProductQuantity { get; set; }
    }
}
