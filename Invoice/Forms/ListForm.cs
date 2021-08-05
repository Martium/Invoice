using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
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

        private readonly NumberService _numberService = new NumberService();

        private static readonly string SearchTextBoxPlaceholderText = "Įveskite paieškos frazę...";

        private const int InvoiceDateIndex = 2;
        private const int InvoiceIsPaidIndex = 4;
        private const int TotalPriceWithPvmIndex = 5;

        private bool _searchActive;
        private bool _isGetAllSelectedYearInfoEmpty;

        public ListForm()
        {
            _invoiceRepository = new InvoiceRepository();

            InitializeComponent();

            ListOfInvoiceDataGridView.Columns[InvoiceDateIndex].DefaultCellStyle.Format = "yyyy/MM/dd";

            SetControlsInitialState();
        }

        private void ListForm_Load(object sender, EventArgs e)
        {
            FillInvoiceNumberYearCreationComboBox();

            FillPaymentStatusComboBox();

            LoadInvoiceList();
        }

        private void SearchTextBox_TextChanged(object sender, EventArgs e)
        {
            SearchButton.Enabled = !string.IsNullOrWhiteSpace(SearchTextBox.Text);
        }

        private void SearchCancelButton_Click(object sender, EventArgs e)
        {
            _searchActive = false;
            _isGetAllSelectedYearInfoEmpty = false;

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

            HideListAndOpenAnotherForm(createNewInvoice);
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            int invoiceNumber = GetSelectedInvoiceNumber();
            int invoiceNumberYearCreation = GetSelectedOrderCreationYear();

            var editSelectedInvoice = new InvoiceForm(InvoiceOperations.Edit, invoiceNumber, invoiceNumberYearCreation);

            editSelectedInvoice.Closed += ShowAndRefreshListForm;

            HideListAndOpenAnotherForm(editSelectedInvoice);
        }

        private void CopyButton_Click(object sender, EventArgs e)
        {
            int invoiceNumber = GetSelectedInvoiceNumber();
            int invoiceNumberYearCreation = GetSelectedOrderCreationYear();

            var copySelectedInvoice = new InvoiceForm(InvoiceOperations.Copy, invoiceNumber, invoiceNumberYearCreation);

            copySelectedInvoice.Closed += ShowAndRefreshListForm;

            HideListAndOpenAnotherForm(copySelectedInvoice);
        }

        private void ChangePaymentButton_Click(object sender, EventArgs e)
        {
            ChangePaymentStatus();
        }

        private void ListOfInvoiceDataGridView_Paint(object sender, PaintEventArgs e)
        {
            DataGridView dataGridView = (DataGridView) sender;

            if (dataGridView.Rows.Count == 0)
            {
                string emptyListReason;

                if (_searchActive && !_isGetAllSelectedYearInfoEmpty)
                {
                    emptyListReason = $"Paieškos frazė '{SearchTextBox.Text}' neatitiko jokių rezultatų. Ieškokite kitos frazės arba atšaukite paiešką.";
                }
                else if (_isGetAllSelectedYearInfoEmpty)
                {
                    emptyListReason = "Rezultatų nėra pagal pasirinktus metus ir apmokėjimo tipą";
                }
                else
                {
                    emptyListReason = "Paslaugų istorija tuščia. Galite pradėti kurti naujas paslaugas pasinaudojęs mygtuku 'Įvesti naują paslaugą' dešiniame viršutiniame kampe.";
                }

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
                row.DefaultCellStyle.BackColor = row.Cells[InvoiceIsPaidIndex].Value.ToString() == "Atsiskaityta"
                    ? Color.Chartreuse
                    : Color.Red;
            }
        }

        private void ListOfInvoiceDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                EditButton_Click(this, new EventArgs());
            }
        }

        private void ListOfInvoiceDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (ListOfInvoiceDataGridView.Rows.Count != 0)
            {
                string paymentStatus = ListOfInvoiceDataGridView.SelectedRows[0].Cells[InvoiceIsPaidIndex].Value.ToString();
                this.BackColor = paymentStatus == "Atsiskaityta" ? Color.Chartreuse : Color.Red;
            }
        }

        private void ListOfInvoiceDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (ListOfInvoiceDataGridView.Rows.Count != 0)
            {
                EditButton_Click(this, new EventArgs());
            }
        }

        private void SellerInfoFormButton_Click(object sender, EventArgs e)
        {
            var sellerInfoForm = new SellerInfoForm();

            sellerInfoForm.Closed += ShowAndRefreshListForm;

            HideListAndOpenAnotherForm(sellerInfoForm);
        }

        private void GetSelectedYearButton_Click(object sender, EventArgs e)
        {
            string invoiceNumberYearCreation = InvoiceNumberYearCreationComboBox.Text;
            string paymentStatus = PaymentStatusComboBox.Text;

            IEnumerable<InvoiceListModel> invoiceListModels =
                _invoiceRepository.GetAllSelectedYearInfo(invoiceNumberYearCreation, paymentStatus);

            invoiceListModelBindingSource.DataSource = invoiceListModels;

            ListOfInvoiceDataGridView.DataSource = invoiceListModelBindingSource;

            ChangePaymentButton.Enabled = ListOfInvoiceDataGridView.Rows.Count != 0;

            SearchCancelButton.Enabled = true;

            if (ListOfInvoiceDataGridView.Rows.Count == 0)
            {
                EditButton.Enabled = false;
                CopyButton.Enabled = false;
                _isGetAllSelectedYearInfoEmpty = true;
            }
            else
            {
                EditButton.Enabled = true;
                CopyButton.Enabled = true;
                _isGetAllSelectedYearInfoEmpty = false;
            }

            LoadAllTotalPriceSums();
        }

        private void OpenProductTypeStorageFormButton_Click(object sender, EventArgs e)
        {
            var productTypeStorageForm = new ProductTypeStorageForm();

            productTypeStorageForm.Closed += ShowAndRefreshListForm;

            HideListAndOpenAnotherForm(productTypeStorageForm);
        }

        private void BuyersInfoFormButton_Click(object sender, EventArgs e)
        {
            var buyersInfoForm = new BuyersInfoForm();

            buyersInfoForm.Closed += ShowAndRefreshListForm;

            HideListAndOpenAnotherForm(buyersInfoForm);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Escape))
            {
                this.Close();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        #region Helpers

        private void HideListAndOpenAnotherForm(Form form)
        {
            this.Hide();

            form.Show(this);
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

            TrySelectFirstRowInDataGridView();

            ChangeFormBackRoundColorByPaymentStatus();

            LoadAllTotalPriceSums();
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

            InvoiceNumberYearCreationComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            PaymentStatusComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
           
        }

        private static void DisplayEmptyListReason(string reason, PaintEventArgs e, DataGridView dataGridView)
        {
            using (Graphics graphics = e.Graphics)
            {
                int leftPadding = 2;
                int topPadding = 41;
                int rowSelectionColumnWidth = 40;
                int messageBackgroundWidth = dataGridView.Columns.GetColumnsWidth(DataGridViewElementStates.Displayed) +
                                             rowSelectionColumnWidth;
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

            return (int) ListOfInvoiceDataGridView.SelectedRows[0].Cells[invoiceNumberColumnIndex].Value;
        }

        private int GetSelectedOrderCreationYear()
        {
            int invoiceNumberYearCreationColumnIndex = 1;

            return (int) ListOfInvoiceDataGridView.SelectedRows[0].Cells[invoiceNumberYearCreationColumnIndex].Value;
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
                var paymentStatus = ListOfInvoiceDataGridView.SelectedRows[0].Cells[InvoiceIsPaidIndex].Value
                    .ToString();

                this.BackColor = paymentStatus == "Atsiskaityta" ? Color.Chartreuse : Color.Red;
            }
            catch
            {
                this.BackColor = Color.Wheat;
            }
        }

        private void TrySelectFirstRowInDataGridView()
        {
            if (ListOfInvoiceDataGridView.Rows.Count > 0)
            {
                ListOfInvoiceDataGridView.Select();
            }
        }

        private void FillInvoiceNumberYearCreationComboBox()
        {
            InvoiceNumberYearCreationComboBox.Items.Clear();

            var allYears = _invoiceRepository.GetInvoiceNumberYearCreationList().ToList();

            InvoiceNumberYearCreationComboBox.DataSource = allYears;
            InvoiceNumberYearCreationComboBox.DisplayMember = "InvoiceNumberYearCreation";
        }

        private void FillPaymentStatusComboBox()
        {
            string[] paymentStatus = {"Atsiskaityta", "Nesumokėta"};

            PaymentStatusComboBox.DataSource = paymentStatus;
            PaymentStatusComboBox.DisplayMember = "Atsiskaityta";
        }

        private void LoadAllTotalPriceSums()
        {
            int rowsCount = ListOfInvoiceDataGridView.Rows.Count;

            double totalPriceWithPvm =
                _numberService.SumAllDataGridViewRowsSpecificColumns(ListOfInvoiceDataGridView, rowsCount,
                    TotalPriceWithPvmIndex);

            TotalPriceWithPvmTextBox.Text = totalPriceWithPvm.ToString(CultureInfo.InvariantCulture);

            PvmPriceTextBox.Text = _numberService.CalculatePvmFromTotalPriceWithPvm(totalPriceWithPvm);

            ProductTotalPriceTextBox.Text = _numberService.CalculateFullPriceFromTotalPriceWithPvm(totalPriceWithPvm);
        }

        #endregion

        
    }
}
