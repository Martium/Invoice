using System.Collections.Generic;
using System.Windows.Forms;
using Invoice.Enums;
using Invoice.Models.ProductType;
using Invoice.Repositories;

namespace Invoice.Forms
{
    public partial class ProductTypeStorageForm : Form
    {
        private readonly ProductTypeRepository _productTypeRepository;
        

        public ProductTypeStorageForm()
        {
            _productTypeRepository = new ProductTypeRepository();
            InitializeComponent();
            SetControlInitialState();
            SetProductTypeDataSource();
        }

        private void ProductTypeStorageForm_Load(object sender, System.EventArgs e)
        {
            FillSpecificProductTypeComboBox();
        }

        private void GetProductTypeButton_Click(object sender, System.EventArgs e)
        {
            ProductTypeOperations productTypeOperation = GetProductTypeOperations(SpecificProductTypeComboBox);


        }

        #region Helpers

        private void SetControlInitialState()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            SpecificProductTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
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

        private void SetProductTypeDataSource()
        {
            FirstSpecificProductTypeModel model = new FirstSpecificProductTypeModel();

            List <FirstSpecificProductTypeModel> info =
                _productTypeRepository.GetSpecificProductTypeFullInfoBySpecialName("1l",
                    ProductTypeOperations.FirstProductType);

            BindingSource bindingSource = new BindingSource {model};

            bindingSource.DataSource = info;

            ProductTypeDataGridView.DataSource = bindingSource;

            ProductTypeDataGridView.Columns[0].HeaderText = "Sąskaitos Nr.";
            ProductTypeDataGridView.Columns[1].HeaderText = "Metai";
            ProductTypeDataGridView.Columns[2].HeaderText = "Tipas";
            ProductTypeDataGridView.Columns[3].HeaderText = "Kiekis";
            ProductTypeDataGridView.Columns[4].HeaderText = "Vnt. Kaina";

            

            ProductTypeDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;


        }


        #endregion

    
    }
}
