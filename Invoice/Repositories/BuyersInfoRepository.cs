using System.Collections.Generic;
using System.Data.SQLite;
using Dapper;
using Invoice.Models.BuyersInfo;

namespace Invoice.Repositories
{
    public class BuyersInfoRepository
    {
        private const string BuyersInfoTable = "BuyersInfo";
        public IEnumerable<BuyersNamesModel> GetExistsBuyersNames()
        {
            using(var dbConnection = new SQLiteConnection(AppConfiguration.ConnectionString))
            {
                dbConnection.Open();

                string getExistingBuyersNamesQuery =
                    $@"
                        SELECT DISTINCT
                          BI.BuyerName 
                        FROM {BuyersInfoTable} BI
                        ORDER BY BI.BuyerName ASC
                    ";

                IEnumerable<BuyersNamesModel> getExistingBuyersNames =
                    dbConnection.Query<BuyersNamesModel>(getExistingBuyersNamesQuery);

                return getExistingBuyersNames;
            }
        }

        public BuyerFullInfoModel BuyerFullInfo(string buyerName)
        {
            using (var dbConnection = new SQLiteConnection(AppConfiguration.ConnectionString))
            {
                dbConnection.Open();

                string getExistingBuyerQuery =
                    $@"
                        SELECT 
                            BI.BuyerName, BI.BuyerFirmCode, BI.BuyerPvmCode, BI.BuyerAddress
                        FROM {BuyersInfoTable} BI
                        WHERE BI.BuyerName = '{buyerName}'
                    ";

                
                BuyerFullInfoModel getBuyerFullInfo = dbConnection.QuerySingleOrDefault<BuyerFullInfoModel>(getExistingBuyerQuery);

                return getBuyerFullInfo;
            }
        }

        public bool CheckIsBuyerExists(string buyerName)
        {
            using (var dbConnection = new SQLiteConnection(AppConfiguration.ConnectionString))
            {
                dbConnection.Open();

                string checkBuyerExists =
                    $@"
                        SELECT EXISTS(SELECT 1 FROM {BuyersInfoTable} WHERE BuyerName = '{buyerName}');
                    ";

                bool isBuyerExists = dbConnection.QuerySingleOrDefault<bool>(checkBuyerExists);

                return isBuyerExists;
            }
        }

        public bool CreateNewBuyerInfo(BuyerFullInfoModel newBuyer)
        {
            using (var dbConnection = new SQLiteConnection(AppConfiguration.ConnectionString))
            {
                dbConnection.Open();

                string createNewBuyerInfoQuery =
                    $@"
                        INSERT INTO '{BuyersInfoTable}'
                            VALUES (NULL, @BuyerName, @BuyerFirmCode, @BuyerPvmCode, @BuyerAddress )
                    ";

                object queryParameters = new
                {
                    newBuyer.BuyerName,
                    newBuyer.BuyerFirmCode,
                    newBuyer.BuyerPvmCode,
                    newBuyer.BuyerAddress
                };

                int affectedRows = dbConnection.Execute(createNewBuyerInfoQuery, queryParameters);

                return affectedRows == 1;
            }
        }

        public bool UpdateBuyerInfo(BuyerFullInfoModel updateBuyer)
        {
            using (var dbConnection = new SQLiteConnection(AppConfiguration.ConnectionString))
            {
                dbConnection.Open();

                string updateBuyerInfoQuery =
                    $@"
                        UPDATE '{BuyersInfoTable}'
                          SET BuyerFirmCode = @BuyerFirmCode, BuyerPvmCode = @BuyerPvmCode, BuyerAddress = @BuyerAddress
                        WHERE BuyerName = @BuyerName
                    ";

                object queryParameters = new
                {
                    updateBuyer.BuyerName,
                    updateBuyer.BuyerFirmCode,
                    updateBuyer.BuyerPvmCode,
                    updateBuyer.BuyerAddress
                };

                int affectedRows = dbConnection.Execute(updateBuyerInfoQuery, queryParameters);

                return affectedRows == 1;
            }
        }
    }
}
