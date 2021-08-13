using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Invoice.Constants;
using Invoice.Enums;
using Invoice.Models;
using Invoice.Models.ProductType;
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

        private readonly ProductTypeRepository _productTypeRepository;

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
            _productTypeRepository = new ProductTypeRepository();

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
            SetCursorAtDateTextBoxEnd();
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
            CalculateButton_Click(this, new EventArgs());

            var invoiceModel = GetAllInfoFromRichTextBox();

            bool isSuccess;
            string successMessage;

            if (_invoiceOperations == InvoiceOperations.Edit && _invoiceNumber.HasValue && _invoiceNumberYearCreation.HasValue)
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
                GetAllProductTypeForNewInvoice();
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

            DialogResult dialogResult = _messageDialogService.ShowChoiceMessage("Ar norite suformuoti Sąskaita ir kvitą");

            if (dialogResult == DialogResult.OK)
            {
                SaveInvoiceToPdf();
                _messageDialogService.ShowInfoMessage("Sąskaitos faktūra išsaugota į pdf failą");
            }
            else
            {
               SaveInvoiceAndMoneyReceiptToPdf();
               _messageDialogService.ShowInfoMessage("Sąskaita faktūra ir kvitas išsaugota į pdf failą");
            }

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

        private void RichTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                RichTextBox richTextBox = sender as RichTextBox;

                if (richTextBox == null) return;

                if (richTextBox.Multiline == false)
                {
                    this.SelectNextControl((Control)sender, true, true, true, true);
                }

                SetCursorAtTextBoxStringEnd();
            }
        }

        private void ProductTypeTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
                SetCursorAtProductTypeStringEnd();
            }
        }

        private void InvoiceRichTextBoxString_TextChanged(object sender, EventArgs e)
        {
            RichTextBox richTextBox = sender as RichTextBox;

            if (richTextBox == null) return;

            if (richTextBox.SelectionStart == richTextBox.MaxLength)
            {
                _messageDialogService.ShowInfoMessage($"Pasiektas maksimalus žodžių ilgis bus išsaugota tik toks tekstas ({richTextBox.Text}) ");
            }

            if (richTextBox.Lines.Length == 3)
            {
                richTextBox.Lines = richTextBox.Lines.Take(richTextBox.Lines.Length - 1).ToArray();
                this.SelectNextControl((Control) sender, true, true, true, true);
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

        #region Helpers

        private void SaveInvoiceToPdf()
        {
            CaptureInvoiceFormScreen();

            PdfWriter newInvoicePdfWriter =
                new PdfWriter(
                    $"{AppConfiguration.PdfFolder}\\Saskaita faktura ir kvitas nr.{InvoiceNumberRichTextBox.Text} {BuyerNameRichTextBox.Text}.pdf");
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

        private void SaveInvoiceAndMoneyReceiptToPdf()
        {
            CaptureInvoiceFormScreen();

            PdfWriter newInvoicePdfWriter =
                new PdfWriter(
                    $"{AppConfiguration.PdfFolder}\\Saskaita faktura nr.{InvoiceNumberRichTextBox.Text} {BuyerNameRichTextBox.Text}.pdf");
            PdfDocument newInvoicePdfDocument = new PdfDocument(newInvoicePdfWriter);
            Document newInvoiceDocument = new Document(newInvoicePdfDocument);

            var convertImageToByteArray = ConvertImageToByteArray(_invoiceMemoryImage);
            var newInvoiceImage =
                new iText.Layout.Element.Image(ImageDataFactory.Create(convertImageToByteArray)).SetTextAlignment(
                    TextAlignment.CENTER);

            newInvoiceDocument.Add(newInvoiceImage);

            newInvoiceDocument.Close();
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
                double priceWithPvm = _numberService.CalculateProductPriceWithPvm(productPriceRichTextBox);
                productInfo = $"{productNameRichTextBox.Text}, {productQuantityRichTextBox.Text} {productSeesRichTextBox.Text}, {priceWithPvm} EUR. ";
            }

            return productInfo;
        }

        private string FillProductsToMoneyReceiptForm()
        {
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



            string allProducts = $@"{firstProductInfo} {secondProductInfo} {thirdProductInfo} 
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

                TotalPriceWithPvm = TotalPriceWithPvmRichTextBox.Text,

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
                    this.Text = @"Nauja Sąskaita faktūra";
                    break;
                case InvoiceOperations.Edit:
                    this.Text = @"Esamos Sąskaitos faktūros keitimas (Peržiūrėti)";
                    break;
                case InvoiceOperations.Copy:
                    this.Text = @"Esamos Sąskaitos faktūros kopijavimas (sukurti naują)";
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

            DisableScrollBarRichTextBoxWithMultiLine();
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
            if (_invoiceOperations == InvoiceOperations.Edit && _invoiceNumber.HasValue)
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
                GetExistingInvoiceInfo();

                GetExistingProductTypeInfo();

                CalculateButton_Click(this, new EventArgs());
            }
        }

        private void GetExistingInvoiceInfo()
        {
            if (_invoiceNumber.HasValue && _invoiceNumberYearCreation.HasValue)
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
            EleventhProductQuantityRichTextBox.Text = _numberService.ChangeCommaToDot(EleventhProductQuantityRichTextBox);
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
            InvoiceDateRichTextBox.MaxLength = FormSettings.TextBoxLengths.DateFormatLength;
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

            FirstProductQuantityRichTextBox.MaxLength = FormSettings.TextBoxLengths.MaxNumberLength;
            SecondProductQuantityRichTextBox.MaxLength = FormSettings.TextBoxLengths.MaxNumberLength;
            ThirdProductQuantityRichTextBox.MaxLength = FormSettings.TextBoxLengths.MaxNumberLength;
            FourthProductQuantityRichTextBox.MaxLength = FormSettings.TextBoxLengths.MaxNumberLength;
            FifthProductQuantityRichTextBox.MaxLength = FormSettings.TextBoxLengths.MaxNumberLength;
            SixthProductQuantityRichTextBox.MaxLength = FormSettings.TextBoxLengths.MaxNumberLength;
            SeventhProductQuantityRichTextBox.MaxLength = FormSettings.TextBoxLengths.MaxNumberLength;
            EighthProductQuantityRichTextBox.MaxLength = FormSettings.TextBoxLengths.MaxNumberLength;
            NinthProductQuantityRichTextBox.MaxLength = FormSettings.TextBoxLengths.MaxNumberLength;
            TenProductQuantityRichTextBox.MaxLength = FormSettings.TextBoxLengths.MaxNumberLength;
            EleventhProductQuantityRichTextBox.MaxLength = FormSettings.TextBoxLengths.MaxNumberLength;
            TwelfthProductQuantityRichTextBox.MaxLength = FormSettings.TextBoxLengths.MaxNumberLength;

            FirstProductPriceRichTextBox.MaxLength = FormSettings.TextBoxLengths.MaxNumberLength;
            SecondProductPriceRichTextBox.MaxLength = FormSettings.TextBoxLengths.MaxNumberLength;
            ThirdProductPriceRichTextBox.MaxLength = FormSettings.TextBoxLengths.MaxNumberLength;
            FourthProductPriceRichTextBox.MaxLength = FormSettings.TextBoxLengths.MaxNumberLength;
            FifthProductPriceRichTextBox.MaxLength = FormSettings.TextBoxLengths.MaxNumberLength;
            SixthProductPriceRichTextBox.MaxLength = FormSettings.TextBoxLengths.MaxNumberLength;
            SeventhProductPriceRichTextBox.MaxLength = FormSettings.TextBoxLengths.MaxNumberLength;
            EighthProductPriceRichTextBox.MaxLength = FormSettings.TextBoxLengths.MaxNumberLength;
            NinthProductPriceRichTextBox.MaxLength = FormSettings.TextBoxLengths.MaxNumberLength;
            TenProductPriceRichTextBox.MaxLength = FormSettings.TextBoxLengths.MaxNumberLength;
            EleventhProductPriceRichTextBox.MaxLength = FormSettings.TextBoxLengths.MaxNumberLength;
            TwelfthProductPriceRichTextBox.MaxLength = FormSettings.TextBoxLengths.MaxNumberLength;

            PriceInWordsRichTextBox.MaxLength = FormSettings.TextBoxLengths.PriceInWords;
            InvoiceMakerRichTextBox.MaxLength = FormSettings.TextBoxLengths.InvoiceMaker;
            InvoiceAcceptedRichTextBox.MaxLength = FormSettings.TextBoxLengths.InvoiceAccepted;

            FirstProductTypeTextBox.MaxLength = FormSettings.TextBoxLengths.ProductType;
            SecondProductTypeTextBox.MaxLength = FormSettings.TextBoxLengths.ProductType;
            ThirdProductTypeTextBox.MaxLength = FormSettings.TextBoxLengths.ProductType;
            FourthProductTypeTextBox.MaxLength = FormSettings.TextBoxLengths.ProductType;
            FifthProductTypeTextBox.MaxLength = FormSettings.TextBoxLengths.ProductType;
            SixthProductTypeTextBox.MaxLength = FormSettings.TextBoxLengths.ProductType;
            SeventhProductTypeTextBox.MaxLength = FormSettings.TextBoxLengths.ProductType;
            EighthProductTypeTextBox.MaxLength = FormSettings.TextBoxLengths.ProductType;
            NinthProductTypeTextBox.MaxLength = FormSettings.TextBoxLengths.ProductType;
            TenProductTypeTextBox.MaxLength = FormSettings.TextBoxLengths.ProductType;
            EleventhProductTypeTextBox.MaxLength = FormSettings.TextBoxLengths.ProductType;
            TwelfthProductTypeTextBox.MaxLength = FormSettings.TextBoxLengths.ProductType;

            FirstProductTypeQuantityTextBox.MaxLength = FormSettings.TextBoxLengths.MaxNumberLength;
            SecondProductTypeQuantityTextBox.MaxLength = FormSettings.TextBoxLengths.MaxNumberLength;
            ThirdProductTypeQuantityTextBox.MaxLength = FormSettings.TextBoxLengths.MaxNumberLength;
            FourthProductTypeQuantityTextBox.MaxLength = FormSettings.TextBoxLengths.MaxNumberLength;
            FifthProductTypeQuantityTextBox.MaxLength = FormSettings.TextBoxLengths.MaxNumberLength;
            SixthProductTypeQuantityTextBox.MaxLength = FormSettings.TextBoxLengths.MaxNumberLength;
            SeventhProductTypeQuantityTextBox.MaxLength = FormSettings.TextBoxLengths.MaxNumberLength;
            EighthProductTypeQuantityTextBox.MaxLength = FormSettings.TextBoxLengths.MaxNumberLength;
            NinthProductTypeQuantityTextBox.MaxLength = FormSettings.TextBoxLengths.MaxNumberLength;
            TenProductTypeQuantityTextBox.MaxLength = FormSettings.TextBoxLengths.MaxNumberLength;
            EleventhProductTypeQuantityTextBox.MaxLength = FormSettings.TextBoxLengths.MaxNumberLength;
            TwelfthProductTypeQuantityTextBox.MaxLength = FormSettings.TextBoxLengths.MaxNumberLength;

            FirstProductTypePriceTextBox.MaxLength = FormSettings.TextBoxLengths.MaxNumberLength;
            SecondProductTypePriceTextBox.MaxLength = FormSettings.TextBoxLengths.MaxNumberLength;
            ThirdProductTypePriceTextBox.MaxLength = FormSettings.TextBoxLengths.MaxNumberLength;
            FourthProductTypePriceTextBox.MaxLength = FormSettings.TextBoxLengths.MaxNumberLength;
            FifthProductTypePriceTextBox.MaxLength = FormSettings.TextBoxLengths.MaxNumberLength;
            SixthProductTypePriceTextBox.MaxLength = FormSettings.TextBoxLengths.MaxNumberLength;
            SeventhProductTypePriceTextBox.MaxLength = FormSettings.TextBoxLengths.MaxNumberLength;
            EighthProductTypePriceTextBox.MaxLength = FormSettings.TextBoxLengths.MaxNumberLength;
            NinthProductTypePriceTextBox.MaxLength = FormSettings.TextBoxLengths.MaxNumberLength;
            TenProductTypePriceTextBox.MaxLength = FormSettings.TextBoxLengths.MaxNumberLength;
            EleventhProductTypePriceTextBox.MaxLength = FormSettings.TextBoxLengths.MaxNumberLength;
            TwelfthProductTypePriceTextBox.MaxLength = FormSettings.TextBoxLengths.MaxNumberLength;
        }

        private void CaptureInvoiceFormScreen()
        {
            _invoiceMemoryImage = new Bitmap(PrintInvoicePanel.Width, PrintInvoicePanel.Height);

            PrintInvoicePanel.DrawToBitmap(
                _invoiceMemoryImage,
                new Rectangle(0, 0, PrintInvoicePanel.Width, PrintInvoicePanel.Height));
        }

        private static byte[] ConvertImageToByteArray(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

        private void SetCursorAtTextBoxStringEnd()
        {
            SerialNumberRichTextBox.SelectionStart = SerialNumberRichTextBox.Text.Length;
            InvoiceDateRichTextBox.SelectionStart = InvoiceDateRichTextBox.Text.Length;

            SellerNameRichTextBox.SelectionStart = SellerNameRichTextBox.Text.Length;
            SellerFirmCodeRichTextBox.SelectionStart = SellerFirmCodeRichTextBox.Text.Length;
            SellerPvmCodeRichTextBox.SelectionStart = SellerPvmCodeRichTextBox.Text.Length;
            SellerAddressRichTextBox.SelectionStart = SellerAddressRichTextBox.Text.Length;
            SellerPhoneNumberRichTextBox.SelectionStart= SellerPhoneNumberRichTextBox.Text.Length;
            SellerBankRichTextBox.SelectionStart = SellerBankRichTextBox.Text.Length;
            SellerBankAccountNumberRichTextBox.SelectionStart = SellerBankAccountNumberRichTextBox.Text.Length;
            SellerEmailAddressRichTextBox.SelectionStart = SellerEmailAddressRichTextBox.Text.Length;

            BuyerNameRichTextBox.SelectionStart = BuyerNameRichTextBox.Text.Length;
            BuyerFirmCodeRichTextBox.SelectionStart = BuyerFirmCodeRichTextBox.Text.Length;
            BuyerPvmCodeRichTextBox.SelectionStart = BuyerPvmCodeRichTextBox.Text.Length;
            BuyerAddressRichTextBox.SelectionStart = BuyerAddressRichTextBox.Text.Length;

            FirstProductNameRichTextBox.SelectionStart = FirstProductNameRichTextBox.Text.Length;
            SecondProductNameRichTextBox.SelectionStart = SecondProductNameRichTextBox.Text.Length;
            ThirdProductNameRichTextBox.SelectionStart = ThirdProductNameRichTextBox.Text.Length;
            FourthProductNameRichTextBox.SelectionStart = FourthProductNameRichTextBox.Text.Length;
            FifthProductNameRichTextBox.SelectionStart = FifthProductNameRichTextBox.Text.Length;
            SixthProductNameRichTextBox.SelectionStart = SixthProductNameRichTextBox.Text.Length;
            SeventhProductNameRichTextBox.SelectionStart = SeventhProductNameRichTextBox.Text.Length;
            EighthProductNameRichTextBox.SelectionStart = EighthProductNameRichTextBox.Text.Length;
            NinthProductNameRichTextBox.SelectionStart = NinthProductNameRichTextBox.Text.Length;
            TenProductNameRichTextBox.SelectionStart = TenProductNameRichTextBox.Text.Length;
            EleventhProductNameRichTextBox.SelectionStart = EleventhProductNameRichTextBox.Text.Length;
            TwelfthProductNameRichTextBox.SelectionStart = TwelfthProductNameRichTextBox.Text.Length;

            FirstProductSeesRichTextBox.SelectionStart = FirstProductSeesRichTextBox.Text.Length;
            SecondProductSeesRichTextBox.SelectionStart = SecondProductSeesRichTextBox.Text.Length;
            ThirdProductSeesRichTextBox.SelectionStart = ThirdProductSeesRichTextBox.Text.Length;
            FourthProductSeesRichTextBox.SelectionStart = FourthProductSeesRichTextBox.Text.Length;
            FifthProductSeesRichTextBox.SelectionStart = FifthProductSeesRichTextBox.Text.Length;
            SixthProductSeesRichTextBox.SelectionStart = SixthProductSeesRichTextBox.Text.Length;
            SeventhProductSeesRichTextBox.SelectionStart = SeventhProductSeesRichTextBox.Text.Length;
            EighthProductSeesRichTextBox.SelectionStart = EighthProductSeesRichTextBox.Text.Length;
            NinthProductSeesRichTextBox.SelectionStart = NinthProductSeesRichTextBox.Text.Length;
            TenProductSeesRichTextBox.SelectionStart = TenProductSeesRichTextBox.Text.Length;
            EleventhProductSeesRichTextBox.SelectionStart = EleventhProductSeesRichTextBox.Text.Length;
            TwelfthProductSeesRichTextBox.SelectionStart = TwelfthProductSeesRichTextBox.Text.Length;

            FirstProductQuantityRichTextBox.SelectionStart = FirstProductQuantityRichTextBox.Text.Length;
            SecondProductQuantityRichTextBox.SelectionStart = SecondProductQuantityRichTextBox.Text.Length;
            ThirdProductQuantityRichTextBox.SelectionStart = ThirdProductQuantityRichTextBox.Text.Length;
            FourthProductQuantityRichTextBox.SelectionStart = FourthProductQuantityRichTextBox.Text.Length;
            FifthProductQuantityRichTextBox.SelectionStart = FifthProductQuantityRichTextBox.Text.Length;
            SixthProductQuantityRichTextBox.SelectionStart = SixthProductQuantityRichTextBox.Text.Length;
            SeventhProductQuantityRichTextBox.SelectionStart = SeventhProductQuantityRichTextBox.Text.Length;
            EighthProductQuantityRichTextBox.SelectionStart = EighthProductQuantityRichTextBox.Text.Length;
            NinthProductQuantityRichTextBox.SelectionStart = NinthProductQuantityRichTextBox.Text.Length;
            TenProductQuantityRichTextBox.SelectionStart = TenProductQuantityRichTextBox.Text.Length;
            EleventhProductQuantityRichTextBox.SelectionStart = EleventhProductQuantityRichTextBox.Text.Length;
            TwelfthProductQuantityRichTextBox.SelectionStart = TwelfthProductQuantityRichTextBox.Text.Length;

            FirstProductPriceRichTextBox.SelectionStart = FirstProductPriceRichTextBox.Text.Length;
            SecondProductPriceRichTextBox.SelectionStart = SecondProductPriceRichTextBox.Text.Length;
            ThirdProductPriceRichTextBox.SelectionStart = ThirdProductPriceRichTextBox.Text.Length;
            FourthProductPriceRichTextBox.SelectionStart = FourthProductPriceRichTextBox.Text.Length;
            FifthProductPriceRichTextBox.SelectionStart = FifthProductPriceRichTextBox.Text.Length;
            SixthProductPriceRichTextBox.SelectionStart = SixthProductPriceRichTextBox.Text.Length;
            SeventhProductPriceRichTextBox.SelectionStart = SeventhProductPriceRichTextBox.Text.Length;
            EighthProductPriceRichTextBox.SelectionStart = EighthProductPriceRichTextBox.Text.Length;
            NinthProductPriceRichTextBox.SelectionStart = NinthProductPriceRichTextBox.Text.Length;
            TenProductPriceRichTextBox.SelectionStart = TenProductPriceRichTextBox.Text.Length;
            EleventhProductPriceRichTextBox.SelectionStart = EleventhProductPriceRichTextBox.Text.Length;
            TwelfthProductPriceRichTextBox.SelectionStart = TwelfthProductPriceRichTextBox.Text.Length;

            PriceInWordsRichTextBox.SelectionStart = PriceInWordsRichTextBox.Text.Length;
            InvoiceMakerRichTextBox.SelectionStart = InvoiceMakerRichTextBox.Text.Length;
            InvoiceAcceptedRichTextBox.SelectionStart = InvoiceAcceptedRichTextBox.Text.Length;
        }

        private void DisableScrollBarRichTextBoxWithMultiLine()
        {
            FirstProductNameRichTextBox.ScrollBars = RichTextBoxScrollBars.None;
            SecondProductNameRichTextBox.ScrollBars = RichTextBoxScrollBars.None;
            ThirdProductNameRichTextBox.ScrollBars = RichTextBoxScrollBars.None;
            FourthProductNameRichTextBox.ScrollBars = RichTextBoxScrollBars.None;
            FifthProductNameRichTextBox.ScrollBars = RichTextBoxScrollBars.None;
            SixthProductNameRichTextBox.ScrollBars = RichTextBoxScrollBars.None;
            SeventhProductNameRichTextBox.ScrollBars = RichTextBoxScrollBars.None;
            EighthProductNameRichTextBox.ScrollBars = RichTextBoxScrollBars.None;
            NinthProductNameRichTextBox.ScrollBars = RichTextBoxScrollBars.None;
            TenProductNameRichTextBox.ScrollBars = RichTextBoxScrollBars.None;
            EleventhProductNameRichTextBox.ScrollBars = RichTextBoxScrollBars.None;
            TwelfthProductNameRichTextBox.ScrollBars = RichTextBoxScrollBars.None;
        }

        private void SetCursorAtProductTypeStringEnd()
        {
            FirstProductTypeTextBox.SelectionStart = FirstProductTypeTextBox.Text.Length;
            SecondProductTypeTextBox.SelectionStart = SecondProductTypeTextBox.Text.Length;
            ThirdProductTypeTextBox.SelectionStart = ThirdProductTypeTextBox.Text.Length;
            FourthProductTypeTextBox.SelectionStart = FourthProductTypeTextBox.Text.Length;
            FifthProductTypeTextBox.SelectionStart = FifthProductTypeTextBox.Text.Length;
            SixthProductTypeTextBox.SelectionStart = SixthProductTypeTextBox.Text.Length;
            SeventhProductTypeTextBox.SelectionStart = SeventhProductTypeTextBox.Text.Length;
            EighthProductTypeTextBox.SelectionStart = EighthProductTypeTextBox.Text.Length;
            NinthProductTypeTextBox.SelectionStart = NinthProductTypeTextBox.Text.Length;
            TenProductTypeTextBox.SelectionStart = TenProductTypeTextBox.Text.Length;
            EleventhProductTypeTextBox.SelectionStart = EleventhProductTypeTextBox.Text.Length;
            TwelfthProductTypeTextBox.SelectionStart = TwelfthProductTypeTextBox.Text.Length;

            FirstProductTypeQuantityTextBox.SelectionStart = FirstProductTypeQuantityTextBox.Text.Length;
            SecondProductTypeQuantityTextBox.SelectionStart = SecondProductTypeQuantityTextBox.Text.Length;
            ThirdProductTypeQuantityTextBox.SelectionStart = ThirdProductTypeQuantityTextBox.Text.Length;
            FourthProductTypeQuantityTextBox.SelectionStart = FourthProductTypeQuantityTextBox.Text.Length;
            FifthProductTypeQuantityTextBox.SelectionStart = FifthProductTypeQuantityTextBox.Text.Length;
            SixthProductTypeQuantityTextBox.SelectionStart = SixthProductTypeQuantityTextBox.Text.Length;
            SeventhProductTypeQuantityTextBox.SelectionStart = SeventhProductTypeQuantityTextBox.Text.Length;
            EighthProductTypeQuantityTextBox.SelectionStart = EighthProductTypeQuantityTextBox.Text.Length;
            NinthProductTypeQuantityTextBox.SelectionStart = NinthProductTypeQuantityTextBox.Text.Length;
            TenProductTypeQuantityTextBox.SelectionStart = TenProductTypeQuantityTextBox.Text.Length;
            EleventhProductTypeQuantityTextBox.SelectionStart = EleventhProductTypeQuantityTextBox.Text.Length;
            TwelfthProductTypeQuantityTextBox.SelectionStart = TwelfthProductTypeQuantityTextBox.Text.Length;

            FirstProductTypePriceTextBox.SelectionStart = FirstProductTypePriceTextBox.Text.Length;
            SecondProductTypePriceTextBox.SelectionStart = SecondProductTypePriceTextBox.Text.Length;
            ThirdProductTypePriceTextBox.SelectionStart = ThirdProductTypePriceTextBox.Text.Length;
            FourthProductTypePriceTextBox.SelectionStart = FourthProductTypePriceTextBox.Text.Length;
            FifthProductTypePriceTextBox.SelectionStart = FifthProductTypePriceTextBox.Text.Length;
            SixthProductTypePriceTextBox.SelectionStart = SixthProductTypePriceTextBox.Text.Length;
            SeventhProductTypePriceTextBox.SelectionStart = SeventhProductTypePriceTextBox.Text.Length;
            EighthProductTypePriceTextBox.SelectionStart = EighthProductTypePriceTextBox.Text.Length;
            NinthProductTypePriceTextBox.SelectionStart = NinthProductTypePriceTextBox.Text.Length;
            TenProductTypePriceTextBox.SelectionStart = TenProductTypePriceTextBox.Text.Length;
            EleventhProductTypePriceTextBox.SelectionStart = EleventhProductTypePriceTextBox.Text.Length;
            TwelfthProductTypePriceTextBox.SelectionStart = TwelfthProductTypePriceTextBox.Text.Length;
        }

        private void SetCursorAtDateTextBoxEnd()
        {
            InvoiceDateRichTextBox.SelectionStart = InvoiceDateRichTextBox.Text.Length;
        }

        private void GetExistingProductTypeInfo()
        {
            if (_invoiceNumber.HasValue && _invoiceNumberYearCreation.HasValue)
            {
                ProductTypeModel getExistingProductTypes =
                    _productTypeRepository.GetExistingProductType(_invoiceNumber.Value, _invoiceNumberYearCreation.Value);

                if (getExistingProductTypes != null)
                {
                    FirstProductTypeTextBox.Text = getExistingProductTypes.FirstProductType;
                    SecondProductTypeTextBox.Text = getExistingProductTypes.SecondProductType;
                    ThirdProductTypeTextBox.Text = getExistingProductTypes.ThirdProductType;
                    FourthProductTypeTextBox.Text = getExistingProductTypes.FourthProductType;
                    FifthProductTypeTextBox.Text = getExistingProductTypes.FifthProductType;
                    SixthProductTypeTextBox.Text = getExistingProductTypes.SixthProductType;
                    SeventhProductTypeTextBox.Text = getExistingProductTypes.SeventhProductType;
                    EighthProductTypeTextBox.Text = getExistingProductTypes.EighthProductType;
                    NinthProductTypeTextBox.Text = getExistingProductTypes.NinthProductType;
                    TenProductTypeTextBox.Text = getExistingProductTypes.TenProductType;
                    EleventhProductTypeTextBox.Text = getExistingProductTypes.EleventhProductType;
                    TwelfthProductTypeTextBox.Text = getExistingProductTypes.TwelfthProductType;

                    FirstProductTypeQuantityTextBox.Text =
                        _numberService.ToStringProductTypeQuantityOrEmpty(
                            ProductTypeQuantityOperations.FirstProductTypeQuantity, getExistingProductTypes);

                    SecondProductTypeQuantityTextBox.Text =
                        _numberService.ToStringProductTypeQuantityOrEmpty(
                            ProductTypeQuantityOperations.SecondProductTypeQuantity, getExistingProductTypes);

                    ThirdProductTypeQuantityTextBox.Text = _numberService.ToStringProductTypeQuantityOrEmpty(
                        ProductTypeQuantityOperations.ThirdProductTypeQuantity, getExistingProductTypes);

                    FourthProductTypeQuantityTextBox.Text = _numberService.ToStringProductTypeQuantityOrEmpty(
                        ProductTypeQuantityOperations.FourthProductTypeQuantity, getExistingProductTypes);

                    FifthProductTypeQuantityTextBox.Text = _numberService.ToStringProductTypeQuantityOrEmpty(
                        ProductTypeQuantityOperations.FifthProductTypeQuantity, getExistingProductTypes);

                    SixthProductTypeQuantityTextBox.Text = _numberService.ToStringProductTypeQuantityOrEmpty(
                        ProductTypeQuantityOperations.SixthProductTypeQuantity, getExistingProductTypes);

                    SeventhProductTypeQuantityTextBox.Text = _numberService.ToStringProductTypeQuantityOrEmpty(
                        ProductTypeQuantityOperations.SeventhProductTypeQuantity, getExistingProductTypes);

                    EighthProductTypeQuantityTextBox.Text = _numberService.ToStringProductTypeQuantityOrEmpty(
                        ProductTypeQuantityOperations.EighthProductTypeQuantity, getExistingProductTypes);

                    NinthProductTypeQuantityTextBox.Text = _numberService.ToStringProductTypeQuantityOrEmpty(
                        ProductTypeQuantityOperations.NinthProductTypeQuantity, getExistingProductTypes);

                    TenProductTypeQuantityTextBox.Text = _numberService.ToStringProductTypeQuantityOrEmpty(
                        ProductTypeQuantityOperations.TenProductTypeQuantity, getExistingProductTypes);

                    EleventhProductTypeQuantityTextBox.Text = _numberService.ToStringProductTypeQuantityOrEmpty(
                        ProductTypeQuantityOperations.EleventhProductTypeQuantity, getExistingProductTypes);

                    TwelfthProductTypeQuantityTextBox.Text = _numberService.ToStringProductTypeQuantityOrEmpty(
                        ProductTypeQuantityOperations.TwelfthProductTypeQuantity, getExistingProductTypes);

                    FirstProductTypePriceTextBox.Text =
                        _numberService.ToStringProductTypePriceOrEmpty(ProductTypePriceOperations.FirstProductTypePrice,
                            getExistingProductTypes);

                    SecondProductTypePriceTextBox.Text = _numberService.ToStringProductTypePriceOrEmpty(ProductTypePriceOperations.SecondProductTypePrice,
                        getExistingProductTypes);

                    ThirdProductTypePriceTextBox.Text = _numberService.ToStringProductTypePriceOrEmpty(ProductTypePriceOperations.ThirdProductTypePrice,
                        getExistingProductTypes);

                    FourthProductTypePriceTextBox.Text = _numberService.ToStringProductTypePriceOrEmpty(ProductTypePriceOperations.FourthProductTypePrice,
                        getExistingProductTypes);

                    FifthProductTypePriceTextBox.Text = _numberService.ToStringProductTypePriceOrEmpty(ProductTypePriceOperations.FifthProductTypePrice,
                        getExistingProductTypes);

                    SixthProductTypePriceTextBox.Text = _numberService.ToStringProductTypePriceOrEmpty(ProductTypePriceOperations.SixthProductTypePrice,
                        getExistingProductTypes);

                    SeventhProductTypePriceTextBox.Text = _numberService.ToStringProductTypePriceOrEmpty(ProductTypePriceOperations.SeventhProductTypePrice,
                        getExistingProductTypes);

                    EighthProductTypePriceTextBox.Text = _numberService.ToStringProductTypePriceOrEmpty(ProductTypePriceOperations.EighthProductTypePrice,
                        getExistingProductTypes);

                    NinthProductTypePriceTextBox.Text = _numberService.ToStringProductTypePriceOrEmpty(ProductTypePriceOperations.NinthProductTypePrice,
                        getExistingProductTypes);
                    TenProductTypePriceTextBox.Text = _numberService.ToStringProductTypePriceOrEmpty(ProductTypePriceOperations.TenProductTypePrice,
                        getExistingProductTypes);

                    EleventhProductTypePriceTextBox.Text = _numberService.ToStringProductTypePriceOrEmpty(ProductTypePriceOperations.EleventhProductTypePrice,
                        getExistingProductTypes);

                    TwelfthProductTypePriceTextBox.Text = _numberService.ToStringProductTypePriceOrEmpty(ProductTypePriceOperations.TwelfthProductTypePrice,
                        getExistingProductTypes);
                }
            }
        }

        private void GetAllProductTypeForNewInvoice()
        {
            ChangeProductTypeQuantityAndPriceFromCommaToDot();
            ProductTypeModel productType = GetAllInfoFromProductTypeTextBox();

            if (_invoiceOperations == InvoiceOperations.Edit && _invoiceNumber.HasValue && _invoiceNumberYearCreation.HasValue)
            {
                 _productTypeRepository.CreateNewProductType(_invoiceNumber.Value,
                    _invoiceNumberYearCreation.Value, productType);
            }
            else
            {
                int invoiceNumber = int.Parse(InvoiceNumberRichTextBox.Text);
                int invoiceNumberYearCreation = DateTime.Now.Year;
                _productTypeRepository.CreateNewProductType(invoiceNumber, invoiceNumberYearCreation, productType);
            }
        }

        private void ChangeProductTypeQuantityAndPriceFromCommaToDot()
        {
            FirstProductTypeQuantityTextBox.Text = _numberService.ChangeCommaToDot(FirstProductTypeQuantityTextBox);
            SecondProductTypeQuantityTextBox.Text = _numberService.ChangeCommaToDot(SecondProductTypeQuantityTextBox);
            ThirdProductTypeQuantityTextBox.Text = _numberService.ChangeCommaToDot(ThirdProductTypeQuantityTextBox);
            FourthProductTypeQuantityTextBox.Text = _numberService.ChangeCommaToDot(FourthProductTypeQuantityTextBox);
            FifthProductTypeQuantityTextBox.Text = _numberService.ChangeCommaToDot(FifthProductTypeQuantityTextBox);
            SixthProductTypeQuantityTextBox.Text = _numberService.ChangeCommaToDot(SixthProductTypeQuantityTextBox);
            SeventhProductTypeQuantityTextBox.Text = _numberService.ChangeCommaToDot(SeventhProductTypeQuantityTextBox);
            EighthProductTypeQuantityTextBox.Text = _numberService.ChangeCommaToDot(EighthProductTypeQuantityTextBox);
            NinthProductTypeQuantityTextBox.Text = _numberService.ChangeCommaToDot(NinthProductTypeQuantityTextBox);
            TenProductTypeQuantityTextBox.Text = _numberService.ChangeCommaToDot(TenProductTypeQuantityTextBox);
            EleventhProductTypeQuantityTextBox.Text = _numberService.ChangeCommaToDot(EleventhProductTypeQuantityTextBox);
            TwelfthProductTypeQuantityTextBox.Text = _numberService.ChangeCommaToDot(TwelfthProductTypeQuantityTextBox);

            FirstProductTypePriceTextBox.Text = _numberService.ChangeCommaToDot(FirstProductTypePriceTextBox);
            SecondProductTypePriceTextBox.Text = _numberService.ChangeCommaToDot(SecondProductTypePriceTextBox);
            ThirdProductTypePriceTextBox.Text = _numberService.ChangeCommaToDot(ThirdProductTypePriceTextBox);
            FourthProductTypePriceTextBox.Text = _numberService.ChangeCommaToDot(FourthProductTypePriceTextBox);
            FifthProductTypePriceTextBox.Text = _numberService.ChangeCommaToDot(FifthProductTypePriceTextBox);
            SixthProductTypePriceTextBox.Text = _numberService.ChangeCommaToDot(SixthProductTypePriceTextBox);
            SeventhProductTypePriceTextBox.Text = _numberService.ChangeCommaToDot(SeventhProductTypePriceTextBox);
            EighthProductTypePriceTextBox.Text = _numberService.ChangeCommaToDot(EighthProductTypePriceTextBox);
            NinthProductTypePriceTextBox.Text = _numberService.ChangeCommaToDot(NinthProductTypePriceTextBox);
            TenProductTypePriceTextBox.Text = _numberService.ChangeCommaToDot(TenProductTypePriceTextBox);
            EleventhProductTypePriceTextBox.Text = _numberService.ChangeCommaToDot(EleventhProductTypePriceTextBox);
            TwelfthProductTypePriceTextBox.Text = _numberService.ChangeCommaToDot(TwelfthProductTypePriceTextBox);
        }

        private ProductTypeModel GetAllInfoFromProductTypeTextBox()
        {
            ProductTypeModel getAllInfo = new ProductTypeModel()
            {
                FirstProductType = FirstProductTypeTextBox.Text,
                SecondProductType = SecondProductTypeTextBox.Text,
                ThirdProductType = ThirdProductTypeTextBox.Text,
                FourthProductType = FourthProductTypeTextBox.Text,
                FifthProductType = FifthProductTypeTextBox.Text,
                SixthProductType = SixthProductTypeTextBox.Text,
                SeventhProductType = SeventhProductTypeTextBox.Text,
                EighthProductType = EighthProductTypeTextBox.Text,
                NinthProductType = NinthProductTypeTextBox.Text,
                TenProductType = TenProductTypeTextBox.Text,
                EleventhProductType = EleventhProductTypeTextBox.Text,
                TwelfthProductType = TwelfthProductTypeTextBox.Text,

                FirstProductTypeQuantity = _numberService.ParseToDoubleOrNull(FirstProductTypeQuantityTextBox),
                SecondProductTypeQuantity = _numberService.ParseToDoubleOrNull(SecondProductTypeQuantityTextBox),
                ThirdProductTypeQuantity = _numberService.ParseToDoubleOrNull(ThirdProductTypeQuantityTextBox),
                FourthProductTypeQuantity = _numberService.ParseToDoubleOrNull(FourthProductTypeQuantityTextBox),
                FifthProductTypeQuantity = _numberService.ParseToDoubleOrNull(FifthProductTypeQuantityTextBox),
                SixthProductTypeQuantity = _numberService.ParseToDoubleOrNull(SixthProductTypeQuantityTextBox),
                SeventhProductTypeQuantity = _numberService.ParseToDoubleOrNull(SeventhProductTypeQuantityTextBox),
                EighthProductTypeQuantity = _numberService.ParseToDoubleOrNull(EighthProductTypeQuantityTextBox),
                NinthProductTypeQuantity = _numberService.ParseToDoubleOrNull(NinthProductTypeQuantityTextBox),
                TenProductTypeQuantity = _numberService.ParseToDoubleOrNull(TenProductTypeQuantityTextBox),
                EleventhProductTypeQuantity = _numberService.ParseToDoubleOrNull(EleventhProductTypeQuantityTextBox),
                TwelfthProductTypeQuantity = _numberService.ParseToDoubleOrNull(TwelfthProductTypeQuantityTextBox),

                FirstProductTypePrice = _numberService.ParseToDoubleOrNull(FirstProductTypePriceTextBox),
                SecondProductTypePrice = _numberService.ParseToDoubleOrNull(SecondProductTypePriceTextBox),
                ThirdProductTypePrice = _numberService.ParseToDoubleOrNull(ThirdProductTypePriceTextBox),
                FourthProductTypePrice = _numberService.ParseToDoubleOrNull(FourthProductTypePriceTextBox),
                FifthProductTypePrice = _numberService.ParseToDoubleOrNull(FifthProductTypePriceTextBox),
                SixthProductTypePrice = _numberService.ParseToDoubleOrNull(SixthProductTypePriceTextBox),
                SeventhProductTypePrice = _numberService.ParseToDoubleOrNull(SeventhProductTypePriceTextBox),
                EighthProductTypePrice = _numberService.ParseToDoubleOrNull(EighthProductTypePriceTextBox),
                NinthProductTypePrice = _numberService.ParseToDoubleOrNull(NinthProductTypePriceTextBox),
                TenProductTypePrice = _numberService.ParseToDoubleOrNull(TenProductTypePriceTextBox),
                EleventhProductTypePrice = _numberService.ParseToDoubleOrNull(EleventhProductTypePriceTextBox),
                TwelfthProductTypePrice = _numberService.ParseToDoubleOrNull(TwelfthProductTypePriceTextBox),
            };

            return getAllInfo;
        }

        #endregion

    }
}
