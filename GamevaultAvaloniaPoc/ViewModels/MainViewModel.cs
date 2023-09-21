using GamevaultAvaloniaPoc.Models;

namespace GamevaultAvaloniaPoc.ViewModels;

internal class MainViewModel : ViewModelBase
{
	#region Singleton
	private static MainViewModel instance = null;
	private static readonly object padlock = new object();

	public static MainViewModel Instance
	{
		get
		{
			lock (padlock)
			{
				if (instance == null)
				{
					instance = new MainViewModel();
				}
				return instance;
			}
		}
	}
	#endregion

	private User? usericon { get; set; }
	internal User? UserIcon
	{
		get
		{
			return usericon;
		}
		set
		{
			usericon = value;
			OnPropertyChanged();
		}
	}
}
