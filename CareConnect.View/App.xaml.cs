using CareConnect.Model.Models;
using CareConnect.Model.Services;
using CareConnect.View.Services;
using CareConnect.View.Views;
using CareConnect.WPF.Services;
using CareConnect.WPF.ViewModels;
using CareConnect.WPF.Views;
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
                services.AddDbContext<ModelContext>(options =>
                {
                    options.UseOracle("User Id=CareConnect;Password=CareConnect1234;Data Source=localhost:1521/XEPDB1;");
                });

                //Views
                services.AddTransient<LoginWindow>();
                services.AddTransient<SignupWindow>();
                services.AddTransient<AdminWindow>();
                services.AddTransient<StaffWindow>();
                services.AddTransient<ClientWindow>();

                //ViewModels
                services.AddTransient<LoginViewModel>();
                services.AddTransient<SignupViewModel>();

                //Services
                services.AddTransient<WindowService>();
                services.AddTransient<ActiveUserService>();
                services.AddTransient<LoginService>();
                services.AddTransient<SignupService>();

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
