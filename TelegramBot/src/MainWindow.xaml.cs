using DesktopApp.Models.Services;
using System.Windows;
using System.Windows.Input;

namespace DesktopApp;

public partial class MainWindow : Window
{
    private readonly IBotService _botService;
   

    public MainWindow(IBotService botService)
    {
        InitializeComponent();

        _botService = botService;
    }

    private void RunButton_OnClick(object sender, RoutedEventArgs e)
    {
        _botService.RunBot();
        StopButton.IsEnabled = true;
        RunButton.IsEnabled = false;
        StateLabel.Content = "Bot Runned";
    }

    private void StopButton_OnClick(object sender, RoutedEventArgs e)
    {
        _botService.StopBot();
        StopButton.IsEnabled = false;
        RunButton.IsEnabled = true;
        StateLabel.Content = "Bot Stopped";
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