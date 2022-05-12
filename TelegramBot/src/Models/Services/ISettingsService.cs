using DesktopApp.Models.Configurations;

namespace DesktopApp.Models.Services;

public interface ISettingsService
{
    TgBotConfiguration BotConfiguration { get; }
}