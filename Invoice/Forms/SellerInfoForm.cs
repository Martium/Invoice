using System.Windows.Forms;
using Invoice.Constants;
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
            SetTextBoxLengths();
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

            bool isIdExists = invoiceRepository.CheckSellerIdExists();
            bool isSuccessful;

            if (isIdExists)
            {
                isSuccessful = invoiceRepository.UodateSellerInfo(sellerInfo);
            }
            else
            { 
                isSuccessful = invoiceRepository.CreateNewSellerInfo(sellerInfo);
            }

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

        private void SetTextBoxLengths()
        {
            SerialNumberRichTextBox.MaxLength = FormSettings.TextBoxLengths.SerialNumber;

            SellerNameRichTextBox.MaxLength = FormSettings.TextBoxLengths.SellerName;
            SellerFirmCodeRichTextBox.MaxLength = FormSettings.TextBoxLengths.SellerFirmCode;
            SellerPvmCodeRichTextBox.MaxLength = FormSettings.TextBoxLengths.SellerPvmCode;
            SellerAddressRichTextBox.MaxLength = FormSettings.TextBoxLengths.SellerAddress;
            SellerPhoneNumberRichTextBox.MaxLength = FormSettings.TextBoxLengths.SellerPhoneNumber;
            SellerBankRichTextBox.MaxLength = FormSettings.TextBoxLengths.SellerBank;
            SellerBankAccountNumberRichTextBox.MaxLength = FormSettings.TextBoxLengths.SellerBankAccountNumber;
            SellerEmailAddressRichTextBox.MaxLength = FormSettings.TextBoxLengths.SellerEmailAddress;

            InvoiceMakerRichTextBox.MaxLength = FormSettings.TextBoxLengths.InvoiceMaker;
        }
    }
}
