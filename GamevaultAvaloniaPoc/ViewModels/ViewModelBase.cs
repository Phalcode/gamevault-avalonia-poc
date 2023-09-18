using ReactiveUI;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GamevaultAvaloniaPoc.ViewModels;

public class ViewModelBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string name = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
