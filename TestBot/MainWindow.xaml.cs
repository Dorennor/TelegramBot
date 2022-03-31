﻿using System;
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
            botClient = new TelegramBotClient("5262349068:AAHNdyvZ2Hjji7nScbyq9V-c79w39FcTuC4");
            source = new CancellationTokenSource();
            token = source.Token;
        }

        private void RunButton_OnClick(object sender, RoutedEventArgs e)
        {
            TextBox.Text = "Enabled";
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }
            };

            botClient.StartReceiving(HandleUpdateAsync, HandleErrorAsync, receiverOptions, token);
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type != UpdateType.Message) return;
            if (update.Message!.Type != MessageType.Text) return;

            MessageBox.Show($"{update.Message.Text}", "Message");

            await botClient.SendTextMessageAsync(update.Id,
                $"{update.Message.Chat.Id}\n{update.Message.Chat.Username}\n{update.Message.Chat.FirstName}\n{update.Message.Chat.LastName}\n{update.Message.Text}", cancellationToken:cancellationToken);
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
            TextBox.Text = "Disabled";
            source.Cancel();
            source.Dispose();

            source = new CancellationTokenSource();
            token = source.Token;
        }
    }
}