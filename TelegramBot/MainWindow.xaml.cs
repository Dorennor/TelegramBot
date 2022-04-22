using DesktopApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using DesktopApp.Models.Database;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Chat = DesktopApp.Models.Entities.Chat;

namespace DesktopApp
{
    public partial class MainWindow : Window
    {
        private static readonly TelegramBotClient botClient = new TelegramBotClient("5262349068:AAHNdyvZ2Hjji7nScbyq9V-c79w39FcTuC4");
        private CancellationTokenSource source;
        private CancellationToken token;
        private TGBotDbContext _context;

        public MainWindow()
        {
            InitializeComponent();

            RunButton.IsEnabled = true;
            StopButton.IsEnabled = false;
            StateLabel.Content = "Disabled";

            source = new CancellationTokenSource();
            token = source.Token;

            _context = new TGBotDbContext();
            _context.Database.Migrate();
            _context.Chats.Load();
            BindComboBox();
        }

        private void BindComboBox()
        {
            var chats = _context.Chats;
            List<string> bindList = new List<string>();
            foreach (var chat in chats)
            {
                bindList.Add(chat.ChatId + (chat.Username != null ? " | " + chat.Username + " | " : " | ") + (chat.FirstName != null ? chat.FirstName + " " : "") + (chat.LastName != null ? chat.LastName : ""));
            }

            ComboBox.ItemsSource = bindList;
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
            if (update.Message!.Type != MessageType.Audio) return;

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

        private async void SendMessageButton_OnClick(object sender, RoutedEventArgs e)
        {
            var cancelationToken = new CancellationTokenSource().Token;
            if (ComboBox.SelectedItem != null)
            {
                var chatId = Convert.ToInt64(ComboBox.SelectedItem.ToString()?.Split("|").First());
                await botClient.SendTextMessageAsync(chatId, $"{_context.Chats.First(u => u.ChatId == chatId).FirstName}, привет!", cancellationToken: cancelationToken);
            }
        }

        private void OpenFileButton_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == false) return;

            var tfile = TagLib.File.Create(openFileDialog.FileName);
            string title = tfile.Tag.Title;
            TimeSpan duration = tfile.Properties.Duration;
            Debug.WriteLine($"Title: {title}, duration: {duration}");
        }
    }
}