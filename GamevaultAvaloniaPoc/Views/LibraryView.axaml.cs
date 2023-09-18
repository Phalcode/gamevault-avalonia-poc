using Avalonia.Controls;
using GamevaultAvaloniaPoc.ViewModels;

namespace GamevaultAvaloniaPoc.Views
{
    public partial class LibraryView : UserControl
    {
        private LibraryViewModel ViewModel { get; set; }
        public LibraryView()
        {
            InitializeComponent();
            ViewModel = new LibraryViewModel();
            this.DataContext = ViewModel;
        }
    }
}
