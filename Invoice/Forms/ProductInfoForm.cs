using System.Collections.Generic;
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

        #endregion
       
    }
}
