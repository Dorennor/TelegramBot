using DesktopApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Telegram.Bot;

namespace DesktopApp.View;

public partial class HomeView : UserControl
{
    private static readonly TelegramBotClient BotClient = new("5262349068:AAHNdyvZ2Hjji7nScbyq9V-c79w39FcTuC4");
    private readonly TgBotDbContext _context;

    public HomeView()
    {
        InitializeComponent();

        _context = new TgBotDbContext();
        _context.Database.Migrate();
        _context.Chats.Load();

        BindComboBox();
    }

    private void BindComboBox()
    {
        var chats = _context.Chats;
        List<string> bindList = new();
        foreach (var chat in chats)
        {
            bindList.Add(chat.ChatId + (chat.Username != null ? " | " + chat.Username + " | " : " | ") + (chat.FirstName != null ? chat.FirstName + " " : "") + (chat.LastName ?? ""));
        }

        ComboBox.ItemsSource = bindList;
        ComboBox.SelectedIndex = 0;
    }

    private async void SendMessageButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (SendTextTextBox.Text != string.Empty)
        {
            var cancelationToken = new CancellationTokenSource().Token;
            if (ComboBox.SelectedItem != null)
            {
                var chatId = Convert.ToInt64(ComboBox.SelectedItem.ToString()?.Split("|").First());
                await BotClient.SendTextMessageAsync(chatId, SendTextTextBox.Text, cancellationToken: cancelationToken);
                SendTextTextBox.Text = string.Empty;
            }
        }
        BindComboBox();
    }
}