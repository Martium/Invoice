using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Invoice.Models.BuyersInfo;
using Invoice.Repositories;
using Invoice.Service;

namespace Invoice.Forms
{
    public partial class BuyersInfoForm : Form
    {
        private readonly BuyersInfoRepository _buyersInfoRepository;

        private readonly MessageDialogService _messageDialogService;

        public BuyersInfoForm()
        {
            _buyersInfoRepository = new BuyersInfoRepository();
            _messageDialogService = new MessageDialogService();

            InitializeComponent();

            SetControlsInitialState();

            FillExistsBuyerListComboBox();
        }

        private void LoadBuyerBySelectedNameButton_Click(object sender, System.EventArgs e)
        {
            LoadInfoToRichTextBoxes();
        }

        #region Helpers

        private void SetControlsInitialState()
        {
            this.StartPosition = FormStartPosition.CenterScreen;

            ExistsBuyerListComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void FillExistsBuyerListComboBox()
        {
            ExistsBuyerListComboBox.Items.Clear();

            List<BuyersNamesModel> allBuyersNames = _buyersInfoRepository.GetExistsBuyersNames().ToList();

            ExistsBuyerListComboBox.DataSource = allBuyersNames;

            ExistsBuyerListComboBox.DisplayMember = "BuyerName";
        }

        private void LoadInfoToRichTextBoxes()
        {
            BuyerFullInfoModel buyerFullInfo = _buyersInfoRepository.BuyerFullInfo(ExistsBuyerListComboBox.Text);

            if (buyerFullInfo != null)
            {
                BuyerNameRichTextBox.Text = buyerFullInfo.BuyerName;
                BuyerFirmCodeRichTextBox.Text = buyerFullInfo.BuyerFirmCode;
                BuyerPvmCodeRichTextBox.Text = buyerFullInfo.BuyerPvmCode;
                BuyerAddressRichTextBox.Text = buyerFullInfo.BuyerAddress;
            }
            else
            {
                _messageDialogService.ShowErrorMassage("Nėra informacijos kurią būtų galima sukelti supildykite bent vieno pirkėjo informaciją ");
            }
        }

        #endregion

        
    }
}
