using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Invoice.Models;
using Invoice.Repositories;

namespace Invoice.Forms
{
    public partial class ListForm : Form
    {
        private readonly InvoiceRepository _invoiceRepository;

        private static readonly string SearchTextBoxPlaceholderText = "Įveskite paieškos frazę...";

        private bool _searchActive;

        public ListForm()
        {
            _invoiceRepository = new InvoiceRepository();

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

        private void ListOfInvoiceDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void CopyButton_Click(object sender, EventArgs e)
        {
            var createNewInvoice = new MoneyRecepitForm();

            createNewInvoice.Closed += ShowAndRefreshListForm;

            HideListAndOpenInvoiceForm(createNewInvoice);
        }

        private void HideListAndOpenInvoiceForm(Form invoiceForm)
        {
            this.Hide();

            invoiceForm.Show(this);
        }

        private void LoadFuneralServiceList(string searchPhrase = null)
        {
            if (_searchActive)
            {
                SearchButton.Enabled = true;
            }

            IEnumerable<InvoiceListModel> invoiceListModels = _invoiceRepository.GetInvoiceList(searchPhrase);

            ToggleExistingListManaging(enabled: invoiceListModels.Any(), searchPhrase);

            ListOfInvoiceDataGridView.DataSource = invoiceListModels;

           // ServiceHistoryDataGridView.DataSource = FuneralServiceBindingSource;
        }

        private void ToggleExistingListManaging(bool enabled, string searchPhrase)
        {
            if (string.IsNullOrWhiteSpace(searchPhrase))
            {
                SearchTextBox.Enabled = enabled;
                SearchButton.Enabled = false;
            }

            EditButton.Enabled = enabled;
            CopyButton.Enabled = enabled;
        }
    }
}
