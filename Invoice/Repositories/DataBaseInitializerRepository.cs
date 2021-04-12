using System;
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
                FillInvoiceTestingInfo(dbConnection);
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
                        [InvoiceAccepted] [nvarchar]({FormSettings.TextBoxLengths.InvoiceAccepted}) NULL,
                        [InvoiceIsPaid] [nvarchar] NOT NULL,
                        UNIQUE(InvoiceNumber, InvoiceNumberYearCreation)
                    );
                 ";

            SQLiteCommand createInvoiceTableCommand = new SQLiteCommand(createInvoiceTableQuery, dbConnection);
            createInvoiceTableCommand.ExecuteNonQuery();
        }

        private void FillInvoiceTestingInfo(SQLiteConnection dbConnection)
        {

            string fillInvoiceTestingInfo =
                $@"BEGIN TRANSACTION;
                    INSERT INTO 'Invoice'
                        Values (1, {DateTime.Now.Year}, '2021-04-02', 'ANA', 'Ežio ūkis', '305652600', 'LT 100013527916', 'Europos pr. 34-47, LT 46370 Kaunas', '867538581', 'Swedbank', 'LT857300010165352098', 
                        'ezioukis@gmail.com', 'Litbana', '110395066', 'LT100000022014', 'Kirtimų g. 57D, Vilnius', 'obuoliu sultys 3l 1', 'granatu sultys 3l 2', 'apelsinu sultys 3l 3', 'morku obuoliu mandarinu sultys 3l 4', 
                        'morku obuoliu  sultys 3l 5', 'imbiero obuliu sultys 3l 6', 'obuliu kriausiu 3l 7', 'obuoliu vynuogiu 3l 8', ' obuoliu aronijų 3l 9', ' obuoliu juodujų serbentų sultys 3l 10', 'obuoliai 5l 11', 'obuoliu apelsinu 5l 12', 'vnt1', 'vnt2', 'vnt3', 'vnt4', 'vnt5', 'vnt6', 'vnt7', 'vnt8', 'vnt9', 'vnt10', 'vnt11', 'vnt12', 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 1.1, 2.2, 2.3, 4.4, 5.5, 2.6, 2.7, 2.8, 2.9, 1, 1.11, 1.12, 'devyniasdešimtdevyni eurai 75ct', 'Direktorius Vitalijus Pranskūnas', 'Bazinga Bazingius is Libanos', 'Nesumokėta');
                    INSERT INTO 'Invoice'
                        Values (2, {DateTime.Now.Year}, '2021-08-30', 'ANA', 'Ežio ūkis', '305652600', 'LT 100013527916', 'Europos pr. 34-47, LT 46370 Kaunas', '867538581', 'Swedbank', 'LT857300010165352098', 
                        'ezioukis@gmail.com', 'Kazkas', '110395066', 'LT100000022014', 'Kirtimų g. 57D, Vilnius', 'obuoliu sultys 3l', 'granatu sultys 3l', 'apelsinu sultys 3l', 'morku obuoliu mandarinu sultys 3l', 
                        'morku obuoliu  sultys 3l', 'imbiero obuliu sultys 3l', 'obuliu kriausiu 3l', 'obuoliu vynuogiu 3l', ' obuoliu aronijų 3l', ' obuoliu juodujų serbentų sultys 3l', 'obuoliai 5l', 'obuoliu apelsinu 5l', 'vnt1', 'vnt2', 'vnt3', 'vnt4', 'vnt5', 'vnt6', 'vnt7', 'vnt8', 'vnt9', 'vnt10', 'vnt11', 'vnt12', 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 1.1, 2.2, 2.3, 4.4, 5.5, 2.6, 2.7, 2.8, 2.9, 1, 1.11, 1.12, 'devyniasdešimtdevyni eurai 75ct', 'Direktorius Vitalijus Pranskūnas', 'Bazinga Bazingius is Libanos', 'Nesumokėta');
                    INSERT INTO 'Invoice'
                        Values (1, {DateTime.Now.AddYears(-1).Year}, '2020-04-02', 'ANA', 'Ežio ūkis', '305652600', 'LT 100013527916', 'Europos pr. 34-47, LT 46370 Kaunas', '867538581', 'Swedbank', 'LT857300010165352098', 
                        'ezioukis@gmail.com', 'bazinga', '110395066', 'LT100000022014', 'Kirtimų g. 57D, Vilnius', 'obuoliu sultys 3l', 'granatu sultys 3l', 'apelsinu sultys 3l', 'morku obuoliu mandarinu sultys 3l', 
                        'morku obuoliu  sultys 3l', 'imbiero obuliu sultys 3l', 'obuliu kriausiu 3l', 'obuoliu vynuogiu 3l', ' obuoliu aronijų 3l', ' obuoliu juodujų serbentų sultys 3l', 'obuoliai 5l', 'obuoliu apelsinu 5l', 'vnt1', 'vnt2', 'vnt3', 'vnt4', 'vnt5', 'vnt6', 'vnt7', 'vnt8', 'vnt9', 'vnt10', 'vnt11', 'vnt12', 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 1.1, 2.2, 2.3, 4.4, 5.5, 2.6, 2.7, 2.8, 2.9, 1, 1.11, 1.12, 'devyniasdešimtdevyni eurai 75ct', 'Direktorius Vitalijus Pranskūnas', 'Bazinga Bazingius is Libanos', 'Sumokėta');
                   COMMIT;
                  ";

            SQLiteCommand fillInvoiceTestingInfoTableCommand = new SQLiteCommand(fillInvoiceTestingInfo, dbConnection);
            fillInvoiceTestingInfoTableCommand.ExecuteNonQuery();
            
        }

        private string GetDropTableQuery(string tableName)
        {
            return $"DROP TABLE IF EXISTS [{tableName}]";
        }
    }
}
