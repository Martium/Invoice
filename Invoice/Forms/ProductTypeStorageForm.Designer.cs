﻿
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
            this.GetAllInfoByYearButton = new System.Windows.Forms.Button();
            this.GetAllInfoByProductNameButton = new System.Windows.Forms.Button();
            this.ProductTypeYearComboBox = new System.Windows.Forms.ComboBox();
            this.ProductTypeSpecificNameComboBox = new System.Windows.Forms.ComboBox();
            this.FullProductTypeQuantityLabel = new System.Windows.Forms.Label();
            this.FullProductTypePriceLabel = new System.Windows.Forms.Label();
            this.FullProductTypeQuantityTextBox = new System.Windows.Forms.TextBox();
            this.FullProductTypePriceTextBox = new System.Windows.Forms.TextBox();
            this.ProductTypeInfoLabel = new System.Windows.Forms.Label();
            this.ProductTypeYearInfoLabel = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ProductTypeDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // ProductTypeDataGridView
            // 
            this.ProductTypeDataGridView.AllowUserToAddRows = false;
            this.ProductTypeDataGridView.AllowUserToDeleteRows = false;
            this.ProductTypeDataGridView.AllowUserToResizeColumns = false;
            this.ProductTypeDataGridView.AllowUserToResizeRows = false;
            this.ProductTypeDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ProductTypeDataGridView.Location = new System.Drawing.Point(40, 88);
            this.ProductTypeDataGridView.MultiSelect = false;
            this.ProductTypeDataGridView.Name = "ProductTypeDataGridView";
            this.ProductTypeDataGridView.ReadOnly = true;
            this.ProductTypeDataGridView.Size = new System.Drawing.Size(557, 563);
            this.ProductTypeDataGridView.TabIndex = 0;
            this.ProductTypeDataGridView.Paint += new System.Windows.Forms.PaintEventHandler(this.ProductTypeDataGridView_Paint);
            // 
            // GetAllInfoByYearButton
            // 
            this.GetAllInfoByYearButton.Location = new System.Drawing.Point(857, 88);
            this.GetAllInfoByYearButton.Name = "GetAllInfoByYearButton";
            this.GetAllInfoByYearButton.Size = new System.Drawing.Size(94, 35);
            this.GetAllInfoByYearButton.TabIndex = 3;
            this.GetAllInfoByYearButton.Text = "  Gauti Info \r\npagal metus ";
            this.GetAllInfoByYearButton.UseVisualStyleBackColor = true;
            this.GetAllInfoByYearButton.Click += new System.EventHandler(this.GetAllInfoByYearButton_Click);
            // 
            // GetAllInfoByProductNameButton
            // 
            this.GetAllInfoByProductNameButton.Location = new System.Drawing.Point(756, 88);
            this.GetAllInfoByProductNameButton.Name = "GetAllInfoByProductNameButton";
            this.GetAllInfoByProductNameButton.Size = new System.Drawing.Size(95, 35);
            this.GetAllInfoByProductNameButton.TabIndex = 4;
            this.GetAllInfoByProductNameButton.Text = "Gauti Info";
            this.GetAllInfoByProductNameButton.UseVisualStyleBackColor = true;
            this.GetAllInfoByProductNameButton.Click += new System.EventHandler(this.GetAllInfoByProductNameButton_Click);
            // 
            // ProductTypeYearComboBox
            // 
            this.ProductTypeYearComboBox.FormattingEnabled = true;
            this.ProductTypeYearComboBox.Location = new System.Drawing.Point(603, 147);
            this.ProductTypeYearComboBox.Name = "ProductTypeYearComboBox";
            this.ProductTypeYearComboBox.Size = new System.Drawing.Size(79, 21);
            this.ProductTypeYearComboBox.TabIndex = 6;
            // 
            // ProductTypeSpecificNameComboBox
            // 
            this.ProductTypeSpecificNameComboBox.FormattingEnabled = true;
            this.ProductTypeSpecificNameComboBox.Location = new System.Drawing.Point(603, 88);
            this.ProductTypeSpecificNameComboBox.Name = "ProductTypeSpecificNameComboBox";
            this.ProductTypeSpecificNameComboBox.Size = new System.Drawing.Size(147, 21);
            this.ProductTypeSpecificNameComboBox.TabIndex = 7;
            // 
            // FullProductTypeQuantityLabel
            // 
            this.FullProductTypeQuantityLabel.AutoSize = true;
            this.FullProductTypeQuantityLabel.Location = new System.Drawing.Point(603, 244);
            this.FullProductTypeQuantityLabel.Name = "FullProductTypeQuantityLabel";
            this.FullProductTypeQuantityLabel.Size = new System.Drawing.Size(143, 13);
            this.FullProductTypeQuantityLabel.TabIndex = 8;
            this.FullProductTypeQuantityLabel.Text = "Visų prodkutų bendras kiekis";
            // 
            // FullProductTypePriceLabel
            // 
            this.FullProductTypePriceLabel.AutoSize = true;
            this.FullProductTypePriceLabel.Location = new System.Drawing.Point(603, 307);
            this.FullProductTypePriceLabel.Name = "FullProductTypePriceLabel";
            this.FullProductTypePriceLabel.Size = new System.Drawing.Size(137, 13);
            this.FullProductTypePriceLabel.TabIndex = 9;
            this.FullProductTypePriceLabel.Text = "Visų prodkutų bendra kaina\r\n";
            // 
            // FullProductTypeQuantityTextBox
            // 
            this.FullProductTypeQuantityTextBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.FullProductTypeQuantityTextBox.Location = new System.Drawing.Point(603, 273);
            this.FullProductTypeQuantityTextBox.Name = "FullProductTypeQuantityTextBox";
            this.FullProductTypeQuantityTextBox.ReadOnly = true;
            this.FullProductTypeQuantityTextBox.Size = new System.Drawing.Size(147, 20);
            this.FullProductTypeQuantityTextBox.TabIndex = 10;
            // 
            // FullProductTypePriceTextBox
            // 
            this.FullProductTypePriceTextBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.FullProductTypePriceTextBox.Location = new System.Drawing.Point(603, 334);
            this.FullProductTypePriceTextBox.Name = "FullProductTypePriceTextBox";
            this.FullProductTypePriceTextBox.ReadOnly = true;
            this.FullProductTypePriceTextBox.Size = new System.Drawing.Size(147, 20);
            this.FullProductTypePriceTextBox.TabIndex = 11;
            // 
            // ProductTypeInfoLabel
            // 
            this.ProductTypeInfoLabel.AutoSize = true;
            this.ProductTypeInfoLabel.Location = new System.Drawing.Point(603, 72);
            this.ProductTypeInfoLabel.Name = "ProductTypeInfoLabel";
            this.ProductTypeInfoLabel.Size = new System.Drawing.Size(75, 13);
            this.ProductTypeInfoLabel.TabIndex = 12;
            this.ProductTypeInfoLabel.Text = "Produkto tipas";
            // 
            // ProductTypeYearInfoLabel
            // 
            this.ProductTypeYearInfoLabel.AutoSize = true;
            this.ProductTypeYearInfoLabel.Location = new System.Drawing.Point(603, 131);
            this.ProductTypeYearInfoLabel.Name = "ProductTypeYearInfoLabel";
            this.ProductTypeYearInfoLabel.Size = new System.Drawing.Size(33, 13);
            this.ProductTypeYearInfoLabel.TabIndex = 13;
            this.ProductTypeYearInfoLabel.Text = "Metai";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(713, 486);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(105, 29);
            this.button1.TabIndex = 14;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ProductTypeStorageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 786);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ProductTypeYearInfoLabel);
            this.Controls.Add(this.ProductTypeInfoLabel);
            this.Controls.Add(this.FullProductTypePriceTextBox);
            this.Controls.Add(this.FullProductTypeQuantityTextBox);
            this.Controls.Add(this.FullProductTypePriceLabel);
            this.Controls.Add(this.FullProductTypeQuantityLabel);
            this.Controls.Add(this.ProductTypeSpecificNameComboBox);
            this.Controls.Add(this.ProductTypeYearComboBox);
            this.Controls.Add(this.GetAllInfoByProductNameButton);
            this.Controls.Add(this.GetAllInfoByYearButton);
            this.Controls.Add(this.ProductTypeDataGridView);
            this.MaximizeBox = false;
            this.Name = "ProductTypeStorageForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Produktų Sandėlys";
            this.Load += new System.EventHandler(this.ProductTypeStorageForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ProductTypeDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView ProductTypeDataGridView;
        private System.Windows.Forms.Button GetAllInfoByYearButton;
        private System.Windows.Forms.Button GetAllInfoByProductNameButton;
        private System.Windows.Forms.ComboBox ProductTypeYearComboBox;
        private System.Windows.Forms.ComboBox ProductTypeSpecificNameComboBox;
        private System.Windows.Forms.Label FullProductTypeQuantityLabel;
        private System.Windows.Forms.Label FullProductTypePriceLabel;
        private System.Windows.Forms.TextBox FullProductTypeQuantityTextBox;
        private System.Windows.Forms.TextBox FullProductTypePriceTextBox;
        private System.Windows.Forms.Label ProductTypeInfoLabel;
        private System.Windows.Forms.Label ProductTypeYearInfoLabel;
        private System.Windows.Forms.Button button1;
    }
}