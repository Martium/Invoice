using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Invoice.Enums;
using Invoice.Models.Deposit;
using Invoice.Models.ProductType;
using Invoice.Repositories;
using Invoice.Service;

namespace Invoice.Forms
{
    public partial class StorageForm : Form
    {
        private List<SpecificNameProductTypeModel> _specificProductTypeAllInfo;
        private List<FullDepositProductWithoutIdModel> _depositInfo;
        private readonly ProductTypeRepository _productTypeRepository;
        private readonly DepositRepository _depositRepository;
        private readonly NumberService _numberService;

        private const int ProductTypeInvoiceIdIndex = 0;
        private const int ProductTypeYearIndex = 1;
        private const int ProductTypeNameIndex = 2;
        private const int ProductTypeQuantityIndex = 3;
        private const int ProductTypePriceIndex = 4;

        private const int DepositInvoiceYearIndex = 0;
        private const int DepositProductNameIndex = 1;
        private const int DepositBarCodeIndex = 2;
        private const int DepositProductQuantityIndex = 3;

        private StorageInfo _storageInfo;

        public StorageForm()
        {
            _specificProductTypeAllInfo = new List<SpecificNameProductTypeModel>();
            _depositInfo = new List<FullDepositProductWithoutIdModel>();
            _productTypeRepository = new ProductTypeRepository();
            _depositRepository = new DepositRepository();
            _numberService = new NumberService();

            InitializeComponent();
            SetControlInitializeState();
        }

        private void StorageForm_Load(object sender, System.EventArgs e)
        {
            LoadSpecificInfoToStorageDataGridView(StorageInfo.ProductType);
            TryFillProductTypeAndDepositYearComboBox();
            TryFillProductTypeSpecificNamesToComboBox();
            TryFillDepositProductNameComboBox();
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

        private void StorageDataGridView_Paint(object sender, PaintEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;

            if (string.IsNullOrWhiteSpace(ProductTypeSpecificNameComboBox.Text) && dataGridView.Rows.Count == 0)
            {
                DisplayEmptyListReason("Sukurkite bent vieną sąskaitą ir supildykite, bent vieną produkto tipą ", e, dataGridView);
            }
        }

        private void GetAllInfoByProductNameButton_Click(object sender, System.EventArgs e)
        {
            _storageInfo = StorageInfo.ProductType;

            GetAllInfoBySpecialProductTypeName();
            LoadSpecificInfoToStorageDataGridView(_storageInfo);
            CalculateFullProductTypeQuantityAndPrice(_storageInfo);
        }

        private void GetAllInfoByYearButton_Click(object sender, System.EventArgs e)
        {
            _storageInfo = StorageInfo.ProductType;

            GetAllInfoBySpecialProductTypeNameAndYear();
            LoadSpecificInfoToStorageDataGridView(_storageInfo);
            CalculateFullProductTypeQuantityAndPrice(_storageInfo);
        }

        private void GetAllInfoByDepositProductsByYearButton_Click(object sender, System.EventArgs e)
        {
            _storageInfo = StorageInfo.Deposit;
            GetAllDepositInfoByYear();

            LoadSpecificInfoToStorageDataGridView(_storageInfo);
            CalculateFullProductTypeQuantityAndPrice(_storageInfo);

        }

        private void GetAllDepositInfoByYearAndNameButton_Click(object sender, System.EventArgs e)
        {
            _storageInfo = StorageInfo.Deposit;
            //

            LoadSpecificInfoToStorageDataGridView(_storageInfo);
            CalculateFullProductTypeQuantityAndPrice(_storageInfo);

        }

        #region MyMethods

        private void SetControlInitializeState()
        {
            StorageDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            ProductTypeSpecificNameComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            ProductTypeYearComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            DepositProductNameListComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            DepositYearComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void SetInformationOfDataGridViewTypeLabel(StorageInfo storageInfo)
        {
            switch (storageInfo)
            {
                case StorageInfo.ProductType:
                    InformationOfDataGridViewTypeLabel.Text = @"Produktai pagal Sąskaitas Faktūras";
                    break;

                case StorageInfo.Deposit:
                    InformationOfDataGridViewTypeLabel.Text = @"Depozito Informacija";
                    break;
            }

        }

        private void ChangeDataGridViewHeadersSize(StorageInfo storageInfo)
        {
            switch (storageInfo)
            {
                case StorageInfo.ProductType:
                    StorageDataGridView.Columns[ProductTypeInvoiceIdIndex].Width = 80;
                    StorageDataGridView.Columns[ProductTypeYearIndex].Width = 80;
                    StorageDataGridView.Columns[ProductTypeNameIndex].Width = 240;
                    StorageDataGridView.Columns[ProductTypeQuantityIndex].Width = 80;
                    StorageDataGridView.Columns[ProductTypePriceIndex].Width = 80;
                    break;

                case StorageInfo.Deposit:
                    StorageDataGridView.Columns[DepositInvoiceYearIndex].Width = 80;
                    StorageDataGridView.Columns[DepositProductNameIndex].Width = 240;
                    StorageDataGridView.Columns[DepositBarCodeIndex].Width = 160;
                    StorageDataGridView.Columns[DepositProductQuantityIndex].Width = 80;
                    break;
            }
        }

        private void ChangeDataGridViewHeaderText(StorageInfo storageInfo)
        {
            switch (storageInfo)
            {
                case StorageInfo.ProductType:
                    StorageDataGridView.Columns[ProductTypeInvoiceIdIndex].HeaderText = @"Sąskaitos Nr.";
                    StorageDataGridView.Columns[ProductTypeYearIndex].HeaderText = @"Metai";
                    StorageDataGridView.Columns[ProductTypeNameIndex].HeaderText = @"Tipas";
                    StorageDataGridView.Columns[ProductTypeQuantityIndex].HeaderText = @"Kiekis";
                    StorageDataGridView.Columns[ProductTypePriceIndex].HeaderText = @"Vnt. Kaina";
                    break;

                case StorageInfo.Deposit:
                    StorageDataGridView.Columns[DepositInvoiceYearIndex].HeaderText = @"Metai";
                    StorageDataGridView.Columns[DepositProductNameIndex].HeaderText = @"Tipas";
                    StorageDataGridView.Columns[DepositBarCodeIndex].HeaderText = @"Bar kodas";
                    StorageDataGridView.Columns[DepositProductQuantityIndex].HeaderText = @"Kiekis";
                    break;
            }
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

        private void LoadSpecificInfoToStorageDataGridView(StorageInfo storageInfo)
        {
          
            switch (storageInfo)
            {
                case StorageInfo.ProductType:

                    var bidingSourceProductModel = new SpecificNameProductTypeModel();
                    BindingSource productBidingSource = new BindingSource { bidingSourceProductModel };

                    productBidingSource.DataSource = _specificProductTypeAllInfo;
                    StorageDataGridView.DataSource = productBidingSource;

                    ChangeDataGridViewHeadersSize(storageInfo);
                    ChangeDataGridViewHeaderText(storageInfo);
                    SetInformationOfDataGridViewTypeLabel(storageInfo);
                    break;

                case StorageInfo.Deposit:

                    var bidingSourceDepositModel = new FullDepositProductWithoutIdModel();
                    BindingSource depositBindingSource = new BindingSource() {bidingSourceDepositModel};

                    depositBindingSource.DataSource = _depositInfo;
                    StorageDataGridView.DataSource = depositBindingSource;

                    ChangeDataGridViewHeadersSize(storageInfo);
                    ChangeDataGridViewHeaderText(storageInfo);
                    SetInformationOfDataGridViewTypeLabel(storageInfo);
                   
                    break;
            }
        }

        private void TryFillProductTypeAndDepositYearComboBox()
        {
            ProductTypeYearComboBox.Items.Clear();
            DepositYearComboBox.Items.Clear();

            IEnumerable<int> allYears = _productTypeRepository.GetAllExistingProductTypeYears();

            foreach (var year in allYears)
            {
                ProductTypeYearComboBox.Items.Add(year);
                DepositYearComboBox.Items.Add(year);
            }

            if (ProductTypeYearComboBox.Items.Count != 0)
            {
                ProductTypeYearComboBox.Text = ProductTypeYearComboBox.Items[0].ToString();
                DepositYearComboBox.Text = ProductTypeYearComboBox.Items[0].ToString();
            }
        }

        private void TryFillProductTypeSpecificNamesToComboBox()
        {
            ProductTypeSpecificNameComboBox.Items.Clear();

            IEnumerable<ProductTypeSpecificNamesModel>
                getInfo = _productTypeRepository.GetAllExistingProductTypeNames();

            foreach (var info in getInfo)
            {
                if (info.FirstProductType != string.Empty && !ProductTypeSpecificNameComboBox.Items.Contains(info.FirstProductType))
                    ProductTypeSpecificNameComboBox.Items.Add(info.FirstProductType);

                if (info.SecondProductType != string.Empty && !ProductTypeSpecificNameComboBox.Items.Contains(info.SecondProductType))
                    ProductTypeSpecificNameComboBox.Items.Add(info.SecondProductType);

                if (info.ThirdProductType != string.Empty && !ProductTypeSpecificNameComboBox.Items.Contains(info.ThirdProductType))
                    ProductTypeSpecificNameComboBox.Items.Add(info.ThirdProductType);

                if (info.FourthProductType != string.Empty && !ProductTypeSpecificNameComboBox.Items.Contains(info.FourthProductType))
                    ProductTypeSpecificNameComboBox.Items.Add(info.FourthProductType);

                if (info.FifthProductType != string.Empty && !ProductTypeSpecificNameComboBox.Items.Contains(info.FifthProductType))
                    ProductTypeSpecificNameComboBox.Items.Add(info.FifthProductType);

                if (info.SixthProductType != string.Empty && !ProductTypeSpecificNameComboBox.Items.Contains(info.SixthProductType))
                    ProductTypeSpecificNameComboBox.Items.Add(info.SixthProductType);

                if (info.SeventhProductType != string.Empty && !ProductTypeSpecificNameComboBox.Items.Contains(info.SeventhProductType))
                    ProductTypeSpecificNameComboBox.Items.Add(info.SeventhProductType);

                if (info.EighthProductType != string.Empty && !ProductTypeSpecificNameComboBox.Items.Contains(info.EighthProductType))
                    ProductTypeSpecificNameComboBox.Items.Add(info.EighthProductType);

                if (info.NinthProductType != string.Empty && !ProductTypeSpecificNameComboBox.Items.Contains(info.NinthProductType))
                    ProductTypeSpecificNameComboBox.Items.Add(info.NinthProductType);

                if (info.TenProductType != string.Empty && !ProductTypeSpecificNameComboBox.Items.Contains(info.TenProductType))
                    ProductTypeSpecificNameComboBox.Items.Add(info.TenProductType);

                if (info.EleventhProductType != string.Empty && !ProductTypeSpecificNameComboBox.Items.Contains(info.EleventhProductType))
                    ProductTypeSpecificNameComboBox.Items.Add(info.EleventhProductType);

                if (info.TwelfthProductType != string.Empty && !ProductTypeSpecificNameComboBox.Items.Contains(info.TwelfthProductType))
                    ProductTypeSpecificNameComboBox.Items.Add(info.TwelfthProductType);

            }

            if (ProductTypeSpecificNameComboBox.Items.Count != 0)
            {
                ProductTypeSpecificNameComboBox.Text = ProductTypeSpecificNameComboBox.Items[0].ToString();
            }

        }

        private void TryFillDepositProductNameComboBox()
        {
            DepositProductNameListComboBox.Items.Clear();

            IEnumerable<string> getAllProductNames = _depositRepository.GetAllProductsNames();

            foreach (var product in getAllProductNames)
            {
                DepositProductNameListComboBox.Items.Add(product);
            }

            if (DepositProductNameListComboBox.Items.Count != 0)
            {
                DepositProductNameListComboBox.Text = DepositProductNameListComboBox.Items[0].ToString();
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
                                               ProductTypeName = firstInfo.FirstProductType,
                                               Quantity = firstInfo.FirstProductTypeQuantity.Value,
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
                                                     ProductTypeName = thirdInfo.ThirdProductType,
                                                     Quantity = thirdInfo.ThirdProductTypeQuantity.Value,
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
                                                     ProductTypeName = fifthInfo.FifthProductType,
                                                     Quantity = fifthInfo.FifthProductTypeQuantity.Value,
                                                     Price = fifthInfo.FifthProductTypePrice.Value
                                                 });

            _specificProductTypeAllInfo.AddRange(from sixthInfo in sixthSpecificProductInfo
                                                 where sixthInfo.SixthProductTypeQuantity.HasValue && sixthInfo.SixthProductTypePrice.HasValue
                                                 select new SpecificNameProductTypeModel()
                                                 {
                                                     IdByInvoiceNumber = sixthInfo.IdByInvoiceNumber,
                                                     IdByInvoiceNumberYearCreation = sixthInfo.IdByInvoiceNumberYearCreation,
                                                     ProductTypeName = sixthInfo.SixthProductType,
                                                     Quantity = sixthInfo.SixthProductTypeQuantity.Value,
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
                                                     ProductTypeName = ninthInfo.NinthProductType,
                                                     Quantity = ninthInfo.NinthProductTypeQuantity.Value,
                                                     Price = ninthInfo.NinthProductTypePrice.Value
                                                 });

            _specificProductTypeAllInfo.AddRange(from tenInfo in tenSpecificProductInfo
                                                 where tenInfo.TenProductTypeQuantity.HasValue && tenInfo.TenProductTypePrice.HasValue
                                                 select new SpecificNameProductTypeModel()
                                                 {
                                                     IdByInvoiceNumber = tenInfo.IdByInvoiceNumber,
                                                     IdByInvoiceNumberYearCreation = tenInfo.IdByInvoiceNumberYearCreation,
                                                     ProductTypeName = tenInfo.TenProductType,
                                                     Quantity = tenInfo.TenProductTypeQuantity.Value,
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

        private void CalculateFullProductTypeQuantityAndPrice(StorageInfo storageInfo)
        {
            int rowsCount = StorageDataGridView.Rows.Count;

            switch (storageInfo)
            {
                case StorageInfo.ProductType:
                    double calculateFullProductTypeQuantity =
                        _numberService.SumAllDataGridViewRowsSpecificColumns(StorageDataGridView, rowsCount,
                            ProductTypeQuantityIndex);

                    double calculateFullProductTypePrice = _numberService.MultiplyAndSumAllDataGridViewRowsTwoSpecificColumns(
                        StorageDataGridView, rowsCount, ProductTypePriceIndex, ProductTypeQuantityIndex);

                    FullProductTypeQuantityTextBox.Text =
                        calculateFullProductTypeQuantity.ToString(CultureInfo.InvariantCulture);
                    FullProductTypePriceTextBox.Text = calculateFullProductTypePrice.ToString(CultureInfo.InvariantCulture);
                    break;

                case StorageInfo.Deposit:

                    double calculateFullDepositQuantity =
                        _numberService.SumAllDataGridViewRowsSpecificColumns(StorageDataGridView, rowsCount,
                            DepositProductQuantityIndex);
                    FullProductTypeQuantityTextBox.Text =
                        calculateFullDepositQuantity.ToString(CultureInfo.InvariantCulture);
                    FullProductTypePriceTextBox.Text = "N/A";
                    break;
            }

            

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
                                               ProductTypeName = firstInfo.FirstProductType,
                                               Quantity = firstInfo.FirstProductTypeQuantity.Value,
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
                                                     ProductTypeName = thirdInfo.ThirdProductType,
                                                     Quantity = thirdInfo.ThirdProductTypeQuantity.Value,
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
                                                     ProductTypeName = fifthInfo.FifthProductType,
                                                     Quantity = fifthInfo.FifthProductTypeQuantity.Value,
                                                     Price = fifthInfo.FifthProductTypePrice.Value
                                                 });

            _specificProductTypeAllInfo.AddRange(from sixthInfo in sixthSpecificProductInfo
                                                 where sixthInfo.SixthProductTypeQuantity.HasValue && sixthInfo.SixthProductTypePrice.HasValue
                                                 select new SpecificNameProductTypeModel()
                                                 {
                                                     IdByInvoiceNumber = sixthInfo.IdByInvoiceNumber,
                                                     IdByInvoiceNumberYearCreation = sixthInfo.IdByInvoiceNumberYearCreation,
                                                     ProductTypeName = sixthInfo.SixthProductType,
                                                     Quantity = sixthInfo.SixthProductTypeQuantity.Value,
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
                                                     ProductTypeName = ninthInfo.NinthProductType,
                                                     Quantity = ninthInfo.NinthProductTypeQuantity.Value,
                                                     Price = ninthInfo.NinthProductTypePrice.Value
                                                 });

            _specificProductTypeAllInfo.AddRange(from tenInfo in tenSpecificProductInfo
                                                 where tenInfo.TenProductTypeQuantity.HasValue && tenInfo.TenProductTypePrice.HasValue
                                                 select new SpecificNameProductTypeModel()
                                                 {
                                                     IdByInvoiceNumber = tenInfo.IdByInvoiceNumber,
                                                     IdByInvoiceNumberYearCreation = tenInfo.IdByInvoiceNumberYearCreation,
                                                     ProductTypeName = tenInfo.TenProductType,
                                                     Quantity = tenInfo.TenProductTypeQuantity.Value,
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

        private void GetAllDepositInfoByYear()
        {
            _depositInfo.Clear();

            int year = int.Parse(DepositYearComboBox.Text);

            _depositInfo = _depositRepository.GetAllDepositProductsByYear(year).ToList();
        }



        #endregion

        
    }
}
