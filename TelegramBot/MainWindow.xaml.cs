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

namespace TelegramBot
{
    public partial class MainWindow : Window
    {
        private TelegramBotClient botClient;
        private CancellationTokenSource source;
        private CancellationToken token;

        public MainWindow()
        {
            InitializeComponent();
            TextBox.Text = "Disabled";
            botClient = new TelegramBotClient("5262349068:AAHNdyvZ2Hjji7nScbyq9V-c79w39FcTuC4");
            source = new CancellationTokenSource();
            token = source.Token;
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

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type != UpdateType.Message) return;
            if (update.Message!.Type != MessageType.Text) return;

            Debug.WriteLine($"Received a '{update.Message.Text}' message in chat {update.Message.Chat.Id}.");

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

        private void StopButton_OnClick(object sender, RoutedEventArgs e)
        {
            TextBox.Text = "Disabled";
            source.Cancel();
            source.Dispose();

            source = new CancellationTokenSource();
            token = source.Token;
        }
    }
}