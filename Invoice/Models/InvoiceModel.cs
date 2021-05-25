using System;

namespace Invoice.Models
{
    public class InvoiceModel
    {
        public DateTime InvoiceDate { get; set; }

        public string SerialNumber { get; set; }
        public string SellerName { get; set; }
        public string SellerFirmCode { get; set; }
        public string SellerPvmCode { get; set; }
        public string SellerAddress { get; set; }
        public string SellerPhoneNumber { get; set; }
        public string SellerBank { get; set; }
        public string SellerBankAccountNumber { get; set; }
        public string SellerEmailAddress { get; set; }

        public string BuyerName { get; set; }
        public string BuyerFirmCode { get; set; }
        public string BuyerPvmCode { get; set; }
        public string BuyerAddress { get; set; }

        public string FirstProductName { get; set; }
        public string SecondProductName { get; set; }
        public string ThirdProductName { get; set; }
        public string FourthProductName { get; set; }
        public string FifthProductName { get; set; }
        public string SixthProductName { get; set; }
        public string SeventhProductName { get; set; }
        public string EighthProductName { get; set; }
        public string NinthProductName { get; set; }
        public string TenProductName { get; set; }
        public string EleventhProductName { get; set; }
        public string TwelfthProductName { get; set; }

        public string FirstProductSees { get; set; }
        public string SecondProductSees { get; set; }
        public string ThirdProductSees { get; set; }
        public string ForthProductSees { get; set; }
        public string FifthProductSees { get; set; }
        public string SixthProductSees { get; set; }
        public string SeventhProductSees { get; set; }
        public string EighthProductSees { get; set; }
        public string NinthProductSees { get; set; }
        public string TenProductSees { get; set; }
        public string EleventhProductSees { get; set; }
        public string TwelfthProductSees { get; set; }

        public double? FirstProductQuantity { get; set; }
        public double? SecondProductQuantity { get; set; }
        public double? ThirdProductQuantity { get; set; }
        public double? FourthProductQuantity { get; set; }
        public double? FifthProductQuantity { get; set; }
        public double? SixthProductQuantity { get; set; }
        public double? SeventhProductQuantity { get; set; }
        public double? EighthProductQuantity { get; set; }
        public double? NinthProductQuantity { get; set; }
        public double? TenProductQuantity { get; set; }
        public double? EleventhProductQuantity { get; set; }
        public double? TwelfthProductQuantity { get; set; }

        public double? FirstProductPrice { get; set; }
        public double? SecondProductPrice { get; set; }
        public double? ThirdProductPrice { get; set; }
        public double? FourthProductPrice { get; set; }
        public double? FifthProductPrice { get; set; }
        public double? SixthProductPrice { get; set; }
        public double? SeventhProductPrice { get; set; }
        public double? EighthProductPrice { get; set; }
        public double? NinthProductPrice { get; set; }
        public double? TenProductPrice { get; set; }
        public double? EleventhProductPrice { get; set; }
        public double? TwelfthProductPrice { get; set; }

        public string PriceInWords { get; set; }
        public string InvoiceMaker { get; set; }
        public string InvoiceAccepted { get; set; }
        public string PaymentStatus { get; set; }

        public string TotalPriceWithPvm { get; set; }

       /* public string FirstProductType { get; set; }
        public string SecondProductType { get; set; }
        public string ThirdProductType { get; set; }
        public string FourthProductType { get; set; }
        public string FifthProductType { get; set; }

        public double? FirstProductTypeQuantity { get; set; }
        public double? SecondProductTypeQuantity { get; set; }
        public double? ThirdProductTypeQuantity { get; set; }
        public double? FourthProductTypeQuantity { get; set; }
        public double? FifthProductTypeQuantity { get; set; }

        public double? FirstProductTypePrice { get; set; }
        public double? SecondProductTypePrice { get; set; }
        public double? ThirdProductTypePrice { get; set; }
        public double? FourthProductTypePrice { get; set; }
        public double? FifthProductTypePrice { get; set; } */

    }
}
