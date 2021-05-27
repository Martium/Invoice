using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Invoice.Models;
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
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string productType = "FirstProductType";
            List<FirstSpecificProductTypeModel> testingInfo =
                _productTypeRepository.GetSpecificProductTypeBySpecialName(productType, "1l");

            foreach (var info in testingInfo)
            {
                comboBox1.Items.Add(info.IdByInvoiceNumber);
                comboBox2.Items.Add(info.IdByInvoiceNumberYearCreation);
                comboBox3.Items.Add(info.FirstProductType);
                comboBox4.Items.Add(info.FirstProductTypePrice.Value);
                comboBox5.Items.Add(info.FirstProductTypeQuantity.Value);
            }

        }
    }
}
