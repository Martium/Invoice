using System.Drawing;
using System.Windows.Forms;
using Invoice.Models;

namespace Invoice.Forms
{
    public partial class MoneyReceiptForm : Form
    {
        private Bitmap _moneyReceiptMemoryImage;
        private const int PositionPanelPointXNumber = 31;

        public MoneyReceiptForm(int emptyLines)
        {
            //need logic for position panel by filled products 
            InitializeComponent();
            SetPositionPanelPlaceByEmptyLines(emptyLines);
        }

        public Bitmap SaveMoneyReceiptForm(MoneyReceiptModel moneyReceiptInfo)
        {
            SellerInfoLabel.Text = moneyReceiptInfo.SellerInfo;
            SellerFirmCodeLabel.Text = moneyReceiptInfo.SellerFirmCode;
            SerialNumberLabel.Text = moneyReceiptInfo.SerialNumber;
            MoneyReceiptNumberLabel.Text = moneyReceiptInfo.MoneyReceiptOfferNumber; // new logic need
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

        private void SetPositionPanelPlaceByEmptyLines(int emptyLines)
        {
            switch (emptyLines)
            {
                case 0:
                    SetPositionPanel(680);
                    break;
                case 1:
                    SetPositionPanel(640);
                    break;
                case 2:
                    SetPositionPanel(600);
                    break;
                case 3:
                    SetPositionPanel(560);
                    break;
                case 4:
                    SetPositionPanel(520);
                    break;
                case 5:
                    SetPositionPanel(480);
                    break;
                case 6:
                    SetPositionPanel(440);
                    break;
                case 7:
                    SetPositionPanel(400);
                    break;
                case 8:
                    SetPositionPanel(360);
                    break;
                case 9:
                    SetPositionPanel(320);
                    break;
                case 10:
                    SetPositionPanel(280);
                    break;
                case 11:
                    SetPositionPanel(240);
                    break;
                case 12:
                    SetPositionPanel(210);
                    break;
            }
        }

        private void SetPositionPanel(int pointY)
        {
            PositionPanel.Location = new Point(PositionPanelPointXNumber, pointY);
        }
    }
}
