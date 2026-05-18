using CareConnect.View.Services;
using CareConnect.View.Views;
using CareConnect.WPF.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Sockets;
using System.Runtime.InteropServices.JavaScript;
using System.Windows;
using System.Windows.Navigation;

namespace CareConnect.View;

public partial class App
{
    private IHost _host;

    public App()
    {
        _host = new HostBuilder()
            //.ConfigureAppConfiguration((context, config) =>
            //{
            //    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            //})
            .ConfigureServices((context, services) =>
            {
                //Views
                services.AddTransient<LoginWindow>();
                services.AddTransient<SignupWindow>();

                //ViewModels
                services.AddTransient<LoginViewModel>();

                //Services
                services.AddTransient<WindowService>();

            })
            .Build();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        _host.StartAsync();

        var loginWindow = _host.Services.GetRequiredService<LoginWindow>();
        loginWindow.Show();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        using (_host)
        {
            _host.StopAsync();
        }
    }
}
