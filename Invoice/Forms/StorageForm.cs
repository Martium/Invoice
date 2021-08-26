using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Invoice.Enums;
using Invoice.Models.ProductType;
using Invoice.Repositories;

namespace Invoice.Forms
{
    public partial class StorageForm : Form
    {
        private List<SpecificNameProductTypeModel> _specificProductTypeAllInfo;
        private readonly ProductTypeRepository _productTypeRepository;

        public StorageForm()
        {
            _specificProductTypeAllInfo = new List<SpecificNameProductTypeModel>();
            _productTypeRepository = new ProductTypeRepository();

            InitializeComponent();
            SetControlInitializeState();

            
        }

        private void StorageForm_Load(object sender, System.EventArgs e)
        {
            LoadSpecificInfoToStorageDataGridView(StorageInfo.ProductType);
            TryFillProductTypeYearComboBox();
            TryFillProductTypeSpecificNamesToComboBox();

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
                DisplayEmptyListReason("Sukurkite Bent Vieną sąskaitą ir supildykite bent vieną produkto tipą ", e, dataGridView);
            }

            
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
                    StorageDataGridView.Columns[0].Width = 80;
                    StorageDataGridView.Columns[1].Width = 80;
                    StorageDataGridView.Columns[2].Width = 240;
                    StorageDataGridView.Columns[3].Width = 80;
                    StorageDataGridView.Columns[4].Width = 80;
                    break;

                case StorageInfo.Deposit:
                    break;
            }
        }

        private void ChangeDataGridViewHeaderText(StorageInfo storageInfo)
        {
            switch (storageInfo)
            {
                case StorageInfo.ProductType:
                    StorageDataGridView.Columns[0].HeaderText = @"Sąskaitos Nr.";
                    StorageDataGridView.Columns[1].HeaderText = @"Metai";
                    StorageDataGridView.Columns[2].HeaderText = @"Tipas";
                    StorageDataGridView.Columns[3].HeaderText = @"Kiekis";
                    StorageDataGridView.Columns[4].HeaderText = @"Vnt. Kaina";
                    break;

                case StorageInfo.Deposit:
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

                    var bidingSourceModel = new SpecificNameProductTypeModel();
                    BindingSource bindingSource = new BindingSource { bidingSourceModel };

                    bindingSource.DataSource = _specificProductTypeAllInfo;

                    StorageDataGridView.DataSource = bindingSource;

                    ChangeDataGridViewHeadersSize(storageInfo);
                    ChangeDataGridViewHeaderText(storageInfo);
                    SetInformationOfDataGridViewTypeLabel(storageInfo);
                    break;

                case StorageInfo.Deposit:
                    break;
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


        #endregion


    }
}
