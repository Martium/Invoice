using System;
using System.Globalization;
using System.Windows.Forms;
using Invoice.Constants;
using Invoice.Enums;
using Invoice.Models;
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

        public InvoiceForm(InvoiceOperations invoiceOperations, int? invoiceNumber = null,
            int? invoiceNumberYearCreation = null)
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
            LoadFormDataForEditOrCopy();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            var invoiceModel = new InvoiceModel
            {
                InvoiceDate =
                    DateTime.ParseExact(InvoiceDateRichTextBox.Text, DateFormat, CultureInfo.InvariantCulture),
                SerialNumber = SerialNumberRichTextBox.Text,

                SellerName = SellerNameRichTextBox.Text,
                SellerFirmCode = SellerFirmCodeRichTextBox.Text,
                SellerPvmCode = SellerPvmCodeRichTextBox.Text,
                SellerAddress = SellerAddressRichTextBox.Text,
                SellerPhoneNumber = SellerPhoneNumberRichTextBox.Text,
                SellerBank = SellerBankRichTextBox.Text,
                SellerBankAccountNumber = SellerBankAccountNumberRichTextBox.Text,
                SellerEmailAddress = SellerEmailAddressRichTextBox.Text,

                BuyerName = BuyerNameRichTextBox.Text,
                BuyerFirmCode = BuyerFirmCodeRichTextBox.Text,
                BuyerPvmCode = BuyerPvmCodeRichTextBox.Text,
                BuyerAddress = BuyerAddressRichTextBox.Text,

                FirstProductName = FirstProductNameRichTextBox.Text,
                SecondProductName = SecondProductNameRichTextBox.Text,
                ThirdProductName = ThirdProductNameRichTextBox.Text,
                FourthProductName = FourthProductNameRichTextBox.Text,
                FifthProductName = FifthProductNameRichTextBox.Text,
                SixthProductName = SixthProductNameRichTextBox.Text,
                SeventhProductName = SeventhProductNameRichTextBox.Text,
                EighthProductName = EighthProductNameRichTextBox.Text,
                NinthProductName = NinthProductNameRichTextBox.Text,
                TenProductName = TenProductNameRichTextBox.Text,
                EleventhProductName = EleventhProductNameRichTextBox.Text,
                TwelfthProductName = TwelfthProductNameRichTextBox.Text,

                FirstProductSees = FirstProductSeesRichTextBox.Text,
                SecondProductSees = SecondProductSeesRichTextBox.Text,
                ThirdProductSees = ThirdProductSeesRichTextBox.Text,
                ForthProductSees = FourthProductSeesRichTextBox.Text,
                FifthProductSees = FifthProductSeesRichTextBox.Text,
                SixthProductSees = SixthProductSeesRichTextBox.Text,
                SeventhProductSees = SeventhProductSeesRichTextBox.Text,
                EighthProductSees = EighthProductSeesRichTextBox.Text,
                NinthProductSees = NinthProductSeesRichTextBox.Text,
                TenProductSees = TenProductSeesRichTextBox.Text,
                EleventhProductSees = EleventhProductSeesRichTextBox.Text,
                TwelfthProductSees = TwelfthProductSeesRichTextBox.Text,

                FirstProductQuantity = double.Parse(FirstProductQuantityRichTextBox.Text, CultureInfo.InvariantCulture),
                SecondProductQuantity = double.Parse(SecondProductQuantityRichTextBox.Text, CultureInfo.InvariantCulture),
                ThirdProductQuantity = double.Parse(ThirdProductQuantityRichTextBox.Text, CultureInfo.InvariantCulture),
                FourthProductQuantity = double.Parse(FourthProductQuantityRichTextBox.Text, CultureInfo.InvariantCulture),
                FifthProductQuantity = double.Parse(FifthProductQuantityRichTextBox.Text, CultureInfo.InvariantCulture),
                SixthProductQuantity = double.Parse(SixthProductQuantityRichTextBox.Text, CultureInfo.InvariantCulture),
                SeventhProductQuantity = double.Parse(SeventhProductQuantityRichTextBox.Text, CultureInfo.InvariantCulture),
                EighthProductQuantity = double.Parse(EighthProductQuantityRichTextBox.Text, CultureInfo.InvariantCulture),
                NinthProductQuantity = double.Parse(NinthProductQuantityRichTextBox.Text, CultureInfo.InvariantCulture),
                TenProductQuantity = double.Parse(TenProductQuantityRichTextBox.Text, CultureInfo.InvariantCulture),
                EleventhProductQuantity = double.Parse(EleventhProductQuantityRichTextBox.Text, CultureInfo.InvariantCulture),
                TwelfthProductQuantity = double.Parse(TwelfthProductQuantityRichTextBox.Text, CultureInfo.InvariantCulture),

                FirstProductPrice = double.Parse(FirstProductPriceRichTextBox.Text, CultureInfo.InvariantCulture),
                SecondProductPrice = double.Parse(SecondProductPriceRichTextBox.Text, CultureInfo.InvariantCulture),
                ThirdProductPrice = double.Parse(ThirdProductPriceRichTextBox.Text, CultureInfo.InvariantCulture),
                FourthProductPrice = double.Parse(FourthProductPriceRichTextBox.Text, CultureInfo.InvariantCulture),
                FifthProductPrice = double.Parse(FifthProductPriceRichTextBox.Text, CultureInfo.InvariantCulture),
                SixthProductPrice = double.Parse(SixthProductPriceRichTextBox.Text, CultureInfo.InvariantCulture),
                SeventhProductPrice = double.Parse(SeventhProductPriceRichTextBox.Text, CultureInfo.InvariantCulture),
                EighthProductPrice = double.Parse(EighthProductPriceRichTextBox.Text, CultureInfo.InvariantCulture),
                NinthProductPrice = double.Parse(NinthProductPriceRichTextBox.Text, CultureInfo.InvariantCulture),
                TenProductPrice = double.Parse(TenProductPriceRichTextBox.Text, CultureInfo.InvariantCulture),
                EleventhProductPrice = double.Parse(EleventhProductPriceRichTextBox.Text, CultureInfo.InvariantCulture),
                TwelfthProductPrice = double.Parse(TwelfthProductPriceRichTextBox.Text, CultureInfo.InvariantCulture),

                PriceInWords = PriceInWordsRichTextBox.Text,
                InvoiceMaker = InvoiceMakerRichTextBox.Text,
                InvoiceAccepted = InvoiceAcceptedRichTextBox.Text
            };

            bool isSuccess;
            string successMessage;

            if (_invoiceOperations == InvoiceOperations.Edit)
            {
                
            }
            else
            {
                isSuccess = _invoiceRepository.CreateNewInvoice(invoiceModel);
                successMessage = "Nauja Sąskaita faktūra sukurta";
            }
        }

        private void ResolveFormOperationDesign()
        {
            switch (_invoiceOperations)
            {
                case InvoiceOperations.Create:
                    this.Text = "Nauja Sąskaita faktūra";
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

        private void LoadFormDataForEditOrCopy()
        {
            if (_invoiceOperations == InvoiceOperations.Edit || _invoiceOperations == InvoiceOperations.Copy)
            {
                InvoiceModel invoiceModel =
                    _invoiceRepository.GetExistingInvoice(_invoiceNumber.Value, _invoiceNumberYearCreation.Value);

                SerialNumberRichTextBox.Text = invoiceModel.SerialNumber;
                InvoiceDateRichTextBox.Text = invoiceModel.InvoiceDate.ToString(DateFormat);

                SellerNameRichTextBox.Text = invoiceModel.SellerName;
                SellerFirmCodeRichTextBox.Text = invoiceModel.SellerFirmCode;
                SellerPvmCodeRichTextBox.Text = invoiceModel.SellerPvmCode;
                SellerAddressRichTextBox.Text = invoiceModel.SellerAddress;
                SellerPhoneNumberRichTextBox.Text = invoiceModel.SellerPhoneNumber;
                SellerBankRichTextBox.Text = invoiceModel.SellerBank;
                SellerBankAccountNumberRichTextBox.Text = invoiceModel.SellerBankAccountNumber;
                SellerEmailAddressRichTextBox.Text = invoiceModel.SellerEmailAddress;

                BuyerNameRichTextBox.Text = invoiceModel.BuyerName;
                BuyerFirmCodeRichTextBox.Text = invoiceModel.BuyerFirmCode;
                BuyerPvmCodeRichTextBox.Text = invoiceModel.BuyerPvmCode;
                BuyerAddressRichTextBox.Text = invoiceModel.BuyerAddress;

                FirstProductNameRichTextBox.Text = invoiceModel.FirstProductName;
                SecondProductNameRichTextBox.Text = invoiceModel.SecondProductName;
                ThirdProductNameRichTextBox.Text = invoiceModel.ThirdProductName;
                FourthProductNameRichTextBox.Text = invoiceModel.FourthProductName;
                FifthProductNameRichTextBox.Text = invoiceModel.FifthProductName;
                SixthProductNameRichTextBox.Text = invoiceModel.SixthProductName;
                SeventhProductNameRichTextBox.Text = invoiceModel.SeventhProductName;
                EighthProductNameRichTextBox.Text = invoiceModel.EighthProductName;
                NinthProductNameRichTextBox.Text = invoiceModel.NinthProductName;
                TenProductNameRichTextBox.Text = invoiceModel.TenProductName;
                EleventhProductNameRichTextBox.Text = invoiceModel.EleventhProductName;
                TwelfthProductNameRichTextBox.Text = invoiceModel.TwelfthProductName;

                FirstProductSeesRichTextBox.Text = invoiceModel.FirstProductSees;
                SecondProductSeesRichTextBox.Text = invoiceModel.SecondProductSees;
                ThirdProductSeesRichTextBox.Text = invoiceModel.ThirdProductSees;
                FourthProductSeesRichTextBox.Text = invoiceModel.ForthProductSees;
                FifthProductSeesRichTextBox.Text = invoiceModel.FifthProductSees;
                SixthProductSeesRichTextBox.Text = invoiceModel.SixthProductSees;
                SeventhProductSeesRichTextBox.Text = invoiceModel.SeventhProductSees;
                EighthProductSeesRichTextBox.Text = invoiceModel.EighthProductSees;
                NinthProductSeesRichTextBox.Text = invoiceModel.NinthProductSees;
                TenProductSeesRichTextBox.Text = invoiceModel.TenProductSees;
                EleventhProductSeesRichTextBox.Text = invoiceModel.EleventhProductSees;
                TwelfthProductSeesRichTextBox.Text = invoiceModel.TwelfthProductSees;

                FirstProductQuantityRichTextBox.Text =
                    invoiceModel.FirstProductQuantity.ToString(CultureInfo.InvariantCulture);
                SecondProductQuantityRichTextBox.Text =
                    invoiceModel.SecondProductQuantity.ToString(CultureInfo.InvariantCulture);
                ThirdProductQuantityRichTextBox.Text =
                    invoiceModel.ThirdProductQuantity.ToString(CultureInfo.InvariantCulture);
                FourthProductQuantityRichTextBox.Text =
                    invoiceModel.FourthProductQuantity.ToString(CultureInfo.InvariantCulture);
                FifthProductQuantityRichTextBox.Text =
                    invoiceModel.FifthProductQuantity.ToString(CultureInfo.InvariantCulture);
                SixthProductQuantityRichTextBox.Text =
                    invoiceModel.SixthProductQuantity.ToString(CultureInfo.InvariantCulture);
                SeventhProductQuantityRichTextBox.Text =
                    invoiceModel.SeventhProductQuantity.ToString(CultureInfo.InvariantCulture);
                EighthProductQuantityRichTextBox.Text =
                    invoiceModel.EighthProductQuantity.ToString(CultureInfo.InvariantCulture);
                NinthProductQuantityRichTextBox.Text =
                    invoiceModel.NinthProductQuantity.ToString(CultureInfo.InvariantCulture);
                TenProductQuantityRichTextBox.Text =
                    invoiceModel.TenProductQuantity.ToString(CultureInfo.InvariantCulture);
                EleventhProductQuantityRichTextBox.Text =
                    invoiceModel.EleventhProductQuantity.ToString(CultureInfo.InvariantCulture);
                TwelfthProductQuantityRichTextBox.Text =
                    invoiceModel.TwelfthProductQuantity.ToString(CultureInfo.InvariantCulture);

                FirstProductPriceRichTextBox.Text =
                    invoiceModel.FirstProductPrice.ToString(CultureInfo.InvariantCulture);
                SecondProductPriceRichTextBox.Text =
                    invoiceModel.SecondProductPrice.ToString(CultureInfo.InvariantCulture);
                ThirdProductPriceRichTextBox.Text =
                    invoiceModel.ThirdProductPrice.ToString(CultureInfo.InvariantCulture);
                FourthProductPriceRichTextBox.Text =
                    invoiceModel.FourthProductPrice.ToString(CultureInfo.InvariantCulture);
                FifthProductPriceRichTextBox.Text =
                    invoiceModel.FifthProductPrice.ToString(CultureInfo.InvariantCulture);
                SixthProductPriceRichTextBox.Text =
                    invoiceModel.SixthProductPrice.ToString(CultureInfo.InvariantCulture);
                SeventhProductPriceRichTextBox.Text =
                    invoiceModel.SeventhProductPrice.ToString(CultureInfo.InvariantCulture);
                EighthProductPriceRichTextBox.Text =
                    invoiceModel.EighthProductPrice.ToString(CultureInfo.InvariantCulture);
                NinthProductPriceRichTextBox.Text =
                    invoiceModel.NinthProductPrice.ToString(CultureInfo.InvariantCulture);
                TenProductPriceRichTextBox.Text = invoiceModel.TenProductPrice.ToString(CultureInfo.InvariantCulture);
                EleventhProductPriceRichTextBox.Text =
                    invoiceModel.EleventhProductPrice.ToString(CultureInfo.InvariantCulture);
                TwelfthProductPriceRichTextBox.Text =
                    invoiceModel.TwelfthProductPrice.ToString(CultureInfo.InvariantCulture);

                PriceInWordsRichTextBox.Text = invoiceModel.PriceInWords;
                InvoiceMakerRichTextBox.Text = invoiceModel.InvoiceMaker;
                InvoiceAcceptedRichTextBox.Text = invoiceModel.InvoiceAccepted;
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
