using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Invoice.Forms;
using Invoice.Repositories;

namespace Invoice
{
    class Program
    {
        private const string AppUuid = "caa556c2 - 952b-11eb-a8b3-0242ac130003";

        private static readonly DataBaseInitializerRepository DataBaseInitializerRepository = new DataBaseInitializerRepository();
        private static readonly PdfFolderInitializer PdfFolderInitializer = new PdfFolderInitializer();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (Mutex mutex = new Mutex(false, "Global\\" + AppUuid))
            {
                if (!mutex.WaitOne(0, false))
                {
                    MessageBox.Show(@"Vienu metu galima paleisti tik vieną 'Invoice' aplikacija!");
                    return;
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                bool isDataBaseInitialize = InitializeDatabase();
                bool isPdfFolderInitialize = InitializePdfFolder();

                if (isDataBaseInitialize && isPdfFolderInitialize)
                {
                    Application.Run(new LoginForm());

                    LoginRepository loginRepository = new LoginRepository();
                    string isPasswordCorrect = loginRepository.GetIsPasswordCorrect().First().ToLower();

                    if (isPasswordCorrect == "true")
                    {
                        loginRepository.ChangeIsPasswordCorrect(false);
                        Application.Run(new ListForm());
                    }
                }
            }
        }

        private static bool InitializeDatabase()
        {
            bool success = true;

            try
            {
                DataBaseInitializerRepository.InitializeDatabaseIfNotExist();
            }
            catch (Exception exception)
            {
                success = false;

                MessageBox.Show(exception.Message, "Klaidos pranešimas", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return success;
        }

        private static bool InitializePdfFolder()
        {
            bool success = true;

            try
            {
                PdfFolderInitializer.InitializePdfFolderIfNotExist();
            }
            catch (Exception exception)
            {
                success = false;

                MessageBox.Show(exception.Message, "Klaidos pranešimas", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return success;
        }
    }
}
