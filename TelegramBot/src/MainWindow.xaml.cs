using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DesktopApp.Models;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Chat = DesktopApp.Models.Chat;

namespace DesktopApp;

public partial class MainWindow : Window
{
    private static readonly TelegramBotClient botClient = new TelegramBotClient("5262349068:AAHNdyvZ2Hjji7nScbyq9V-c79w39FcTuC4");
    private CancellationTokenSource source;
    private CancellationToken token;
    private TGBotDbContext _context;

    public MainWindow()
    {
        InitializeComponent();

        source = new CancellationTokenSource();
        token = source.Token;

        _context = new TGBotDbContext();
        _context.Database.Migrate();
        _context.Chats.Load();
    }

    private async void RunButton_OnClick(object sender, RoutedEventArgs e)
    {
        StateLabel.Content = "Enabled";

        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = { }
        };

        botClient.StartReceiving(HandleUpdateAsync, HandleErrorAsync, receiverOptions, token);

        var me = await botClient.GetMeAsync();

        Debug.WriteLine($"Start listening for @{me.Username}");
    }

    private async void StopButton_OnClick(object sender, RoutedEventArgs e)
    {
        StateLabel.Content = "Disabled";

        source.Cancel();
        source.Dispose();

        source = new CancellationTokenSource();
        token = source.Token;

        var me = await botClient.GetMeAsync();

        Debug.WriteLine($"Stop listening for @{me.Username}");
    }

    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Type != UpdateType.Message) return;
        if (update.Message!.Type != MessageType.Text && update.Message!.Type != MessageType.Audio) return;

        var message = update.Message;
        var song = message.Audio;
        var chat = message.Chat;

        if (!IsExist(message.Chat.Id))
        {
            await _context.Chats.AddAsync(new Chat(chat.Id, chat.Username, chat.FirstName, chat.LastName));
            await _context.SaveChangesAsync();
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

    private bool IsExist(long chatId)
    {
        var chat = _context.Chats.FirstOrDefault(c => c.ChatId == chatId);
        return chat != null;
    }

    private void ExitButton_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void Minimize_OnClick(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void MainWindow_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        DragMove();
    }
}