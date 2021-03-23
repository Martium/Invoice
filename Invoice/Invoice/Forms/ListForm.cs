using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Invoice.Forms;

namespace Invoice
{
    public partial class ListForm : Form
    {
        public ListForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var createNewInvoice = new InvoiceForm();

            createNewInvoice.Closed += ShowAndRefreshListForm;

            HideListAndOpenInvoiceForm(createNewInvoice);
        }

        private void ShowAndRefreshListForm(object sender, EventArgs e)
        {
            this.Show();
        }

        private void HideListAndOpenInvoiceForm(Form invoiceForm)
        {
            this.Hide();

            invoiceForm.Show(this);
        }
    }
}
