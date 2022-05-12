using DesktopApp.Models.Configurations;
using DesktopApp.Models.Data;
using DesktopApp.Models.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Windows;

namespace DesktopApp;

public partial class App : Application
{
    private readonly IHost _host;
    private IConfiguration Configuration { get; }

    public App()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        Configuration = builder.Build();

        _host = Host.CreateDefaultBuilder().ConfigureServices(ConfigureServices).Build();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<TgBotDbContext>(options =>
            options.UseSqlite(Configuration.GetConnectionString("TGBotDb")));
        services.Configure<TgBotConfiguration>(Configuration.GetSection("TgBotConfiguration"));
        services.AddScoped<ISongsRepository, SongsRepository>();
        services.AddScoped<IUsersRepository, UsersRepository>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IBotService, BotService>();
        services.AddScoped<ISettingsService, SettingsService>();
#if DEBUG
        services.AddScoped<MainWindow>();
#else
        services.AddSingleton<MainWindow>();
#endif
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await _host.StartAsync();

        var mainWindow = _host.Services.GetRequiredService<MainWindow>();
        mainWindow.Show();

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        using (_host)
        {
            await _host.StopAsync();
        }

        base.OnExit(e);
    }
}