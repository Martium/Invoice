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
using Invoice.Models.ProductType;
using Invoice.Repositories;
using Invoice.Service;

namespace Invoice.Forms
{
    public partial class ProductTypeStorageForm : Form
    {
        private readonly ProductTypeRepository _productTypeRepository;

        private readonly StorageRepository _storageRepository;

        private readonly MessageDialogService _messageDialogService;

        private readonly NumberService _numberService;

        private List<SpecificNameProductTypeModel> _specificProductTypeAllInfo;

        private List<StorageModel> _storageInfo;

        private const int StorageColumnCount = 7;

        private const int ProductTypeQuantityIndex = 3;
        private const int ProductTypePriceIndex = 4;

        private const int StorageIdIndex = 0;
        private const int StorageSerialNumberIndex = 1;
        private const int StorageProductNameIndex = 2;
        private const int StorageMadeDateIndex = 3;
        private const int StorageExpireDateIndex = 4;
        private const int StorageQuantityIndex = 5;
        private const int StoragePriceIndex = 6;

        private const string DateFormat = "yyyy-MM-dd";

        public ProductTypeStorageForm()
        {
            _specificProductTypeAllInfo = new List<SpecificNameProductTypeModel>();
            _productTypeRepository = new ProductTypeRepository();
            _storageRepository = new StorageRepository();
            _storageInfo = new List<StorageModel>();
            _numberService = new NumberService();
            _messageDialogService = new MessageDialogService();

            InitializeComponent();

            SetControlInitialState();
            SetTextBoxMaxLengths();

            TryFillProductTypeYearComboBox();
            TryFillProductTypeSpecificNamesToComboBox();
            TryFillStorageProductNameComboBox();
        }

        private void ProductTypeStorageForm_Load(object sender, EventArgs e)
        {
            InitializeButtonControl();

            GetAllInfoBySpecialProductTypeName();

            LoadSpecificProductTypeToDataGridView();

            CalculateFullProductTypeQuantityAndPrice(isProductType: true);
        }

        private void ProductTypeDataGridView_Paint(object sender, PaintEventArgs e)
        {
            DataGridView dataGridView = (DataGridView) sender;

            if (string.IsNullOrWhiteSpace(ProductTypeSpecificNameComboBox.Text) &&
                string.IsNullOrWhiteSpace(ProductTypeYearComboBox.Text) && ProductTypeOrStorageDataGridView.Rows.Count == 0)
            {
                DisplayEmptyListReason("Informacijos nėra sukurkite bent vieną sąskaitą", e, dataGridView);

            }
            else if (string.IsNullOrWhiteSpace(ProductTypeSpecificNameComboBox.Text) && ProductTypeOrStorageDataGridView.Rows.Count == 0)
            {
                DisplayEmptyListReason("Nesupildėte pakankamai informacijos kad galėtumėte naudotis produktų sandėliu",
                    e, dataGridView);
            }
            else if (ProductTypeOrStorageDataGridView.Rows.Count == 0)
            {
                DisplayEmptyListReason("Informacijos Nerasta pasirinkite tipą iš lentelės ir spauskite Gauti info", e,
                    dataGridView);
            }
        }

        private void GetAllInfoByProductNameButton_Click(object sender, EventArgs e)
        {
            GetAllInfoBySpecialProductTypeName();

            LoadSpecificProductTypeToDataGridView();

            CalculateFullProductTypeQuantityAndPrice(isProductType: true);
        }

        private void GetAllInfoByYearButton_Click(object sender, EventArgs e)
        {
            GetAllInfoBySpecialProductTypeNameAndYear();

            LoadSpecificProductTypeToDataGridView();

            CalculateFullProductTypeQuantityAndPrice(isProductType: true);
        }

        private void GetAllInfoStorage_Click(object sender, EventArgs e)
        {
            GetStorageAllInfo();

            LoadStorageModelToDataGridView();

            CalculateFullProductTypeQuantityAndPrice(isProductType: false);
        }

        private void GetStorageInfoByNameButton_Click(object sender, EventArgs e)
        {
            GetAllStorageInfoByProductName();

            LoadStorageModelToDataGridView();

            CalculateFullProductTypeQuantityAndPrice(isProductType: false);
        }

        private void CreateNewStorageButton_Click(object sender, EventArgs e)
        {
            bool isAllTextBoxFilled = CheckIsStorageTextBoxAllFilled();

            CreateNewStorageProduct(isAllTextBoxFilled);
        }

        private void StorageSerialNumberTextBox_TextChanged(object sender, EventArgs e)
        {
            SetTextBoxBackColorToDefault(StorageSerialNumberTextBox);
        }

        private void StorageProductNameTextBox_TextChanged(object sender, EventArgs e)
        {
            SetTextBoxBackColorToDefault(StorageProductNameTextBox);
        }

        private void StorageProductMadeDateTextBox_Validating(object sender, CancelEventArgs e)
        {
            SetTextBoxBackColorToDefault(StorageProductMadeDateTextBox);
            ValidateDateFormat(StorageProductMadeDateTextBox, e);
        }

        private void StorageProductExpireDateTextBox_Validating(object sender, CancelEventArgs e)
        {
            SetTextBoxBackColorToDefault(StorageProductExpireDateTextBox);
            ValidateDateFormat(StorageProductExpireDateTextBox, e);
        }

        private void StorageProductQuantityTextBox_TextChanged(object sender, EventArgs e)
        {
            SetTextBoxBackColorToDefault(StorageProductQuantityTextBox);
            ValidateTextIsDouble(StorageProductQuantityTextBox, StorageProductPriceTextBox, CreateNewStorageButton, UpdateStorageButton);
        }

        private void StorageProductPriceTextBox_TextChanged(object sender, EventArgs e)
        {
            SetTextBoxBackColorToDefault(StorageProductPriceTextBox);
            ValidateTextIsDouble(StorageProductPriceTextBox, StorageProductQuantityTextBox, CreateNewStorageButton, UpdateStorageButton);
        }

        private void ProductTypeDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (ProductTypeOrStorageDataGridView.ColumnCount == StorageColumnCount && ProductTypeOrStorageDataGridView.Rows.Count != 0)
            {
                int rowIndex = e.RowIndex;

                DateTime expireDate = DateTime.Parse(ProductTypeOrStorageDataGridView.Rows[rowIndex].Cells[StorageExpireDateIndex].Value.ToString());

                int monthDifference = ((expireDate.Year - DateTime.Now.Year) * 12) + expireDate.Month - DateTime.Now.Month;

                if (monthDifference >= 6)
                {
                    ProductTypeOrStorageDataGridView.Rows[rowIndex].DefaultCellStyle.BackColor = Color.Chartreuse;
                }
                else if (monthDifference >= 3)
                {
                    ProductTypeOrStorageDataGridView.Rows[rowIndex].DefaultCellStyle.BackColor = Color.Yellow;
                }
                else if (monthDifference >= 1)
                {
                    ProductTypeOrStorageDataGridView.Rows[rowIndex].DefaultCellStyle.BackColor = Color.Red;
                }
                else
                {
                    ProductTypeOrStorageDataGridView.Rows[rowIndex].DefaultCellStyle.BackColor = Color.Black;
                }
            }
        }

        private void StoreInfoFromDataGridViewButton_Click(object sender, EventArgs e)
        {
            TryStoreStorageInfoFromDataGridViewToTextBox();
        }

        private void UpdateStorageButton_Click(object sender, EventArgs e)
        {
            if (ProductTypeOrStorageDataGridView.ColumnCount == StorageColumnCount && ProductTypeOrStorageDataGridView.Rows.Count != 0)
            {
                bool isAllTextBoxFilled = CheckIsStorageTextBoxAllFilled();

                UpdateStorageProduct(isAllTextBoxFilled);
            }
            else if (ProductTypeOrStorageDataGridView.ColumnCount != StorageColumnCount && ProductTypeOrStorageDataGridView.Rows.Count != 0)
            {
                _messageDialogService.ShowErrorMassage("Produktų tipas nepriklauso sandėlio informacijai, informaciją kurią pasirinkote, priklauso Sąskaitoms faktūroms");
            }
            else
            {
                _messageDialogService.ShowErrorMassage("Nėra jokios informacijos, kurią būtų galima atnaujinti");
            }
        }

        private void AddStorageQuantityButton_Click(object sender, EventArgs e)
        {
            if (ProductTypeOrStorageDataGridView.ColumnCount == StorageColumnCount && ProductTypeOrStorageDataGridView.Rows.Count != 0)
            {
                bool isStorageQuantityTextBoxFilled = CheckAddStorageQuantityTextBoxIsFilled();
                UpdateStorageQuantity(isStorageQuantityTextBoxFilled);
            }
            else if (ProductTypeOrStorageDataGridView.ColumnCount != StorageColumnCount && ProductTypeOrStorageDataGridView.Rows.Count != 0)
            {
                _messageDialogService.ShowErrorMassage("Produktų tipas nepriklauso sandėlio informacijai, informaciją kurią pasirinkote, priklauso Sąskaitoms faktūroms");
            }
            else
            {
                _messageDialogService.ShowErrorMassage("Nėra jokios informacijos, kurią būtų galima atnaujinti");
            }
        }

        private void AddStorageQuantityTextBox_TextChanged(object sender, EventArgs e)
        {
            SetTextBoxBackColorToDefault(AddStorageQuantityTextBox);

            bool isNumber = ValidateTextIsDouble(AddStorageQuantityTextBox, AddStorageQuantityButton);

            ChangeButtonTextIfNumberIsPositiveOrNegative(isNumber);
        }

        private void DeleteStorageButton_Click(object sender, EventArgs e)
        {
            DeleteStorageProduct();
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

        private void StorageTextBoxes_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
                SetCursorAtTextBoxStringEnd();
            }
        }

        private void ProductTypeOrStorageDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (ProductTypeOrStorageDataGridView.Rows.Count != 0 && ProductTypeOrStorageDataGridView.ColumnCount == StorageColumnCount)
            {
                DateTime expireDate = DateTime.Parse(ProductTypeOrStorageDataGridView.SelectedRows[0].Cells[StorageExpireDateIndex].Value.ToString());

                int monthDifference = ((expireDate.Year - DateTime.Now.Year) * 12) + expireDate.Month - DateTime.Now.Month;

                StorageProductMonthsLeftTextBox.Text = monthDifference.ToString();
            }
        }

        #region Helpers

        private void SetControlInitialState()
        {
            this.StartPosition = FormStartPosition.CenterScreen;

            ProductTypeOrStorageDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            ProductTypeYearComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            ProductTypeSpecificNameComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            StorageProductNameListComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void CreateNewStorageProduct(bool isAllTextBoxFilled)
        {
            if (isAllTextBoxFilled)
            {
                StorageProductQuantityTextBox.Text = _numberService.ChangeCommaToDot(StorageProductQuantityTextBox);
                StorageProductPriceTextBox.Text = _numberService.ChangeCommaToDot(StorageProductPriceTextBox);

                NewProductStorageModel newProductStorage = new NewProductStorageModel
                {
                    StorageSerialNumber = StorageSerialNumberTextBox.Text,
                    StorageProductName = StorageProductNameTextBox.Text,
                    StorageProductMadeDate = DateTime.ParseExact(StorageProductMadeDateTextBox.Text, DateFormat,
                        CultureInfo.InvariantCulture),
                    StorageProductExpireDate = DateTime.ParseExact(StorageProductExpireDateTextBox.Text, DateFormat,
                        CultureInfo.InvariantCulture),
                    StorageProductQuantity = double.Parse(StorageProductQuantityTextBox.Text),
                    StorageProductPrice = double.Parse(StorageProductPriceTextBox.Text)
                };

                bool isCreated = _storageRepository.CreateNewProduct(newProductStorage);

                if (isCreated)
                {
                    _messageDialogService.ShowInfoMessage("Sekmingai išsaugota į Sandėlį");
                    TryFillStorageProductNameComboBox();
                    GetAllInfoStorage_Click(this, new EventArgs());

                    GetAllInfoStorage.Enabled = true;
                    GetStorageInfoByNameButton.Enabled = true;
                }
                else
                {
                    _messageDialogService.ShowErrorMassage("Nepavyko išsaugoti");
                }
            }
        }

        private void DeleteStorageProduct()
        {
            if (ProductTypeOrStorageDataGridView.ColumnCount == StorageColumnCount && ProductTypeOrStorageDataGridView.Rows.Count != 0)
            {
                int id = int.Parse(ProductTypeOrStorageDataGridView.SelectedRows[0].Cells[StorageIdIndex].Value.ToString());
                bool isDeleted = _storageRepository.DeleteStorageProductById(id);

                if (isDeleted)
                {
                    _messageDialogService.ShowInfoMessage("Sekmingai Ištrinta");
                    TryFillStorageProductNameComboBox();
                    GetAllInfoStorage_Click(this, new EventArgs());
                }
                else
                {
                    _messageDialogService.ShowErrorMassage("Nepavyko ištrinti");
                }
            }
            else if (ProductTypeOrStorageDataGridView.ColumnCount != StorageColumnCount && ProductTypeOrStorageDataGridView.Rows.Count != 0)
            {
                _messageDialogService.ShowErrorMassage("Produktų tipas nepriklauso sandėlio informacijai, informaciją kurią pasirinkote priklauso Sąskaitoms faktūroms");
            }
            else
            {
                _messageDialogService.ShowErrorMassage("Nėra jokios informacijos kurią būtų galima trinti");
            }
        }

        private void UpdateStorageQuantity(bool isStorageQuantityTextBoxFilled)
        {
            if (isStorageQuantityTextBoxFilled)
            {
                AddStorageQuantityTextBox.Text = _numberService.ChangeCommaToDot(AddStorageQuantityTextBox);
                double add = double.Parse(AddStorageQuantityTextBox.Text);
                int id = int.Parse(ProductTypeOrStorageDataGridView.SelectedRows[0].Cells[StorageIdIndex].Value.ToString());

                bool isAdd = _storageRepository.UpdateQuantityById(add, id);

                if (isAdd)
                {
                    _messageDialogService.ShowInfoMessage("Atnaujinta Sėkmingai");
                    GetAllInfoStorage_Click(this, new EventArgs());
                }
                else
                {
                    _messageDialogService.ShowErrorMassage("Neatnaujinta bandykit dar kartą (Kreiptis į Administratorių)");
                }
            }
        }

        private void UpdateStorageProduct(bool isAllTextBoxFilled)
        {
            if (isAllTextBoxFilled)
            {
                StorageProductQuantityTextBox.Text = _numberService.ChangeCommaToDot(StorageProductQuantityTextBox);
                StorageProductPriceTextBox.Text = _numberService.ChangeCommaToDot(StorageProductPriceTextBox);

                StorageModel updateStorage = new StorageModel
                {
                    Id = int.Parse(ProductTypeOrStorageDataGridView.SelectedRows[0].Cells[StorageIdIndex].Value.ToString()),
                    StorageSerialNumber = StorageSerialNumberTextBox.Text,
                    StorageProductName = StorageProductNameTextBox.Text,
                    StorageProductMadeDate = DateTime.ParseExact(StorageProductMadeDateTextBox.Text, DateFormat,
                        CultureInfo.InvariantCulture),
                    StorageProductExpireDate = DateTime.ParseExact(StorageProductExpireDateTextBox.Text, DateFormat,
                        CultureInfo.InvariantCulture),
                    StorageProductQuantity = double.Parse(StorageProductQuantityTextBox.Text),
                    StorageProductPrice = double.Parse(StorageProductPriceTextBox.Text)
                };

                bool isUpdated = _storageRepository.UpdateProduct(updateStorage);

                if (isUpdated)
                {
                    _messageDialogService.ShowInfoMessage("Sekmingai atnaujinta");
                    TryFillStorageProductNameComboBox();
                    GetAllInfoStorage_Click(this, new EventArgs());
                }
                else
                {
                    _messageDialogService.ShowErrorMassage("Nepavyko atnaujinti");
                }
            }
        }

        private bool CheckIsStorageTextBoxAllFilled()
        {
            bool isAllTextBoxFilled = true;

            if (string.IsNullOrWhiteSpace(StorageSerialNumberTextBox.Text) ||
                string.IsNullOrWhiteSpace(StorageProductNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(StorageProductMadeDateTextBox.Text) ||
                string.IsNullOrWhiteSpace(StorageProductExpireDateTextBox.Text) ||
                string.IsNullOrWhiteSpace(StorageProductQuantityTextBox.Text) ||
                string.IsNullOrWhiteSpace(StorageProductPriceTextBox.Text))
            {
                if (string.IsNullOrWhiteSpace(StorageSerialNumberTextBox.Text))
                    StorageSerialNumberTextBox.BackColor = Color.Yellow;

                if (string.IsNullOrWhiteSpace(StorageProductNameTextBox.Text))
                    StorageProductNameTextBox.BackColor = Color.Yellow;

                if (string.IsNullOrWhiteSpace(StorageProductMadeDateTextBox.Text))
                    StorageProductMadeDateTextBox.BackColor = Color.Yellow;

                if (string.IsNullOrWhiteSpace(StorageProductExpireDateTextBox.Text))
                    StorageProductExpireDateTextBox.BackColor = Color.Yellow;

                if (string.IsNullOrWhiteSpace(StorageProductQuantityTextBox.Text))
                    StorageProductQuantityTextBox.BackColor = Color.Yellow;

                if (string.IsNullOrWhiteSpace(StorageProductPriceTextBox.Text))
                    StorageProductPriceTextBox.BackColor = Color.Yellow;

                _messageDialogService.ShowErrorMassage("Geltoni langeliai turi būt supildyti");
                isAllTextBoxFilled = false;
            }

            return isAllTextBoxFilled;
        }

        private bool CheckAddStorageQuantityTextBoxIsFilled()
        {
            bool isStorageQuantityTextBoxFilled = true;

            if (string.IsNullOrWhiteSpace(AddStorageQuantityTextBox.Text))
            {
                AddStorageQuantityTextBox.BackColor = Color.Yellow;
                _messageDialogService.ShowErrorMassage("Geltoni langeliai turi būt supildyti");
                isStorageQuantityTextBoxFilled = false;
            }

            return isStorageQuantityTextBoxFilled;
        }

        private void LoadSpecificProductTypeToDataGridView()
        {
            var bidingSourceModel = new SpecificNameProductTypeModel();

            BindingSource bindingSource = new BindingSource {bidingSourceModel};

            bindingSource.DataSource = _specificProductTypeAllInfo;

            ProductTypeOrStorageDataGridView.DataSource = bindingSource;

            ChangeDataGridViewHeaderText(isProductType: true);

            SetInformationOfDataGridViewTypeLabel(ProductTypeOrStorageListOperation.ProductType);
        }

        private void LoadStorageModelToDataGridView()
        {
            var bidingSourceModel = new StorageModel();

            BindingSource bindingSource = new BindingSource {bidingSourceModel};

            bindingSource.DataSource = _storageInfo;

            ProductTypeOrStorageDataGridView.DataSource = bindingSource;

            ChangeDataGridViewHeaderText(isProductType: false);
            ChangeDataGridViewHeadersSize();

            SetInformationOfDataGridViewTypeLabel(ProductTypeOrStorageListOperation.Storage);
        }

        private void GetStorageAllInfo()
        {
            _storageInfo.Clear();

            _storageInfo = _storageRepository.GetStorageInfo().ToList();
        }

        private void GetAllStorageInfoByProductName()
        {
            _storageInfo.Clear();

            _storageInfo = _storageRepository.GetAllInfoByProductName(StorageProductNameListComboBox.Text).ToList();
        }

        private void ChangeDataGridViewHeaderText(bool isProductType)
        {
            if (isProductType)
            {
                ProductTypeOrStorageDataGridView.Columns[0].HeaderText = @"Sąskaitos Nr.";
                ProductTypeOrStorageDataGridView.Columns[1].HeaderText = @"Metai";
                ProductTypeOrStorageDataGridView.Columns[2].HeaderText = @"Tipas";
                ProductTypeOrStorageDataGridView.Columns[ProductTypeQuantityIndex].HeaderText = @"Kiekis";
                ProductTypeOrStorageDataGridView.Columns[ProductTypePriceIndex].HeaderText = @"Vnt. Kaina";
            }
            else
            {
                ProductTypeOrStorageDataGridView.Columns[StorageSerialNumberIndex].HeaderText = @"Serija";
                ProductTypeOrStorageDataGridView.Columns[StorageProductNameIndex].HeaderText = @"Tipas";
                ProductTypeOrStorageDataGridView.Columns[StorageMadeDateIndex].HeaderText = @"Pagaminimo Data";
                ProductTypeOrStorageDataGridView.Columns[StorageExpireDateIndex].HeaderText = @"Galiojimo Data";
                ProductTypeOrStorageDataGridView.Columns[StorageQuantityIndex].HeaderText = @"Kiekis";
                ProductTypeOrStorageDataGridView.Columns[StoragePriceIndex].HeaderText = @"Kaina";
            }
        }

        private void ChangeDataGridViewHeadersSize()
        {
            ProductTypeOrStorageDataGridView.Columns[StorageIdIndex].Width = 30;
            ProductTypeOrStorageDataGridView.Columns[StorageMadeDateIndex].Width = 70;
            ProductTypeOrStorageDataGridView.Columns[StorageExpireDateIndex].Width = 70;
            ProductTypeOrStorageDataGridView.Columns[StorageQuantityIndex].Width = 60;
            ProductTypeOrStorageDataGridView.Columns[StoragePriceIndex].Width = 60;
        }

        private static void DisplayEmptyListReason(string reason, PaintEventArgs e, DataGridView dataGridView)
        {
            using (Graphics graphics = e.Graphics)
            {
                int leftPadding = 2;
                int topPadding = 41;
                int rowSelectionColumnWidth = 40;
                int messageBackgroundWidth = dataGridView.Columns.GetColumnsWidth(DataGridViewElementStates.Displayed) +
                                             rowSelectionColumnWidth;
                int messageBackgroundHeight = 25;

                graphics.FillRectangle(
                    Brushes.White,
                    new Rectangle(
                        new Point(leftPadding, topPadding),
                        new Size(messageBackgroundWidth, messageBackgroundHeight)
                    )
                );
                graphics.DrawString(
                    reason,
                    new Font("Times New Roman", 12),
                    Brushes.DarkGray,
                    new PointF(leftPadding, topPadding));
            }
        }

        private void CalculateFullProductTypeQuantityAndPrice(bool isProductType)
        {
            int rowsCount = ProductTypeOrStorageDataGridView.Rows.Count;

            if (isProductType)
            {
                double calculateFullProductTypeQuantity =
                    _numberService.SumAllDataGridViewRowsSpecificColumns(ProductTypeOrStorageDataGridView, rowsCount,
                        ProductTypeQuantityIndex);

               // double calculateFullProductTypePrice = _numberService.SumAllDataGridViewRowsSpecificColumns(ProductTypeOrStorageDataGridView, rowsCount, ProductTypePriceIndex);

               double calculateFullProductTypePrice = _numberService.SumAllDataGridViewRowsTwoSpecificColumns(
                   ProductTypeOrStorageDataGridView, rowsCount, ProductTypePriceIndex, ProductTypeQuantityIndex);

                FullProductTypeQuantityTextBox.Text =
                    calculateFullProductTypeQuantity.ToString(CultureInfo.InvariantCulture);
                FullProductTypePriceTextBox.Text = calculateFullProductTypePrice.ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                double calculateFullStorageQuantity =
                    _numberService.SumAllDataGridViewRowsSpecificColumns(ProductTypeOrStorageDataGridView, rowsCount,
                        StorageQuantityIndex);

               // double calculateFullStoragePrice = _numberService.SumAllDataGridViewRowsSpecificColumns(ProductTypeOrStorageDataGridView, rowsCount, StoragePriceIndex);
               double calculateFullStoragePrice =
                   _numberService.SumAllDataGridViewRowsTwoSpecificColumns(ProductTypeOrStorageDataGridView, rowsCount,
                       StoragePriceIndex, StorageQuantityIndex);

                FullProductTypeQuantityTextBox.Text =
                    calculateFullStorageQuantity.ToString(CultureInfo.InvariantCulture);
                FullProductTypePriceTextBox.Text = calculateFullStoragePrice.ToString(CultureInfo.InvariantCulture);
            }

        }

        private void TryFillProductTypeSpecificNamesToComboBox()
        {
            ProductTypeSpecificNameComboBox.Items.Clear();

            IEnumerable<ProductTypeSpecificNamesModel>
                getInfo = _productTypeRepository.GetAllExistingProductTypeNames();

            foreach (var info in getInfo)
            {
                if (info.FirstProductType != string.Empty)
                    ProductTypeSpecificNameComboBox.Items.Add(info.FirstProductType);

                if (info.SecondProductType != string.Empty)
                    ProductTypeSpecificNameComboBox.Items.Add(info.SecondProductType);

                if (info.ThirdProductType != string.Empty)
                    ProductTypeSpecificNameComboBox.Items.Add(info.ThirdProductType);

                if (info.FourthProductType != string.Empty)
                    ProductTypeSpecificNameComboBox.Items.Add(info.FourthProductType);

                if (info.FifthProductType != string.Empty)
                    ProductTypeSpecificNameComboBox.Items.Add(info.FifthProductType);

                if (info.SixthProductType != string.Empty)
                    ProductTypeSpecificNameComboBox.Items.Add(info.SixthProductType);

                if (info.SeventhProductType != string.Empty)
                    ProductTypeSpecificNameComboBox.Items.Add(info.SeventhProductType);

                if (info.EighthProductType != string.Empty)
                    ProductTypeSpecificNameComboBox.Items.Add(info.EighthProductType);

                if (info.NinthProductType != string.Empty)
                    ProductTypeSpecificNameComboBox.Items.Add(info.NinthProductType);

                if (info.TenProductType != string.Empty)
                    ProductTypeSpecificNameComboBox.Items.Add(info.TenProductType);

                if (info.EleventhProductType != string.Empty)
                    ProductTypeSpecificNameComboBox.Items.Add(info.EleventhProductType);

                if (info.TwelfthProductType != string.Empty)
                    ProductTypeSpecificNameComboBox.Items.Add(info.TwelfthProductType);

            }

            if (ProductTypeSpecificNameComboBox.Items.Count != 0)
            {
                ProductTypeSpecificNameComboBox.Text = ProductTypeSpecificNameComboBox.Items[0].ToString();
            }

        }

        private void TryFillProductTypeYearComboBox()
        {
            ProductTypeYearComboBox.Items.Clear();

            IEnumerable<int> allYears = _productTypeRepository.GetAllExistingProductTypeYears();

            foreach (var year in allYears)
            {
                ProductTypeYearComboBox.Items.Add(year);
            }

            if (ProductTypeYearComboBox.Items.Count != 0)
            {
                ProductTypeYearComboBox.Text = ProductTypeYearComboBox.Items[0].ToString();
            }
        }

        private void GetAllInfoBySpecialProductTypeName()
        {
            _specificProductTypeAllInfo.Clear();

            IEnumerable<FirstSpecificProductTypeModel> firstSpecificProductInfo =
                _productTypeRepository.GetSpecificProductTypeFullInfoBySpecialName(ProductTypeSpecificNameComboBox.Text,
                    ProductTypeOperations.FirstProductType);
            IEnumerable<SecondSpecificProductTypeModel> secondSpecificProductInfo =
                _productTypeRepository.GetSpecificProductTypeFullInfoBySpecialName(ProductTypeSpecificNameComboBox.Text,
                    ProductTypeOperations.SecondProductType);
            IEnumerable<ThirdSpecificProductTypeModel> thirdSpecificProductInfo =
                _productTypeRepository.GetSpecificProductTypeFullInfoBySpecialName(ProductTypeSpecificNameComboBox.Text,
                    ProductTypeOperations.ThirdProductType);
            IEnumerable<FourthSpecificProductTypeModel> fourthSpecificProductInfo =
                _productTypeRepository.GetSpecificProductTypeFullInfoBySpecialName(ProductTypeSpecificNameComboBox.Text,
                    ProductTypeOperations.FourthProductType);
            IEnumerable<FifthSpecificProductTypeModel> fifthSpecificProductInfo =
                _productTypeRepository.GetSpecificProductTypeFullInfoBySpecialName(ProductTypeSpecificNameComboBox.Text,
                    ProductTypeOperations.FifthProductType);
            IEnumerable<SixthSpecificProductTypeModel> sixthSpecificProductInfo =
                _productTypeRepository.GetSpecificProductTypeFullInfoBySpecialName(ProductTypeSpecificNameComboBox.Text,
                    ProductTypeOperations.SixthProductType);
            IEnumerable<SeventhSpecificProductTypeModel> seventhSpecificProductInfo =
                _productTypeRepository.GetSpecificProductTypeFullInfoBySpecialName(ProductTypeSpecificNameComboBox.Text,
                    ProductTypeOperations.SeventhProductType);
            IEnumerable<EighthSpecificProductTypeModel> eighthSpecificProductInfo =
                _productTypeRepository.GetSpecificProductTypeFullInfoBySpecialName(ProductTypeSpecificNameComboBox.Text,
                    ProductTypeOperations.EighthProductType);
            IEnumerable<NinthSpecificProductTypeModel> ninthSpecificProductInfo =
                _productTypeRepository.GetSpecificProductTypeFullInfoBySpecialName(ProductTypeSpecificNameComboBox.Text,
                    ProductTypeOperations.NinthProductType);
            IEnumerable<TenSpecificProductTypeModel> tenSpecificProductInfo =
                _productTypeRepository.GetSpecificProductTypeFullInfoBySpecialName(ProductTypeSpecificNameComboBox.Text,
                    ProductTypeOperations.TenProductType);
            IEnumerable<EleventhSpecificProductTypeModel> eleventhSpecificProductInfo =
                _productTypeRepository.GetSpecificProductTypeFullInfoBySpecialName(ProductTypeSpecificNameComboBox.Text,
                    ProductTypeOperations.EleventhProductType);
            IEnumerable<TwelfthSpecificProductTypeModel> twelfthSpecificProductInfo =
                _productTypeRepository.GetSpecificProductTypeFullInfoBySpecialName(ProductTypeSpecificNameComboBox.Text,
                    ProductTypeOperations.TwelfthProductType);

            _specificProductTypeAllInfo = (from firstInfo in firstSpecificProductInfo
                where firstInfo.FirstProductTypeQuantity.HasValue && firstInfo.FirstProductTypePrice.HasValue
                select new SpecificNameProductTypeModel()
                {
                    IdByInvoiceNumber = firstInfo.IdByInvoiceNumber,
                    IdByInvoiceNumberYearCreation = firstInfo.IdByInvoiceNumberYearCreation,
                    ProductTypeName = firstInfo.FirstProductType, Quantity = firstInfo.FirstProductTypeQuantity.Value,
                    Price = firstInfo.FirstProductTypePrice.Value
                }).ToList();

            _specificProductTypeAllInfo.AddRange(from secondInfo in secondSpecificProductInfo
                where secondInfo.SecondProductTypeQuantity.HasValue && secondInfo.SecondProductTypePrice.HasValue
                select new SpecificNameProductTypeModel()
                {
                    IdByInvoiceNumber = secondInfo.IdByInvoiceNumber,
                    IdByInvoiceNumberYearCreation = secondInfo.IdByInvoiceNumberYearCreation,
                    ProductTypeName = secondInfo.SecondProductType,
                    Quantity = secondInfo.SecondProductTypeQuantity.Value,
                    Price = secondInfo.SecondProductTypePrice.Value
                });

            _specificProductTypeAllInfo.AddRange(from thirdInfo in thirdSpecificProductInfo
                where thirdInfo.ThirdProductTypeQuantity.HasValue && thirdInfo.ThirdProductTypePrice.HasValue
                select new SpecificNameProductTypeModel()
                {
                    IdByInvoiceNumber = thirdInfo.IdByInvoiceNumber,
                    IdByInvoiceNumberYearCreation = thirdInfo.IdByInvoiceNumberYearCreation,
                    ProductTypeName = thirdInfo.ThirdProductType, Quantity = thirdInfo.ThirdProductTypeQuantity.Value,
                    Price = thirdInfo.ThirdProductTypePrice.Value
                });

            _specificProductTypeAllInfo.AddRange(from fourthInfo in fourthSpecificProductInfo
                where fourthInfo.FourthProductTypeQuantity.HasValue && fourthInfo.FourthProductTypePrice.HasValue
                select new SpecificNameProductTypeModel()
                {
                    IdByInvoiceNumber = fourthInfo.IdByInvoiceNumber,
                    IdByInvoiceNumberYearCreation = fourthInfo.IdByInvoiceNumberYearCreation,
                    ProductTypeName = fourthInfo.FourthProductType,
                    Quantity = fourthInfo.FourthProductTypeQuantity.Value,
                    Price = fourthInfo.FourthProductTypePrice.Value
                });

            _specificProductTypeAllInfo.AddRange(from fifthInfo in fifthSpecificProductInfo
                where fifthInfo.FifthProductTypeQuantity.HasValue && fifthInfo.FifthProductTypePrice.HasValue
                select new SpecificNameProductTypeModel()
                {
                    IdByInvoiceNumber = fifthInfo.IdByInvoiceNumber,
                    IdByInvoiceNumberYearCreation = fifthInfo.IdByInvoiceNumberYearCreation,
                    ProductTypeName = fifthInfo.FifthProductType, Quantity = fifthInfo.FifthProductTypeQuantity.Value,
                    Price = fifthInfo.FifthProductTypePrice.Value
                });

            _specificProductTypeAllInfo.AddRange(from sixthInfo in sixthSpecificProductInfo
                where sixthInfo.SixthProductTypeQuantity.HasValue && sixthInfo.SixthProductTypePrice.HasValue
                select new SpecificNameProductTypeModel()
                {
                    IdByInvoiceNumber = sixthInfo.IdByInvoiceNumber,
                    IdByInvoiceNumberYearCreation = sixthInfo.IdByInvoiceNumberYearCreation,
                    ProductTypeName = sixthInfo.SixthProductType, Quantity = sixthInfo.SixthProductTypeQuantity.Value,
                    Price = sixthInfo.SixthProductTypePrice.Value
                });

            _specificProductTypeAllInfo.AddRange(from seventhInfo in seventhSpecificProductInfo
                where seventhInfo.SeventhProductTypeQuantity.HasValue && seventhInfo.SeventhProductTypePrice.HasValue
                select new SpecificNameProductTypeModel()
                {
                    IdByInvoiceNumber = seventhInfo.IdByInvoiceNumber,
                    IdByInvoiceNumberYearCreation = seventhInfo.IdByInvoiceNumberYearCreation,
                    ProductTypeName = seventhInfo.SeventhProductType,
                    Quantity = seventhInfo.SeventhProductTypeQuantity.Value,
                    Price = seventhInfo.SeventhProductTypePrice.Value
                });

            _specificProductTypeAllInfo.AddRange(from eighthInfo in eighthSpecificProductInfo
                where eighthInfo.EighthProductTypeQuantity.HasValue && eighthInfo.EighthProductTypePrice.HasValue
                select new SpecificNameProductTypeModel()
                {
                    IdByInvoiceNumber = eighthInfo.IdByInvoiceNumber,
                    IdByInvoiceNumberYearCreation = eighthInfo.IdByInvoiceNumberYearCreation,
                    ProductTypeName = eighthInfo.EighthProductType,
                    Quantity = eighthInfo.EighthProductTypeQuantity.Value,
                    Price = eighthInfo.EighthProductTypePrice.Value
                });

            _specificProductTypeAllInfo.AddRange(from ninthInfo in ninthSpecificProductInfo
                where ninthInfo.NinthProductTypeQuantity.HasValue && ninthInfo.NinthProductTypePrice.HasValue
                select new SpecificNameProductTypeModel()
                {
                    IdByInvoiceNumber = ninthInfo.IdByInvoiceNumber,
                    IdByInvoiceNumberYearCreation = ninthInfo.IdByInvoiceNumberYearCreation,
                    ProductTypeName = ninthInfo.NinthProductType, Quantity = ninthInfo.NinthProductTypeQuantity.Value,
                    Price = ninthInfo.NinthProductTypePrice.Value
                });

            _specificProductTypeAllInfo.AddRange(from tenInfo in tenSpecificProductInfo
                where tenInfo.TenProductTypeQuantity.HasValue && tenInfo.TenProductTypePrice.HasValue
                select new SpecificNameProductTypeModel()
                {
                    IdByInvoiceNumber = tenInfo.IdByInvoiceNumber,
                    IdByInvoiceNumberYearCreation = tenInfo.IdByInvoiceNumberYearCreation,
                    ProductTypeName = tenInfo.TenProductType, Quantity = tenInfo.TenProductTypeQuantity.Value,
                    Price = tenInfo.TenProductTypePrice.Value
                });

            _specificProductTypeAllInfo.AddRange(from eleventhInfo in eleventhSpecificProductInfo
                where eleventhInfo.EleventhProductTypeQuantity.HasValue &&
                      eleventhInfo.EleventhProductTypePrice.HasValue
                select new SpecificNameProductTypeModel()
                {
                    IdByInvoiceNumber = eleventhInfo.IdByInvoiceNumber,
                    IdByInvoiceNumberYearCreation = eleventhInfo.IdByInvoiceNumberYearCreation,
                    ProductTypeName = eleventhInfo.EleventhProductType,
                    Quantity = eleventhInfo.EleventhProductTypeQuantity.Value,
                    Price = eleventhInfo.EleventhProductTypePrice.Value
                });

            _specificProductTypeAllInfo.AddRange(from twelfthInfo in twelfthSpecificProductInfo
                where twelfthInfo.TwelfthProductTypeQuantity.HasValue && twelfthInfo.TwelfthProductTypePrice.HasValue
                select new SpecificNameProductTypeModel()
                {
                    IdByInvoiceNumber = twelfthInfo.IdByInvoiceNumber,
                    IdByInvoiceNumberYearCreation = twelfthInfo.IdByInvoiceNumberYearCreation,
                    ProductTypeName = twelfthInfo.TwelfthProductType,
                    Quantity = twelfthInfo.TwelfthProductTypeQuantity.Value,
                    Price = twelfthInfo.TwelfthProductTypePrice.Value
                });
        }

        private void GetAllInfoBySpecialProductTypeNameAndYear()
        {
            _specificProductTypeAllInfo.Clear();

            int year = int.Parse(ProductTypeYearComboBox.Text);

            IEnumerable<FirstSpecificProductTypeModel> firstSpecificProductInfo =
                _productTypeRepository.GetSpecificProductTypeFullInfoBySpecialNameAndYear(
                    ProductTypeSpecificNameComboBox.Text, year,
                    ProductTypeOperations.FirstProductType);
            IEnumerable<SecondSpecificProductTypeModel> secondSpecificProductInfo =
                _productTypeRepository.GetSpecificProductTypeFullInfoBySpecialNameAndYear(
                    ProductTypeSpecificNameComboBox.Text, year,
                    ProductTypeOperations.SecondProductType);
            IEnumerable<ThirdSpecificProductTypeModel> thirdSpecificProductInfo =
                _productTypeRepository.GetSpecificProductTypeFullInfoBySpecialNameAndYear(
                    ProductTypeSpecificNameComboBox.Text, year,
                    ProductTypeOperations.ThirdProductType);
            IEnumerable<FourthSpecificProductTypeModel> fourthSpecificProductInfo =
                _productTypeRepository.GetSpecificProductTypeFullInfoBySpecialNameAndYear(
                    ProductTypeSpecificNameComboBox.Text, year,
                    ProductTypeOperations.FourthProductType);
            IEnumerable<FifthSpecificProductTypeModel> fifthSpecificProductInfo =
                _productTypeRepository.GetSpecificProductTypeFullInfoBySpecialNameAndYear(
                    ProductTypeSpecificNameComboBox.Text, year,
                    ProductTypeOperations.FifthProductType);
            IEnumerable<SixthSpecificProductTypeModel> sixthSpecificProductInfo =
                _productTypeRepository.GetSpecificProductTypeFullInfoBySpecialNameAndYear(
                    ProductTypeSpecificNameComboBox.Text, year,
                    ProductTypeOperations.SixthProductType);
            IEnumerable<SeventhSpecificProductTypeModel> seventhSpecificProductInfo =
                _productTypeRepository.GetSpecificProductTypeFullInfoBySpecialNameAndYear(
                    ProductTypeSpecificNameComboBox.Text, year,
                    ProductTypeOperations.SeventhProductType);
            IEnumerable<EighthSpecificProductTypeModel> eighthSpecificProductInfo =
                _productTypeRepository.GetSpecificProductTypeFullInfoBySpecialNameAndYear(
                    ProductTypeSpecificNameComboBox.Text, year,
                    ProductTypeOperations.EighthProductType);
            IEnumerable<NinthSpecificProductTypeModel> ninthSpecificProductInfo =
                _productTypeRepository.GetSpecificProductTypeFullInfoBySpecialNameAndYear(
                    ProductTypeSpecificNameComboBox.Text, year,
                    ProductTypeOperations.NinthProductType);
            IEnumerable<TenSpecificProductTypeModel> tenSpecificProductInfo =
                _productTypeRepository.GetSpecificProductTypeFullInfoBySpecialNameAndYear(
                    ProductTypeSpecificNameComboBox.Text, year,
                    ProductTypeOperations.TenProductType);
            IEnumerable<EleventhSpecificProductTypeModel> eleventhSpecificProductInfo =
                _productTypeRepository.GetSpecificProductTypeFullInfoBySpecialNameAndYear(
                    ProductTypeSpecificNameComboBox.Text, year,
                    ProductTypeOperations.EleventhProductType);
            IEnumerable<TwelfthSpecificProductTypeModel> twelfthSpecificProductInfo =
                _productTypeRepository.GetSpecificProductTypeFullInfoBySpecialNameAndYear(
                    ProductTypeSpecificNameComboBox.Text, year,
                    ProductTypeOperations.TwelfthProductType);

            _specificProductTypeAllInfo = (from firstInfo in firstSpecificProductInfo
                where firstInfo.FirstProductTypeQuantity.HasValue && firstInfo.FirstProductTypePrice.HasValue
                select new SpecificNameProductTypeModel()
                {
                    IdByInvoiceNumber = firstInfo.IdByInvoiceNumber,
                    IdByInvoiceNumberYearCreation = firstInfo.IdByInvoiceNumberYearCreation,
                    ProductTypeName = firstInfo.FirstProductType, Quantity = firstInfo.FirstProductTypeQuantity.Value,
                    Price = firstInfo.FirstProductTypePrice.Value
                }).ToList();

            _specificProductTypeAllInfo.AddRange(from secondInfo in secondSpecificProductInfo
                where secondInfo.SecondProductTypeQuantity.HasValue && secondInfo.SecondProductTypePrice.HasValue
                select new SpecificNameProductTypeModel()
                {
                    IdByInvoiceNumber = secondInfo.IdByInvoiceNumber,
                    IdByInvoiceNumberYearCreation = secondInfo.IdByInvoiceNumberYearCreation,
                    ProductTypeName = secondInfo.SecondProductType,
                    Quantity = secondInfo.SecondProductTypeQuantity.Value,
                    Price = secondInfo.SecondProductTypePrice.Value
                });

            _specificProductTypeAllInfo.AddRange(from thirdInfo in thirdSpecificProductInfo
                where thirdInfo.ThirdProductTypeQuantity.HasValue && thirdInfo.ThirdProductTypePrice.HasValue
                select new SpecificNameProductTypeModel()
                {
                    IdByInvoiceNumber = thirdInfo.IdByInvoiceNumber,
                    IdByInvoiceNumberYearCreation = thirdInfo.IdByInvoiceNumberYearCreation,
                    ProductTypeName = thirdInfo.ThirdProductType, Quantity = thirdInfo.ThirdProductTypeQuantity.Value,
                    Price = thirdInfo.ThirdProductTypePrice.Value
                });

            _specificProductTypeAllInfo.AddRange(from fourthInfo in fourthSpecificProductInfo
                where fourthInfo.FourthProductTypeQuantity.HasValue && fourthInfo.FourthProductTypePrice.HasValue
                select new SpecificNameProductTypeModel()
                {
                    IdByInvoiceNumber = fourthInfo.IdByInvoiceNumber,
                    IdByInvoiceNumberYearCreation = fourthInfo.IdByInvoiceNumberYearCreation,
                    ProductTypeName = fourthInfo.FourthProductType,
                    Quantity = fourthInfo.FourthProductTypeQuantity.Value,
                    Price = fourthInfo.FourthProductTypePrice.Value
                });

            _specificProductTypeAllInfo.AddRange(from fifthInfo in fifthSpecificProductInfo
                where fifthInfo.FifthProductTypeQuantity.HasValue && fifthInfo.FifthProductTypePrice.HasValue
                select new SpecificNameProductTypeModel()
                {
                    IdByInvoiceNumber = fifthInfo.IdByInvoiceNumber,
                    IdByInvoiceNumberYearCreation = fifthInfo.IdByInvoiceNumberYearCreation,
                    ProductTypeName = fifthInfo.FifthProductType, Quantity = fifthInfo.FifthProductTypeQuantity.Value,
                    Price = fifthInfo.FifthProductTypePrice.Value
                });

            _specificProductTypeAllInfo.AddRange(from sixthInfo in sixthSpecificProductInfo
                where sixthInfo.SixthProductTypeQuantity.HasValue && sixthInfo.SixthProductTypePrice.HasValue
                select new SpecificNameProductTypeModel()
                {
                    IdByInvoiceNumber = sixthInfo.IdByInvoiceNumber,
                    IdByInvoiceNumberYearCreation = sixthInfo.IdByInvoiceNumberYearCreation,
                    ProductTypeName = sixthInfo.SixthProductType, Quantity = sixthInfo.SixthProductTypeQuantity.Value,
                    Price = sixthInfo.SixthProductTypePrice.Value
                });

            _specificProductTypeAllInfo.AddRange(from seventhInfo in seventhSpecificProductInfo
                where seventhInfo.SeventhProductTypeQuantity.HasValue && seventhInfo.SeventhProductTypePrice.HasValue
                select new SpecificNameProductTypeModel()
                {
                    IdByInvoiceNumber = seventhInfo.IdByInvoiceNumber,
                    IdByInvoiceNumberYearCreation = seventhInfo.IdByInvoiceNumberYearCreation,
                    ProductTypeName = seventhInfo.SeventhProductType,
                    Quantity = seventhInfo.SeventhProductTypeQuantity.Value,
                    Price = seventhInfo.SeventhProductTypePrice.Value
                });

            _specificProductTypeAllInfo.AddRange(from eighthInfo in eighthSpecificProductInfo
                where eighthInfo.EighthProductTypeQuantity.HasValue && eighthInfo.EighthProductTypePrice.HasValue
                select new SpecificNameProductTypeModel()
                {
                    IdByInvoiceNumber = eighthInfo.IdByInvoiceNumber,
                    IdByInvoiceNumberYearCreation = eighthInfo.IdByInvoiceNumberYearCreation,
                    ProductTypeName = eighthInfo.EighthProductType,
                    Quantity = eighthInfo.EighthProductTypeQuantity.Value,
                    Price = eighthInfo.EighthProductTypePrice.Value
                });

            _specificProductTypeAllInfo.AddRange(from ninthInfo in ninthSpecificProductInfo
                where ninthInfo.NinthProductTypeQuantity.HasValue && ninthInfo.NinthProductTypePrice.HasValue
                select new SpecificNameProductTypeModel()
                {
                    IdByInvoiceNumber = ninthInfo.IdByInvoiceNumber,
                    IdByInvoiceNumberYearCreation = ninthInfo.IdByInvoiceNumberYearCreation,
                    ProductTypeName = ninthInfo.NinthProductType, Quantity = ninthInfo.NinthProductTypeQuantity.Value,
                    Price = ninthInfo.NinthProductTypePrice.Value
                });

            _specificProductTypeAllInfo.AddRange(from tenInfo in tenSpecificProductInfo
                where tenInfo.TenProductTypeQuantity.HasValue && tenInfo.TenProductTypePrice.HasValue
                select new SpecificNameProductTypeModel()
                {
                    IdByInvoiceNumber = tenInfo.IdByInvoiceNumber,
                    IdByInvoiceNumberYearCreation = tenInfo.IdByInvoiceNumberYearCreation,
                    ProductTypeName = tenInfo.TenProductType, Quantity = tenInfo.TenProductTypeQuantity.Value,
                    Price = tenInfo.TenProductTypePrice.Value
                });

            _specificProductTypeAllInfo.AddRange(from eleventhInfo in eleventhSpecificProductInfo
                where eleventhInfo.EleventhProductTypeQuantity.HasValue &&
                      eleventhInfo.EleventhProductTypePrice.HasValue
                select new SpecificNameProductTypeModel()
                {
                    IdByInvoiceNumber = eleventhInfo.IdByInvoiceNumber,
                    IdByInvoiceNumberYearCreation = eleventhInfo.IdByInvoiceNumberYearCreation,
                    ProductTypeName = eleventhInfo.EleventhProductType,
                    Quantity = eleventhInfo.EleventhProductTypeQuantity.Value,
                    Price = eleventhInfo.EleventhProductTypePrice.Value
                });

            _specificProductTypeAllInfo.AddRange(from twelfthInfo in twelfthSpecificProductInfo
                where twelfthInfo.TwelfthProductTypeQuantity.HasValue && twelfthInfo.TwelfthProductTypePrice.HasValue
                select new SpecificNameProductTypeModel()
                {
                    IdByInvoiceNumber = twelfthInfo.IdByInvoiceNumber,
                    IdByInvoiceNumberYearCreation = twelfthInfo.IdByInvoiceNumberYearCreation,
                    ProductTypeName = twelfthInfo.TwelfthProductType,
                    Quantity = twelfthInfo.TwelfthProductTypeQuantity.Value,
                    Price = twelfthInfo.TwelfthProductTypePrice.Value
                });
        }

        private void InitializeButtonControl()
        {

            if (string.IsNullOrWhiteSpace(ProductTypeSpecificNameComboBox.Text) ||
                string.IsNullOrWhiteSpace(ProductTypeYearComboBox.Text))
            {
                GetAllInfoByProductNameButton.Enabled = false;
                GetAllInfoByYearButton.Enabled = false;
            }
            else
            {
                GetAllInfoByProductNameButton.Enabled = true;
                GetAllInfoByYearButton.Enabled = true;
            }

            if (string.IsNullOrWhiteSpace(StorageProductNameListComboBox.Text))
            {
                GetAllInfoStorage.Enabled = false;
                GetStorageInfoByNameButton.Enabled = false;
            }
            else
            {
                GetAllInfoStorage.Enabled = true;
                GetStorageInfoByNameButton.Enabled = true;
            }
        }

        private void TryFillStorageProductNameComboBox()
        {
            StorageProductNameListComboBox.Items.Clear();

            IEnumerable<string> allProductNames = _storageRepository.GetAllStorageInfoProductNames();

            foreach (var productName in allProductNames)
            {
                StorageProductNameListComboBox.Items.Add(productName);
            }

            if (StorageProductNameListComboBox.Items.Count != 0)
            {
                StorageProductNameListComboBox.Text = StorageProductNameListComboBox.Items[0].ToString();
            }
        }

        private void SetTextBoxMaxLengths()
        {
            StorageSerialNumberTextBox.MaxLength = FormSettings.TextBoxLengths.StorageSerialNumber;
            StorageProductNameTextBox.MaxLength = FormSettings.TextBoxLengths.StorageProductName;
        }

        private void ValidateDateFormat(TextBox textBox, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                e.Cancel = true;
                CreateNewStorageButton.Enabled = false;
                _messageDialogService.DisplayLabelAndTextBoxError(
                    $"Raudonas langelis negali būti tuščias! pvz.: {DateTime.Now.ToString(DateFormat)}", textBox, ErrorLabel);
            }
            else if (!DateTime.TryParseExact(textBox.Text, DateFormat, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out _))
            {
                e.Cancel = true;
                CreateNewStorageButton.Enabled = false;
                _messageDialogService.DisplayLabelAndTextBoxError(
                    $"Raudonam langelyje įveskite teisingą datą! pvz.: {DateTime.Now.ToString(DateFormat)}", textBox, ErrorLabel);
            }
            else
            {
                e.Cancel = false;
                CreateNewStorageButton.Enabled = true;
                _messageDialogService.HideLabelAndTextBoxError(ErrorLabel, textBox);
            }
        }

        private bool ValidateTextIsDouble(TextBox textBox, Button button)
        {
            bool isNumber = double.TryParse(textBox.Text, out _);

            if (isNumber)
            {
                button.Enabled = true;
                _messageDialogService.HideLabelAndTextBoxError(ErrorLabel, textBox);
            }
            else if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                button.Enabled = false;
                _messageDialogService.DisplayLabelAndTextBoxError("Raudonam langelis negali būti tuščias. Jame turi būti skaičius",
                    textBox, ErrorLabel);
            }
            else
            {
                button.Enabled = false;
                _messageDialogService.DisplayLabelAndTextBoxError("Raudonam langelyje turi būti skaičius (tai, ką įvedėte, nėra skaičius)",
                    textBox, ErrorLabel);
            }

            return isNumber;
        }

        private void ValidateTextIsDouble(TextBox textBox, TextBox secondTextBox, Button button, Button secondButton)
        {
            button.Enabled = false;
            secondButton.Enabled = false;

            bool isNumber = double.TryParse(textBox.Text, out _);
            bool isSecondNumber = double.TryParse(secondTextBox.Text, out _);

            if (isNumber && isSecondNumber)
            {
                button.Enabled = true;
                secondButton.Enabled = true;
            }

            if (isNumber)
            {
                _messageDialogService.HideLabelAndTextBoxError(ErrorLabel, textBox);
            }
            else if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                _messageDialogService.DisplayLabelAndTextBoxError("Raudonam langelyje turi būti skaičius, negali būti tuščias",
                    textBox, ErrorLabel);
            }
            else
            {
                _messageDialogService.DisplayLabelAndTextBoxError("Raudonam langelyje tai ką įvedėte nėra skaičius, turi būti skaičius",
                    textBox, ErrorLabel);
            }

            if (isNumber && !isSecondNumber)
            {
                _messageDialogService.DisplayLabelAndTextBoxError("Raudonas langelis nėra skaičius, turi būti skaičius",
                    secondTextBox, ErrorLabel);
            }
            else if (isNumber && string.IsNullOrWhiteSpace(secondTextBox.Text))
            {
                _messageDialogService.DisplayLabelAndTextBoxError("Raudonas langelis negali būti tuščias",
                    secondTextBox, ErrorLabel);
            }
        }

        private void SetTextBoxBackColorToDefault(TextBox textBox)
        {
            textBox.BackColor = default;
        }

        private void TryStoreStorageInfoFromDataGridViewToTextBox()
        {

            if (ProductTypeOrStorageDataGridView.ColumnCount == StorageColumnCount && ProductTypeOrStorageDataGridView.Rows.Count != 0)
            {
                StorageSerialNumberTextBox.Text = ProductTypeOrStorageDataGridView.SelectedRows[0]
                    .Cells[StorageSerialNumberIndex].Value.ToString();

                StorageProductNameTextBox.Text = ProductTypeOrStorageDataGridView.SelectedRows[0]
                    .Cells[StorageProductNameIndex].Value.ToString();

                StorageProductMadeDateTextBox.Text = DateTime
                    .Parse(ProductTypeOrStorageDataGridView.SelectedRows[0].Cells[StorageMadeDateIndex].Value
                        .ToString()).ToString(DateFormat);

                StorageProductExpireDateTextBox.Text = DateTime.Parse(ProductTypeOrStorageDataGridView.SelectedRows[0]
                    .Cells[StorageExpireDateIndex].Value
                    .ToString()).ToString(DateFormat);

                StorageProductQuantityTextBox.Text = ProductTypeOrStorageDataGridView.SelectedRows[0]
                    .Cells[StorageQuantityIndex].Value.ToString();

                StorageProductPriceTextBox.Text = ProductTypeOrStorageDataGridView.SelectedRows[0]
                    .Cells[StoragePriceIndex].Value.ToString();
            }
            else if (ProductTypeOrStorageDataGridView.ColumnCount < StorageColumnCount && ProductTypeOrStorageDataGridView.Rows.Count != 0)
            {
                _messageDialogService.ShowErrorMassage("Produktų tipas nepriklauso sandėlio informacijai, informaciją kurią pasirinkote priklauso Sąskaitoms faktūroms");
            }
            else
            {
                _messageDialogService.ShowErrorMassage("Nėra jokios informacijos kurią būtų galima įkelti");
            }
        }

        private void ChangeButtonTextIfNumberIsPositiveOrNegative(bool isNumber)
        {
            if (isNumber)
            {
                double numberValue = double.Parse(AddStorageQuantityTextBox.Text);

                AddStorageQuantityButton.Text = numberValue >= 0 ? "Pridėti" : "Atimti";
            }
        }

        private void SetCursorAtTextBoxStringEnd()
        {
            StorageSerialNumberTextBox.SelectionStart = StorageSerialNumberTextBox.Text.Length;
            StorageProductNameTextBox.SelectionStart = StorageProductNameTextBox.Text.Length;
            StorageProductMadeDateTextBox.SelectionStart = StorageProductMadeDateTextBox.Text.Length;
            StorageProductExpireDateTextBox.SelectionStart = StorageProductExpireDateTextBox.Text.Length;
            StorageProductQuantityTextBox.SelectionStart = StorageProductQuantityTextBox.Text.Length;
            StorageProductPriceTextBox.SelectionStart = StorageProductPriceTextBox.Text.Length;
        }

        private void SetInformationOfDataGridViewTypeLabel(ProductTypeOrStorageListOperation productTypeOrStorageList)
        {
            InformationOfDataGridViewTypeLabel.Text = productTypeOrStorageList == ProductTypeOrStorageListOperation.ProductType ? "Produktai pagal Sąskaitas Faktūras" : "Sandėlio informacija";
        }

        #endregion

    }
}
