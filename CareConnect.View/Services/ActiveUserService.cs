using CareConnect.Model.Models;
using CareConnect.View.Services;
using CareConnect.WPF.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace CareConnect.WPF.Services
{
    public class ActiveUserService
    {
        private readonly WindowService _windowService;

        public User? ActiveUser { get; set; }

        public ActiveUserService(WindowService windowService)
        {
            _windowService = windowService;
        }
        public void OpenActiveUserWindow()
        {
            if (ActiveUser == null)
                return;

            switch (ActiveUser.UserRole.ToUpper())
            {
                case "ADMIN":
                    _windowService.OpenWindow<AdminWindow>();
                    break;
                case "STAFF":
                    _windowService.OpenWindow<StaffWindow>();
                    break;
                case "CLIENT":
                    _windowService.OpenWindow<ClientWindow>();
                    break;
            }


        }
    }
}
