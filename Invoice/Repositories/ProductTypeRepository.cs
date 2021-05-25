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

        public ProductTypeNameModel GetProductTypeNameInfoFromInvoiceAndCreationYear(int lastInvoiceNumber)
        {
            int year = DateTime.Now.Year;
            ProductTypeNameModel productTypeNameModel;

            if (lastInvoiceNumber == 0)
            {
                productTypeNameModel = null;
            }
            else
            {
                using (var dbConnection = new SQLiteConnection(AppConfiguration.ConnectionString))
                {
                    dbConnection.Open();

                    string getAllExistingNamesCommand =
                        $@"SELECT
                        PT.FirstProductType, PT.SecondProductType, PT.ThirdProductType, PT.FourthProductType, PT.FifthProductType
                      FROM ProductType PT 
                      WHERE PT.Id = {lastInvoiceNumber} AND PT.YearId = {year}
                    ";

                    try
                    {
                        productTypeNameModel = dbConnection.QuerySingleOrDefault<ProductTypeNameModel>(getAllExistingNamesCommand);
                    }
                    catch
                    {
                        productTypeNameModel = null;
                    }
                }
            }

            return productTypeNameModel;
        }
    }
}
