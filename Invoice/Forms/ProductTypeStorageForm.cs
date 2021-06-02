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

        private List<SpecificNameProductTypeModel> _specificProductTypeAllInfo;

        private const int ProductTypeQuantityIndex = 3;
        private const int ProductTypePriceIndex = 4;

        public ProductTypeStorageForm()
        {
            _specificProductTypeAllInfo = new List<SpecificNameProductTypeModel>();
            _productTypeRepository = new ProductTypeRepository();
            _numberService = new NumberService();

            InitializeComponent();
            SetControlInitialState();
        }

        private void ProductTypeStorageForm_Load(object sender, System.EventArgs e)
        {
            TryFillProductTypeYearComboBox();

            TryFillProductTypeSpecificNamesToComboBox();

            GetAllInfoBySpecialProductTypeName();

            LoadSpecificProductTypeToDataGridView();

            CalculateFullProductTypeQuantityAndPrice();
        }

        private void ProductTypeDataGridView_Paint(object sender, PaintEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;

            if (ProductTypeDataGridView.Rows.Count == 0)
            {
                DisplayEmptyListReason("Informacijos Nerasta pasirinkite tipą iš lentelės ir spauskite Gauti info", e, dataGridView);
            }
        }

        private void GetAllInfoByProductNameButton_Click(object sender, System.EventArgs e)
        {
            GetAllInfoBySpecialProductTypeName();

            LoadSpecificProductTypeToDataGridView();

            CalculateFullProductTypeQuantityAndPrice();
        }

        #region Helpers

        private void SetControlInitialState()
        {
            this.StartPosition = FormStartPosition.CenterScreen;

            ProductTypeDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            ProductTypeYearComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            ProductTypeSpecificNameComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void LoadSpecificProductTypeToDataGridView()
        {
            var bidingSourceModel = new SpecificNameProductTypeModel();

            BindingSource bindingSource = new BindingSource {bidingSourceModel};

            GetAllInfoBySpecialProductTypeName();

            bindingSource.DataSource = _specificProductTypeAllInfo;

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

        private void TryFillProductTypeYearComboBox()
        {
            ProductTypeYearComboBox.Items.Clear();

            IEnumerable<int> allYears = _productTypeRepository.GetAllExistingProductTypeYears();

            foreach (var year in allYears)
            {
                if (year != default)
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

             _specificProductTypeAllInfo = (from firstInfo in firstSpecificProductInfo where firstInfo.FirstProductTypeQuantity.HasValue && firstInfo.FirstProductTypePrice.HasValue select new SpecificNameProductTypeModel() {IdByInvoiceNumber = firstInfo.IdByInvoiceNumber, IdByInvoiceNumberYearCreation = firstInfo.IdByInvoiceNumberYearCreation, ProductTypeName = firstInfo.FirstProductType, Quantity = firstInfo.FirstProductTypeQuantity.Value, Price = firstInfo.FirstProductTypePrice.Value }).ToList();

            _specificProductTypeAllInfo.AddRange(from secondInfo in secondSpecificProductInfo where secondInfo.SecondProductTypeQuantity.HasValue && secondInfo.SecondProductTypePrice.HasValue select new SpecificNameProductTypeModel() {IdByInvoiceNumber = secondInfo.IdByInvoiceNumber, IdByInvoiceNumberYearCreation = secondInfo.IdByInvoiceNumberYearCreation, ProductTypeName = secondInfo.SecondProductType, Quantity = secondInfo.SecondProductTypeQuantity.Value, Price = secondInfo.SecondProductTypePrice.Value });

            _specificProductTypeAllInfo.AddRange(from thirdInfo in thirdSpecificProductInfo where thirdInfo.ThirdProductTypeQuantity.HasValue && thirdInfo.ThirdProductTypePrice.HasValue select new SpecificNameProductTypeModel() { IdByInvoiceNumber = thirdInfo.IdByInvoiceNumber, IdByInvoiceNumberYearCreation = thirdInfo.IdByInvoiceNumberYearCreation, ProductTypeName = thirdInfo.ThirdProductType, Quantity = thirdInfo.ThirdProductTypeQuantity.Value, Price = thirdInfo.ThirdProductTypePrice.Value });

            _specificProductTypeAllInfo.AddRange(from fourthInfo in fourthSpecificProductInfo where fourthInfo.FourthProductTypeQuantity.HasValue && fourthInfo.FourthProductTypePrice.HasValue select new SpecificNameProductTypeModel() {IdByInvoiceNumber = fourthInfo.IdByInvoiceNumber, IdByInvoiceNumberYearCreation = fourthInfo.IdByInvoiceNumberYearCreation, ProductTypeName = fourthInfo.FourthProductType, Quantity = fourthInfo.FourthProductTypeQuantity.Value, Price = fourthInfo.FourthProductTypePrice.Value });

            _specificProductTypeAllInfo.AddRange(from fifthInfo in fifthSpecificProductInfo where fifthInfo.FifthProductTypeQuantity.HasValue && fifthInfo.FifthProductTypePrice.HasValue select new SpecificNameProductTypeModel() {IdByInvoiceNumber = fifthInfo.IdByInvoiceNumber, IdByInvoiceNumberYearCreation = fifthInfo.IdByInvoiceNumberYearCreation, ProductTypeName = fifthInfo.FifthProductType, Quantity = fifthInfo.FifthProductTypeQuantity.Value, Price = fifthInfo.FifthProductTypePrice.Value });

            _specificProductTypeAllInfo.AddRange(from sixthInfo in sixthSpecificProductInfo where sixthInfo.SixthProductTypeQuantity.HasValue && sixthInfo.SixthProductTypePrice.HasValue select new SpecificNameProductTypeModel() {IdByInvoiceNumber = sixthInfo.IdByInvoiceNumber, IdByInvoiceNumberYearCreation = sixthInfo.IdByInvoiceNumberYearCreation, ProductTypeName = sixthInfo.SixthProductType, Quantity = sixthInfo.SixthProductTypeQuantity.Value, Price = sixthInfo.SixthProductTypePrice.Value });

            _specificProductTypeAllInfo.AddRange(from seventhInfo in seventhSpecificProductInfo where seventhInfo.SeventhProductTypeQuantity.HasValue && seventhInfo.SeventhProductTypePrice.HasValue select new SpecificNameProductTypeModel() {IdByInvoiceNumber = seventhInfo.IdByInvoiceNumber, IdByInvoiceNumberYearCreation = seventhInfo.IdByInvoiceNumberYearCreation, ProductTypeName = seventhInfo.SeventhProductType, Quantity = seventhInfo.SeventhProductTypeQuantity.Value, Price = seventhInfo.SeventhProductTypePrice.Value });

            _specificProductTypeAllInfo.AddRange(from eighthInfo in eighthSpecificProductInfo where eighthInfo.EighthProductTypeQuantity.HasValue && eighthInfo.EighthProductTypePrice.HasValue select new SpecificNameProductTypeModel() {IdByInvoiceNumber = eighthInfo.IdByInvoiceNumber, IdByInvoiceNumberYearCreation = eighthInfo.IdByInvoiceNumberYearCreation, ProductTypeName = eighthInfo.EighthProductType, Quantity = eighthInfo.EighthProductTypeQuantity.Value, Price = eighthInfo.EighthProductTypePrice.Value });

            _specificProductTypeAllInfo.AddRange(from ninthInfo in ninthSpecificProductInfo where ninthInfo.NinthProductTypeQuantity.HasValue && ninthInfo.NinthProductTypePrice.HasValue select new SpecificNameProductTypeModel() {IdByInvoiceNumber = ninthInfo.IdByInvoiceNumber, IdByInvoiceNumberYearCreation = ninthInfo.IdByInvoiceNumberYearCreation, ProductTypeName = ninthInfo.NinthProductType, Quantity = ninthInfo.NinthProductTypeQuantity.Value, Price = ninthInfo.NinthProductTypePrice.Value });

            _specificProductTypeAllInfo.AddRange(from tenInfo in tenSpecificProductInfo where tenInfo.TenProductTypeQuantity.HasValue && tenInfo.TenProductTypePrice.HasValue select new SpecificNameProductTypeModel() {IdByInvoiceNumber = tenInfo.IdByInvoiceNumber, IdByInvoiceNumberYearCreation = tenInfo.IdByInvoiceNumberYearCreation, ProductTypeName = tenInfo.TenProductType, Quantity = tenInfo.TenProductTypeQuantity.Value, Price = tenInfo.TenProductTypePrice.Value });

            _specificProductTypeAllInfo.AddRange(from eleventhInfo in eleventhSpecificProductInfo where eleventhInfo.EleventhProductTypeQuantity.HasValue && eleventhInfo.EleventhProductTypePrice.HasValue select new SpecificNameProductTypeModel() {IdByInvoiceNumber = eleventhInfo.IdByInvoiceNumber, IdByInvoiceNumberYearCreation = eleventhInfo.IdByInvoiceNumberYearCreation, ProductTypeName = eleventhInfo.EleventhProductType, Quantity = eleventhInfo.EleventhProductTypeQuantity.Value, Price = eleventhInfo.EleventhProductTypePrice.Value });

            _specificProductTypeAllInfo.AddRange(from twelfthInfo in twelfthSpecificProductInfo where twelfthInfo.TwelfthProductTypeQuantity.HasValue && twelfthInfo.TwelfthProductTypePrice.HasValue select new SpecificNameProductTypeModel() {IdByInvoiceNumber = twelfthInfo.IdByInvoiceNumber, IdByInvoiceNumberYearCreation = twelfthInfo.IdByInvoiceNumberYearCreation, ProductTypeName = twelfthInfo.TwelfthProductType, Quantity = twelfthInfo.TwelfthProductTypeQuantity.Value, Price = twelfthInfo.TwelfthProductTypePrice.Value });
           
        }

        #endregion

        
    }
}
