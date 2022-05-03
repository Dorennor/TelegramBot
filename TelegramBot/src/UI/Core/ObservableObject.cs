using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DesktopApp.UI.Core;

public class ObservableObject : INotifyPropertyChanged
{
#pragma warning disable

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

#pragma warning restore
}