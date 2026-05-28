using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.RightsManagement;
using System.Text;
using System.Windows;

namespace CareConnect.WPF.Services
{
    public class NavigationService
    {
        private IServiceProvider _serviceProvider;
        public NavigationService(IServiceProvider serviceProvider) 
        { 
            _serviceProvider = serviceProvider;
        }
        public void OpenWindow<TWindow>() where TWindow : class
        {
            var objWindow = _serviceProvider.GetRequiredService<TWindow>();

            if (objWindow is Window window)
                window.Show();

            else
                throw new Exception($"This type of window {nameof(TWindow)} doesn't exist!");
        }

        public void CloseWindow<TWindow>() where TWindow : class
        {
            var objWindow =  Application.Current.Windows.OfType<TWindow>().FirstOrDefault();

            if (objWindow is Window window)
                window.Close();
            else
                throw new Exception($"This type of window {nameof(TWindow)} is not created!");
        }

        public object? GetUserControlViewModel<TUserControl>() where TUserControl : class
        {
            try
            {
                return _serviceProvider.GetRequiredService<TUserControl>();
            }
            catch(Exception)
            {
                throw new Exception($"This type of user control viewModel {nameof(TUserControl)} doesn't exist!");
            }
        }

        public void OpenDialogWindow<TWindow>() where TWindow : class
        {
            var objWindow = _serviceProvider.GetRequiredService<TWindow>();

            if (objWindow is Window window)
                window.ShowDialog();

            else
                throw new Exception($"This type of window {nameof(TWindow)} doesn't exist!");
        }
    }
}
