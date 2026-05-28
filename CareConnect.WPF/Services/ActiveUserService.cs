using CareConnect.Model.Models;
using CareConnect.WPF.Views;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows;

namespace CareConnect.WPF.Services
{
    public class ActiveUserService
    {
        private readonly NavigationService _windowService;

        public User? ActiveUser { get; set; }

        public ActiveUserService(NavigationService windowService)
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

        public string GreetingMessage()
        {
            string greeting;
            var hour = DateTime.Now.Hour;

            if (hour < 12)
                greeting = "Good morning";
            else if (hour < 18)
                greeting = "Good afternoon";
            else
                greeting = "Good evening";

            return $"{greeting}, {ActiveUser!.FirstName} {ActiveUser!.LastName}";
        }
    }
}
