using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Invoice.Forms;
using Invoice.Repositories;

namespace Invoice
{
    static class Program
    {
        private const string AppUuid = "caa556c2 - 952b-11eb-a8b3-0242ac130003";

        private static readonly DataBaseInitializerRepository DataBaseInitializerRepository = new DataBaseInitializerRepository();
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

                if (isDataBaseInitialize)
                {
                    Application.Run(new ListForm());
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
    }
}
