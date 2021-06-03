using System.Collections.Generic;
using System.Data.SQLite;
using Dapper;
using Invoice.Models;

namespace Invoice.Repositories
{
    public class StorageRepository
    {
        public IEnumerable<StorageModel> GetStorageInfo()
        {
            using (var dbConnection = new SQLiteConnection(AppConfiguration.ConnectionString))
            {
                dbConnection.Open();

                string getExistingStorage =
                    @"SELECT 
                         S.StorageSerialNumber, S.StorageProductName, S.StorageProductMadeDate, S.StorageProductExpireDate, S.StorageProductQuantity, S.StorageProductPrice
                      FROM Storage S
                      ORDER BY S.StorageProductMadeDate DESC
                    ";

                IEnumerable<StorageModel> getAllStorageInfo = dbConnection.Query<StorageModel>(getExistingStorage);

                return getAllStorageInfo;
            }
        }
    }
}
