using System;
using System.Data.SQLite;
using Dapper;
using Invoice.Models;

namespace Invoice.Repositories
{
    public class ProductTypeRepository
    {
        public ProductTypeModel GetProductTypeInfoFromInvoiceNumberAndCreationYear(int invoiceNumber,
            int invoiceNumberYearCreation)
        {
            using (var dbConnection = new SQLiteConnection(AppConfiguration.ConnectionString))
            {
                dbConnection.Open();

                string getExistingProductTypeCommand =
                    @"SELECT * FROM ProductType PT
                        WHERE PT.Id = @InvoiceNumber AND PT.YearId = @InvoiceNumberYearCreation
                    ";

                object queryParameters = new
                {
                    InvoiceNumber = invoiceNumber,
                    InvoiceNumberYearCreation = invoiceNumberYearCreation
                };

                ProductTypeModel productType;

                try
                {
                    productType =
                        dbConnection.QuerySingleOrDefault<ProductTypeModel>(getExistingProductTypeCommand,
                            queryParameters);
                }
                catch
                {
                    productType = null;
                }

                return productType;
            }
        }

        public ProductTypeNameModel GetProductTypeNameInfo()
        {

            using (var dbConnection = new SQLiteConnection(AppConfiguration.ConnectionString))
            {
                dbConnection.Open();

                string getAllExistingNamesCommand =
                    @"SELECT
                        PT.FirstProductType, PT.SecondProductType, PT.ThirdProductType, PT.FourthProductType, PT.FifthProductType
                      FROM ProductType PT 
                        ";

                ProductTypeNameModel productTypeNameModel;

                try
                {
                    productTypeNameModel =
                        dbConnection.QuerySingleOrDefault<ProductTypeNameModel>(getAllExistingNamesCommand);
                }
                catch
                {
                    productTypeNameModel = null;
                }

                return productTypeNameModel;
            }

        }

        public bool CreateNewProductType(int invoiceNumber, int invoiceNumberYearCreation,
            ProductTypeNameModel productTypeNameModel)
        {
            using (var dbConnection = new SQLiteConnection(AppConfiguration.ConnectionString))
            {
                dbConnection.Open();

                string createNewProductTypeCommand =
                    $@"INSERT INTO 'ProductType'
                        VALUES (@Id, @YearId, @FirstProductType, @SecondProductType, @ThirdProductType, @FourthProductType, @FifthProductType)
                    ";

                object queryParameters = new
                {
                    Id = invoiceNumber,
                    YearId = invoiceNumberYearCreation,

                    productTypeNameModel.FirstProductType,
                    productTypeNameModel.SecondProductType,
                    productTypeNameModel.ThirdProductType,
                    productTypeNameModel.FourthProductType,
                    productTypeNameModel.FifthProductType
                };

                int affectedRows = dbConnection.Execute(createNewProductTypeCommand, queryParameters);

                return affectedRows == 1;
            }
        }
    }
}
