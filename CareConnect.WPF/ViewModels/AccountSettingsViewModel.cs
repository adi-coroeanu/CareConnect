using CareConnect.Model.Services;
using CareConnect.ViewModel.Commands;
using CareConnect.WPF.Services;
using CareConnect.WPF.Views;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Text;
using System.Windows.Input;

namespace CareConnect.WPF.ViewModels
{
    public class AccountSettingsViewModel : NotifyPropertyService
    {
        private readonly ActiveUserService _activeUserService;
        private readonly NavigationService _navigationService;
        private readonly AccountSettingsService _accountSettingsService;

        private string? _email;
        private string? _firstName;
        private string? _lastName;
        private string? _password;
        private string? _repassword;
        private bool _emailUsedError = false;
        private bool _passwordFormatError = false;
        private bool _passwordMatchError = false;
        private bool _minimumOneParameterCanged = false;
        private bool _parametersChangedError = false;

        public ICommand ConfirmSettingsCommand { get; }
        public AccountSettingsViewModel(ActiveUserService activeUserService, NavigationService navigationService, AccountSettingsService accountSettingsService) 
        {
            _activeUserService = activeUserService;
            _navigationService = navigationService;
            _accountSettingsService = accountSettingsService;

            ConfirmSettingsCommand = new RelayCommand(ConfirmSettings, CanConfirmSettings);

            InitParameters();
        }

        public string? Email
        {
            get => _email;
            set
            {
                _email = value;
                EmailUsedError = false;
                _minimumOneParameterCanged = true;
                OnPropertyChanged(nameof(EmailUsedError));
                OnPropertyChanged();
                ((RelayCommand)ConfirmSettingsCommand).Refresh();
            }
        }

        public string? FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                _minimumOneParameterCanged = true;
                OnPropertyChanged();
                ((RelayCommand)ConfirmSettingsCommand).Refresh();
            }
        }
        public string? LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                _minimumOneParameterCanged = true;
                OnPropertyChanged();
                ((RelayCommand)ConfirmSettingsCommand).Refresh();
            }
        }
        public string? Password
        {
            get => _password;
            set
            {
                _password = value;

                if (!string.IsNullOrWhiteSpace(_password))
                {
                    PasswordFormatError = false;
                    PasswordMatchError = false;
                }

                _minimumOneParameterCanged = true;
                OnPropertyChanged();
                ((RelayCommand)ConfirmSettingsCommand).Refresh();
            }
        }

        public string? Repassword
        {
            get => _repassword;
            set
            {
                _repassword = value;

                if (!string.IsNullOrWhiteSpace(_password))
                {
                    PasswordFormatError = false;
                    PasswordMatchError = false;
                }

                _minimumOneParameterCanged = true;
                OnPropertyChanged();
                ((RelayCommand)ConfirmSettingsCommand).Refresh();
            }
        }

        public bool EmailUsedError
        {
            get => _emailUsedError;
            set
            {
                _emailUsedError = value;

                if (_emailUsedError)
                    Email = string.Empty;

                OnPropertyChanged();
            }
        }
        public bool PasswordFormatError
        {
            get => _passwordFormatError;
            set
            {
                _passwordFormatError = value;

                if (_passwordFormatError)
                {
                    Password = string.Empty;
                    Repassword = string.Empty;
                }

                OnPropertyChanged();
            }
        }
        public bool PasswordMatchError
        {
            get => _passwordMatchError;
            set
            {
                _passwordMatchError = value;

                if (_passwordMatchError)
                {
                    Password = string.Empty;
                    Repassword = string.Empty;
                }

                OnPropertyChanged();
            }
        }
        public bool ParametersChangedError 
        {
            get => _parametersChangedError;
            set
            {
                _parametersChangedError = value;

                OnPropertyChanged();
            }
        }

        private void ConfirmSettings(object? parameter)
        {
            EmailUsedError = _accountSettingsService.ExistingEmail(Email, _activeUserService.ActiveUser!.Id);

            PasswordFormatError = !_accountSettingsService.CorrectPasswordFormat(Password);
            PasswordMatchError = !_accountSettingsService.MatchingPasswords(Password, Repassword);

            if (EmailUsedError || PasswordFormatError || PasswordMatchError)
            {
                ParametersChangedError = true;
                return;
            }

            if (_accountSettingsService.SaveAccountSettings(_activeUserService.ActiveUser, Email, FirstName, LastName, Password))
                _navigationService.CloseWindow<AccountSettingsView>();
        }

        private bool CanConfirmSettings(object? parameter)
        {
            return _minimumOneParameterCanged;
        }

        private void InitParameters()
        {
            _email = _activeUserService.ActiveUser!.Email;
            _firstName = _activeUserService.ActiveUser.FirstName;
            _lastName = _activeUserService.ActiveUser.LastName;
        }
    }
}
