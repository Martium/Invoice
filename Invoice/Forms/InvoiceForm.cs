using System;
using System.Windows.Forms;
using Invoice.Constants;
using Invoice.Enums;
using Invoice.Repositories;

namespace Invoice.Forms
{
    public partial class InvoiceForm : Form
    {
        private readonly InvoiceRepository _invoiceRepository;

        private readonly FillDefaultSellerInfo _fillDefaultSellerInfo = new FillDefaultSellerInfo();

        private readonly InvoiceOperations _invoiceOperations;
        private readonly int? _invoiceNumber;
        private readonly int? _invoiceNumberYearCreation;

        private const string DateFormat = "yyyy-MM-dd";

        public InvoiceForm(InvoiceOperations invoiceOperations, int? invoiceNumber = null, int? invoiceNumberYearCreation = null)
        {
            _invoiceRepository = new InvoiceRepository();

            _invoiceOperations = invoiceOperations;
            _invoiceNumber = invoiceNumber;
            _invoiceNumberYearCreation = invoiceNumberYearCreation;

            ResolveFormOperationDesign();

            InitializeComponent();

            SetTextBoxLengths();

            FillDefaultSellerInfoForNewInvoice();

            SetControlInitialState();
        }

        private void InvoiceForm_Load(object sender, EventArgs e)
        {
            ResolveInvoiceNumberText();
        }

        private void ResolveFormOperationDesign()
        {
            switch (_invoiceOperations)
            {
                case InvoiceOperations.Create:
                    this.Text = "Naujos Sąskaita faktūra";
                    //this.Icon = Properties.Resources.CreateIcon;
                    break;
                case InvoiceOperations.Edit:
                    this.Text = "Esamos Sąskaitos faktūros keitimas";
                    //this.Icon = Properties.Resources.EditIcon;
                    break;
                case InvoiceOperations.Copy:
                    this.Text = "Esamos Sąskaitos faktūros kopijavimas (sukurti naują)";
                    //this.Icon = Properties.Resources.CopyIcon;
                    break;
                default:
                    throw new Exception($"Paslaugų valdymo formoje gauta nežinoma opercija: '{_invoiceOperations}'");
            }
        }

        private void SetControlInitialState()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            InvoiceNumberRichTextBox.ReadOnly = true;
        }

        private void FillDefaultSellerInfoForNewInvoice()
        {
            if (_invoiceOperations == InvoiceOperations.Create)
            {
                SerialNumberRichTextBox.Text = _fillDefaultSellerInfo.SerialNumber;

                SellerNameRichTextBox.Text = _fillDefaultSellerInfo.SellerName;
                SellerFirmCodeRichTextBox.Text = _fillDefaultSellerInfo.SellerFirmCode;
                SellerPvmCodeRichTextBox.Text = _fillDefaultSellerInfo.SellerPvmCode;
                SellerAddressRichTextBox.Text = _fillDefaultSellerInfo.SellerAddress;
                SellerPhoneNumberRichTextBox.Text = _fillDefaultSellerInfo.SellerPhoneNumber;
                SellerBankRichTextBox.Text = _fillDefaultSellerInfo.SellerBank;
                SellerBankAccountNumberRichTextBox.Text = _fillDefaultSellerInfo.SellerBankAccount;
                SellerEmailAddressRichTextBox.Text = _fillDefaultSellerInfo.SellerEmailAddress;
            }
        }

        private void ResolveInvoiceNumberText()
        {
            if (_invoiceOperations == InvoiceOperations.Edit)
            {
               InvoiceNumberRichTextBox.Text = _invoiceNumber.Value.ToString();
            }
            else
            { 
                InvoiceNumberRichTextBox.Text = _invoiceRepository.GetNextInvoiceNumber().ToString();

                InvoiceDateRichTextBox.Text = DateTime.Now.ToString(DateFormat);
            }
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

            BuyerNameRichTextBox.MaxLength = FormSettings.TextBoxLengths.BuyerName;
            BuyerFirmCodeRichTextBox.MaxLength = FormSettings.TextBoxLengths.BuyerFirmCode;
            BuyerPvmCodeRichTextBox.MaxLength = FormSettings.TextBoxLengths.BuyerPvmCode;
            BuyerAddressRichTextBox.MaxLength = FormSettings.TextBoxLengths.BuyerAddress;

            FirstProductNameRichTextBox.MaxLength = FormSettings.TextBoxLengths.FirstProductName;
            SecondProductNameRichTextBox.MaxLength = FormSettings.TextBoxLengths.SecondProductName;
            ThirdProductNameRichTextBox.MaxLength = FormSettings.TextBoxLengths.ThirdProductName;
            FourthProductNameRichTextBox.MaxLength = FormSettings.TextBoxLengths.FourthProductName;
            FifthProductNameRichTextBox.MaxLength = FormSettings.TextBoxLengths.FifthProductName;
            SixthProductNameRichTextBox.MaxLength = FormSettings.TextBoxLengths.SixthProductName;
            SeventhProductNameRichTextBox.MaxLength = FormSettings.TextBoxLengths.SeventhProductName;
            EighthProductNameRichTextBox.MaxLength = FormSettings.TextBoxLengths.EighthProductName;
            NinthProductNameRichTextBox.MaxLength = FormSettings.TextBoxLengths.NinthProductName;
            TenProductNameRichTextBox.MaxLength = FormSettings.TextBoxLengths.TenProductName;
            EleventhProductNameRichTextBox.MaxLength = FormSettings.TextBoxLengths.EleventhProductName;
            TwelfthProductNameRichTextBox.MaxLength = FormSettings.TextBoxLengths.TwelfthProductName;

            FirstProductSeesRichTextBox.MaxLength = FormSettings.TextBoxLengths.FirstProductSees;
            SecondProductSeesRichTextBox.MaxLength = FormSettings.TextBoxLengths.SecondProductSees;
            ThirdProductSeesRichTextBox.MaxLength = FormSettings.TextBoxLengths.ThirdProductSees;
            FourthProductSeesRichTextBox.MaxLength = FormSettings.TextBoxLengths.FourthProductSees;
            FifthProductSeesRichTextBox.MaxLength = FormSettings.TextBoxLengths.FifthProductSees;
            SixthProductSeesRichTextBox.MaxLength = FormSettings.TextBoxLengths.SixthProductSees;
            SeventhProductSeesRichTextBox.MaxLength = FormSettings.TextBoxLengths.SeventhProductSees;
            EighthProductSeesRichTextBox.MaxLength = FormSettings.TextBoxLengths.EighthProductSees;
            NinthProductSeesRichTextBox.MaxLength = FormSettings.TextBoxLengths.NinthProductSees;
            TenProductSeesRichTextBox.MaxLength = FormSettings.TextBoxLengths.TenProductSees;
            EleventhProductSeesRichTextBox.MaxLength = FormSettings.TextBoxLengths.EleventhProductSees;
            TwelfthProductSeesRichTextBox.MaxLength = FormSettings.TextBoxLengths.TwelfthProductSees;

            PriceInWordsRichTextBox.MaxLength = FormSettings.TextBoxLengths.PriceInWords;
            InvoiceMakerRichTextBox.MaxLength = FormSettings.TextBoxLengths.InvoiceMaker;
            InvoiceAcceptedRichTextBox.MaxLength = FormSettings.TextBoxLengths.InvoiceAccepted;
        }
    }
}
