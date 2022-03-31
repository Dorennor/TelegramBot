using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
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
            TextBox.Text = "Disabled";
            botClient = new TelegramBotClient("5262349068:AAHNdyvZ2Hjji7nScbyq9V-c79w39FcTuC4");
            source = new CancellationTokenSource();
            token = source.Token;
            _context = new ChatsDbContext();
            _context.Chats.Load();
        }

        private async void RunButton_OnClick(object sender, RoutedEventArgs e)
        {
            TextBox.Text = "Enabled";

            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }
            };

            botClient.StartReceiving(HandleUpdateAsync, HandleErrorAsync, receiverOptions, token);

            var me = await botClient.GetMeAsync();

            Debug.WriteLine($"Start listening for @{me.Username}");
        }

        private void StopButton_OnClick(object sender, RoutedEventArgs e)
        {
            TextBox.Text = "Disabled";
            source.Cancel();
            source.Dispose();

            source = new CancellationTokenSource();
            token = source.Token;
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type != UpdateType.Message) return;
            if (update.Message!.Type != MessageType.Text) return;

            Debug.WriteLine($"Received a '{update.Message.Text}' message in chat {update.Message.Chat.Id}.");

            try
            {
                await _context.Chats.AddAsync(new Chat(update.Message.Chat.Id, update.Message.Chat.Username, update.Message.Chat.FirstName, update.Message.Chat.LastName));
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                Debug.WriteLine(e);
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
    }
}