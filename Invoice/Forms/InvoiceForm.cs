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
using Invoice.Repositories;

namespace Invoice.Forms
{
    public partial class InvoiceForm : Form
    {
        private readonly InvoiceRepository _invoiceRepository;

        private readonly InvoiceOperations _invoiceOperations;
        private readonly int? _invoiceNumber;
        private readonly int? _invoiceNumberYearCreation;

        public InvoiceForm(InvoiceOperations invoiceOperations, int? invoiceNumber = null, int? invoiceNumberYearCreation = null)
        {
            _invoiceRepository = new InvoiceRepository();

            _invoiceOperations = invoiceOperations;
            _invoiceNumber = invoiceNumber;
            _invoiceNumberYearCreation = invoiceNumberYearCreation;

            ResolveFormOperationDesign();

            InitializeComponent();
        }

        private void ResolveFormOperationDesign()
        {
            if (_invoiceOperations == InvoiceOperations.Create)
            {
                this.Text = "Naujos Sąskaita faktūra";
                //this.Icon = Properties.Resources.CreateIcon;
            }
            else if (_invoiceOperations == InvoiceOperations.Edit)
            {
                this.Text = "Esamos Sąskaitos faktūros keitimas";
                //this.Icon = Properties.Resources.EditIcon;
            }
            else if (_invoiceOperations == InvoiceOperations.Copy)
            {
                this.Text = "Esamos Sąskaitos faktūros kopijavimas (sukurti naują)";
                //this.Icon = Properties.Resources.CopyIcon;
            }
            else
            {
                throw new Exception($"Paslaugų valdymo formoje gauta nežinoma opercija: '{_invoiceOperations}'");
            }
        }

        private void SetTextBoxLengths()
        {

        }
    }
}
