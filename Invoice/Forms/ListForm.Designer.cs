
using System.Windows.Forms;

namespace Invoice.Forms
{
    partial class ListForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.EditButton = new System.Windows.Forms.Button();
            this.SearchTextBox = new System.Windows.Forms.TextBox();
            this.SearchButton = new System.Windows.Forms.Button();
            this.NewInvoiceButton = new System.Windows.Forms.Button();
            this.ListOfInvoiceDataGridView = new System.Windows.Forms.DataGridView();
            this.invoiceNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.invoiceNumberYearCreationDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.invoiceDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buyerNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.paymentStatusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.totalPriceWithPvmDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.invoiceListModelBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.CopyButton = new System.Windows.Forms.Button();
            this.SearchCancelButton = new System.Windows.Forms.Button();
            this.ChangePaymentButton = new System.Windows.Forms.Button();
            this.SellerInfoFormButton = new System.Windows.Forms.Button();
            this.InvoiceNumberYearCreationComboBox = new System.Windows.Forms.ComboBox();
            this.ProductTotalPriceTextBox = new System.Windows.Forms.TextBox();
            this.PvmPriceTextBox = new System.Windows.Forms.TextBox();
            this.TotalPriceWithPvmTextBox = new System.Windows.Forms.TextBox();
            this.ProductTotalPriceLabel = new System.Windows.Forms.Label();
            this.PvmPriceLabel = new System.Windows.Forms.Label();
            this.TotalPriceWithPvmLabel = new System.Windows.Forms.Label();
            this.GetSelectedYearButton = new System.Windows.Forms.Button();
            this.PaymentStatusComboBox = new System.Windows.Forms.ComboBox();
            this.OpenProductTypeStorageFormButton = new System.Windows.Forms.Button();
            this.BuyersInfoFormButton = new System.Windows.Forms.Button();
            this.ProductsInfoFormButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ListOfInvoiceDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.invoiceListModelBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // EditButton
            // 
            this.EditButton.BackColor = System.Drawing.SystemColors.Control;
            this.EditButton.Location = new System.Drawing.Point(205, 938);
            this.EditButton.Name = "EditButton";
            this.EditButton.Size = new System.Drawing.Size(180, 37);
            this.EditButton.TabIndex = 0;
            this.EditButton.TabStop = false;
            this.EditButton.Text = "Keisti esamą sąskaitą (Peržiūrėti)";
            this.EditButton.UseVisualStyleBackColor = false;
            this.EditButton.Click += new System.EventHandler(this.EditButton_Click);
            // 
            // SearchTextBox
            // 
            this.SearchTextBox.Location = new System.Drawing.Point(12, 38);
            this.SearchTextBox.Name = "SearchTextBox";
            this.SearchTextBox.Size = new System.Drawing.Size(158, 20);
            this.SearchTextBox.TabIndex = 1;
            this.SearchTextBox.TextChanged += new System.EventHandler(this.SearchTextBox_TextChanged);
            this.SearchTextBox.GotFocus += new System.EventHandler(this.SearchTextBox_GotFocus);
            this.SearchTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SearchTextBox_KeyDown);
            this.SearchTextBox.LostFocus += new System.EventHandler(this.SearchTextBox_LostFocus);
            // 
            // SearchButton
            // 
            this.SearchButton.Location = new System.Drawing.Point(176, 37);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(50, 20);
            this.SearchButton.TabIndex = 2;
            this.SearchButton.TabStop = false;
            this.SearchButton.Text = "ieškoti";
            this.SearchButton.UseVisualStyleBackColor = true;
            this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // NewInvoiceButton
            // 
            this.NewInvoiceButton.BackColor = System.Drawing.SystemColors.Control;
            this.NewInvoiceButton.Location = new System.Drawing.Point(959, 24);
            this.NewInvoiceButton.Name = "NewInvoiceButton";
            this.NewInvoiceButton.Size = new System.Drawing.Size(93, 45);
            this.NewInvoiceButton.TabIndex = 3;
            this.NewInvoiceButton.TabStop = false;
            this.NewInvoiceButton.Text = "Nauja sąskaita";
            this.NewInvoiceButton.UseVisualStyleBackColor = false;
            this.NewInvoiceButton.Click += new System.EventHandler(this.NewInvoiceButton_Click);
            // 
            // ListOfInvoiceDataGridView
            // 
            this.ListOfInvoiceDataGridView.AllowUserToAddRows = false;
            this.ListOfInvoiceDataGridView.AllowUserToDeleteRows = false;
            this.ListOfInvoiceDataGridView.AllowUserToResizeColumns = false;
            this.ListOfInvoiceDataGridView.AllowUserToResizeRows = false;
            this.ListOfInvoiceDataGridView.AutoGenerateColumns = false;
            this.ListOfInvoiceDataGridView.BackgroundColor = System.Drawing.SystemColors.ButtonShadow;
            this.ListOfInvoiceDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ListOfInvoiceDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.invoiceNumberDataGridViewTextBoxColumn,
            this.invoiceNumberYearCreationDataGridViewTextBoxColumn,
            this.invoiceDateDataGridViewTextBoxColumn,
            this.buyerNameDataGridViewTextBoxColumn,
            this.paymentStatusDataGridViewTextBoxColumn,
            this.totalPriceWithPvmDataGridViewTextBoxColumn});
            this.ListOfInvoiceDataGridView.DataSource = this.invoiceListModelBindingSource;
            this.ListOfInvoiceDataGridView.Location = new System.Drawing.Point(12, 82);
            this.ListOfInvoiceDataGridView.MultiSelect = false;
            this.ListOfInvoiceDataGridView.Name = "ListOfInvoiceDataGridView";
            this.ListOfInvoiceDataGridView.ReadOnly = true;
            this.ListOfInvoiceDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ListOfInvoiceDataGridView.Size = new System.Drawing.Size(1040, 825);
            this.ListOfInvoiceDataGridView.TabIndex = 4;
            this.ListOfInvoiceDataGridView.TabStop = false;
            this.ListOfInvoiceDataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ListOfInvoiceDataGridView_CellClick);
            this.ListOfInvoiceDataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ListOfInvoiceDataGridView_CellDoubleClick);
            this.ListOfInvoiceDataGridView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.ListOfInvoiceDataGridView_CellFormatting);
            this.ListOfInvoiceDataGridView.Paint += new System.Windows.Forms.PaintEventHandler(this.ListOfInvoiceDataGridView_Paint);
            this.ListOfInvoiceDataGridView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ListOfInvoiceDataGridView_KeyDown);
            // 
            // invoiceNumberDataGridViewTextBoxColumn
            // 
            this.invoiceNumberDataGridViewTextBoxColumn.DataPropertyName = "InvoiceNumber";
            this.invoiceNumberDataGridViewTextBoxColumn.HeaderText = "Sąskaitos numeris";
            this.invoiceNumberDataGridViewTextBoxColumn.Name = "invoiceNumberDataGridViewTextBoxColumn";
            this.invoiceNumberDataGridViewTextBoxColumn.ReadOnly = true;
            this.invoiceNumberDataGridViewTextBoxColumn.Width = 120;
            // 
            // invoiceNumberYearCreationDataGridViewTextBoxColumn
            // 
            this.invoiceNumberYearCreationDataGridViewTextBoxColumn.DataPropertyName = "InvoiceNumberYearCreation";
            this.invoiceNumberYearCreationDataGridViewTextBoxColumn.HeaderText = "Sąskaitos sukūrimo metai";
            this.invoiceNumberYearCreationDataGridViewTextBoxColumn.Name = "invoiceNumberYearCreationDataGridViewTextBoxColumn";
            this.invoiceNumberYearCreationDataGridViewTextBoxColumn.ReadOnly = true;
            this.invoiceNumberYearCreationDataGridViewTextBoxColumn.Width = 120;
            // 
            // invoiceDateDataGridViewTextBoxColumn
            // 
            this.invoiceDateDataGridViewTextBoxColumn.DataPropertyName = "InvoiceDate";
            this.invoiceDateDataGridViewTextBoxColumn.HeaderText = "Data";
            this.invoiceDateDataGridViewTextBoxColumn.Name = "invoiceDateDataGridViewTextBoxColumn";
            this.invoiceDateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // buyerNameDataGridViewTextBoxColumn
            // 
            this.buyerNameDataGridViewTextBoxColumn.DataPropertyName = "BuyerName";
            this.buyerNameDataGridViewTextBoxColumn.HeaderText = "Pirkėjas";
            this.buyerNameDataGridViewTextBoxColumn.Name = "buyerNameDataGridViewTextBoxColumn";
            this.buyerNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.buyerNameDataGridViewTextBoxColumn.Width = 320;
            // 
            // paymentStatusDataGridViewTextBoxColumn
            // 
            this.paymentStatusDataGridViewTextBoxColumn.DataPropertyName = "PaymentStatus";
            this.paymentStatusDataGridViewTextBoxColumn.HeaderText = "Sąskaitos Būsena";
            this.paymentStatusDataGridViewTextBoxColumn.Name = "paymentStatusDataGridViewTextBoxColumn";
            this.paymentStatusDataGridViewTextBoxColumn.ReadOnly = true;
            this.paymentStatusDataGridViewTextBoxColumn.Width = 180;
            // 
            // totalPriceWithPvmDataGridViewTextBoxColumn
            // 
            this.totalPriceWithPvmDataGridViewTextBoxColumn.DataPropertyName = "TotalPriceWithPvm";
            this.totalPriceWithPvmDataGridViewTextBoxColumn.HeaderText = "Pilna suma su PVM";
            this.totalPriceWithPvmDataGridViewTextBoxColumn.Name = "totalPriceWithPvmDataGridViewTextBoxColumn";
            this.totalPriceWithPvmDataGridViewTextBoxColumn.ReadOnly = true;
            this.totalPriceWithPvmDataGridViewTextBoxColumn.Width = 140;
            // 
            // invoiceListModelBindingSource
            // 
            this.invoiceListModelBindingSource.DataSource = typeof(Invoice.Models.InvoiceListModel);
            // 
            // CopyButton
            // 
            this.CopyButton.BackColor = System.Drawing.SystemColors.Control;
            this.CopyButton.Location = new System.Drawing.Point(12, 938);
            this.CopyButton.Name = "CopyButton";
            this.CopyButton.Size = new System.Drawing.Size(187, 37);
            this.CopyButton.TabIndex = 5;
            this.CopyButton.TabStop = false;
            this.CopyButton.Text = "Kopijuoti sąskaitą ( kurti naują )";
            this.CopyButton.UseVisualStyleBackColor = false;
            this.CopyButton.Click += new System.EventHandler(this.CopyButton_Click);
            // 
            // SearchCancelButton
            // 
            this.SearchCancelButton.Location = new System.Drawing.Point(232, 37);
            this.SearchCancelButton.Name = "SearchCancelButton";
            this.SearchCancelButton.Size = new System.Drawing.Size(66, 20);
            this.SearchCancelButton.TabIndex = 6;
            this.SearchCancelButton.TabStop = false;
            this.SearchCancelButton.Text = "Atšaukti";
            this.SearchCancelButton.UseVisualStyleBackColor = true;
            this.SearchCancelButton.Click += new System.EventHandler(this.SearchCancelButton_Click);
            // 
            // ChangePaymentButton
            // 
            this.ChangePaymentButton.BackColor = System.Drawing.SystemColors.Control;
            this.ChangePaymentButton.Location = new System.Drawing.Point(903, 938);
            this.ChangePaymentButton.Name = "ChangePaymentButton";
            this.ChangePaymentButton.Size = new System.Drawing.Size(149, 37);
            this.ChangePaymentButton.TabIndex = 7;
            this.ChangePaymentButton.TabStop = false;
            this.ChangePaymentButton.Text = "Keisti sąskaitos būseną";
            this.ChangePaymentButton.UseVisualStyleBackColor = false;
            this.ChangePaymentButton.Click += new System.EventHandler(this.ChangePaymentButton_Click);
            // 
            // SellerInfoFormButton
            // 
            this.SellerInfoFormButton.BackColor = System.Drawing.SystemColors.Control;
            this.SellerInfoFormButton.Location = new System.Drawing.Point(839, 24);
            this.SellerInfoFormButton.Name = "SellerInfoFormButton";
            this.SellerInfoFormButton.Size = new System.Drawing.Size(114, 45);
            this.SellerInfoFormButton.TabIndex = 8;
            this.SellerInfoFormButton.TabStop = false;
            this.SellerInfoFormButton.Text = "Pildyti Pardavėjo informaciją";
            this.SellerInfoFormButton.UseVisualStyleBackColor = false;
            this.SellerInfoFormButton.Click += new System.EventHandler(this.SellerInfoFormButton_Click);
            // 
            // InvoiceNumberYearCreationComboBox
            // 
            this.InvoiceNumberYearCreationComboBox.FormattingEnabled = true;
            this.InvoiceNumberYearCreationComboBox.Location = new System.Drawing.Point(304, 6);
            this.InvoiceNumberYearCreationComboBox.MaxDropDownItems = 100;
            this.InvoiceNumberYearCreationComboBox.Name = "InvoiceNumberYearCreationComboBox";
            this.InvoiceNumberYearCreationComboBox.Size = new System.Drawing.Size(81, 21);
            this.InvoiceNumberYearCreationComboBox.TabIndex = 9;
            this.InvoiceNumberYearCreationComboBox.TabStop = false;
            // 
            // ProductTotalPriceTextBox
            // 
            this.ProductTotalPriceTextBox.Location = new System.Drawing.Point(499, 46);
            this.ProductTotalPriceTextBox.Name = "ProductTotalPriceTextBox";
            this.ProductTotalPriceTextBox.ReadOnly = true;
            this.ProductTotalPriceTextBox.Size = new System.Drawing.Size(114, 20);
            this.ProductTotalPriceTextBox.TabIndex = 10;
            this.ProductTotalPriceTextBox.TabStop = false;
            // 
            // PvmPriceTextBox
            // 
            this.PvmPriceTextBox.Location = new System.Drawing.Point(619, 46);
            this.PvmPriceTextBox.Name = "PvmPriceTextBox";
            this.PvmPriceTextBox.ReadOnly = true;
            this.PvmPriceTextBox.Size = new System.Drawing.Size(98, 20);
            this.PvmPriceTextBox.TabIndex = 11;
            this.PvmPriceTextBox.TabStop = false;
            // 
            // TotalPriceWithPvmTextBox
            // 
            this.TotalPriceWithPvmTextBox.Location = new System.Drawing.Point(723, 46);
            this.TotalPriceWithPvmTextBox.Name = "TotalPriceWithPvmTextBox";
            this.TotalPriceWithPvmTextBox.ReadOnly = true;
            this.TotalPriceWithPvmTextBox.Size = new System.Drawing.Size(101, 20);
            this.TotalPriceWithPvmTextBox.TabIndex = 12;
            this.TotalPriceWithPvmTextBox.TabStop = false;
            // 
            // ProductTotalPriceLabel
            // 
            this.ProductTotalPriceLabel.AutoSize = true;
            this.ProductTotalPriceLabel.Location = new System.Drawing.Point(516, 17);
            this.ProductTotalPriceLabel.Name = "ProductTotalPriceLabel";
            this.ProductTotalPriceLabel.Size = new System.Drawing.Size(79, 26);
            this.ProductTotalPriceLabel.TabIndex = 13;
            this.ProductTotalPriceLabel.Text = "Sąskaitų suma \r\n   be PVM";
            // 
            // PvmPriceLabel
            // 
            this.PvmPriceLabel.AutoSize = true;
            this.PvmPriceLabel.Location = new System.Drawing.Point(625, 24);
            this.PvmPriceLabel.Name = "PvmPriceLabel";
            this.PvmPriceLabel.Size = new System.Drawing.Size(83, 13);
            this.PvmPriceLabel.TabIndex = 14;
            this.PvmPriceLabel.Text = " Sąskaitų  PVM ";
            // 
            // TotalPriceWithPvmLabel
            // 
            this.TotalPriceWithPvmLabel.AutoSize = true;
            this.TotalPriceWithPvmLabel.Location = new System.Drawing.Point(731, 17);
            this.TotalPriceWithPvmLabel.Name = "TotalPriceWithPvmLabel";
            this.TotalPriceWithPvmLabel.Size = new System.Drawing.Size(79, 26);
            this.TotalPriceWithPvmLabel.TabIndex = 15;
            this.TotalPriceWithPvmLabel.Text = "Sąskaitų suma \r\n      su PVM";
            // 
            // GetSelectedYearButton
            // 
            this.GetSelectedYearButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GetSelectedYearButton.Location = new System.Drawing.Point(304, 37);
            this.GetSelectedYearButton.Name = "GetSelectedYearButton";
            this.GetSelectedYearButton.Size = new System.Drawing.Size(189, 36);
            this.GetSelectedYearButton.TabIndex = 16;
            this.GetSelectedYearButton.TabStop = false;
            this.GetSelectedYearButton.Text = "Pateikti pasirinktų metų sąskaitas";
            this.GetSelectedYearButton.UseVisualStyleBackColor = true;
            this.GetSelectedYearButton.Click += new System.EventHandler(this.GetSelectedYearButton_Click);
            // 
            // PaymentStatusComboBox
            // 
            this.PaymentStatusComboBox.FormattingEnabled = true;
            this.PaymentStatusComboBox.Location = new System.Drawing.Point(391, 6);
            this.PaymentStatusComboBox.MaxDropDownItems = 100;
            this.PaymentStatusComboBox.Name = "PaymentStatusComboBox";
            this.PaymentStatusComboBox.Size = new System.Drawing.Size(102, 21);
            this.PaymentStatusComboBox.TabIndex = 17;
            this.PaymentStatusComboBox.TabStop = false;
            // 
            // OpenProductTypeStorageFormButton
            // 
            this.OpenProductTypeStorageFormButton.BackColor = System.Drawing.SystemColors.Control;
            this.OpenProductTypeStorageFormButton.Location = new System.Drawing.Point(391, 938);
            this.OpenProductTypeStorageFormButton.Name = "OpenProductTypeStorageFormButton";
            this.OpenProductTypeStorageFormButton.Size = new System.Drawing.Size(180, 37);
            this.OpenProductTypeStorageFormButton.TabIndex = 18;
            this.OpenProductTypeStorageFormButton.TabStop = false;
            this.OpenProductTypeStorageFormButton.Text = "Atidaryti Produktų Sandelį";
            this.OpenProductTypeStorageFormButton.UseVisualStyleBackColor = false;
            this.OpenProductTypeStorageFormButton.Click += new System.EventHandler(this.OpenProductTypeStorageFormButton_Click);
            // 
            // BuyersInfoFormButton
            // 
            this.BuyersInfoFormButton.BackColor = System.Drawing.SystemColors.Control;
            this.BuyersInfoFormButton.Location = new System.Drawing.Point(619, 938);
            this.BuyersInfoFormButton.Name = "BuyersInfoFormButton";
            this.BuyersInfoFormButton.Size = new System.Drawing.Size(114, 37);
            this.BuyersInfoFormButton.TabIndex = 19;
            this.BuyersInfoFormButton.TabStop = false;
            this.BuyersInfoFormButton.Text = "Pirkėjų informacija";
            this.BuyersInfoFormButton.UseVisualStyleBackColor = false;
            this.BuyersInfoFormButton.Click += new System.EventHandler(this.BuyersInfoFormButton_Click);
            // 
            // ProductsInfoFormButton
            // 
            this.ProductsInfoFormButton.BackColor = System.Drawing.SystemColors.Control;
            this.ProductsInfoFormButton.Location = new System.Drawing.Point(739, 938);
            this.ProductsInfoFormButton.Name = "ProductsInfoFormButton";
            this.ProductsInfoFormButton.Size = new System.Drawing.Size(114, 37);
            this.ProductsInfoFormButton.TabIndex = 20;
            this.ProductsInfoFormButton.TabStop = false;
            this.ProductsInfoFormButton.Text = "Produktų informaciją";
            this.ProductsInfoFormButton.UseVisualStyleBackColor = false;
            this.ProductsInfoFormButton.Click += new System.EventHandler(this.ProductsInfoFormButton_Click);
            // 
            // ListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1064, 1001);
            this.Controls.Add(this.ProductsInfoFormButton);
            this.Controls.Add(this.BuyersInfoFormButton);
            this.Controls.Add(this.OpenProductTypeStorageFormButton);
            this.Controls.Add(this.PaymentStatusComboBox);
            this.Controls.Add(this.GetSelectedYearButton);
            this.Controls.Add(this.TotalPriceWithPvmLabel);
            this.Controls.Add(this.PvmPriceLabel);
            this.Controls.Add(this.ProductTotalPriceLabel);
            this.Controls.Add(this.TotalPriceWithPvmTextBox);
            this.Controls.Add(this.PvmPriceTextBox);
            this.Controls.Add(this.ProductTotalPriceTextBox);
            this.Controls.Add(this.InvoiceNumberYearCreationComboBox);
            this.Controls.Add(this.SellerInfoFormButton);
            this.Controls.Add(this.ChangePaymentButton);
            this.Controls.Add(this.SearchCancelButton);
            this.Controls.Add(this.CopyButton);
            this.Controls.Add(this.ListOfInvoiceDataGridView);
            this.Controls.Add(this.NewInvoiceButton);
            this.Controls.Add(this.SearchButton);
            this.Controls.Add(this.SearchTextBox);
            this.Controls.Add(this.EditButton);
            this.MaximizeBox = false;
            this.Name = "ListForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sąskaitų sąrašas";
            this.Load += new System.EventHandler(this.ListForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ListOfInvoiceDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.invoiceListModelBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button EditButton;
        private System.Windows.Forms.TextBox SearchTextBox;
        private System.Windows.Forms.Button SearchButton;
        private System.Windows.Forms.Button NewInvoiceButton;
        private System.Windows.Forms.DataGridView ListOfInvoiceDataGridView;
        private System.Windows.Forms.Button CopyButton;
        private System.Windows.Forms.BindingSource invoiceListModelBindingSource;
        private Button SearchCancelButton;
        private Button ChangePaymentButton;
        private Button SellerInfoFormButton;
        private DataGridViewTextBoxColumn invoiceNumberDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn invoiceNumberYearCreationDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn invoiceDateDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn buyerNameDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn paymentStatusDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn totalPriceWithPvmDataGridViewTextBoxColumn;
        private ComboBox InvoiceNumberYearCreationComboBox;
        private TextBox ProductTotalPriceTextBox;
        private TextBox PvmPriceTextBox;
        private TextBox TotalPriceWithPvmTextBox;
        private Label ProductTotalPriceLabel;
        private Label PvmPriceLabel;
        private Label TotalPriceWithPvmLabel;
        private Button GetSelectedYearButton;
        private ComboBox PaymentStatusComboBox;
        private Button OpenProductTypeStorageFormButton;
        private Button BuyersInfoFormButton;
        private Button ProductsInfoFormButton;
    }
}

