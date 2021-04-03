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

        private BindingSource _invoiceDataGridViewBindingSource = new BindingSource();

        public ListForm()
        {
            _invoiceRepository = new InvoiceRepository();

            InitializeComponent();
        }

        private void ListForm_Load(object sender, EventArgs e)
        {
            LoadInvoiceList();
        }

        private void SearchTextBox_TextChanged(object sender, EventArgs e)
        {
            SearchButton.Enabled = !string.IsNullOrWhiteSpace(SearchTextBox.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var createNewInvoice = new InvoiceForm();

            createNewInvoice.Closed += ShowAndRefreshListForm;

            HideListAndOpenInvoiceForm(createNewInvoice);
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            _searchActive = true;
            LoadInvoiceList(SearchTextBox.Text);
        }

        private void SearchTextBox_GotFocus(object sender, EventArgs e)
        {
            if (SearchTextBox.Text == SearchTextBoxPlaceholderText)
            {
                SearchTextBox.Text = string.Empty;
            }
        }

        private void SearchTextBox_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                SearchTextBox.Text = SearchTextBoxPlaceholderText;
                SearchButton.Enabled = false;
            }
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

        private void LoadInvoiceList(string searchPhrase = null)
        {
            if (_searchActive)
            {
                SearchButton.Enabled = true;
            }

            IEnumerable<InvoiceListModel> invoiceListModels = _invoiceRepository.GetInvoiceList(searchPhrase);

            ToggleExistingListManaging(enabled: invoiceListModels.Any(), searchPhrase);

            invoiceListModelBindingSource.DataSource = invoiceListModels;

            ListOfInvoiceDataGridView.DataSource = invoiceListModelBindingSource;
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
