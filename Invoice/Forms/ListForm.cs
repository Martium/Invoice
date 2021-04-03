using System;
using System.Collections.Generic;
using System.Drawing;
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

            SetControlsInitialState();
        }

        private void ListForm_Load(object sender, EventArgs e)
        {
            LoadInvoiceList();
        }

        private void SearchTextBox_TextChanged(object sender, EventArgs e)
        {
            SearchButton.Enabled = !string.IsNullOrWhiteSpace(SearchTextBox.Text);
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            var createNewInvoice = new InvoiceForm();

            createNewInvoice.Closed += ShowAndRefreshListForm;

            HideListAndOpenInvoiceForm(createNewInvoice);
        }

        private void SearchCancelButton_Click(object sender, EventArgs e)
        {
            _searchActive = false;

            SetControlsInitialState();

            LoadInvoiceList();
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

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchButton_Click(this, new EventArgs());
            }
        }

        private void ShowAndRefreshListForm(object sender, EventArgs e)
        {
            this.Show();
        }

        private void CopyButton_Click(object sender, EventArgs e)
        {
            var createNewInvoice = new MoneyRecepitForm();

            createNewInvoice.Closed += ShowAndRefreshListForm;

            HideListAndOpenInvoiceForm(createNewInvoice);
        }

        private void ListOfInvoiceDataGridView_Paint(object sender, PaintEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;

            if (dataGridView.Rows.Count == 0)
            {
                string emptyListReason = _searchActive
                    ? $"Paieškos frazė '{SearchTextBox.Text}' neatitiko jokių rezultatų. Ieškokite kitos frazės arba atšaukite paiešką."
                    : "Paslaugų istorija tuščia. Galite pradėti kurti naujas paslaugas pasinaudojęs mygtuku 'Įvesti naują paslaugą' dešiniame viršutiniame kampe.";

                DisplayEmptyListReason(emptyListReason, e, dataGridView);
            }
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
                SearchCancelButton.Enabled = true;
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

        private void SetControlsInitialState()
        {
            this.StartPosition = FormStartPosition.CenterScreen;

            ActiveControl = NewInvoiceButton;

            SearchTextBox.Text = SearchTextBoxPlaceholderText;
            SearchButton.Enabled = false;
            SearchCancelButton.Enabled = false;
        }

        private static void DisplayEmptyListReason(string reason, PaintEventArgs e, DataGridView dataGridView)
        {
            using (Graphics graphics = e.Graphics)
            {
                int leftPadding = 2;
                int topPadding = 41;
                int rowSelectionColumnWidth = 40;
                int messageBackgroundWidth = dataGridView.Columns.GetColumnsWidth(DataGridViewElementStates.Displayed) + rowSelectionColumnWidth;
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
    }
}
