using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Repositories
{
    public class PdfFolderInitializer
    {
        public void InitializePdfFolderIfNotExist()
        {
            if (File.Exists(AppConfiguration.PdfFolder))
            {
#if DEBUG

#else
                return;
#endif
            }

            if (!Directory.Exists(AppConfiguration.PdfFolder))
            {
                Directory.CreateDirectory(AppConfiguration.PdfFolder);
            }
            else
            {
                DeleteLeftoverFilesAndFolders();
            }
        }

        private void DeleteLeftoverFilesAndFolders()
        {
            var directory = new DirectoryInfo(AppConfiguration.PdfFolder);

            foreach (FileInfo file in directory.GetFiles())
            {
                file.Delete();
            }

            foreach (DirectoryInfo subdirectory in directory.GetDirectories())
            {
                subdirectory.Delete(true);
            }
        }
    }

   
}
