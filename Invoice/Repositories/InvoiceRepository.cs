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
                        I.InvoiceNumber,I.InvoiceNumberYearCreation, I.InvoiceDate, I.BuyerName
                      FROM Invoice I
                    ";

                if (!string.IsNullOrWhiteSpace(searchPhrase))
                {
                    getExistingInvoiceQuery += @" WHERE 
                                                I.InvoiceNumber LIKE @SearchPhrase OR I.InvoiceNumberYearCreation LIKE @SearchPhrase OR I.InvoiceDate LIKE @SearchPhrase OR I.BuyerName LIKE @SearchPhrase
                                                ";

                    queryParameters = new
                    {
                        SearchPhrase = $"%{searchPhrase}%"
                    };
                }

                getExistingInvoiceQuery += @" ORDER BY 
                                               I.InvoiceNumberYearCreation DESC, I.InvoiceNumber DESC";

                IEnumerable<InvoiceListModel> InvoiceList = dbConnection.Query<InvoiceListModel>(getExistingInvoiceQuery, queryParameters);

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
                        I.TwelfthProductPrice , I.PriceInWords , I.InvoiceMaker , I.InvoiceAccepted
                      FROM Invoice I
                      WHERE I.InvoiceNumber = @InvoiceNumber AND InvoiceNumberYearCreation = @InvoiceNumberYearCreation";

                object queryParameters = new
                {
                    InvoiceNumber = invoiceNumber,
                    InvoiceNumberYearCreation = invoiceNumberYearCreation
                };

                InvoiceModel invoiceService = dbConnection.QuerySingle<InvoiceModel>(getExistingServiceQuery, queryParameters);

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
                          @EighthProductPrice, @NinthProductPrice, @TenProductPrice, @EleventhProductPrice, @TwelfthProductPrice, @PriceInWords, @InvoiceMaker, @InvoiceAccepted
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
                    invoiceModel.InvoiceAccepted
                };

                int affectedRows = dbConnection.Execute(createNewInvoiceCommand, queryParameters);

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

                int? biggestOrderNumber = dbConnection.QuerySingleOrDefault<int?>(getBiggestInvoiceNumberQuery, queryParameters) ?? 0;

                return biggestOrderNumber.Value + 1;
            }
        }
    }
}
