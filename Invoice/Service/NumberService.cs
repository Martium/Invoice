using System;
using System.Globalization;
using System.Windows.Forms;
using Invoice.Models;

namespace Invoice.Service
{
    public class NumberService
    {
        private const int RoundDigitNumber = 2;

        public string ChangeCommaToDotFromRichTextBoxText(RichTextBox richTextBox)
        {
            string changeComma = richTextBox.Text;

            bool isComma = richTextBox.Text.Contains(",");

            if (isComma)
            {
                changeComma = richTextBox.Text.Replace(",", ".");
            }

            return changeComma;
        }

        public string ChangeCommaToDotFromTextBoxText(TextBox textBox)
        {
            string changeComma = textBox.Text;

            bool isComma = textBox.Text.Contains(",");

            if (isComma)
            {
                changeComma = textBox.Text.Replace(",", ".");
            }

            return changeComma;
        }

        public double? ParseToDoubleOrNullFromRichTextBoxText(RichTextBox richTextBox)
        {
            double? doubleOrNull;
            try
            {
                doubleOrNull = double.Parse(richTextBox.Text, CultureInfo.InvariantCulture);
            }
            catch
            {
                doubleOrNull = null;
            }

            return doubleOrNull;
        }

        public double? ParseToDoubleOrNullFromTextBoxText(TextBox textBox)
        {
            double? doubleOrNull;
            try
            {
                doubleOrNull = double.Parse(textBox.Text, CultureInfo.InvariantCulture);
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

        public string DoubleToStringOrEmptyInvoiceModel(InvoiceModel invoiceModel, string invoiceModelProp)
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

                case "FirstProductTypeQuantity":
                    newString = invoiceModel.FirstProductTypeQuantity.HasValue ? invoiceModel.FirstProductTypeQuantity.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
                    break;
                case "SecondProductTypeQuantity":
                    newString = invoiceModel.SecondProductTypeQuantity.HasValue ? invoiceModel.SecondProductTypeQuantity.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
                    break;
                case "ThirdProductTypeQuantity":
                    newString = invoiceModel.ThirdProductTypeQuantity.HasValue ? invoiceModel.ThirdProductTypeQuantity.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
                    break;
                case "FourthProductTypeQuantity":
                    newString = invoiceModel.FourthProductTypeQuantity.HasValue ? invoiceModel.FourthProductTypeQuantity.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
                    break;
                case "FifthProductTypeQuantity":
                    newString = invoiceModel.FifthProductTypeQuantity.HasValue ? invoiceModel.FifthProductTypeQuantity.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
                    break;

                case "FirstProductTypePrice":
                    newString = invoiceModel.FirstProductTypePrice.HasValue ? invoiceModel.FirstProductTypePrice.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
                    break;
                case "SecondProductTypePrice":
                    newString = invoiceModel.SecondProductTypePrice.HasValue ? invoiceModel.SecondProductTypePrice.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
                    break;
                case "ThirdProductTypePrice":
                    newString = invoiceModel.ThirdProductTypePrice.HasValue ? invoiceModel.ThirdProductTypePrice.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
                    break;
                case "FourthProductTypePrice":
                    newString = invoiceModel.FourthProductTypePrice.HasValue ? invoiceModel.FourthProductTypePrice.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
                    break;
                case "FifthProductTypePrice":
                    newString = invoiceModel.FifthProductTypePrice.HasValue ? invoiceModel.FifthProductTypePrice.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
                    break;

                default:
                     newString = "wrong prop string was passed!";
                     break;
            }

            return newString;
        }

        public string DoubleToStringOrEmptyProductTypeModel(ProductTypeModel productTypeModel, string productTypeProp)
        {
            string newString;

            switch (productTypeProp)
            {
                case "FirstProductTypeQuantity":
                    newString = productTypeModel.FirstProductTypeQuantity.HasValue ? productTypeModel.FirstProductTypeQuantity.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
                    break;
                case "SecondProductTypeQuantity":
                    newString = productTypeModel.SecondProductTypeQuantity.HasValue ? productTypeModel.SecondProductTypeQuantity.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
                    break;
                case "ThirdProductTypeQuantity":
                    newString = productTypeModel.ThirdProductTypeQuantity.HasValue ? productTypeModel.ThirdProductTypeQuantity.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
                    break;
                case "FourthProductTypeQuantity":
                    newString = productTypeModel.FourthProductTypeQuantity.HasValue ? productTypeModel.FourthProductTypeQuantity.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
                    break;
                case "FifthProductTypeQuantity":
                    newString = productTypeModel.FifthProductTypeQuantity.HasValue ? productTypeModel.FifthProductTypeQuantity.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
                    break;

                case "FirstProductTypePrice":
                    newString = productTypeModel.FirstProductTypePrice.HasValue ? productTypeModel.FirstProductTypePrice.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
                    break;
                case "SecondProductTypePrice":
                    newString = productTypeModel.SecondProductTypePrice.HasValue ? productTypeModel.SecondProductTypePrice.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
                    break;
                case "ThirdProductTypePrice":
                    newString = productTypeModel.ThirdProductTypePrice.HasValue ? productTypeModel.ThirdProductTypePrice.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
                    break;
                case "FourthProductTypePrice":
                    newString = productTypeModel.FourthProductTypePrice.HasValue ? productTypeModel.FourthProductTypePrice.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
                    break;
                case "FifthProductTypePrice":
                    newString = productTypeModel.FifthProductTypePrice.HasValue ? productTypeModel.FifthProductTypePrice.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
                    break;

                default:
                    newString = "wrong prop string was passed!";
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

            if (calculateFullPrice.HasValue)
            {
                double roundCalculateFullPrice = Math.Round(calculateFullPrice.Value, RoundDigitNumber, MidpointRounding.ToEven);
                calculateFullPrice = roundCalculateFullPrice;
            }

            return calculateFullPrice;
        }

        public double? CalculatePvm(RichTextBox richTextBox)
        {
            double? calculatePvm = double.Parse(richTextBox.Text, CultureInfo.InvariantCulture) * 21 / 100;

            double roundCalculatePvm = Math.Round(calculatePvm.Value, RoundDigitNumber, MidpointRounding.ToEven);
            calculatePvm = roundCalculatePvm;

            return calculatePvm;
        }

        public double? CalculateTotalPriceWithPvm(RichTextBox productTotalPriceRichTextBox, RichTextBox pvmPriceRichTextBox)
        {
            double? calculateTotalPriceWithPvm = double.Parse(productTotalPriceRichTextBox.Text, CultureInfo.InvariantCulture) +
                                                 double.Parse(pvmPriceRichTextBox.Text, CultureInfo.InvariantCulture);

            double roundCalculateTotalPriceWithPvm =
                    Math.Round(calculateTotalPriceWithPvm.Value, RoundDigitNumber, MidpointRounding.ToEven);

            calculateTotalPriceWithPvm = roundCalculateTotalPriceWithPvm;


            return calculateTotalPriceWithPvm;
        }

        public string CalculatePvmFromTotalPriceWithPvm(double totalPriceWithPvm)
        {
            double pvmFromTotalPriceWithPvm = totalPriceWithPvm * 21 / 121;

            double roundPvmFromTotalPriceWithPvm =
                Math.Round(pvmFromTotalPriceWithPvm, RoundDigitNumber, MidpointRounding.ToEven);

           string pvm = roundPvmFromTotalPriceWithPvm.ToString(CultureInfo.InvariantCulture);

            return pvm;
        }

        public string CalculateFullPriceFromTotalPriceWithPvm(double totalPriceWithPvm)
        {
            double fullProductPriceWithOutPvm = totalPriceWithPvm * 100 / 121;

            double roundFullProductPrice = Math.Round(fullProductPriceWithOutPvm, RoundDigitNumber, MidpointRounding.ToEven);

            string fullProductPrice = roundFullProductPrice.ToString(CultureInfo.InvariantCulture);

            return fullProductPrice;
        }

        public double CalculateProductPriceWithPvm(RichTextBox productPrice)
        {
            double productPriceWithPvm = double.Parse(productPrice.Text) * 121 / 100;

            double roundProductPrice = Math.Round(productPriceWithPvm, RoundDigitNumber, MidpointRounding.ToEven);

            return roundProductPrice;
        }
    }
}
