using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using Dapper;
using Invoice.Models;

namespace Invoice.Repositories
{
    public class ProductTypeRepository
    {
        public ProductTypeModel GetExistingProductType(int invoiceNumber, int invoiceNumberYearCreation)
        {
            using (var dbConnection = new SQLiteConnection(AppConfiguration.ConnectionString))
            {
                dbConnection.Open();

                string getExistingServiceQuery =
                    $@"SELECT
                        PT.FirstProductType, PT.SecondProductType, PT.ThirdProductType, PT.FourthProductType, PT.FifthProductType, PT.SixthProductType, PT.SeventhProductType, PT.EighthProductType, PT.NinthProductType, PT.TenProductType, PT.EleventhProductType, PT.TwelfthProductType, PT.FirstProductTypeQuantity, PT.SecondProductTypeQuantity, PT.ThirdProductTypeQuantity, PT.FourthProductTypeQuantity, PT.FifthProductTypeQuantity, PT.SixthProductTypeQuantity, PT.SeventhProductTypeQuantity, PT.EighthProductTypeQuantity, PT.NinthProductTypeQuantity, PT.TenProductTypeQuantity, PT.EleventhProductTypeQuantity, PT.TwelfthProductTypeQuantity, PT.FirstProductTypePrice, PT.SecondProductTypePrice, PT.ThirdProductTypePrice, PT.FourthProductTypePrice, PT.FifthProductTypePrice, PT.SixthProductTypePrice, PT.SeventhProductTypePrice, PT.EighthProductTypePrice, PT.NinthProductTypePrice, PT.TenProductTypePrice, PT.EleventhProductTypePrice, PT.TwelfthProductTypePrice
                      FROM ProductType PT
                      WHERE IdByInvoiceNumber = {invoiceNumber} AND IdByInvoiceNumberYearCreation = {invoiceNumberYearCreation}
                    ";

                ProductTypeModel getExistingProductType = dbConnection.QuerySingleOrDefault<ProductTypeModel>(getExistingServiceQuery);

                return getExistingProductType;
            }
        }

        public List<FirstSpecificProductTypeModel> GetSpecificProductTypeBySpecialName(string productType, string productTypeName)
        {
            using (var dbConnection = new SQLiteConnection(AppConfiguration.ConnectionString))
            {
                dbConnection.Open();

                string getExistingServiceQuery =
                    $@"SELECT * FROM ProductType PT
                       WHERE PT.{productType} = @{productTypeName};
                    ";
                

                IEnumerable<FirstSpecificProductTypeModel> getAllSpecificProductType =
                    dbConnection.Query<FirstSpecificProductTypeModel>(getExistingServiceQuery);

                return getAllSpecificProductType.ToList();

            }
        }

        public bool CreateNewProductType(int invoiceNumber, int invoiceNumberYearCreation, ProductTypeModel productType)
        {
            using (var dbConnection = new SQLiteConnection(AppConfiguration.ConnectionString))
            {
                dbConnection.Open();

                string createNewProductTypeCommand =
                    @"INSERT OR REPLACE INTO 'ProductType'
                        VALUES ( @IdByInvoiceNumber, @IdByInvoiceNumberYearCreation, @FirstProductType, @SecondProductType, @ThirdProductType, @FourthProductType, @FifthProductType, @SixthProductType, @SeventhProductType, @EighthProductType, @NinthProductType, @TenProductType, @EleventhProductType, @TwelfthProductType, @FirstProductTypeQuantity, @SecondProductTypeQuantity, @ThirdProductTypeQuantity, @FourthProductTypeQuantity, @FifthProductTypeQuantity, @SixthProductTypeQuantity, @SeventhProductTypeQuantity, @EighthProductTypeQuantity, @NinthProductTypeQuantity, @TenProductTypeQuantity, @EleventhProductTypeQuantity, @TwelfthProductTypeQuantity, @FirstProductTypePrice, @SecondProductTypePrice, @ThirdProductTypePrice, @FourthProductTypePrice, @FifthProductTypePrice, @SixthProductTypePrice, @SeventhProductTypePrice, @EighthProductTypePrice, @NinthProductTypePrice, @TenProductTypePrice, @EleventhProductTypePrice, @TwelfthProductTypePrice
                        )
                    ";

                object queryParameters = new
                {
                    IdByInvoiceNumber = invoiceNumber,
                    IdByInvoiceNumberYearCreation = invoiceNumberYearCreation,

                    productType.FirstProductType,
                    productType.SecondProductType,
                    productType.ThirdProductType,
                    productType.FourthProductType,
                    productType.FifthProductType,
                    productType.SixthProductType,
                    productType.SeventhProductType,
                    productType.EighthProductType,
                    productType.NinthProductType,
                    productType.TenProductType,
                    productType.EleventhProductType,
                    productType.TwelfthProductType,

                    productType.FirstProductTypeQuantity,
                    productType.SecondProductTypeQuantity,
                    productType.ThirdProductTypeQuantity,
                    productType.FourthProductTypeQuantity,
                    productType.FifthProductTypeQuantity,
                    productType.SixthProductTypeQuantity,
                    productType.SeventhProductTypeQuantity,
                    productType.EighthProductTypeQuantity,
                    productType.NinthProductTypeQuantity,
                    productType.TenProductTypeQuantity,
                    productType.EleventhProductTypeQuantity,
                    productType.TwelfthProductTypeQuantity,

                    productType.FirstProductTypePrice,
                    productType.SecondProductTypePrice,
                    productType.ThirdProductTypePrice,
                    productType.FourthProductTypePrice,
                    productType.FifthProductTypePrice,
                    productType.SixthProductTypePrice,
                    productType.SeventhProductTypePrice,
                    productType.EighthProductTypePrice,
                    productType.NinthProductTypePrice,
                    productType.TenProductTypePrice,
                    productType.EleventhProductTypePrice,
                    productType.TwelfthProductTypePrice
                };

                int affectedRows = dbConnection.Execute(createNewProductTypeCommand, queryParameters);

                return affectedRows == 1;
            }
        }
    }
}
