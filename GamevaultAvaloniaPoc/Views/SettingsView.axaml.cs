using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Notifications;
using Avalonia.Interactivity;
using GamevaultAvaloniaPoc.Helper;
using GamevaultAvaloniaPoc.ViewModels;
using System;
using System.Reactive;
using Notification = Avalonia.Controls.Notifications.Notification;

namespace GamevaultAvaloniaPoc.Views
{
    public partial class SettingsView : UserControl
    {
        public SettingsView()
        {
            InitializeComponent();
            this.DataContext = SettingsViewModel.Instance;
            SettingsViewModel.Instance.ServerUrl = "https://gamevault.platform.alfagun74.de/";
            SettingsViewModel.Instance.Username = "devuser";
            SettingsViewModel.Instance.Password = "123456789";
        }
        private async void Connect_Click(object sender, RoutedEventArgs e)
        {
            ((Button)sender).IsEnabled = false;
            LoginState state = await LoginManager.Instance.ManualLogin(SettingsViewModel.Instance.Username, SettingsViewModel.Instance.Password);
            Notification not = new Notification("Message", "Already logged in", NotificationType.Information);
            if (!LoginManager.Instance.IsLoggedIn())
            {
                if (LoginState.Success == state)
                {
                    not = new Notification("Message", $"Successfully logged in as '{SettingsViewModel.Instance.Username}'", NotificationType.Success);
                }
                else if (LoginState.Unauthorized == state)
                {
                    not = new Notification("Message", "Login failed. Username or password was not correct", NotificationType.Warning);
                }
                else if (LoginState.Forbidden == state)
                {
                    not = new Notification("Message", "Login failed. User is not activated. Contact an Administrator to activate the User.", NotificationType.Warning);
                }
                else if (LoginState.Error == state)
                {
                    not = new Notification("Message", "Could not connect to server", NotificationType.Warning);
                }
            }
            //var nm = new WindowNotificationManager(App.MainWindow)
            //{
            //    Position = NotificationPosition.BottomRight,
            //    MaxItems = 1
            //};
            //nm.TemplateApplied += (sender, args) =>
            //{
            //    nm.Show(not);
            //};
            ((Button)sender).IsEnabled = true;
        }
    }
}
