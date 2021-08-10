
namespace Invoice.Forms
{
    partial class ProductInfoForm
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
            this.ProductNameRichTextBox = new System.Windows.Forms.RichTextBox();
            this.ProductBarCodeRichTextBox = new System.Windows.Forms.RichTextBox();
            this.ProductSeesRichTextBox = new System.Windows.Forms.RichTextBox();
            this.ProductPriceRichTextBox = new System.Windows.Forms.RichTextBox();
            this.ProductTypeTextBox = new System.Windows.Forms.TextBox();
            this.ProductTypePriceTextBox = new System.Windows.Forms.TextBox();
            this.ProductNameInfoLabel = new System.Windows.Forms.Label();
            this.ProductBarCodeLabel = new System.Windows.Forms.Label();
            this.ProductSeesInfoLabel = new System.Windows.Forms.Label();
            this.ProductPriceInfoLabel = new System.Windows.Forms.Label();
            this.ProductSellingInfoLabel = new System.Windows.Forms.Label();
            this.ProductExpenditureInfoLabel = new System.Windows.Forms.Label();
            this.ProductTypeInfoLabel = new System.Windows.Forms.Label();
            this.ProductTypePriceInfoLabel = new System.Windows.Forms.Label();
            this.NewProductButton = new System.Windows.Forms.Button();
            this.UpdateProductButton = new System.Windows.Forms.Button();
            this.ExistsProductListComboBox = new System.Windows.Forms.ComboBox();
            this.ProductListInfoLabel = new System.Windows.Forms.Label();
            this.ChooseProductButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ProductNameRichTextBox
            // 
            this.ProductNameRichTextBox.Location = new System.Drawing.Point(156, 85);
            this.ProductNameRichTextBox.Multiline = false;
            this.ProductNameRichTextBox.Name = "ProductNameRichTextBox";
            this.ProductNameRichTextBox.Size = new System.Drawing.Size(295, 19);
            this.ProductNameRichTextBox.TabIndex = 38;
            this.ProductNameRichTextBox.Text = "";
            this.ProductNameRichTextBox.TextChanged += new System.EventHandler(this.RichTextBox_TextChanged);
            this.ProductNameRichTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RichTextBox_KeyDown);
            // 
            // ProductBarCodeRichTextBox
            // 
            this.ProductBarCodeRichTextBox.Location = new System.Drawing.Point(156, 110);
            this.ProductBarCodeRichTextBox.Multiline = false;
            this.ProductBarCodeRichTextBox.Name = "ProductBarCodeRichTextBox";
            this.ProductBarCodeRichTextBox.Size = new System.Drawing.Size(295, 19);
            this.ProductBarCodeRichTextBox.TabIndex = 39;
            this.ProductBarCodeRichTextBox.Text = "";
            this.ProductBarCodeRichTextBox.TextChanged += new System.EventHandler(this.RichTextBox_TextChanged);
            this.ProductBarCodeRichTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RichTextBox_KeyDown);
            // 
            // ProductSeesRichTextBox
            // 
            this.ProductSeesRichTextBox.Location = new System.Drawing.Point(156, 135);
            this.ProductSeesRichTextBox.Multiline = false;
            this.ProductSeesRichTextBox.Name = "ProductSeesRichTextBox";
            this.ProductSeesRichTextBox.Size = new System.Drawing.Size(72, 19);
            this.ProductSeesRichTextBox.TabIndex = 40;
            this.ProductSeesRichTextBox.Text = "";
            this.ProductSeesRichTextBox.TextChanged += new System.EventHandler(this.RichTextBox_TextChanged);
            this.ProductSeesRichTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RichTextBox_KeyDown);
            // 
            // ProductPriceRichTextBox
            // 
            this.ProductPriceRichTextBox.Location = new System.Drawing.Point(156, 160);
            this.ProductPriceRichTextBox.Multiline = false;
            this.ProductPriceRichTextBox.Name = "ProductPriceRichTextBox";
            this.ProductPriceRichTextBox.Size = new System.Drawing.Size(56, 19);
            this.ProductPriceRichTextBox.TabIndex = 41;
            this.ProductPriceRichTextBox.Text = "";
            this.ProductPriceRichTextBox.TextChanged += new System.EventHandler(this.RichTextBox_TextChanged);
            this.ProductPriceRichTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RichTextBox_KeyDown);
            // 
            // ProductTypeTextBox
            // 
            this.ProductTypeTextBox.Location = new System.Drawing.Point(156, 241);
            this.ProductTypeTextBox.Name = "ProductTypeTextBox";
            this.ProductTypeTextBox.Size = new System.Drawing.Size(122, 20);
            this.ProductTypeTextBox.TabIndex = 42;
            this.ProductTypeTextBox.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            this.ProductTypeTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyDown);
            // 
            // ProductTypePriceTextBox
            // 
            this.ProductTypePriceTextBox.Location = new System.Drawing.Point(156, 267);
            this.ProductTypePriceTextBox.Name = "ProductTypePriceTextBox";
            this.ProductTypePriceTextBox.Size = new System.Drawing.Size(49, 20);
            this.ProductTypePriceTextBox.TabIndex = 43;
            this.ProductTypePriceTextBox.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            this.ProductTypePriceTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyDown);
            // 
            // ProductNameInfoLabel
            // 
            this.ProductNameInfoLabel.AutoSize = true;
            this.ProductNameInfoLabel.Location = new System.Drawing.Point(38, 88);
            this.ProductNameInfoLabel.Name = "ProductNameInfoLabel";
            this.ProductNameInfoLabel.Size = new System.Drawing.Size(112, 13);
            this.ProductNameInfoLabel.TabIndex = 44;
            this.ProductNameInfoLabel.Text = "Produkto pavadinimas";
            // 
            // ProductBarCodeLabel
            // 
            this.ProductBarCodeLabel.AutoSize = true;
            this.ProductBarCodeLabel.Location = new System.Drawing.Point(43, 113);
            this.ProductBarCodeLabel.Name = "ProductBarCodeLabel";
            this.ProductBarCodeLabel.Size = new System.Drawing.Size(107, 13);
            this.ProductBarCodeLabel.TabIndex = 45;
            this.ProductBarCodeLabel.Text = "Produkto BAR kodas";
            // 
            // ProductSeesInfoLabel
            // 
            this.ProductSeesInfoLabel.AutoSize = true;
            this.ProductSeesInfoLabel.Location = new System.Drawing.Point(94, 138);
            this.ProductSeesInfoLabel.Name = "ProductSeesInfoLabel";
            this.ProductSeesInfoLabel.Size = new System.Drawing.Size(56, 13);
            this.ProductSeesInfoLabel.TabIndex = 46;
            this.ProductSeesInfoLabel.Text = "Mato tipas";
            // 
            // ProductPriceInfoLabel
            // 
            this.ProductPriceInfoLabel.AutoSize = true;
            this.ProductPriceInfoLabel.Location = new System.Drawing.Point(116, 163);
            this.ProductPriceInfoLabel.Name = "ProductPriceInfoLabel";
            this.ProductPriceInfoLabel.Size = new System.Drawing.Size(34, 13);
            this.ProductPriceInfoLabel.TabIndex = 47;
            this.ProductPriceInfoLabel.Text = "Kaina";
            // 
            // ProductSellingInfoLabel
            // 
            this.ProductSellingInfoLabel.AutoSize = true;
            this.ProductSellingInfoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProductSellingInfoLabel.Location = new System.Drawing.Point(152, 43);
            this.ProductSellingInfoLabel.Name = "ProductSellingInfoLabel";
            this.ProductSellingInfoLabel.Size = new System.Drawing.Size(303, 24);
            this.ProductSellingInfoLabel.TabIndex = 48;
            this.ProductSellingInfoLabel.Text = "Produkto pardavimo Informacija";
            // 
            // ProductExpenditureInfoLabel
            // 
            this.ProductExpenditureInfoLabel.AutoSize = true;
            this.ProductExpenditureInfoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProductExpenditureInfoLabel.Location = new System.Drawing.Point(152, 201);
            this.ProductExpenditureInfoLabel.Name = "ProductExpenditureInfoLabel";
            this.ProductExpenditureInfoLabel.Size = new System.Drawing.Size(286, 24);
            this.ProductExpenditureInfoLabel.TabIndex = 49;
            this.ProductExpenditureInfoLabel.Text = "Produkto sąnaudų informacija";
            // 
            // ProductTypeInfoLabel
            // 
            this.ProductTypeInfoLabel.AutoSize = true;
            this.ProductTypeInfoLabel.Location = new System.Drawing.Point(75, 244);
            this.ProductTypeInfoLabel.Name = "ProductTypeInfoLabel";
            this.ProductTypeInfoLabel.Size = new System.Drawing.Size(75, 13);
            this.ProductTypeInfoLabel.TabIndex = 50;
            this.ProductTypeInfoLabel.Text = "Produkto tipas";
            // 
            // ProductTypePriceInfoLabel
            // 
            this.ProductTypePriceInfoLabel.AutoSize = true;
            this.ProductTypePriceInfoLabel.Location = new System.Drawing.Point(116, 270);
            this.ProductTypePriceInfoLabel.Name = "ProductTypePriceInfoLabel";
            this.ProductTypePriceInfoLabel.Size = new System.Drawing.Size(34, 13);
            this.ProductTypePriceInfoLabel.TabIndex = 51;
            this.ProductTypePriceInfoLabel.Text = "Kaina";
            // 
            // NewProductButton
            // 
            this.NewProductButton.Location = new System.Drawing.Point(41, 397);
            this.NewProductButton.Name = "NewProductButton";
            this.NewProductButton.Size = new System.Drawing.Size(144, 28);
            this.NewProductButton.TabIndex = 52;
            this.NewProductButton.TabStop = false;
            this.NewProductButton.Text = "Pridėti naują produktą";
            this.NewProductButton.UseVisualStyleBackColor = true;
            // 
            // UpdateProductButton
            // 
            this.UpdateProductButton.Location = new System.Drawing.Point(191, 397);
            this.UpdateProductButton.Name = "UpdateProductButton";
            this.UpdateProductButton.Size = new System.Drawing.Size(118, 28);
            this.UpdateProductButton.TabIndex = 53;
            this.UpdateProductButton.TabStop = false;
            this.UpdateProductButton.Text = "Atnaujinti produktą";
            this.UpdateProductButton.UseVisualStyleBackColor = true;
            // 
            // ExistsProductListComboBox
            // 
            this.ExistsProductListComboBox.FormattingEnabled = true;
            this.ExistsProductListComboBox.Location = new System.Drawing.Point(550, 108);
            this.ExistsProductListComboBox.Name = "ExistsProductListComboBox";
            this.ExistsProductListComboBox.Size = new System.Drawing.Size(221, 21);
            this.ExistsProductListComboBox.TabIndex = 54;
            this.ExistsProductListComboBox.TabStop = false;
            // 
            // ProductListInfoLabel
            // 
            this.ProductListInfoLabel.AutoSize = true;
            this.ProductListInfoLabel.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProductListInfoLabel.Location = new System.Drawing.Point(546, 82);
            this.ProductListInfoLabel.Name = "ProductListInfoLabel";
            this.ProductListInfoLabel.Size = new System.Drawing.Size(173, 19);
            this.ProductListInfoLabel.TabIndex = 55;
            this.ProductListInfoLabel.Text = " Esamų Produktų sąrašas";
            // 
            // ChooseProductButton
            // 
            this.ChooseProductButton.Location = new System.Drawing.Point(777, 103);
            this.ChooseProductButton.Name = "ChooseProductButton";
            this.ChooseProductButton.Size = new System.Drawing.Size(162, 28);
            this.ChooseProductButton.TabIndex = 56;
            this.ChooseProductButton.TabStop = false;
            this.ChooseProductButton.Text = "Pateikti produkto informaciją";
            this.ChooseProductButton.UseVisualStyleBackColor = true;
            // 
            // ProductInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(964, 450);
            this.Controls.Add(this.ChooseProductButton);
            this.Controls.Add(this.ProductListInfoLabel);
            this.Controls.Add(this.ExistsProductListComboBox);
            this.Controls.Add(this.UpdateProductButton);
            this.Controls.Add(this.NewProductButton);
            this.Controls.Add(this.ProductTypePriceInfoLabel);
            this.Controls.Add(this.ProductTypeInfoLabel);
            this.Controls.Add(this.ProductExpenditureInfoLabel);
            this.Controls.Add(this.ProductSellingInfoLabel);
            this.Controls.Add(this.ProductPriceInfoLabel);
            this.Controls.Add(this.ProductSeesInfoLabel);
            this.Controls.Add(this.ProductBarCodeLabel);
            this.Controls.Add(this.ProductNameInfoLabel);
            this.Controls.Add(this.ProductTypePriceTextBox);
            this.Controls.Add(this.ProductTypeTextBox);
            this.Controls.Add(this.ProductPriceRichTextBox);
            this.Controls.Add(this.ProductSeesRichTextBox);
            this.Controls.Add(this.ProductBarCodeRichTextBox);
            this.Controls.Add(this.ProductNameRichTextBox);
            this.MaximizeBox = false;
            this.Name = "ProductInfoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Produktų Informacija";
            this.TextChanged += new System.EventHandler(this.RichTextBox_TextChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox ProductNameRichTextBox;
        private System.Windows.Forms.RichTextBox ProductBarCodeRichTextBox;
        private System.Windows.Forms.RichTextBox ProductSeesRichTextBox;
        private System.Windows.Forms.RichTextBox ProductPriceRichTextBox;
        private System.Windows.Forms.TextBox ProductTypeTextBox;
        private System.Windows.Forms.TextBox ProductTypePriceTextBox;
        private System.Windows.Forms.Label ProductNameInfoLabel;
        private System.Windows.Forms.Label ProductBarCodeLabel;
        private System.Windows.Forms.Label ProductSeesInfoLabel;
        private System.Windows.Forms.Label ProductPriceInfoLabel;
        private System.Windows.Forms.Label ProductSellingInfoLabel;
        private System.Windows.Forms.Label ProductExpenditureInfoLabel;
        private System.Windows.Forms.Label ProductTypeInfoLabel;
        private System.Windows.Forms.Label ProductTypePriceInfoLabel;
        private System.Windows.Forms.Button NewProductButton;
        private System.Windows.Forms.Button UpdateProductButton;
        private System.Windows.Forms.ComboBox ExistsProductListComboBox;
        private System.Windows.Forms.Label ProductListInfoLabel;
        private System.Windows.Forms.Button ChooseProductButton;
    }
}