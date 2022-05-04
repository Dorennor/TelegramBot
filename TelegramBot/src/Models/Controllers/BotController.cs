using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace DesktopApp.Models.Controllers;

public class BotController
{
    public TelegramBotClient BotClient { get; } = new("5324310799:AAEH9YQlB4asYeLe0Dr2O1j0XOdlSgZyJHM");
    private CancellationTokenSource _botCancellationTokenSource;
    private CancellationToken _botCancellationToken;

    public BotController()
    {
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
        if (update.Type != UpdateType.Message) return;
        if (update.Message!.Type != MessageType.Text && update.Message!.Type != MessageType.Audio) return;

        var lastHandledMessage = update.Message;
        var userChat = lastHandledMessage.Chat;

        using (var userController = new UserController())
        {
            await userController.AddToDbIfUserDoesNotExist(userChat);
        }

        if (lastHandledMessage.Type == MessageType.Audio)
        {
            var song = lastHandledMessage.Audio;

            using (var songsController = new SongsController())
            {
                if (await songsController.AddToDbIfDoesNotExist(song))
                {
                    await BotClient.SendTextMessageAsync(userChat.Id, "Audio saved!");
                }
            }
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