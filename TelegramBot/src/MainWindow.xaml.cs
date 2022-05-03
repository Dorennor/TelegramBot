using DesktopApp.Models.Controllers;
using System.Windows;
using System.Windows.Input;

namespace DesktopApp;

public partial class MainWindow : Window
{
    private readonly BotController _botController;

    public MainWindow()
    {
        InitializeComponent();
        _botController = new BotController();
    }

    private void RunButton_OnClick(object sender, RoutedEventArgs e)
    {
        StateLabel.Content = "Enabled";
        _botController.RunBot();
    }

    private void StopButton_OnClick(object sender, RoutedEventArgs e)
    {
        StateLabel.Content = "Disabled";
        _botController.StopBot();
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