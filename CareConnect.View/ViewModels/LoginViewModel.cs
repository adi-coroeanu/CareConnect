using CareConnect.Model.Services;
using CareConnect.View.Services;
using CareConnect.View.Views;
using CareConnect.ViewModel.Commands;
using CareConnect.ViewModel.Services;
using CareConnect.WPF.Services;
using CareConnect.WPF.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CareConnect.WPF.ViewModels
{
    public class LoginViewModel : NotifyPropertyService
    {
        private readonly WindowService _windowService;
        private readonly LoginService _loginService;
        private readonly ActiveUserService _activeUserService;

        private string? _email;
        private string? _password;


        public ICommand LoginCommand { get; }
        public ICommand SignupCommand { get; }

        public LoginViewModel(WindowService windowService, LoginService loginService, ActiveUserService activeUserService)
        {
            _windowService = windowService;
            _loginService = loginService;
            _activeUserService = activeUserService;

            LoginCommand = new RelayCommand(Login, CanLogin);
            SignupCommand = new RelayCommand(Signup);
        }

        public string? Email
        {
            get => _email;
            set
            {
                _email = value;
                NoUserFoundError = false;
                OnPropertyChanged(nameof(NoUserFoundError));
                ((RelayCommand)LoginCommand).Refresh();
            }
        }

        public string? Password
        {
            get => _password;
            set
            {
                _password = value;
                NoUserFoundError = false;
                OnPropertyChanged(nameof(NoUserFoundError));
                ((RelayCommand)LoginCommand).Refresh();
            }
        }

        public bool NoUserFoundError { get; set; } = false;
        
        private bool CanLogin(object? parameter)
        {
            return _loginService.AllFieldsCompleted(Email, Password);
        }
        private void Login(object? parameter)
        {

            if (Email == null || Password == null)
                return;

            var user = _loginService.GetUserFromDb(Email, Password);

            if(user != null)
            {
                _activeUserService.ActiveUser = user;
                _activeUserService.OpenActiveUserWindow();
                _windowService.CloseWindow<LoginWindow>();
            }

            Email = string.Empty;
            Password = string.Empty;

            OnPropertyChanged(nameof(Email));
            OnPropertyChanged(nameof(Password));

            NoUserFoundError = true;
            OnPropertyChanged(nameof(NoUserFoundError));
        }
        private void Signup(object? parameter)
        {
            _windowService.OpenWindow<SignupWindow>();

            _windowService.CloseWindow<LoginWindow>();
        }

    }
}


