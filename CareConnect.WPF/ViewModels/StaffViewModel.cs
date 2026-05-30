using CareConnect.ViewModel.Commands;
using CareConnect.WPF.Services;
using CareConnect.WPF.ViewModels.UserControls;
using CareConnect.WPF.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace CareConnect.WPF.ViewModels
{
    public class StaffViewModel : NotifyPropertyService
    {
        private readonly NavigationService _navigationService;
        private readonly ActiveUserService _activeUserService;

        private object? _currentUserControlViewModel;
        private string _selectedItemSideBar;

        public ICommand SignoutCommand { get; }

        public StaffViewModel(NavigationService navigationService, ActiveUserService activeUserService)
        {
            _navigationService = navigationService;
            _activeUserService = activeUserService;

            CurrentUserControlViewModel = _navigationService.GetUserControlViewModel<AppointmentsStaffViewModel>();
            _selectedItemSideBar = "Appointments";

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
            if (userControlString == "Appointments")
                return _navigationService.GetUserControlViewModel<AppointmentsStaffViewModel>();
            if (userControlString == "Edit Services")
                return _navigationService.GetUserControlViewModel<ServicesStaffViewModel>();
            if (userControlString == "Billing & Payments")
                return _navigationService.GetUserControlViewModel<PaymentsStaffViewModel>();
            if (userControlString == "Settings")
            {
                _navigationService.OpenDialogWindow<AccountSettingsView>();
                _selectedItemSideBar = "Appointments";
                return _navigationService.GetUserControlViewModel<AppointmentsStaffViewModel>();
            }
            return null;
        }

        private void Signout(object? parameter)
        {
            _navigationService.OpenWindow<LoginWindow>();
            _navigationService.CloseWindow<StaffWindow>();
        }
    }
}

