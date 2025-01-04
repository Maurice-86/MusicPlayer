using Microsoft.Extensions.DependencyInjection;
using MusicPlayer.Helps;
using MusicPlayer.Models;
using MusicPlayer.Services;
using MusicPlayer.ViewModels;
using System.Windows;

namespace MusicPlayer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static new App Current => (App)Application.Current;
        public IServiceProvider Services { get; }
        public Settings Settings { get; set; }

        public App()
        {
            var container = new ServiceCollection();
            container.AddSingleton<NavigationService>();
            container.AddSingleton<AudioService>();
            container.AddSingleton<MainWindow>();
            container.AddSingleton<MainViewModel>();
            container.AddSingleton<LocalMusicViewModel>();
            container.AddSingleton<SettingsViewModel>();
            container.AddSingleton<PlayerViewModel>();
            Services = container.BuildServiceProvider();

            Settings = SettingsHelp.LoadSettings();
            PlayListHelp.SetPlayList(Settings.PlayList);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainWindow = Services.GetRequiredService<MainWindow>();
            mainWindow.DataContext = Services.GetRequiredService<MainViewModel>();

            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            Settings.PlayList = PlayListHelp.GetPlayList();
            SettingsHelp.SaveSettings(Settings);
        }
    }

}
