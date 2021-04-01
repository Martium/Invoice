
namespace Invoice
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
            this.EditButton = new System.Windows.Forms.Button();
            this.SearchTextBox = new System.Windows.Forms.TextBox();
            this.SearchButton = new System.Windows.Forms.Button();
            this.NewInvoiceButton = new System.Windows.Forms.Button();
            this.ListOfInvoiceDataGridView = new System.Windows.Forms.DataGridView();
            this.CopyButton = new System.Windows.Forms.Button();
            this.InvoiceNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OrderDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NameOfCustumer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.ListOfInvoiceDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // EditButton
            // 
            this.EditButton.BackColor = System.Drawing.SystemColors.Control;
            this.EditButton.Location = new System.Drawing.Point(205, 938);
            this.EditButton.Name = "EditButton";
            this.EditButton.Size = new System.Drawing.Size(149, 37);
            this.EditButton.TabIndex = 0;
            this.EditButton.Text = "Keisti esamą sąskaitą";
            this.EditButton.UseVisualStyleBackColor = false;
            this.EditButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // SearchTextBox
            // 
            this.SearchTextBox.Location = new System.Drawing.Point(12, 37);
            this.SearchTextBox.Name = "SearchTextBox";
            this.SearchTextBox.Size = new System.Drawing.Size(158, 20);
            this.SearchTextBox.TabIndex = 1;
            // 
            // SearchButton
            // 
            this.SearchButton.Location = new System.Drawing.Point(176, 37);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(75, 20);
            this.SearchButton.TabIndex = 2;
            this.SearchButton.Text = "ieškoti";
            this.SearchButton.UseVisualStyleBackColor = true;
            // 
            // NewInvoiceButton
            // 
            this.NewInvoiceButton.BackColor = System.Drawing.SystemColors.Control;
            this.NewInvoiceButton.Location = new System.Drawing.Point(959, 24);
            this.NewInvoiceButton.Name = "NewInvoiceButton";
            this.NewInvoiceButton.Size = new System.Drawing.Size(93, 45);
            this.NewInvoiceButton.TabIndex = 3;
            this.NewInvoiceButton.Text = "Nauja sąskaita";
            this.NewInvoiceButton.UseVisualStyleBackColor = false;
            // 
            // ListOfInvoiceDataGridView
            // 
            this.ListOfInvoiceDataGridView.BackgroundColor = System.Drawing.SystemColors.ButtonShadow;
            this.ListOfInvoiceDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ListOfInvoiceDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.InvoiceNumber,
            this.OrderDate,
            this.NameOfCustumer});
            this.ListOfInvoiceDataGridView.Location = new System.Drawing.Point(12, 82);
            this.ListOfInvoiceDataGridView.Name = "ListOfInvoiceDataGridView";
            this.ListOfInvoiceDataGridView.Size = new System.Drawing.Size(1040, 825);
            this.ListOfInvoiceDataGridView.TabIndex = 4;
            this.ListOfInvoiceDataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ListOfInvoiceDataGridView_CellContentClick);
            // 
            // CopyButton
            // 
            this.CopyButton.BackColor = System.Drawing.SystemColors.Control;
            this.CopyButton.Location = new System.Drawing.Point(12, 938);
            this.CopyButton.Name = "CopyButton";
            this.CopyButton.Size = new System.Drawing.Size(187, 37);
            this.CopyButton.TabIndex = 5;
            this.CopyButton.Text = "Kopijuoti sąskaitą ( kurti naują )";
            this.CopyButton.UseVisualStyleBackColor = false;
            this.CopyButton.Click += new System.EventHandler(this.CopyButton_Click);
            // 
            // InvoiceNumber
            // 
            this.InvoiceNumber.HeaderText = "Sąskaitos numeris";
            this.InvoiceNumber.Name = "InvoiceNumber";
            this.InvoiceNumber.ReadOnly = true;
            // 
            // OrderDate
            // 
            this.OrderDate.HeaderText = "Data";
            this.OrderDate.Name = "OrderDate";
            this.OrderDate.ReadOnly = true;
            // 
            // NameOfCustumer
            // 
            this.NameOfCustumer.HeaderText = "Užsakovas";
            this.NameOfCustumer.Name = "NameOfCustumer";
            this.NameOfCustumer.ReadOnly = true;
            this.NameOfCustumer.Width = 780;
            // 
            // ListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1064, 1001);
            this.Controls.Add(this.CopyButton);
            this.Controls.Add(this.ListOfInvoiceDataGridView);
            this.Controls.Add(this.NewInvoiceButton);
            this.Controls.Add(this.SearchButton);
            this.Controls.Add(this.SearchTextBox);
            this.Controls.Add(this.EditButton);
            this.MaximizeBox = false;
            this.Name = "ListForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Vitalijaus Pranskūno sąskaitos";
            ((System.ComponentModel.ISupportInitialize)(this.ListOfInvoiceDataGridView)).EndInit();
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
        private System.Windows.Forms.DataGridViewTextBoxColumn InvoiceNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn OrderDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameOfCustumer;
    }
}

