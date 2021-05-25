
namespace Invoice.Forms
{
    partial class ProductTypeForm
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
            this.FirstProductTypeTextBox = new System.Windows.Forms.TextBox();
            this.SecondProductTypeTextBox = new System.Windows.Forms.TextBox();
            this.ThirdProductTypeTextBox = new System.Windows.Forms.TextBox();
            this.FourthProductTypeTextBox = new System.Windows.Forms.TextBox();
            this.FifthProductTypeTextBox = new System.Windows.Forms.TextBox();
            this.ProductTypeNameLabel = new System.Windows.Forms.Label();
            this.SaveButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // FirstProductTypeTextBox
            // 
            this.FirstProductTypeTextBox.Location = new System.Drawing.Point(72, 83);
            this.FirstProductTypeTextBox.Name = "FirstProductTypeTextBox";
            this.FirstProductTypeTextBox.Size = new System.Drawing.Size(161, 20);
            this.FirstProductTypeTextBox.TabIndex = 0;
            // 
            // SecondProductTypeTextBox
            // 
            this.SecondProductTypeTextBox.Location = new System.Drawing.Point(72, 120);
            this.SecondProductTypeTextBox.Name = "SecondProductTypeTextBox";
            this.SecondProductTypeTextBox.Size = new System.Drawing.Size(161, 20);
            this.SecondProductTypeTextBox.TabIndex = 1;
            // 
            // ThirdProductTypeTextBox
            // 
            this.ThirdProductTypeTextBox.Location = new System.Drawing.Point(72, 158);
            this.ThirdProductTypeTextBox.Name = "ThirdProductTypeTextBox";
            this.ThirdProductTypeTextBox.Size = new System.Drawing.Size(161, 20);
            this.ThirdProductTypeTextBox.TabIndex = 2;
            // 
            // FourthProductTypeTextBox
            // 
            this.FourthProductTypeTextBox.Location = new System.Drawing.Point(72, 194);
            this.FourthProductTypeTextBox.Name = "FourthProductTypeTextBox";
            this.FourthProductTypeTextBox.Size = new System.Drawing.Size(161, 20);
            this.FourthProductTypeTextBox.TabIndex = 3;
            // 
            // FifthProductTypeTextBox
            // 
            this.FifthProductTypeTextBox.Location = new System.Drawing.Point(72, 231);
            this.FifthProductTypeTextBox.Name = "FifthProductTypeTextBox";
            this.FifthProductTypeTextBox.Size = new System.Drawing.Size(161, 20);
            this.FifthProductTypeTextBox.TabIndex = 4;
            // 
            // ProductTypeNameLabel
            // 
            this.ProductTypeNameLabel.AutoSize = true;
            this.ProductTypeNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProductTypeNameLabel.Location = new System.Drawing.Point(67, 43);
            this.ProductTypeNameLabel.Name = "ProductTypeNameLabel";
            this.ProductTypeNameLabel.Size = new System.Drawing.Size(158, 25);
            this.ProductTypeNameLabel.TabIndex = 5;
            this.ProductTypeNameLabel.Text = "Produktų tipai";
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(72, 257);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(93, 24);
            this.SaveButton.TabIndex = 6;
            this.SaveButton.Text = "Išsaugoti";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // ProductTypeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1064, 1001);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.ProductTypeNameLabel);
            this.Controls.Add(this.FifthProductTypeTextBox);
            this.Controls.Add(this.FourthProductTypeTextBox);
            this.Controls.Add(this.ThirdProductTypeTextBox);
            this.Controls.Add(this.SecondProductTypeTextBox);
            this.Controls.Add(this.FirstProductTypeTextBox);
            this.MaximizeBox = false;
            this.Name = "ProductTypeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Produktų tipai";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox FirstProductTypeTextBox;
        private System.Windows.Forms.TextBox SecondProductTypeTextBox;
        private System.Windows.Forms.TextBox ThirdProductTypeTextBox;
        private System.Windows.Forms.TextBox FourthProductTypeTextBox;
        private System.Windows.Forms.TextBox FifthProductTypeTextBox;
        private System.Windows.Forms.Label ProductTypeNameLabel;
        private System.Windows.Forms.Button SaveButton;
    }
}