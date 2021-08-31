using System.Collections.Generic;
using System.Data.SQLite;
using Dapper;
using Invoice.Models.ProductInfo;

namespace Invoice.Repositories
{
    public class ProductInfoRepository
    {
        private const string ProductInfoTable = "ProductInfo";

        public IEnumerable<ProductInfoNameModel> GetAllProductsNames()
        {
            using (var dbConnection = new SQLiteConnection(AppConfiguration.ConnectionString))
            {
                dbConnection.Open();

                string getProductsNamesQuery =
                    $@"
                        SELECT DISTINCT
                          PI.ProductName 
                        FROM {ProductInfoTable} PI
                        ORDER BY PI.ProductName ASC
                    ";

                IEnumerable<ProductInfoNameModel> getExistingProductNames =
                    dbConnection.Query<ProductInfoNameModel>(getProductsNamesQuery);

                return getExistingProductNames;
            }
        }

        public int GetBiggestProductId()
        {
            using (var dbConnection = new SQLiteConnection(AppConfiguration.ConnectionString))
            {
                dbConnection.Open();

                int lastId;
                string getLastIdQuery =
                    $@"SELECT  
                        MAX(PI.Id)
                      FROM {ProductInfoTable} PI
                    ";

                try
                {
                     lastId = dbConnection.QuerySingle<int>(getLastIdQuery);
                }
                catch
                {
                     lastId = 1;
                }

                return lastId;
            }
        }

        public string GetProductName(int id)
        {
            using (var dbConnection = new SQLiteConnection(AppConfiguration.ConnectionString))
            {
                dbConnection.Open();

                string getProductNameQuery =
                    $@"
                        SELECT
                          PI.ProductName
                        FROM {ProductInfoTable} PI
                        WHERE PI.Id = {id}
                    ";
                //select distinct and maybe year attribute must be pass
                string getProductName = dbConnection.QuerySingleOrDefault<string>(getProductNameQuery);
                return getProductName;
            }
        }

        public FullProductInfoModel GetFullProductInfo(string productName, int year)
        {
            using (var dbConnection = new SQLiteConnection(AppConfiguration.ConnectionString))
            {
                dbConnection.Open();

                string getExistingProductQuery =
                    $@"
                        SELECT 
                            PI.Year, PI.ProductName, PI.BarCode, PI.ProductSees, PI.ProductPrice, PI.ProductType, PI.ProductTypePrice
                        FROM {ProductInfoTable} PI
                        WHERE PI.ProductName = '{productName}' AND PI.Year = {year}
                    ";
                FullProductInfoModel getFullProductInfo = dbConnection.QuerySingleOrDefault<FullProductInfoModel>(getExistingProductQuery);

                return getFullProductInfo;
            }
        }

        public FullProductInfoWithId GetFullProductInfoWithId(string productName, int year)
        {
            using (var dbConnection = new SQLiteConnection(AppConfiguration.ConnectionString))
            {
                dbConnection.Open();

                string getExistingProductQuery =
                    $@"
                        SELECT *
                        FROM {ProductInfoTable} PI
                        WHERE PI.ProductName = '{productName}' AND Year = {year}
                    ";

                FullProductInfoWithId getFullProductInfoWithId =
                    dbConnection.QuerySingleOrDefault<FullProductInfoWithId>(getExistingProductQuery);

                return getFullProductInfoWithId;
            }
        }

        public bool CheckIsProductNameExists(string productName, int year)
        {
            using (var dbConnection = new SQLiteConnection(AppConfiguration.ConnectionString))
            {
                dbConnection.Open();

                string checkIsProductExists =
                    $@"
                        SELECT EXISTS(SELECT 1 FROM {ProductInfoTable} WHERE ProductName = '{productName}' AND Year = {year});
                    ";

                bool isProductExists = dbConnection.QuerySingleOrDefault<bool>(checkIsProductExists);

                return isProductExists;
            }
        }

        public bool CreateNewProductInfo(FullProductInfoModel newProduct)
        {
            using (var dbConnection = new SQLiteConnection(AppConfiguration.ConnectionString))
            {
                dbConnection.Open();

                string createNewProductInfoQuery =
                    $@"
                        INSERT INTO '{ProductInfoTable}'
                            VALUES (NULL, @Year, @ProductName, @BarCode, @ProductSees, @ProductPrice, @ProductType, @ProductTypePrice)
                    ";

                object queryParameters = new
                {
                    newProduct.Year,
                    newProduct.ProductName,
                    newProduct.BarCode,
                    newProduct.ProductSees,
                    newProduct.ProductPrice,

                    newProduct.ProductType,
                    newProduct.ProductTypePrice
                };

                int affectedRows = dbConnection.Execute(createNewProductInfoQuery, queryParameters);

                return affectedRows == 1;
            }
        }

        public bool UpdateProduct(FullProductInfoModel updateProduct)
        {
            using (var dbConnection = new SQLiteConnection(AppConfiguration.ConnectionString))
            {
                dbConnection.Open();

                string updateProductInfoQuery =
                    $@"
                        UPDATE '{ProductInfoTable}'
                          SET BarCode = @BarCode, ProductSees = @ProductSees, ProductPrice = @ProductPrice, ProductType = @ProductType, ProductTypePrice = @ProductTypePrice
                        WHERE ProductName = @ProductName AND Year = @Year
                    ";

                object queryParameters = new
                {
                    updateProduct.Year,
                    updateProduct.ProductName,
                    updateProduct.BarCode,
                    updateProduct.ProductSees,
                    updateProduct.ProductPrice,

                    updateProduct.ProductType,
                    updateProduct.ProductTypePrice
                };

                int affectedRows = dbConnection.Execute(updateProductInfoQuery, queryParameters);

                return affectedRows == 1;
            }
        }
    }
}
