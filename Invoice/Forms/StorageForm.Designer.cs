﻿
namespace Invoice.Forms
{
    partial class StorageForm
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
            this.InformationOfDataGridViewTypeLabel = new System.Windows.Forms.Label();
            this.StorageDataGridView = new System.Windows.Forms.DataGridView();
            this.ProductTypeInformationLabel = new System.Windows.Forms.Label();
            this.ProductTypeControlPanel = new System.Windows.Forms.Panel();
            this.ProductTypeYearInfoLabel = new System.Windows.Forms.Label();
            this.ProductTypeYearComboBox = new System.Windows.Forms.ComboBox();
            this.GetAllInfoByYearButton = new System.Windows.Forms.Button();
            this.GetAllInfoByProductNameButton = new System.Windows.Forms.Button();
            this.ProductTypeSpecificNameComboBox = new System.Windows.Forms.ComboBox();
            this.ProductTypeInfoLabel = new System.Windows.Forms.Label();
            this.DepositControlPanel = new System.Windows.Forms.Panel();
            this.DepositYearComboBox = new System.Windows.Forms.ComboBox();
            this.DepositYearInfoLabel = new System.Windows.Forms.Label();
            this.GetDepositInfoByNameButton = new System.Windows.Forms.Button();
            this.GetAllInfoDeposit = new System.Windows.Forms.Button();
            this.DepositProductNameListComboBox = new System.Windows.Forms.ComboBox();
            this.DepositProductnameInformationLabel = new System.Windows.Forms.Label();
            this.DepositInformationLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.StorageDataGridView)).BeginInit();
            this.ProductTypeControlPanel.SuspendLayout();
            this.DepositControlPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // InformationOfDataGridViewTypeLabel
            // 
            this.InformationOfDataGridViewTypeLabel.AutoSize = true;
            this.InformationOfDataGridViewTypeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InformationOfDataGridViewTypeLabel.Location = new System.Drawing.Point(45, 46);
            this.InformationOfDataGridViewTypeLabel.Name = "InformationOfDataGridViewTypeLabel";
            this.InformationOfDataGridViewTypeLabel.Size = new System.Drawing.Size(92, 20);
            this.InformationOfDataGridViewTypeLabel.TabIndex = 42;
            this.InformationOfDataGridViewTypeLabel.Text = "Data type ";
            // 
            // StorageDataGridView
            // 
            this.StorageDataGridView.AllowUserToAddRows = false;
            this.StorageDataGridView.AllowUserToDeleteRows = false;
            this.StorageDataGridView.AllowUserToResizeColumns = false;
            this.StorageDataGridView.AllowUserToResizeRows = false;
            this.StorageDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.StorageDataGridView.Location = new System.Drawing.Point(49, 81);
            this.StorageDataGridView.MultiSelect = false;
            this.StorageDataGridView.Name = "StorageDataGridView";
            this.StorageDataGridView.ReadOnly = true;
            this.StorageDataGridView.Size = new System.Drawing.Size(614, 563);
            this.StorageDataGridView.TabIndex = 43;
            this.StorageDataGridView.TabStop = false;
            this.StorageDataGridView.Paint += new System.Windows.Forms.PaintEventHandler(this.StorageDataGridView_Paint);
            // 
            // ProductTypeInformationLabel
            // 
            this.ProductTypeInformationLabel.AutoSize = true;
            this.ProductTypeInformationLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProductTypeInformationLabel.Location = new System.Drawing.Point(54, 20);
            this.ProductTypeInformationLabel.Name = "ProductTypeInformationLabel";
            this.ProductTypeInformationLabel.Size = new System.Drawing.Size(294, 20);
            this.ProductTypeInformationLabel.TabIndex = 44;
            this.ProductTypeInformationLabel.Text = "Produktai pagal Sąskaitas Faktūras";
            // 
            // ProductTypeControlPanel
            // 
            this.ProductTypeControlPanel.Controls.Add(this.ProductTypeYearInfoLabel);
            this.ProductTypeControlPanel.Controls.Add(this.ProductTypeYearComboBox);
            this.ProductTypeControlPanel.Controls.Add(this.GetAllInfoByYearButton);
            this.ProductTypeControlPanel.Controls.Add(this.GetAllInfoByProductNameButton);
            this.ProductTypeControlPanel.Controls.Add(this.ProductTypeSpecificNameComboBox);
            this.ProductTypeControlPanel.Controls.Add(this.ProductTypeInfoLabel);
            this.ProductTypeControlPanel.Controls.Add(this.ProductTypeInformationLabel);
            this.ProductTypeControlPanel.Location = new System.Drawing.Point(681, 81);
            this.ProductTypeControlPanel.Name = "ProductTypeControlPanel";
            this.ProductTypeControlPanel.Size = new System.Drawing.Size(437, 233);
            this.ProductTypeControlPanel.TabIndex = 45;
            // 
            // ProductTypeYearInfoLabel
            // 
            this.ProductTypeYearInfoLabel.AutoSize = true;
            this.ProductTypeYearInfoLabel.Location = new System.Drawing.Point(261, 116);
            this.ProductTypeYearInfoLabel.Name = "ProductTypeYearInfoLabel";
            this.ProductTypeYearInfoLabel.Size = new System.Drawing.Size(33, 13);
            this.ProductTypeYearInfoLabel.TabIndex = 49;
            this.ProductTypeYearInfoLabel.Text = "Metai";
            // 
            // ProductTypeYearComboBox
            // 
            this.ProductTypeYearComboBox.FormattingEnabled = true;
            this.ProductTypeYearComboBox.Location = new System.Drawing.Point(300, 113);
            this.ProductTypeYearComboBox.Name = "ProductTypeYearComboBox";
            this.ProductTypeYearComboBox.Size = new System.Drawing.Size(79, 21);
            this.ProductTypeYearComboBox.TabIndex = 46;
            this.ProductTypeYearComboBox.TabStop = false;
            // 
            // GetAllInfoByYearButton
            // 
            this.GetAllInfoByYearButton.Location = new System.Drawing.Point(138, 62);
            this.GetAllInfoByYearButton.Name = "GetAllInfoByYearButton";
            this.GetAllInfoByYearButton.Size = new System.Drawing.Size(109, 35);
            this.GetAllInfoByYearButton.TabIndex = 48;
            this.GetAllInfoByYearButton.TabStop = false;
            this.GetAllInfoByYearButton.Text = "  Gauti Info \r\npagal metus ";
            this.GetAllInfoByYearButton.UseVisualStyleBackColor = true;
            // 
            // GetAllInfoByProductNameButton
            // 
            this.GetAllInfoByProductNameButton.Location = new System.Drawing.Point(37, 62);
            this.GetAllInfoByProductNameButton.Name = "GetAllInfoByProductNameButton";
            this.GetAllInfoByProductNameButton.Size = new System.Drawing.Size(95, 35);
            this.GetAllInfoByProductNameButton.TabIndex = 47;
            this.GetAllInfoByProductNameButton.TabStop = false;
            this.GetAllInfoByProductNameButton.Text = "Gauti Info";
            this.GetAllInfoByProductNameButton.UseVisualStyleBackColor = true;
            this.GetAllInfoByProductNameButton.Click += new System.EventHandler(this.GetAllInfoByProductNameButton_Click);
            // 
            // ProductTypeSpecificNameComboBox
            // 
            this.ProductTypeSpecificNameComboBox.FormattingEnabled = true;
            this.ProductTypeSpecificNameComboBox.Location = new System.Drawing.Point(37, 141);
            this.ProductTypeSpecificNameComboBox.Name = "ProductTypeSpecificNameComboBox";
            this.ProductTypeSpecificNameComboBox.Size = new System.Drawing.Size(257, 21);
            this.ProductTypeSpecificNameComboBox.TabIndex = 46;
            this.ProductTypeSpecificNameComboBox.TabStop = false;
            // 
            // ProductTypeInfoLabel
            // 
            this.ProductTypeInfoLabel.AutoSize = true;
            this.ProductTypeInfoLabel.Location = new System.Drawing.Point(34, 116);
            this.ProductTypeInfoLabel.Name = "ProductTypeInfoLabel";
            this.ProductTypeInfoLabel.Size = new System.Drawing.Size(75, 13);
            this.ProductTypeInfoLabel.TabIndex = 45;
            this.ProductTypeInfoLabel.Text = "Produkto tipas";
            // 
            // DepositControlPanel
            // 
            this.DepositControlPanel.Controls.Add(this.DepositYearComboBox);
            this.DepositControlPanel.Controls.Add(this.DepositYearInfoLabel);
            this.DepositControlPanel.Controls.Add(this.GetDepositInfoByNameButton);
            this.DepositControlPanel.Controls.Add(this.GetAllInfoDeposit);
            this.DepositControlPanel.Controls.Add(this.DepositProductNameListComboBox);
            this.DepositControlPanel.Controls.Add(this.DepositProductnameInformationLabel);
            this.DepositControlPanel.Controls.Add(this.DepositInformationLabel);
            this.DepositControlPanel.Location = new System.Drawing.Point(681, 339);
            this.DepositControlPanel.Name = "DepositControlPanel";
            this.DepositControlPanel.Size = new System.Drawing.Size(437, 253);
            this.DepositControlPanel.TabIndex = 46;
            // 
            // DepositYearComboBox
            // 
            this.DepositYearComboBox.FormattingEnabled = true;
            this.DepositYearComboBox.Location = new System.Drawing.Point(300, 151);
            this.DepositYearComboBox.Name = "DepositYearComboBox";
            this.DepositYearComboBox.Size = new System.Drawing.Size(79, 21);
            this.DepositYearComboBox.TabIndex = 50;
            this.DepositYearComboBox.TabStop = false;
            // 
            // DepositYearInfoLabel
            // 
            this.DepositYearInfoLabel.AutoSize = true;
            this.DepositYearInfoLabel.Location = new System.Drawing.Point(261, 154);
            this.DepositYearInfoLabel.Name = "DepositYearInfoLabel";
            this.DepositYearInfoLabel.Size = new System.Drawing.Size(33, 13);
            this.DepositYearInfoLabel.TabIndex = 50;
            this.DepositYearInfoLabel.Text = "Metai";
            // 
            // GetDepositInfoByNameButton
            // 
            this.GetDepositInfoByNameButton.Location = new System.Drawing.Point(138, 70);
            this.GetDepositInfoByNameButton.Name = "GetDepositInfoByNameButton";
            this.GetDepositInfoByNameButton.Size = new System.Drawing.Size(109, 47);
            this.GetDepositInfoByNameButton.TabIndex = 49;
            this.GetDepositInfoByNameButton.TabStop = false;
            this.GetDepositInfoByNameButton.Text = "Gauti Depozito info\r\npagal pavadinimą";
            this.GetDepositInfoByNameButton.UseVisualStyleBackColor = true;
            // 
            // GetAllInfoDeposit
            // 
            this.GetAllInfoDeposit.Location = new System.Drawing.Point(37, 70);
            this.GetAllInfoDeposit.Name = "GetAllInfoDeposit";
            this.GetAllInfoDeposit.Size = new System.Drawing.Size(95, 47);
            this.GetAllInfoDeposit.TabIndex = 47;
            this.GetAllInfoDeposit.TabStop = false;
            this.GetAllInfoDeposit.Text = "Gauti Depozito \r\nPilną Info";
            this.GetAllInfoDeposit.UseVisualStyleBackColor = true;
            // 
            // DepositProductNameListComboBox
            // 
            this.DepositProductNameListComboBox.FormattingEnabled = true;
            this.DepositProductNameListComboBox.Location = new System.Drawing.Point(37, 185);
            this.DepositProductNameListComboBox.Name = "DepositProductNameListComboBox";
            this.DepositProductNameListComboBox.Size = new System.Drawing.Size(257, 21);
            this.DepositProductNameListComboBox.TabIndex = 48;
            this.DepositProductNameListComboBox.TabStop = false;
            // 
            // DepositProductnameInformationLabel
            // 
            this.DepositProductnameInformationLabel.AutoSize = true;
            this.DepositProductnameInformationLabel.Location = new System.Drawing.Point(34, 154);
            this.DepositProductnameInformationLabel.Name = "DepositProductnameInformationLabel";
            this.DepositProductnameInformationLabel.Size = new System.Drawing.Size(75, 13);
            this.DepositProductnameInformationLabel.TabIndex = 47;
            this.DepositProductnameInformationLabel.Text = "Produkto tipas";
            // 
            // DepositInformationLabel
            // 
            this.DepositInformationLabel.AutoSize = true;
            this.DepositInformationLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DepositInformationLabel.Location = new System.Drawing.Point(54, 28);
            this.DepositInformationLabel.Name = "DepositInformationLabel";
            this.DepositInformationLabel.Size = new System.Drawing.Size(159, 20);
            this.DepositInformationLabel.TabIndex = 47;
            this.DepositInformationLabel.Text = "Depozito valdymas";
            // 
            // StorageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1130, 742);
            this.Controls.Add(this.DepositControlPanel);
            this.Controls.Add(this.ProductTypeControlPanel);
            this.Controls.Add(this.StorageDataGridView);
            this.Controls.Add(this.InformationOfDataGridViewTypeLabel);
            this.MaximizeBox = false;
            this.Name = "StorageForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sandėlio valdymo Forma";
            this.Load += new System.EventHandler(this.StorageForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.StorageDataGridView)).EndInit();
            this.ProductTypeControlPanel.ResumeLayout(false);
            this.ProductTypeControlPanel.PerformLayout();
            this.DepositControlPanel.ResumeLayout(false);
            this.DepositControlPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label InformationOfDataGridViewTypeLabel;
        private System.Windows.Forms.DataGridView StorageDataGridView;
        private System.Windows.Forms.Label ProductTypeInformationLabel;
        private System.Windows.Forms.Panel ProductTypeControlPanel;
        private System.Windows.Forms.Label ProductTypeInfoLabel;
        private System.Windows.Forms.ComboBox ProductTypeSpecificNameComboBox;
        private System.Windows.Forms.Button GetAllInfoByProductNameButton;
        private System.Windows.Forms.Button GetAllInfoByYearButton;
        private System.Windows.Forms.ComboBox ProductTypeYearComboBox;
        private System.Windows.Forms.Label ProductTypeYearInfoLabel;
        private System.Windows.Forms.Panel DepositControlPanel;
        private System.Windows.Forms.Label DepositInformationLabel;
        private System.Windows.Forms.Label DepositProductnameInformationLabel;
        private System.Windows.Forms.ComboBox DepositProductNameListComboBox;
        private System.Windows.Forms.Button GetAllInfoDeposit;
        private System.Windows.Forms.Button GetDepositInfoByNameButton;
        private System.Windows.Forms.ComboBox DepositYearComboBox;
        private System.Windows.Forms.Label DepositYearInfoLabel;
    }
}