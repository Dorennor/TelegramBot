using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = DesktopApp.Models.Entities.User;

namespace DesktopApp.Models.Services;

public class BotService : IBotService
{
    private TelegramBotClient BotClient { get; }
    private CancellationTokenSource _botCancellationTokenSource;
    private CancellationToken _botCancellationToken;
    private readonly IUserService _userService;
    public ISettingsService SettingsService { get; }

    public BotService(ISettingsService settingsService, IUserService userService)
    {
        SettingsService = settingsService;
        _userService = userService;

        BotClient = new TelegramBotClient(settingsService.BotConfiguration.BotToken);

        _botCancellationTokenSource = new CancellationTokenSource();
        _botCancellationToken = _botCancellationTokenSource.Token;
    }

    public void RunBot()
    {
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = { }
        };

        BotClient.StartReceiving(HandleUpdateAsync, HandleErrorAsync, receiverOptions, _botCancellationToken);
    }

    public void StopBot()
    {
        _botCancellationTokenSource.Cancel();
        _botCancellationTokenSource.Dispose();

        _botCancellationTokenSource = new CancellationTokenSource();
        _botCancellationToken = _botCancellationTokenSource.Token;
    }

    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Type == UpdateType.Message)
        {
            var message = update.Message;
            var chat = message.Chat;
            var chatId = chat.Id;

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                .WriteTo.File("logs/log.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            Log.Information("Hello, world!");

            await _userService.AddUserIfNotExistAsync(new User(chatId, update.Message.Chat.Username, update.Message.Chat.FirstName, update.Message.Chat.LastName));
        }
    }

    private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Debug.WriteLine(errorMessage);
        return Task.CompletedTask;
    }
}