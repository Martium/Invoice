using System.Collections.Generic;
using System.Data.SQLite;
using Dapper;
using Invoice.Enums;
using Invoice.Models.ProductType;
using Invoice.Service;

namespace Invoice.Repositories
{
    public class ProductTypeRepository
    {
        private readonly ProductTypeStringService _productTypeStringService;

        public ProductTypeRepository()
        {
            _productTypeStringService = new ProductTypeStringService();
        }

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

        public IEnumerable<ProductTypeSpecificNamesModel> GetAllExistingProductTypeNames()
        {
            using (var dbConnection = new SQLiteConnection(AppConfiguration.ConnectionString))
            {
                dbConnection.Open();

                string getExistingAllNamesQuery =
                    @"SELECT DISTINCT
                        PT.FirstProductType, PT.SecondProductType, PT.ThirdProductType, PT.FourthProductType,
                        PT.FifthProductType, PT.SixthProductType, PT.SeventhProductType, PT.EighthProductType,
                        PT.NinthProductType, PT.TenProductType, PT.EleventhProductType, PT.TwelfthProductType
                      FROM ProductType PT
                    ";

                IEnumerable<ProductTypeSpecificNamesModel> getExistingNames = dbConnection.Query<ProductTypeSpecificNamesModel>(getExistingAllNamesQuery);

                return getExistingNames;
            }
        }

        public IEnumerable<int> GetAllExistingProductTypeYears()
        {
            using (var dbConnection = new SQLiteConnection(AppConfiguration.ConnectionString))
            {
                dbConnection.Open();

                string getExistingYearsQuery =
                    @"SELECT DISTINCT
                        PT.IdByInvoiceNumberYearCreation
                      FROM ProductType PT
                      ORDER BY PT.IdByInvoiceNumberYearCreation DESC
                    ";

                IEnumerable<int> getExistingYears = dbConnection.Query<int>(getExistingYearsQuery);

                return getExistingYears;
            }
        }

        public dynamic GetSpecificProductTypeFullInfoBySpecialName(string productTypeName, ProductTypeOperations productTypeOperations)
        {
            string productType = _productTypeStringService.SetProductType(productTypeOperations);
            string productTypeQuantity = _productTypeStringService.SetProductTypeQuantity(productTypeOperations);
            string productTypePrice = _productTypeStringService.SetProductTypePrice(productTypeOperations);

            using (var dbConnection = new SQLiteConnection(AppConfiguration.ConnectionString))
            {
                dbConnection.Open();

                string getExistingServiceQuery =
                    $@"SELECT * FROM ProductType PT
                       WHERE PT.{productType} = '{productTypeName}' AND PT.{productType} IS NOT NULL AND PT.{productTypeQuantity} IS NOT NULL AND PT.{productTypePrice} IS NOT NULL
                       ORDER BY PT.IdByInvoiceNumber DESC, PT.IdByInvoiceNumberYearCreation DESC
                    ";

                switch (productTypeOperations)
                {
                    case ProductTypeOperations.FirstProductType:
                        IEnumerable<FirstSpecificProductTypeModel> getAllFirstSpecificProductType =
                            dbConnection.Query<FirstSpecificProductTypeModel>(getExistingServiceQuery);
                        return getAllFirstSpecificProductType;

                    case ProductTypeOperations.SecondProductType:
                        IEnumerable<SecondSpecificProductTypeModel> getAllSecondSpecificProductType =
                            dbConnection.Query<SecondSpecificProductTypeModel>(getExistingServiceQuery);
                        return getAllSecondSpecificProductType;

                    case ProductTypeOperations.ThirdProductType:
                        IEnumerable<ThirdSpecificProductTypeModel> getAllThirdSpecificProductType =
                            dbConnection.Query<ThirdSpecificProductTypeModel>(getExistingServiceQuery);
                        return getAllThirdSpecificProductType;

                    case ProductTypeOperations.FourthProductType:
                        IEnumerable<FourthSpecificProductTypeModel> getAllFourthSpecificProductType =
                            dbConnection.Query<FourthSpecificProductTypeModel>(getExistingServiceQuery);
                        return getAllFourthSpecificProductType;

                    case ProductTypeOperations.FifthProductType:
                        IEnumerable<FifthSpecificProductTypeModel> getAllFifthSpecificProductType =
                            dbConnection.Query<FifthSpecificProductTypeModel>(getExistingServiceQuery);
                        return getAllFifthSpecificProductType;

                    case ProductTypeOperations.SixthProductType:
                        IEnumerable<SixthSpecificProductTypeModel> getAllSixthSpecificProductType =
                            dbConnection.Query<SixthSpecificProductTypeModel>(getExistingServiceQuery);
                        return getAllSixthSpecificProductType;

                    case ProductTypeOperations.SeventhProductType:
                        IEnumerable<SeventhSpecificProductTypeModel> getAllSeventhSpecificProductType =
                            dbConnection.Query<SeventhSpecificProductTypeModel>(getExistingServiceQuery);
                        return getAllSeventhSpecificProductType;

                    case ProductTypeOperations.EighthProductType:
                        IEnumerable<EighthSpecificProductTypeModel> getAllEighthSpecificProductType =
                            dbConnection.Query<EighthSpecificProductTypeModel>(getExistingServiceQuery);
                        return getAllEighthSpecificProductType;

                    case ProductTypeOperations.NinthProductType:
                        IEnumerable<NinthSpecificProductTypeModel> getAllNinthSpecificProductType =
                            dbConnection.Query<NinthSpecificProductTypeModel>(getExistingServiceQuery);
                        return getAllNinthSpecificProductType;

                    case ProductTypeOperations.TenProductType:
                        IEnumerable<TenSpecificProductTypeModel> getAllTenSpecificProductType =
                            dbConnection.Query<TenSpecificProductTypeModel>(getExistingServiceQuery);
                        return getAllTenSpecificProductType;

                    case ProductTypeOperations.EleventhProductType:
                        IEnumerable<EleventhSpecificProductTypeModel> getAllEleventhSpecificProductType =
                            dbConnection.Query<EleventhSpecificProductTypeModel>(getExistingServiceQuery);
                        return getAllEleventhSpecificProductType;

                    case ProductTypeOperations.TwelfthProductType:
                        IEnumerable<TwelfthSpecificProductTypeModel> getAllTwelfthSpecificProductType =
                            dbConnection.Query<TwelfthSpecificProductTypeModel>(getExistingServiceQuery);
                        return getAllTwelfthSpecificProductType;
                    default:
                        return null;
                }
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
                        );
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
