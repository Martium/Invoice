using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using Invoice.Models;

namespace Invoice.Forms
{
    public partial class MoneyReceiptForm : Form
    {
        private Bitmap _moneyReceiptMemoryImage;

        public MoneyReceiptForm()
        {
            InitializeComponent();
        }

        public Bitmap SaveMoneyReceiptForm(MoneyReceiptModel moneyReceiptInfo)
        {
            SellerInfoLabel.Text = moneyReceiptInfo.SellerInfo;
            SellerFirmCodeLabel.Text = moneyReceiptInfo.SellerFirmCode;
            SerialNumberLabel.Text = moneyReceiptInfo.SerialNumber;
            InvoiceNumberLabel.Text = moneyReceiptInfo.InvoiceNumber;
            InvoiceDateLabel.Text = moneyReceiptInfo.InvoiceDate;
            AllProductsLabel.Text = moneyReceiptInfo.AllProducts;
            PriceInWordsLabel.Text = moneyReceiptInfo.PriceInWords;
            InvoiceMakerLabel.Text = moneyReceiptInfo.InvoiceMaker;

            CaptureMoneyReceiptFormScreen();

            return _moneyReceiptMemoryImage;
        }

        private void CaptureMoneyReceiptFormScreen()
        {
            _moneyReceiptMemoryImage = new Bitmap(PrintMoneyReceiptPanel.Width, PrintMoneyReceiptPanel.Height);

            PrintMoneyReceiptPanel.DrawToBitmap(
                _moneyReceiptMemoryImage,
                new Rectangle(0, 0, PrintMoneyReceiptPanel.Width, PrintMoneyReceiptPanel.Height));
        }
    }
}
