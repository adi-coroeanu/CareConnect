using CareConnect.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CareConnect.View.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow(LoginViewModel loginViewModel)
        {
            InitializeComponent();

            DataContext = loginViewModel;

            loginViewModel.PropertyChanged += LoginViewModel_PropertyChanged;
        }

        public void PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel dataContext)
            {
                dataContext.Password = PasswordBox.Password;
            }
        }

        private void LoginViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs args)
        {
            if (sender is LoginViewModel vm)
            {
                if (args.PropertyName == nameof(LoginViewModel.Password))
                {
                    if (PasswordBox.Password != vm.Password)
                    {
                        PasswordBox.Password = vm.Password;
                    }
                }
            }
        }
    }
}
