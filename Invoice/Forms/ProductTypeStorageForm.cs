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
        }

        private void ProductTypeStorageForm_Load(object sender, System.EventArgs e)
        {
            FillSpecificProductTypeComboBox();
        }

        private void GetProductTypeButton_Click(object sender, System.EventArgs e)
        {
            ProductTypeOperations productTypeOperation = GetProductTypeOperations(SpecificProductTypeComboBox);

            LoadSpecificProductTypeToDataGridView(productTypeOperation);

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

        private void LoadSpecificProductTypeToDataGridView(ProductTypeOperations productTypeOperations)
        {

            var specificModel = GetSpecificProductModel(productTypeOperations);

            BindingSource bindingSource = new BindingSource {specificModel};

            var specificProductTypeFullInfo = LoadSpecificProductType(productTypeOperations);

            bindingSource.DataSource = specificProductTypeFullInfo;

            ProductTypeDataGridView.DataSource = bindingSource;

            ProductTypeDataGridView.Columns[0].HeaderText = "Sąskaitos Nr.";
            ProductTypeDataGridView.Columns[1].HeaderText = "Metai";
            ProductTypeDataGridView.Columns[2].HeaderText = "Tipas";
            ProductTypeDataGridView.Columns[3].HeaderText = "Kiekis";
            ProductTypeDataGridView.Columns[4].HeaderText = "Vnt. Kaina";

            

            ProductTypeDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

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

        #endregion

    
    }
}
