using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Invoice
{
    class AppConfiguration
    {
        private static readonly string DatabaseName = "InvoiceFactory";

        public static string DatabaseFolder => $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\Database";
        public static string DatabaseFile => $"{DatabaseFolder}\\{DatabaseName}.db";
        public static string ConnectionString => $"Data Source={DatabaseFile};Version=3;UseUTF16Encoding=True;";
    }
}
