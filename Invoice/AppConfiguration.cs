using System.IO;
using System.Reflection;

namespace Invoice
{
    class AppConfiguration
    {
        private static readonly string DatabaseName = "InvoiceFactory";
        
        public static string PdfFolder => $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\PdfFiles";
        public static string DatabaseFolder => $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\Database";
        public static string DatabaseFile => $"{DatabaseFolder}\\{DatabaseName}.db";
        public static string ConnectionString => $"Data Source={DatabaseFile};Version=3;UseUTF16Encoding=True; foreign keys=true;";
    }
}
