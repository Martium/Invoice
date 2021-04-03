using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Invoice.Enums;

namespace Invoice.Forms
{
    public partial class InvoiceForm : Form
    {
        public InvoiceForm(InvoiceOperations invoiceOperations, int? invoiceNumber = null, int? invoiceNumberYearCreation = null)
        {
            InitializeComponent();
        }
    }
}
