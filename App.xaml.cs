using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Media;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using MusicPlayer.Extensions;
using MusicPlayer.Helpers;
using MusicPlayer.Services;
using MusicPlayer.ViewModels;

namespace MusicPlayer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider Services { get; }
        public static new App Current => (App)Application.Current;

        public App()
        {
            var container = new ServiceCollection();
            container.AddSingleton<PlaylistService>();
            container.AddSingleton<AudioPlayerService>();
            container.AddSingleton<MainWindow>();
            container.AddSingleton<MainViewModel>();

            Services = container.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var mainWindow = Services.GetRequiredService<MainWindow>();
            ThemeHelper.SwitchTheme(SettingsService.Instance.Model.ThemeMode == Enum.ThemeMode.Dark);
            mainWindow.Show();
            MessageService.Instance.ShowMessage("欢迎");
        }

        protected override void OnExit(ExitEventArgs e)
        {
            var audioPlayerService = Services.GetRequiredService<AudioPlayerService>();
            var playlistService = Services.GetRequiredService<PlaylistService>();
            audioPlayerService.Dispose();
            playlistService.SaveInfoToJson();
            SettingsService.Instance.SaveInfoToJson();
            base.OnExit(e);
        }
    }
}
