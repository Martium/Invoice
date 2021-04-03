using System.Windows.Forms;

namespace Invoice.Service
{
    public class MessageDialogService
    {
        public static void ShowInfoMessage(string message)
        {
            MessageBox.Show(message, "Info Pranešimas", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void ShowErrorMassage(string message)
        {
            MessageBox.Show(message, "Klaidos pranešimas", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
