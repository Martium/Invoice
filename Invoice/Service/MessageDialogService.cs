using System.Drawing;
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

        public DialogResult ShowPaymentStatusSaveChoiceMessage(string message)
        {
            DialogResult dialogResult = MessageBox.Show(message, "Saugojimo Pranešimas", MessageBoxButtons.OKCancel);

            return dialogResult;
        }

        public void DisplayLabelAndTextBoxError(string errorText, RichTextBox richTextBox, Label label)
        {
            richTextBox.BackColor = Color.Red;
            label.Text = errorText;
            label.Visible = true;
        }

        public void HideLabelAndTextBoxError(Label label, RichTextBox richTextBox)
        {
            label.Visible = false;
            richTextBox.BackColor = Color.White;
        }
    }
}
