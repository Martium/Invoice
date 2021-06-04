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
                        S.Id, S.StorageSerialNumber, S.StorageProductName, S.StorageProductMadeDate, S.StorageProductExpireDate, S.StorageProductQuantity, S.StorageProductPrice
                      FROM Storage S
                      ORDER BY S.StorageProductMadeDate DESC
                    ";

                IEnumerable<StorageModel> getAllStorageInfo = dbConnection.Query<StorageModel>(getExistingStorage);

                return getAllStorageInfo;
            }
        }

        public IEnumerable<StorageModel> GetAllInfoByProductName(string storageProductName)
        {
            using (var dbConnection = new SQLiteConnection(AppConfiguration.ConnectionString))
            {
                dbConnection.Open();

                string getExistingStorageByNameQuery =
                    $@"SELECT * FROM Storage S
                      WHERE S.StorageProductName = '{storageProductName}'
                      ORDER BY S.StorageProductMadeDate DESC
                    ";

                IEnumerable<StorageModel> getExistingStorageByName =
                    dbConnection.Query<StorageModel>(getExistingStorageByNameQuery);

                return getExistingStorageByName;
            }
        }

        public IEnumerable<string> GetAllStorageInfoProductNames()
        {
            using (var dbConnection = new SQLiteConnection(AppConfiguration.ConnectionString))
            {
                dbConnection.Open();

                string getAllNamesQuery =
                    @"SELECT DISTINCT
                        S.StorageProductName
                      FROM Storage S
                    ";

                IEnumerable<string> getAllNames = dbConnection.Query<string>(getAllNamesQuery);

                return getAllNames;
            }
        }

        public bool CreateNewProduct(NewProductStorageModel newProductStorage)
        {
            using (var dbConnection = new SQLiteConnection(AppConfiguration.ConnectionString))
            {
                dbConnection.Open();

                string createNewProductCommand =
                    $@"INSERT INTO 'Storage'
                        Values ( NULL, @StorageSerialNumber, @StorageProductName, @StorageProductMadeDate, @StorageProductExpireDate, 
                         @StorageProductQuantity, @StorageProductPrice )
                    ";

                object queryParameters = new
                {
                    newProductStorage.StorageSerialNumber,
                    newProductStorage.StorageProductName,
                    newProductStorage.StorageProductMadeDate,
                    newProductStorage.StorageProductExpireDate,
                    newProductStorage.StorageProductQuantity,
                    newProductStorage.StorageProductPrice
                };

                int affectedRows = dbConnection.Execute(createNewProductCommand, queryParameters);

                return affectedRows == 1;
            }
        }
    }
}
