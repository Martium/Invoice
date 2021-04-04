using System.Globalization;
using System.Windows.Forms;
using Invoice.Models;

namespace Invoice.Service
{
    public class NumberService
    {
        public double? ParseToDoubleOrNull(RichTextBox richTexBox)
        {
            double? doubleOrNull;
            try
            {
                doubleOrNull = double.Parse(richTexBox.Text, CultureInfo.InvariantCulture);
            }
            catch
            {
                doubleOrNull = null;
            }

            return doubleOrNull;
        }

        public string DoubleToStringOrEmpty(InvoiceModel invoiceModel, string invoiceModelProp)
        {
            string newString;

            switch (invoiceModelProp)
            {
                case "FirstProductQuantity":
                    newString = invoiceModel.FirstProductQuantity.HasValue ? invoiceModel.FirstProductQuantity.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
                    break;
                case "SecondProductQuantity":
                    newString = invoiceModel.SecondProductQuantity.HasValue ? invoiceModel.SecondProductQuantity.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
                    break;
                case "ThirdProductQuantity":
                    newString = invoiceModel.ThirdProductQuantity.HasValue ? invoiceModel.ThirdProductQuantity.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
                    break;
                case "FourthProductQuantity":
                    newString = invoiceModel.FourthProductQuantity.HasValue ? invoiceModel.FourthProductQuantity.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
                    break;
                case "FifthProductQuantity":
                    newString = invoiceModel.FifthProductQuantity.HasValue ? invoiceModel.FifthProductQuantity.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
                    break;
                case "SixthProductQuantity":
                    newString = invoiceModel.SixthProductQuantity.HasValue ? invoiceModel.SixthProductQuantity.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
                    break;
                case "SeventhProductQuantity":
                    newString = invoiceModel.SeventhProductQuantity.HasValue ? invoiceModel.SeventhProductQuantity.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
                    break;
                case "EighthProductQuantity":
                    newString = invoiceModel.EighthProductQuantity.HasValue ? invoiceModel.EighthProductQuantity.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
                    break;
                case "NinthProductQuantity":
                    newString = invoiceModel.NinthProductQuantity.HasValue ? invoiceModel.NinthProductQuantity.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
                    break;
                case "TenProductQuantity":
                    newString = invoiceModel.TenProductQuantity.HasValue ? invoiceModel.TenProductQuantity.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
                    break;
                case "EleventhProductQuantity":
                    newString = invoiceModel.EleventhProductQuantity.HasValue ? invoiceModel.EleventhProductQuantity.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
                    break;
                case "TwelfthProductQuantity":
                    newString = invoiceModel.TwelfthProductQuantity.HasValue ? invoiceModel.TwelfthProductQuantity.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
                    break;
                 default:
                     newString = "coding map error";
                     break;
            }

            return newString;

        }
        
    }
}
