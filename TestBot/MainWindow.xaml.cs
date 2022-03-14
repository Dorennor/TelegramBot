using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TestBot
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_OnClick(object sender, RoutedEventArgs e)
        {
            var botClient = new TelegramBotClient("5262349068:AAHPDBCb-eNQJr-Q6CRVUdfkSzg3KNtlH4s");
            var me = await botClient.GetMeAsync();
            
        }
    }
}