using System.Windows.Forms;
using Invoice.Models;
using Invoice.Repositories;
using Invoice.Service;

namespace Invoice.Forms
{
    public partial class SellerInfoForm : Form
    {
        public SellerInfoForm()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, System.EventArgs e)
        {
            MessageDialogService messageDialogService = new MessageDialogService();
            InvoiceRepository invoiceRepository = new InvoiceRepository();

            SellerInfoModel sellerInfo = new SellerInfoModel()
            {
                SerialNumber = SerialNumberRichTextBox.Text,
                SellerName = SellerNameRichTextBox.Text,
                SellerFirmCode = SellerFirmCodeRichTextBox.Text,
                SellerPvmCode = SellerPvmCodeRichTextBox.Text,
                SellerAddress = SellerAddressRichTextBox.Text,
                SellerPhoneNumber = SellerPhoneNumberRichTextBox.Text,
                SellerBank = SellerBankRichTextBox.Text,
                SellerBankAccountNumber = SellerBankAccountNumberRichTextBox.Text,
                SellerEmailAddress = SellerEmailAddressRichTextBox.Text
            };

            bool isSuccessful = invoiceRepository.CreateNewSellerInfo(sellerInfo);

            if (isSuccessful)
            {
                messageDialogService.ShowInfoMessage("Sekmingai išsaugota");
            }
            else
            {
                messageDialogService.ShowErrorMassage("neišsaugota bandykit dar kartą");
            }

            this.Close();
        }
    }
}
