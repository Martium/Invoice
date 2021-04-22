using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Invoice.Constants;
using Invoice.Enums;
using Invoice.Models;
using Invoice.Repositories;
using Invoice.Service;
using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Properties;

namespace Invoice.Forms
{
    public partial class InvoiceForm : Form
    {
        private readonly InvoiceRepository _invoiceRepository;

        private readonly MessageDialogService _messageDialogService = new MessageDialogService();

        private readonly NumberService _numberService = new NumberService();

        private readonly InvoiceOperations _invoiceOperations;
        private readonly int? _invoiceNumber;
        private readonly int? _invoiceNumberYearCreation;
        private string _paymentStatus = "Nesumokėta";

        private Bitmap _invoiceMemoryImage;

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

        private void InvoiceDateRichTextBox_TextChanged(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(InvoiceDateRichTextBox.Text))
            {
                SaveButton.Enabled = false;
            }
            else
            {
                SaveButton.Enabled = true;
            }
        }

        private void InvoiceDateRichTextBox_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(InvoiceDateRichTextBox.Text))
            {
                SaveButton.Enabled = false;
                e.Cancel = true;
                _messageDialogService.DisplayLabelAndTextBoxError(
                    $"Negali būti tuščias! pvz.: {DateTime.Now.ToString(DateFormat)}", InvoiceDateRichTextBox,
                    ErrorMassageLabel);
            }
            else if (!DateTime.TryParseExact(InvoiceDateRichTextBox.Text, DateFormat, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out _))
            {
                SaveButton.Enabled = false;
                e.Cancel = true;
                _messageDialogService.DisplayLabelAndTextBoxError(
                    $"Įveskite teisingą datą! pvz.: {DateTime.Now.ToString(DateFormat)}", InvoiceDateRichTextBox,
                    ErrorMassageLabel);
            }
            else
            {
                SaveButton.Enabled = true;
                e.Cancel = false;
                _messageDialogService.HideLabelAndTextBoxError(ErrorMassageLabel, InvoiceDateRichTextBox);
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            ChangeDoubleCommaToDot();

            var invoiceModel = GetAllInfoFromRichTextBox();

            bool isSuccess;
            string successMessage;

            if (_invoiceOperations == InvoiceOperations.Edit)
            {
                isSuccess = _invoiceRepository.UpdateExistingInvoice(_invoiceNumber.Value,
                    _invoiceNumberYearCreation.Value, invoiceModel);
                successMessage = "Sąskaita faktūra atnaujinta sekmingai";
            }
            else
            {
                isSuccess = _invoiceRepository.CreateNewInvoice(invoiceModel);
                successMessage = "Nauja Sąskaita faktūra sukurta";
            }

            if (isSuccess)
            {
                _messageDialogService.ShowInfoMessage(successMessage);
                this.Close();
            }
            else
            {
                _messageDialogService.ShowErrorMassage("nepavyko išsaugot bandykit dar kartą");
            }
        }

        private void SaveToPdf_Click(object sender, EventArgs e)
        {
            CalculateButton_Click(this, new EventArgs());

            DialogResult dialogResult = _messageDialogService.ShowChoiceMessage("Ar norite suformuoti Sąskaitos kvitą");

            if (dialogResult == DialogResult.OK)
            {
                CaptureInvoiceFormScreen();

                PdfWriter newInvoicePdfWriter =
                    new PdfWriter(
                        $"{AppConfiguration.PdfFolder}\\Saskaitos faktura ir kvitas nr.{InvoiceNumberRichTextBox.Text} {BuyerNameRichTextBox.Text}.pdf");
                PdfDocument newInvoicePdfDocument = new PdfDocument(newInvoicePdfWriter);
                Document newInvoiceDocument = new Document(newInvoicePdfDocument);

                var convertImageToByteArray = ConvertImageToByteArray(_invoiceMemoryImage);
                var newInvoiceImage =
                    new iText.Layout.Element.Image(ImageDataFactory.Create(convertImageToByteArray)).SetTextAlignment(
                        TextAlignment.CENTER);

                newInvoiceDocument.Add(newInvoiceImage);

                SaveMoneyReceiptFormToPdf(newInvoiceDocument);

                newInvoiceDocument.Close();
            }
            else
            {
                CaptureInvoiceFormScreen();

                PdfWriter newInvoicePdfWriter =
                    new PdfWriter(
                        $"{AppConfiguration.PdfFolder}\\Saskaitos faktura nr.{InvoiceNumberRichTextBox.Text} {BuyerNameRichTextBox.Text}.pdf");
                PdfDocument newInvoicePdfDocument = new PdfDocument(newInvoicePdfWriter);
                Document newInvoiceDocument = new Document(newInvoicePdfDocument);

                var convertImageToByteArray = ConvertImageToByteArray(_invoiceMemoryImage);
                var newInvoiceImage =
                    new iText.Layout.Element.Image(ImageDataFactory.Create(convertImageToByteArray)).SetTextAlignment(
                        TextAlignment.CENTER);

                newInvoiceDocument.Add(newInvoiceImage);

                newInvoiceDocument.Close();
            }

            _messageDialogService.ShowInfoMessage("Sąskaitos faktūros anketa išsaugota į pdf failą");

            SaveButton_Click(this, new EventArgs());
            this.Close();
        }

        private void CalculateButton_Click(object sender, EventArgs e)
        {
            ChangeDoubleCommaToDot();

            var invoiceQuantityAndPriceModel = GetAllProductsQuantityAndPriceNumbersFromRichTextBox();

            ProductTotalPriceRichTextBox.Text =
                _numberService.CalculateFullPrice(invoiceQuantityAndPriceModel).ToString();

            PvmPriceRichTextBox.Text =
                _numberService.CalculatePvm(ProductTotalPriceRichTextBox).ToString();

            TotalPriceWithPvmRichTextBox.Text = _numberService
                .CalculateTotalPriceWithPvm(ProductTotalPriceRichTextBox, PvmPriceRichTextBox).ToString();
        }

        private void ControlRichTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl((RichTextBox)sender, true, true, true, true);
                e.SuppressKeyPress = true;
            }
        }

        private void PrintButton_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = _messageDialogService.ShowChoiceMessage("Ar norite spausdinti kvitą (jei paspausit 'OK' spausdins kvitą jei 'Cancel' spausdins Sąskaitą faktūrą) ?");

            if (dialogResult == DialogResult.OK)
            {
                PrintMoneyReceiptForm();
            }
            else
            {
                PrintInvoiceForm();
            }
        }

        private void printDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(_invoiceMemoryImage, 0, this.PrintInvoicePanel.Location.Y);
        }

        private void PrintInvoiceForm()
        {
            CaptureInvoiceFormScreen();
            printPreviewDialog.Document = printDocument;
            printPreviewDialog.ShowDialog();
        }

        private void PrintMoneyReceiptForm()
        {
            var moneyReceiptForm = new MoneyReceiptForm();

            string allProducts = FillProductsToMoneyReceiptForm();

            MoneyReceiptModel moneyReceiptInfo = new MoneyReceiptModel()
            {
                SellerInfo = SellerNameRichTextBox.Text,
                SellerFirmCode = SellerFirmCodeRichTextBox.Text,
                SerialNumber = SerialNumberRichTextBox.Text,
                InvoiceNumber = InvoiceNumberRichTextBox.Text,
                InvoiceDate = InvoiceDateRichTextBox.Text,
                AllProducts = $@"{allProducts}",

                PriceInWords = PriceInWordsRichTextBox.Text,
                InvoiceMaker = InvoiceMakerRichTextBox.Text
            };

            moneyReceiptForm.Show();
            moneyReceiptForm.Hide();

            _invoiceMemoryImage = moneyReceiptForm.SaveMoneyReceiptForm(moneyReceiptInfo);
            printPreviewDialog.Document = printDocument;
            printPreviewDialog.ShowDialog();

            moneyReceiptForm.Close();
        }

        private void SaveMoneyReceiptFormToPdf(Document newInvoiceDocument)
        {
            var moneyReceiptForm = new MoneyReceiptForm();

            string allProducts = FillProductsToMoneyReceiptForm();

            MoneyReceiptModel moneyReceiptInfo = new MoneyReceiptModel()
            {
                SellerInfo = SellerNameRichTextBox.Text,
                SellerFirmCode = SellerFirmCodeRichTextBox.Text,
                SerialNumber = SerialNumberRichTextBox.Text,
                InvoiceNumber = InvoiceNumberRichTextBox.Text,
                InvoiceDate = InvoiceDateRichTextBox.Text,
                AllProducts = $@"{allProducts}",
                    
                PriceInWords = PriceInWordsRichTextBox.Text,
                InvoiceMaker = InvoiceMakerRichTextBox.Text
            };

            moneyReceiptForm.Show();

            var convertMoneyReceiptImageToByteArray =
                ConvertImageToByteArray(moneyReceiptForm.SaveMoneyReceiptForm(moneyReceiptInfo));

            var newMoneyReceiptImage =
                new iText.Layout.Element.Image(ImageDataFactory.Create(convertMoneyReceiptImageToByteArray)).SetTextAlignment(
                    TextAlignment.CENTER);

            newInvoiceDocument.Add(newMoneyReceiptImage);
            moneyReceiptForm.Close();
        }

        private string CheckProductsRichTextBox(RichTextBox productNameRichTextBox, RichTextBox productQuantityRichTextBox, RichTextBox productSeesRichTextBox, RichTextBox productPriceRichTextBox)
        {
            string productInfo;

            if (string.IsNullOrWhiteSpace(productNameRichTextBox.Text) || string.IsNullOrWhiteSpace(productQuantityRichTextBox.Text) || string.IsNullOrWhiteSpace(productSeesRichTextBox.Text) || string.IsNullOrWhiteSpace(productPriceRichTextBox.Text))
            {
                productInfo = string.Empty;
            }
            else
            {
                productInfo = $"{productNameRichTextBox.Text}, {productQuantityRichTextBox.Text} {productSeesRichTextBox.Text}, {productPriceRichTextBox.Text} EUR. ";
            }

            return productInfo;
        }

        private string FillProductsToMoneyReceiptForm()
        {
            string allProducts;

            string firstProductInfo = CheckProductsRichTextBox(FirstProductNameRichTextBox, FirstProductQuantityRichTextBox,
                FirstProductSeesRichTextBox, FirstProductPriceRichTextBox);
            string secondProductInfo = CheckProductsRichTextBox(SecondProductNameRichTextBox,
                SecondProductQuantityRichTextBox, SecondProductSeesRichTextBox, SecondProductPriceRichTextBox);
            string thirdProductInfo = CheckProductsRichTextBox(ThirdProductNameRichTextBox,
                ThirdProductQuantityRichTextBox, ThirdProductSeesRichTextBox, ThirdProductPriceRichTextBox);
            string forthProductInfo = CheckProductsRichTextBox(FourthProductNameRichTextBox,
                FourthProductQuantityRichTextBox, FourthProductSeesRichTextBox, FourthProductPriceRichTextBox);

            string fifthProductInfo = CheckProductsRichTextBox(FifthProductNameRichTextBox,
                FifthProductQuantityRichTextBox, FifthProductSeesRichTextBox, FifthProductPriceRichTextBox);
            string sixthProductInfo = CheckProductsRichTextBox(SixthProductNameRichTextBox,
                SixthProductQuantityRichTextBox, SixthProductSeesRichTextBox, SixthProductPriceRichTextBox);
            string seventhProductInfo = CheckProductsRichTextBox(SeventhProductNameRichTextBox,
                SeventhProductQuantityRichTextBox, SeventhProductSeesRichTextBox, SeventhProductPriceRichTextBox);
            string eighthProductInfo = CheckProductsRichTextBox(EighthProductNameRichTextBox,
                EighthProductQuantityRichTextBox, EighthProductSeesRichTextBox, EighthProductPriceRichTextBox);

            string ninthProductInfo = CheckProductsRichTextBox(NinthProductNameRichTextBox,
                NinthProductQuantityRichTextBox, NinthProductSeesRichTextBox, NinthProductPriceRichTextBox);
            string tenProductInfo = CheckProductsRichTextBox(TenProductNameRichTextBox, TenProductQuantityRichTextBox,
                TenProductSeesRichTextBox, TenProductPriceRichTextBox);
            string eleventhProductInfo = CheckProductsRichTextBox(EleventhProductNameRichTextBox,
                EleventhProductQuantityRichTextBox, EleventhProductSeesRichTextBox, EleventhProductPriceRichTextBox);
            string twelfthProductName = CheckProductsRichTextBox(TwelfthProductNameRichTextBox,
                TwelfthProductQuantityRichTextBox, TwelfthProductSeesRichTextBox, TwelfthProductPriceRichTextBox);



            allProducts = $@"{firstProductInfo} {secondProductInfo} {thirdProductInfo} 
{forthProductInfo}{fifthProductInfo} {sixthProductInfo} 
{seventhProductInfo} {eighthProductInfo} {ninthProductInfo} 
{tenProductInfo} {eleventhProductInfo} {twelfthProductName}";

            return allProducts;
        }

        private InvoiceModel GetAllInfoFromRichTextBox()
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

                FirstProductQuantity = _numberService.ParseToDoubleOrNull(FirstProductQuantityRichTextBox),
                SecondProductQuantity = _numberService.ParseToDoubleOrNull(SecondProductQuantityRichTextBox),
                ThirdProductQuantity = _numberService.ParseToDoubleOrNull(ThirdProductQuantityRichTextBox),
                FourthProductQuantity = _numberService.ParseToDoubleOrNull(FourthProductQuantityRichTextBox),
                FifthProductQuantity = _numberService.ParseToDoubleOrNull(FifthProductQuantityRichTextBox),
                SixthProductQuantity = _numberService.ParseToDoubleOrNull(SixthProductQuantityRichTextBox),
                SeventhProductQuantity = _numberService.ParseToDoubleOrNull(SeventhProductQuantityRichTextBox),
                EighthProductQuantity = _numberService.ParseToDoubleOrNull(EighthProductQuantityRichTextBox),
                NinthProductQuantity = _numberService.ParseToDoubleOrNull(NinthProductQuantityRichTextBox),
                TenProductQuantity = _numberService.ParseToDoubleOrNull(TenProductQuantityRichTextBox),
                EleventhProductQuantity = _numberService.ParseToDoubleOrNull(EleventhProductQuantityRichTextBox),
                TwelfthProductQuantity = _numberService.ParseToDoubleOrNull(TwelfthProductQuantityRichTextBox),

                FirstProductPrice = _numberService.ParseToDoubleOrNull(FirstProductPriceRichTextBox),
                SecondProductPrice = _numberService.ParseToDoubleOrNull(SecondProductPriceRichTextBox),
                ThirdProductPrice = _numberService.ParseToDoubleOrNull(ThirdProductPriceRichTextBox),
                FourthProductPrice = _numberService.ParseToDoubleOrNull(FourthProductPriceRichTextBox),
                FifthProductPrice = _numberService.ParseToDoubleOrNull(FifthProductPriceRichTextBox),
                SixthProductPrice = _numberService.ParseToDoubleOrNull(SixthProductPriceRichTextBox),
                SeventhProductPrice = _numberService.ParseToDoubleOrNull(SeventhProductPriceRichTextBox),
                EighthProductPrice = _numberService.ParseToDoubleOrNull(EighthProductPriceRichTextBox),
                NinthProductPrice = _numberService.ParseToDoubleOrNull(NinthProductPriceRichTextBox),
                TenProductPrice = _numberService.ParseToDoubleOrNull(TenProductPriceRichTextBox),
                EleventhProductPrice = _numberService.ParseToDoubleOrNull(EleventhProductPriceRichTextBox),
                TwelfthProductPrice = _numberService.ParseToDoubleOrNull(TwelfthProductPriceRichTextBox),

                PriceInWords = PriceInWordsRichTextBox.Text,
                InvoiceMaker = InvoiceMakerRichTextBox.Text,
                InvoiceAccepted = InvoiceAcceptedRichTextBox.Text,

                PaymentStatus = _paymentStatus
            };
            return invoiceModel;
        }

        private InvoiceQuantityAndPriceModel GetAllProductsQuantityAndPriceNumbersFromRichTextBox()
        {
            var invoiceQuantityAndPriceModel = new InvoiceQuantityAndPriceModel()
            {
                FirstProductQuantity = _numberService.ParseToDoubleOrZero(FirstProductQuantityRichTextBox),
                SecondProductQuantity = _numberService.ParseToDoubleOrZero(SecondProductQuantityRichTextBox),
                ThirdProductQuantity = _numberService.ParseToDoubleOrZero(ThirdProductQuantityRichTextBox),
                FourthProductQuantity = _numberService.ParseToDoubleOrZero(FourthProductQuantityRichTextBox),
                FifthProductQuantity = _numberService.ParseToDoubleOrZero(FifthProductQuantityRichTextBox),
                SixthProductQuantity = _numberService.ParseToDoubleOrZero(SixthProductQuantityRichTextBox),
                SeventhProductQuantity = _numberService.ParseToDoubleOrZero(SeventhProductQuantityRichTextBox),
                EighthProductQuantity = _numberService.ParseToDoubleOrZero(EighthProductQuantityRichTextBox),
                NinthProductQuantity = _numberService.ParseToDoubleOrZero(NinthProductQuantityRichTextBox),
                TenProductQuantity = _numberService.ParseToDoubleOrZero(TenProductQuantityRichTextBox),
                EleventhProductQuantity = _numberService.ParseToDoubleOrZero(EleventhProductQuantityRichTextBox),
                TwelfthProductQuantity = _numberService.ParseToDoubleOrZero(TwelfthProductQuantityRichTextBox),

                FirstProductPrice = _numberService.ParseToDoubleOrZero(FirstProductPriceRichTextBox),
                SecondProductPrice = _numberService.ParseToDoubleOrZero(SecondProductPriceRichTextBox),
                ThirdProductPrice = _numberService.ParseToDoubleOrZero(ThirdProductPriceRichTextBox),
                FourthProductPrice = _numberService.ParseToDoubleOrZero(FourthProductPriceRichTextBox),
                FifthProductPrice = _numberService.ParseToDoubleOrZero(FifthProductPriceRichTextBox),
                SixthProductPrice = _numberService.ParseToDoubleOrZero(SixthProductPriceRichTextBox),
                SeventhProductPrice = _numberService.ParseToDoubleOrZero(SeventhProductPriceRichTextBox),
                EighthProductPrice = _numberService.ParseToDoubleOrZero(EighthProductPriceRichTextBox),
                NinthProductPrice = _numberService.ParseToDoubleOrZero(NinthProductPriceRichTextBox),
                TenProductPrice = _numberService.ParseToDoubleOrZero(TenProductPriceRichTextBox),
                EleventhProductPrice = _numberService.ParseToDoubleOrZero(EleventhProductPriceRichTextBox),
                TwelfthProductPrice = _numberService.ParseToDoubleOrZero(TwelfthProductPriceRichTextBox)
            };
            return invoiceQuantityAndPriceModel;
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
                    this.Text = "Esamos Sąskaitos faktūros keitimas (Peržiūrėti)";
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
            (printPreviewDialog as Form).WindowState = FormWindowState.Maximized;
        }

        private void FillDefaultSellerInfoForNewInvoice()
        {
            if (_invoiceOperations == InvoiceOperations.Create)
            {
                SellerInfoModel sellerInfo = _invoiceRepository.GetSellerInfo();

                if (sellerInfo != null)
                {
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
                    _numberService.ToStringDoubleOrEmpty(invoiceModel, "FirstProductQuantity");
                SecondProductQuantityRichTextBox.Text =
                    _numberService.ToStringDoubleOrEmpty(invoiceModel, "SecondProductQuantity");
                ThirdProductQuantityRichTextBox.Text =
                    _numberService.ToStringDoubleOrEmpty(invoiceModel, "ThirdProductQuantity");
                FourthProductQuantityRichTextBox.Text =
                    _numberService.ToStringDoubleOrEmpty(invoiceModel, "FourthProductQuantity");
                FifthProductQuantityRichTextBox.Text =
                    _numberService.ToStringDoubleOrEmpty(invoiceModel, "FifthProductQuantity");
                SixthProductQuantityRichTextBox.Text =
                    _numberService.ToStringDoubleOrEmpty(invoiceModel, "SixthProductQuantity");
                SeventhProductQuantityRichTextBox.Text =
                    _numberService.ToStringDoubleOrEmpty(invoiceModel, "SeventhProductQuantity");
                EighthProductQuantityRichTextBox.Text =
                    _numberService.ToStringDoubleOrEmpty(invoiceModel, "EighthProductQuantity");
                NinthProductQuantityRichTextBox.Text =
                    _numberService.ToStringDoubleOrEmpty(invoiceModel, "NinthProductQuantity");
                TenProductQuantityRichTextBox.Text =
                    _numberService.ToStringDoubleOrEmpty(invoiceModel, "TenProductQuantity");
                EleventhProductQuantityRichTextBox.Text =
                    _numberService.ToStringDoubleOrEmpty(invoiceModel, "EleventhProductQuantity");
                TwelfthProductQuantityRichTextBox.Text =
                    _numberService.ToStringDoubleOrEmpty(invoiceModel, "TwelfthProductQuantity");

                FirstProductPriceRichTextBox.Text =
                    _numberService.ToStringDoubleOrEmpty(invoiceModel, "FirstProductPrice");
                SecondProductPriceRichTextBox.Text =
                    _numberService.ToStringDoubleOrEmpty(invoiceModel, "SecondProductPrice");
                ThirdProductPriceRichTextBox.Text =
                    _numberService.ToStringDoubleOrEmpty(invoiceModel, "ThirdProductPrice");
                FourthProductPriceRichTextBox.Text =
                    _numberService.ToStringDoubleOrEmpty(invoiceModel, "FourthProductPrice");
                FifthProductPriceRichTextBox.Text =
                    _numberService.ToStringDoubleOrEmpty(invoiceModel, "FifthProductPrice");
                SixthProductPriceRichTextBox.Text =
                    _numberService.ToStringDoubleOrEmpty(invoiceModel, "SixthProductPrice");
                SeventhProductPriceRichTextBox.Text =
                    _numberService.ToStringDoubleOrEmpty(invoiceModel, "SeventhProductPrice");
                EighthProductPriceRichTextBox.Text =
                    _numberService.ToStringDoubleOrEmpty(invoiceModel, "EighthProductPrice");
                NinthProductPriceRichTextBox.Text =
                    _numberService.ToStringDoubleOrEmpty(invoiceModel, "NinthProductPrice");
                TenProductPriceRichTextBox.Text = _numberService.ToStringDoubleOrEmpty(invoiceModel, "TenProductPrice");
                EleventhProductPriceRichTextBox.Text =
                    _numberService.ToStringDoubleOrEmpty(invoiceModel, "EleventhProductPrice");
                TwelfthProductPriceRichTextBox.Text =
                    _numberService.ToStringDoubleOrEmpty(invoiceModel, "TwelfthProductPrice");

                PriceInWordsRichTextBox.Text = invoiceModel.PriceInWords;
                InvoiceMakerRichTextBox.Text = invoiceModel.InvoiceMaker;
                InvoiceAcceptedRichTextBox.Text = invoiceModel.InvoiceAccepted;

                if (invoiceModel.PaymentStatus == "Atsiskaityta" && _invoiceOperations == InvoiceOperations.Edit)
                {
                    this.BackColor = Color.Chartreuse;
                    _paymentStatus = "Atsiskaityta";

                }
                else if (_invoiceOperations == InvoiceOperations.Edit)
                {
                    this.BackColor = Color.Red;
                }

                CalculateButton_Click(this, new EventArgs());
            }
        }

        private void ChangeDoubleCommaToDot()
        {
            FirstProductQuantityRichTextBox.Text = _numberService.ChangeCommaToDot(FirstProductQuantityRichTextBox);
            SecondProductQuantityRichTextBox.Text = _numberService.ChangeCommaToDot(SecondProductQuantityRichTextBox);
            ThirdProductQuantityRichTextBox.Text = _numberService.ChangeCommaToDot(ThirdProductQuantityRichTextBox);
            FourthProductQuantityRichTextBox.Text = _numberService.ChangeCommaToDot(FourthProductQuantityRichTextBox);
            FifthProductQuantityRichTextBox.Text = _numberService.ChangeCommaToDot(FifthProductQuantityRichTextBox);
            SixthProductQuantityRichTextBox.Text = _numberService.ChangeCommaToDot(SixthProductQuantityRichTextBox);
            SeventhProductQuantityRichTextBox.Text = _numberService.ChangeCommaToDot(SeventhProductQuantityRichTextBox);
            EighthProductQuantityRichTextBox.Text = _numberService.ChangeCommaToDot(EighthProductQuantityRichTextBox);
            NinthProductQuantityRichTextBox.Text = _numberService.ChangeCommaToDot(NinthProductQuantityRichTextBox);
            TenProductQuantityRichTextBox.Text = _numberService.ChangeCommaToDot(TenProductQuantityRichTextBox);
            EleventhProductQuantityRichTextBox.Text =
                _numberService.ChangeCommaToDot(EleventhProductQuantityRichTextBox);
            TwelfthProductQuantityRichTextBox.Text = _numberService.ChangeCommaToDot(TwelfthProductQuantityRichTextBox);

            FirstProductPriceRichTextBox.Text = _numberService.ChangeCommaToDot(FirstProductPriceRichTextBox);
            SecondProductPriceRichTextBox.Text = _numberService.ChangeCommaToDot(SecondProductPriceRichTextBox);
            ThirdProductPriceRichTextBox.Text = _numberService.ChangeCommaToDot(ThirdProductPriceRichTextBox);
            FourthProductPriceRichTextBox.Text = _numberService.ChangeCommaToDot(FourthProductPriceRichTextBox);
            FifthProductPriceRichTextBox.Text = _numberService.ChangeCommaToDot(FifthProductPriceRichTextBox);
            SixthProductPriceRichTextBox.Text = _numberService.ChangeCommaToDot(SixthProductPriceRichTextBox);
            SeventhProductPriceRichTextBox.Text = _numberService.ChangeCommaToDot(SeventhProductPriceRichTextBox);
            EighthProductPriceRichTextBox.Text = _numberService.ChangeCommaToDot(EighthProductPriceRichTextBox);
            NinthProductPriceRichTextBox.Text = _numberService.ChangeCommaToDot(NinthProductPriceRichTextBox);
            TenProductPriceRichTextBox.Text = _numberService.ChangeCommaToDot(TenProductPriceRichTextBox);
            EleventhProductPriceRichTextBox.Text = _numberService.ChangeCommaToDot(EleventhProductPriceRichTextBox);
            TwelfthProductPriceRichTextBox.Text = _numberService.ChangeCommaToDot(TwelfthProductPriceRichTextBox);

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

        private void CaptureInvoiceFormScreen()
        {
            _invoiceMemoryImage = new Bitmap(PrintInvoicePanel.Width, PrintInvoicePanel.Height);

            PrintInvoicePanel.DrawToBitmap(
                _invoiceMemoryImage,
                new Rectangle(0, 0, PrintInvoicePanel.Width, PrintInvoicePanel.Height));
        }

        private static byte[] ConvertImageToByteArray(System.Drawing.Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[]) converter.ConvertTo(img, typeof(byte[]));
        }
    }
}
