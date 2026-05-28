using CareConnect.Model.Models;
using CareConnect.View;
using CareConnect.ViewModel.Commands;
using CareConnect.WPF.Services;
using CareConnect.WPF.ViewModels.UserControls;
using CareConnect.WPF.Views;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace CareConnect.WPF.ViewModels
{
    public class ClientViewModel : NotifyPropertyService
    {
        private readonly NavigationService _navigationService;
        private readonly ActiveUserService _activeUserService;

        private object? _currentUserControlViewModel;
        private string _selectedItemSideBar;

        public ICommand SignoutCommand { get; }

        public ClientViewModel(NavigationService navigationService, ActiveUserService activeUserService)
        {
            _navigationService = navigationService;
            _activeUserService = activeUserService;

            CurrentUserControlViewModel = _navigationService.GetUserControlViewModel<HomeClientViewModel>();
            _selectedItemSideBar = "Home";

            SignoutCommand = new RelayCommand(Signout);
            _activeUserService = activeUserService;
        }

        public object? CurrentUserControlViewModel
        {
            get => _currentUserControlViewModel;
            set
            {
                _currentUserControlViewModel = value;
                OnPropertyChanged(nameof(CurrentUserControlViewModel));
            }
        }

        public string SelectedItemSideBar
        {
            get => _selectedItemSideBar;
            set
            {

                _selectedItemSideBar = value;

                CurrentUserControlViewModel = GetUserControl(SelectedItemSideBar);
            }
        }

        public string GreetingMessage
        {
            get => _activeUserService.GreetingMessage();
        }

        private object? GetUserControl(string userControlString)
        {
            if(userControlString == "Home")
                return _navigationService.GetUserControlViewModel<HomeClientViewModel>();
            if(userControlString == "Appointments")
                return _navigationService.GetUserControlViewModel<AppointmentsClientViewModel>();
            if (userControlString == "Billing & Payments")
                return _navigationService.GetUserControlViewModel<PaymentsClientViewModel>();
            if (userControlString == "Settings")
            {
                _navigationService.OpenDialogWindow<AccountSettingsView>();
                _selectedItemSideBar = "Home";
                return _navigationService.GetUserControlViewModel<HomeClientViewModel>();
            }
            return null;
        }

        private void Signout(object? parameter)
        {
            _navigationService.OpenWindow<LoginWindow>();
            _navigationService.CloseWindow<ClientWindow>();
        }
    }
}
