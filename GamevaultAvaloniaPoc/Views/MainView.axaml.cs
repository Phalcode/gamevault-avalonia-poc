using Avalonia.Controls;
using Avalonia.Interactivity;
using GamevaultAvaloniaPoc.ViewModels;

namespace GamevaultAvaloniaPoc.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
        this.DataContext = MainViewModel.Instance;
    }  
}
