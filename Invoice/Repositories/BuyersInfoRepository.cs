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
    }
}
