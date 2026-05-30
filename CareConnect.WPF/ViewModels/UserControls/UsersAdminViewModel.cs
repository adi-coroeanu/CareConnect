using CareConnect.Model.Models;
using CareConnect.Model.Services;
using CareConnect.ViewModel.Commands;
using CareConnect.WPF.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace CareConnect.WPF.ViewModels.UserControls
{
    public class UsersAdminViewModel : NotifyPropertyService
    {
        private readonly UsersAdminService _usersAdminService;
        private readonly ActiveUserService _activeUserService;

        private string? _selectedTypeUsers;
        private User? _selectedUser;

        public ObservableCollection<User> UsersList { get; set; }
        public ICommand DeleteUserCommand { get; }

        public UsersAdminViewModel(UsersAdminService usersAdminService, ActiveUserService activeUserService)
        {
            _usersAdminService = usersAdminService;
            _activeUserService = activeUserService;

            DeleteUserCommand = new RelayCommand(DeleteUser, CanDeleteUser);

            UsersList = new ObservableCollection<User>(_usersAdminService.GetUsers());
            SelectedTypeUsers = "All users";
            SelectedUser = null;
        }

        public string? SelectedTypeUsers
        {
            get => _selectedTypeUsers;
            set
            {
                _selectedTypeUsers = value;

                SelectedUser = null;
                UsersList = new ObservableCollection<User>(_usersAdminService.GetUsers(SelectedTypeUsers));
                OnPropertyChanged(nameof(UsersList));
            }
        }

        public User? SelectedUser 
        { 
            get => _selectedUser;
            set
            {
                _selectedUser = value;

                OnPropertyChanged();
                ((RelayCommand)DeleteUserCommand).Refresh();
            } 
        }

        private void DeleteUser(object? paramater)
        {
            _usersAdminService.DeleteUser(SelectedUser!.Id, _activeUserService.ActiveUser!.Id);
            UsersList = new ObservableCollection<User>(_usersAdminService.GetUsers(SelectedTypeUsers));

            OnPropertyChanged(nameof(UsersList));
            MessageBox.Show("User deleted");
        }

        private bool CanDeleteUser(object? paramater)
        {
            if (SelectedUser == null || SelectedUser.UserRole == "ADMIN")
                return false;
            return true;
        }
    }
}
