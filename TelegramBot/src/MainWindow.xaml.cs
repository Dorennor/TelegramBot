using DesktopApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Chat = DesktopApp.Models.Chat;

namespace DesktopApp;

public partial class MainWindow : Window
{
    private static readonly TelegramBotClient BotClient = new("5262349068:AAHNdyvZ2Hjji7nScbyq9V-c79w39FcTuC4");
    private CancellationTokenSource _source;
    private CancellationToken _token;
    private readonly TGBotDbContext _context;

    public MainWindow()
    {
        InitializeComponent();

        _source = new CancellationTokenSource();
        _token = _source.Token;

        _context = new TGBotDbContext();
        _context.Database.MigrateAsync();
        _context.Chats.LoadAsync();
        _context.Songs.LoadAsync();
    }

    private async void RunButton_OnClick(object sender, RoutedEventArgs e)
    {
        StateLabel.Content = "Enabled";

        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = { }
        };

        BotClient.StartReceiving(HandleUpdateAsync, HandleErrorAsync, receiverOptions, _token);

        var me = await BotClient.GetMeAsync();

        Debug.WriteLine($"Start listening for @{me.Username}");
    }

    private async void StopButton_OnClick(object sender, RoutedEventArgs e)
    {
        StateLabel.Content = "Disabled";

        _source.Cancel();
        _source.Dispose();

        _source = new CancellationTokenSource();
        _token = _source.Token;

        var me = await BotClient.GetMeAsync();

        Debug.WriteLine($"Stop listening for @{me.Username}");
    }

    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Type != UpdateType.Message) return;
        if (update.Message!.Type != MessageType.Text && update.Message!.Type != MessageType.Audio) return;

        var message = update.Message;
        var chat = message.Chat;

        if (message.Type == MessageType.Audio)
        {
            var song = message.Audio;
            var file = BotClient.GetFileAsync(song.FileId);
            var fileName = song.FileName;

            CreatePathIfNotExist();

            var filePath = Directory.GetCurrentDirectory() + @"\Downloads\Music\";

            using (var saveAudioStream = new FileStream(filePath + fileName, FileMode.Create, FileAccess.Write))
            {
                await BotClient.DownloadFileAsync(file.Result.FilePath!, saveAudioStream);
                SendMessage(chat.Id, "Audio saved!");
            }

            using (SHA512 sha512 = SHA512.Create())
            {
                await using (var openAudioStream = new FileStream(filePath + fileName, FileMode.Open, FileAccess.Read))
                {
                    byte[] hashCode = await sha512.ComputeHashAsync(openAudioStream);
                    var hashCodeString = BitConverter.ToString(hashCode);
                    SendMessage(chat.Id, hashCodeString);
                }
            }
        }

        if (!IsExist(message.Chat.Id))
        {
            await _context.Chats.AddAsync(new Chat(chat.Id, chat.Username, chat.FirstName, chat.LastName));
            await _context.SaveChangesAsync();
        }
    }

    private static void CreatePathIfNotExist()
    {
        if (!Directory.Exists(@"\Downloads\Music\"))
        {
            var filePath = Directory.GetCurrentDirectory();
            Directory.CreateDirectory("Downloads");
            Directory.SetCurrentDirectory("Downloads");
            Directory.CreateDirectory("Music");
            Directory.SetCurrentDirectory(filePath);
        }
    }

    private static async void SendMessage(long id, string message)
    {
        await BotClient.SendTextMessageAsync(id, message);
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