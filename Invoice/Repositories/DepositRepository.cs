using System.Collections.Generic;
using System.Data.SQLite;
using Dapper;
using Invoice.Models.Deposit;

namespace Invoice.Repositories
{
    public class DepositRepository
    {
        private const string DepositTable = "Deposit";
        private const string Id = "Id";
        private const string InvoiceYear = "InvoiceYear";
        private const string ProductName = "ProductName";
        private const string BarCode = "BarCode";
        private const string ProductQuantity = "ProductQuantity";

        public IEnumerable<string> GetAllProductsNames()
        {
            using (var dbConnection = new SQLiteConnection(AppConfiguration.ConnectionString))
            {
                dbConnection.Open();

                string getProductsNamesQuery =
                    $@"
                        SELECT DISTINCT
                          D.ProductName 
                        FROM {DepositTable} D
                        ORDER BY D.{ProductName} ASC
                    ";

                IEnumerable<string> getExistingProductNames =
                    dbConnection.Query<string>(getProductsNamesQuery);

                return getExistingProductNames;
            }
        }

        public IEnumerable<FullDepositProductWithoutIdModel> GetAllDepositProductsByYear(int year)
        {
            using (var dbConnection = new SQLiteConnection(AppConfiguration.ConnectionString))
            {
                dbConnection.Open();

                string getDepositAllInfoQuery =
                    $@"
                        SELECT
                          D.{InvoiceYear}, D.{ProductName}, D.{BarCode}, D.{ProductQuantity}
                        FROM {DepositTable} D
                        WHERE D.{InvoiceYear} = {year}
                        ORDER BY D.{ProductName} ASC
                    ";

                IEnumerable<FullDepositProductWithoutIdModel> getDepositAllInfoCommand =
                    dbConnection.Query<FullDepositProductWithoutIdModel>(getDepositAllInfoQuery);

                return getDepositAllInfoCommand;
            }
        }

        public IEnumerable<FullDepositProductWithoutIdModel> GetAllDepositInfoByYearAndName(int year, string productName)
        {
            using (var dbConnection = new SQLiteConnection(AppConfiguration.ConnectionString))
            {
                dbConnection.Open();

                string getDepositAllInfoQuery =
                    $@"
                        SELECT
                          D.{InvoiceYear}, D.{ProductName}, D.{BarCode}, D.{ProductQuantity}
                        FROM {DepositTable} D
                        WHERE D.{InvoiceYear} = {year} AND D.{ProductName} = '{productName}'
                        ORDER BY D.{ProductName} ASC
                    ";

                IEnumerable<FullDepositProductWithoutIdModel> getDepositAllInfoCommand =
                    dbConnection.Query<FullDepositProductWithoutIdModel>(getDepositAllInfoQuery);

                return getDepositAllInfoCommand;
            }
        }

        public IEnumerable<FullDepositProductWithoutIdModel> GetAllInfo()
        {
            using (var dbConnection = new SQLiteConnection(AppConfiguration.ConnectionString))
            {
                dbConnection.Open();

                string getAllInfoQuery =
                    $@"
                        SELECT
                           D.{InvoiceYear}, D.{ProductName}, D.{BarCode}, D.{ProductQuantity}
                        FROM {DepositTable} D
                        ORDER BY D.{ProductName} ASC
                    ";

                IEnumerable<FullDepositProductWithoutIdModel> getAllInfoCommand =
                    dbConnection.Query<FullDepositProductWithoutIdModel>(getAllInfoQuery);

                return getAllInfoCommand;
            }
        }

        public void AddQuantityByIdAndYear(DepositAddQuantityModel updateQuantity)
        {
            using (var dbConnection = new SQLiteConnection(AppConfiguration.ConnectionString))
            {
                dbConnection.Open();

                string updateQuantityQuery =
                    $@"UPDATE '{DepositTable}'
                        SET
                          {ProductQuantity} = {ProductQuantity} + {updateQuantity.ProductQuantity}
                        WHERE {Id} = {updateQuantity.Id} AND {InvoiceYear} = {updateQuantity.InvoiceYear}
                    ";

                dbConnection.Execute(updateQuantityQuery);
            }
        }

        

        





    }
}
