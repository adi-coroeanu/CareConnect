using CareConnect.Model.Services;
using CareConnect.View.Services;
using CareConnect.View.Views;
using CareConnect.ViewModel.Commands;
using CareConnect.ViewModel.Services;
using CareConnect.WPF.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace CareConnect.WPF.ViewModels
{
    public class SignupViewModel : NotifyPropertyService
    {
        private readonly SignupService _signupService;
        private readonly ActiveUserService _activeUserService;
        private readonly WindowService _windowService;

        private string? _email;
        private string? _firstName;
        private string? _lastName;
        private string? _password;
        private string? _repassword;
        private string? _staffCode;

        public ICommand SignupCommand { get; }

        public SignupViewModel(SignupService signupService, ActiveUserService activeUserService, WindowService windowService)
        {
            _signupService = signupService;
            _activeUserService = activeUserService;
            _windowService = windowService;

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
                ((RelayCommand)SignupCommand).Refresh();
            }
        }

        public string? FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                ((RelayCommand)SignupCommand).Refresh();
            }
        }
        public string? LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                ((RelayCommand)SignupCommand).Refresh();
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
                OnPropertyChanged(nameof(PasswordFormatError));
                OnPropertyChanged(nameof(PasswordMatchError));
                ((RelayCommand)SignupCommand).Refresh();
            }
        }

        public string? Repassword
        {
            get => _repassword;
            set
            {
                _repassword = value;
                OnPropertyChanged(nameof(PasswordMatchError));
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
                OnPropertyChanged(nameof(CodeError));
            }
        }

        public bool EmailUsedError { get; set; } = false;
        public bool PasswordFormatError { get; set; } = false;
        public bool PasswordMatchError { get; set; } = false;
        public bool CodeError { get; set; } = false;

        public bool CanSignup(object? parameter)
        {
            return _signupService.AllFieldsCompleted(Email, FirstName, LastName, Password, Repassword);
        }

        public void Signup(object? parameter)
        {
            bool success = true;

            var email = Email;
            var password = Password;
            var repassword = Repassword;
            var staffCode = StaffCode;
            var firstName = FirstName;
            var lastName = LastName;

            Email = string.Empty;
            Password = string.Empty;
            Repassword = string.Empty;
            StaffCode = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;

            if(_signupService.ExistingEmail(email))
            {
                success = false;
                EmailUsedError = true;
            }
            if(!_signupService.CorrectPasswordFormat(password))
            {
                success = false;
                PasswordFormatError = true;
            }
            if(!_signupService.MatchingPasswords(password, repassword))
            {
                success = false;
                PasswordMatchError = true;
            }

            if (success)
            {
                string? role = null;

                if (string.IsNullOrWhiteSpace(staffCode))
                    role = "CLIENT";
                else if (_signupService.ExistingCode(staffCode))
                    role = "STAFF";
                else
                    CodeError = true;

                if (role != null)
                {
                    var user = _signupService.AddUser(email, firstName, lastName, password, role);
                    
                    _activeUserService.ActiveUser = user;
                    _activeUserService.OpenActiveUserWindow();

                    _windowService.CloseWindow<SignupWindow>();
                }
            }

            OnPropertyChanged(nameof(EmailUsedError));
            OnPropertyChanged(nameof(PasswordFormatError)); 
            OnPropertyChanged(nameof(PasswordMatchError)); 
            OnPropertyChanged(nameof(CodeError));
        }
    }
}
