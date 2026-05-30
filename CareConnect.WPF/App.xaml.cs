using CareConnect.Model.Models;
using CareConnect.Model.Services;
using CareConnect.WPF.Views;
using CareConnect.WPF.Services;
using CareConnect.WPF.ViewModels;
using CareConnect.WPF.ViewModels.UserControls;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Sockets;
using System.Runtime.InteropServices.JavaScript;
using System.Windows;
using CareConnect.WPF.Views.UserControls;
using CareConnect.WPF.Workers;

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
                services.AddTransient<AccountSettingsView>();

                //ViewModels
                services.AddTransient<LoginViewModel>();
                services.AddTransient<SignupViewModel>();
                services.AddTransient<ClientViewModel>();
                services.AddTransient<HomeClientViewModel>();
                services.AddTransient<AppointmentsClientViewModel>();
                services.AddTransient<PaymentsClientViewModel>();
                services.AddTransient<AccountSettingsViewModel>();
                services.AddTransient<StaffViewModel>();
                services.AddTransient<AppointmentsStaffViewModel>();
                services.AddTransient<ServicesStaffViewModel>();
                services.AddTransient<PaymentsStaffViewModel>();
                services.AddTransient<ServicesStaffViewModel>();
                services.AddTransient<StatsAdminViewModel>();
                services.AddTransient<UsersAdminViewModel>();
                services.AddTransient<CodeAdminViewModel>();
                services.AddTransient<AuditAdminViewModel>();
                services.AddTransient<AdminViewModel>();

                //Services
                services.AddTransient<NavigationService>();
                services.AddSingleton<ActiveUserService>();
                services.AddTransient<LoginService>();
                services.AddTransient<SignupService>();
                services.AddTransient<HomeClientService>();
                services.AddTransient<AppointmentsClientService>();
                services.AddTransient<AccountSettingsService>();
                services.AddTransient<PaymentsClientService>();
                services.AddTransient<AppointmentsStaffService>();
                services.AddTransient<ServiceStaffService>();
                services.AddTransient<PaymentsStaffService>();
                services.AddTransient<CodeAdminService>();
                services.AddTransient<UsersAdminService>();
                services.AddSingleton<AuditService>();

                //Workers
                services.AddHostedService<CodesWorker>();
            })
            .Build();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        _host.StartAsync();

        var loginWindow = _host.Services.GetRequiredService<LoginWindow>(); //!!!!!!
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
