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

        public FullProductInfoModel GetFullProductInfo(string productName)
        {
            using (var dbConnection = new SQLiteConnection(AppConfiguration.ConnectionString))
            {
                dbConnection.Open();

                string getExistingProductQuery =
                    $@"
                        SELECT 
                            PI.ProductName, PI.BarCode, PI.ProductSees, PI.ProductPrice, PI.ProductType, PI.ProductTypePrice
                        FROM {ProductInfoTable} PI
                        WHERE PI.ProductName = '{productName}'
                    ";

                FullProductInfoModel getFullProductInfo = dbConnection.QuerySingleOrDefault<FullProductInfoModel>(getExistingProductQuery);

                return getFullProductInfo;
            }
        }

        public FullProductInfoWithId GetFullProductInfoWithId(string productName)
        {
            using (var dbConnection = new SQLiteConnection(AppConfiguration.ConnectionString))
            {
                dbConnection.Open();

                string getExistingProductQuery =
                    $@"
                        SELECT *
                        FROM {ProductInfoTable} PI
                        WHERE PI.ProductName = '{productName}'
                    ";

                FullProductInfoWithId getFullProductInfoWithId =
                    dbConnection.QuerySingleOrDefault<FullProductInfoWithId>(getExistingProductQuery);

                return getFullProductInfoWithId;
            }
        }

        public bool CheckIsProductNameExists(string productName)
        {
            using (var dbConnection = new SQLiteConnection(AppConfiguration.ConnectionString))
            {
                dbConnection.Open();

                string checkIsProductExists =
                    $@"
                        SELECT EXISTS(SELECT 1 FROM {ProductInfoTable} WHERE ProductName = '{productName}');
                    ";

                bool isProductExists = dbConnection.QuerySingleOrDefault<bool>(checkIsProductExists);

                return isProductExists;
            }
        }

        public bool CreateNewBuyerInfo(FullProductInfoModel newProduct)
        {
            using (var dbConnection = new SQLiteConnection(AppConfiguration.ConnectionString))
            {
                dbConnection.Open();

                string createNewProductInfoQuery =
                    $@"
                        INSERT INTO '{ProductInfoTable}'
                            VALUES (NULL, @ProductName, @BarCode, @ProductSees, @ProductPrice, @ProductType, @ProductTypePrice)
                    ";

                object queryParameters = new
                {
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
                        WHERE ProductName = @ProductName
                    ";

                object queryParameters = new
                {
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
