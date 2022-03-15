using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TestBot
{
    public partial class MainWindow : Window
    {
        private TelegramBotClient botClient;
        private CancellationTokenSource source;
        private CancellationToken token;

        public MainWindow()
        {
            InitializeComponent();
            botClient = new TelegramBotClient("5262349068:AAHPDBCb-eNQJr-Q6CRVUdfkSzg3KNtlH4s");
            source = new CancellationTokenSource();
            token = source.Token;
        }

        private async void RunButton_OnClick(object sender, RoutedEventArgs e)
        {
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }
            };

            botClient.StartReceiving(HandleUpdateAsync, HandleErrorAsync, receiverOptions, source.Token);

            var me = await botClient.GetMeAsync();

            MessageBox.Show($"Start listening for @{me.Username}", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type != UpdateType.Message) return;
            if (update.Message!.Type != MessageType.Text) return;

            var chatId = update.Message.Chat.Id;
            var messageText = update.Message.Text;

            Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

            Message sentMessage = await botClient.SendTextMessageAsync(chatId: chatId, text: "You said:\n" + messageText, cancellationToken: cancellationToken);
        }

        private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        private void StopButton_OnClick(object sender, RoutedEventArgs e)
        {
            source.Cancel();
            source.Dispose();

            source = new CancellationTokenSource();
            token = source.Token;
        }
    }
}