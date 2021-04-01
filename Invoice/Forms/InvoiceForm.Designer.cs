
namespace Invoice.Forms
{
    partial class InvoiceForm
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
            this.PrintInvoicePanel = new System.Windows.Forms.Panel();
            this.InvoiceNameLabel = new System.Windows.Forms.Label();
            this.SerialNumberLabel = new System.Windows.Forms.Label();
            this.InvoiceNumberLabel = new System.Windows.Forms.Label();
            this.DateLabel = new System.Windows.Forms.Label();
            this.DateRichTextBox = new System.Windows.Forms.RichTextBox();
            this.InvoiceNumberRichTextBox = new System.Windows.Forms.RichTextBox();
            this.SerialNumberRichTextBox = new System.Windows.Forms.RichTextBox();
            this.SellerNameLabel = new System.Windows.Forms.Label();
            this.SellerNameRichTextBox = new System.Windows.Forms.RichTextBox();
            this.BuyerNameLabel = new System.Windows.Forms.Label();
            this.BuyerNameRichTextBox = new System.Windows.Forms.RichTextBox();
            this.SellerFirmCodeLabel = new System.Windows.Forms.Label();
            this.SellerFirmCodeRichTextBox = new System.Windows.Forms.RichTextBox();
            this.BuyerFirmCodeLabel = new System.Windows.Forms.Label();
            this.BuyerFirmCodeRichTextBox = new System.Windows.Forms.RichTextBox();
            this.SellerPvmCodeLabel = new System.Windows.Forms.Label();
            this.SellerPvmCodeRichTextBox = new System.Windows.Forms.RichTextBox();
            this.BuyerPvmCodeLabel = new System.Windows.Forms.Label();
            this.BuyerPvmCodeRichTextBox = new System.Windows.Forms.RichTextBox();
            this.SellerAddressLabel = new System.Windows.Forms.Label();
            this.SellerAddressRichTextBox = new System.Windows.Forms.RichTextBox();
            this.BuyerAddressLabel = new System.Windows.Forms.Label();
            this.BuyerAddressRichTextBox = new System.Windows.Forms.RichTextBox();
            this.BuyerPhoneNumberLabel = new System.Windows.Forms.Label();
            this.SellerPhoneNumberRichTextBox = new System.Windows.Forms.RichTextBox();
            this.SellerBankLabel = new System.Windows.Forms.Label();
            this.SellerRichTextBox = new System.Windows.Forms.RichTextBox();
            this.SellerBankAccountNumberLabel = new System.Windows.Forms.Label();
            this.SellerBankAccountNumberRichTextBox = new System.Windows.Forms.RichTextBox();
            this.SellerEmailAddressLabel = new System.Windows.Forms.Label();
            this.SellerEmailAddressRichTextBox = new System.Windows.Forms.RichTextBox();
            this.PrintInvoicePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // PrintInvoicePanel
            // 
            this.PrintInvoicePanel.Controls.Add(this.SellerEmailAddressRichTextBox);
            this.PrintInvoicePanel.Controls.Add(this.SellerEmailAddressLabel);
            this.PrintInvoicePanel.Controls.Add(this.SellerBankAccountNumberRichTextBox);
            this.PrintInvoicePanel.Controls.Add(this.SellerBankAccountNumberLabel);
            this.PrintInvoicePanel.Controls.Add(this.SellerRichTextBox);
            this.PrintInvoicePanel.Controls.Add(this.SellerBankLabel);
            this.PrintInvoicePanel.Controls.Add(this.SellerPhoneNumberRichTextBox);
            this.PrintInvoicePanel.Controls.Add(this.BuyerPhoneNumberLabel);
            this.PrintInvoicePanel.Controls.Add(this.BuyerAddressRichTextBox);
            this.PrintInvoicePanel.Controls.Add(this.BuyerAddressLabel);
            this.PrintInvoicePanel.Controls.Add(this.SellerAddressRichTextBox);
            this.PrintInvoicePanel.Controls.Add(this.SellerAddressLabel);
            this.PrintInvoicePanel.Controls.Add(this.BuyerPvmCodeRichTextBox);
            this.PrintInvoicePanel.Controls.Add(this.BuyerPvmCodeLabel);
            this.PrintInvoicePanel.Controls.Add(this.SellerPvmCodeRichTextBox);
            this.PrintInvoicePanel.Controls.Add(this.SellerPvmCodeLabel);
            this.PrintInvoicePanel.Controls.Add(this.BuyerFirmCodeRichTextBox);
            this.PrintInvoicePanel.Controls.Add(this.BuyerFirmCodeLabel);
            this.PrintInvoicePanel.Controls.Add(this.SellerFirmCodeRichTextBox);
            this.PrintInvoicePanel.Controls.Add(this.SellerFirmCodeLabel);
            this.PrintInvoicePanel.Controls.Add(this.BuyerNameRichTextBox);
            this.PrintInvoicePanel.Controls.Add(this.BuyerNameLabel);
            this.PrintInvoicePanel.Controls.Add(this.SellerNameRichTextBox);
            this.PrintInvoicePanel.Controls.Add(this.SellerNameLabel);
            this.PrintInvoicePanel.Controls.Add(this.SerialNumberRichTextBox);
            this.PrintInvoicePanel.Controls.Add(this.InvoiceNumberRichTextBox);
            this.PrintInvoicePanel.Controls.Add(this.DateRichTextBox);
            this.PrintInvoicePanel.Controls.Add(this.DateLabel);
            this.PrintInvoicePanel.Controls.Add(this.InvoiceNumberLabel);
            this.PrintInvoicePanel.Controls.Add(this.SerialNumberLabel);
            this.PrintInvoicePanel.Controls.Add(this.InvoiceNameLabel);
            this.PrintInvoicePanel.Location = new System.Drawing.Point(125, 12);
            this.PrintInvoicePanel.Name = "PrintInvoicePanel";
            this.PrintInvoicePanel.Size = new System.Drawing.Size(820, 926);
            this.PrintInvoicePanel.TabIndex = 0;
            // 
            // InvoiceNameLabel
            // 
            this.InvoiceNameLabel.AutoSize = true;
            this.InvoiceNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InvoiceNameLabel.Location = new System.Drawing.Point(266, 38);
            this.InvoiceNameLabel.Name = "InvoiceNameLabel";
            this.InvoiceNameLabel.Size = new System.Drawing.Size(274, 24);
            this.InvoiceNameLabel.TabIndex = 0;
            this.InvoiceNameLabel.Text = "PVM SĄSKAITA - FAKTŪRA";
            // 
            // SerialNumberLabel
            // 
            this.SerialNumberLabel.AutoSize = true;
            this.SerialNumberLabel.Location = new System.Drawing.Point(43, 93);
            this.SerialNumberLabel.Name = "SerialNumberLabel";
            this.SerialNumberLabel.Size = new System.Drawing.Size(44, 13);
            this.SerialNumberLabel.TabIndex = 1;
            this.SerialNumberLabel.Text = "SERIJA";
            // 
            // InvoiceNumberLabel
            // 
            this.InvoiceNumberLabel.AutoSize = true;
            this.InvoiceNumberLabel.Location = new System.Drawing.Point(345, 90);
            this.InvoiceNumberLabel.Name = "InvoiceNumberLabel";
            this.InvoiceNumberLabel.Size = new System.Drawing.Size(26, 13);
            this.InvoiceNumberLabel.TabIndex = 2;
            this.InvoiceNumberLabel.Text = "NR.";
            // 
            // DateLabel
            // 
            this.DateLabel.AutoSize = true;
            this.DateLabel.Location = new System.Drawing.Point(657, 90);
            this.DateLabel.Name = "DateLabel";
            this.DateLabel.Size = new System.Drawing.Size(30, 13);
            this.DateLabel.TabIndex = 3;
            this.DateLabel.Text = "Data";
            // 
            // DateRichTextBox
            // 
            this.DateRichTextBox.Location = new System.Drawing.Point(693, 87);
            this.DateRichTextBox.Name = "DateRichTextBox";
            this.DateRichTextBox.Size = new System.Drawing.Size(83, 19);
            this.DateRichTextBox.TabIndex = 6;
            this.DateRichTextBox.Text = "";
            // 
            // InvoiceNumberRichTextBox
            // 
            this.InvoiceNumberRichTextBox.Location = new System.Drawing.Point(377, 87);
            this.InvoiceNumberRichTextBox.Name = "InvoiceNumberRichTextBox";
            this.InvoiceNumberRichTextBox.Size = new System.Drawing.Size(83, 19);
            this.InvoiceNumberRichTextBox.TabIndex = 7;
            this.InvoiceNumberRichTextBox.Text = "";
            // 
            // SerialNumberRichTextBox
            // 
            this.SerialNumberRichTextBox.Location = new System.Drawing.Point(93, 90);
            this.SerialNumberRichTextBox.Name = "SerialNumberRichTextBox";
            this.SerialNumberRichTextBox.Size = new System.Drawing.Size(83, 19);
            this.SerialNumberRichTextBox.TabIndex = 8;
            this.SerialNumberRichTextBox.Text = "";
            // 
            // SellerNameLabel
            // 
            this.SellerNameLabel.AutoSize = true;
            this.SellerNameLabel.Location = new System.Drawing.Point(43, 135);
            this.SellerNameLabel.Name = "SellerNameLabel";
            this.SellerNameLabel.Size = new System.Drawing.Size(117, 13);
            this.SellerNameLabel.TabIndex = 9;
            this.SellerNameLabel.Text = "Pardavėjo pavadinimas";
            // 
            // SellerNameRichTextBox
            // 
            this.SellerNameRichTextBox.Location = new System.Drawing.Point(166, 135);
            this.SellerNameRichTextBox.Name = "SellerNameRichTextBox";
            this.SellerNameRichTextBox.Size = new System.Drawing.Size(205, 19);
            this.SellerNameRichTextBox.TabIndex = 10;
            this.SellerNameRichTextBox.Text = "";
            // 
            // BuyerNameLabel
            // 
            this.BuyerNameLabel.AutoSize = true;
            this.BuyerNameLabel.Location = new System.Drawing.Point(448, 138);
            this.BuyerNameLabel.Name = "BuyerNameLabel";
            this.BuyerNameLabel.Size = new System.Drawing.Size(101, 13);
            this.BuyerNameLabel.TabIndex = 11;
            this.BuyerNameLabel.Text = "Pirkėjo pavadinimas";
            // 
            // BuyerNameRichTextBox
            // 
            this.BuyerNameRichTextBox.Location = new System.Drawing.Point(555, 135);
            this.BuyerNameRichTextBox.Name = "BuyerNameRichTextBox";
            this.BuyerNameRichTextBox.Size = new System.Drawing.Size(221, 19);
            this.BuyerNameRichTextBox.TabIndex = 12;
            this.BuyerNameRichTextBox.Text = "";
            // 
            // SellerFirmCodeLabel
            // 
            this.SellerFirmCodeLabel.AutoSize = true;
            this.SellerFirmCodeLabel.Location = new System.Drawing.Point(43, 173);
            this.SellerFirmCodeLabel.Name = "SellerFirmCodeLabel";
            this.SellerFirmCodeLabel.Size = new System.Drawing.Size(73, 13);
            this.SellerFirmCodeLabel.TabIndex = 13;
            this.SellerFirmCodeLabel.Text = "Įmonės kodas";
            // 
            // SellerFirmCodeRichTextBox
            // 
            this.SellerFirmCodeRichTextBox.Location = new System.Drawing.Point(166, 170);
            this.SellerFirmCodeRichTextBox.Name = "SellerFirmCodeRichTextBox";
            this.SellerFirmCodeRichTextBox.Size = new System.Drawing.Size(205, 19);
            this.SellerFirmCodeRichTextBox.TabIndex = 14;
            this.SellerFirmCodeRichTextBox.Text = "";
            // 
            // BuyerFirmCodeLabel
            // 
            this.BuyerFirmCodeLabel.AutoSize = true;
            this.BuyerFirmCodeLabel.Location = new System.Drawing.Point(448, 173);
            this.BuyerFirmCodeLabel.Name = "BuyerFirmCodeLabel";
            this.BuyerFirmCodeLabel.Size = new System.Drawing.Size(73, 13);
            this.BuyerFirmCodeLabel.TabIndex = 15;
            this.BuyerFirmCodeLabel.Text = "Įmonės kodas";
            // 
            // BuyerFirmCodeRichTextBox
            // 
            this.BuyerFirmCodeRichTextBox.Location = new System.Drawing.Point(555, 170);
            this.BuyerFirmCodeRichTextBox.Name = "BuyerFirmCodeRichTextBox";
            this.BuyerFirmCodeRichTextBox.Size = new System.Drawing.Size(221, 19);
            this.BuyerFirmCodeRichTextBox.TabIndex = 16;
            this.BuyerFirmCodeRichTextBox.Text = "";
            // 
            // SellerPvmCodeLabel
            // 
            this.SellerPvmCodeLabel.AutoSize = true;
            this.SellerPvmCodeLabel.Location = new System.Drawing.Point(43, 207);
            this.SellerPvmCodeLabel.Name = "SellerPvmCodeLabel";
            this.SellerPvmCodeLabel.Size = new System.Drawing.Size(62, 13);
            this.SellerPvmCodeLabel.TabIndex = 17;
            this.SellerPvmCodeLabel.Text = "PVM kodas";
            // 
            // SellerPvmCodeRichTextBox
            // 
            this.SellerPvmCodeRichTextBox.Location = new System.Drawing.Point(166, 204);
            this.SellerPvmCodeRichTextBox.Name = "SellerPvmCodeRichTextBox";
            this.SellerPvmCodeRichTextBox.Size = new System.Drawing.Size(205, 19);
            this.SellerPvmCodeRichTextBox.TabIndex = 18;
            this.SellerPvmCodeRichTextBox.Text = "";
            // 
            // BuyerPvmCodeLabel
            // 
            this.BuyerPvmCodeLabel.AutoSize = true;
            this.BuyerPvmCodeLabel.Location = new System.Drawing.Point(448, 207);
            this.BuyerPvmCodeLabel.Name = "BuyerPvmCodeLabel";
            this.BuyerPvmCodeLabel.Size = new System.Drawing.Size(62, 13);
            this.BuyerPvmCodeLabel.TabIndex = 19;
            this.BuyerPvmCodeLabel.Text = "PVM kodas";
            // 
            // BuyerPvmCodeRichTextBox
            // 
            this.BuyerPvmCodeRichTextBox.Location = new System.Drawing.Point(555, 204);
            this.BuyerPvmCodeRichTextBox.Name = "BuyerPvmCodeRichTextBox";
            this.BuyerPvmCodeRichTextBox.Size = new System.Drawing.Size(221, 19);
            this.BuyerPvmCodeRichTextBox.TabIndex = 20;
            this.BuyerPvmCodeRichTextBox.Text = "";
            // 
            // SellerAddressLabel
            // 
            this.SellerAddressLabel.AutoSize = true;
            this.SellerAddressLabel.Location = new System.Drawing.Point(43, 241);
            this.SellerAddressLabel.Name = "SellerAddressLabel";
            this.SellerAddressLabel.Size = new System.Drawing.Size(45, 13);
            this.SellerAddressLabel.TabIndex = 21;
            this.SellerAddressLabel.Text = "Adresas";
            // 
            // SellerAddressRichTextBox
            // 
            this.SellerAddressRichTextBox.Location = new System.Drawing.Point(166, 238);
            this.SellerAddressRichTextBox.Name = "SellerAddressRichTextBox";
            this.SellerAddressRichTextBox.Size = new System.Drawing.Size(205, 19);
            this.SellerAddressRichTextBox.TabIndex = 22;
            this.SellerAddressRichTextBox.Text = "";
            // 
            // BuyerAddressLabel
            // 
            this.BuyerAddressLabel.AutoSize = true;
            this.BuyerAddressLabel.Location = new System.Drawing.Point(448, 241);
            this.BuyerAddressLabel.Name = "BuyerAddressLabel";
            this.BuyerAddressLabel.Size = new System.Drawing.Size(45, 13);
            this.BuyerAddressLabel.TabIndex = 23;
            this.BuyerAddressLabel.Text = "Adresas";
            // 
            // BuyerAddressRichTextBox
            // 
            this.BuyerAddressRichTextBox.Location = new System.Drawing.Point(555, 238);
            this.BuyerAddressRichTextBox.Name = "BuyerAddressRichTextBox";
            this.BuyerAddressRichTextBox.Size = new System.Drawing.Size(221, 19);
            this.BuyerAddressRichTextBox.TabIndex = 24;
            this.BuyerAddressRichTextBox.Text = "";
            // 
            // BuyerPhoneNumberLabel
            // 
            this.BuyerPhoneNumberLabel.AutoSize = true;
            this.BuyerPhoneNumberLabel.Location = new System.Drawing.Point(43, 277);
            this.BuyerPhoneNumberLabel.Name = "BuyerPhoneNumberLabel";
            this.BuyerPhoneNumberLabel.Size = new System.Drawing.Size(54, 13);
            this.BuyerPhoneNumberLabel.TabIndex = 25;
            this.BuyerPhoneNumberLabel.Text = "Telefonas";
            // 
            // SellerPhoneNumberRichTextBox
            // 
            this.SellerPhoneNumberRichTextBox.Location = new System.Drawing.Point(166, 274);
            this.SellerPhoneNumberRichTextBox.Name = "SellerPhoneNumberRichTextBox";
            this.SellerPhoneNumberRichTextBox.Size = new System.Drawing.Size(205, 19);
            this.SellerPhoneNumberRichTextBox.TabIndex = 26;
            this.SellerPhoneNumberRichTextBox.Text = "";
            // 
            // SellerBankLabel
            // 
            this.SellerBankLabel.AutoSize = true;
            this.SellerBankLabel.Location = new System.Drawing.Point(43, 312);
            this.SellerBankLabel.Name = "SellerBankLabel";
            this.SellerBankLabel.Size = new System.Drawing.Size(43, 13);
            this.SellerBankLabel.TabIndex = 27;
            this.SellerBankLabel.Text = "Bankas";
            // 
            // SellerRichTextBox
            // 
            this.SellerRichTextBox.Location = new System.Drawing.Point(166, 309);
            this.SellerRichTextBox.Name = "SellerRichTextBox";
            this.SellerRichTextBox.Size = new System.Drawing.Size(205, 19);
            this.SellerRichTextBox.TabIndex = 28;
            this.SellerRichTextBox.Text = "";
            // 
            // SellerBankAccountNumberLabel
            // 
            this.SellerBankAccountNumberLabel.AutoSize = true;
            this.SellerBankAccountNumberLabel.Location = new System.Drawing.Point(43, 347);
            this.SellerBankAccountNumberLabel.Name = "SellerBankAccountNumberLabel";
            this.SellerBankAccountNumberLabel.Size = new System.Drawing.Size(48, 13);
            this.SellerBankAccountNumberLabel.TabIndex = 29;
            this.SellerBankAccountNumberLabel.Text = "Sąskaita";
            // 
            // SellerBankAccountNumberRichTextBox
            // 
            this.SellerBankAccountNumberRichTextBox.Location = new System.Drawing.Point(166, 344);
            this.SellerBankAccountNumberRichTextBox.Name = "SellerBankAccountNumberRichTextBox";
            this.SellerBankAccountNumberRichTextBox.Size = new System.Drawing.Size(205, 19);
            this.SellerBankAccountNumberRichTextBox.TabIndex = 30;
            this.SellerBankAccountNumberRichTextBox.Text = "";
            // 
            // SellerEmailAddressLabel
            // 
            this.SellerEmailAddressLabel.AutoSize = true;
            this.SellerEmailAddressLabel.Location = new System.Drawing.Point(43, 381);
            this.SellerEmailAddressLabel.Name = "SellerEmailAddressLabel";
            this.SellerEmailAddressLabel.Size = new System.Drawing.Size(48, 13);
            this.SellerEmailAddressLabel.TabIndex = 31;
            this.SellerEmailAddressLabel.Text = "Sąskaita";
            // 
            // SellerEmailAddressRichTextBox
            // 
            this.SellerEmailAddressRichTextBox.Location = new System.Drawing.Point(166, 378);
            this.SellerEmailAddressRichTextBox.Name = "SellerEmailAddressRichTextBox";
            this.SellerEmailAddressRichTextBox.Size = new System.Drawing.Size(205, 19);
            this.SellerEmailAddressRichTextBox.TabIndex = 32;
            this.SellerEmailAddressRichTextBox.Text = "";
            // 
            // InvoiceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1064, 1001);
            this.Controls.Add(this.PrintInvoicePanel);
            this.MaximizeBox = false;
            this.Name = "InvoiceForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "InvoiceForm";
            this.PrintInvoicePanel.ResumeLayout(false);
            this.PrintInvoicePanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PrintInvoicePanel;
        private System.Windows.Forms.Label InvoiceNameLabel;
        private System.Windows.Forms.RichTextBox DateRichTextBox;
        private System.Windows.Forms.Label DateLabel;
        private System.Windows.Forms.Label InvoiceNumberLabel;
        private System.Windows.Forms.Label SerialNumberLabel;
        private System.Windows.Forms.RichTextBox SerialNumberRichTextBox;
        private System.Windows.Forms.RichTextBox InvoiceNumberRichTextBox;
        private System.Windows.Forms.RichTextBox BuyerNameRichTextBox;
        private System.Windows.Forms.Label BuyerNameLabel;
        private System.Windows.Forms.RichTextBox SellerNameRichTextBox;
        private System.Windows.Forms.Label SellerNameLabel;
        private System.Windows.Forms.RichTextBox BuyerAddressRichTextBox;
        private System.Windows.Forms.Label BuyerAddressLabel;
        private System.Windows.Forms.RichTextBox SellerAddressRichTextBox;
        private System.Windows.Forms.Label SellerAddressLabel;
        private System.Windows.Forms.RichTextBox BuyerPvmCodeRichTextBox;
        private System.Windows.Forms.Label BuyerPvmCodeLabel;
        private System.Windows.Forms.RichTextBox SellerPvmCodeRichTextBox;
        private System.Windows.Forms.Label SellerPvmCodeLabel;
        private System.Windows.Forms.RichTextBox BuyerFirmCodeRichTextBox;
        private System.Windows.Forms.Label BuyerFirmCodeLabel;
        private System.Windows.Forms.RichTextBox SellerFirmCodeRichTextBox;
        private System.Windows.Forms.Label SellerFirmCodeLabel;
        private System.Windows.Forms.RichTextBox SellerPhoneNumberRichTextBox;
        private System.Windows.Forms.Label BuyerPhoneNumberLabel;
        private System.Windows.Forms.RichTextBox SellerRichTextBox;
        private System.Windows.Forms.Label SellerBankLabel;
        private System.Windows.Forms.RichTextBox SellerEmailAddressRichTextBox;
        private System.Windows.Forms.Label SellerEmailAddressLabel;
        private System.Windows.Forms.RichTextBox SellerBankAccountNumberRichTextBox;
        private System.Windows.Forms.Label SellerBankAccountNumberLabel;
    }
}