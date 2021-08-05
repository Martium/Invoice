using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Invoice.Models.BuyersInfo;
using Invoice.Repositories;

namespace Invoice.Forms
{
    public partial class BuyersInfoForm : Form
    {
        private readonly BuyersInfoRepository _buyersInfoRepository;

        public BuyersInfoForm()
        {
            _buyersInfoRepository = new BuyersInfoRepository();

            InitializeComponent();

            SetControlsInitialState();

            FillExistsBuyerListComboBox();
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

        #endregion
    }
}
