using System;
using System.Linq;
using System.Windows.Forms;
using Invoice.Models;
using Invoice.Repositories;
using Invoice.Service;

namespace Invoice.Forms
{
    public partial class LoginForm : Form
    {
        private readonly LoginRepository _loginRepository;
        private readonly MessageDialogService _messageDialogService;

        private string _password;
        public LoginForm()
        {
            _loginRepository = new LoginRepository();

            _messageDialogService = new MessageDialogService();

            InitializeComponent();
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            _password = _loginRepository.GetPassword().First();

            if (PasswordTextBox.Text == _password)
            {
                _loginRepository.ChangeIsPasswordCorrect(true);
                this.Close();
            }
            else
            {
                _messageDialogService.ShowErrorMassage("Neteisingas slaptažodis");
            }
        }

        private void ChangePasswordButton_Click(object sender, EventArgs e)
        {
            _password = _loginRepository.GetPassword().First();

            if (PasswordTextBox.Text == _password && !string.IsNullOrWhiteSpace(ChangePasswordTextBox.Text))
            {
                PasswordModel newPassword = new PasswordModel
                {
                    Password = ChangePasswordTextBox.Text
                };

                bool isPasswordChanged = _loginRepository.ChangePassword(newPassword);

                if (isPasswordChanged)
                {
                    _messageDialogService.ShowInfoMessage("Slaptažodis sekmingai pakeistas");
                }
                else
                {
                    _messageDialogService.ShowErrorMassage("nepavyko pakeisti");
                }
            }
            else if (string.IsNullOrWhiteSpace(ChangePasswordTextBox.Text))
            {
                _messageDialogService.ShowErrorMassage("Norint Pakeisti slaptažodį turite įvesti seną slaptažodį į slaptažožio lentelę");
            }
            else
            {
                _messageDialogService.ShowErrorMassage("Turite slaptažodžio tekste įvesti teisingą slaptažodį, tik tuomet galėsite pakeisti slaptažodį");
            }
        }

        private void PasswordTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoginButton_Click(this, new EventArgs());
            }
        }

        private void ChangePasswordTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ChangePasswordButton_Click(this, new EventArgs());
            }
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
    }
}
