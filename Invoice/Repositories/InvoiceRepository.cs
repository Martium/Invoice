using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Dapper;
using Invoice.Models;

namespace Invoice.Repositories
{
    public class InvoiceRepository
    {
        public IEnumerable<InvoiceListModel> GetInvoiceList(string searchPhrase = null)
        {
            using (var dbConnection = new SQLiteConnection(AppConfiguration.ConnectionString))
            {
                dbConnection.Open();

                object queryParameters = new { };

                string getExistingInvoiceQuery =
                    @"SELECT  
                        I.InvoiceNumber,I.InvoiceNumberYearCreation, I.InvoiceDate, I.BuyerName, I.PaymentStatus
                      FROM Invoice I
                    ";

                if (!string.IsNullOrWhiteSpace(searchPhrase))
                {
                    getExistingInvoiceQuery += @" WHERE 
                                                I.InvoiceNumber LIKE @SearchPhrase OR I.InvoiceNumberYearCreation LIKE @SearchPhrase OR I.InvoiceDate LIKE @SearchPhrase OR I.BuyerName LIKE @SearchPhrase OR I.PaymentStatus LIKE @SearchPhrase
                                                ";

                    queryParameters = new
                    {
                        SearchPhrase = $"%{searchPhrase}%"
                    };
                }

                getExistingInvoiceQuery += @" ORDER BY 
                                               I.InvoiceNumberYearCreation DESC, I.InvoiceNumber DESC";

                IEnumerable<InvoiceListModel> InvoiceList =
                    dbConnection.Query<InvoiceListModel>(getExistingInvoiceQuery, queryParameters);

                return InvoiceList;
            }
        }

        public InvoiceModel GetExistingInvoice(int invoiceNumber, int invoiceNumberYearCreation)
        {
            using (var dbConnection = new SQLiteConnection(AppConfiguration.ConnectionString))
            {
                dbConnection.Open();

                string getExistingServiceQuery =
                    @"SELECT  
                        I.InvoiceDate , I.SerialNumber , I.SerialNumber , I.SellerName , I.SellerFirmCode , I.SellerPvmCode , I.SellerAddress , I.SellerPhoneNumber , I.SellerBank , I.SellerBankAccountNumber , I.SellerEmailAddress , 
                        I.BuyerName , I.BuyerFirmCode , I.BuyerPvmCode , I.BuyerAddress , I.FirstProductName , I.SecondProductName , I.ThirdProductName , I.FourthProductName , I.FifthProductName , I.SixthProductName , 
                        I.SeventhProductName , I.EighthProductName , I.NinthProductName , I.TenProductName , I.EleventhProductName , I.TwelfthProductName , I.FirstProductSees , I.SecondProductSees , I.ThirdProductSees , 
                        I.ForthProductSees , I.FifthProductSees , I.SixthProductSees , I.SeventhProductSees , I.EighthProductSees , I.NinthProductSees , I.TenProductSees , I.EleventhProductSees , I.TwelfthProductSees , 
                        I.FirstProductQuantity , I.SecondProductQuantity , I.ThirdProductQuantity , I.FourthProductQuantity , I.FifthProductQuantity , I.SixthProductQuantity , I.SeventhProductQuantity , 
                        I.EighthProductQuantity , I.NinthProductQuantity , i.TenProductQuantity , I.EleventhProductQuantity , I.TwelfthProductQuantity , I.FirstProductPrice , I.SecondProductPrice , I.ThirdProductPrice , 
                        I.FourthProductPrice , I.FifthProductPrice , I.SixthProductPrice , I.SeventhProductPrice , I.EighthProductPrice , I.NinthProductPrice , I.TenProductPrice , I.EleventhProductPrice , 
                        I.TwelfthProductPrice , I.PriceInWords , I.InvoiceMaker , I.InvoiceAccepted , I.PaymentStatus
                      FROM Invoice I
                      WHERE I.InvoiceNumber = @InvoiceNumber AND InvoiceNumberYearCreation = @InvoiceNumberYearCreation";

                object queryParameters = new
                {
                    InvoiceNumber = invoiceNumber,
                    InvoiceNumberYearCreation = invoiceNumberYearCreation
                };

                InvoiceModel invoiceService =
                    dbConnection.QuerySingle<InvoiceModel>(getExistingServiceQuery, queryParameters);

                return invoiceService;
            }
        }

        public bool CreateNewInvoice(InvoiceModel invoiceModel)
        {
            int createNextInvoiceNumber = GetNextInvoiceNumber();

            using (var dbConnection = new SQLiteConnection(AppConfiguration.ConnectionString))
            {
                dbConnection.Open();

                string createNewInvoiceCommand =
                    @"INSERT INTO 'Invoice' 
	                    VALUES ( @InvoiceNumber, @InvoiceNumberYearCreation, @InvoiceDate, @SerialNumber, @SellerName, @SellerFirmCode, @SellerPvmCode, @SellerAddress, @SellerPhoneNumber, @SellerBank, @SellerBankAccountNumber, @SellerEmailAddress, @BuyerName, @BuyerFirmCode, @BuyerPvmCode, @BuyerAddress, @FirstProductName, @SecondProductName, @ThirdProductName, @FourthProductName, @FifthProductName, @SixthProductName, 
                          @SeventhProductName, @EighthProductName, @NinthProductName, @TenProductName, @EleventhProductName, @TwelfthProductName, @FirstProductSees, @SecondProductSees, @ThirdProductSees, @ForthProductSees,
                          @FifthProductSees, @SixthProductSees, @SeventhProductSees, @EighthProductSees, @NinthProductSees, @TenProductSees, @EleventhProductSees, @TwelfthProductSees, @FirstProductQuantity, @SecondProductQuantity, 
                          @ThirdProductQuantity, @FourthProductQuantity, @FifthProductQuantity, @SixthProductQuantity, @SeventhProductQuantity, @EighthProductQuantity, @NinthProductQuantity, @TenProductQuantity, 
                          @EleventhProductQuantity, @TwelfthProductQuantity, @FirstProductPrice, @SecondProductPrice, @ThirdProductPrice, @FourthProductPrice, @FifthProductPrice, @SixthProductPrice, @SeventhProductPrice,
                          @EighthProductPrice, @NinthProductPrice, @TenProductPrice, @EleventhProductPrice, @TwelfthProductPrice, @PriceInWords, @InvoiceMaker, @InvoiceAccepted, @PaymentStatus
                     )";

                object queryParameters = new
                {
                    InvoiceNumber = createNextInvoiceNumber,
                    InvoiceNumberYearCreation = DateTime.Now.Year,
                    invoiceModel.InvoiceDate,
                    invoiceModel.SerialNumber,

                    invoiceModel.SellerName,
                    invoiceModel.SellerFirmCode,
                    invoiceModel.SellerPvmCode,
                    invoiceModel.SellerAddress,
                    invoiceModel.SellerPhoneNumber,
                    invoiceModel.SellerBank,
                    invoiceModel.SellerBankAccountNumber,
                    invoiceModel.SellerEmailAddress,

                    invoiceModel.BuyerName,
                    invoiceModel.BuyerFirmCode,
                    invoiceModel.BuyerPvmCode,
                    invoiceModel.BuyerAddress,

                    invoiceModel.FirstProductName,
                    invoiceModel.SecondProductName,
                    invoiceModel.ThirdProductName,
                    invoiceModel.FourthProductName,
                    invoiceModel.FifthProductName,
                    invoiceModel.SixthProductName,
                    invoiceModel.SeventhProductName,
                    invoiceModel.EighthProductName,
                    invoiceModel.NinthProductName,
                    invoiceModel.TenProductName,
                    invoiceModel.EleventhProductName,
                    invoiceModel.TwelfthProductName,

                    invoiceModel.FirstProductSees,
                    invoiceModel.SecondProductSees,
                    invoiceModel.ThirdProductSees,
                    invoiceModel.ForthProductSees,
                    invoiceModel.FifthProductSees,
                    invoiceModel.SixthProductSees,
                    invoiceModel.SeventhProductSees,
                    invoiceModel.EighthProductSees,
                    invoiceModel.NinthProductSees,
                    invoiceModel.TenProductSees,
                    invoiceModel.EleventhProductSees,
                    invoiceModel.TwelfthProductSees,

                    invoiceModel.FirstProductQuantity,
                    invoiceModel.SecondProductQuantity,
                    invoiceModel.ThirdProductQuantity,
                    invoiceModel.FourthProductQuantity,
                    invoiceModel.FifthProductQuantity,
                    invoiceModel.SixthProductQuantity,
                    invoiceModel.SeventhProductQuantity,
                    invoiceModel.EighthProductQuantity,
                    invoiceModel.NinthProductQuantity,
                    invoiceModel.TenProductQuantity,
                    invoiceModel.EleventhProductQuantity,
                    invoiceModel.TwelfthProductQuantity,

                    invoiceModel.FirstProductPrice,
                    invoiceModel.SecondProductPrice,
                    invoiceModel.ThirdProductPrice,
                    invoiceModel.FourthProductPrice,
                    invoiceModel.FifthProductPrice,
                    invoiceModel.SixthProductPrice,
                    invoiceModel.SeventhProductPrice,
                    invoiceModel.EighthProductPrice,
                    invoiceModel.NinthProductPrice,
                    invoiceModel.TenProductPrice,
                    invoiceModel.EleventhProductPrice,
                    invoiceModel.TwelfthProductPrice,

                    invoiceModel.PriceInWords,
                    invoiceModel.InvoiceMaker,
                    invoiceModel.InvoiceAccepted,
                    invoiceModel.PaymentStatus
                };

                int affectedRows = dbConnection.Execute(createNewInvoiceCommand, queryParameters);

                return affectedRows == 1;
            }
        }

        public bool UpdateExistingInvoice(int invoiceNumber, int invoiceNumberYearCreation, InvoiceModel updatedInvoice)
        {
            using (var dbConnection = new SQLiteConnection(AppConfiguration.ConnectionString))
            {
                dbConnection.Open();

                string updateExistingInvoiceCommand =
                    @"Update 'Invoice'
                        SET InvoiceDate = @InvoiceDate, SerialNumber = @SerialNumber, SellerName = @SellerName, SellerFirmCode = @SellerFirmCode, SellerPvmCode = @SellerPvmCode, SellerAddress = @SellerAddress, 
                           SellerPhoneNumber = @SellerPhoneNumber, SellerBank = @SellerBank, SellerBankAccountNumber = @SellerBankAccountNumber, SellerEmailAddress = @SellerEmailAddress, BuyerName = @BuyerName, 
                           BuyerFirmCode = @BuyerFirmCode, BuyerPvmCode = @BuyerPvmCode, BuyerAddress = @BuyerAddress, FirstProductName = @FirstProductName, SecondProductName = @SecondProductName, 
                           ThirdProductName = @ThirdProductName, FourthProductName = @FourthProductName, FifthProductName = @FifthProductName, SixthProductName = @SixthProductName, 
                           SeventhProductName = @SeventhProductName, EighthProductName = @EighthProductName, NinthProductName = @NinthProductName, TenProductName = @TenProductName, EleventhProductName = @EleventhProductName, TwelfthProductName = @TwelfthProductName, FirstProductSees = @FirstProductSees, SecondProductSees = @SecondProductSees, ThirdProductSees = @ThirdProductSees, ForthProductSees = @ForthProductSees, 
                          FifthProductSees = @FifthProductSees, SixthProductSees = @SixthProductSees, SeventhProductSees = @SeventhProductSees, EighthProductSees = @EighthProductSees, NinthProductSees = @NinthProductSees,  TenProductSees = @TenProductSees, EleventhProductSees = @EleventhProductSees, TwelfthProductSees = @TwelfthProductSees, FirstProductQuantity = @FirstProductQuantity, 
                          SecondProductQuantity = @SecondProductQuantity, ThirdProductQuantity = @ThirdProductQuantity, FourthProductQuantity = @FourthProductQuantity, FifthProductQuantity = @FifthProductQuantity,  SixthProductQuantity = @SixthProductQuantity, SeventhProductQuantity = @SeventhProductQuantity, EighthProductQuantity = @EighthProductQuantity, NinthProductQuantity = @NinthProductQuantity, TenProductQuantity = @TenProductQuantity, EleventhProductQuantity = @EleventhProductQuantity, TwelfthProductQuantity = @TwelfthProductQuantity, FirstProductPrice = @FirstProductPrice, 
                          SecondProductPrice = @SecondProductPrice, ThirdProductPrice = @ThirdProductPrice, FourthProductPrice = @FourthProductPrice, FifthProductPrice = @FifthProductPrice, SixthProductPrice = @SixthProductPrice, SeventhProductPrice = @SeventhProductPrice, EighthProductPrice = @EighthProductPrice, NinthProductPrice = @NinthProductPrice,TenProductPrice = @TenProductPrice, EleventhProductPrice = @EleventhProductPrice,  TwelfthProductPrice = @TwelfthProductPrice, PriceInWords = @PriceInWords, InvoiceMaker = @InvoiceMaker, InvoiceAccepted = @InvoiceAccepted, PaymentStatus = @PaymentStatus
                      WHERE InvoiceNumber = @InvoiceNumber AND InvoiceNumberYearCreation = @InvoiceNumberYearCreation

                     ";

                object queryParameters = new
                {
                    updatedInvoice.InvoiceDate,
                    updatedInvoice.SerialNumber,

                    updatedInvoice.SellerName,
                    updatedInvoice.SellerFirmCode,
                    updatedInvoice.SellerPvmCode,
                    updatedInvoice.SellerAddress,
                    updatedInvoice.SellerPhoneNumber,
                    updatedInvoice.SellerBank,
                    updatedInvoice.SellerBankAccountNumber,
                    updatedInvoice.SellerEmailAddress,

                    updatedInvoice.BuyerName,
                    updatedInvoice.BuyerFirmCode,
                    updatedInvoice.BuyerPvmCode,
                    updatedInvoice.BuyerAddress,

                    updatedInvoice.FirstProductName,
                    updatedInvoice.SecondProductName,
                    updatedInvoice.ThirdProductName,
                    updatedInvoice.FourthProductName,
                    updatedInvoice.FifthProductName,
                    updatedInvoice.SixthProductName,
                    updatedInvoice.SeventhProductName,
                    updatedInvoice.EighthProductName,
                    updatedInvoice.NinthProductName,
                    updatedInvoice.TenProductName,
                    updatedInvoice.EleventhProductName,
                    updatedInvoice.TwelfthProductName,

                    updatedInvoice.FirstProductSees,
                    updatedInvoice.SecondProductSees,
                    updatedInvoice.ThirdProductSees,
                    updatedInvoice.ForthProductSees,
                    updatedInvoice.FifthProductSees,
                    updatedInvoice.SixthProductSees,
                    updatedInvoice.SeventhProductSees,
                    updatedInvoice.EighthProductSees,
                    updatedInvoice.NinthProductSees,
                    updatedInvoice.TenProductSees,
                    updatedInvoice.EleventhProductSees,
                    updatedInvoice.TwelfthProductSees,

                    updatedInvoice.FirstProductQuantity,
                    updatedInvoice.SecondProductQuantity,
                    updatedInvoice.ThirdProductQuantity,
                    updatedInvoice.FourthProductQuantity,
                    updatedInvoice.FifthProductQuantity,
                    updatedInvoice.SixthProductQuantity,
                    updatedInvoice.SeventhProductQuantity,
                    updatedInvoice.EighthProductQuantity,
                    updatedInvoice.NinthProductQuantity,
                    updatedInvoice.TenProductQuantity,
                    updatedInvoice.EleventhProductQuantity,
                    updatedInvoice.TwelfthProductQuantity,

                    updatedInvoice.FirstProductPrice,
                    updatedInvoice.SecondProductPrice,
                    updatedInvoice.ThirdProductPrice,
                    updatedInvoice.FourthProductPrice,
                    updatedInvoice.FifthProductPrice,
                    updatedInvoice.SixthProductPrice,
                    updatedInvoice.SeventhProductPrice,
                    updatedInvoice.EighthProductPrice,
                    updatedInvoice.NinthProductPrice,
                    updatedInvoice.TenProductPrice,
                    updatedInvoice.EleventhProductPrice,
                    updatedInvoice.TwelfthProductPrice,

                    updatedInvoice.PriceInWords,
                    updatedInvoice.InvoiceMaker,
                    updatedInvoice.InvoiceAccepted,
                    updatedInvoice.PaymentStatus,

                    InvoiceNumber = invoiceNumber,
                    InvoiceNumberYearCreation = invoiceNumberYearCreation
                };

                int affectedRows = dbConnection.Execute(updateExistingInvoiceCommand, queryParameters);

                return affectedRows == 1;
            }
        }

        public bool UpdateExistingInvoicePaymentStatus(int invoiceNumber, int invoiceNumberYearCreation, string updatePaymentStatus)
        {
            using (var dbConnection = new SQLiteConnection(AppConfiguration.ConnectionString))
            {
                dbConnection.Open();

                string updateExistingInvoiceCommand =
                    @"Update 'Invoice'
                        SET PaymentStatus = @PaymentStatus
                      WHERE InvoiceNumber = @InvoiceNumber AND InvoiceNumberYearCreation = @InvoiceNumberYearCreation
                     ";

                object queryParameters = new
                {
                    PaymentStatus = updatePaymentStatus,

                    InvoiceNumber = invoiceNumber,
                    InvoiceNumberYearCreation = invoiceNumberYearCreation
                };

                int affectedRows = dbConnection.Execute(updateExistingInvoiceCommand, queryParameters);

                return affectedRows == 1;
            }
        }

        public int GetNextInvoiceNumber()
        {
            using (var dbConnection = new SQLiteConnection(AppConfiguration.ConnectionString))
            {
                dbConnection.Open();

                string getBiggestInvoiceNumberQuery =
                    @"SELECT  
                        MAX(I.InvoiceNumber)
                      FROM Invoice I
                      WHERE I.InvoiceNumberYearCreation = @InvoiceNumberYearCreation
                    ";

                object queryParameters = new
                {
                    InvoiceNumberYearCreation = DateTime.Now.Year
                };

                int? biggestOrderNumber =
                    dbConnection.QuerySingleOrDefault<int?>(getBiggestInvoiceNumberQuery, queryParameters) ?? 0;

                return biggestOrderNumber.Value + 1;
            }
        }
    }
}
