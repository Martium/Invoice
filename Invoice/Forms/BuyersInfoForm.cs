﻿using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Invoice.Constants;
using Invoice.Models.BuyersInfo;
using Invoice.Repositories;
using Invoice.Service;

namespace Invoice.Forms
{
    public partial class BuyersInfoForm : Form
    {
        private readonly BuyersInfoRepository _buyersInfoRepository;

        private readonly MessageDialogService _messageDialogService;

        private string[] _lastBuyerInfo = new string[4];

        public BuyersInfoForm()
        {
            _buyersInfoRepository = new BuyersInfoRepository();
            _messageDialogService = new MessageDialogService();

            InitializeComponent();

            SetControlsInitialState();

            SetRichTextBoxMaxLength();

            FillExistsBuyerListComboBox();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Escape))
            {
                this.Close();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void RichBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
                SetCursorsAtRichTextBoxStringEnd();
            }
        }

        private void RichTextBox_TextChanged(object sender, System.EventArgs e)
        {
            RichTextBox richTextBox = sender as RichTextBox;

            if (richTextBox == null) return;

            if (richTextBox.SelectionStart == richTextBox.MaxLength)
            {
                _messageDialogService.ShowInfoMessage($"Pasiektas maksimalus žodžių ilgis bus išsaugota tik toks tekstas ({richTextBox.Text}) ");
            }
        }

        private void LoadBuyerBySelectedNameButton_Click(object sender, System.EventArgs e)
        {
            LoadInfoToRichTextBoxes();

            BuyerFirmCodeRichTextBox.Focus();
            BuyerFirmCodeRichTextBox.SelectionStart = BuyerFirmCodeRichTextBox.Text.Length;
        }

        private void AddNewBuyerButton_Click(object sender, System.EventArgs e)
        {
            bool isBuyerNameFilled = !string.IsNullOrWhiteSpace(BuyerNameRichTextBox.Text);

            bool isBuyerExists = _buyersInfoRepository.CheckIsBuyerExists(BuyerNameRichTextBox.Text);

            bool isAllInfoFilled = CheckAllBuyerInfoIsFilled();


            if (!isBuyerExists && isBuyerNameFilled && isAllInfoFilled)
            {
               CreateNewBuyerInfo();
               FillExistsBuyerListComboBox();
            }
            else if (!isBuyerExists && isBuyerNameFilled)
            {
                DialogResult dialogResult = _messageDialogService.ShowChoiceMessage(
                    "Kai kurie langeliai nesupildyti ar norite išsaugoti nepilną informaciją apie pirkėją ?");

                if (dialogResult == DialogResult.OK)
                {
                    CreateNewBuyerInfo();
                    FillExistsBuyerListComboBox();
                }
            }
            else if (!isBuyerExists)
            {
                _messageDialogService.ShowErrorMassage("Kad išsaugotumėte naują pirkėją būtina supildyti pirkėjo pavadinimas langelį");

            }
            else
            {
                _messageDialogService.ShowErrorMassage("Toks pirkėjas egzistuoja jei norite pakeisti jo informaciją spauskite atnaujinti pirkėjo informaciją mygtuką");
            }

            SetCursorAtBuyerNameStringEnd();
        }

        private void UpdateBuyerButton_Click(object sender, System.EventArgs e)
        {
            bool isBuyerNameFilled = !string.IsNullOrWhiteSpace(BuyerNameRichTextBox.Text);

            bool isBuyerExists = _buyersInfoRepository.CheckIsBuyerExists(BuyerNameRichTextBox.Text);

            bool isAllValuesSame = CheckIsBuyerAllValuesSameAsInDataBase();

            if (isBuyerExists && isBuyerNameFilled && !isAllValuesSame)
            {
                UpdateBuyerInfo();
            }
            else if (isBuyerExists && isBuyerNameFilled)
            {
                _messageDialogService.ShowErrorMassage("Jūs nieko nepakeitėte todėl nebus atnaujinta informacija liks tokia pati ");
            }
            else if (!isBuyerNameFilled)
            {
                _messageDialogService.ShowErrorMassage("Kad atnaujintumėte pirkėją būtina supildyti pirkėjo pavadinimas langelį");
            }
            else
            {
                _messageDialogService.ShowErrorMassage("Pirkėjas nerastas atnaujinimas negalimas");
            }

            SetCursorAtBuyerNameStringEnd();
        }

        #region Helpers

        private void SetRichTextBoxMaxLength()
        {
            BuyerNameRichTextBox.MaxLength = FormSettings.TextBoxLengths.BuyerName;
            BuyerFirmCodeRichTextBox.MaxLength = FormSettings.TextBoxLengths.BuyerFirmCode;
            BuyerPvmCodeRichTextBox.MaxLength = FormSettings.TextBoxLengths.BuyerPvmCode;
            BuyerAddressRichTextBox.MaxLength = FormSettings.TextBoxLengths.BuyerAddress;
        }

        private void SetCursorsAtRichTextBoxStringEnd()
        {
            BuyerNameRichTextBox.SelectionStart = BuyerNameRichTextBox.Text.Length;
            BuyerFirmCodeRichTextBox.SelectionStart = BuyerFirmCodeRichTextBox.Text.Length;
            BuyerPvmCodeRichTextBox.SelectionStart = BuyerPvmCodeRichTextBox.Text.Length;
            BuyerAddressRichTextBox.SelectionStart = BuyerAddressRichTextBox.Text.Length;
        }

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
            BuyerFullInfoModel buyerFullInfo = _buyersInfoRepository.GetBuyerFullInfo(ExistsBuyerListComboBox.Text);

            if (buyerFullInfo != null)
            {
                BuyerNameRichTextBox.Text = buyerFullInfo.BuyerName;
                BuyerFirmCodeRichTextBox.Text = buyerFullInfo.BuyerFirmCode;
                BuyerPvmCodeRichTextBox.Text = buyerFullInfo.BuyerPvmCode;
                BuyerAddressRichTextBox.Text = buyerFullInfo.BuyerAddress;

                _lastBuyerInfo[0] = BuyerNameRichTextBox.Text;
                _lastBuyerInfo[1] = BuyerFirmCodeRichTextBox.Text;
                _lastBuyerInfo[2] = BuyerPvmCodeRichTextBox.Text;
                _lastBuyerInfo[3] = BuyerAddressRichTextBox.Text;

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
                BuyerPvmCode = BuyerPvmCodeRichTextBox.Text,
                BuyerAddress = BuyerAddressRichTextBox.Text
            };

            bool isBuyerCreated = _buyersInfoRepository.CreateNewBuyerInfo(newBuyer);

            if (isBuyerCreated)
            {
                _messageDialogService.ShowInfoMessage("Naujas pirkėjas pridėtas į duomenų bazę");
            }
            else
            {
                _messageDialogService.ShowErrorMassage("kažkas nepavyko kreiptis į administratorių ar bandykit dar kartą");
            }

        }

        private bool CheckAllBuyerInfoIsFilled()
        {
            bool isAllBuyerInfoFilled = !string.IsNullOrWhiteSpace(BuyerFirmCodeRichTextBox.Text) &&
                                        !string.IsNullOrWhiteSpace(BuyerPvmCodeRichTextBox.Text) &&
                                        !string.IsNullOrWhiteSpace(BuyerAddressRichTextBox.Text);

            return isAllBuyerInfoFilled;
        }

        private void UpdateBuyerInfo()
        {
            BuyerFullInfoModel updateBuyer = new BuyerFullInfoModel
            {
                BuyerName = BuyerNameRichTextBox.Text,
                BuyerFirmCode = BuyerFirmCodeRichTextBox.Text,
                BuyerPvmCode = BuyerPvmCodeRichTextBox.Text,
                BuyerAddress = BuyerAddressRichTextBox.Text
            };

            bool isBuyerUpdated = _buyersInfoRepository.UpdateBuyerInfo(updateBuyer);

            if (isBuyerUpdated)
            {
                _messageDialogService.ShowInfoMessage(" Pirkėjas atnaujintas sekmingai");
            }
            else
            {
                _messageDialogService.ShowErrorMassage("kažkas nepavyko kreiptis į administratorių ar bandykit dar kartą");
            }

        }

        private bool CheckIsBuyerAllValuesSameAsInDataBase()
        {
            bool isAllValuesSame = _lastBuyerInfo[0] == BuyerNameRichTextBox.Text &&
                                   _lastBuyerInfo[1] == BuyerFirmCodeRichTextBox.Text &&
                                   _lastBuyerInfo[2] == BuyerPvmCodeRichTextBox.Text &&
                                   _lastBuyerInfo[3] == BuyerAddressRichTextBox.Text;

            return isAllValuesSame;
        }

        private void SetCursorAtBuyerNameStringEnd()
        {
            BuyerNameRichTextBox.Focus();
            BuyerNameRichTextBox.SelectionStart = BuyerNameRichTextBox.Text.Length;
        }
        #endregion
    }
}
