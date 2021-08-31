using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Invoice.Constants;
using Invoice.Enums;
using Invoice.Models;
using Invoice.Models.BuyersInfo;
using Invoice.Models.Deposit;
using Invoice.Models.MoneyReceipt;
using Invoice.Models.ProductInfo;
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
        private readonly ProductInfoRepository _productInfoRepository;
        private readonly BuyersInfoRepository _buyersInfoRepository;
        private readonly MoneyReceiptRepository _moneyReceiptRepository;
        private readonly DepositRepository _depositRepository;

        private readonly MessageDialogService _messageDialogService;
        private readonly NumberService _numberService;
        private readonly StringService _stringService;

        private readonly InvoiceOperations _invoiceOperations;
        private readonly int? _invoiceNumber;
        private int? _invoiceNumberYearCreation;
        private string _paymentStatus = "Nesumokėta";

        private Bitmap _invoiceMemoryImage;

        private const string DateFormat = "yyyy-MM-dd";

        private string[] _lastProductLineFilled = new string[12];
        private double?[] _lastQuantityValues;
        private int[] _idProductLinesValues;
        private int[] _idProductOldLineValues;
        private int _oldInvoiceYear;
        private int[] _lastFilledYearForProductInfo;

        private static readonly int[] ProductLineIndex = {0, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11};

        private string _lastBuyerFilled;

        private int _lastMoneyReceiptNumber;
        private int _countEmptyLinesForMoneyReceipt;

        public InvoiceForm(InvoiceOperations invoiceOperations, int? invoiceNumber = null,
            int? invoiceNumberYearCreation = null)
        {
            _invoiceRepository = new InvoiceRepository();
            _productTypeRepository = new ProductTypeRepository();
            _productInfoRepository = new ProductInfoRepository();
            _buyersInfoRepository = new BuyersInfoRepository();
            _moneyReceiptRepository = new MoneyReceiptRepository();
            _depositRepository = new DepositRepository();

            _numberService = new NumberService();
            _messageDialogService = new MessageDialogService();
            _stringService = new StringService();

            _invoiceOperations = invoiceOperations;
            _invoiceNumber = invoiceNumber;
            _invoiceNumberYearCreation = invoiceNumberYearCreation;

            ResolveFormOperationDesign();

            InitializeComponent();

            SetTextBoxLengths();

            SetControlInitialState();
        }

        private void InvoiceForm_Load(object sender, EventArgs e)
        {
            ResolveInvoiceNumberText();
            LoadFormDataForEditOrCopy();
            SetCursorAtDateTextBoxEnd();
            FillAllProductComboBoxes();
            FillBuyerComboBox();
            FillDefaultSellerInfoForNewInvoice();
            LoadSuggestedMoneyReceiptNumber();
            LoadInvoiceControlYearTextBox();
            FillDepositInfoToEditInvoiceOperation();
            _lastFilledYearForProductInfo = new int[12];
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
                    $" Raudonas langelis Negali būti tuščias! pvz.: {DateTime.Now.ToString(DateFormat)}",
                    InvoiceDateRichTextBox,
                    ErrorMassageLabel);
            }
            else if (!DateTime.TryParseExact(InvoiceDateRichTextBox.Text, DateFormat, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out _))
            {
                SaveButton.Enabled = false;
                e.Cancel = true;
                _messageDialogService.DisplayLabelAndTextBoxError(
                    $"Raudoname langelyje Įveskite teisingą datą! pvz.: {DateTime.Now.ToString(DateFormat)}",
                    InvoiceDateRichTextBox,
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

            if (_invoiceOperations == InvoiceOperations.Edit && _invoiceNumber.HasValue &&
                _invoiceNumberYearCreation.HasValue)
            {
                isSuccess = _invoiceRepository.UpdateExistingInvoice(_invoiceNumber.Value,
                    _invoiceNumberYearCreation.Value, invoiceModel);
                successMessage = "Sąskaita faktūra atnaujinta sekmingai";

                if (isSuccess)
                {
                    bool isAllQuantityFilled = CheckIsAllProductTypeQuantityFilledByInvoiceProductQuantity();
                    SuggestToFillProductTypeQuantityIfEmpty(isAllQuantityFilled);
                    GetAllProductTypeForNewInvoice();

                    SaveProductInfoId();

                    SaveNewProductsQuantityValuesForEditInvoice();

                    _messageDialogService.ShowInfoMessage(successMessage);
                    this.Close();
                }
                else
                {
                    _messageDialogService.ShowErrorMassage("nepavyko išsaugot bandykit dar kartą");
                }
            }
            else
            {
                if (!_invoiceNumberYearCreation.HasValue) return;

                isSuccess = _invoiceRepository.CreateNewInvoice(invoiceModel, _invoiceNumberYearCreation.Value);
                successMessage = "Nauja Sąskaita faktūra sukurta";

                if (isSuccess)
                {
                    bool isAllQuantityFilled = CheckIsAllProductTypeQuantityFilledByInvoiceProductQuantity();
                    SuggestToFillProductTypeQuantityIfEmpty(isAllQuantityFilled);
                    GetAllProductTypeForNewInvoice();

                    SaveProductInfoId();
                    AddQuantityFromNewInvoiceToDeposit();

                    _messageDialogService.ShowInfoMessage(successMessage);
                    this.Close();
                }
                else
                {
                    _messageDialogService.ShowErrorMassage("nepavyko išsaugot bandykit dar kartą");
                }
            }
        }

        private void SaveToPdf_Click(object sender, EventArgs e)
        {
            CalculateButton_Click(this, new EventArgs());

            DialogResult dialogResult =
                _messageDialogService.ShowChoiceMessage("Ar norite suformuoti Sąskaita ir kvitą");

            if (dialogResult == DialogResult.OK)
            {
                AddOneToMoneyReceiptSuggestedNumber();

                SaveInvoiceAndMoneyReceiptToToPdf();
                _messageDialogService.ShowInfoMessage("Sąskaitos faktūra ir kvitas išsaugota į pdf failą");
            }
            else
            {
                SaveInvoiceToPdf();
                _messageDialogService.ShowInfoMessage("Sąskaita faktūra išsaugota į pdf failą");
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
            DialogResult dialogResult = _messageDialogService.ShowChoiceMessage(
                "Ar norite spausdinti kvitą (jei paspausit 'OK' spausdins kvitą jei 'Cancel' spausdins Sąskaitą faktūrą) ?");

            if (dialogResult == DialogResult.OK)
            {
                PrintMoneyReceiptForm();
            }
            else
            {
                PrintInvoiceForm();
            }
        }

        private void MoneyReceiptOfferNumberTextBox_Validating(object sender, CancelEventArgs e)
        {
            bool isNumber = int.TryParse(MoneyReceiptOfferNumberTextBox.Text, out int number);

            if (string.IsNullOrWhiteSpace(MoneyReceiptOfferNumberTextBox.Text))
            {
                SaveMoneyReceiptSuggestionNumberButton.Enabled = false;
                SaveToPdf.Enabled = false;
                e.Cancel = true;

                _messageDialogService.DisplayLabelAndTextBoxError(
                    $"raudonas langelis negali būt tuščias, turi būt sveikas skaičius, negali būt lygus arba mažesnis nei 0 pvz ",
                    MoneyReceiptOfferNumberTextBox, ErrorMassageLabel);

                MoneyReceiptOfferNumberTextBox.SelectionStart = MoneyReceiptOfferNumberTextBox.Text.Length;
            }
            else if (isNumber && number <= 0)
            {
                SaveMoneyReceiptSuggestionNumberButton.Enabled = false;
                SaveToPdf.Enabled = false;
                e.Cancel = true;

                _messageDialogService.DisplayLabelAndTextBoxError(
                    $"raudonas langelis negali būt lygus arba mažesnis nei 0, pvz ", MoneyReceiptOfferNumberTextBox,
                    ErrorMassageLabel);

                MoneyReceiptOfferNumberTextBox.SelectionStart = MoneyReceiptOfferNumberTextBox.Text.Length;
            }
            else if (!isNumber)
            {
                SaveMoneyReceiptSuggestionNumberButton.Enabled = false;
                SaveToPdf.Enabled = false;
                e.Cancel = true;

                _messageDialogService.DisplayLabelAndTextBoxError(
                    $"raudonas langelis turi būti sveikas skaičius negali būt lygus arba mažesnis nei 0, pvz ",
                    MoneyReceiptOfferNumberTextBox, ErrorMassageLabel);

                MoneyReceiptOfferNumberTextBox.SelectionStart = MoneyReceiptOfferNumberTextBox.Text.Length;
            }
            else
            {
                SaveMoneyReceiptSuggestionNumberButton.Enabled = true;
                SaveToPdf.Enabled = true;
                e.Cancel = false;
                _messageDialogService.HideLabelAndTextBoxError(ErrorMassageLabel, MoneyReceiptOfferNumberTextBox);
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
                    this.SelectNextControl((Control) sender, true, true, true, true);
                }

                SetCursorAtTextBoxStringEnd();
            }
        }

        private void ProductTypeTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl((Control) sender, true, true, true, true);
                SetCursorAtProductTypeStringEnd();
            }
        }

        private void InvoiceRichTextBoxString_TextChanged(object sender, EventArgs e)
        {
            RichTextBox richTextBox = sender as RichTextBox;

            if (richTextBox == null) return;

            if (richTextBox.SelectionStart == richTextBox.MaxLength)
            {
                _messageDialogService.ShowInfoMessage(
                    $"Pasiektas maksimalus žodžių ilgis bus išsaugota tik toks tekstas ({richTextBox.Text}) ");
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

        private void AddToFirstProductInfoButton_Click(object sender, EventArgs e)
        {
            if (_invoiceNumberYearCreation != null && FirstProductNameComboBox.Text == _lastProductLineFilled[ProductLineIndex[1]] && _invoiceNumberYearCreation.Value == _lastFilledYearForProductInfo[ProductLineIndex[1]]) return;

            _lastProductLineFilled[ProductLineIndex[1]] = FirstProductNameComboBox.Text;
            FillSpecificProductLineTextBox(InvoiceProductLine.First, FirstProductNameComboBox.Text);

        }

        private void AddToSecondProductInfoButton_Click(object sender, EventArgs e)
        {
            if (_invoiceNumberYearCreation != null && SecondProductNameComboBox.Text == _lastProductLineFilled[ProductLineIndex[2]] && _invoiceNumberYearCreation.Value == _lastFilledYearForProductInfo[ProductLineIndex[2]]) return;

            _lastProductLineFilled[ProductLineIndex[2]] = SecondProductNameComboBox.Text;
            FillSpecificProductLineTextBox(InvoiceProductLine.Second, SecondProductNameComboBox.Text);
        }

        private void AddToThirdProductInfoButton_Click(object sender, EventArgs e)
        {
            if (_invoiceNumberYearCreation != null && ThirdProductNameComboBox.Text == _lastProductLineFilled[ProductLineIndex[3]] && _invoiceNumberYearCreation.Value == _lastFilledYearForProductInfo[ProductLineIndex[3]]) return;

            _lastProductLineFilled[ProductLineIndex[3]] = ThirdProductNameComboBox.Text;
            FillSpecificProductLineTextBox(InvoiceProductLine.Third, ThirdProductNameComboBox.Text);
        }

        private void AddToFourthProductInfoButton_Click(object sender, EventArgs e)
        {
            if (_invoiceNumberYearCreation != null && FourthProductNameComboBox.Text == _lastProductLineFilled[ProductLineIndex[4]] && _invoiceNumberYearCreation.Value == _lastFilledYearForProductInfo[ProductLineIndex[4]]) return;

            _lastProductLineFilled[ProductLineIndex[4]] = FourthProductNameComboBox.Text;
            FillSpecificProductLineTextBox(InvoiceProductLine.Fourth, FourthProductNameComboBox.Text);
        }

        private void AddToFifthProductInfoButton_Click(object sender, EventArgs e)
        {

            if (_invoiceNumberYearCreation != null && FifthProductNameComboBox.Text == _lastProductLineFilled[ProductLineIndex[5]] && _invoiceNumberYearCreation.Value == _lastFilledYearForProductInfo[ProductLineIndex[5]]) return;

            _lastProductLineFilled[ProductLineIndex[5]] = FifthProductNameComboBox.Text;
            FillSpecificProductLineTextBox(InvoiceProductLine.Fifth, FifthProductNameComboBox.Text);
        }

        private void AddToSixthProductInfoButton_Click(object sender, EventArgs e)
        {
            if (_invoiceNumberYearCreation != null && SixthProductNameComboBox.Text == _lastProductLineFilled[ProductLineIndex[6]] && _invoiceNumberYearCreation.Value == _lastFilledYearForProductInfo[ProductLineIndex[6]]) return;

            _lastProductLineFilled[ProductLineIndex[6]] = SixthProductNameComboBox.Text;
            FillSpecificProductLineTextBox(InvoiceProductLine.Sixth, SixthProductNameComboBox.Text);
        }

        private void AddToSeventhProductInfoButton_Click(object sender, EventArgs e)
        {
            if (_invoiceNumberYearCreation != null && SeventhProductNameComboBox.Text == _lastProductLineFilled[ProductLineIndex[7]] && _invoiceNumberYearCreation.Value == _lastFilledYearForProductInfo[ProductLineIndex[7]]) return;

            _lastProductLineFilled[ProductLineIndex[7]] = SeventhProductNameComboBox.Text;
            FillSpecificProductLineTextBox(InvoiceProductLine.Seventh, SeventhProductNameComboBox.Text);
        }

        private void AddToEighthProductInfoButton_Click(object sender, EventArgs e)
        {
            if (_invoiceNumberYearCreation != null && EighthProductNameComboBox.Text == _lastProductLineFilled[ProductLineIndex[8]] && _invoiceNumberYearCreation.Value == _lastFilledYearForProductInfo[ProductLineIndex[8]]) return;

            _lastProductLineFilled[ProductLineIndex[8]] = EighthProductNameComboBox.Text;
            FillSpecificProductLineTextBox(InvoiceProductLine.Eighth, EighthProductNameComboBox.Text);
        }

        private void AddToNinthProductInfoButton_Click(object sender, EventArgs e)
        {
            if (_invoiceNumberYearCreation != null && NinthProductNameComboBox.Text == _lastProductLineFilled[ProductLineIndex[9]] && _invoiceNumberYearCreation.Value == _lastFilledYearForProductInfo[ProductLineIndex[9]]) return;

            _lastProductLineFilled[ProductLineIndex[9]] = NinthProductNameComboBox.Text;
            FillSpecificProductLineTextBox(InvoiceProductLine.Ninth, NinthProductNameComboBox.Text);
        }

        private void AddToTenProductInfoButton_Click(object sender, EventArgs e)
        {
            if (_invoiceNumberYearCreation != null && TenProductNameComboBox.Text == _lastProductLineFilled[ProductLineIndex[10]] && _invoiceNumberYearCreation.Value == _lastFilledYearForProductInfo[ProductLineIndex[10]]) return;

            _lastProductLineFilled[ProductLineIndex[10]] = TenProductNameComboBox.Text;
            FillSpecificProductLineTextBox(InvoiceProductLine.Ten, TenProductNameComboBox.Text);
        }

        private void AddToEleventhProductInfoButton_Click(object sender, EventArgs e)
        {
            if (_invoiceNumberYearCreation != null && EleventhProductNameComboBox.Text == _lastProductLineFilled[ProductLineIndex[11]] && _invoiceNumberYearCreation.Value == _lastFilledYearForProductInfo[ProductLineIndex[11]]) return;

            _lastProductLineFilled[ProductLineIndex[11]] = EleventhProductNameComboBox.Text;
            FillSpecificProductLineTextBox(InvoiceProductLine.Eleventh, EleventhProductNameComboBox.Text);
        }

        private void AddToTwelfthProductInfoButton_Click(object sender, EventArgs e)
        {
            if (_invoiceNumberYearCreation != null && TwelfthProductNameComboBox.Text == _lastProductLineFilled[ProductLineIndex[12]] && _invoiceNumberYearCreation.Value == _lastFilledYearForProductInfo[ProductLineIndex[12]]) return;

            _lastProductLineFilled[ProductLineIndex[12]] = TwelfthProductNameComboBox.Text;
            FillSpecificProductLineTextBox(InvoiceProductLine.Twelfth, TwelfthProductNameComboBox.Text);
        }

        private void AddBuyerInfoButton_Click(object sender, EventArgs e)
        {
            FillBuyerInfo();
        }

        private void SaveMoneyReceiptSuggestionNumberButton_Click(object sender, EventArgs e)
        {
            int moneyReceiptNewNumber = int.Parse(MoneyReceiptOfferNumberTextBox.Text);

            if (_lastMoneyReceiptNumber == moneyReceiptNewNumber)
            {
                _messageDialogService.ShowInfoMessage(
                    "Siūlomas Kvito skaičius yra vienodas duomenų bazėje todėl nebus išsaugotas");
                return;
            }

            bool isUpdated = _moneyReceiptRepository.UpdateMoneyReceiptSuggestedNumber(moneyReceiptNewNumber);

            if (isUpdated)
            {
                _lastMoneyReceiptNumber = moneyReceiptNewNumber;
                _messageDialogService.ShowInfoMessage("Naujas Kvito numeris išsaugotas sekmingai");
            }
            else
            {
                _messageDialogService.ShowErrorMassage("Neišsisaugojo kreiptis į administratorių");
            }
        }

        private void InvoiceYearControlTextBox_Validating(object sender, CancelEventArgs e)
        {
            bool isNumber = int.TryParse(InvoiceYearControlTextBox.Text, out int number);

            if (string.IsNullOrWhiteSpace(InvoiceYearControlTextBox.Text))
            {
                SaveButton.Enabled = false;
                e.Cancel = true;
                _messageDialogService.DisplayLabelAndTextBoxError(
                    $" Raudonas langelis Negali būti tuščias! pvz.: {DateTime.Now.Year}", InvoiceYearControlTextBox,
                    ErrorMassageLabel);
            }
            else if (!isNumber)
            {
                SaveButton.Enabled = false;
                e.Cancel = true;
                _messageDialogService.DisplayLabelAndTextBoxError(
                    $" Raudonas langelis turi būti metai pvz.: {DateTime.Now.Year}", InvoiceYearControlTextBox,
                    ErrorMassageLabel);
            }
            else if (number > DateTime.Now.Year)
            {
                SaveButton.Enabled = false;
                e.Cancel = true;
                _messageDialogService.DisplayLabelAndTextBoxError(
                    $" Raudonam langelį metai negali būti ateityje pvz.: {DateTime.Now.Year}",
                    InvoiceYearControlTextBox,
                    ErrorMassageLabel);
            }
            else if (number < 2000)
            {
                SaveButton.Enabled = false;
                e.Cancel = true;
                _messageDialogService.DisplayLabelAndTextBoxError(
                    $" Raudonam langelyje negali būti mažiau nei 2000 pvz.: {DateTime.Now.Year}",
                    InvoiceYearControlTextBox,
                    ErrorMassageLabel);
            }
            else
            {
                SaveButton.Enabled = true;
                e.Cancel = false;
                _messageDialogService.HideLabelAndTextBoxError(ErrorMassageLabel, InvoiceYearControlTextBox);
            }
        }

        private void InvoiceYearControlTextBox_TextChanged(object sender, EventArgs e)
        {
            bool isNumber = int.TryParse(InvoiceYearControlTextBox.Text, out int number);

            if (InvoiceYearControlTextBox.Text.Length == InvoiceYearControlTextBox.MaxLength && isNumber)
            {
                _invoiceNumberYearCreation = number;
            }
        }

        #region Helpers

        private void SaveInvoiceAndMoneyReceiptToToPdf()
        {
            CaptureInvoiceFormScreen();

            PdfWriter newInvoicePdfWriter =
                new PdfWriter(
                    $"{AppConfiguration.PdfFolder}\\Saskaita faktura  nr.{InvoiceNumberRichTextBox.Text} {BuyerNameRichTextBox.Text} ir kvitas nr.{MoneyReceiptOfferNumberTextBox.Text}.pdf");
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

        private void SaveInvoiceToPdf()
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
            string[] allProducts = FillProductsToArray();

            _countEmptyLinesForMoneyReceipt = _numberService.CountEmptyStrings(allProducts);

            allProducts = allProducts.Where(p => !string.IsNullOrEmpty(p)).ToArray();

            string filledProducts = _stringService.MakeFormatFilledProducts(allProducts);

            MoneyReceiptModel moneyReceiptInfo = new MoneyReceiptModel()
            {
                SellerInfo = SellerNameRichTextBox.Text,
                SellerFirmCode = SellerFirmCodeRichTextBox.Text,
                SerialNumber = SerialNumberRichTextBox.Text,
                MoneyReceiptOfferNumber = MoneyReceiptOfferNumberTextBox.Text,
                InvoiceDate = InvoiceDateRichTextBox.Text,
                AllProducts = $@"{filledProducts}",

                PriceInWords = PriceInWordsRichTextBox.Text,
                InvoiceMaker = InvoiceMakerRichTextBox.Text
            };

            var moneyReceiptForm = new MoneyReceiptForm(_countEmptyLinesForMoneyReceipt);

            moneyReceiptForm.Show();
            moneyReceiptForm.Hide();

            _invoiceMemoryImage = moneyReceiptForm.SaveMoneyReceiptForm(moneyReceiptInfo);
            printPreviewDialog.Document = printDocument;
            printPreviewDialog.ShowDialog();

            moneyReceiptForm.Close();
        }

        private void SaveMoneyReceiptFormToPdf(Document newInvoiceDocument)
        {
            string[] allProducts = FillProductsToArray();

            _countEmptyLinesForMoneyReceipt = _numberService.CountEmptyStrings(allProducts);

            allProducts = allProducts.Where(p => !string.IsNullOrEmpty(p)).ToArray();

            string filledProducts = _stringService.MakeFormatFilledProducts(allProducts);

            MoneyReceiptModel moneyReceiptInfo = new MoneyReceiptModel()
            {
                SellerInfo = SellerNameRichTextBox.Text,
                SellerFirmCode = SellerFirmCodeRichTextBox.Text,
                SerialNumber = SerialNumberRichTextBox.Text,
                MoneyReceiptOfferNumber = MoneyReceiptOfferNumberTextBox.Text,
                InvoiceDate = InvoiceDateRichTextBox.Text,
                AllProducts = $@"{filledProducts}",

                PriceInWords = PriceInWordsRichTextBox.Text,
                InvoiceMaker = InvoiceMakerRichTextBox.Text
            };

            var moneyReceiptForm = new MoneyReceiptForm(_countEmptyLinesForMoneyReceipt);

            moneyReceiptForm.Show();

            var convertMoneyReceiptImageToByteArray =
                ConvertImageToByteArray(moneyReceiptForm.SaveMoneyReceiptForm(moneyReceiptInfo));

            var newMoneyReceiptImage =
                new iText.Layout.Element.Image(ImageDataFactory.Create(convertMoneyReceiptImageToByteArray))
                    .SetTextAlignment(
                        TextAlignment.CENTER);

            newInvoiceDocument.Add(newMoneyReceiptImage);
            moneyReceiptForm.Close();
        }

        private string CheckProductsRichTextBox(RichTextBox productNameRichTextBox,
            RichTextBox productQuantityRichTextBox, RichTextBox productSeesRichTextBox,
            RichTextBox productPriceRichTextBox)
        {
            string productInfo;

            if (string.IsNullOrWhiteSpace(productNameRichTextBox.Text) ||
                string.IsNullOrWhiteSpace(productQuantityRichTextBox.Text) ||
                string.IsNullOrWhiteSpace(productSeesRichTextBox.Text) ||
                string.IsNullOrWhiteSpace(productPriceRichTextBox.Text))
            {
                productInfo = string.Empty;
            }
            else
            {
                double priceWithPvm = _numberService.CalculateProductPriceWithPvm(productPriceRichTextBox);
                productInfo =
                    $"{productNameRichTextBox.Text}, {productQuantityRichTextBox.Text} {productSeesRichTextBox.Text}, {priceWithPvm} EUR. ";
            }

            return productInfo;
        }

        private string[] FillProductsToArray()
        {
            string firstProductInfo = CheckProductsRichTextBox(FirstProductNameRichTextBox,
                FirstProductQuantityRichTextBox,
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
            string twelfthProductInfo = CheckProductsRichTextBox(TwelfthProductNameRichTextBox,
                TwelfthProductQuantityRichTextBox, TwelfthProductSeesRichTextBox, TwelfthProductPriceRichTextBox);

            string[] allProducts = new string[]
            {
                firstProductInfo,
                secondProductInfo,
                thirdProductInfo,
                forthProductInfo,
                fifthProductInfo,
                sixthProductInfo,
                seventhProductInfo,
                eighthProductInfo,
                ninthProductInfo,
                tenProductInfo,
                eleventhProductInfo,
                twelfthProductInfo
            };

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
            SetComboBoxesDropDown();
        }

        private void SetComboBoxesDropDown()
        {
            FirstProductNameComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            SecondProductNameComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            ThirdProductNameComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            FourthProductNameComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            FifthProductNameComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            SixthProductNameComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            SeventhProductNameComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            EighthProductNameComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            NinthProductNameComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            TenProductNameComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            EleventhProductNameComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            TwelfthProductNameComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            BuyerInfoNameComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
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

            MoneyReceiptOfferNumberTextBox.MaxLength = FormSettings.TextBoxLengths.MaxNumberLength;
            InvoiceYearControlTextBox.MaxLength = FormSettings.TextBoxLengths.InvoiceYearControl;
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
            return (byte[]) converter.ConvertTo(img, typeof(byte[]));
        }

        private void SetCursorAtTextBoxStringEnd()
        {
            SerialNumberRichTextBox.SelectionStart = SerialNumberRichTextBox.Text.Length;
            InvoiceDateRichTextBox.SelectionStart = InvoiceDateRichTextBox.Text.Length;

            SellerNameRichTextBox.SelectionStart = SellerNameRichTextBox.Text.Length;
            SellerFirmCodeRichTextBox.SelectionStart = SellerFirmCodeRichTextBox.Text.Length;
            SellerPvmCodeRichTextBox.SelectionStart = SellerPvmCodeRichTextBox.Text.Length;
            SellerAddressRichTextBox.SelectionStart = SellerAddressRichTextBox.Text.Length;
            SellerPhoneNumberRichTextBox.SelectionStart = SellerPhoneNumberRichTextBox.Text.Length;
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
                    _productTypeRepository.GetExistingProductType(_invoiceNumber.Value,
                        _invoiceNumberYearCreation.Value);

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

                    SecondProductTypePriceTextBox.Text = _numberService.ToStringProductTypePriceOrEmpty(
                        ProductTypePriceOperations.SecondProductTypePrice,
                        getExistingProductTypes);

                    ThirdProductTypePriceTextBox.Text = _numberService.ToStringProductTypePriceOrEmpty(
                        ProductTypePriceOperations.ThirdProductTypePrice,
                        getExistingProductTypes);

                    FourthProductTypePriceTextBox.Text = _numberService.ToStringProductTypePriceOrEmpty(
                        ProductTypePriceOperations.FourthProductTypePrice,
                        getExistingProductTypes);

                    FifthProductTypePriceTextBox.Text = _numberService.ToStringProductTypePriceOrEmpty(
                        ProductTypePriceOperations.FifthProductTypePrice,
                        getExistingProductTypes);

                    SixthProductTypePriceTextBox.Text = _numberService.ToStringProductTypePriceOrEmpty(
                        ProductTypePriceOperations.SixthProductTypePrice,
                        getExistingProductTypes);

                    SeventhProductTypePriceTextBox.Text = _numberService.ToStringProductTypePriceOrEmpty(
                        ProductTypePriceOperations.SeventhProductTypePrice,
                        getExistingProductTypes);

                    EighthProductTypePriceTextBox.Text = _numberService.ToStringProductTypePriceOrEmpty(
                        ProductTypePriceOperations.EighthProductTypePrice,
                        getExistingProductTypes);

                    NinthProductTypePriceTextBox.Text = _numberService.ToStringProductTypePriceOrEmpty(
                        ProductTypePriceOperations.NinthProductTypePrice,
                        getExistingProductTypes);
                    TenProductTypePriceTextBox.Text = _numberService.ToStringProductTypePriceOrEmpty(
                        ProductTypePriceOperations.TenProductTypePrice,
                        getExistingProductTypes);

                    EleventhProductTypePriceTextBox.Text = _numberService.ToStringProductTypePriceOrEmpty(
                        ProductTypePriceOperations.EleventhProductTypePrice,
                        getExistingProductTypes);

                    TwelfthProductTypePriceTextBox.Text = _numberService.ToStringProductTypePriceOrEmpty(
                        ProductTypePriceOperations.TwelfthProductTypePrice,
                        getExistingProductTypes);
                }
            }
        }

        private void GetAllProductTypeForNewInvoice()
        {
            ChangeProductTypeQuantityAndPriceFromCommaToDot();
            ProductTypeModel productType = GetAllInfoFromProductTypeTextBox();

            if (_invoiceOperations == InvoiceOperations.Edit && _invoiceNumber.HasValue &&
                _invoiceNumberYearCreation.HasValue)
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
            EleventhProductTypeQuantityTextBox.Text =
                _numberService.ChangeCommaToDot(EleventhProductTypeQuantityTextBox);
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

        private void FillAllProductComboBoxes()
        {
            List<ProductInfoNameModel> getAllProductInfoNames = _productInfoRepository.GetAllProductsNames().ToList();

            SetComboBoxDataSource(FirstProductNameComboBox, getAllProductInfoNames);
            SetComboBoxDataSource(SecondProductNameComboBox, getAllProductInfoNames);
            SetComboBoxDataSource(ThirdProductNameComboBox, getAllProductInfoNames);
            SetComboBoxDataSource(FourthProductNameComboBox, getAllProductInfoNames);
            SetComboBoxDataSource(FifthProductNameComboBox, getAllProductInfoNames);
            SetComboBoxDataSource(SixthProductNameComboBox, getAllProductInfoNames);
            SetComboBoxDataSource(SeventhProductNameComboBox, getAllProductInfoNames);
            SetComboBoxDataSource(EighthProductNameComboBox, getAllProductInfoNames);
            SetComboBoxDataSource(NinthProductNameComboBox, getAllProductInfoNames);
            SetComboBoxDataSource(TenProductNameComboBox, getAllProductInfoNames);
            SetComboBoxDataSource(EleventhProductNameComboBox, getAllProductInfoNames);
            SetComboBoxDataSource(TwelfthProductNameComboBox, getAllProductInfoNames);
        }

        private void SetComboBoxDataSource(ComboBox comboBox, List<ProductInfoNameModel> getAllProductInfoNames)
        {
            comboBox.BindingContext = new BindingContext();
            comboBox.DataSource = getAllProductInfoNames;
            comboBox.DisplayMember = "ProductName";
        }

        private void FillSpecificProductLineTextBox(InvoiceProductLine productLine, string productName)
        {
            if (!_invoiceNumberYearCreation.HasValue)return;
           
            FullProductInfoWithId productInfo = _productInfoRepository.GetFullProductInfoWithId(productName, _invoiceNumberYearCreation.Value);

            if (productInfo != null)
            {
                FillProductLine(productLine, productInfo);
            }
            else
            {
                _messageDialogService.ShowErrorMassage(
                    "Nėra jokios informacijos duomenų bazėje apie galimus produktus arba informacija produkto ne tų metų");
            }
        }

        private void FillProductLine(InvoiceProductLine productLine, FullProductInfoWithId productInfo)
        {
            switch (productLine)
            {
                case InvoiceProductLine.First:
                    FirstProductIdTextBox.Text = productInfo.Id.ToString();

                    FillProductNameTextBoxWithBarCode(productInfo, FirstProductNameRichTextBox);
                    FirstProductSeesRichTextBox.Text = productInfo.ProductSees;
                    FirstProductPriceRichTextBox.Text = productInfo.ProductPrice.ToString();

                    FirstProductTypeTextBox.Text = productInfo.ProductType;
                    FirstProductTypePriceTextBox.Text = productInfo.ProductTypePrice.ToString();
                    if (_invoiceNumberYearCreation != null)
                        _lastFilledYearForProductInfo[ProductLineIndex[1]] = _invoiceNumberYearCreation.Value;
                    break;

                case InvoiceProductLine.Second:
                    SecondProductIdTextBox.Text = productInfo.Id.ToString();

                    FillProductNameTextBoxWithBarCode(productInfo, SecondProductNameRichTextBox);
                    SecondProductSeesRichTextBox.Text = productInfo.ProductSees;
                    SecondProductPriceRichTextBox.Text = productInfo.ProductPrice.ToString();

                    SecondProductTypeTextBox.Text = productInfo.ProductType;
                    SecondProductTypePriceTextBox.Text = productInfo.ProductPrice.ToString();
                    if (_invoiceNumberYearCreation != null)
                        _lastFilledYearForProductInfo[ProductLineIndex[2]] = _invoiceNumberYearCreation.Value;
                    break;

                case InvoiceProductLine.Third:
                    ThirdProductIdTextBox.Text = productInfo.Id.ToString();

                    FillProductNameTextBoxWithBarCode(productInfo, ThirdProductNameRichTextBox);
                    ThirdProductSeesRichTextBox.Text = productInfo.ProductSees;
                    ThirdProductPriceRichTextBox.Text = productInfo.ProductPrice.ToString();

                    ThirdProductTypeTextBox.Text = productInfo.ProductType;
                    ThirdProductTypePriceTextBox.Text = productInfo.ProductTypePrice.ToString();
                    if (_invoiceNumberYearCreation != null)
                        _lastFilledYearForProductInfo[ProductLineIndex[3]] = _invoiceNumberYearCreation.Value;
                    break;

                case InvoiceProductLine.Fourth:
                    FourthProductIdTextBox.Text = productInfo.Id.ToString();

                    FillProductNameTextBoxWithBarCode(productInfo, FourthProductNameRichTextBox);
                    FourthProductSeesRichTextBox.Text = productInfo.ProductSees;
                    FourthProductPriceRichTextBox.Text = productInfo.ProductPrice.ToString();

                    FourthProductTypeTextBox.Text = productInfo.ProductType;
                    FourthProductTypePriceTextBox.Text = productInfo.ProductTypePrice.ToString();
                    if (_invoiceNumberYearCreation != null)
                        _lastFilledYearForProductInfo[ProductLineIndex[4]] = _invoiceNumberYearCreation.Value;
                    break;

                case InvoiceProductLine.Fifth:
                    FifthProductIdTextBox.Text = productInfo.Id.ToString();

                    FillProductNameTextBoxWithBarCode(productInfo, FifthProductNameRichTextBox);
                    FifthProductSeesRichTextBox.Text = productInfo.ProductSees;
                    FifthProductPriceRichTextBox.Text = productInfo.ProductPrice.ToString();

                    FifthProductTypeTextBox.Text = productInfo.ProductType;
                    FifthProductTypePriceTextBox.Text = productInfo.ProductTypePrice.ToString();
                    if (_invoiceNumberYearCreation != null)
                        _lastFilledYearForProductInfo[ProductLineIndex[5]] = _invoiceNumberYearCreation.Value;
                    break;

                case InvoiceProductLine.Sixth:
                    SixthProductIdTextBox.Text = productInfo.Id.ToString();

                    FillProductNameTextBoxWithBarCode(productInfo, SixthProductNameRichTextBox);
                    SixthProductSeesRichTextBox.Text = productInfo.ProductSees;
                    SixthProductPriceRichTextBox.Text = productInfo.ProductPrice.ToString();

                    SixthProductTypeTextBox.Text = productInfo.ProductType;
                    SixthProductTypePriceTextBox.Text = productInfo.ProductTypePrice.ToString();
                    if (_invoiceNumberYearCreation != null)
                        _lastFilledYearForProductInfo[ProductLineIndex[6]] = _invoiceNumberYearCreation.Value;
                    break;

                case InvoiceProductLine.Seventh:
                    SeventhProductIdTextBox.Text = productInfo.Id.ToString();

                    FillProductNameTextBoxWithBarCode(productInfo, SeventhProductNameRichTextBox);
                    SeventhProductSeesRichTextBox.Text = productInfo.ProductSees;
                    SeventhProductPriceRichTextBox.Text = productInfo.ProductPrice.ToString();

                    SeventhProductTypeTextBox.Text = productInfo.ProductType;
                    SeventhProductTypePriceTextBox.Text = productInfo.ProductTypePrice.ToString();
                    if (_invoiceNumberYearCreation != null)
                        _lastFilledYearForProductInfo[ProductLineIndex[7]] = _invoiceNumberYearCreation.Value;
                    break;

                case InvoiceProductLine.Eighth:
                    EighthProductIdTextBox.Text = productInfo.Id.ToString();

                    FillProductNameTextBoxWithBarCode(productInfo, EighthProductNameRichTextBox);
                    EighthProductSeesRichTextBox.Text = productInfo.ProductSees;
                    EighthProductPriceRichTextBox.Text = productInfo.ProductPrice.ToString();

                    EighthProductTypeTextBox.Text = productInfo.ProductType;
                    EighthProductTypePriceTextBox.Text = productInfo.ProductTypePrice.ToString();
                    if (_invoiceNumberYearCreation != null)
                        _lastFilledYearForProductInfo[ProductLineIndex[8]] = _invoiceNumberYearCreation.Value;
                    break;

                case InvoiceProductLine.Ninth:
                    NinthProductIdTextBox.Text = productInfo.Id.ToString();

                    FillProductNameTextBoxWithBarCode(productInfo, NinthProductNameRichTextBox);
                    NinthProductSeesRichTextBox.Text = productInfo.ProductSees;
                    NinthProductPriceRichTextBox.Text = productInfo.ProductPrice.ToString();

                    NinthProductTypeTextBox.Text = productInfo.ProductType;
                    NinthProductTypePriceTextBox.Text = productInfo.ProductTypePrice.ToString();
                    if (_invoiceNumberYearCreation != null)
                        _lastFilledYearForProductInfo[ProductLineIndex[9]] = _invoiceNumberYearCreation.Value;
                    break;

                case InvoiceProductLine.Ten:
                    TenProductIdTextBox.Text = productInfo.Id.ToString();

                    FillProductNameTextBoxWithBarCode(productInfo, TenProductNameRichTextBox);
                    TenProductSeesRichTextBox.Text = productInfo.ProductSees;
                    TenProductPriceRichTextBox.Text = productInfo.ProductPrice.ToString();

                    TenProductTypeTextBox.Text = productInfo.ProductType;
                    TenProductTypePriceTextBox.Text = productInfo.ProductTypePrice.ToString();
                    if (_invoiceNumberYearCreation != null)
                        _lastFilledYearForProductInfo[ProductLineIndex[10]] = _invoiceNumberYearCreation.Value;
                    break;

                case InvoiceProductLine.Eleventh:
                    EleventhProductIdTextBox.Text = productInfo.Id.ToString();

                    FillProductNameTextBoxWithBarCode(productInfo, EleventhProductNameRichTextBox);
                    EleventhProductSeesRichTextBox.Text = productInfo.ProductSees;
                    EleventhProductPriceRichTextBox.Text = productInfo.ProductPrice.ToString();

                    EleventhProductTypeTextBox.Text = productInfo.ProductType;
                    EleventhProductTypePriceTextBox.Text = productInfo.ProductTypePrice.ToString();
                    if (_invoiceNumberYearCreation != null)
                        _lastFilledYearForProductInfo[ProductLineIndex[11]] = _invoiceNumberYearCreation.Value;
                    break;

                case InvoiceProductLine.Twelfth:
                    TwelfthProductIdTextBox.Text = productInfo.Id.ToString();

                    FillProductNameTextBoxWithBarCode(productInfo, TwelfthProductNameRichTextBox);
                    TwelfthProductSeesRichTextBox.Text = productInfo.ProductSees;
                    TwelfthProductPriceRichTextBox.Text = productInfo.ProductPrice.ToString();

                    TwelfthProductTypeTextBox.Text = productInfo.ProductType;
                    TwelfthProductTypePriceTextBox.Text = productInfo.ProductTypePrice.ToString();
                    if (_invoiceNumberYearCreation != null)
                        _lastFilledYearForProductInfo[ProductLineIndex[12]] = _invoiceNumberYearCreation.Value;
                    break;
            }
        }

        private void FillProductNameTextBoxWithBarCode(FullProductInfoWithId productInfo, RichTextBox richTextBox)
        {
            int numLines = productInfo.ProductName.Split('\n').Length;

            if (numLines == 1)
            {
                richTextBox.Text = string.Format(@"{0} {1}{2}", productInfo.ProductName, Environment.NewLine,
                    productInfo.BarCode);
            }
            else
            {
                richTextBox.Text = string.Format(@"{0} {1}", productInfo.ProductName, productInfo.BarCode);
            }
        }

        private void FillBuyerInfo()
        {
            if (BuyerInfoNameComboBox.Text == _lastBuyerFilled) return;

            BuyerFullInfoWithIdModel buyerFullInfo =
                _buyersInfoRepository.GetBuyerFullInfoWithId(BuyerInfoNameComboBox.Text);

            if (buyerFullInfo != null)
            {
                _lastBuyerFilled = BuyerInfoNameComboBox.Text;

                BuyerInfoIdTextBox.Text = buyerFullInfo.Id.ToString();

                BuyerNameRichTextBox.Text = buyerFullInfo.BuyerName;
                BuyerFirmCodeRichTextBox.Text = buyerFullInfo.BuyerFirmCode;
                BuyerPvmCodeRichTextBox.Text = buyerFullInfo.BuyerPvmCode;
                BuyerAddressRichTextBox.Text = buyerFullInfo.BuyerAddress;
            }
            else
            {
                _messageDialogService.ShowErrorMassage("Nėra informacijos kurią galima sukelti");
            }
        }

        private void FillBuyerComboBox()
        {
            List<BuyersNamesModel> getBuyerFullInfoWithId = _buyersInfoRepository.GetExistsBuyersNames().ToList();

            BuyerInfoNameComboBox.BindingContext = new BindingContext();
            BuyerInfoNameComboBox.DataSource = getBuyerFullInfoWithId;
            BuyerInfoNameComboBox.DisplayMember = "BuyerName";
        }

        private void SuggestToFillProductTypeQuantityIfEmpty(bool isAllQuantityFilled)
        {
            if (isAllQuantityFilled) return;

            DialogResult dialogResult = _messageDialogService.ShowChoiceMessage(
                "Kai kurie kiekio langeliai valdymo centre produkto tipai nėra supildyti arba supildyti ne pagal sąskaitos faktūros duomenis ar norite juos kad automatiškai supildytų pagal sąskaitos faktūros duomenis ?");

            if (dialogResult == DialogResult.OK)
            {
                FirstProductTypeQuantityTextBox.Text = FirstProductQuantityRichTextBox.Text;
                SecondProductTypeQuantityTextBox.Text = SecondProductQuantityRichTextBox.Text;
                ThirdProductTypeQuantityTextBox.Text = ThirdProductQuantityRichTextBox.Text;
                FourthProductTypeQuantityTextBox.Text = FourthProductQuantityRichTextBox.Text;
                FifthProductTypeQuantityTextBox.Text = FifthProductQuantityRichTextBox.Text;
                SixthProductTypeQuantityTextBox.Text = SixthProductQuantityRichTextBox.Text;
                SeventhProductTypeQuantityTextBox.Text = SeventhProductQuantityRichTextBox.Text;
                EighthProductTypeQuantityTextBox.Text = EighthProductQuantityRichTextBox.Text;
                NinthProductTypeQuantityTextBox.Text = NinthProductQuantityRichTextBox.Text;
                TenProductTypeQuantityTextBox.Text = TenProductQuantityRichTextBox.Text;
                EleventhProductTypeQuantityTextBox.Text = EleventhProductQuantityRichTextBox.Text;
                TwelfthProductTypeQuantityTextBox.Text = TwelfthProductQuantityRichTextBox.Text;
            }
        }

        private bool CheckIsProductTypeQuantityFilledByInvoiceProductQuantity(RichTextBox richTextBox, TextBox textBox)
        {
            bool isFilled;
            bool isQuantityFilled = richTextBox.Text != string.Empty;
            bool isTypeQuantityFilled = textBox.Text != string.Empty;

            if (isQuantityFilled && !isTypeQuantityFilled)
            {
                isFilled = false;
            }
            else if (!isQuantityFilled && isTypeQuantityFilled)
            {
                isFilled = false;
            }
            else if (isQuantityFilled && richTextBox.Text != textBox.Text)
            {
                isFilled = false;
            }
            else
            {
                isFilled = true;
            }

            return isFilled;
        }

        private bool CheckIsAllProductTypeQuantityFilledByInvoiceProductQuantity()
        {
            bool isAllFilled =
                CheckIsProductTypeQuantityFilledByInvoiceProductQuantity(FirstProductQuantityRichTextBox,
                    FirstProductTypeQuantityTextBox) &&
                CheckIsProductTypeQuantityFilledByInvoiceProductQuantity(SecondProductQuantityRichTextBox,
                    SecondProductTypeQuantityTextBox) &&
                CheckIsProductTypeQuantityFilledByInvoiceProductQuantity(ThirdProductQuantityRichTextBox,
                    ThirdProductTypeQuantityTextBox) &&
                CheckIsProductTypeQuantityFilledByInvoiceProductQuantity(FourthProductQuantityRichTextBox,
                    FourthProductTypeQuantityTextBox) &&
                CheckIsProductTypeQuantityFilledByInvoiceProductQuantity(FifthProductQuantityRichTextBox,
                    FifthProductTypeQuantityTextBox) &&
                CheckIsProductTypeQuantityFilledByInvoiceProductQuantity(SixthProductQuantityRichTextBox,
                    SixthProductTypeQuantityTextBox) &&
                CheckIsProductTypeQuantityFilledByInvoiceProductQuantity(SeventhProductQuantityRichTextBox,
                    SeventhProductTypeQuantityTextBox) &&
                CheckIsProductTypeQuantityFilledByInvoiceProductQuantity(EighthProductQuantityRichTextBox,
                    EighthProductTypeQuantityTextBox) &&
                CheckIsProductTypeQuantityFilledByInvoiceProductQuantity(NinthProductQuantityRichTextBox,
                    NinthProductTypeQuantityTextBox) &&
                CheckIsProductTypeQuantityFilledByInvoiceProductQuantity(TenProductQuantityRichTextBox,
                    TenProductTypeQuantityTextBox) &&
                CheckIsProductTypeQuantityFilledByInvoiceProductQuantity(EleventhProductQuantityRichTextBox,
                    EleventhProductTypeQuantityTextBox) &&
                CheckIsProductTypeQuantityFilledByInvoiceProductQuantity(TwelfthProductQuantityRichTextBox,
                    TwelfthProductTypeQuantityTextBox);

            return isAllFilled;
        }

        private void LoadSuggestedMoneyReceiptNumber()
        {
            MoneyReceiptSuggestedNumberModel suggestedNumber = _moneyReceiptRepository.GetSuggestedMoneyReceiptNumber();

            MoneyReceiptOfferNumberTextBox.Text = suggestedNumber.MoneyReceiptSuggestedNumber.ToString();

            _lastMoneyReceiptNumber = suggestedNumber.MoneyReceiptSuggestedNumber;
        }

        private void AddOneToMoneyReceiptSuggestedNumber()
        {
            int suggestedNumber = int.Parse(MoneyReceiptOfferNumberTextBox.Text);

            if (_lastMoneyReceiptNumber == suggestedNumber)
            {
                _moneyReceiptRepository.AddOneToMoneyReceiptSuggestedNumber();
            }
            else
            {
                _moneyReceiptRepository.UpdateNewSuggestedNumberAndAddOne(suggestedNumber);
            }
        }

        private void LoadInvoiceControlYearTextBox()
        {
            if (_invoiceOperations == InvoiceOperations.Edit && _invoiceNumberYearCreation.HasValue)
            {
                InvoiceYearControlTextBox.Text = _invoiceNumberYearCreation.Value.ToString();
            }
            else
            {
                InvoiceYearControlTextBox.Text = DateTime.Now.Year.ToString();
                _invoiceNumberYearCreation = DateTime.Now.Year;
            }
        }

        private void AddQuantityFromNewInvoiceToDeposit()
        {
            bool isFirstHasId = CheckProductInfoId(FirstProductIdTextBox);
            bool isSecondHasId = CheckProductInfoId(SecondProductIdTextBox);
            bool isThirdHasId = CheckProductInfoId(ThirdProductIdTextBox);
            bool isFourthHasId = CheckProductInfoId(FourthProductIdTextBox);
            bool isFifthHasId = CheckProductInfoId(FifthProductIdTextBox);
            bool isSixthHasId = CheckProductInfoId(SixthProductIdTextBox);
            bool isSeventhHasId = CheckProductInfoId(SeventhProductIdTextBox);
            bool isEighthHasId = CheckProductInfoId(EighthProductIdTextBox);
            bool isNinthHasId = CheckProductInfoId(NinthProductIdTextBox);
            bool isTenHasId = CheckProductInfoId(TenProductIdTextBox);
            bool isEleventhHasId = CheckProductInfoId(EleventhProductIdTextBox);
            bool isTwelfthHasId = CheckProductInfoId(TwelfthProductIdTextBox);

            FilledQuantityForNewInvoiceToDepositDataBaseById(isFirstHasId, FirstProductIdTextBox,
                FirstProductQuantityRichTextBox);
            FilledQuantityForNewInvoiceToDepositDataBaseById(isSecondHasId, SecondProductIdTextBox,
                SecondProductQuantityRichTextBox);
            FilledQuantityForNewInvoiceToDepositDataBaseById(isThirdHasId, ThirdProductIdTextBox,
                ThirdProductQuantityRichTextBox);
            FilledQuantityForNewInvoiceToDepositDataBaseById(isFourthHasId, FourthProductIdTextBox,
                FourthProductQuantityRichTextBox);
            FilledQuantityForNewInvoiceToDepositDataBaseById(isFifthHasId, FifthProductIdTextBox,
                FifthProductQuantityRichTextBox);
            FilledQuantityForNewInvoiceToDepositDataBaseById(isSixthHasId, SixthProductIdTextBox,
                SixthProductQuantityRichTextBox);
            FilledQuantityForNewInvoiceToDepositDataBaseById(isSeventhHasId, SeventhProductIdTextBox,
                SeventhProductQuantityRichTextBox);
            FilledQuantityForNewInvoiceToDepositDataBaseById(isEighthHasId, EighthProductIdTextBox,
                EighthProductQuantityRichTextBox);
            FilledQuantityForNewInvoiceToDepositDataBaseById(isNinthHasId, NinthProductIdTextBox,
                NinthProductQuantityRichTextBox);
            FilledQuantityForNewInvoiceToDepositDataBaseById(isTenHasId, TenProductIdTextBox,
                TenProductQuantityRichTextBox);
            FilledQuantityForNewInvoiceToDepositDataBaseById(isEleventhHasId, EleventhProductIdTextBox,
                EleventhProductQuantityRichTextBox);
            FilledQuantityForNewInvoiceToDepositDataBaseById(isTwelfthHasId, TwelfthProductIdTextBox,
                TwelfthProductQuantityRichTextBox);
        }

        private bool CheckProductInfoId(TextBox textBox)
        {
            bool isHasId = !string.IsNullOrEmpty(textBox.Text);
            return isHasId;
        }

        private void FilledQuantityForNewInvoiceToDepositDataBaseById(bool isHasId, TextBox textBox,
            RichTextBox richTextBox)
        {
            bool isNumber = double.TryParse(richTextBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture,
                out double quantity);

            if (!isHasId || !isNumber || !_invoiceNumberYearCreation.HasValue) return;

            DepositAddQuantityModel depositAddQuantity = new DepositAddQuantityModel()
            {
                Id = int.Parse(textBox.Text),
                InvoiceYear = _invoiceNumberYearCreation.Value,
                ProductQuantity = quantity
            };

            _depositRepository.AddQuantityByIdAndYear(depositAddQuantity);
        }

        private void FilledQuantityForOldInvoiceInfoToDepositDataBaseById(bool isHasId, int productLineIndex)
        {
            if (!isHasId && _lastQuantityValues[productLineIndex].HasValue) return;
            
            double quantity = _lastQuantityValues[productLineIndex].Value;

            DepositAddQuantityModel depositAddQuantity = new DepositAddQuantityModel()
            {
                Id = _idProductOldLineValues[productLineIndex], 
                InvoiceYear = _oldInvoiceYear,
                ProductQuantity = quantity
            };

            _depositRepository.SubtractQuantityByIdAndYear(depositAddQuantity);
        }

        private void SubtractOldValuesFromDeposit()
        {
            bool isFirstHasId = CheckOldProductInfoId(0);
            bool isSecondHasId = CheckOldProductInfoId(1);
            bool isThirdHasId = CheckOldProductInfoId(2);
            bool isFourthHasId = CheckOldProductInfoId(3);
            bool isFifthHasId = CheckOldProductInfoId(4);
            bool isSixthHasId = CheckOldProductInfoId(5);
            bool isSeventhHasId = CheckOldProductInfoId(6);
            bool isEighthHasId = CheckOldProductInfoId(7);
            bool isNinthHasId = CheckOldProductInfoId(8);
            bool isTenHasId = CheckOldProductInfoId(9);
            bool isEleventhHasId = CheckOldProductInfoId(10);
            bool isTwelfthHasId = CheckOldProductInfoId(11);

            FilledQuantityForOldInvoiceInfoToDepositDataBaseById(isFirstHasId, 0);
            FilledQuantityForOldInvoiceInfoToDepositDataBaseById(isSecondHasId, 1);
            FilledQuantityForOldInvoiceInfoToDepositDataBaseById(isThirdHasId, 2);
            FilledQuantityForOldInvoiceInfoToDepositDataBaseById(isFourthHasId, 3);
            FilledQuantityForOldInvoiceInfoToDepositDataBaseById(isFifthHasId, 4);
            FilledQuantityForOldInvoiceInfoToDepositDataBaseById(isSixthHasId, 5);
            FilledQuantityForOldInvoiceInfoToDepositDataBaseById(isSeventhHasId, 6);
            FilledQuantityForOldInvoiceInfoToDepositDataBaseById(isEighthHasId, 7);
            FilledQuantityForOldInvoiceInfoToDepositDataBaseById(isNinthHasId, 8);
            FilledQuantityForOldInvoiceInfoToDepositDataBaseById(isTenHasId, 9);
            FilledQuantityForOldInvoiceInfoToDepositDataBaseById(isEleventhHasId, 10);
            FilledQuantityForOldInvoiceInfoToDepositDataBaseById(isTwelfthHasId, 11);
        }

        private bool CheckOldProductInfoId(int productLineIndex)
        {
            bool isHasId = _idProductOldLineValues[productLineIndex] != 0;
            return isHasId;
        }

        private void FillDepositInfoToEditInvoiceOperation()
        {
            if (_invoiceOperations != InvoiceOperations.Edit || !_invoiceNumberYearCreation.HasValue) return;

            _oldInvoiceYear = _invoiceNumberYearCreation.Value;

            _lastQuantityValues = new double?[12];
            _lastQuantityValues[0] = _numberService.ParseToDoubleOrNull(FirstProductQuantityRichTextBox);
            _lastQuantityValues[1] = _numberService.ParseToDoubleOrNull(SecondProductQuantityRichTextBox);
            _lastQuantityValues[2] = _numberService.ParseToDoubleOrNull(ThirdProductQuantityRichTextBox);
            _lastQuantityValues[3] = _numberService.ParseToDoubleOrNull(FourthProductQuantityRichTextBox);
            _lastQuantityValues[4] = _numberService.ParseToDoubleOrNull(FifthProductQuantityRichTextBox);
            _lastQuantityValues[5] = _numberService.ParseToDoubleOrNull(SixthProductQuantityRichTextBox);
            _lastQuantityValues[6] = _numberService.ParseToDoubleOrNull(SeventhProductQuantityRichTextBox);
            _lastQuantityValues[7] = _numberService.ParseToDoubleOrNull(EighthProductQuantityRichTextBox);
            _lastQuantityValues[8] = _numberService.ParseToDoubleOrNull(NinthProductQuantityRichTextBox);
            _lastQuantityValues[9] = _numberService.ParseToDoubleOrNull(TenProductQuantityRichTextBox);
            _lastQuantityValues[10] = _numberService.ParseToDoubleOrNull(EleventhProductQuantityRichTextBox);
            _lastQuantityValues[11] = _numberService.ParseToDoubleOrNull(TwelfthProductQuantityRichTextBox);

            LoadProductInfoId();
        }

        private void SaveProductInfoId()
        {
            _idProductLinesValues = new int[12];

            FillIdProductLinesValuesSaveModel(FirstProductIdTextBox, 0);
            FillIdProductLinesValuesSaveModel(SecondProductIdTextBox, 1);
            FillIdProductLinesValuesSaveModel(ThirdProductIdTextBox, 2);
            FillIdProductLinesValuesSaveModel(FourthProductIdTextBox, 3);
            FillIdProductLinesValuesSaveModel(FifthProductIdTextBox, 4);
            FillIdProductLinesValuesSaveModel(SixthProductIdTextBox, 5);
            FillIdProductLinesValuesSaveModel(SeventhProductIdTextBox, 6);
            FillIdProductLinesValuesSaveModel(EighthProductIdTextBox, 7);
            FillIdProductLinesValuesSaveModel(NinthProductIdTextBox, 8);
            FillIdProductLinesValuesSaveModel(TenProductIdTextBox, 9);
            FillIdProductLinesValuesSaveModel(EleventhProductIdTextBox, 10);
            FillIdProductLinesValuesSaveModel(TwelfthProductIdTextBox, 11);

            DepositIdSaveModel saveProductInfoId = new DepositIdSaveModel
            {
                InvoiceId = int.Parse(InvoiceNumberRichTextBox.Text),

                FirstLineId = _idProductLinesValues[0],
                SecondLineId = _idProductLinesValues[1],
                ThirdLineId = _idProductLinesValues[2],
                FourthLineId = _idProductLinesValues[3],
                FifthLineId = _idProductLinesValues[4],
                SixthLineId = _idProductLinesValues[5],
                SeventhLineId = _idProductLinesValues[6],
                EighthLineId = _idProductLinesValues[7],
                NinthLineId = _idProductLinesValues[8],
                TenLineId = _idProductLinesValues[9],
                EleventhLineId = _idProductLinesValues[10],
                TwelfthLineId = _idProductLinesValues[11]
            };

            _depositRepository.SaveDepositIdLinesInfo(saveProductInfoId);
        }

        private void FillIdProductLinesValuesSaveModel(TextBox textBox, int idLineIndex)
        {
            if (textBox.Text != string.Empty)
            {
                _idProductLinesValues[idLineIndex] = int.Parse(textBox.Text);
            }
            else
            {
                _idProductLinesValues[idLineIndex] = 0;
            }
        }

        private void LoadProductInfoId()
        {
            if (!_invoiceNumber.HasValue) return;

            DepositIdLoadModel getProductId = _depositRepository.LoadDepositIdLinesInfo(_invoiceNumber.Value);

            LoadProductInfoIdWhenIdNotZero(getProductId);

            LoadProductInfoNamesById(getProductId);
        }

        private void LoadProductInfoIdWhenIdNotZero(DepositIdLoadModel getProductId)
        {
            _idProductOldLineValues = new int[12];
            _idProductOldLineValues[0] = 0;
            _idProductOldLineValues[1] = 0;
            _idProductOldLineValues[2] = 0;
            _idProductOldLineValues[3] = 0;
            _idProductOldLineValues[4] = 0;
            _idProductOldLineValues[5] = 0;
            _idProductOldLineValues[6] = 0;
            _idProductOldLineValues[7] = 0;
            _idProductOldLineValues[8] = 0;
            _idProductOldLineValues[9] = 0;
            _idProductOldLineValues[10] = 0;
            _idProductOldLineValues[11] = 0;

            if (getProductId.FirstLineId != 0)
            {
                FirstProductIdTextBox.Text = getProductId.FirstLineId.ToString();
                _idProductOldLineValues[0] = getProductId.FirstLineId;
            }

            if (getProductId.SecondLineId != 0)
            {
                SecondProductIdTextBox.Text = getProductId.SecondLineId.ToString();
                _idProductOldLineValues[1] = getProductId.SecondLineId;
            }

            if (getProductId.ThirdLineId != 0)
            {
                ThirdProductIdTextBox.Text = getProductId.ThirdLineId.ToString();
                _idProductOldLineValues[2] = getProductId.ThirdLineId;
            }

            if (getProductId.FourthLineId != 0)
            {
                FourthProductIdTextBox.Text = getProductId.FourthLineId.ToString();
                _idProductOldLineValues[3] = getProductId.FourthLineId;
            }

            if (getProductId.FifthLineId != 0)
            {
                FifthProductIdTextBox.Text = getProductId.FifthLineId.ToString();
                _idProductOldLineValues[4] = getProductId.FifthLineId;
            }

            if (getProductId.SixthLineId != 0)
            {
                SixthProductIdTextBox.Text = getProductId.SixthLineId.ToString();
                _idProductOldLineValues[5] = getProductId.SixthLineId;
            }

            if (getProductId.SeventhLineId != 0)
            {
                SeventhProductIdTextBox.Text = getProductId.SeventhLineId.ToString();
                _idProductOldLineValues[6] = getProductId.SeventhLineId;
            }

            if (getProductId.EighthLineId != 0)
            {
                EighthProductIdTextBox.Text = getProductId.EighthLineId.ToString();
                _idProductOldLineValues[7] = getProductId.EighthLineId;
            }

            if (getProductId.NinthLineId != 0)
            {
                NinthProductIdTextBox.Text = getProductId.NinthLineId.ToString();
                _idProductOldLineValues[8] = getProductId.NinthLineId;
            }

            if (getProductId.TenLineId != 0)
            {
                TenProductIdTextBox.Text = getProductId.TenLineId.ToString();
                _idProductOldLineValues[9] = getProductId.TenLineId;
            }

            if (getProductId.EleventhLineId != 0)
            {
                EleventhProductIdTextBox.Text = getProductId.EleventhLineId.ToString();
                _idProductOldLineValues[10] = getProductId.EleventhLineId;
            }

            if (getProductId.TwelfthLineId != 0)
            {
                TwelfthProductIdTextBox.Text = getProductId.TwelfthLineId.ToString();
                _idProductOldLineValues[11] = getProductId.TwelfthLineId;
            }
        }

        private void LoadProductInfoNamesById(DepositIdLoadModel getProductId)
        {
            if (FirstProductIdTextBox.Text != string.Empty)
                FirstProductNameComboBox.Text = _productInfoRepository.GetProductName(getProductId.FirstLineId);
            if (SecondProductIdTextBox.Text != string.Empty)
                SecondProductNameComboBox.Text = _productInfoRepository.GetProductName(getProductId.SecondLineId);
            if (ThirdProductIdTextBox.Text != string.Empty)
                ThirdProductNameComboBox.Text = _productInfoRepository.GetProductName(getProductId.ThirdLineId);
            if (FourthProductIdTextBox.Text != string.Empty)
                FourthProductNameComboBox.Text = _productInfoRepository.GetProductName(getProductId.FourthLineId);
            if (FifthProductIdTextBox.Text != string.Empty)
                FifthProductNameComboBox.Text = _productInfoRepository.GetProductName(getProductId.FifthLineId);
            if (SixthProductIdTextBox.Text != string.Empty)
                SixthProductNameComboBox.Text = _productInfoRepository.GetProductName(getProductId.SixthLineId);
            if (SeventhProductIdTextBox.Text != string.Empty)
                SeventhProductNameComboBox.Text = _productInfoRepository.GetProductName(getProductId.SeventhLineId);
            if (EighthProductIdTextBox.Text != string.Empty)
                EighthProductNameComboBox.Text = _productInfoRepository.GetProductName(getProductId.EighthLineId);
            if (NinthProductIdTextBox.Text != string.Empty)
                NinthProductNameComboBox.Text = _productInfoRepository.GetProductName(getProductId.NinthLineId);
            if (TenProductIdTextBox.Text != string.Empty)
                TenProductNameComboBox.Text = _productInfoRepository.GetProductName(getProductId.TenLineId);
            if (EleventhProductIdTextBox.Text != string.Empty)
                EleventhProductNameComboBox.Text = _productInfoRepository.GetProductName(getProductId.EleventhLineId);
            if (TwelfthProductIdTextBox.Text != string.Empty)
                TwelfthProductNameComboBox.Text = _productInfoRepository.GetProductName(getProductId.TwelfthLineId);

            // todo can create better database call by getting all id and all names then filled values by id number in each combo box 
        }

        private void SaveNewProductsQuantityValuesForEditInvoice()
        {
            bool isYearSame = CheckInvoiceYearAreNotChanged();

            double?[] newQuantityValues = new double?[12];

            newQuantityValues[0] = _numberService.ParseToDoubleOrNull(FirstProductQuantityRichTextBox);
            newQuantityValues[1] = _numberService.ParseToDoubleOrNull(SecondProductQuantityRichTextBox);
            newQuantityValues[2] = _numberService.ParseToDoubleOrNull(ThirdProductQuantityRichTextBox);
            newQuantityValues[3] = _numberService.ParseToDoubleOrNull(FourthProductQuantityRichTextBox);
            newQuantityValues[4] = _numberService.ParseToDoubleOrNull(FifthProductQuantityRichTextBox);
            newQuantityValues[5] = _numberService.ParseToDoubleOrNull(SixthProductQuantityRichTextBox);
            newQuantityValues[6] = _numberService.ParseToDoubleOrNull(SeventhProductQuantityRichTextBox);
            newQuantityValues[7] = _numberService.ParseToDoubleOrNull(EighthProductQuantityRichTextBox);
            newQuantityValues[8] = _numberService.ParseToDoubleOrNull(NinthProductQuantityRichTextBox);
            newQuantityValues[9] = _numberService.ParseToDoubleOrNull(TenProductQuantityRichTextBox);
            newQuantityValues[10] = _numberService.ParseToDoubleOrNull(EleventhProductQuantityRichTextBox);
            newQuantityValues[11] = _numberService.ParseToDoubleOrNull(TwelfthProductQuantityRichTextBox);

            if (isYearSame)
            {
                AddToDepositNewInfoForEditOperationWhenYearSame(0, newQuantityValues);
                AddToDepositNewInfoForEditOperationWhenYearSame(1, newQuantityValues);
                AddToDepositNewInfoForEditOperationWhenYearSame(2, newQuantityValues);
                AddToDepositNewInfoForEditOperationWhenYearSame(3, newQuantityValues);
                AddToDepositNewInfoForEditOperationWhenYearSame(4, newQuantityValues);
                AddToDepositNewInfoForEditOperationWhenYearSame(5, newQuantityValues);
                AddToDepositNewInfoForEditOperationWhenYearSame(6, newQuantityValues);
                AddToDepositNewInfoForEditOperationWhenYearSame(7, newQuantityValues);
                AddToDepositNewInfoForEditOperationWhenYearSame(8, newQuantityValues);
                AddToDepositNewInfoForEditOperationWhenYearSame(9, newQuantityValues);
                AddToDepositNewInfoForEditOperationWhenYearSame(10, newQuantityValues);
                AddToDepositNewInfoForEditOperationWhenYearSame(11, newQuantityValues);
            }
            else
            {
                AddToDepositNewInfoForEditOperationWhenYearNotSame();
            }
        }

        private bool CheckInvoiceYearAreNotChanged()
        {
            bool isYearSame = false;
            if (_invoiceNumberYearCreation.HasValue)
            {
                isYearSame = _oldInvoiceYear == _invoiceNumberYearCreation.Value;
            }

            return isYearSame;
        }

        private void AddToDepositNewInfoForEditOperationWhenYearSame(int lineIndex, double?[] newQuantityValues)
        {
            if (!_invoiceNumberYearCreation.HasValue) return;

            if (_idProductLinesValues[lineIndex] == _idProductOldLineValues[lineIndex])
            {
                SaveNewQuantityToDeposit(newQuantityValues, lineIndex, _idProductLinesValues[lineIndex]);
            }
            else if (_idProductLinesValues[lineIndex] != _idProductOldLineValues[lineIndex])
            {
                SaveQuantityToDepositWhenProductIdIsNotSame(newQuantityValues, lineIndex);
            }
        }

        private void SaveQuantityToDepositWhenProductIdIsNotSame(double?[] newQuantityValues, int quantityLineIndex)
        {
            if (_lastQuantityValues[quantityLineIndex] == null && newQuantityValues[quantityLineIndex] == null || !_invoiceNumberYearCreation.HasValue) return;

            if (_lastQuantityValues[quantityLineIndex] != null)
            {
                DepositAddQuantityModel subtractQuantity = new DepositAddQuantityModel
                {
                    Id = _idProductOldLineValues[quantityLineIndex],
                    InvoiceYear = _invoiceNumberYearCreation.Value,
                    ProductQuantity = _lastQuantityValues[quantityLineIndex].Value
                };

                _depositRepository.SubtractQuantityByIdAndYear(subtractQuantity);
            }

            if (newQuantityValues[quantityLineIndex] != null)
            {
                DepositAddQuantityModel addQuantity = new DepositAddQuantityModel
                {
                    Id = _idProductLinesValues[quantityLineIndex],
                    InvoiceYear = _invoiceNumberYearCreation.Value,
                    ProductQuantity = newQuantityValues[quantityLineIndex].Value
                };

                _depositRepository.AddQuantityByIdAndYear(addQuantity);
            }
        }

        private void SaveNewQuantityToDeposit(double?[] newQuantityValues, int quantityLineIndex, int productId)
        {
            if (_lastQuantityValues[quantityLineIndex] == null && newQuantityValues[quantityLineIndex] == null || !_invoiceNumberYearCreation.HasValue) return;

            if (newQuantityValues[quantityLineIndex] == null && _lastQuantityValues[quantityLineIndex] != null)
            {
                DepositAddQuantityModel subtractQuantity = new DepositAddQuantityModel
                {
                    Id = productId,
                    InvoiceYear = _invoiceNumberYearCreation.Value,
                    ProductQuantity = _lastQuantityValues[quantityLineIndex].Value
                };

                _depositRepository.SubtractQuantityByIdAndYear(subtractQuantity);
            }
            else if (newQuantityValues[quantityLineIndex] != null && _lastQuantityValues[quantityLineIndex] == null)
            {
                DepositAddQuantityModel addQuantity = new DepositAddQuantityModel
                {
                    Id = productId,
                    InvoiceYear = _invoiceNumberYearCreation.Value,
                    ProductQuantity = newQuantityValues[quantityLineIndex].Value
                };

                _depositRepository.AddQuantityByIdAndYear(addQuantity);
            }
            else if (newQuantityValues[quantityLineIndex] > _lastQuantityValues[quantityLineIndex])
            {
                double newValue = newQuantityValues[quantityLineIndex].Value -
                                   _lastQuantityValues[quantityLineIndex].Value;

                DepositAddQuantityModel addQuantity = new DepositAddQuantityModel
                {
                    Id = productId,
                    InvoiceYear = _invoiceNumberYearCreation.Value,
                    ProductQuantity = newValue
                };

                _depositRepository.AddQuantityByIdAndYear(addQuantity);
            }
            else if (newQuantityValues[quantityLineIndex] < _lastQuantityValues[quantityLineIndex])
            {
                double newValue = _lastQuantityValues[quantityLineIndex].Value - newQuantityValues[quantityLineIndex].Value;

                DepositAddQuantityModel subtractQuantity = new DepositAddQuantityModel
                {
                    Id = productId,
                    InvoiceYear = _invoiceNumberYearCreation.Value,
                    ProductQuantity = newValue
                };

                _depositRepository.SubtractQuantityByIdAndYear(subtractQuantity);
            }
        }

        private void AddToDepositNewInfoForEditOperationWhenYearNotSame()
        {
            AddQuantityFromNewInvoiceToDeposit();
            SubtractOldValuesFromDeposit();
        }

        #endregion
    }
}
