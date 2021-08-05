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

        private void AddNewBuyerButton_Click(object sender, System.EventArgs e)
        {

            bool isBuyerNameFilled = !string.IsNullOrWhiteSpace(BuyerNameRichTextBox.Text);

            bool isAllInfoFilled = CheckAllBuyerInfoIsFilled();

            bool isBuyerExists = _buyersInfoRepository.CheckIsBuyerExists(BuyerNameRichTextBox.Text);



            if (!isBuyerExists && isBuyerNameFilled && isAllInfoFilled)
            {
               CreateNewBuyerInfo();
               FillExistsBuyerListComboBox();

            }
            else if (!isBuyerExists && isBuyerNameFilled && !isAllInfoFilled)
            {
                DialogResult dialogResult = _messageDialogService.ShowChoiceMessage(
                    "Kai kurie langeliai nesupildyti ar norite išsaugoti nepilną informaciją apie pirkėją ?");

                if (dialogResult == DialogResult.OK)
                {
                    CreateNewBuyerInfo();
                    FillExistsBuyerListComboBox();
                }
            }
            else if (!isBuyerExists && !isBuyerNameFilled)
            {
                _messageDialogService.ShowErrorMassage("Kad išsaugotumėte naują pirkėją būtina supildyti pirkėjo pavadinimas langelį");
            }
            else
            {
                _messageDialogService.ShowErrorMassage("Toks pirkėjas egzistuoja jei norite pakeisti jo informaciją spauskite atnaujinti pirkėjo informaciją mygtuką");
            }

        }

        #region Helpers

        private void SetControlsInitialState()
        {
            this.StartPosition = FormStartPosition.CenterScreen;

            ExistsBuyerListComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void FillExistsBuyerListComboBox()
        {
            ExistsBuyerListComboBox.DataSource = null;

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

        private void CreateNewBuyerInfo()
        {
            BuyerFullInfoModel newBuyer = new BuyerFullInfoModel
            {
                BuyerName = BuyerNameRichTextBox.Text,
                BuyerFirmCode = BuyerFirmCodeRichTextBox.Text,
                BuyerPvmCode = BuyerAddressRichTextBox.Text,
                BuyerAddress = BuyerAddressRichTextBox.Text
            };

            bool isBuyerCreated = _buyersInfoRepository.CreateNewBuyerInfo(newBuyer);

            if (isBuyerCreated)
            {
                _messageDialogService.ShowInfoMessage("Naujas pirkėjas pridėtas į duomenų bazę");
            }
            else
            {
                _messageDialogService.ShowErrorMassage("kažkas nepavyko kreiptis į administratorių ar bandyti dar kartą");
            }

        }

        private bool CheckAllBuyerInfoIsFilled()
        {
            bool isAllBuyerInfoFilled = !string.IsNullOrWhiteSpace(BuyerFirmCodeRichTextBox.Text) &&
                                        !string.IsNullOrWhiteSpace(BuyerFirmCodeRichTextBox.Text) &&
                                        !string.IsNullOrWhiteSpace(BuyerAddressRichTextBox.Text);

            return isAllBuyerInfoFilled;
        }

        #endregion

        
    }
}
