using System.Windows.Forms;

namespace Invoice.Service
{
    public class MessageDialogService
    {
        public void ShowInfoMessage(string message)
        {
            MessageBox.Show(message, "Info Pranešimas", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void ShowErrorMassage(string message)
        {
            MessageBox.Show(message, "Klaidos pranešimas", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
