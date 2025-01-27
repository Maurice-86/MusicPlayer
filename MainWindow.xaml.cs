using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using MusicPlayer.Dialogs;
using MusicPlayer.Helpers;
using MusicPlayer.Models;
using MusicPlayer.Resources;
using MusicPlayer.Services;
using MusicPlayer.Utils;
using MusicPlayer.ViewModels;

namespace MusicPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel;
        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            originalWindowHeight = this.Height;
            _viewModel = viewModel;
            this.DataContext = viewModel;
        }

        private SettingsDialog? settingsDialog;

        /// <summary>
        /// 渲染窗口尺寸变化
        /// </summary>
        /// <param name="sizeInfo"></param>
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            // width属性可以通过 sizeInfo.NewSize.Width 获得
            if (settingsDialog != null)
            {
                settingsDialog.Width = sizeInfo.NewSize.Width * Constants.WindowRatioConstants.SettingsDialogWidthRatio;
                settingsDialog.Height = sizeInfo.NewSize.Height * Constants.WindowRatioConstants.SettingsDialogHeightRatio;
            }
        }

        /// <summary>
        /// 打开设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenSettingsDialogButton_Click(object sender, RoutedEventArgs e)
        {
            var identifier = "RootDialog";
            settingsDialog = new SettingsDialog(dialogIdentifier: identifier)
            {
                Width = this.Width * Constants.WindowRatioConstants.SettingsDialogWidthRatio,
                Height = this.Height * Constants.WindowRatioConstants.SettingsDialogHeightRatio,
                MaxHeight = 300,
                MinHeight = 200
            };
            MaterialDesignThemes.Wpf.DialogHost.Show(settingsDialog, identifier);
        }

        /// <summary>
        /// 最小化窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        // /// <summary>
        // /// 最大化窗口
        // /// </summary>
        // /// <param name="sender"></param>
        // /// <param name="e"></param>
        // private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        // {
        //     // this.WindowState = this.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
        // }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 拖动窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ColorZone_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        /// <summary>
        /// 导入歌曲
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImportSongsButton_Click(object sender, RoutedEventArgs e)
        {
            // 打开文件选择器
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "音频文件 (*.mp3;*.wav;*.flac)|*.mp3;*.wav;*.flac"; // 只允许选择音频文件
            openFileDialog.Multiselect = true; // 允许选择多个文件
            if (openFileDialog.ShowDialog() != true) return;

            // 处理选中的文件
            List<Song> songs = [];
            foreach (string filePath in openFileDialog.FileNames)
            {
                var (title, artist, duration) = AudioHelper.ParseAudioFile(filePath);
                Song song = new()
                {
                    Title = title,
                    Artist = artist,
                    Duration = duration,
                    FilePath = filePath
                };
                songs.Add(song);
            }

            var playlistService = App.Current.Services.GetRequiredService<PlaylistService>();
            var count = playlistService.Add(songs);
            MessageService.Instance.ShowMessage(string.Format(Lang.Snackbar_ImportSongsMessage, songs.Count, count));
        }

        private double originalWindowHeight;
        /// <summary>
        /// 展示歌词界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            var toggleButton = sender as ToggleButton;
            if (toggleButton?.IsChecked == true)
            {
                double targetHeight = this.Height - DrawerHost.ActualHeight;
                while (this.Height > targetHeight)
                {
                    this.Height -= Math.Min(20, this.Height - targetHeight); // 每次减少20
                }
            }
            else
            {
                double targetHeight = originalWindowHeight;
                while (this.Height < targetHeight)
                {
                    this.Height += Math.Min(20, targetHeight - this.Height); // 每次增加20
                }
            }
        }

        ///// <summary>
        ///// Slider 按下鼠标
        ///// </summary>
        //private void Slider_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (_viewModel.Song != null)
        //        _viewModel.Song.IsDragging = true;
        //}

        /// <summary>
        /// Slider 松开鼠标
        /// </summary>
        private void Slider_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _viewModel.UpdateWaveSourcePosition();
        }

        /// <summary>
        /// 开始拖动 Slider
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Slider_DragStarted(object sender, DragStartedEventArgs e)
        {
            if (_viewModel.Song != null)
                _viewModel.Song.IsDragging = true;
        }

        /// <summary>
        /// 结束拖动 Slider
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Slider_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            _viewModel.UpdateWaveSourcePosition();
            if (_viewModel.Song != null)
                _viewModel.Song.IsDragging = false;
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _viewModel.RowDoubleClick();
        }

        /// <summary>
        /// 当汉堡菜单按钮被选中时，滚动播放列表以显示当前播放的歌曲。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HamburgerMenuButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleButton { IsChecked: true } && _viewModel?.Song != null)
            {
                var item = PlayListView.Items[_viewModel.Song.Id];
                PlayListView.ScrollIntoView(item);
            }
        }
    }
}