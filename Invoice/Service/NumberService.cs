using System;
using System.Globalization;
using System.Windows.Forms;
using Invoice.Models;

namespace Invoice.Service
{
    public class NumberService
    {
        public double? ParseToDoubleOrNull(RichTextBox richTexBox)
        {
            double? parseToDoubleOrNull;
            try
            {
                parseToDoubleOrNull = double.Parse(richTexBox.Text, CultureInfo.InvariantCulture);
            }
            catch
            {
                parseToDoubleOrNull = null;
            }

            return parseToDoubleOrNull;
        }

        public string DoubleToStringOrEmpty(InvoiceModel invoiceModel)
        {
            string newString;

            newString = invoiceModel.FirstProductQuantity.HasValue ? invoiceModel.FirstProductQuantity.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;

            return newString;
        }
        
    }
}
