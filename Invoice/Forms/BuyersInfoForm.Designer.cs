
namespace Invoice.Forms
{
    partial class BuyersInfoForm
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
            this.BuyerNameRichTextBox = new System.Windows.Forms.RichTextBox();
            this.BuyerFirmCodeRichTextBox = new System.Windows.Forms.RichTextBox();
            this.BuyerPvmCodeRichTextBox = new System.Windows.Forms.RichTextBox();
            this.BuyerAddressRichTextBox = new System.Windows.Forms.RichTextBox();
            this.BuyerNameLabel = new System.Windows.Forms.Label();
            this.BuyerFirmCodeLabel = new System.Windows.Forms.Label();
            this.BuyerPvmCodeLabel = new System.Windows.Forms.Label();
            this.BuyerAddressLabel = new System.Windows.Forms.Label();
            this.AddNewBuyerButton = new System.Windows.Forms.Button();
            this.UpdateBuyerButton = new System.Windows.Forms.Button();
            this.ExistsBuyerListComboBox = new System.Windows.Forms.ComboBox();
            this.ExistsBuyersListInfoLabel = new System.Windows.Forms.Label();
            this.LoadBuyerBySelectedNameButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BuyerNameRichTextBox
            // 
            this.BuyerNameRichTextBox.Location = new System.Drawing.Point(138, 86);
            this.BuyerNameRichTextBox.Multiline = false;
            this.BuyerNameRichTextBox.Name = "BuyerNameRichTextBox";
            this.BuyerNameRichTextBox.Size = new System.Drawing.Size(221, 19);
            this.BuyerNameRichTextBox.TabIndex = 13;
            this.BuyerNameRichTextBox.Text = "";
            // 
            // BuyerFirmCodeRichTextBox
            // 
            this.BuyerFirmCodeRichTextBox.Location = new System.Drawing.Point(138, 123);
            this.BuyerFirmCodeRichTextBox.Multiline = false;
            this.BuyerFirmCodeRichTextBox.Name = "BuyerFirmCodeRichTextBox";
            this.BuyerFirmCodeRichTextBox.Size = new System.Drawing.Size(221, 19);
            this.BuyerFirmCodeRichTextBox.TabIndex = 17;
            this.BuyerFirmCodeRichTextBox.Text = "";
            // 
            // BuyerPvmCodeRichTextBox
            // 
            this.BuyerPvmCodeRichTextBox.Location = new System.Drawing.Point(138, 160);
            this.BuyerPvmCodeRichTextBox.Multiline = false;
            this.BuyerPvmCodeRichTextBox.Name = "BuyerPvmCodeRichTextBox";
            this.BuyerPvmCodeRichTextBox.Size = new System.Drawing.Size(221, 19);
            this.BuyerPvmCodeRichTextBox.TabIndex = 21;
            this.BuyerPvmCodeRichTextBox.Text = "";
            // 
            // BuyerAddressRichTextBox
            // 
            this.BuyerAddressRichTextBox.Location = new System.Drawing.Point(138, 197);
            this.BuyerAddressRichTextBox.Multiline = false;
            this.BuyerAddressRichTextBox.Name = "BuyerAddressRichTextBox";
            this.BuyerAddressRichTextBox.Size = new System.Drawing.Size(221, 19);
            this.BuyerAddressRichTextBox.TabIndex = 25;
            this.BuyerAddressRichTextBox.Text = "";
            // 
            // BuyerNameLabel
            // 
            this.BuyerNameLabel.AutoSize = true;
            this.BuyerNameLabel.Location = new System.Drawing.Point(31, 89);
            this.BuyerNameLabel.Name = "BuyerNameLabel";
            this.BuyerNameLabel.Size = new System.Drawing.Size(101, 13);
            this.BuyerNameLabel.TabIndex = 26;
            this.BuyerNameLabel.Text = "Pirkėjo pavadinimas";
            // 
            // BuyerFirmCodeLabel
            // 
            this.BuyerFirmCodeLabel.AutoSize = true;
            this.BuyerFirmCodeLabel.Location = new System.Drawing.Point(31, 126);
            this.BuyerFirmCodeLabel.Name = "BuyerFirmCodeLabel";
            this.BuyerFirmCodeLabel.Size = new System.Drawing.Size(73, 13);
            this.BuyerFirmCodeLabel.TabIndex = 27;
            this.BuyerFirmCodeLabel.Text = "Įmonės kodas";
            // 
            // BuyerPvmCodeLabel
            // 
            this.BuyerPvmCodeLabel.AutoSize = true;
            this.BuyerPvmCodeLabel.Location = new System.Drawing.Point(31, 163);
            this.BuyerPvmCodeLabel.Name = "BuyerPvmCodeLabel";
            this.BuyerPvmCodeLabel.Size = new System.Drawing.Size(62, 13);
            this.BuyerPvmCodeLabel.TabIndex = 28;
            this.BuyerPvmCodeLabel.Text = "PVM kodas";
            // 
            // BuyerAddressLabel
            // 
            this.BuyerAddressLabel.AutoSize = true;
            this.BuyerAddressLabel.Location = new System.Drawing.Point(33, 200);
            this.BuyerAddressLabel.Name = "BuyerAddressLabel";
            this.BuyerAddressLabel.Size = new System.Drawing.Size(45, 13);
            this.BuyerAddressLabel.TabIndex = 29;
            this.BuyerAddressLabel.Text = "Adresas";
            // 
            // AddNewBuyerButton
            // 
            this.AddNewBuyerButton.Location = new System.Drawing.Point(34, 27);
            this.AddNewBuyerButton.Name = "AddNewBuyerButton";
            this.AddNewBuyerButton.Size = new System.Drawing.Size(141, 30);
            this.AddNewBuyerButton.TabIndex = 30;
            this.AddNewBuyerButton.Text = "Pridėti Naują pirkėją";
            this.AddNewBuyerButton.UseVisualStyleBackColor = true;
            // 
            // UpdateBuyerButton
            // 
            this.UpdateBuyerButton.Location = new System.Drawing.Point(183, 27);
            this.UpdateBuyerButton.Name = "UpdateBuyerButton";
            this.UpdateBuyerButton.Size = new System.Drawing.Size(176, 30);
            this.UpdateBuyerButton.TabIndex = 31;
            this.UpdateBuyerButton.Text = "Atnaujinti pirkėjo informaciją";
            this.UpdateBuyerButton.UseVisualStyleBackColor = true;
            // 
            // ExistsBuyerListComboBox
            // 
            this.ExistsBuyerListComboBox.FormattingEnabled = true;
            this.ExistsBuyerListComboBox.Location = new System.Drawing.Point(138, 229);
            this.ExistsBuyerListComboBox.Name = "ExistsBuyerListComboBox";
            this.ExistsBuyerListComboBox.Size = new System.Drawing.Size(221, 21);
            this.ExistsBuyerListComboBox.TabIndex = 32;
            // 
            // ExistsBuyersListInfoLabel
            // 
            this.ExistsBuyersListInfoLabel.AutoSize = true;
            this.ExistsBuyersListInfoLabel.Location = new System.Drawing.Point(33, 237);
            this.ExistsBuyersListInfoLabel.Name = "ExistsBuyersListInfoLabel";
            this.ExistsBuyersListInfoLabel.Size = new System.Drawing.Size(80, 13);
            this.ExistsBuyersListInfoLabel.TabIndex = 33;
            this.ExistsBuyersListInfoLabel.Text = "Pirkėjų Sąrašas";
            // 
            // LoadBuyerBySelectedNameButton
            // 
            this.LoadBuyerBySelectedNameButton.Location = new System.Drawing.Point(365, 223);
            this.LoadBuyerBySelectedNameButton.Name = "LoadBuyerBySelectedNameButton";
            this.LoadBuyerBySelectedNameButton.Size = new System.Drawing.Size(154, 30);
            this.LoadBuyerBySelectedNameButton.TabIndex = 34;
            this.LoadBuyerBySelectedNameButton.Text = "Pateikti Pirkėjo Informaciją";
            this.LoadBuyerBySelectedNameButton.UseVisualStyleBackColor = true;
            // 
            // BuyersInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(599, 423);
            this.Controls.Add(this.LoadBuyerBySelectedNameButton);
            this.Controls.Add(this.ExistsBuyersListInfoLabel);
            this.Controls.Add(this.ExistsBuyerListComboBox);
            this.Controls.Add(this.UpdateBuyerButton);
            this.Controls.Add(this.AddNewBuyerButton);
            this.Controls.Add(this.BuyerAddressLabel);
            this.Controls.Add(this.BuyerPvmCodeLabel);
            this.Controls.Add(this.BuyerFirmCodeLabel);
            this.Controls.Add(this.BuyerNameLabel);
            this.Controls.Add(this.BuyerAddressRichTextBox);
            this.Controls.Add(this.BuyerPvmCodeRichTextBox);
            this.Controls.Add(this.BuyerFirmCodeRichTextBox);
            this.Controls.Add(this.BuyerNameRichTextBox);
            this.MaximizeBox = false;
            this.Name = "BuyersInfoForm";
            this.Text = "Pirkėjų informacija";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox BuyerNameRichTextBox;
        private System.Windows.Forms.RichTextBox BuyerFirmCodeRichTextBox;
        private System.Windows.Forms.RichTextBox BuyerPvmCodeRichTextBox;
        private System.Windows.Forms.RichTextBox BuyerAddressRichTextBox;
        private System.Windows.Forms.Label BuyerNameLabel;
        private System.Windows.Forms.Label BuyerFirmCodeLabel;
        private System.Windows.Forms.Label BuyerPvmCodeLabel;
        private System.Windows.Forms.Label BuyerAddressLabel;
        private System.Windows.Forms.Button AddNewBuyerButton;
        private System.Windows.Forms.Button UpdateBuyerButton;
        private System.Windows.Forms.ComboBox ExistsBuyerListComboBox;
        private System.Windows.Forms.Label ExistsBuyersListInfoLabel;
        private System.Windows.Forms.Button LoadBuyerBySelectedNameButton;
    }
}