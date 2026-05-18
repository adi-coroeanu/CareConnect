using CareConnect.ViewModel.Commands;
using CareConnect.ViewModel.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;

namespace CareConnect.ViewModel.ViewModels
{
    public class LoginViewModel : NotifyProperty
    {
        private readonly IWindowService _windowService;
        private string? _username;
        private bool _errorPassword = false;


        public ICommand LoginCommand { get; }
        public ICommand SignupCommand { get; }

        public LoginViewModel(IWindowService windowService)
        {
            _windowService = windowService;

            LoginCommand = new RelayCommand(Login, CanLogin);
            SignupCommand = new RelayCommand(Signup);
        }

        public string? Username
        {
            get => _username;
            set
            {
                _username = value;
                ErrorPassword = true;
                OnPropertyChanged(nameof(ErrorPassword));
            }
        }

        public bool ErrorPassword { get; set; } = false;
        
        private bool CanLogin(object? parameter)
        {
            return _errorPassword;
        }
        private void Login(object? parameter)
        {
            Debug.WriteLine("dsadas");
        }
        private void Signup(object? parameter)
        {
            //_windowService.OpenWindow<SignupWindow>();
        }

    }
}


