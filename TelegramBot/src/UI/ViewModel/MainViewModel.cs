using DesktopApp.UI.Core;

namespace DesktopApp.UI.ViewModel;

public class MainViewModel : ObservableObject
{
#pragma warning disable
    public RelayCommand StatisticsViewCommand { get; set; }
    public RelayCommand HomeViewCommand { get; set; }

    public HomeViewModel HomeVM { get; set; }
    public StatisticsViewModel StatisticsVM { get; set; }

    private object _currentView;

    public object CurrentView {
        get => _currentView;
        set {
            _currentView = value;
            OnPropertyChanged();
        }
    }

    public MainViewModel()
    {
        HomeVM = new HomeViewModel();
        StatisticsVM = new StatisticsViewModel();

        CurrentView = HomeVM;

        HomeViewCommand = new RelayCommand(o =>
        {
            CurrentView = HomeVM;
        });
        StatisticsViewCommand = new RelayCommand(o =>
        {
            CurrentView = StatisticsVM;
        });
    }

#pragma warning restore
}