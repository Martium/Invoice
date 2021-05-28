using Invoice.Enums;

namespace Invoice.Service
{
    public class ProductTypeStringService
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
    }
}
