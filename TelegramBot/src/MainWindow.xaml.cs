using DesktopApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.IO;
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
    private static readonly TelegramBotClient BotClient = new("5324310799:AAEH9YQlB4asYeLe0Dr2O1j0XOdlSgZyJHM");
    private CancellationTokenSource _source;
    private CancellationToken _token;
    private readonly TgBotDbContext _context;

    public MainWindow()
    {
        InitializeComponent();

        _source = new CancellationTokenSource();
        _token = _source.Token;

        _context = new TgBotDbContext();
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

        if (!_context.IsChatExist(message.Chat.Id))
        {
            await _context.Chats.AddAsync(new Chat(chat.Id, chat.Username, chat.FirstName, chat.LastName));
            await _context.SaveChangesAsync();
        }

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
                await BotClient.SendTextMessageAsync(chat.Id, "Audio saved!");
            }

            var songTags = TagLib.File.Create(filePath + fileName).Tag;

            await _context.Songs.AddAsync(new Song(song.FileId, song.FileUniqueId, song.FileName, song.Duration, DateTime.Now, song.Title, songTags.FirstAlbumArtist, songTags.Album, songTags.Year, null, songTags.Performers, songTags.Genres, null, GenerateHashCode(filePath, fileName).Result));
            await _context.SaveChangesAsync();
        }
    }

    private async Task<string> GenerateHashCode(string filePath, string fileName)
    {
        using (SHA512 sha512 = SHA512.Create())
        {
            using (var openAudioStream = new FileStream(filePath + fileName, FileMode.Open, FileAccess.Read))
            {
                byte[] hashCode = await sha512.ComputeHashAsync(openAudioStream);
                return BitConverter.ToString(hashCode);
            }
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