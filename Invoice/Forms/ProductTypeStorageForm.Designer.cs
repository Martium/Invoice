
namespace Invoice.Forms
{
    partial class ProductTypeStorageForm
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
            this.ProductTypeDataGridView = new System.Windows.Forms.DataGridView();
            this.GetProductTypeButton = new System.Windows.Forms.Button();
            this.SpecificProductTypeComboBox = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.ProductTypeDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // ProductTypeDataGridView
            // 
            this.ProductTypeDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ProductTypeDataGridView.Location = new System.Drawing.Point(40, 93);
            this.ProductTypeDataGridView.Name = "ProductTypeDataGridView";
            this.ProductTypeDataGridView.Size = new System.Drawing.Size(938, 563);
            this.ProductTypeDataGridView.TabIndex = 0;
            // 
            // GetProductTypeButton
            // 
            this.GetProductTypeButton.Location = new System.Drawing.Point(193, 13);
            this.GetProductTypeButton.Name = "GetProductTypeButton";
            this.GetProductTypeButton.Size = new System.Drawing.Size(85, 21);
            this.GetProductTypeButton.TabIndex = 1;
            this.GetProductTypeButton.Text = "Gauti išrašus";
            this.GetProductTypeButton.UseVisualStyleBackColor = true;
            this.GetProductTypeButton.Click += new System.EventHandler(this.GetProductTypeButton_Click);
            // 
            // SpecificProductTypeComboBox
            // 
            this.SpecificProductTypeComboBox.FormattingEnabled = true;
            this.SpecificProductTypeComboBox.Location = new System.Drawing.Point(40, 13);
            this.SpecificProductTypeComboBox.Name = "SpecificProductTypeComboBox";
            this.SpecificProductTypeComboBox.Size = new System.Drawing.Size(147, 21);
            this.SpecificProductTypeComboBox.TabIndex = 2;
            // 
            // ProductTypeStorageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 786);
            this.Controls.Add(this.SpecificProductTypeComboBox);
            this.Controls.Add(this.GetProductTypeButton);
            this.Controls.Add(this.ProductTypeDataGridView);
            this.MaximizeBox = false;
            this.Name = "ProductTypeStorageForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Produktų Sandėlys";
            this.Load += new System.EventHandler(this.ProductTypeStorageForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ProductTypeDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView ProductTypeDataGridView;
        private System.Windows.Forms.Button GetProductTypeButton;
        private System.Windows.Forms.ComboBox SpecificProductTypeComboBox;
    }
}