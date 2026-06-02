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
        private bool _minimumOneParameterCanged = false;

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
                ((RelayCommand)ConfirmSettingsCommand).Refresh();
            }
        }
        public string? Password
        {
            get => _password;
            set
            {
                _password = value;
                PasswordFormatError = false;
                PasswordMatchError = false;
                _minimumOneParameterCanged = true;
                OnPropertyChanged(nameof(PasswordFormatError));
                OnPropertyChanged(nameof(PasswordMatchError));
                ((RelayCommand)ConfirmSettingsCommand).Refresh();
            }
        }

        public string? Repassword
        {
            get => _repassword;
            set
            {
                _repassword = value;
                _minimumOneParameterCanged = true;
                PasswordMatchError = false;
                OnPropertyChanged(nameof(PasswordMatchError));
                ((RelayCommand)ConfirmSettingsCommand).Refresh();
            }
        }

        public bool EmailUsedError { get; set; } = false;
        public bool PasswordFormatError { get; set; } = false;
        public bool PasswordMatchError { get; set; } = false;
        public bool ParametersChangedError { get; set; } = false;

        private void ConfirmSettings(object? parameter)
        {
            bool anyError = false;

            if(_accountSettingsService.ExistingEmail(Email, _activeUserService.ActiveUser!.Id))
            {
                EmailUsedError = true;
                OnPropertyChanged(nameof(EmailUsedError));
                anyError = true;
            }

            if (Password != null && Repassword != null)
            {
                if (!_accountSettingsService.CorrectPasswordFormat(Password))
                {
                    PasswordFormatError = true;
                    OnPropertyChanged(nameof(PasswordFormatError));
                    anyError = true;
                }
                if (!_accountSettingsService.MatchingPasswords(Password, Repassword))
                {
                    PasswordMatchError = true;
                    OnPropertyChanged(nameof(PasswordMatchError));
                    anyError = true;
                }
            }

            if (!anyError)
            {
                if (_accountSettingsService.SaveAccountSettings(_activeUserService.ActiveUser, Email, FirstName, LastName, Password))
                    _navigationService.CloseWindow<AccountSettingsView>();
            }

            ParametersChangedError = true;
            OnPropertyChanged(nameof(ParametersChangedError));
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
