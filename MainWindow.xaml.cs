using MusicPlayer.Enum;
using System.Windows;

namespace MusicPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MinimizeButton.Click += (s, e) =>
            {
                this.WindowState = WindowState.Minimized;
            };

            ExitButton.Click += (s, e) =>
            {
                switch (App.Current.Settings.ExitMode)
                {
                    case ExitModeEnum.Minimize:
                        this.WindowState = WindowState.Minimized;
                        break;
                    case ExitModeEnum.Close:
                        this.Close();
                        break;
                }
            };
        }
    }
}