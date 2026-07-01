using CareConnect.Model.Services;
using CareConnect.ViewModel.Commands;
using CareConnect.WPF.Services;
using CareConnect.WPF.Views;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Text;
using System.Windows.Input;

namespace CareConnect.WPF.ViewModels
{
    public class SignupViewModel : NotifyPropertyService
    {
        private readonly SignupService _signupService;
        private readonly ActiveUserService _activeUserService;
        private readonly NavigationService _navigationService;

        private string? _email;
        private string? _firstName;
        private string? _lastName;
        private string? _password;
        private string? _repassword;
        private string? _staffCode;
        private bool _emailUsedError = false;
        private bool _passwordFormatError = false;
        private bool _passwordMatchError = false;
        private bool _codeError = false;

        public ICommand SignupCommand { get; }

        public SignupViewModel(SignupService signupService, ActiveUserService activeUserService, NavigationService windowService)
        {
            _signupService = signupService;
            _activeUserService = activeUserService;
            _navigationService = windowService;

            SignupCommand = new RelayCommand(Signup, CanSignup);
        }

        public string? Email
        {
            get => _email;
            set
            {
                _email = value;
                EmailUsedError = false;
                OnPropertyChanged(nameof(EmailUsedError));
                OnPropertyChanged();
                ((RelayCommand)SignupCommand).Refresh();
            }
        }

        public string? FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged();
                ((RelayCommand)SignupCommand).Refresh();
            }
        }
        public string? LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged();
                ((RelayCommand)SignupCommand).Refresh();
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

                OnPropertyChanged();
                ((RelayCommand)SignupCommand).Refresh();
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

                OnPropertyChanged();
                ((RelayCommand)SignupCommand).Refresh();
            }
        }

        public string? StaffCode
        {
            get => _staffCode;
            set
            {
                _staffCode = value;
                CodeError = false;
                OnPropertyChanged();
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

                if(_passwordFormatError)
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
        public bool CodeError
        {
            get => _codeError;
            set
            {
                _codeError = value;

                if (_codeError)
                    StaffCode = string.Empty;

            OnPropertyChanged();
            }
        }

        public bool CanSignup(object? parameter)
        {
            return _signupService.AllFieldsCompleted(Email, FirstName, LastName, Password, Repassword);
        }

        public void Signup(object? parameter)
        {
            EmailUsedError = _signupService.ExistingEmail(Email);
            PasswordFormatError = !_signupService.CorrectPasswordFormat(Password);
            PasswordMatchError = !_signupService.MatchingPasswords(Password, Repassword);

            if (!string.IsNullOrWhiteSpace(StaffCode) && !_signupService.ExistingCode(StaffCode))
                CodeError = true;

            if (EmailUsedError || PasswordFormatError || PasswordMatchError || CodeError)
            {
                return;
            }
            string role;

            if (_signupService.ExistingCode(StaffCode))
                role = "STAFF";
            else
                role = "CLIENT";


            var user = _signupService.AddUser(Email, FirstName, LastName, Password, role);
                    
            _activeUserService.ActiveUser = user;
            _activeUserService.OpenActiveUserWindow();
            _navigationService.CloseWindow<SignupWindow>();
        }
    }
}
