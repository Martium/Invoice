using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Models
{
    public class InvoiceListModel
    {
        public int InvoiceNumber { get; set; }
        public int InvoiceNumberYearCreation { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string BuyerName { get; set; }
        public string InvoiceIsPaid { get; set; }
    }
}
