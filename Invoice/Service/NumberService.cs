using System;
using System.Globalization;
using System.Windows.Forms;
using Invoice.Enums;
using Invoice.Models;
using Invoice.Models.ProductType;

namespace Invoice.Service
{
    public class NumberService
    {
        private const int RoundDigitNumber = 2;

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

        public string ChangeCommaToDot(TextBox textBox)
        {
            string changeComma = textBox.Text;

            bool isComma = textBox.Text.Contains(",");

            if (isComma)
            {
                changeComma = textBox.Text.Replace(",", ".");
            }

            return changeComma;
        }

        public double? ParseToDoubleOrNull(RichTextBox richTexBox)
        {
            double? doubleOrNull = null;

            bool isNumber = double.TryParse(richTexBox.Text,NumberStyles.Any,CultureInfo.InvariantCulture, out double number);

            if (isNumber && number <= int.MaxValue)
            {
                doubleOrNull = number;
            }

            return doubleOrNull;
        }

        public double? ParseToDoubleOrZero(RichTextBox richTextBox)
        {
            double? doubleOrDefault = 0;

            bool isNumber = double.TryParse(richTextBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double number);

            if (isNumber && number <= int.MaxValue)
            {
                doubleOrDefault = number;
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

        public string ToStringProductTypeQuantityOrEmpty(ProductTypeQuantityOperations productTypeQuantityOperations, ProductTypeModel productType)
        {
            string newString;

            switch (productTypeQuantityOperations)
            {
                case ProductTypeQuantityOperations.FirstProductTypeQuantity:
                    newString = productType.FirstProductTypeQuantity.HasValue 
                        ? productType.FirstProductTypeQuantity.Value.ToString(CultureInfo.InvariantCulture) 
                        : string.Empty;
                    break;
                case ProductTypeQuantityOperations.SecondProductTypeQuantity:
                    newString = productType.SecondProductTypeQuantity.HasValue 
                        ? productType.SecondProductTypeQuantity.Value.ToString(CultureInfo.InvariantCulture) 
                        : string.Empty;
                    break;
                case ProductTypeQuantityOperations.ThirdProductTypeQuantity:
                    newString = productType.ThirdProductTypeQuantity.HasValue
                        ? productType.ThirdProductTypeQuantity.Value.ToString(CultureInfo.InvariantCulture)
                        : string.Empty;
                    break;
                case ProductTypeQuantityOperations.FourthProductTypeQuantity:
                    newString = productType.FourthProductTypeQuantity.HasValue
                        ? productType.FourthProductTypeQuantity.Value.ToString(CultureInfo.InvariantCulture)
                        : string.Empty;
                    break;
                case ProductTypeQuantityOperations.FifthProductTypeQuantity:
                    newString = productType.FifthProductTypeQuantity.HasValue
                        ? productType.FifthProductTypeQuantity.Value.ToString(CultureInfo.InvariantCulture)
                        : string.Empty;
                    break;
                case ProductTypeQuantityOperations.SixthProductTypeQuantity:
                    newString = productType.SixthProductTypeQuantity.HasValue
                        ? productType.SixthProductTypeQuantity.Value.ToString(CultureInfo.InvariantCulture)
                        : string.Empty;
                    break;
                case ProductTypeQuantityOperations.SeventhProductTypeQuantity:
                    newString = productType.SeventhProductTypeQuantity.HasValue
                        ? productType.SeventhProductTypeQuantity.Value.ToString(CultureInfo.InvariantCulture)
                        : string.Empty;
                    break;
                case ProductTypeQuantityOperations.EighthProductTypeQuantity:
                    newString = productType.EighthProductTypeQuantity.HasValue
                        ? productType.EighthProductTypeQuantity.Value.ToString(CultureInfo.InvariantCulture)
                        : string.Empty;
                    break;
                case ProductTypeQuantityOperations.NinthProductTypeQuantity:
                    newString = productType.NinthProductTypeQuantity.HasValue
                        ? productType.NinthProductTypeQuantity.Value.ToString(CultureInfo.InvariantCulture)
                        : string.Empty;
                    break;
                case ProductTypeQuantityOperations.TenProductTypeQuantity:
                    newString = productType.TenProductTypeQuantity.HasValue
                        ? productType.TenProductTypeQuantity.Value.ToString(CultureInfo.InvariantCulture)
                        : string.Empty;
                    break;
                case ProductTypeQuantityOperations.EleventhProductTypeQuantity:
                    newString = productType.EleventhProductTypeQuantity.HasValue
                        ? productType.EleventhProductTypeQuantity.Value.ToString(CultureInfo.InvariantCulture)
                        : string.Empty;
                    break;
                case ProductTypeQuantityOperations.TwelfthProductTypeQuantity:
                    newString = productType.TwelfthProductTypeQuantity.HasValue
                        ? productType.TwelfthProductTypeQuantity.Value.ToString(CultureInfo.InvariantCulture)
                        : string.Empty;
                    break;
                default:
                    newString = "code map error";
                    break;
            }

            return newString;
        }

        public string ToStringProductTypePriceOrEmpty(ProductTypePriceOperations productTypePriceOperations, ProductTypeModel productType)
        {
            string newString;

            switch (productTypePriceOperations)
            {
                case ProductTypePriceOperations.FirstProductTypePrice:
                    newString = productType.FirstProductTypePrice.HasValue
                        ? productType.FirstProductTypePrice.Value.ToString(CultureInfo.InvariantCulture)
                        : string.Empty;
                    break;
                case ProductTypePriceOperations.SecondProductTypePrice:
                    newString = productType.SecondProductTypePrice.HasValue
                        ? productType.SecondProductTypePrice.Value.ToString(CultureInfo.InvariantCulture)
                        : string.Empty;
                    break;
                case ProductTypePriceOperations.ThirdProductTypePrice:
                    newString = productType.ThirdProductTypePrice.HasValue
                        ? productType.ThirdProductTypePrice.Value.ToString(CultureInfo.InvariantCulture)
                        : string.Empty;
                    break;
                case ProductTypePriceOperations.FourthProductTypePrice:
                    newString = productType.FourthProductTypePrice.HasValue
                        ? productType.FourthProductTypePrice.Value.ToString(CultureInfo.InvariantCulture)
                        : string.Empty;
                    break;
                case ProductTypePriceOperations.FifthProductTypePrice:
                    newString = productType.FifthProductTypePrice.HasValue
                        ? productType.FifthProductTypePrice.Value.ToString(CultureInfo.InvariantCulture)
                        : string.Empty;
                    break;
                case ProductTypePriceOperations.SixthProductTypePrice:
                    newString = productType.SixthProductTypePrice.HasValue
                        ? productType.SixthProductTypePrice.Value.ToString(CultureInfo.InvariantCulture)
                        : string.Empty;
                    break;
                case ProductTypePriceOperations.SeventhProductTypePrice:
                    newString = productType.SeventhProductTypePrice.HasValue
                        ? productType.SeventhProductTypePrice.Value.ToString(CultureInfo.InvariantCulture)
                        : string.Empty;
                    break;
                case ProductTypePriceOperations.EighthProductTypePrice:
                    newString = productType.EighthProductTypePrice.HasValue
                        ? productType.EighthProductTypePrice.Value.ToString(CultureInfo.InvariantCulture)
                        : string.Empty;
                    break;
                case ProductTypePriceOperations.NinthProductTypePrice:
                    newString = productType.NinthProductTypePrice.HasValue
                        ? productType.NinthProductTypePrice.Value.ToString(CultureInfo.InvariantCulture)
                        : string.Empty;
                    break;
                case ProductTypePriceOperations.TenProductTypePrice:
                    newString = productType.TenProductTypePrice.HasValue
                        ? productType.TenProductTypePrice.Value.ToString(CultureInfo.InvariantCulture)
                        : string.Empty;
                    break;
                case ProductTypePriceOperations.EleventhProductTypePrice:
                    newString = productType.EleventhProductTypePrice.HasValue
                        ? productType.EleventhProductTypePrice.Value.ToString(CultureInfo.InvariantCulture)
                        : string.Empty;
                    break;
                case ProductTypePriceOperations.TwelfthProductTypePrice:
                    newString = productType.TwelfthProductTypePrice.HasValue
                        ? productType.TwelfthProductTypePrice.Value.ToString(CultureInfo.InvariantCulture)
                        : string.Empty;
                    break;
                default:
                    newString = "code map error";
                    break;
            }

            return newString;
        }

        public double? ParseToDoubleOrNull(TextBox textBox)
        {
            double? doubleOrNull = null;

            bool isNumber = double.TryParse(textBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double number);

            if (isNumber && number <= int.MaxValue)
            {
                doubleOrNull = number;
            }

            return doubleOrNull;
        }

        public double SumAllDataGridViewRowsSpecificColumns(DataGridView dataGridView, int rowsCount, int cellIndex)
        {
            double sum = 0;

            for (int i = 0; i <= rowsCount - 1; i++)
            {
                try
                {
                    sum = sum + double.Parse(dataGridView.Rows[i].Cells[cellIndex].Value.ToString());
                }
                catch
                {
                    sum = sum + 0;
                }
            }

            return sum;
        }

        public double MultiplyAndSumAllDataGridViewRowsTwoSpecificColumns(DataGridView dataGridView, int rowsCount, int cellIndex, int secondCellIndex)
        {
            double multiplication = 0;

            for (int i = 0; i <= rowsCount; i++)
            {
                try
                {
                    multiplication = multiplication +
                                     (double.Parse(dataGridView.Rows[i].Cells[cellIndex].Value.ToString()) *
                                      double.Parse(dataGridView.Rows[i].Cells[secondCellIndex].Value.ToString()));
                }
                catch
                {
                    multiplication = multiplication + 0;
                }
            }

            return multiplication;
        }

       
    }
}
