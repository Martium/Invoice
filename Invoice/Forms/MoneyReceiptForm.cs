using System.Drawing;
using System.Windows.Forms;
using Invoice.Models;

namespace Invoice.Forms
{
    public partial class MoneyReceiptForm : Form
    {
        private Bitmap _MoneyReceiptMemoryImage;
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

            return _MoneyReceiptMemoryImage;
        }

        private void CaptureMoneyReceiptFormScreen()
        {
            _MoneyReceiptMemoryImage = new Bitmap(PrintMoneyReceiptPanel.Width, PrintMoneyReceiptPanel.Height);

            PrintMoneyReceiptPanel.DrawToBitmap(
                _MoneyReceiptMemoryImage,
                new Rectangle(0, 0, PrintMoneyReceiptPanel.Width, PrintMoneyReceiptPanel.Height));
        }
    }
}
