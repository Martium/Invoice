using System;
using Invoice.Enums;

namespace Invoice.Service
{
    public class StringService
    {
        public string SetProductType(ProductTypeOperations productTypeOperations)
        {
            string productType;

            switch (productTypeOperations)
            {
                case ProductTypeOperations.FirstProductType:
                    productType = "FirstProductType";
                    break;
                case ProductTypeOperations.SecondProductType:
                    productType = "SecondProductType";
                    break;
                case ProductTypeOperations.ThirdProductType:
                    productType = "ThirdProductType";
                    break;
                case ProductTypeOperations.FourthProductType:
                    productType = "FourthProductType";
                    break;
                case ProductTypeOperations.FifthProductType:
                    productType = "FifthProductType";
                    break;
                case ProductTypeOperations.SixthProductType:
                    productType = "SixthProductType";
                    break;
                case ProductTypeOperations.SeventhProductType:
                    productType = "SeventhProductType";
                    break;
                case ProductTypeOperations.EighthProductType:
                    productType = "EighthProductType";
                    break;
                case ProductTypeOperations.NinthProductType:
                    productType = "NinthProductType";
                    break;
                case ProductTypeOperations.TenProductType:
                    productType = "TenProductType";
                    break;
                case ProductTypeOperations.EleventhProductType:
                    productType = "EleventhProductType";
                    break;
                case ProductTypeOperations.TwelfthProductType:
                    productType = "TwelfthProductType";
                    break;

                default:
                    productType = "FirstProductType";
                    break;
            }

            return productType;
        }

        public string SetProductTypeQuantity(ProductTypeOperations productTypeOperations)
        {
            string productTypeQuantity;

            switch (productTypeOperations)
            {
                case ProductTypeOperations.FirstProductType:
                    productTypeQuantity = "FirstProductTypeQuantity";
                    break;
                case ProductTypeOperations.SecondProductType:
                    productTypeQuantity = "SecondProductTypeQuantity";
                    break;
                case ProductTypeOperations.ThirdProductType:
                    productTypeQuantity = "ThirdProductTypeQuantity";
                    break;
                case ProductTypeOperations.FourthProductType:
                    productTypeQuantity = "FourthProductTypeQuantity";
                    break;
                case ProductTypeOperations.FifthProductType:
                    productTypeQuantity = "FifthProductTypeQuantity";
                    break;
                case ProductTypeOperations.SixthProductType:
                    productTypeQuantity = "SixthProductTypeQuantity";
                    break;
                case ProductTypeOperations.SeventhProductType:
                    productTypeQuantity = "SeventhProductTypeQuantity";
                    break;
                case ProductTypeOperations.EighthProductType:
                    productTypeQuantity = "EighthProductTypeQuantity";
                    break;
                case ProductTypeOperations.NinthProductType:
                    productTypeQuantity = "NinthProductTypeQuantity";
                    break;
                case ProductTypeOperations.TenProductType:
                    productTypeQuantity = "TenProductTypeQuantity";
                    break;
                case ProductTypeOperations.EleventhProductType:
                    productTypeQuantity = "EleventhProductTypeQuantity";
                    break;
                case ProductTypeOperations.TwelfthProductType:
                    productTypeQuantity = "TwelfthProductTypeQuantity";
                    break;

                default:
                    productTypeQuantity = "FirstProductTypeQuantity";
                    break;
            }

            return productTypeQuantity;
        }

        public string SetProductTypePrice(ProductTypeOperations productTypeOperations)
        {
            string productTypePrice;

            switch (productTypeOperations)
            {
                case ProductTypeOperations.FirstProductType:
                    productTypePrice = "FirstProductTypePrice";
                    break;
                case ProductTypeOperations.SecondProductType:
                    productTypePrice = "SecondProductTypePrice";
                    break;
                case ProductTypeOperations.ThirdProductType:
                    productTypePrice = "ThirdProductTypePrice";
                    break;
                case ProductTypeOperations.FourthProductType:
                    productTypePrice = "FourthProductTypePrice";
                    break;
                case ProductTypeOperations.FifthProductType:
                    productTypePrice = "FifthProductTypePrice";
                    break;
                case ProductTypeOperations.SixthProductType:
                    productTypePrice = "SixthProductTypePrice";
                    break;
                case ProductTypeOperations.SeventhProductType:
                    productTypePrice = "SeventhProductTypePrice";
                    break;
                case ProductTypeOperations.EighthProductType:
                    productTypePrice = "EighthProductTypePrice";
                    break;
                case ProductTypeOperations.NinthProductType:
                    productTypePrice = "NinthProductTypePrice";
                    break;
                case ProductTypeOperations.TenProductType:
                    productTypePrice = "TenProductTypePrice";
                    break;
                case ProductTypeOperations.EleventhProductType:
                    productTypePrice = "EleventhProductTypePrice";
                    break;
                case ProductTypeOperations.TwelfthProductType:
                    productTypePrice = "TwelfthProductTypePrice";
                    break;

                default:
                    productTypePrice = "FirstProductTypePrice";
                    break;
            }

            return productTypePrice;
        }

        public string MakeFormatFilledProducts(string[] allProducts, int emptyLines)
        {
            string filledProducts = null;

            foreach (var product in allProducts)
            {
                filledProducts += $"{product}{Environment.NewLine}{Environment.NewLine}";
            }

            return filledProducts;
        }

    }
}
