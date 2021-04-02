using System.Data.SQLite;
using System.IO;

namespace Invoice.Repositories
{
    class DataBaseInitializerRepository
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

                //CreateInvoiceTable(dbConnection);

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

        private void CreateInvoiceFormTable(SQLiteConnection dbConnection)
        {
            string dropInvoiceTableQuery = GetDropTableQuery("Invoice");
            SQLiteCommand dropAInvoiceTableCommand = new SQLiteCommand(dropInvoiceTableQuery, dbConnection);
            dropAInvoiceTableCommand.ExecuteNonQuery();

            string createInvoiceTableQuery =
                @"
                    CREATE TABLE [Invoice] (
                        
                    );
                 ";
        }

        private string GetDropTableQuery(string tableName)
        {
            return $"DROP TABLE IF EXISTS [{tableName}]";
        }
    }
}
