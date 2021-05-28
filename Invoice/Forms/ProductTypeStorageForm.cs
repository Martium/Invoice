using System.Windows.Forms;
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

        


        #endregion

    
    }
}
