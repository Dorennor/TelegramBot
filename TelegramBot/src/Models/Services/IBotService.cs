using DesktopApp.Models.Configurations;

namespace DesktopApp.Models.Services;

public interface IBotService
{
    ISettingsService SettingsService { get; }

    void RunBot();

    void StopBot();
}