namespace Invoice.Models.Deposit
{
    public class DepositAddQuantityModel
    {
        public int Id { get; set; }
        public int InvoiceYear { get; set; }
        public double? ProductQuantity { get; set; }

    }
}
