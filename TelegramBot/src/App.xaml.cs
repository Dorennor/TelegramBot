using DesktopApp.Models.Data;
using DesktopApp.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Windows;

namespace DesktopApp;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        using (var context = new TgBotDbContext())
        {
            context.Database.Migrate();
        }
    }
}