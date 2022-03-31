using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Models;
using Chat = TelegramBot.Models.Chat;

namespace TelegramBot
{
    public partial class MainWindow : Window
    {
        private TelegramBotClient botClient;
        private CancellationTokenSource source;
        private CancellationToken token;
        private ChatsDbContext _context;

        public MainWindow()
        {
            InitializeComponent();
            RunButton.IsEnabled = true;
            StopButton.IsEnabled = false;
            StateLabel.Content = "Disabled";
            botClient = new TelegramBotClient("5262349068:AAHNdyvZ2Hjji7nScbyq9V-c79w39FcTuC4");
            source = new CancellationTokenSource();
            token = source.Token;
            _context = new ChatsDbContext();
            _context.Chats.Load();
        }

        private async void RunButton_OnClick(object sender, RoutedEventArgs e)
        {
            StateLabel.Content = "Enabled";
            RunButton.IsEnabled = false;
            StopButton.IsEnabled = true;

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
            RunButton.IsEnabled = true;
            StopButton.IsEnabled = false;

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
            if (update.Message!.Type != MessageType.Text) return;

            Debug.WriteLine($"Received a '{update.Message.Text}' message in chat {update.Message.Chat.Id}.");

            if (!IsExist(update.Message.Chat.Id))
            {
                await _context.Chats.AddAsync(new Chat(update.Message.Chat.Id, update.Message.Chat.Username, update.Message.Chat.FirstName, update.Message.Chat.LastName));
                await _context.SaveChangesAsync();
            }

            await botClient.SendTextMessageAsync(update.Message.Chat.Id, $"ChatID: {update.Message.Chat.Id}\nUsername: {update.Message.Chat.Username}\nFirstName: {update.Message.Chat.FirstName}\nLastName: {update.Message.Chat.LastName}", cancellationToken: cancellationToken);
        }

        private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Debug.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        private bool IsExist(long ChatId)
        {
            var temp = _context.Chats.FirstOrDefault(c => c.ChatId == ChatId);
            return temp != null;
        }
    }
}