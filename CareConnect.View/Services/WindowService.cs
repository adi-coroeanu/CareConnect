using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Security.RightsManagement;
using System.Text;
using System.Windows;

namespace CareConnect.View.Services
{
    public class WindowService
    {
        private IServiceProvider _serviceProvider;
        public WindowService(IServiceProvider serviceProvider) 
        { 
            _serviceProvider = serviceProvider;
        }
        public void OpenWindow<TWindow>() where TWindow : class
        {
            var objWindow = _serviceProvider.GetRequiredService<TWindow>();

            if (objWindow is Window window)
            {
                window.Show();
            }

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


    }
}
