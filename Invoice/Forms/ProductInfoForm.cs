using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Invoice.Constants;
using Invoice.Models.ProductInfo;
using Invoice.Repositories;
using Invoice.Service;

namespace Invoice.Forms
{
    public partial class ProductInfoForm : Form
    {
        private readonly ProductInfoRepository _productInfoRepository;
        private readonly MessageDialogService _messageDialogService;

        private string[] _lastProductInfo = new string[6];

        public ProductInfoForm()
        {
            _productInfoRepository = new ProductInfoRepository();
            _messageDialogService = new MessageDialogService();

            InitializeComponent();

            SetControlsInitialState();
            SetTextBoxMaxLength();

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
                this.SelectNextControl((Control)sender, true, true, true, true);
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

        private void RichTextBox_TextChanged(object sender, System.EventArgs e)
        {
            RichTextBox richTextBox = sender as RichTextBox;

            if (richTextBox == null) return;

            if (richTextBox.SelectionStart == richTextBox.MaxLength)
            {
                _messageDialogService.ShowInfoMessage($"Pasiektas maksimalus žodžių ilgis bus išsaugota tik toks tekstas ({richTextBox.Text}) ");
            }
        }

        private void TextBox_TextChanged(object sender, System.EventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (textBox == null) return;

            if (textBox.SelectionStart == textBox.MaxLength)
            {
                _messageDialogService.ShowInfoMessage($"Pasiektas maksimalus žodžių ilgis bus išsaugota tik toks tekstas ({textBox.Text}) ");
            }
        }

        private void ChooseProductButton_Click(object sender, System.EventArgs e)
        {
            LoadProductInfoToTextBoxes();

            ProductBarCodeRichTextBox.Focus();
            ProductBarCodeRichTextBox.SelectionStart = ProductBarCodeRichTextBox.Text.Length;
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
        }

        private void SetCursorsAtRichTextBoxStringEnd()
        {
            ProductNameRichTextBox.SelectionStart = ProductNameRichTextBox.Text.Length;
            ProductBarCodeRichTextBox.SelectionStart = ProductBarCodeRichTextBox.Text.Length;
            ProductSeesRichTextBox.SelectionStart = ProductSeesRichTextBox.Text.Length;
            ProductPriceRichTextBox.SelectionStart = ProductPriceRichTextBox.Text.Length;
        }

        private void SetCursorsAtTextBoxStringEnd()
        {
            ProductTypeTextBox.SelectionStart = ProductTypeTextBox.Text.Length;
            ProductTypePriceTextBox.SelectionStart = ProductTypeTextBox.Text.Length;
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
            FullProductInfoModel getFullProductInfo = _productInfoRepository.GetFullProductInfo(ExistsProductListComboBox.Text);

            if (getFullProductInfo != null)
            {
                ProductNameRichTextBox.Text = getFullProductInfo.ProductName;
                ProductBarCodeRichTextBox.Text = getFullProductInfo.BarCode;
                ProductSeesRichTextBox.Text = getFullProductInfo.ProductSees;
                ProductPriceRichTextBox.Text = getFullProductInfo.ProductPrice.ToString(CultureInfo.InvariantCulture);

                ProductTypeTextBox.Text = getFullProductInfo.ProductType;
                ProductTypePriceTextBox.Text = getFullProductInfo.ProductTypePrice.ToString(CultureInfo.InvariantCulture);

                _lastProductInfo[0] = ProductNameRichTextBox.Text;
                _lastProductInfo[1] = ProductBarCodeRichTextBox.Text;
                _lastProductInfo[2] = ProductSeesRichTextBox.Text;
                _lastProductInfo[3] = ProductPriceRichTextBox.Text;

                _lastProductInfo[4] = ProductTypeTextBox.Text;
                _lastProductInfo[5] = ProductTypePriceTextBox.Text;
            }
            else
            {
                _messageDialogService.ShowErrorMassage("Nėra informacijos kurią būtų galima sukelti supildykite bent vieną produkto informaciją ");
            }
        }



        #endregion

        
    }
}
