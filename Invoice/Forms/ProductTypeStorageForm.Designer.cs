
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
            this.ProductTypeOrStorageDataGridView = new System.Windows.Forms.DataGridView();
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
            this.GetAllInfoStorage = new System.Windows.Forms.Button();
            this.ProductTypeInformationLabel = new System.Windows.Forms.Label();
            this.StorageInformationLabel = new System.Windows.Forms.Label();
            this.SerialInfoLabel = new System.Windows.Forms.Label();
            this.StorageProductNameInfoLabel = new System.Windows.Forms.Label();
            this.MadeDateInfoLabel = new System.Windows.Forms.Label();
            this.ExpireDateInfoLabel = new System.Windows.Forms.Label();
            this.StorageQuantityIInfoLabel = new System.Windows.Forms.Label();
            this.StoragePriceInfoLabel = new System.Windows.Forms.Label();
            this.StorageSerialNumberTextBox = new System.Windows.Forms.TextBox();
            this.StorageProductNameTextBox = new System.Windows.Forms.TextBox();
            this.StorageProductMadeDateTextBox = new System.Windows.Forms.TextBox();
            this.StorageProductExpireDateTextBox = new System.Windows.Forms.TextBox();
            this.StorageProductQuantityTextBox = new System.Windows.Forms.TextBox();
            this.StorageProductPriceTextBox = new System.Windows.Forms.TextBox();
            this.StorageProductNameListComboBox = new System.Windows.Forms.ComboBox();
            this.StorageProductnameInformationLabel = new System.Windows.Forms.Label();
            this.NewOrUpdateStorageInfoLabel = new System.Windows.Forms.Label();
            this.GetStorageInfoByNameButton = new System.Windows.Forms.Button();
            this.CreateNewStorageButton = new System.Windows.Forms.Button();
            this.ErrorLabel = new System.Windows.Forms.Label();
            this.StoreInfoInStorageTextBoxButton = new System.Windows.Forms.Button();
            this.UpdateStorageButton = new System.Windows.Forms.Button();
            this.AddStorageQuantityTextBox = new System.Windows.Forms.TextBox();
            this.AddStorageQuantityButton = new System.Windows.Forms.Button();
            this.AddQuantityInfoLabel = new System.Windows.Forms.Label();
            this.DeleteStorageButton = new System.Windows.Forms.Button();
            this.InformationOfDataGridViewTypeLabel = new System.Windows.Forms.Label();
            this.StorageProductMonthsLeftTextBox = new System.Windows.Forms.TextBox();
            this.StorageMonthsLeftInfoLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ProductTypeOrStorageDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // ProductTypeOrStorageDataGridView
            // 
            this.ProductTypeOrStorageDataGridView.AllowUserToAddRows = false;
            this.ProductTypeOrStorageDataGridView.AllowUserToDeleteRows = false;
            this.ProductTypeOrStorageDataGridView.AllowUserToResizeColumns = false;
            this.ProductTypeOrStorageDataGridView.AllowUserToResizeRows = false;
            this.ProductTypeOrStorageDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ProductTypeOrStorageDataGridView.Location = new System.Drawing.Point(40, 88);
            this.ProductTypeOrStorageDataGridView.MultiSelect = false;
            this.ProductTypeOrStorageDataGridView.Name = "ProductTypeOrStorageDataGridView";
            this.ProductTypeOrStorageDataGridView.ReadOnly = true;
            this.ProductTypeOrStorageDataGridView.Size = new System.Drawing.Size(557, 563);
            this.ProductTypeOrStorageDataGridView.TabIndex = 0;
            this.ProductTypeOrStorageDataGridView.TabStop = false;
            this.ProductTypeOrStorageDataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ProductTypeOrStorageDataGridView_CellClick);
            this.ProductTypeOrStorageDataGridView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.ProductTypeDataGridView_CellFormatting);
            this.ProductTypeOrStorageDataGridView.Paint += new System.Windows.Forms.PaintEventHandler(this.ProductTypeDataGridView_Paint);
            // 
            // GetAllInfoByYearButton
            // 
            this.GetAllInfoByYearButton.Location = new System.Drawing.Point(857, 88);
            this.GetAllInfoByYearButton.Name = "GetAllInfoByYearButton";
            this.GetAllInfoByYearButton.Size = new System.Drawing.Size(109, 35);
            this.GetAllInfoByYearButton.TabIndex = 3;
            this.GetAllInfoByYearButton.TabStop = false;
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
            this.GetAllInfoByProductNameButton.TabStop = false;
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
            this.ProductTypeYearComboBox.TabStop = false;
            // 
            // ProductTypeSpecificNameComboBox
            // 
            this.ProductTypeSpecificNameComboBox.FormattingEnabled = true;
            this.ProductTypeSpecificNameComboBox.Location = new System.Drawing.Point(603, 88);
            this.ProductTypeSpecificNameComboBox.Name = "ProductTypeSpecificNameComboBox";
            this.ProductTypeSpecificNameComboBox.Size = new System.Drawing.Size(147, 21);
            this.ProductTypeSpecificNameComboBox.TabIndex = 7;
            this.ProductTypeSpecificNameComboBox.TabStop = false;
            // 
            // FullProductTypeQuantityLabel
            // 
            this.FullProductTypeQuantityLabel.AutoSize = true;
            this.FullProductTypeQuantityLabel.Location = new System.Drawing.Point(37, 669);
            this.FullProductTypeQuantityLabel.Name = "FullProductTypeQuantityLabel";
            this.FullProductTypeQuantityLabel.Size = new System.Drawing.Size(143, 13);
            this.FullProductTypeQuantityLabel.TabIndex = 8;
            this.FullProductTypeQuantityLabel.Text = "Visų prodkutų bendras kiekis";
            // 
            // FullProductTypePriceLabel
            // 
            this.FullProductTypePriceLabel.AutoSize = true;
            this.FullProductTypePriceLabel.Location = new System.Drawing.Point(190, 669);
            this.FullProductTypePriceLabel.Name = "FullProductTypePriceLabel";
            this.FullProductTypePriceLabel.Size = new System.Drawing.Size(137, 13);
            this.FullProductTypePriceLabel.TabIndex = 9;
            this.FullProductTypePriceLabel.Text = "Visų prodkutų bendra kaina\r\n";
            // 
            // FullProductTypeQuantityTextBox
            // 
            this.FullProductTypeQuantityTextBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.FullProductTypeQuantityTextBox.Location = new System.Drawing.Point(40, 696);
            this.FullProductTypeQuantityTextBox.Name = "FullProductTypeQuantityTextBox";
            this.FullProductTypeQuantityTextBox.ReadOnly = true;
            this.FullProductTypeQuantityTextBox.Size = new System.Drawing.Size(147, 20);
            this.FullProductTypeQuantityTextBox.TabIndex = 10;
            this.FullProductTypeQuantityTextBox.TabStop = false;
            // 
            // FullProductTypePriceTextBox
            // 
            this.FullProductTypePriceTextBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.FullProductTypePriceTextBox.Location = new System.Drawing.Point(193, 696);
            this.FullProductTypePriceTextBox.Name = "FullProductTypePriceTextBox";
            this.FullProductTypePriceTextBox.ReadOnly = true;
            this.FullProductTypePriceTextBox.Size = new System.Drawing.Size(147, 20);
            this.FullProductTypePriceTextBox.TabIndex = 11;
            this.FullProductTypePriceTextBox.TabStop = false;
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
            // GetAllInfoStorage
            // 
            this.GetAllInfoStorage.Location = new System.Drawing.Point(763, 223);
            this.GetAllInfoStorage.Name = "GetAllInfoStorage";
            this.GetAllInfoStorage.Size = new System.Drawing.Size(88, 47);
            this.GetAllInfoStorage.TabIndex = 14;
            this.GetAllInfoStorage.TabStop = false;
            this.GetAllInfoStorage.Text = "Gauti Sandėlio \r\nPilną Info";
            this.GetAllInfoStorage.UseVisualStyleBackColor = true;
            this.GetAllInfoStorage.Click += new System.EventHandler(this.GetAllInfoStorage_Click);
            // 
            // ProductTypeInformationLabel
            // 
            this.ProductTypeInformationLabel.AutoSize = true;
            this.ProductTypeInformationLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProductTypeInformationLabel.Location = new System.Drawing.Point(637, 31);
            this.ProductTypeInformationLabel.Name = "ProductTypeInformationLabel";
            this.ProductTypeInformationLabel.Size = new System.Drawing.Size(294, 20);
            this.ProductTypeInformationLabel.TabIndex = 15;
            this.ProductTypeInformationLabel.Text = "Produktai pagal Sąskaitas Faktūras";
            // 
            // StorageInformationLabel
            // 
            this.StorageInformationLabel.AutoSize = true;
            this.StorageInformationLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StorageInformationLabel.Location = new System.Drawing.Point(637, 191);
            this.StorageInformationLabel.Name = "StorageInformationLabel";
            this.StorageInformationLabel.Size = new System.Drawing.Size(157, 20);
            this.StorageInformationLabel.TabIndex = 16;
            this.StorageInformationLabel.Text = "Sandėlio valdymas";
            // 
            // SerialInfoLabel
            // 
            this.SerialInfoLabel.AutoSize = true;
            this.SerialInfoLabel.Location = new System.Drawing.Point(657, 348);
            this.SerialInfoLabel.Name = "SerialInfoLabel";
            this.SerialInfoLabel.Size = new System.Drawing.Size(33, 13);
            this.SerialInfoLabel.TabIndex = 17;
            this.SerialInfoLabel.Text = "Serija";
            // 
            // StorageProductNameInfoLabel
            // 
            this.StorageProductNameInfoLabel.AutoSize = true;
            this.StorageProductNameInfoLabel.Location = new System.Drawing.Point(631, 374);
            this.StorageProductNameInfoLabel.Name = "StorageProductNameInfoLabel";
            this.StorageProductNameInfoLabel.Size = new System.Drawing.Size(79, 13);
            this.StorageProductNameInfoLabel.TabIndex = 18;
            this.StorageProductNameInfoLabel.Text = "Produkto Tipas";
            // 
            // MadeDateInfoLabel
            // 
            this.MadeDateInfoLabel.AutoSize = true;
            this.MadeDateInfoLabel.Location = new System.Drawing.Point(631, 400);
            this.MadeDateInfoLabel.Name = "MadeDateInfoLabel";
            this.MadeDateInfoLabel.Size = new System.Drawing.Size(90, 13);
            this.MadeDateInfoLabel.TabIndex = 19;
            this.MadeDateInfoLabel.Text = "Pagaminimo Data";
            // 
            // ExpireDateInfoLabel
            // 
            this.ExpireDateInfoLabel.AutoSize = true;
            this.ExpireDateInfoLabel.Location = new System.Drawing.Point(638, 426);
            this.ExpireDateInfoLabel.Name = "ExpireDateInfoLabel";
            this.ExpireDateInfoLabel.Size = new System.Drawing.Size(75, 13);
            this.ExpireDateInfoLabel.TabIndex = 20;
            this.ExpireDateInfoLabel.Text = "Galiojimo Data";
            // 
            // StorageQuantityIInfoLabel
            // 
            this.StorageQuantityIInfoLabel.AutoSize = true;
            this.StorageQuantityIInfoLabel.Location = new System.Drawing.Point(657, 455);
            this.StorageQuantityIInfoLabel.Name = "StorageQuantityIInfoLabel";
            this.StorageQuantityIInfoLabel.Size = new System.Drawing.Size(35, 13);
            this.StorageQuantityIInfoLabel.TabIndex = 21;
            this.StorageQuantityIInfoLabel.Text = "Kiekis";
            // 
            // StoragePriceInfoLabel
            // 
            this.StoragePriceInfoLabel.AutoSize = true;
            this.StoragePriceInfoLabel.Location = new System.Drawing.Point(657, 484);
            this.StoragePriceInfoLabel.Name = "StoragePriceInfoLabel";
            this.StoragePriceInfoLabel.Size = new System.Drawing.Size(34, 13);
            this.StoragePriceInfoLabel.TabIndex = 22;
            this.StoragePriceInfoLabel.Text = "Kaina";
            // 
            // StorageSerialNumberTextBox
            // 
            this.StorageSerialNumberTextBox.Location = new System.Drawing.Point(727, 345);
            this.StorageSerialNumberTextBox.Name = "StorageSerialNumberTextBox";
            this.StorageSerialNumberTextBox.Size = new System.Drawing.Size(239, 20);
            this.StorageSerialNumberTextBox.TabIndex = 23;
            this.StorageSerialNumberTextBox.TextChanged += new System.EventHandler(this.StorageSerialNumberTextBox_TextChanged);
            this.StorageSerialNumberTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.StorageTextBoxes_KeyUp);
            // 
            // StorageProductNameTextBox
            // 
            this.StorageProductNameTextBox.Location = new System.Drawing.Point(727, 371);
            this.StorageProductNameTextBox.Name = "StorageProductNameTextBox";
            this.StorageProductNameTextBox.Size = new System.Drawing.Size(239, 20);
            this.StorageProductNameTextBox.TabIndex = 24;
            this.StorageProductNameTextBox.TextChanged += new System.EventHandler(this.StorageProductNameTextBox_TextChanged);
            this.StorageProductNameTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.StorageTextBoxes_KeyUp);
            // 
            // StorageProductMadeDateTextBox
            // 
            this.StorageProductMadeDateTextBox.Location = new System.Drawing.Point(727, 397);
            this.StorageProductMadeDateTextBox.Name = "StorageProductMadeDateTextBox";
            this.StorageProductMadeDateTextBox.Size = new System.Drawing.Size(112, 20);
            this.StorageProductMadeDateTextBox.TabIndex = 25;
            this.StorageProductMadeDateTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.StorageTextBoxes_KeyUp);
            this.StorageProductMadeDateTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.StorageProductMadeDateTextBox_Validating);
            // 
            // StorageProductExpireDateTextBox
            // 
            this.StorageProductExpireDateTextBox.Location = new System.Drawing.Point(727, 423);
            this.StorageProductExpireDateTextBox.Name = "StorageProductExpireDateTextBox";
            this.StorageProductExpireDateTextBox.Size = new System.Drawing.Size(112, 20);
            this.StorageProductExpireDateTextBox.TabIndex = 26;
            this.StorageProductExpireDateTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.StorageTextBoxes_KeyUp);
            this.StorageProductExpireDateTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.StorageProductExpireDateTextBox_Validating);
            // 
            // StorageProductQuantityTextBox
            // 
            this.StorageProductQuantityTextBox.Location = new System.Drawing.Point(727, 452);
            this.StorageProductQuantityTextBox.Name = "StorageProductQuantityTextBox";
            this.StorageProductQuantityTextBox.Size = new System.Drawing.Size(112, 20);
            this.StorageProductQuantityTextBox.TabIndex = 27;
            this.StorageProductQuantityTextBox.TextChanged += new System.EventHandler(this.StorageProductQuantityTextBox_TextChanged);
            this.StorageProductQuantityTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.StorageTextBoxes_KeyUp);
            // 
            // StorageProductPriceTextBox
            // 
            this.StorageProductPriceTextBox.Location = new System.Drawing.Point(727, 481);
            this.StorageProductPriceTextBox.Name = "StorageProductPriceTextBox";
            this.StorageProductPriceTextBox.Size = new System.Drawing.Size(112, 20);
            this.StorageProductPriceTextBox.TabIndex = 28;
            this.StorageProductPriceTextBox.TextChanged += new System.EventHandler(this.StorageProductPriceTextBox_TextChanged);
            this.StorageProductPriceTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.StorageTextBoxes_KeyUp);
            // 
            // StorageProductNameListComboBox
            // 
            this.StorageProductNameListComboBox.FormattingEnabled = true;
            this.StorageProductNameListComboBox.Location = new System.Drawing.Point(603, 237);
            this.StorageProductNameListComboBox.Name = "StorageProductNameListComboBox";
            this.StorageProductNameListComboBox.Size = new System.Drawing.Size(147, 21);
            this.StorageProductNameListComboBox.TabIndex = 29;
            this.StorageProductNameListComboBox.TabStop = false;
            // 
            // StorageProductnameInformationLabel
            // 
            this.StorageProductnameInformationLabel.AutoSize = true;
            this.StorageProductnameInformationLabel.Location = new System.Drawing.Point(603, 221);
            this.StorageProductnameInformationLabel.Name = "StorageProductnameInformationLabel";
            this.StorageProductnameInformationLabel.Size = new System.Drawing.Size(75, 13);
            this.StorageProductnameInformationLabel.TabIndex = 30;
            this.StorageProductnameInformationLabel.Text = "Produkto tipas";
            // 
            // NewOrUpdateStorageInfoLabel
            // 
            this.NewOrUpdateStorageInfoLabel.AutoSize = true;
            this.NewOrUpdateStorageInfoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NewOrUpdateStorageInfoLabel.Location = new System.Drawing.Point(638, 307);
            this.NewOrUpdateStorageInfoLabel.Name = "NewOrUpdateStorageInfoLabel";
            this.NewOrUpdateStorageInfoLabel.Size = new System.Drawing.Size(335, 16);
            this.NewOrUpdateStorageInfoLabel.TabIndex = 31;
            this.NewOrUpdateStorageInfoLabel.Text = "Atnaujinti arba sukurti naują produktą sandėlyje";
            // 
            // GetStorageInfoByNameButton
            // 
            this.GetStorageInfoByNameButton.Location = new System.Drawing.Point(857, 223);
            this.GetStorageInfoByNameButton.Name = "GetStorageInfoByNameButton";
            this.GetStorageInfoByNameButton.Size = new System.Drawing.Size(109, 47);
            this.GetStorageInfoByNameButton.TabIndex = 32;
            this.GetStorageInfoByNameButton.TabStop = false;
            this.GetStorageInfoByNameButton.Text = "Gauti Sandėlio info\r\npagal pavadinimą";
            this.GetStorageInfoByNameButton.UseVisualStyleBackColor = true;
            this.GetStorageInfoByNameButton.Click += new System.EventHandler(this.GetStorageInfoByNameButton_Click);
            // 
            // CreateNewStorageButton
            // 
            this.CreateNewStorageButton.Location = new System.Drawing.Point(641, 556);
            this.CreateNewStorageButton.Name = "CreateNewStorageButton";
            this.CreateNewStorageButton.Size = new System.Drawing.Size(128, 37);
            this.CreateNewStorageButton.TabIndex = 33;
            this.CreateNewStorageButton.TabStop = false;
            this.CreateNewStorageButton.Text = "  Sukurt Naują \r\nProduktą Sandėlyje";
            this.CreateNewStorageButton.UseVisualStyleBackColor = true;
            this.CreateNewStorageButton.Click += new System.EventHandler(this.CreateNewStorageButton_Click);
            // 
            // ErrorLabel
            // 
            this.ErrorLabel.AutoSize = true;
            this.ErrorLabel.ForeColor = System.Drawing.Color.Red;
            this.ErrorLabel.Location = new System.Drawing.Point(638, 523);
            this.ErrorLabel.Name = "ErrorLabel";
            this.ErrorLabel.Size = new System.Drawing.Size(57, 13);
            this.ErrorLabel.TabIndex = 34;
            this.ErrorLabel.Text = "error Label";
            this.ErrorLabel.Visible = false;
            // 
            // StoreInfoInStorageTextBoxButton
            // 
            this.StoreInfoInStorageTextBoxButton.Location = new System.Drawing.Point(483, 661);
            this.StoreInfoInStorageTextBoxButton.Name = "StoreInfoInStorageTextBoxButton";
            this.StoreInfoInStorageTextBoxButton.Size = new System.Drawing.Size(114, 29);
            this.StoreInfoInStorageTextBoxButton.TabIndex = 35;
            this.StoreInfoInStorageTextBoxButton.TabStop = false;
            this.StoreInfoInStorageTextBoxButton.Text = "Sukelti informaciją ";
            this.StoreInfoInStorageTextBoxButton.UseVisualStyleBackColor = true;
            this.StoreInfoInStorageTextBoxButton.Click += new System.EventHandler(this.StoreInfoFromDataGridViewButton_Click);
            // 
            // UpdateStorageButton
            // 
            this.UpdateStorageButton.Location = new System.Drawing.Point(775, 556);
            this.UpdateStorageButton.Name = "UpdateStorageButton";
            this.UpdateStorageButton.Size = new System.Drawing.Size(128, 37);
            this.UpdateStorageButton.TabIndex = 36;
            this.UpdateStorageButton.TabStop = false;
            this.UpdateStorageButton.Text = "Atnaujinti Produktą\r\n Sandėlyje";
            this.UpdateStorageButton.UseVisualStyleBackColor = true;
            this.UpdateStorageButton.Click += new System.EventHandler(this.UpdateStorageButton_Click);
            // 
            // AddStorageQuantityTextBox
            // 
            this.AddStorageQuantityTextBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.AddStorageQuantityTextBox.Location = new System.Drawing.Point(641, 630);
            this.AddStorageQuantityTextBox.Name = "AddStorageQuantityTextBox";
            this.AddStorageQuantityTextBox.Size = new System.Drawing.Size(98, 20);
            this.AddStorageQuantityTextBox.TabIndex = 37;
            this.AddStorageQuantityTextBox.TabStop = false;
            this.AddStorageQuantityTextBox.TextChanged += new System.EventHandler(this.AddStorageQuantityTextBox_TextChanged);
            // 
            // AddStorageQuantityButton
            // 
            this.AddStorageQuantityButton.Location = new System.Drawing.Point(775, 630);
            this.AddStorageQuantityButton.Name = "AddStorageQuantityButton";
            this.AddStorageQuantityButton.Size = new System.Drawing.Size(83, 20);
            this.AddStorageQuantityButton.TabIndex = 38;
            this.AddStorageQuantityButton.TabStop = false;
            this.AddStorageQuantityButton.Text = "Pridėti";
            this.AddStorageQuantityButton.UseVisualStyleBackColor = true;
            this.AddStorageQuantityButton.Click += new System.EventHandler(this.AddStorageQuantityButton_Click);
            // 
            // AddQuantityInfoLabel
            // 
            this.AddQuantityInfoLabel.AutoSize = true;
            this.AddQuantityInfoLabel.Location = new System.Drawing.Point(638, 615);
            this.AddQuantityInfoLabel.Name = "AddQuantityInfoLabel";
            this.AddQuantityInfoLabel.Size = new System.Drawing.Size(101, 13);
            this.AddQuantityInfoLabel.TabIndex = 39;
            this.AddQuantityInfoLabel.Text = "Pridėti ar Atimti kiekį";
            // 
            // DeleteStorageButton
            // 
            this.DeleteStorageButton.Location = new System.Drawing.Point(909, 556);
            this.DeleteStorageButton.Name = "DeleteStorageButton";
            this.DeleteStorageButton.Size = new System.Drawing.Size(98, 37);
            this.DeleteStorageButton.TabIndex = 40;
            this.DeleteStorageButton.TabStop = false;
            this.DeleteStorageButton.Text = "Trinti produktą\r\nSandėlyje";
            this.DeleteStorageButton.UseVisualStyleBackColor = true;
            this.DeleteStorageButton.Click += new System.EventHandler(this.DeleteStorageButton_Click);
            // 
            // InformationOfDataGridViewTypeLabel
            // 
            this.InformationOfDataGridViewTypeLabel.AutoSize = true;
            this.InformationOfDataGridViewTypeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InformationOfDataGridViewTypeLabel.Location = new System.Drawing.Point(36, 31);
            this.InformationOfDataGridViewTypeLabel.Name = "InformationOfDataGridViewTypeLabel";
            this.InformationOfDataGridViewTypeLabel.Size = new System.Drawing.Size(92, 20);
            this.InformationOfDataGridViewTypeLabel.TabIndex = 41;
            this.InformationOfDataGridViewTypeLabel.Text = "Data type ";
            // 
            // StorageProductMonthsLeftTextBox
            // 
            this.StorageProductMonthsLeftTextBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.StorageProductMonthsLeftTextBox.Location = new System.Drawing.Point(112, 727);
            this.StorageProductMonthsLeftTextBox.Name = "StorageProductMonthsLeftTextBox";
            this.StorageProductMonthsLeftTextBox.ReadOnly = true;
            this.StorageProductMonthsLeftTextBox.Size = new System.Drawing.Size(75, 20);
            this.StorageProductMonthsLeftTextBox.TabIndex = 42;
            this.StorageProductMonthsLeftTextBox.TabStop = false;
            // 
            // StorageMonthsLeftInfoLabel
            // 
            this.StorageMonthsLeftInfoLabel.AutoSize = true;
            this.StorageMonthsLeftInfoLabel.Location = new System.Drawing.Point(37, 730);
            this.StorageMonthsLeftInfoLabel.Name = "StorageMonthsLeftInfoLabel";
            this.StorageMonthsLeftInfoLabel.Size = new System.Drawing.Size(69, 13);
            this.StorageMonthsLeftInfoLabel.TabIndex = 43;
            this.StorageMonthsLeftInfoLabel.Text = "Liko mėnesių";
            // 
            // ProductTypeStorageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1119, 786);
            this.Controls.Add(this.StorageMonthsLeftInfoLabel);
            this.Controls.Add(this.StorageProductMonthsLeftTextBox);
            this.Controls.Add(this.InformationOfDataGridViewTypeLabel);
            this.Controls.Add(this.DeleteStorageButton);
            this.Controls.Add(this.AddQuantityInfoLabel);
            this.Controls.Add(this.AddStorageQuantityButton);
            this.Controls.Add(this.AddStorageQuantityTextBox);
            this.Controls.Add(this.UpdateStorageButton);
            this.Controls.Add(this.StoreInfoInStorageTextBoxButton);
            this.Controls.Add(this.ErrorLabel);
            this.Controls.Add(this.CreateNewStorageButton);
            this.Controls.Add(this.GetStorageInfoByNameButton);
            this.Controls.Add(this.NewOrUpdateStorageInfoLabel);
            this.Controls.Add(this.StorageProductnameInformationLabel);
            this.Controls.Add(this.StorageProductNameListComboBox);
            this.Controls.Add(this.StorageProductPriceTextBox);
            this.Controls.Add(this.StorageProductQuantityTextBox);
            this.Controls.Add(this.StorageProductExpireDateTextBox);
            this.Controls.Add(this.StorageProductMadeDateTextBox);
            this.Controls.Add(this.StorageProductNameTextBox);
            this.Controls.Add(this.StorageSerialNumberTextBox);
            this.Controls.Add(this.StoragePriceInfoLabel);
            this.Controls.Add(this.StorageQuantityIInfoLabel);
            this.Controls.Add(this.ExpireDateInfoLabel);
            this.Controls.Add(this.MadeDateInfoLabel);
            this.Controls.Add(this.StorageProductNameInfoLabel);
            this.Controls.Add(this.SerialInfoLabel);
            this.Controls.Add(this.StorageInformationLabel);
            this.Controls.Add(this.ProductTypeInformationLabel);
            this.Controls.Add(this.GetAllInfoStorage);
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
            this.Controls.Add(this.ProductTypeOrStorageDataGridView);
            this.MaximizeBox = false;
            this.Name = "ProductTypeStorageForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Produktų Sandėlys";
            this.Load += new System.EventHandler(this.ProductTypeStorageForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ProductTypeOrStorageDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView ProductTypeOrStorageDataGridView;
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
        private System.Windows.Forms.Button GetAllInfoStorage;
        private System.Windows.Forms.Label ProductTypeInformationLabel;
        private System.Windows.Forms.Label StorageInformationLabel;
        private System.Windows.Forms.Label SerialInfoLabel;
        private System.Windows.Forms.Label StorageProductNameInfoLabel;
        private System.Windows.Forms.Label MadeDateInfoLabel;
        private System.Windows.Forms.Label ExpireDateInfoLabel;
        private System.Windows.Forms.Label StorageQuantityIInfoLabel;
        private System.Windows.Forms.Label StoragePriceInfoLabel;
        private System.Windows.Forms.TextBox StorageSerialNumberTextBox;
        private System.Windows.Forms.TextBox StorageProductNameTextBox;
        private System.Windows.Forms.TextBox StorageProductMadeDateTextBox;
        private System.Windows.Forms.TextBox StorageProductExpireDateTextBox;
        private System.Windows.Forms.TextBox StorageProductQuantityTextBox;
        private System.Windows.Forms.TextBox StorageProductPriceTextBox;
        private System.Windows.Forms.ComboBox StorageProductNameListComboBox;
        private System.Windows.Forms.Label StorageProductnameInformationLabel;
        private System.Windows.Forms.Label NewOrUpdateStorageInfoLabel;
        private System.Windows.Forms.Button GetStorageInfoByNameButton;
        private System.Windows.Forms.Button CreateNewStorageButton;
        private System.Windows.Forms.Label ErrorLabel;
        private System.Windows.Forms.Button StoreInfoInStorageTextBoxButton;
        private System.Windows.Forms.Button UpdateStorageButton;
        private System.Windows.Forms.TextBox AddStorageQuantityTextBox;
        private System.Windows.Forms.Button AddStorageQuantityButton;
        private System.Windows.Forms.Label AddQuantityInfoLabel;
        private System.Windows.Forms.Button DeleteStorageButton;
        private System.Windows.Forms.Label InformationOfDataGridViewTypeLabel;
        private System.Windows.Forms.TextBox StorageProductMonthsLeftTextBox;
        private System.Windows.Forms.Label StorageMonthsLeftInfoLabel;
    }
}