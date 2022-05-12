using DesktopApp.Models.Configurations;
using Microsoft.Extensions.Options;

namespace DesktopApp.Models.Services;

public class SettingsService : ISettingsService
{
    public TgBotConfiguration BotConfiguration { get; }

    public SettingsService(IOptions<TgBotConfiguration> botConfiguration)
    {
        BotConfiguration = botConfiguration.Value;
    }
}