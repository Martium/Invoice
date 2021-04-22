using System.Globalization;
using System.Windows.Forms;
using Invoice.Models;

namespace Invoice.Service
{
    public class NumberService
    {
        public string ChangeCommaToDot(RichTextBox richTextBox)
        {
            string changeComma = richTextBox.Text;

            bool isComma = richTextBox.Text.Contains(",");

            if (isComma)
            {
                changeComma = richTextBox.Text.Replace(",", ".");
            }

            return changeComma;
        }

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

        public double? ParseToDoubleOrZero(RichTextBox richTextBox)
        {
            double? doubleOrDefault;
            try
            {
                doubleOrDefault = double.Parse(richTextBox.Text, CultureInfo.InvariantCulture);
            }
            catch
            {
                doubleOrDefault = 0;
            }

            return doubleOrDefault;
        }

        public string ToStringDoubleOrEmpty(InvoiceModel invoiceModel, string invoiceModelProp)
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

                case "FirstProductPrice":
                    newString = invoiceModel.FirstProductPrice.HasValue ? invoiceModel.FirstProductPrice.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
                    break;
                case "SecondProductPrice":
                    newString = invoiceModel.SecondProductPrice.HasValue ? invoiceModel.SecondProductPrice.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
                    break;
                case "ThirdProductPrice":
                    newString = invoiceModel.ThirdProductPrice.HasValue ? invoiceModel.ThirdProductPrice.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
                    break;
                case "FourthProductPrice":
                    newString = invoiceModel.FourthProductPrice.HasValue ? invoiceModel.FourthProductPrice.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
                    break;
                case "FifthProductPrice":
                    newString = invoiceModel.FifthProductPrice.HasValue ? invoiceModel.FifthProductPrice.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
                    break;
                case "SixthProductPrice":
                    newString = invoiceModel.SixthProductPrice.HasValue ? invoiceModel.SixthProductPrice.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
                    break;
                case "SeventhProductPrice":
                    newString = invoiceModel.SeventhProductPrice.HasValue ? invoiceModel.SeventhProductPrice.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
                    break;
                case "EighthProductPrice":
                    newString = invoiceModel.EighthProductPrice.HasValue ? invoiceModel.EighthProductPrice.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
                    break;
                case "NinthProductPrice":
                    newString = invoiceModel.NinthProductPrice.HasValue ? invoiceModel.NinthProductPrice.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
                    break;
                case "TenProductPrice":
                    newString = invoiceModel.TenProductPrice.HasValue ? invoiceModel.TenProductPrice.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
                    break;
                case "EleventhProductPrice":
                    newString = invoiceModel.EleventhProductPrice.HasValue ? invoiceModel.EleventhProductPrice.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
                    break;
                case "TwelfthProductPrice":
                    newString = invoiceModel.TwelfthProductPrice.HasValue ? invoiceModel.TwelfthProductPrice.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
                    break;
                 default:
                     newString = "coding map error";
                     break;
            }

            return newString;
        }

        public double? CalculateFullPrice(InvoiceQuantityAndPriceModel invoiceQuantityAndPriceModel)
        {
            double? calculateFullPrice =
                (invoiceQuantityAndPriceModel.FirstProductQuantity * invoiceQuantityAndPriceModel.FirstProductPrice) +
                (invoiceQuantityAndPriceModel.SecondProductQuantity * invoiceQuantityAndPriceModel.SecondProductPrice) +
                (invoiceQuantityAndPriceModel.ThirdProductQuantity * invoiceQuantityAndPriceModel.ThirdProductPrice) +
                (invoiceQuantityAndPriceModel.FourthProductQuantity * invoiceQuantityAndPriceModel.FourthProductPrice) +
                (invoiceQuantityAndPriceModel.FifthProductQuantity * invoiceQuantityAndPriceModel.FifthProductPrice) +
                (invoiceQuantityAndPriceModel.SixthProductQuantity * invoiceQuantityAndPriceModel.SixthProductPrice) +
                (invoiceQuantityAndPriceModel.SeventhProductQuantity * invoiceQuantityAndPriceModel.SeventhProductPrice) +
                (invoiceQuantityAndPriceModel.EighthProductQuantity * invoiceQuantityAndPriceModel.EighthProductPrice) +
                (invoiceQuantityAndPriceModel.NinthProductQuantity * invoiceQuantityAndPriceModel.NinthProductPrice) +
                (invoiceQuantityAndPriceModel.TenProductQuantity * invoiceQuantityAndPriceModel.TenProductPrice) +
                (invoiceQuantityAndPriceModel.EleventhProductQuantity * invoiceQuantityAndPriceModel.EleventhProductPrice) +
                (invoiceQuantityAndPriceModel.TwelfthProductQuantity * invoiceQuantityAndPriceModel.TwelfthProductPrice);

            return calculateFullPrice;
        }

        public double? CalculatePvm(RichTextBox richTextBox)
        {
            double? calculatePvm = double.Parse(richTextBox.Text, CultureInfo.InvariantCulture) * 21 / 100;
            return calculatePvm;
        }

        public double? CalculateTotalPriceWithPvm(RichTextBox productTotalPriceRichTextBox, RichTextBox pvmPriceRichTextBox)
        {
            double? calculateTotalPriceWithPvm = double.Parse(productTotalPriceRichTextBox.Text, CultureInfo.InvariantCulture) +
                                                 double.Parse(pvmPriceRichTextBox.Text, CultureInfo.InvariantCulture);

            return calculateTotalPriceWithPvm;
        }
    }
}
