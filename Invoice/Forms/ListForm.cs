using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Invoice.Enums;
using Invoice.Models;
using Invoice.Repositories;
using Invoice.Service;

namespace Invoice.Forms
{
    public partial class ListForm : Form
    {
        private readonly InvoiceRepository _invoiceRepository;

        private readonly MessageDialogService _messageDialogService = new MessageDialogService();

        private static readonly string SearchTextBoxPlaceholderText = "Įveskite paieškos frazę...";

        private const int InvoiceIsPaidIndex = 4;

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

        private void NewInvoiceButton_Click(object sender, EventArgs e)
        {
            var createNewInvoice = new InvoiceForm(InvoiceOperations.Create);

            createNewInvoice.Closed += ShowAndRefreshListForm;

            HideListAndOpenInvoiceForm(createNewInvoice);
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            int invoiceNumber = GetSelectedInvoiceNumber();
            int invoiceNumberYearCreation = GetSelectedOrderCreationYear();

            var editSelectedInvoice = new InvoiceForm(InvoiceOperations.Edit, invoiceNumber, invoiceNumberYearCreation);

            editSelectedInvoice.Closed += ShowAndRefreshListForm;

            HideListAndOpenInvoiceForm(editSelectedInvoice);
        }

        private void CopyButton_Click(object sender, EventArgs e)
        {
            int invoiceNumber = GetSelectedInvoiceNumber();
            int invoiceNumberYearCreation = GetSelectedOrderCreationYear();

            var copySelectedInvoice = new InvoiceForm(InvoiceOperations.Copy, invoiceNumber, invoiceNumberYearCreation);

            copySelectedInvoice.Closed += ShowAndRefreshListForm;

            HideListAndOpenInvoiceForm(copySelectedInvoice);
        }

        private void ChangePaymentButton_Click(object sender, EventArgs e)
        {
            ChangePaymentStatus();
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

        private void ShowAndRefreshListForm(object sender, EventArgs e)
        {
            _searchActive = false;

            SetControlsInitialState();

            LoadInvoiceList();

            this.Show();
        }

        private void ListOfInvoiceDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            foreach (DataGridViewRow row in ListOfInvoiceDataGridView.Rows)
            {
                row.DefaultCellStyle.BackColor = row.Cells[InvoiceIsPaidIndex].Value.ToString() == "Atsiskaityta" ? Color.Chartreuse : Color.Red;
            }
        }

        private void ListOfInvoiceDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string paymentStatus = ListOfInvoiceDataGridView.SelectedRows[0].Cells[InvoiceIsPaidIndex].Value.ToString();

            this.BackColor = paymentStatus == "Atsiskaityta" ? Color.Chartreuse : Color.Red;
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

            ChangePaymentButton.Enabled = ListOfInvoiceDataGridView.Rows.Count != 0;

            ChangeFormBackRoundColorByPaymentStatus();
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

        private int GetSelectedInvoiceNumber()
        {
            int invoiceNumberColumnIndex = 0;

            return (int)ListOfInvoiceDataGridView.SelectedRows[0].Cells[invoiceNumberColumnIndex].Value;
        }

        private int GetSelectedOrderCreationYear()
        {
            int invoiceNumberYearCreationColumnIndex = 1;

            return (int)ListOfInvoiceDataGridView.SelectedRows[0].Cells[invoiceNumberYearCreationColumnIndex].Value;
        }

        private void ChangePaymentStatus()
        {
            int invoiceNumber = GetSelectedInvoiceNumber();
            int invoiceNumberYearCreation = GetSelectedOrderCreationYear();
            string paymentStatus = ListOfInvoiceDataGridView.SelectedRows[0].Cells[InvoiceIsPaidIndex].Value.ToString();

            DialogResult dialogResult = _messageDialogService.ShowChoiceMessage("Ar tikrai norite pakeisti statusą");

            if (dialogResult == DialogResult.OK)
            {
                if (paymentStatus == "Atsiskaityta")
                {
                    ListOfInvoiceDataGridView.SelectedRows[0].Cells[InvoiceIsPaidIndex].Value = "Nesumokėta";
                    this.BackColor = Color.Red;
                    _invoiceRepository.UpdateExistingInvoicePaymentStatus(invoiceNumber, invoiceNumberYearCreation,
                        "Nesumokėta");
                }
                else
                {
                    ListOfInvoiceDataGridView.SelectedRows[0].Cells[InvoiceIsPaidIndex].Value = "Atsiskaityta";
                    this.BackColor = Color.Chartreuse;
                    _invoiceRepository.UpdateExistingInvoicePaymentStatus(invoiceNumber, invoiceNumberYearCreation,
                        "Atsiskaityta");
                }
            }
        }

        private void ChangeFormBackRoundColorByPaymentStatus()
        {
            try
            {
                string paymentStatus = ListOfInvoiceDataGridView.SelectedRows[0].Cells[InvoiceIsPaidIndex].Value.ToString();

                this.BackColor = paymentStatus == "Atsiskaityta" ? Color.Chartreuse : Color.Red;
            }
            catch
            {
                this.BackColor = Color.Wheat;
            }
        }
    }
}
