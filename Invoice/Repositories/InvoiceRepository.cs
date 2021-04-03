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
                        I.InvoiceDate , I.SerialNumber , I.SerialNumber , I.SellerFirmCode , I.SellerPvmCode , I.SellerAddress , I.SellerPhoneNumber , I.SellerBank , I.SellerBankAccountNumber , I.SellerEmailAddress , 
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
