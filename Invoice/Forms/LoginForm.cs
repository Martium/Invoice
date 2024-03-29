﻿using System;
using System.Linq;
using System.Windows.Forms;
using Invoice.Constants;
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

            SetTextBoxMaxLengths();

            ChangeTextBoxTypingType();
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

            if (PasswordTextBox.Text == _password && !string.IsNullOrWhiteSpace(ChangePasswordTextBox.Text) && RepeatNewPasswordTextBox.Text == ChangePasswordTextBox.Text)
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
            else if (string.IsNullOrWhiteSpace(ChangePasswordTextBox.Text) || string.IsNullOrWhiteSpace(RepeatNewPasswordTextBox.Text) || string.IsNullOrWhiteSpace(PasswordTextBox.Text))
            {
                _messageDialogService.ShowErrorMassage("Norint Pakeisti slaptažodį 1) Turite įvesti seną slaptažodį (į slaptažožio lentelę.) " +
                                                       "2) Turite įvesti naują slaptažodį (į naujas slaptažodį lentelę) " +
                                                       "3) Turite pakartoti naują slaptažodį ( į pakartoti naują slaptažodį lentelę) ");
            }
            else
            {
                _messageDialogService.ShowErrorMassage("Slaptažodis neatitinka");
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
                this.SelectNextControl((Control) sender, true, true, true, true);
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

        private void RepeatNewPasswordTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ChangePasswordButton_Click(this, new EventArgs());
            }
        }

        private void ChangeTextBoxTypingType()
        {
            PasswordTextBox.PasswordChar = '*';
            ChangePasswordTextBox.PasswordChar = '*';
            RepeatNewPasswordTextBox.PasswordChar = '*';
        }

        private void SetTextBoxMaxLengths()
        {
            PasswordTextBox.MaxLength = FormSettings.TextBoxLengths.Password;
            ChangePasswordTextBox.MaxLength = FormSettings.TextBoxLengths.Password;
            RepeatNewPasswordTextBox.MaxLength = FormSettings.TextBoxLengths.Password;
        }
    }
}
