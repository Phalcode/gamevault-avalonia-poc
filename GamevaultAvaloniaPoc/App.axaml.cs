using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

using GamevaultAvaloniaPoc.ViewModels;
using GamevaultAvaloniaPoc.Views;

namespace GamevaultAvaloniaPoc;

public partial class App : Application
{
	public static Window MainWindow;
	public override void Initialize()
	{
		AvaloniaXamlLoader.Load(this);
	}

	public override void OnFrameworkInitializationCompleted()
	{
		if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
		{
			desktop.MainWindow = new MainWindow();
			MainWindow = desktop.MainWindow;
		}
		else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
		{
			singleViewPlatform.MainView = new MainView
			{
				DataContext = MainViewModel.Instance
			};
		}

		base.OnFrameworkInitializationCompleted();
	}
}
