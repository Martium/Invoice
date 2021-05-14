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

            var invoiceModel = GetAllInfoFromAllTextBoxesAndRichTextBoxes();

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
                this.SelectNextControl((Control)sender, true, true, true, true);
                SetCursorAtRichTextBoxStringEnd();
            }
        }

        private void TextBox_Keyup(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
                SerCursorAtTextBoxStringEnd();
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

        private InvoiceModel GetAllInfoFromAllTextBoxesAndRichTextBoxes()
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

                FirstProductQuantity = _numberService.ParseToDoubleOrNullFromRichTextBoxText(FirstProductQuantityRichTextBox),
                SecondProductQuantity = _numberService.ParseToDoubleOrNullFromRichTextBoxText(SecondProductQuantityRichTextBox),
                ThirdProductQuantity = _numberService.ParseToDoubleOrNullFromRichTextBoxText(ThirdProductQuantityRichTextBox),
                FourthProductQuantity = _numberService.ParseToDoubleOrNullFromRichTextBoxText(FourthProductQuantityRichTextBox),
                FifthProductQuantity = _numberService.ParseToDoubleOrNullFromRichTextBoxText(FifthProductQuantityRichTextBox),
                SixthProductQuantity = _numberService.ParseToDoubleOrNullFromRichTextBoxText(SixthProductQuantityRichTextBox),
                SeventhProductQuantity = _numberService.ParseToDoubleOrNullFromRichTextBoxText(SeventhProductQuantityRichTextBox),
                EighthProductQuantity = _numberService.ParseToDoubleOrNullFromRichTextBoxText(EighthProductQuantityRichTextBox),
                NinthProductQuantity = _numberService.ParseToDoubleOrNullFromRichTextBoxText(NinthProductQuantityRichTextBox),
                TenProductQuantity = _numberService.ParseToDoubleOrNullFromRichTextBoxText(TenProductQuantityRichTextBox),
                EleventhProductQuantity = _numberService.ParseToDoubleOrNullFromRichTextBoxText(EleventhProductQuantityRichTextBox),
                TwelfthProductQuantity = _numberService.ParseToDoubleOrNullFromRichTextBoxText(TwelfthProductQuantityRichTextBox),

                FirstProductPrice = _numberService.ParseToDoubleOrNullFromRichTextBoxText(FirstProductPriceRichTextBox),
                SecondProductPrice = _numberService.ParseToDoubleOrNullFromRichTextBoxText(SecondProductPriceRichTextBox),
                ThirdProductPrice = _numberService.ParseToDoubleOrNullFromRichTextBoxText(ThirdProductPriceRichTextBox),
                FourthProductPrice = _numberService.ParseToDoubleOrNullFromRichTextBoxText(FourthProductPriceRichTextBox),
                FifthProductPrice = _numberService.ParseToDoubleOrNullFromRichTextBoxText(FifthProductPriceRichTextBox),
                SixthProductPrice = _numberService.ParseToDoubleOrNullFromRichTextBoxText(SixthProductPriceRichTextBox),
                SeventhProductPrice = _numberService.ParseToDoubleOrNullFromRichTextBoxText(SeventhProductPriceRichTextBox),
                EighthProductPrice = _numberService.ParseToDoubleOrNullFromRichTextBoxText(EighthProductPriceRichTextBox),
                NinthProductPrice = _numberService.ParseToDoubleOrNullFromRichTextBoxText(NinthProductPriceRichTextBox),
                TenProductPrice = _numberService.ParseToDoubleOrNullFromRichTextBoxText(TenProductPriceRichTextBox),
                EleventhProductPrice = _numberService.ParseToDoubleOrNullFromRichTextBoxText(EleventhProductPriceRichTextBox),
                TwelfthProductPrice = _numberService.ParseToDoubleOrNullFromRichTextBoxText(TwelfthProductPriceRichTextBox),

                PriceInWords = PriceInWordsRichTextBox.Text,
                InvoiceMaker = InvoiceMakerRichTextBox.Text,
                InvoiceAccepted = InvoiceAcceptedRichTextBox.Text,

                TotalPriceWithPvm = TotalPriceWithPvmRichTextBox.Text,

                PaymentStatus = _paymentStatus,

                FirstProductType = FirstProductTypeTextBox.Text,
                SecondProductType = SecondProductTypeTextBox.Text,
                ThirdProductType = ThirdProductTypeTextBox.Text,
                FourthProductType = FourthProductTypeTextBox.Text,
                FifthProductType = FifthProductTypeTextBox.Text,

                FirstProductTypeQuantity = _numberService.ParseToDoubleOrNullFromTextBoxText(FirstProductTypeQuantityTextBox),
                SecondProductTypeQuantity = _numberService.ParseToDoubleOrNullFromTextBoxText(SecondProductTypeQuantityTextBox),
                ThirdProductTypeQuantity = _numberService.ParseToDoubleOrNullFromTextBoxText(ThirdProductTypeQuantityTextBox),
                FourthProductTypeQuantity = _numberService.ParseToDoubleOrNullFromTextBoxText(FourthProductTypeQuantityTextBox),
                FifthProductTypeQuantity = _numberService.ParseToDoubleOrNullFromTextBoxText(FifthProductTypeQuantityTextBox),

                FirstProductTypePrice = _numberService.ParseToDoubleOrNullFromTextBoxText(FirstProductTypePriceTextBox),
                SecondProductTypePrice = _numberService.ParseToDoubleOrNullFromTextBoxText(SecondProductTypePriceTextBox),
                ThirdProductTypePrice = _numberService.ParseToDoubleOrNullFromTextBoxText(ThirdProductTypePriceTextBox),
                FourthProductTypePrice = _numberService.ParseToDoubleOrNullFromTextBoxText(FourthProductTypePriceTextBox),
                FifthProductTypePrice = _numberService.ParseToDoubleOrNullFromTextBoxText(FifthProductTypePriceTextBox)
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
                    _numberService.DoubleToStringOrEmptyInvoiceModel(invoiceModel, "FirstProductQuantity");
                SecondProductQuantityRichTextBox.Text =
                    _numberService.DoubleToStringOrEmptyInvoiceModel(invoiceModel, "SecondProductQuantity");
                ThirdProductQuantityRichTextBox.Text =
                    _numberService.DoubleToStringOrEmptyInvoiceModel(invoiceModel, "ThirdProductQuantity");
                FourthProductQuantityRichTextBox.Text =
                    _numberService.DoubleToStringOrEmptyInvoiceModel(invoiceModel, "FourthProductQuantity");
                FifthProductQuantityRichTextBox.Text =
                    _numberService.DoubleToStringOrEmptyInvoiceModel(invoiceModel, "FifthProductQuantity");
                SixthProductQuantityRichTextBox.Text =
                    _numberService.DoubleToStringOrEmptyInvoiceModel(invoiceModel, "SixthProductQuantity");
                SeventhProductQuantityRichTextBox.Text =
                    _numberService.DoubleToStringOrEmptyInvoiceModel(invoiceModel, "SeventhProductQuantity");
                EighthProductQuantityRichTextBox.Text =
                    _numberService.DoubleToStringOrEmptyInvoiceModel(invoiceModel, "EighthProductQuantity");
                NinthProductQuantityRichTextBox.Text =
                    _numberService.DoubleToStringOrEmptyInvoiceModel(invoiceModel, "NinthProductQuantity");
                TenProductQuantityRichTextBox.Text =
                    _numberService.DoubleToStringOrEmptyInvoiceModel(invoiceModel, "TenProductQuantity");
                EleventhProductQuantityRichTextBox.Text =
                    _numberService.DoubleToStringOrEmptyInvoiceModel(invoiceModel, "EleventhProductQuantity");
                TwelfthProductQuantityRichTextBox.Text =
                    _numberService.DoubleToStringOrEmptyInvoiceModel(invoiceModel, "TwelfthProductQuantity");

                FirstProductPriceRichTextBox.Text =
                    _numberService.DoubleToStringOrEmptyInvoiceModel(invoiceModel, "FirstProductPrice");
                SecondProductPriceRichTextBox.Text =
                    _numberService.DoubleToStringOrEmptyInvoiceModel(invoiceModel, "SecondProductPrice");
                ThirdProductPriceRichTextBox.Text =
                    _numberService.DoubleToStringOrEmptyInvoiceModel(invoiceModel, "ThirdProductPrice");
                FourthProductPriceRichTextBox.Text =
                    _numberService.DoubleToStringOrEmptyInvoiceModel(invoiceModel, "FourthProductPrice");
                FifthProductPriceRichTextBox.Text =
                    _numberService.DoubleToStringOrEmptyInvoiceModel(invoiceModel, "FifthProductPrice");
                SixthProductPriceRichTextBox.Text =
                    _numberService.DoubleToStringOrEmptyInvoiceModel(invoiceModel, "SixthProductPrice");
                SeventhProductPriceRichTextBox.Text =
                    _numberService.DoubleToStringOrEmptyInvoiceModel(invoiceModel, "SeventhProductPrice");
                EighthProductPriceRichTextBox.Text =
                    _numberService.DoubleToStringOrEmptyInvoiceModel(invoiceModel, "EighthProductPrice");
                NinthProductPriceRichTextBox.Text =
                    _numberService.DoubleToStringOrEmptyInvoiceModel(invoiceModel, "NinthProductPrice");
                TenProductPriceRichTextBox.Text = _numberService.DoubleToStringOrEmptyInvoiceModel(invoiceModel, "TenProductPrice");
                EleventhProductPriceRichTextBox.Text =
                    _numberService.DoubleToStringOrEmptyInvoiceModel(invoiceModel, "EleventhProductPrice");
                TwelfthProductPriceRichTextBox.Text =
                    _numberService.DoubleToStringOrEmptyInvoiceModel(invoiceModel, "TwelfthProductPrice");

                PriceInWordsRichTextBox.Text = invoiceModel.PriceInWords;
                InvoiceMakerRichTextBox.Text = invoiceModel.InvoiceMaker;
                InvoiceAcceptedRichTextBox.Text = invoiceModel.InvoiceAccepted;

                ProductTypeModel productTypeModel =
                    _invoiceRepository.GetProductTypeInfoFromInvoiceNumberAndCreationYear(_invoiceNumber.Value,
                        _invoiceNumberYearCreation.Value);

                FirstProductTypeTextBox.Text = productTypeModel.FirstProductType;
                SecondProductTypeTextBox.Text = productTypeModel.SecondProductType;
                ThirdProductTypeTextBox.Text = productTypeModel.ThirdProductType;
                FourthProductTypeTextBox.Text = productTypeModel.FourthProductType;
                FifthProductTypeTextBox.Text = productTypeModel.FifthProductType;

                FirstProductTypeQuantityTextBox.Text =
                    _numberService.DoubleToStringOrEmptyProductTypeModel(productTypeModel, "FirstProductTypeQuantity");
                SecondProductTypeQuantityTextBox.Text =
                    _numberService.DoubleToStringOrEmptyProductTypeModel(productTypeModel, "SecondProductTypeQuantity");
                ThirdProductTypeQuantityTextBox.Text =
                    _numberService.DoubleToStringOrEmptyProductTypeModel(productTypeModel, "ThirdProductTypeQuantity");
                FourthProductTypeQuantityTextBox.Text =
                    _numberService.DoubleToStringOrEmptyProductTypeModel(productTypeModel, "FourthProductTypeQuantity");
                FifthProductTypeQuantityTextBox.Text =
                    _numberService.DoubleToStringOrEmptyProductTypeModel(productTypeModel, "FifthProductTypeQuantity");

                FirstProductTypePriceTextBox.Text =
                    _numberService.DoubleToStringOrEmptyProductTypeModel(productTypeModel, "FirstProductTypePrice");
                SecondProductTypePriceTextBox.Text =
                    _numberService.DoubleToStringOrEmptyProductTypeModel(productTypeModel, "SecondProductTypePrice");
                ThirdProductTypePriceTextBox.Text =
                    _numberService.DoubleToStringOrEmptyProductTypeModel(productTypeModel, "ThirdProductTypePrice");
                FourthProductTypePriceTextBox.Text =
                    _numberService.DoubleToStringOrEmptyProductTypeModel(productTypeModel, "FourthProductTypePrice");
                FifthProductTypePriceTextBox.Text =
                    _numberService.DoubleToStringOrEmptyProductTypeModel(productTypeModel, "FifthProductTypePrice");

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
            FirstProductQuantityRichTextBox.Text = _numberService.ChangeCommaToDotFromRichTextBoxText(FirstProductQuantityRichTextBox);
            SecondProductQuantityRichTextBox.Text = _numberService.ChangeCommaToDotFromRichTextBoxText(SecondProductQuantityRichTextBox);
            ThirdProductQuantityRichTextBox.Text = _numberService.ChangeCommaToDotFromRichTextBoxText(ThirdProductQuantityRichTextBox);
            FourthProductQuantityRichTextBox.Text = _numberService.ChangeCommaToDotFromRichTextBoxText(FourthProductQuantityRichTextBox);
            FifthProductQuantityRichTextBox.Text = _numberService.ChangeCommaToDotFromRichTextBoxText(FifthProductQuantityRichTextBox);
            SixthProductQuantityRichTextBox.Text = _numberService.ChangeCommaToDotFromRichTextBoxText(SixthProductQuantityRichTextBox);
            SeventhProductQuantityRichTextBox.Text = _numberService.ChangeCommaToDotFromRichTextBoxText(SeventhProductQuantityRichTextBox);
            EighthProductQuantityRichTextBox.Text = _numberService.ChangeCommaToDotFromRichTextBoxText(EighthProductQuantityRichTextBox);
            NinthProductQuantityRichTextBox.Text = _numberService.ChangeCommaToDotFromRichTextBoxText(NinthProductQuantityRichTextBox);
            TenProductQuantityRichTextBox.Text = _numberService.ChangeCommaToDotFromRichTextBoxText(TenProductQuantityRichTextBox);
            EleventhProductQuantityRichTextBox.Text = _numberService.ChangeCommaToDotFromRichTextBoxText(EleventhProductQuantityRichTextBox);
            TwelfthProductQuantityRichTextBox.Text = _numberService.ChangeCommaToDotFromRichTextBoxText(TwelfthProductQuantityRichTextBox);

            FirstProductPriceRichTextBox.Text = _numberService.ChangeCommaToDotFromRichTextBoxText(FirstProductPriceRichTextBox);
            SecondProductPriceRichTextBox.Text = _numberService.ChangeCommaToDotFromRichTextBoxText(SecondProductPriceRichTextBox);
            ThirdProductPriceRichTextBox.Text = _numberService.ChangeCommaToDotFromRichTextBoxText(ThirdProductPriceRichTextBox);
            FourthProductPriceRichTextBox.Text = _numberService.ChangeCommaToDotFromRichTextBoxText(FourthProductPriceRichTextBox);
            FifthProductPriceRichTextBox.Text = _numberService.ChangeCommaToDotFromRichTextBoxText(FifthProductPriceRichTextBox);
            SixthProductPriceRichTextBox.Text = _numberService.ChangeCommaToDotFromRichTextBoxText(SixthProductPriceRichTextBox);
            SeventhProductPriceRichTextBox.Text = _numberService.ChangeCommaToDotFromRichTextBoxText(SeventhProductPriceRichTextBox);
            EighthProductPriceRichTextBox.Text = _numberService.ChangeCommaToDotFromRichTextBoxText(EighthProductPriceRichTextBox);
            NinthProductPriceRichTextBox.Text = _numberService.ChangeCommaToDotFromRichTextBoxText(NinthProductPriceRichTextBox);
            TenProductPriceRichTextBox.Text = _numberService.ChangeCommaToDotFromRichTextBoxText(TenProductPriceRichTextBox);
            EleventhProductPriceRichTextBox.Text = _numberService.ChangeCommaToDotFromRichTextBoxText(EleventhProductPriceRichTextBox);
            TwelfthProductPriceRichTextBox.Text = _numberService.ChangeCommaToDotFromRichTextBoxText(TwelfthProductPriceRichTextBox);

            FirstProductTypeQuantityTextBox.Text =
                _numberService.ChangeCommaToDotFromTextBoxText(FirstProductTypeQuantityTextBox);
            SecondProductTypeQuantityTextBox.Text =
                _numberService.ChangeCommaToDotFromTextBoxText(SecondProductTypeQuantityTextBox);
            ThirdProductTypeQuantityTextBox.Text =
                _numberService.ChangeCommaToDotFromTextBoxText(ThirdProductTypeQuantityTextBox);
            FourthProductTypeQuantityTextBox.Text =
                _numberService.ChangeCommaToDotFromTextBoxText(FourthProductTypeQuantityTextBox);
            FifthProductTypeQuantityTextBox.Text =
                _numberService.ChangeCommaToDotFromTextBoxText(FifthProductTypeQuantityTextBox);

            FirstProductTypePriceTextBox.Text = _numberService.ChangeCommaToDotFromTextBoxText(FirstProductTypePriceTextBox);
            SecondProductTypePriceTextBox.Text = _numberService.ChangeCommaToDotFromTextBoxText(SecondProductTypePriceTextBox);
            ThirdProductTypePriceTextBox.Text = _numberService.ChangeCommaToDotFromTextBoxText(ThirdProductTypePriceTextBox);
            FourthProductTypePriceTextBox.Text = _numberService.ChangeCommaToDotFromTextBoxText(FourthProductTypePriceTextBox);
            FifthProductTypePriceTextBox.Text = _numberService.ChangeCommaToDotFromTextBoxText(FifthProductTypePriceTextBox);

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

            FirstProductTypeTextBox.MaxLength = FormSettings.TextBoxLengths.FirstProductType;
            SecondProductTypeTextBox.MaxLength = FormSettings.TextBoxLengths.SecondProductType;
            ThirdProductTypeTextBox.MaxLength = FormSettings.TextBoxLengths.ThirdProductType;
            FourthProductTypeTextBox.MaxLength = FormSettings.TextBoxLengths.FourthProductType;
            FifthProductTypeTextBox.MaxLength = FormSettings.TextBoxLengths.FifthProductType;
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
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

        private void SetCursorAtRichTextBoxStringEnd()
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

        private void SerCursorAtTextBoxStringEnd()
        {
            FirstProductTypeTextBox.SelectionStart = FirstProductTypeTextBox.Text.Length;
            SecondProductTypeTextBox.SelectionStart = SecondProductTypeTextBox.Text.Length;
            ThirdProductTypeTextBox.SelectionStart = ThirdProductTypeTextBox.Text.Length;
            FourthProductTypeTextBox.SelectionStart = FourthProductTypeTextBox.Text.Length;
            FifthProductTypeTextBox.SelectionStart = FifthProductTypeTextBox.Text.Length;

            FirstProductTypeQuantityTextBox.SelectionStart = FirstProductTypeQuantityTextBox.Text.Length;
            SecondProductTypeQuantityTextBox.SelectionStart = SecondProductTypeQuantityTextBox.Text.Length;
            ThirdProductTypeQuantityTextBox.SelectionStart = ThirdProductTypeQuantityTextBox.Text.Length;
            FourthProductTypeQuantityTextBox.SelectionStart = FourthProductTypeQuantityTextBox.Text.Length;
            FifthProductTypeQuantityTextBox.SelectionStart = FifthProductTypeQuantityTextBox.Text.Length;

            FirstProductTypePriceTextBox.SelectionStart = FirstProductTypePriceTextBox.Text.Length;
            SecondProductTypePriceTextBox.SelectionStart = SecondProductPriceRichTextBox.Text.Length;
            ThirdProductTypePriceTextBox.SelectionStart = ThirdProductTypePriceTextBox.Text.Length;
            FourthProductTypePriceTextBox.SelectionStart = FourthProductTypePriceTextBox.Text.Length;
            FifthProductTypePriceTextBox.SelectionStart = FifthProductTypePriceTextBox.Text.Length;
        }

        private void SetCursorAtDateTextBoxEnd()
        {
            InvoiceDateRichTextBox.SelectionStart = InvoiceDateRichTextBox.Text.Length;
        }


        #endregion
        
    }
}
