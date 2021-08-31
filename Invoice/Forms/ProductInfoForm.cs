using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Invoice.Constants;
using Invoice.Models.Deposit;
using Invoice.Models.ProductInfo;
using Invoice.Repositories;
using Invoice.Service;

namespace Invoice.Forms
{
    public partial class ProductInfoForm : Form
    {
        private readonly ProductInfoRepository _productInfoRepository;
        private readonly DepositRepository _depositRepository;
        private readonly MessageDialogService _messageDialogService;
        private readonly NumberService _numberService;

        private string[] _lastProductInfo = new string[7];

        public ProductInfoForm()
        {
            _productInfoRepository = new ProductInfoRepository();
            _depositRepository = new DepositRepository();
            _messageDialogService = new MessageDialogService();
            _numberService = new NumberService();

            InitializeComponent();

            SetControlsInitialState();
            SetTextBoxMaxLength();

            DepositYearTextBox.Text = DateTime.Now.Year.ToString();
            LoadAllProductsNamesToComboBox();
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

        private void RichTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                RichTextBox richTextBox = sender as RichTextBox;

                if (richTextBox == null) return;

                if (richTextBox.Multiline == false)
                {
                    this.SelectNextControl((Control)sender, true, true, true, true);
                }

                SetCursorsAtRichTextBoxStringEnd();
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
                SetCursorsAtTextBoxStringEnd();
            }
        }

        private void RichTextBox_TextChanged(object sender, EventArgs e)
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
                this.SelectNextControl((Control)sender, true, true, true, true);
            }
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (textBox == null) return;

            if (textBox.SelectionStart == textBox.MaxLength)
            {
                _messageDialogService.ShowInfoMessage($"Pasiektas maksimalus žodžių ilgis bus išsaugota tik toks tekstas ({textBox.Text}) ");
            }
        }

        private void ChooseProductButton_Click(object sender, EventArgs e)
        {
            LoadProductInfoToTextBoxes();

            ProductBarCodeRichTextBox.Focus();
            ProductBarCodeRichTextBox.SelectionStart = ProductBarCodeRichTextBox.Text.Length;
        }

        private void NewProductButton_Click(object sender, EventArgs e)
        {
            int year = int.Parse(DepositYearTextBox.Text);
            bool isProductNameFilled = !string.IsNullOrWhiteSpace(ProductNameRichTextBox.Text);
            bool isProductExists = _productInfoRepository.CheckIsProductNameExists(ProductNameRichTextBox.Text, year);
            bool isAllTextBoxFilled = CheckIsAllInfoFilled();

            if (!isProductExists && isProductNameFilled && isAllTextBoxFilled)
            {
                CreateNewProduct();
                LoadAllProductsNamesToComboBox();
            }
            else if (!isProductExists && isProductNameFilled)
            {
                DialogResult dialogResult = _messageDialogService.ShowChoiceMessage("Kai kurie langeliai nesupildyti ar norite išsaugoti nepilną informaciją apie produktą ?");

                if (dialogResult == DialogResult.OK)
                {
                    CreateNewProduct();
                    LoadAllProductsNamesToComboBox();
                }
            }
            else if (!isProductExists)
            {
                _messageDialogService.ShowErrorMassage("Kad išsaugotumėte naują produktą būtina supildyti produkto pavadinimas langelį");
            }
            else
            {
                _messageDialogService.ShowErrorMassage("Toks produktas egzistuoja jei norite pakeisti jo informaciją spauskite atnaujinti produktą mygtuką");
            }

            SetCursorAtProductNameStringEnd();
        }

        private void UpdateProductButton_Click(object sender, EventArgs e)
        {
            int year = int.Parse(DepositYearTextBox.Text);
            bool isProductNameFilled = !string.IsNullOrWhiteSpace(ProductNameRichTextBox.Text);
            bool isProductExists = _productInfoRepository.CheckIsProductNameExists(ProductNameRichTextBox.Text, year);
            bool isAllValuesSameAsinDatabase = CheckIsProductAllValuesSameAsInDataBase();

            if (isProductExists && isProductNameFilled && !isAllValuesSameAsinDatabase)
            {
                UpdateProductInfo();
            }
            else if (isProductExists && isProductNameFilled)
            {
                _messageDialogService.ShowErrorMassage("Jūs nieko nepakeitėte todėl nebus atnaujinta informacija liks tokia pati ");
            }
            else if (isProductNameFilled)
            {
                _messageDialogService.ShowErrorMassage("Kad atnaujintumėte produktą būtina supildyti produkto pavadinimas langelį");
            }
            else
            {
                _messageDialogService.ShowErrorMassage("Produktas nerastas atnaujinimas negalimas");
            }

            SetCursorAtProductNameStringEnd();
        }

        private void DepositYearTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            bool isNumber = int.TryParse(DepositYearTextBox.Text, out int number);

            if (string.IsNullOrWhiteSpace(DepositYearTextBox.Text))
            {
                SetButtonControl(false);
                e.Cancel = true;
                _messageDialogService.DisplayLabelAndTextBoxError(
                    $" Raudonas langelis Negali būti tuščias! pvz.: {DateTime.Now.Year}", DepositYearTextBox,
                    ErrorMassageLabel);
            }
            else if (!isNumber)
            {
                SetButtonControl(false);
                e.Cancel = true;
                _messageDialogService.DisplayLabelAndTextBoxError(
                    $" Raudonas langelis turi būti metai pvz.: {DateTime.Now.Year}", DepositYearTextBox,
                    ErrorMassageLabel);
            }
            else if (number > DateTime.Now.Year)
            {
                SetButtonControl(false);
                e.Cancel = true;
                _messageDialogService.DisplayLabelAndTextBoxError(
                    $" Raudonam langelį metai negali būti ateityje pvz.: {DateTime.Now.Year}",
                    DepositYearTextBox,
                    ErrorMassageLabel);
            }
            else if (number < 2000)
            {
                SetButtonControl(false);
                e.Cancel = true;
                _messageDialogService.DisplayLabelAndTextBoxError(
                    $" Raudonam langelyje negali būti mažiau nei 2000 pvz.: {DateTime.Now.Year}",
                    DepositYearTextBox,
                    ErrorMassageLabel);
            }
            else
            {
                SetButtonControl(true);
                e.Cancel = false;
                _messageDialogService.HideLabelAndTextBoxError(ErrorMassageLabel, DepositYearTextBox);
            }
        }

        #region Helpers

        private void SetControlsInitialState()
        {
            this.StartPosition = FormStartPosition.CenterScreen;

            ExistsProductListComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void SetTextBoxMaxLength()
        {
            ProductNameRichTextBox.MaxLength = FormSettings.TextBoxLengths.ProductName;
            ProductBarCodeRichTextBox.MaxLength = FormSettings.TextBoxLengths.BarCode;
            ProductSeesRichTextBox.MaxLength = FormSettings.TextBoxLengths.FirstProductSees;
            ProductPriceRichTextBox.MaxLength = FormSettings.TextBoxLengths.MaxNumberLength;

            ProductTypeTextBox.MaxLength = FormSettings.TextBoxLengths.ProductType;
            ProductTypePriceTextBox.MaxLength = FormSettings.TextBoxLengths.MaxNumberLength;

            DepositYearTextBox.MaxLength = FormSettings.TextBoxLengths.InvoiceYearControl;
        }

        private void SetCursorsAtRichTextBoxStringEnd()
        {
            ProductNameRichTextBox.SelectionStart = ProductNameRichTextBox.Text.Length;
            ProductBarCodeRichTextBox.SelectionStart = ProductBarCodeRichTextBox.Text.Length;
            ProductSeesRichTextBox.SelectionStart = ProductSeesRichTextBox.Text.Length;
            ProductPriceRichTextBox.SelectionStart = ProductPriceRichTextBox.Text.Length;
            DepositYearTextBox.SelectionStart = DepositYearTextBox.Text.Length;
        }

        private void SetCursorsAtTextBoxStringEnd()
        {
            ProductTypeTextBox.SelectionStart = ProductTypeTextBox.Text.Length;
            ProductTypePriceTextBox.SelectionStart = ProductTypeTextBox.Text.Length;
        }

        private void SetCursorAtProductNameStringEnd()
        {
            ProductNameRichTextBox.Focus();
            ProductNameRichTextBox.SelectionStart = ProductNameRichTextBox.Text.Length;
        }

        private void LoadAllProductsNamesToComboBox()
        {
            ExistsProductListComboBox.DataSource = null;

            ExistsProductListComboBox.Items.Clear();

            List<ProductInfoNameModel> allBuyersNames = _productInfoRepository.GetAllProductsNames().ToList();

            ExistsProductListComboBox.DataSource = allBuyersNames;

            ExistsProductListComboBox.DisplayMember = "ProductName";
        }

        private void LoadProductInfoToTextBoxes()
        {
            int year = int.Parse(DepositYearTextBox.Text);
            FullProductInfoModel getFullProductInfo = _productInfoRepository.GetFullProductInfo(ExistsProductListComboBox.Text, year);

            if (getFullProductInfo != null)
            {
                ProductNameRichTextBox.Text = getFullProductInfo.ProductName;
                ProductBarCodeRichTextBox.Text = getFullProductInfo.BarCode;
                ProductSeesRichTextBox.Text = getFullProductInfo.ProductSees;
                ProductPriceRichTextBox.Text = getFullProductInfo.ProductPrice.HasValue
                    ? getFullProductInfo.ProductPrice.Value.ToString(CultureInfo.InvariantCulture)
                    : string.Empty;

                ProductTypeTextBox.Text = getFullProductInfo.ProductType;
                ProductTypePriceTextBox.Text = getFullProductInfo.ProductTypePrice.HasValue
                    ? getFullProductInfo.ProductTypePrice.Value.ToString(CultureInfo.InvariantCulture)
                    : string.Empty;

                DepositYearTextBox.Text = getFullProductInfo.Year.ToString();

                _lastProductInfo[0] = ProductNameRichTextBox.Text;
                _lastProductInfo[1] = ProductBarCodeRichTextBox.Text;
                _lastProductInfo[2] = ProductSeesRichTextBox.Text;
                _lastProductInfo[3] = ProductPriceRichTextBox.Text;

                _lastProductInfo[4] = ProductTypeTextBox.Text;
                _lastProductInfo[5] = ProductTypePriceTextBox.Text;

                _lastProductInfo[6] = DepositYearTextBox.Text;
            }
            else
            {
                _messageDialogService.ShowErrorMassage("Nėra informacijos kurią būtų galima sukelti supildykite bent vieną produkto informaciją arba nera tokio įrašo dėl blogai pasirinktų metų");
            }
        }

        private bool CheckIsAllInfoFilled()
        {
            bool isAllInfoFilled = !(string.IsNullOrWhiteSpace(ProductBarCodeRichTextBox.Text) &&
                                     string.IsNullOrWhiteSpace(ProductSeesRichTextBox.Text) &&
                                     string.IsNullOrWhiteSpace(ProductPriceRichTextBox.Text) &&
                                     string.IsNullOrWhiteSpace(ProductTypeTextBox.Text) &&
                                     string.IsNullOrWhiteSpace(ProductTypePriceTextBox.Text));

            return isAllInfoFilled;
        }

        private void CreateNewProduct()
        {
            ChangeCommaToDotForNumbersTextBox();

            double? productPrice = _numberService.ParseToDoubleOrNull(ProductPriceRichTextBox);
            double? productTypePrice = _numberService.ParseToDoubleOrNull(ProductTypePriceTextBox);

            FullProductInfoModel newProduct = new FullProductInfoModel
            {
                Year = int.Parse(DepositYearTextBox.Text),
                ProductName = ProductNameRichTextBox.Text,
                BarCode = ProductBarCodeRichTextBox.Text,
                ProductSees = ProductSeesRichTextBox.Text,
                ProductPrice = productPrice,
                
                ProductType = ProductTypeTextBox.Text,
                ProductTypePrice = productTypePrice
            };

            bool isProductCreated = _productInfoRepository.CreateNewProductInfo(newProduct);

            if (isProductCreated)
            {
                CreateNewDepositInfo();
                _messageDialogService.ShowInfoMessage("Naujas produktas pridėtas į duomenų bazę");
            }
            else
            {
                _messageDialogService.ShowErrorMassage("kažkas nepavyko kreiptis į administratorių ar bandykit dar kartą");
            }
        }

        private void UpdateProductInfo()
        {
            ChangeCommaToDotForNumbersTextBox();

            double? productPrice = _numberService.ParseToDoubleOrNull(ProductPriceRichTextBox);
            double? productTypePrice = _numberService.ParseToDoubleOrNull(ProductTypePriceTextBox);

            FullProductInfoModel updateProduct = new FullProductInfoModel
            {
                Year = int.Parse(DepositYearTextBox.Text),
                ProductName = ProductNameRichTextBox.Text,
                BarCode = ProductBarCodeRichTextBox.Text,
                ProductSees = ProductSeesRichTextBox.Text,
                ProductPrice = productPrice,

                ProductType = ProductTypeTextBox.Text,
                ProductTypePrice = productTypePrice
            };

            bool isProductUpdated = _productInfoRepository.UpdateProduct(updateProduct);

            if (isProductUpdated)
            {
                _messageDialogService.ShowInfoMessage(" Produktas atnaujintas sekmingai");
            }
            else
            {
                _messageDialogService.ShowErrorMassage("kažkas nepavyko kreiptis į administratorių ar bandykit dar kartą");
            }
        }

        private bool CheckIsProductAllValuesSameAsInDataBase()
        {
            ChangeCommaToDotForNumbersTextBox();

            bool isAllValuesSameAsinDatabase = _lastProductInfo[0] == ProductNameRichTextBox.Text &&
                                               _lastProductInfo[1] == ProductBarCodeRichTextBox.Text &&
                                               _lastProductInfo[2] == ProductSeesRichTextBox.Text &&
                                               _lastProductInfo[3] == ProductPriceRichTextBox.Text &&
                                               _lastProductInfo[4] == ProductTypeTextBox.Text &&
                                               _lastProductInfo[5] == ProductTypePriceTextBox.Text;

            return isAllValuesSameAsinDatabase;
        }

        private void ChangeCommaToDotForNumbersTextBox()
        {
            ProductPriceRichTextBox.Text = _numberService.ChangeCommaToDot(ProductPriceRichTextBox);
            ProductTypePriceTextBox.Text = _numberService.ChangeCommaToDot(ProductTypePriceTextBox);
        }

        private void CreateNewDepositInfo()
        {
            int lastId = _productInfoRepository.GetBiggestProductId();

            var newDepositInfo = new FullDepositProductModel
            {
                Id = lastId,
                InvoiceYear = int.Parse(DepositYearTextBox.Text),
                ProductName = ProductNameRichTextBox.Text,
                BarCode = ProductBarCodeRichTextBox.Text,
                ProductQuantity = 0
            };

            _depositRepository.CreateNewDepositProduct(newDepositInfo);
        }

        private void SetButtonControl(bool isButtonActive)
        {
            NewProductButton.Enabled = isButtonActive;
            UpdateProductButton.Enabled = isButtonActive;
            ChooseProductButton.Enabled = isButtonActive;
        }

        #endregion
    }
}
