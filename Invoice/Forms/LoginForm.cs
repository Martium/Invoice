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

        private void LoginButton_Click(object sender, System.EventArgs e)
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

        private void ChangePasswordButton_Click(object sender, System.EventArgs e)
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
                _messageDialogService.ShowErrorMassage("Norint Pakeisti slaptažodį turite įvesti naują slaptažodį ");
            }
            else
            {
                _messageDialogService.ShowErrorMassage("Turite slaptažodžio tekste įvesti teisingą slaptažodį, tik tuomet galėsite pakeisti slaptažodį");
            }
        }
    }
}
