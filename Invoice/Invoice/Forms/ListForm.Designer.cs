
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.CopyButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
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
            this.NewInvoiceButton.Location = new System.Drawing.Point(958, 12);
            this.NewInvoiceButton.Name = "NewInvoiceButton";
            this.NewInvoiceButton.Size = new System.Drawing.Size(93, 45);
            this.NewInvoiceButton.TabIndex = 3;
            this.NewInvoiceButton.Text = "Nauja sąskaita";
            this.NewInvoiceButton.UseVisualStyleBackColor = false;
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.ButtonShadow;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 78);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(1039, 825);
            this.dataGridView1.TabIndex = 4;
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
            // 
            // ListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1064, 1001);
            this.Controls.Add(this.CopyButton);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.NewInvoiceButton);
            this.Controls.Add(this.SearchButton);
            this.Controls.Add(this.SearchTextBox);
            this.Controls.Add(this.EditButton);
            this.MaximizeBox = false;
            this.Name = "ListForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Vitalijaus Pranskūno sąskaitos";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button EditButton;
        private System.Windows.Forms.TextBox SearchTextBox;
        private System.Windows.Forms.Button SearchButton;
        private System.Windows.Forms.Button NewInvoiceButton;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button CopyButton;
    }
}

