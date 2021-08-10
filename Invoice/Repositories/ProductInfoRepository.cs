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

                string getExistingBuyerQuery =
                    $@"
                        SELECT 
                            PI.ProductName, PI.BarCode, PI.ProductSees, PI.ProductPrice, PI.ProductType, PI.ProductTypePrice
                        FROM {ProductInfoTable} PI
                        WHERE PI.ProductName = '{productName}'
                    ";

                FullProductInfoModel getFullProductInfo = dbConnection.QuerySingleOrDefault<FullProductInfoModel>(getExistingBuyerQuery);

                return getFullProductInfo;
            }
        }
    }
}
