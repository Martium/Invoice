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
                CreateSellerInfoTable(dbConnection);
                CreateProductTypeTable(dbConnection);
                CreateStorageTable(dbConnection);
                CreatePasswordTable(dbConnection);
                CreateBuyersInfoTable(dbConnection);
                CreateProductInfoTable(dbConnection);
                CreateDepositTable(dbConnection);
                CreateDepositIdLineTable(dbConnection);
                CreateMoneyReceiptTable(dbConnection);

                SetDefaultPassword(dbConnection);
                SetDefaultMoneyReceiptNumber(dbConnection);

#if DEBUG
                //FillInvoiceTestingInfo(dbConnection);
               // FillProductTypeTestingInfo(dbConnection);
               // FillStorageTestingInfo(dbConnection);
               // FillBuyersInfoTestingInfo(dbConnection);
              //  FillProductInfoTestingInfo(dbConnection);
               // FillDepositTestingInfo(dbConnection);
               // FillDepositIdLineTestingInfo(dbConnection);
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

            foreach (DirectoryInfo subdirectories in directory.GetDirectories())
            {
                subdirectories.Delete(true);
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
                        [PaymentStatus] [nvarchar]({FormSettings.TextBoxLengths.InvoiceIsPaid}) NOT NULL,

                        [TotalPriceWithPvm] [nvarchar]({FormSettings.TextBoxLengths.TotalPriceWithPvm}) NULL,
                        UNIQUE(InvoiceNumber, InvoiceNumberYearCreation)
                    );
                 ";

            SQLiteCommand createInvoiceTableCommand = new SQLiteCommand(createInvoiceTableQuery, dbConnection);
            createInvoiceTableCommand.ExecuteNonQuery();
        }

        private void CreateStorageTable(SQLiteConnection dbConnection)
        {
            string dropAStorageTableQuery = GetDropTableQuery("Storage");
            SQLiteCommand dropAStorageTableCommand = new SQLiteCommand(dropAStorageTableQuery, dbConnection);
            dropAStorageTableCommand.ExecuteNonQuery();

            string createStorageTableQuery =
                $@"
                    CREATE TABLE [Storage] (
                        [Id] [INTEGER] PRIMARY KEY AUTOINCREMENT NOT NULL,
                        
                        [StorageSerialNumber] [nvarchar] ({FormSettings.TextBoxLengths.StorageSerialNumber}) NOT NULL,
                        [StorageProductName] [nvarchar] ({FormSettings.TextBoxLengths.StorageProductName}) NOT NULL,
                        [StorageProductMadeDate] [Date] NOT NULL,
                        [StorageProductExpireDate] [Date] NOT NULL,
                        [StorageProductQuantity] [Numeric] NOT NULL,
                        [StorageProductPrice] [Numeric]  NULL,
                        
                        UNIQUE (Id)
                    );
                ";

            SQLiteCommand createStorageTableCommand = new SQLiteCommand(createStorageTableQuery, dbConnection);
            createStorageTableCommand.ExecuteNonQuery();
        }

        private void CreateDepositTable(SQLiteConnection dbConnection)
        {
            string dropADepositTableQuery = GetDropTableQuery("Deposit");
            SQLiteCommand dropADepositTableCommand = new SQLiteCommand(dropADepositTableQuery, dbConnection);
            dropADepositTableCommand.ExecuteNonQuery();

            string createDepositTableQuery =
                $@"
                    CREATE TABLE [Deposit] (
                        [Id] [INTEGER] NOT NULL,
                        [InvoiceYear] [INTEGER] NOT NULL,
                        [ProductName] [nvarchar]({FormSettings.TextBoxLengths.ProductName}) NULL,
                        [BarCode] [nvarchar]({FormSettings.TextBoxLengths.BarCode}) NULL,
                        [ProductQuantity] [NUMERIC] NULL,
                        
                        UNIQUE (Id)
                    );
                ";

            SQLiteCommand createDepositTableCommand = new SQLiteCommand(createDepositTableQuery, dbConnection);
            createDepositTableCommand.ExecuteNonQuery();
        }

        private void CreateDepositIdLineTable(SQLiteConnection dbConnection)
        {
            string dropADepositIdLineTable = GetDropTableQuery("DepositIdLine");
            SQLiteCommand dropADepositIdLineTableCommand = new SQLiteCommand(dropADepositIdLineTable, dbConnection);
            dropADepositIdLineTableCommand.ExecuteNonQuery();

            string createDepositIdLineTableQuery =
                $@"
                    CREATE TABLE [DepositIdLine] (
                        [InvoiceId] [INTEGER] NOT NULL,

                        [FirstLineId] [INTEGER] NOT NULL,
                        [SecondLineId] [INTEGER] NOT NULL,
                        [ThirdLineId] [INTEGER] NOT NULL,
                        [FourthLineId] [INTEGER] NOT NULL,
                        [FifthLineId] [INTEGER] NOT NULL,
                        [SixthLineId] [INTEGER] NOT NULL,
                        [SeventhLineId] [INTEGER] NOT NULL,
                        [EighthLineId] [INTEGER] NOT NULL,
                        [NinthLineId] [INTEGER] NOT NULL,
                        [TenLineId] [INTEGER] NOT NULL,
                        [EleventhLineId] [INTEGER] NOT NULL,
                        [TwelfthLineId] [INTEGER] NOT NULL,
                        
                        UNIQUE (InvoiceId)
                    );
                ";

            SQLiteCommand createDepositIdLineCommand = new SQLiteCommand(createDepositIdLineTableQuery, dbConnection);
            createDepositIdLineCommand.ExecuteNonQuery();
        }

        private void CreateMoneyReceiptTable(SQLiteConnection dbConnection)
        {
            string dropAMoneyReceiptTableQuery = GetDropTableQuery("MoneyReceipt");
            SQLiteCommand dropAMoneyReceiptCommand = new SQLiteCommand(dropAMoneyReceiptTableQuery, dbConnection);
            dropAMoneyReceiptCommand.ExecuteNonQuery();

            string createMoneyReceiptTableQuery =
                $@"
                    CREATE TABLE [MoneyReceipt] (
                        [Id] [INTEGER] NOT NULL,
                        [MoneyReceiptSuggestedNumber] [INTEGER] ({FormSettings.TextBoxLengths.MaxNumberLength}) NOT NULL,
                        
                        UNIQUE (Id)
                    );
                ";

            SQLiteCommand createMoneyReceiptCommand = new SQLiteCommand(createMoneyReceiptTableQuery, dbConnection);
            createMoneyReceiptCommand.ExecuteNonQuery();

        }

        private void CreateSellerInfoTable(SQLiteConnection dbConnection)
        {
            string dropSellerInfoTableQuery = GetDropTableQuery("SellerInfo");
            SQLiteCommand dropASellerInfoTableCommand = new SQLiteCommand(dropSellerInfoTableQuery, dbConnection);
            dropASellerInfoTableCommand.ExecuteNonQuery();

            string createSellerInfoTableQuery =
                $@"
                   CREATE TABLE [SellerInfo] (
                      [Id] [INTEGER] NOT NULL,
                      [SerialNumber] [nvarchar]({FormSettings.TextBoxLengths.SerialNumber}) NULL,
                      [SellerName] [nvarchar]({FormSettings.TextBoxLengths.SellerName}) NULL,
                      [SellerFirmCode] [nvarchar]({FormSettings.TextBoxLengths.SellerFirmCode}) NULL,
                      [SellerPvmCode] [nvarchar]({FormSettings.TextBoxLengths.SellerPvmCode}) NULL,
                      [SellerAddress] [nvarchar]({FormSettings.TextBoxLengths.SellerAddress}) NULL,
                      [SellerPhoneNumber] [nvarchar]({FormSettings.TextBoxLengths.SellerPhoneNumber}) NULL,
                      [SellerBank] [nvarchar]({FormSettings.TextBoxLengths.SellerBank}) NULL,
                      [SellerBankAccountNumber] [nvarchar]({FormSettings.TextBoxLengths.SellerBankAccountNumber}) NULL,
                      [SellerEmailAddress] [nvarchar]({FormSettings.TextBoxLengths.SellerEmailAddress}) NULL,
                      [InvoiceMaker] [nvarchar]({FormSettings.TextBoxLengths.InvoiceMaker}) NULL
                   );
                ";

            SQLiteCommand createSellerInfoTableCommand = new SQLiteCommand(createSellerInfoTableQuery, dbConnection);
            createSellerInfoTableCommand.ExecuteNonQuery();
        }

        private void CreateProductTypeTable(SQLiteConnection dbConnection)
        {
            string dropProductTypeTable = GetDropTableQuery("ProductType");
            SQLiteCommand dropAProductTypeTableCommand = new SQLiteCommand(dropProductTypeTable, dbConnection);
            dropAProductTypeTableCommand.ExecuteNonQuery();

            string createProductTypeTableQuery =
                $@"
                    CREATE TABLE [ProductType] (
                        [IdByInvoiceNumber] [INTEGER] NOT NULL,
                        [IdByInvoiceNumberYearCreation] [INTEGER] NOT NULL,
                        
                        [FirstProductType] [nvarchar] ({FormSettings.TextBoxLengths.ProductType}) NULL,
                        [SecondProductType] [nvarchar] ({FormSettings.TextBoxLengths.ProductType}) NULL,
                        [ThirdProductType] [nvarchar] ({FormSettings.TextBoxLengths.ProductType}) NULL,
                        [FourthProductType] [nvarchar] ({FormSettings.TextBoxLengths.ProductType}) NULL,
                        [FifthProductType] [nvarchar] ({FormSettings.TextBoxLengths.ProductType}) NULL,
                        [SixthProductType] [nvarchar] ({FormSettings.TextBoxLengths.ProductType}) NULL,
                        [SeventhProductType] [nvarchar] ({FormSettings.TextBoxLengths.ProductType}) NULL,
                        [EighthProductType] [nvarchar] ({FormSettings.TextBoxLengths.ProductType}) NULL,
                        [NinthProductType] [nvarchar] ({FormSettings.TextBoxLengths.ProductType}) NULL,
                        [TenProductType] [nvarchar] ({FormSettings.TextBoxLengths.ProductType}) NULL,
                        [EleventhProductType] [nvarchar] ({FormSettings.TextBoxLengths.ProductType}) NULL,
                        [TwelfthProductType] [nvarchar] ({FormSettings.TextBoxLengths.ProductType}) NULL,

                        [FirstProductTypeQuantity] [NUMERIC] NULL,
                        [SecondProductTypeQuantity] [NUMERIC] NULL,
                        [ThirdProductTypeQuantity] [NUMERIC] NULL,
                        [FourthProductTypeQuantity] [NUMERIC] NULL,
                        [FifthProductTypeQuantity] [NUMERIC] NULL,
                        [SixthProductTypeQuantity] [NUMERIC] NULL,
                        [SeventhProductTypeQuantity] [NUMERIC] NULL,
                        [EighthProductTypeQuantity] [NUMERIC] NULL,
                        [NinthProductTypeQuantity] [NUMERIC] NULL,
                        [TenProductTypeQuantity] [NUMERIC] NULL,
                        [EleventhProductTypeQuantity] [NUMERIC] NULL,
                        [TwelfthProductTypeQuantity] [NUMERIC] NULL,

                        [FirstProductTypePrice] [NUMERIC] NULL,
                        [SecondProductTypePrice] [NUMERIC] NULL,
                        [ThirdProductTypePrice] [NUMERIC] NULL,
                        [FourthProductTypePrice] [NUMERIC] NULL,
                        [FifthProductTypePrice] [NUMERIC] NULL,
                        [SixthProductTypePrice] [NUMERIC] NULL,
                        [SeventhProductTypePrice] [NUMERIC] NULL,
                        [EighthProductTypePrice] [NUMERIC] NULL,
                        [NinthProductTypePrice] [NUMERIC] NULL,
                        [TenProductTypePrice] [NUMERIC] NULL,
                        [EleventhProductTypePrice] [NUMERIC] NULL,
                        [TwelfthProductTypePrice] [NUMERIC] NULL,
                        
                        UNIQUE (IdByInvoiceNumber, IdByInvoiceNumberYearCreation)
                    );
                        
                ";

            SQLiteCommand createProductTypeTableCommand = new SQLiteCommand(createProductTypeTableQuery, dbConnection);
            createProductTypeTableCommand.ExecuteNonQuery();
        }

        private void CreatePasswordTable(SQLiteConnection dbConnection)
        {
            string dropPasswordTable = GetDropTableQuery("Password");
            SQLiteCommand dropAPasswordTableCommand = new SQLiteCommand(dropPasswordTable, dbConnection);
            dropAPasswordTableCommand.ExecuteNonQuery();

            string createPasswordTableQuery = 
                $@"
                    CREATE TABLE [Password] (
                        [Id] [INTEGER]  NOT NULL,
                        [Password] [nvarchar] ({FormSettings.TextBoxLengths.Password}) NOT NULL,
                        [IsPasswordCorrect] [nvarchar] ({FormSettings.TextBoxLengths.IsPasswordCorrect}) NOT NULL,

                        UNIQUE (Id)
                    );
                ";

            SQLiteCommand createPasswordCommand = new SQLiteCommand(createPasswordTableQuery, dbConnection);
            createPasswordCommand.ExecuteNonQuery();
        }

        private void CreateBuyersInfoTable(SQLiteConnection dbConnection)
        {
            string dropBuyersInfoTable = GetDropTableQuery("BuyersInfo");
            SQLiteCommand dropABuyersInfoTable = new SQLiteCommand(dropBuyersInfoTable, dbConnection);
            dropABuyersInfoTable.ExecuteNonQuery();

            string createBuyersInfoTableQuery =
                $@"
                    CREATE TABLE [BuyersInfo] (
                        [Id] [INTEGER] PRIMARY KEY AUTOINCREMENT NOT NULL,
                        [BuyerName] [nvarchar]({FormSettings.TextBoxLengths.BuyerName}) NULL,
                        [BuyerFirmCode] [nvarchar]({FormSettings.TextBoxLengths.BuyerFirmCode}) NULL,
                        [BuyerPvmCode] [nvarchar]({FormSettings.TextBoxLengths.BuyerPvmCode}) NULL,
                        [BuyerAddress] [nvarchar]({FormSettings.TextBoxLengths.BuyerAddress}) NULL,

                        UNIQUE (Id)
                    );
                ";

            SQLiteCommand createBuyersInfoCommand = new SQLiteCommand(createBuyersInfoTableQuery, dbConnection);
            createBuyersInfoCommand.ExecuteNonQuery();
        }

        private void CreateProductInfoTable(SQLiteConnection dbConnection)
        {
            string dropProductInfoTable = GetDropTableQuery("ProductInfo");
            SQLiteCommand dropAProductInfoTable = new SQLiteCommand(dropProductInfoTable, dbConnection);
            dropAProductInfoTable.ExecuteNonQuery();

            string createProductInfoTableQuery =
                $@"
                    CREATE TABLE [ProductInfo] (
                        [Id] [INTEGER] PRIMARY KEY AUTOINCREMENT NOT NULL,
                        [Year] [INTEGER] NOT NULL,
                        [ProductName] [nvarchar]({FormSettings.TextBoxLengths.ProductName}) NULL,
                        [BarCode] [nvarchar]({FormSettings.TextBoxLengths.BarCode}) NULL,
                        [ProductSees] [nvarchar]({FormSettings.TextBoxLengths.FirstProductSees}) NULL,
                        [ProductPrice] [NUMERIC] NULL,

                        [ProductType] [nvarchar]({FormSettings.TextBoxLengths.ProductType}) NULL,
                        [ProductTypePrice] [NUMERIC] NULL,

                        UNIQUE (Id)
                    );
                ";

            SQLiteCommand createProductInfoCommand = new SQLiteCommand(createProductInfoTableQuery, dbConnection);
            createProductInfoCommand.ExecuteNonQuery();
        }

        private void SetDefaultPassword(SQLiteConnection dbConnection)
        {
            string setDefaultPassword =
                @"BEGIN TRANSACTION;
                    INSERT INTO 'Password'
                        VALUES (1, '1234', 'false');
                  COMMIT;
                ";

            SQLiteCommand setDefaultPasswordCommand = new SQLiteCommand(setDefaultPassword, dbConnection);
            setDefaultPasswordCommand.ExecuteNonQuery();
        }

        private void SetDefaultMoneyReceiptNumber(SQLiteConnection dbConnection)
        {
            string setDefaultMoneyReceiptNumber =
                @"BEGIN TRANSACTION;
                    INSERT INTO 'MoneyReceipt'
                        VALUES (1, 1);
                  COMMIT;
                ";

            SQLiteCommand setDefaultMoneyReceiptCommand = new SQLiteCommand(setDefaultMoneyReceiptNumber, dbConnection);
            setDefaultMoneyReceiptCommand.ExecuteNonQuery();
        }

        private void FillInvoiceTestingInfo(SQLiteConnection dbConnection)
        {

            string fillInvoiceTestingInfo =
                $@"BEGIN TRANSACTION;
                    INSERT INTO 'Invoice'
                        Values (1, {DateTime.Now.AddYears(-1).Year}, '2021-04-02', 'ANA', 'Ežio ūkis', '305652600', 'LT 10lkuyt27916', 'Europos pr. 1, LT kazkas Kaunas', '58454587', 'Swedbank', 'LT857304444165354444', 
                        'ezioukis@gmail.com', 'Litbana', '110395066', 'LT100000022014', 'Kirtimų g. 57D, Vilnius', 'obuoliu sultys 3l 1', 'granatu sultys 3l 2', 'apelsinu sultys 3l 3', 'morku obuoliu mandarinu sultys 3l 4', 
                        'morku obuoliu  sultys 3l 5', 'imbiero obuliu sultys 3l 6', 'obuliu kriausiu 3l 7', 'obuoliu vynuogiu 3l 8', ' obuoliu aronijų 3l 9', ' obuoliu juodujų serbentų sultys 3l 10', 'obuoliai 5l 11', 'obuoliu apelsinu 5l 12', 'vnt1', 'vnt2', 'vnt3', 'vnt4', 'vnt5', 'vnt6', 'vnt7', 'vnt8', 'vnt9', 'vnt10', 'vnt11', 'vnt12', 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 1.1, 2.2, 2.3, 4.4, 5.5, 2.6, 2.7, 2.8, 2.9, 1, 1.11, 1.12, 'devyniasdešimtdevyni eurai 75ct', 'Direktorius Vi4785jus 12a5894nas', 'Bazinga Bazingius is Libanos', 'Nesumokėta', '213.14' );
                    INSERT INTO 'Invoice'
                        Values (2, {DateTime.Now.Year}, '2021-08-30', 'ANA', 'Ežio ūkis', '305652600', 'LT 10lkiop27916', 'Europos pr. ka, LT 46kk0 Kaunas', '867k3l5l1', 'Swedbank', 'LT85730001lkjh352098', 
                        'ezioujkis@gmail.com', 'Kazkas', '110395066', 'LT100000022014', 'Kirtimų g. 57D, Vilnius', 'obuoliu sultys 3l', 'granatu sultys 3l', 'apelsinu sultys 3l', 'morku obuoliu mandarinu sultys 3l', 
                        'morku obuoliu  sultys 3l', 'imbiero obuliu sultys 3l', 'obuliu kriausiu 3l', 'obuoliu vynuogiu 3l', ' obuoliu aronijų 3l', ' obuoliu juodujų serbentų sultys 3l', 'obuoliai 5l', 'obuoliu apelsinu 5l', 'vnt1', 'vnt2', 'vnt3', 'vnt4', 'vnt5', 'vnt6', 'vnt7', 'vnt8', 'vnt9', 'vnt10', 'vnt11', 'vnt12', 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 1.1, 2.2, 2.3, 4.4, 5.5, 2.6, 2.7, 2.8, 2.9, 1, 1.11, 1.12, 'devyniasdešimtdevyni eurai 75ct', 'Direktorius 124piulus Prlns4k12s', 'Bazinga Bazingius is Libanos', 'Nesumokėta', '213.14' );
                    INSERT INTO 'Invoice'
                        Values (3, {DateTime.Now.Year}, '2020-04-02', 'ANA', 'Ežio ūkis', '305652600', 'LT 100kjhgf7916', 'Europos pr. 34-47, LT 46370 Kaunas', '8675lll81', 'Swedbank', 'LT8573000101lkjh2098', 
                        'ezioukis@gmail.com', 'bazinga', '110395066', 'LT100000022014', 'Kirtimų g. 57D, Vilnius', 'obuoliu sultys 3l', 'granatu sultys 3l', 'apelsinu sultys 3l', 'morku obuoliu mandarinu sultys 3l', 
                        'morku obuoliu  sultys 3l', 'imbiero obuliu sultys 3l', 'obuliu kriausiu 3l', 'obuoliu vynuogiu 3l', ' obuoliu aronijų 3l', ' obuoliu juodujų serbentų sultys 3l', 'obuoliai 5l', 'obuoliu apelsinu 5l', 'vnt1', 'vnt2', 'vnt3', 'vnt4', 'vnt5', 'vnt6', 'vnt7', 'vnt8', 'vnt9', 'vnt10', 'vnt11', 'vnt12', 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 1.1, 2.2, 2.3, 4.4, 5.5, 2.6, 2.7, 2.8, 2.9, 1, 1.11, 1.12, 'devyniasdešimtdevyni eurai 75ct', 'Direktorius Vitalkjh Prkiol14as', 'Bazinga Bazingius is Libanos', 'Atsiskaityta', '213.14' );
                   COMMIT;
                  ";

            SQLiteCommand fillInvoiceTestingInfoTableCommand = new SQLiteCommand(fillInvoiceTestingInfo, dbConnection);
            fillInvoiceTestingInfoTableCommand.ExecuteNonQuery();
        }

        private void FillProductTypeTestingInfo(SQLiteConnection dbConnection)
        {
            string fillProductTypeTestingInfo =
                $@"BEGIN TRANSACTION;
                    INSERT INTO 'ProductType'
                        Values (1, {DateTime.Now.AddYears(-1).Year}, '1l', '2l', '3l', '4l', '5l', '6l', '7l', '8l', '9l', '10l', '11l', '12l', 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 1.1, 2.2, 3.3, 4.4, 5.5, 6.6, 7.7, 8.8, 9.9, 10.1, 11.11, 12.12 );
                    INSERT INTO 'ProductType'
                        Values (2, {DateTime.Now.Year}, '1l', '2l', '3l', '4l', '5l', '6l', '7l', '8l', '9l', '10l', '11l', '12l', 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 1.1, 2.2, 3.3, 4.4, 5.5, 6.6, 7.7, 8.8, 9.9, 10.1, 11.11, 12.12 );
                    INSERT INTO 'ProductType'
                        Values (3, {DateTime.Now.Year}, '1l', '2l', '3l', '4l', '5l', '6l', '7l', '8l', '9l', '10l', '11l', '12l', 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 1.1, 2.2, 3.3, 4.4, 5.5, 6.6, 7.7, 8.8, 9.9, 10.1, 11.11, 12.12 );
                   COMMIT;
                ";

            SQLiteCommand fillProductTypeTestingInfoCommand = new SQLiteCommand(fillProductTypeTestingInfo, dbConnection);
            fillProductTypeTestingInfoCommand.ExecuteNonQuery();
        }

        private void FillStorageTestingInfo(SQLiteConnection dbConnection)
        {
            string fillStorageTestingInfoQuery =
                @"BEGIN TRANSACTION;
                    INSERT INTO 'Storage'
                        Values (NULL, 'A-1234BC', '1L APELSINAS', '2021-06-03', '2022-06-03', 1, 1.1);
                    INSERT INTO 'Storage'
                        Values (NULL, 'B-1234BC', '2L APELSINAS', '2022-02-04', '2022-06-03', 2, 2.2);
                    INSERT INTO 'Storage'
                        Values (NULL, 'C-1234BC', '2L APELSINAS', '2022-05-05', '2022-06-03', 3, 3.3);
                   COMMIT;
                ";

            SQLiteCommand fillStorageTestingInfoCommand = new SQLiteCommand(fillStorageTestingInfoQuery, dbConnection);
            fillStorageTestingInfoCommand.ExecuteNonQuery();
        }

        private void FillBuyersInfoTestingInfo(SQLiteConnection dbConnection)
        {
            string fillBuyersInfoTestingInfo =
                @"BEGIN TRANSACTION;
                    INSERT INTO 'BuyersInfo'
                        Values (NULL, 'Litbana', '1', '2', '3');
                    INSERT INTO 'BuyersInfo'
                        Values (NULL, 'Bazinga', '11', '22', '33');
                    INSERT INTO 'BuyersInfo'
                        Values (NULL, 'Bazinge', '111', '222', '333');
                   COMMIT;
                ";

            SQLiteCommand fillTestingBuyersInfoCommand = new SQLiteCommand(fillBuyersInfoTestingInfo, dbConnection);
            fillTestingBuyersInfoCommand.ExecuteNonQuery();
        }

        private void FillProductInfoTestingInfo(SQLiteConnection dbConnection)
        {
            string text = "sjdajsdfnasdfasdfasdcsadcac//dfsdfvsdfvsdfasdasdc";
            text = text.Replace("//", "" + Environment.NewLine);

            string fillProductTestingInfo =
                $@"BEGIN TRANSACTION;
                    INSERT INTO 'ProductInfo'
                        Values (NULL, {DateTime.Now.AddYears(-1).Year}, '{text}', '1 23456 7891123', 'vnt', 1, '1l', 0.2);
                    INSERT INTO 'ProductInfo'
                        Values (NULL, {DateTime.Now.Year},'2 LITRAI','2 23456 7592123', 'vnt', 2, '2l', 0.2);
                    INSERT INTO 'ProductInfo'
                        Values (NULL, {DateTime.Now.Year}, '3 LITRAI', '3 23456 7891143', 'vnt', 3, '3l', 0.2);
                    COMMIT;
                ";

            SQLiteCommand fillTestingProductinfoCommand = new SQLiteCommand(fillProductTestingInfo, dbConnection);
            fillTestingProductinfoCommand.ExecuteNonQuery();
        }

        private void FillDepositTestingInfo(SQLiteConnection dbConnection)
        {
            string text = "sjdajsdfnasdfasdfasdcsadcac//dfsdfvsdfvsdfasdasdc";
            text = text.Replace("//", "" + Environment.NewLine);

            string fillDepositInfo =
                $@"BEGIN TRANSACTION;
                    INSERT INTO 'Deposit'
                        Values (1, {DateTime.Now.AddYears(-1).Year}, '{text}', '1 23456 7891123', 0);
                    INSERT INTO 'Deposit'
                        Values (2, {DateTime.Now.Year}, '2 LITRAI', '2 23456 7592123', 0);
                    INSERT INTO 'Deposit'
                        Values (3, {DateTime.Now.Year}, '3 LITRAI', '3 23456 7891143', 0);
                    COMMIT;
                ";

            SQLiteCommand fillTestingDepositInfoCommand = new SQLiteCommand(fillDepositInfo, dbConnection);
            fillTestingDepositInfoCommand.ExecuteNonQuery();
        }

        private void FillDepositIdLineTestingInfo(SQLiteConnection dbConnection)
        {
            string fillDepositIdLineInfo =
                $@"BEGIN TRANSACTION;
                    INSERT INTO 'DepositIdLine'
                        Values (1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                    INSERT INTO 'DepositIdLine'
                        Values (2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                    INSERT INTO 'DepositIdLine'
                        Values (3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                    COMMIT;
                ";

            SQLiteCommand fillTestingInfoCommand = new SQLiteCommand(fillDepositIdLineInfo, dbConnection);
            fillTestingInfoCommand.ExecuteNonQuery();
        }

        private string GetDropTableQuery(string tableName)
        {
            return $"DROP TABLE IF EXISTS [{tableName}]";
        }
    }
}
