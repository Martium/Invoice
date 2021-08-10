using System.IO;
using System.Reflection;

namespace Invoice
{
    class AppConfiguration
    {
        private static readonly string DatabaseName = "InvoiceFactory";
        private static readonly string DatabaseHelperName = "InvoiceHelpers";
        
        public static string PdfFolder => $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\PdfFiles";

        public static string DatabaseFolder => $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\Database";
        public static string DatabaseFolderHelper => $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\DatabaseHelper";

        public static string DatabaseFile => $"{DatabaseFolder}\\{DatabaseName}.db";
        public static string DatabaseFileHelper => $"{DatabaseFolderHelper}\\{DatabaseHelperName}.db";

        public static string ConnectionString => $"Data Source={DatabaseFile};Version=3;UseUTF16Encoding=True;";
        public static string ConnectionHelperString => $"Data Source={DatabaseFileHelper};Version=3;UseUTF16Encoding=True;";
    }
}
