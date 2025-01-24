using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Media;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
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
            container.AddSingleton<SettingsService>();
            container.AddSingleton<MainWindow>();
            container.AddSingleton<MainViewModel>();

            Services = container.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var mainWindow = Services.GetRequiredService<MainWindow>();
            var settingsService = Services.GetRequiredService<SettingsService>();
            SwitchTheme(settingsService.Model.ThemeNumber == 0);
            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            var audioPlayerService = Services.GetRequiredService<AudioPlayerService>();
            var playlistService = Services.GetRequiredService<PlaylistService>();
            var settingsService = Services.GetRequiredService<SettingsService>(); 
            audioPlayerService.Dispose();
            playlistService.SaveInfoToJson();
            settingsService.SaveInfoToJson();
            base.OnExit(e);
        }

        public void SwitchTheme(bool isDarkTheme)
        {
            ModifyTheme(theme =>
            {
                if (isDarkTheme)
                {
                    theme.SetBaseTheme(BaseTheme.Dark);
                    // 更新按钮前景色资源
                    App.Current.Resources["WhiteForeground"] = new SolidColorBrush(Colors.White);
                }
                else
                {
                    theme.SetBaseTheme(BaseTheme.Light);
                    // 更新按钮前景色资源
                    App.Current.Resources["WhiteForeground"] = new SolidColorBrush(Colors.Black);
                }
            });
        }

        private static void ModifyTheme(Action<Theme> modificationAction)
        {
            var paletteHelper = new PaletteHelper();
            Theme theme = paletteHelper.GetTheme();
            modificationAction?.Invoke(theme);
            paletteHelper.SetTheme(theme);
        }

    }
}
