using System.Data.SQLite;
using System.IO;
using Invoice.Constants;

namespace Invoice.Repositories
{
    public class DataBaseInitializerRepository
    {
        public void InitializeDatabaseIfNotExist()
        {
            if (File.Exists(AppConfiguration.DatabaseFile))
            {
#if DEBUG

#else
                return;
#endif
            }

            if (!Directory.Exists(AppConfiguration.DatabaseFolder))
            {
                Directory.CreateDirectory(AppConfiguration.DatabaseFolder);
            }
            else
            {
                DeleteLeftoverFilesAndFolders();
            }

            SQLiteConnection.CreateFile(AppConfiguration.DatabaseFile);

            using (var dbConnection = new SQLiteConnection(AppConfiguration.ConnectionString))
            {
                dbConnection.Open();

                CreateInvoiceTable(dbConnection);

#if DEBUG
                // FillInvoiceDefaultInfo(dbConnection);
#endif

            }
        }

        private void DeleteLeftoverFilesAndFolders()
        {
            var directory = new DirectoryInfo(AppConfiguration.DatabaseFolder);

            foreach (FileInfo file in directory.GetFiles())
            {
                file.Delete();
            }

            foreach (DirectoryInfo subdirectory in directory.GetDirectories())
            {
                subdirectory.Delete(true);
            }
        }

        private void CreateInvoiceTable(SQLiteConnection dbConnection)
        {
            string dropInvoiceTableQuery = GetDropTableQuery("Invoice");
            SQLiteCommand dropAInvoiceTableCommand = new SQLiteCommand(dropInvoiceTableQuery, dbConnection);
            dropAInvoiceTableCommand.ExecuteNonQuery();

            string createInvoiceTableQuery =
                $@"
                    CREATE TABLE [Invoice] (
                        [InvoiceNumber] [INTEGER] NOT NULL,
                        [InvoiceNumberYearCreation] [INTEGER] NOT NULL,
                        [InvoiceDate] [Date] NOT NULL,
                        [SerialNumber] [nvarchar]({FormSettings.TextBoxLengths.SerialNumber}) NULL,
                        [SellerName] [nvarchar]({FormSettings.TextBoxLengths.SellerName}) NULL,
                        [SellerFirmCode] [nvarchar]({FormSettings.TextBoxLengths.SellerFirmCode}) NULL,
                        [SellerPvmCode] [nvarchar]({FormSettings.TextBoxLengths.SellerPvmCode}) NULL,
                        [SellerAddress] [nvarchar]({FormSettings.TextBoxLengths.SellerAddress}) NULL,
                        [SellerPhoneNumber] [nvarchar]({FormSettings.TextBoxLengths.SellerPhoneNumber}) NULL,
                        [SellerBank] [nvarchar]({FormSettings.TextBoxLengths.SellerBank}) NULL,
                        [SellerBankAccountNumber] [nvarchar]({FormSettings.TextBoxLengths.SellerBankAccountNumber}) NULL,
                        [SellerEmailAddress] [nvarchar]({FormSettings.TextBoxLengths.SellerEmailAddress}) NULL,
                        [BuyerName] [nvarchar]({FormSettings.TextBoxLengths.BuyerName}) NULL,
                        [BuyerFirmCode] [nvarchar]({FormSettings.TextBoxLengths.BuyerFirmCode}) NULL,
                        [BuyerPvmCode] [nvarchar]({FormSettings.TextBoxLengths.BuyerPvmCode}) NULL,
                        [BuyerAddress] [nvarchar]({FormSettings.TextBoxLengths.BuyerAddress}) NULL,
                        [FirstProductName] [nvarchar]({FormSettings.TextBoxLengths.FirstProductName}) NULL,
                        [SecondProductName] [nvarchar]({FormSettings.TextBoxLengths.SecondProductName}) NULL,
                        [ThirdProductName] [nvarchar]({FormSettings.TextBoxLengths.ThirdProductName}) NULL,
                        [FourthProductName] [nvarchar]({FormSettings.TextBoxLengths.FourthProductName}) NULL,
                        [FifthProductName] [nvarchar]({FormSettings.TextBoxLengths.FifthProductName}) NULL,
                        [SixthProductName] [nvarchar]({FormSettings.TextBoxLengths.SixthProductName}) NULL,
                        [SeventhProductName] [nvarchar]({FormSettings.TextBoxLengths.SeventhProductName}) NULL,
                        [EighthProductName] [nvarchar]({FormSettings.TextBoxLengths.EighthProductName}) NULL,
                        [NinthProductName] [nvarchar]({FormSettings.TextBoxLengths.NinthProductName}) NULL,
                        [TenProductName] [nvarchar]({FormSettings.TextBoxLengths.TenProductName}) NULL,
                        [EleventhProductName] [nvarchar]({FormSettings.TextBoxLengths.EleventhProductName}) NULL,
                        [TwelfthProductName] [nvarchar]({FormSettings.TextBoxLengths.TwelfthProductName}) NULL,
                        [FirstProductSees] [nvarchar]({FormSettings.TextBoxLengths.FirstProductSees}) NULL,
                        [SecondProductSees] [nvarchar]({FormSettings.TextBoxLengths.SecondProductSees}) NULL,
                        [ThirdProductSees] [nvarchar]({FormSettings.TextBoxLengths.ThirdProductSees}) NULL,
                        [ForthProductSees] [nvarchar]({FormSettings.TextBoxLengths.FourthProductSees}) NULL,
                        [FifthProductSees] [nvarchar]({FormSettings.TextBoxLengths.FifthProductSees}) NULL,
                        [SixthProductSees] [nvarchar]({FormSettings.TextBoxLengths.SixthProductSees}) NULL,
                        [SeventhProductSees] [nvarchar]({FormSettings.TextBoxLengths.SeventhProductSees}) NULL,
                        [EighthProductSees] [nvarchar]({FormSettings.TextBoxLengths.EighthProductSees}) NULL,
                        [NinthProductSees] [nvarchar]({FormSettings.TextBoxLengths.NinthProductSees}) NULL,
                        [TenProductSees] [nvarchar]({FormSettings.TextBoxLengths.TenProductSees}) NULL,
                        [EleventhProductSees] [nvarchar]({FormSettings.TextBoxLengths.EleventhProductSees}) NULL,
                        [TwelfthProductSees] [nvarchar]({FormSettings.TextBoxLengths.TwelfthProductSees}) NULL,
                        [FirstProductQuantity] [NUMERIC] NULL,
                        [SecondProductQuantity] [NUMERIC] NULL,
                        [ThirdProductQuantity] [NUMERIC] NULL,
                        [FourthProductQuantity] [NUMERIC] NULL,
                        [FifthProductQuantity] [NUMERIC] NULL,
                        [SixthProductQuantity] [NUMERIC] NULL,
                        [SeventhProductQuantity] [NUMERIC] NULL,
                        [EighthProductQuantity] [NUMERIC] NULL,
                        [NinthProductQuantity] [NUMERIC] NULL,
                        [TenProductQuantity] [NUMERIC] NULL,
                        [EleventhProductQuantity] [NUMERIC] NULL,
                        [TwelfthProductQuantity] [NUMERIC] NULL,
                        [FirstProductPrice] [NUMERIC] NULL,
                        [SecondProductPrice] [NUMERIC] NULL,
                        [ThirdProductPrice] [NUMERIC] NULL,
                        [FourthProductPrice] [NUMERIC] NULL,
                        [FifthProductPrice] [NUMERIC] NULL,
                        [SixthProductPrice] [NUMERIC] NULL,
                        [SeventhProductPrice] [NUMERIC] NULL,
                        [EighthProductPrice] [NUMERIC] NULL,
                        [NinthProductPrice] [NUMERIC] NULL,
                        [TenProductPrice] [NUMERIC] NULL,
                        [EleventhProductPrice] [NUMERIC] NULL,
                        [TwelfthProductPrice] [NUMERIC] NULL,
                        [PriceInWords] [nvarchar]({FormSettings.TextBoxLengths.PriceInWords}) NULL,
                        [InvoiceMaker] [nvarchar]({FormSettings.TextBoxLengths.InvoiceMaker}) NULL,
                        [InvoiceAccepted] [nvarchar]({FormSettings.TextBoxLengths.InvoiceAccepted}) NULL
                    );
                 ";

            SQLiteCommand createInvoiceTableCommand = new SQLiteCommand(createInvoiceTableQuery, dbConnection);
            createInvoiceTableCommand.ExecuteNonQuery();
        }

        private string GetDropTableQuery(string tableName)
        {
            return $"DROP TABLE IF EXISTS [{tableName}]";
        }
    }
}
