using System.Windows.Forms;
using Invoice.Models;

namespace Invoice.Forms
{
    public partial class MoneyReceiptForm : Form
    {
        public MoneyReceiptForm()
        {
            InitializeComponent();
        }

        public void FillMoneyReceiptForm(MoneyReceiptModel moneyReceiptInfo)
        {
            SellerInfoLabel.Text = moneyReceiptInfo.SellerInfo;
            SellerFirmCodeLabel.Text = moneyReceiptInfo.SellerFirmCode;
            SerialNumberLabel.Text = moneyReceiptInfo.SerialNumber;
            InvoiceNumberLabel.Text = moneyReceiptInfo.InvoiceNumber;
            InvoiceDateLabel.Text = moneyReceiptInfo.InvoiceDate;
            AllProductsLabel.Text = moneyReceiptInfo.AllProducts;
            PriceInWordsLabel.Text = moneyReceiptInfo.PriceInWords;
            InvoiceMakerLabel.Text = moneyReceiptInfo.InvoiceMaker;
        }
    }
}
