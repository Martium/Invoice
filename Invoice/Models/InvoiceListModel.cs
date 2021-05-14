using System;

namespace Invoice.Models
{
    public class InvoiceListModel
    {
        public int InvoiceNumber { get; set; }
        public int InvoiceNumberYearCreation { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string BuyerName { get; set; }
        public string PaymentStatus { get; set; }

        public string TotalPriceWithPvm { get; set; }
    }
}
