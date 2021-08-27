using System.Data.SQLite;
using Dapper;
using Invoice.Models.MoneyReceipt;

namespace Invoice.Repositories
{
    public class MoneyReceiptRepository
    {
        private const string MoneyReceiptTable = "MoneyReceipt";
        private const string MoneyReceiptSuggestedNumber = "MoneyReceiptSuggestedNumber";
        private const int Id = 1;

        public MoneyReceiptSuggestedNumberModel GetSuggestedMoneyReceiptNumber()
        {
            using (var dbConnection = new SQLiteConnection(AppConfiguration.ConnectionString))
            {
                dbConnection.Open();

                string getExistingMoneyReceipt =
                    $@"
                        SELECT 
                            MR.{MoneyReceiptSuggestedNumber}
                        FROM {MoneyReceiptTable} MR
                        WHERE MR.Id = {Id}
                    ";

                MoneyReceiptSuggestedNumberModel getSuggestedNumber = dbConnection.QuerySingle<MoneyReceiptSuggestedNumberModel>(getExistingMoneyReceipt);

                return getSuggestedNumber;
            }
        }

        public bool UpdateMoneyReceiptSuggestedNumber(int newSuggestedNumber)
        {
            using (var dbConnection = new SQLiteConnection(AppConfiguration.ConnectionString))
            {
                dbConnection.Open();

                string updateMoneyReceiptNumberQuery =
                    $@"
                        UPDATE '{MoneyReceiptTable}'
                         SET {MoneyReceiptSuggestedNumber} = {newSuggestedNumber}
                        WHERE Id = {Id}
                    ";

                int affectedRows = dbConnection.Execute(updateMoneyReceiptNumberQuery);

                return affectedRows == 1;
            }
        }

        public void AddOneToMoneyReceiptSuggestedNumber()
        {
            using (var dbConnection = new SQLiteConnection(AppConfiguration.ConnectionString))
            {
                dbConnection.Open();

                string updateMoneyReceiptNumberQuery =
                    $@"
                        UPDATE '{MoneyReceiptTable}'
                         SET {MoneyReceiptSuggestedNumber} = {MoneyReceiptSuggestedNumber} + 1
                        WHERE Id = {Id}
                    ";

                 dbConnection.Execute(updateMoneyReceiptNumberQuery);
            }
        }

        public void UpdateNewSuggestedNumberAndAddOne(int suggestedNumber)
        {
            using (var dbConnection = new SQLiteConnection(AppConfiguration.ConnectionString))
            {
                dbConnection.Open();

                string updateMoneyReceiptNumberQuery =
                    $@"
                        UPDATE '{MoneyReceiptTable}'
                         SET {MoneyReceiptSuggestedNumber} = {suggestedNumber} + 1
                        WHERE Id = {Id}
                    ";

                dbConnection.Execute(updateMoneyReceiptNumberQuery);
            }
        }
    }
}
