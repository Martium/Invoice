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

        private const string DepositIdLineTable = "DepositIdLine";
        private const string InvoiceId = "InvoiceId";
        private const string FirstLineId = "FirstLineId";
        private const string SecondLineId = "SecondLineId";
        private const string ThirdLineId = "ThirdLineId";
        private const string FourthLineId = "FourthLineId";
        private const string FifthLineId = "FifthLineId";
        private const string SixthLineId = "SixthLineId";
        private const string SeventhLineId = "SeventhLineId";
        private const string EighthLineId = "EighthLineId";
        private const string NinthLineId = "NinthLineId";
        private const string TenLineId = "TenLineId";
        private const string EleventhLineId = "EleventhLineId";
        private const string TwelfthLineId = "TwelfthLineId";


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

        public void SaveDepositIdLinesInfo(DepositIdSaveModel saveId)
        {
            using (var dbConnection = new SQLiteConnection(AppConfiguration.ConnectionString))
            {
                dbConnection.Open();

                string saveIdQuery =
                    $@"
                        INSERT OR REPLACE INTO '{DepositIdLineTable}'
                            VALUES ({saveId.InvoiceId}, {saveId.FirstLineId}, {saveId.SecondLineId}, {saveId.ThirdLineId}, {saveId.FourthLineId}, {saveId.FifthLineId},
                                    {saveId.SixthLineId}, {saveId.SeventhLineId}, {saveId.EighthLineId}, {saveId.NinthLineId}, {saveId.TenLineId}, {saveId.EleventhLineId}, {saveId.TwelfthLineId}
                        )
                    ";

                dbConnection.Execute(saveIdQuery);
            }
        }

        public DepositIdLoadModel LoadDepositIdLinesInfo(int invoiceId)
        {
            using (var dbConnection = new SQLiteConnection(AppConfiguration.ConnectionString))
            {
                dbConnection.Open();

                string loadIdQuery =
                    $@"
                        SELECT
                          DILT.{FirstLineId}, DILT.{SecondLineId}, DILT.{ThirdLineId}, DILT.{FourthLineId}, DILT.{FifthLineId}, DILT.{SixthLineId},
                          DILT.{SeventhLineId}, DILT.{EighthLineId}, DILT.{NinthLineId}, DILT.{TenLineId},DILT.{EleventhLineId}, DILT.{TwelfthLineId}
                        FROM {DepositIdLineTable} DILT
                        WHERE {InvoiceId} = {invoiceId}
                    ";

                DepositIdLoadModel getId = dbConnection.QuerySingle<DepositIdLoadModel>(loadIdQuery);
                return getId;
            }
        }
        
    }
}
