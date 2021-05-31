using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Invoice.Enums;
using Invoice.Models.ProductType;
using Invoice.Repositories;
using Invoice.Service;

namespace Invoice.Forms
{
    public partial class ProductTypeStorageForm : Form
    {
        private readonly ProductTypeRepository _productTypeRepository;

        private readonly NumberService _numberService;

        private const int ProductTypeNameIndex = 2;
        private const int ProductTypeQuantityIndex = 3;
        private const int ProductTypePriceIndex = 4;

        public ProductTypeStorageForm()
        {
            _productTypeRepository = new ProductTypeRepository();
            _numberService = new NumberService();

            InitializeComponent();
            SetControlInitialState();
        }

        private void ProductTypeStorageForm_Load(object sender, System.EventArgs e)
        {
            FillSpecificProductTypeComboBox();

            LoadSpecificProductTypeToDataGridView(ProductTypeOperations.FirstProductType);

            CalculateFullProductTypeQuantityAndPrice();

            TryFillProductTypeSpecificNamesToComboBox();
        }

        private void GetProductTypeButton_Click(object sender, System.EventArgs e)
        {
            ProductTypeOperations productTypeOperation = GetProductTypeOperations(SpecificProductTypeComboBox);

            LoadSpecificProductTypeToDataGridView(productTypeOperation);

            CalculateFullProductTypeQuantityAndPrice();

            TryFillProductTypeSpecificNamesToComboBox();
        }

        private void ProductTypeDataGridView_Paint(object sender, PaintEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;

            if (ProductTypeDataGridView.Rows.Count == 0)
            {
                DisplayEmptyListReason("Informacijos Nerasta pasirinkite kitą lentelę ir spauskite Gauti išrašus", e, dataGridView);
            }
        }

        private void GetAllInfoByProductNameButton_Click(object sender, System.EventArgs e)
        {
            AddQuantityAndPriceAllInfoBySpecialProductTypeName();
        }

        #region Helpers

        private void SetControlInitialState()
        {
            this.StartPosition = FormStartPosition.CenterScreen;

            ProductTypeDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            SpecificProductTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            ProductTypeYearComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            ProductTypeSpecificNameComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void FillSpecificProductTypeComboBox()
        { 
            object[] productTypes =
            {
                "Pirmos lentelės Info", 
                "Antros lentelės Info", 
                "Trečios lentelės Info", 
                "Ketvirtos lentelės Info", 
                "Penktos lentelės Info",
                "Šeštos lentelės Info",
                "Septintos lentelės Info",
                "Aštuntos lentelės Info",
                "Devintos lentelės Info",
                "Dešimtos lentelės Info",
                "Vienuoliktos lentelės Info",
                "Dvyliktos lentelės Info"
            } ;

            SpecificProductTypeComboBox.Items.AddRange(productTypes);

            SpecificProductTypeComboBox.Text = productTypes[0].ToString();
        }

        private ProductTypeOperations GetProductTypeOperations(ComboBox comboBox)
        {
            ProductTypeOperations productTypeOperations;

            switch (comboBox.Text)
            {
                case "Pirmos lentelės Info":
                    productTypeOperations = ProductTypeOperations.FirstProductType;
                    break;
                case "Antros lentelės Info":
                    productTypeOperations = ProductTypeOperations.SecondProductType;
                    break;
                case "Trečios lentelės Info":
                    productTypeOperations = ProductTypeOperations.ThirdProductType;
                    break;
                case "Ketvirtos lentelės Info":
                    productTypeOperations = ProductTypeOperations.FourthProductType;
                    break;
                case "Penktos lentelės Info":
                    productTypeOperations = ProductTypeOperations.FifthProductType;
                    break;
                case "Šeštos lentelės Info":
                    productTypeOperations = ProductTypeOperations.SixthProductType;
                    break;
                case "Septintos lentelės Info":
                    productTypeOperations = ProductTypeOperations.SeventhProductType;
                    break;
                case "Aštuntos lentelės Info":
                    productTypeOperations = ProductTypeOperations.EighthProductType;
                    break;
                case "Devintos lentelės Info":
                    productTypeOperations = ProductTypeOperations.NinthProductType;
                    break;
                case "Dešimtos lentelės Info":
                    productTypeOperations = ProductTypeOperations.TenProductType;
                    break;
                case "Vienuoliktos lentelės Info":
                    productTypeOperations = ProductTypeOperations.EleventhProductType;
                    break;
                case "Dvyliktos lentelės Info":
                    productTypeOperations = ProductTypeOperations.TwelfthProductType;
                    break;

                default:
                    productTypeOperations = ProductTypeOperations.FirstProductType;
                    break;
            }

            return productTypeOperations;
        }

        private void LoadSpecificProductTypeToDataGridView(ProductTypeOperations productTypeOperations)
        {
            var specificModel = GetSpecificProductModel(productTypeOperations);

            BindingSource bindingSource = new BindingSource {specificModel};

            var specificProductTypeFullInfo = LoadSpecificProductType(productTypeOperations);

            bindingSource.DataSource = specificProductTypeFullInfo;

            ProductTypeDataGridView.DataSource = bindingSource;

            ChangeDataGridViewHeaderText();

        }

        private void ChangeDataGridViewHeaderText()
        {
            ProductTypeDataGridView.Columns[0].HeaderText = @"Sąskaitos Nr.";
            ProductTypeDataGridView.Columns[1].HeaderText = @"Metai";
            ProductTypeDataGridView.Columns[2].HeaderText = @"Tipas";
            ProductTypeDataGridView.Columns[3].HeaderText = @"Kiekis";
            ProductTypeDataGridView.Columns[4].HeaderText = @"Vnt. Kaina";
        }

        private dynamic GetSpecificProductModel(ProductTypeOperations productTypeOperations)
        {
            switch (productTypeOperations)
            {
                case ProductTypeOperations.FirstProductType:
                    FirstSpecificProductTypeModel firstModel = new FirstSpecificProductTypeModel();
                    return firstModel;
                case ProductTypeOperations.SecondProductType:
                    SecondSpecificProductTypeModel secondModel = new SecondSpecificProductTypeModel();
                    return secondModel;
                case ProductTypeOperations.ThirdProductType:
                    ThirdSpecificProductTypeModel thirdModel = new ThirdSpecificProductTypeModel();
                    return thirdModel;
                case ProductTypeOperations.FourthProductType:
                    FourthSpecificProductTypeModel fourthModel = new FourthSpecificProductTypeModel();
                    return fourthModel;
                case ProductTypeOperations.FifthProductType:
                    FifthSpecificProductTypeModel fifthModel = new FifthSpecificProductTypeModel();
                    return fifthModel;
                case ProductTypeOperations.SixthProductType:
                    SixthSpecificProductTypeModel sixthModel = new SixthSpecificProductTypeModel();
                    return sixthModel;
                case ProductTypeOperations.SeventhProductType:
                    SeventhSpecificProductTypeModel seventhModel = new SeventhSpecificProductTypeModel();
                    return seventhModel;
                case ProductTypeOperations.EighthProductType:
                    EighthSpecificProductTypeModel eighthModel = new EighthSpecificProductTypeModel();
                    return eighthModel;
                case ProductTypeOperations.NinthProductType:
                    NinthSpecificProductTypeModel ninthModel = new NinthSpecificProductTypeModel();
                    return ninthModel;
                case ProductTypeOperations.TenProductType:
                    TenSpecificProductTypeModel tenModel = new TenSpecificProductTypeModel();
                    return tenModel;
                case ProductTypeOperations.EleventhProductType:
                    EleventhSpecificProductTypeModel eleventhModel = new EleventhSpecificProductTypeModel();
                    return eleventhModel;
                case ProductTypeOperations.TwelfthProductType:
                    TwelfthSpecificProductTypeModel twelfthModel = new TwelfthSpecificProductTypeModel();
                    return twelfthModel;
                default:
                    FirstSpecificProductTypeModel defaultModel = new FirstSpecificProductTypeModel();
                    return defaultModel;
            }
        }

        private dynamic LoadSpecificProductType(ProductTypeOperations productTypeOperations)
        {
            switch (productTypeOperations)
            {
                case ProductTypeOperations.FirstProductType:
                    IEnumerable<FirstSpecificProductTypeModel> firstSpecificProductType =
                        _productTypeRepository.GetSpecificProductTypeFullInfo(productTypeOperations);
                    return firstSpecificProductType;
                case ProductTypeOperations.SecondProductType:
                    IEnumerable<SecondSpecificProductTypeModel> secondSpecificProductType =
                        _productTypeRepository.GetSpecificProductTypeFullInfo(productTypeOperations);
                    return secondSpecificProductType;
                case ProductTypeOperations.ThirdProductType:
                    IEnumerable<ThirdSpecificProductTypeModel> thirdSpecificProductType =
                        _productTypeRepository.GetSpecificProductTypeFullInfo(productTypeOperations);
                    return thirdSpecificProductType;
                case ProductTypeOperations.FourthProductType:
                    IEnumerable<FourthSpecificProductTypeModel> fourthSpecificProductType =
                        _productTypeRepository.GetSpecificProductTypeFullInfo(productTypeOperations);
                    return fourthSpecificProductType;
                case ProductTypeOperations.FifthProductType:
                    IEnumerable<FifthSpecificProductTypeModel> fifthSpecificProductType =
                        _productTypeRepository.GetSpecificProductTypeFullInfo(productTypeOperations);
                    return fifthSpecificProductType;
                case ProductTypeOperations.SixthProductType:
                    IEnumerable<SixthSpecificProductTypeModel> sixthSpecificProductType =
                        _productTypeRepository.GetSpecificProductTypeFullInfo(productTypeOperations);
                    return sixthSpecificProductType;
                case ProductTypeOperations.SeventhProductType:
                    IEnumerable<SeventhSpecificProductTypeModel> seventhSpecificProductType =
                        _productTypeRepository.GetSpecificProductTypeFullInfo(productTypeOperations);
                    return seventhSpecificProductType;
                case ProductTypeOperations.EighthProductType:
                    IEnumerable<EighthSpecificProductTypeModel> eighthSpecificProductType =
                        _productTypeRepository.GetSpecificProductTypeFullInfo(productTypeOperations);
                    return eighthSpecificProductType;
                case ProductTypeOperations.NinthProductType:
                    IEnumerable<NinthSpecificProductTypeModel> ninthSpecificProductType =
                        _productTypeRepository.GetSpecificProductTypeFullInfo(productTypeOperations);
                    return ninthSpecificProductType;
                case ProductTypeOperations.TenProductType:
                    IEnumerable<TenSpecificProductTypeModel> tenSpecificProductType =
                        _productTypeRepository.GetSpecificProductTypeFullInfo(productTypeOperations);
                    return tenSpecificProductType;
                case ProductTypeOperations.EleventhProductType:
                    IEnumerable<EleventhSpecificProductTypeModel> eleventhSpecificProductType =
                        _productTypeRepository.GetSpecificProductTypeFullInfo(productTypeOperations);
                    return eleventhSpecificProductType;
                case ProductTypeOperations.TwelfthProductType:
                    IEnumerable<TwelfthSpecificProductTypeModel> twelfthSpecificProductType =
                        _productTypeRepository.GetSpecificProductTypeFullInfo(productTypeOperations);
                    return twelfthSpecificProductType;

                default:
                    IEnumerable<FirstSpecificProductTypeModel> defaultSpecificProductType =
                        _productTypeRepository.GetSpecificProductTypeFullInfo(productTypeOperations);
                    return defaultSpecificProductType;
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

        private void CalculateFullProductTypeQuantityAndPrice()
        {
            int rowsCount = ProductTypeDataGridView.Rows.Count;

            double calculateFullProductTypeQuantity =
                _numberService.SumAllDataGridViewRowsSpecificColumns(ProductTypeDataGridView, rowsCount,
                    ProductTypeQuantityIndex);

            double calculateFullProductTypePrice =
                _numberService.SumAllDataGridViewRowsSpecificColumns(ProductTypeDataGridView, rowsCount,
                    ProductTypePriceIndex);

            FullProductTypeQuantityTextBox.Text = calculateFullProductTypeQuantity.ToString(CultureInfo.InvariantCulture);
            FullProductTypePriceTextBox.Text = calculateFullProductTypePrice.ToString(CultureInfo.InvariantCulture);
        }

        private void TryFillProductTypeSpecificNamesToComboBox()
        {
            ProductTypeSpecificNameComboBox.Items.Clear();

            IEnumerable<ProductTypeSpecificNamesModel> getInfo = _productTypeRepository.GetAllExistingProductTypeNames();

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

        private void AddQuantityAndPriceAllInfoBySpecialProductTypeName()
        {
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

            List<ProductTypeQuantityAndPriceModel> allInfoQuantityAndPrice = (from firstInfo in firstSpecificProductInfo where firstInfo.FirstProductTypeQuantity.HasValue && firstInfo.FirstProductTypePrice.HasValue select new ProductTypeQuantityAndPriceModel() { Quantity = firstInfo.FirstProductTypeQuantity.Value, Price = firstInfo.FirstProductTypePrice.Value }).ToList();

            allInfoQuantityAndPrice.AddRange(from secondInfo in secondSpecificProductInfo where secondInfo.SecondProductTypeQuantity.HasValue && secondInfo.SecondProductTypePrice.HasValue select new ProductTypeQuantityAndPriceModel() { Quantity = secondInfo.SecondProductTypeQuantity.Value, Price = secondInfo.SecondProductTypePrice.Value });

            allInfoQuantityAndPrice.AddRange(from thirdInfo in thirdSpecificProductInfo where thirdInfo.ThirdProductTypeQuantity.HasValue && thirdInfo.ThirdProductTypePrice.HasValue select new ProductTypeQuantityAndPriceModel() { Quantity = thirdInfo.ThirdProductTypeQuantity.Value, Price = thirdInfo.ThirdProductTypePrice.Value });

            allInfoQuantityAndPrice.AddRange(from fourthInfo in fourthSpecificProductInfo where fourthInfo.FourthProductTypeQuantity.HasValue && fourthInfo.FourthProductTypePrice.HasValue select new ProductTypeQuantityAndPriceModel() { Quantity = fourthInfo.FourthProductTypeQuantity.Value, Price = fourthInfo.FourthProductTypePrice.Value });

            allInfoQuantityAndPrice.AddRange(from fifthInfo in fifthSpecificProductInfo where fifthInfo.FifthProductTypeQuantity.HasValue && fifthInfo.FifthProductTypePrice.HasValue select new ProductTypeQuantityAndPriceModel() { Quantity = fifthInfo.FifthProductTypeQuantity.Value, Price = fifthInfo.FifthProductTypePrice.Value });

            allInfoQuantityAndPrice.AddRange(from sixthInfo in sixthSpecificProductInfo where sixthInfo.SixthProductTypeQuantity.HasValue && sixthInfo.SixthProductTypePrice.HasValue select new ProductTypeQuantityAndPriceModel() { Quantity = sixthInfo.SixthProductTypeQuantity.Value, Price = sixthInfo.SixthProductTypePrice.Value });

            allInfoQuantityAndPrice.AddRange(from seventhInfo in seventhSpecificProductInfo where seventhInfo.SeventhProductTypeQuantity.HasValue && seventhInfo.SeventhProductTypePrice.HasValue select new ProductTypeQuantityAndPriceModel() { Quantity = seventhInfo.SeventhProductTypeQuantity.Value, Price = seventhInfo.SeventhProductTypePrice.Value });

            allInfoQuantityAndPrice.AddRange(from eighthInfo in eighthSpecificProductInfo where eighthInfo.EighthProductTypeQuantity.HasValue && eighthInfo.EighthProductTypePrice.HasValue select new ProductTypeQuantityAndPriceModel() { Quantity = eighthInfo.EighthProductTypeQuantity.Value, Price = eighthInfo.EighthProductTypePrice.Value });

            allInfoQuantityAndPrice.AddRange(from ninthInfo in ninthSpecificProductInfo where ninthInfo.NinthProductTypeQuantity.HasValue && ninthInfo.NinthProductTypePrice.HasValue select new ProductTypeQuantityAndPriceModel() { Quantity = ninthInfo.NinthProductTypeQuantity.Value, Price = ninthInfo.NinthProductTypePrice.Value });

            allInfoQuantityAndPrice.AddRange(from tenInfo in tenSpecificProductInfo where tenInfo.TenProductTypeQuantity.HasValue && tenInfo.TenProductTypePrice.HasValue select new ProductTypeQuantityAndPriceModel() { Quantity = tenInfo.TenProductTypeQuantity.Value, Price = tenInfo.TenProductTypePrice.Value });

            allInfoQuantityAndPrice.AddRange(from eleventhInfo in eleventhSpecificProductInfo where eleventhInfo.EleventhProductTypeQuantity.HasValue && eleventhInfo.EleventhProductTypePrice.HasValue select new ProductTypeQuantityAndPriceModel() { Quantity = eleventhInfo.EleventhProductTypeQuantity.Value, Price = eleventhInfo.EleventhProductTypePrice.Value });

            allInfoQuantityAndPrice.AddRange(from twelfthInfo in twelfthSpecificProductInfo where twelfthInfo.TwelfthProductTypeQuantity.HasValue && twelfthInfo.TwelfthProductTypePrice.HasValue select new ProductTypeQuantityAndPriceModel() { Quantity = twelfthInfo.TwelfthProductTypeQuantity.Value, Price = twelfthInfo.TwelfthProductTypePrice.Value });


            double totalQuantitySum = allInfoQuantityAndPrice.Sum(q => q.Quantity);
            double totalPriceSum = allInfoQuantityAndPrice.Sum(p => p.Price);

            FullProductTypeQuantityTextBox.Text = totalQuantitySum.ToString(CultureInfo.InvariantCulture);
            FullProductTypePriceTextBox.Text = totalPriceSum.ToString(CultureInfo.InvariantCulture);
        }

        #endregion

        
    }
}
