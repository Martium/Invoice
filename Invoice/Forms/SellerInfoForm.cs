using System.Windows.Forms;
using Invoice.Constants;
using Invoice.Models;
using Invoice.Repositories;
using Invoice.Service;

namespace Invoice.Forms
{
    public partial class SellerInfoForm : Form
    {
        private readonly InvoiceRepository _invoiceRepository = new InvoiceRepository();

        public SellerInfoForm()
        {
            InitializeComponent();
            SetTextBoxLengths();
            LoadSellerInfo();
        }

        private void SaveButton_Click(object sender, System.EventArgs e)
        {
            MessageDialogService messageDialogService = new MessageDialogService();

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
                SellerEmailAddress = SellerEmailAddressRichTextBox.Text,
                InvoiceMaker = InvoiceMakerRichTextBox.Text
            };

            bool isIdExists = _invoiceRepository.CheckSellerIdExists();
            bool isSuccessful;

            isSuccessful = isIdExists ? _invoiceRepository.UpdateSellerInfo(sellerInfo) : _invoiceRepository.CreateNewSellerInfo(sellerInfo);

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

        private void RichTextBoxes_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl((RichTextBox)sender, true, true, true, true);
                e.SuppressKeyPress = true;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Escape))
            {
                this.Close();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
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

        private void LoadSellerInfo()
        {
            bool isIdExists = _invoiceRepository.CheckSellerIdExists();

            if (isIdExists)
            {
                SellerInfoModel sellerInfo = _invoiceRepository.GetSellerInfo();

                SerialNumberRichTextBox.Text = sellerInfo.SerialNumber;

                SellerNameRichTextBox.Text = sellerInfo.SellerName;
                SellerFirmCodeRichTextBox.Text = sellerInfo.SellerFirmCode;
                SellerPvmCodeRichTextBox.Text = sellerInfo.SellerPvmCode;
                SellerAddressRichTextBox.Text = sellerInfo.SellerAddress;
                SellerPhoneNumberRichTextBox.Text = sellerInfo.SellerPhoneNumber;
                SellerBankRichTextBox.Text = sellerInfo.SellerBank;
                SellerBankAccountNumberRichTextBox.Text = sellerInfo.SellerBankAccountNumber;
                SellerEmailAddressRichTextBox.Text = sellerInfo.SellerEmailAddress;

                InvoiceMakerRichTextBox.Text = sellerInfo.InvoiceMaker;

            }
        }
    }
}
