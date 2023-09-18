using Avalonia.Controls;
using GamevaultAvaloniaPoc.ViewModels;

namespace GamevaultAvaloniaPoc.Views;

public partial class MainView : UserControl
{
    private MainViewModel ViewModel { get; set; }
    public MainView()
    {
        InitializeComponent();
        ViewModel = new MainViewModel();
        this.DataContext = ViewModel;
    }
}
