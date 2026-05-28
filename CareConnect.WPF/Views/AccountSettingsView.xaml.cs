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

namespace CareConnect.WPF.Views
{
    public partial class AccountSettingsView : Window
    {
        public AccountSettingsView(AccountSettingsViewModel accountSettingsViewModel)
        {
            InitializeComponent();

            DataContext = accountSettingsViewModel;
            accountSettingsViewModel.PropertyChanged += AccountSettingsViewModel_PropertyChanged;
        }

        public void PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is AccountSettingsViewModel dataContext)
            {
                dataContext.Password = PasswordBox.Password;
            }
        }

        public void RepasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is AccountSettingsViewModel dataContext)
            {
                dataContext.Repassword = RepasswordBox.Password;
            }
        }

        private void AccountSettingsViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs args)
        {
            if (sender is AccountSettingsViewModel vm)
            {
                if (args.PropertyName == nameof(AccountSettingsViewModel.Password))
                {
                    if (PasswordBox.Password != vm.Password)
                    {
                        PasswordBox.Password = vm.Password;
                    }
                }
                else if (args.PropertyName == nameof(AccountSettingsViewModel.Repassword))
                {
                    if (RepasswordBox.Password != vm.Repassword)
                    {
                        RepasswordBox.Password = vm.Repassword;
                    }
                }
            }
        }
    }
}
