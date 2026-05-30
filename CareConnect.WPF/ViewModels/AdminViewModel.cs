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
    public class AdminViewModel : NotifyPropertyService
    {
        private readonly NavigationService _navigationService;
        private readonly ActiveUserService _activeUserService;

        private object? _currentUserControlViewModel;
        private string _selectedItemSideBar;

        public ICommand SignoutCommand { get; }

        public AdminViewModel(NavigationService navigationService, ActiveUserService activeUserService)
        {
            _navigationService = navigationService;
            _activeUserService = activeUserService;

            CurrentUserControlViewModel = _navigationService.GetUserControlViewModel<HomeClientViewModel>();
            _selectedItemSideBar = "Stats";

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
            if (userControlString == "Stats")
                return _navigationService.GetUserControlViewModel<StatsAdminViewModel>();
            if (userControlString == "Edit users")
                return _navigationService.GetUserControlViewModel<UsersAdminViewModel>();
            if (userControlString == "Generate code")
                return _navigationService.GetUserControlViewModel<CodeAdminViewModel>();
            if (userControlString == "Audit")
                return _navigationService.GetUserControlViewModel<AuditAdminViewModel>();
            if (userControlString == "Settings")
            {
                _navigationService.OpenDialogWindow<AccountSettingsView>();
                _selectedItemSideBar = "Home";
                return _navigationService.GetUserControlViewModel<StatsAdminViewModel>();
            }
            return null;
        }

        private void Signout(object? parameter)
        {
            _navigationService.OpenWindow<LoginWindow>();
            _navigationService.CloseWindow<AdminWindow>();
        }
    }
}
