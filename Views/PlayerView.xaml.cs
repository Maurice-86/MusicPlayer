using Microsoft.Extensions.DependencyInjection;
using MusicPlayer.ViewModels;
using System.Windows.Controls;
using System.Windows.Input;

namespace MusicPlayer.Views
{
    /// <summary>
    /// PlayerView.xaml 的交互逻辑
    /// </summary>
    public partial class PlayerView : UserControl
    {
        private readonly PlayerViewModel viewModel;
        public PlayerView()
        {
            InitializeComponent();
            viewModel = App.Current.Services.GetRequiredService<PlayerViewModel>();
        }

        // 鼠标按下时开始拖动
        private void ProgressBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var song = viewModel.CurrentSong;
            if (song == null) return;

            song.IsDragging = true;

            // 捕获鼠标，确保鼠标离开进度条时仍然能接收到鼠标事件
            Mouse.Capture(progressBar);
            UpdateProgressBarValue(e.GetPosition(progressBar).X); // 更新进度条
        }

        // 鼠标移动时更新进度
        private void ProgressBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.Captured == progressBar) // 只有在鼠标被捕获时才处理
            {
                UpdateProgressBarValue(e.GetPosition(progressBar).X); // 更新进度条
            }
        }

        // 鼠标释放时停止拖动
        private void ProgressBar_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // 释放鼠标捕获
            Mouse.Capture(null);

            var song = viewModel.CurrentSong;
            if (song == null) return;

            song.IsDragging = false;
            viewModel.UpdateAudioFileCurrentTime();
        }

        // 更新进度条值
        private void UpdateProgressBarValue(double positionX)
        {
            var song = viewModel.CurrentSong;
            if (song == null) return;

            var progressBarWidth = progressBar.ActualWidth;
            var max = progressBar.Maximum;
            var min = progressBar.Minimum;

            // 根据点击位置计算新值
            var newValue = (positionX / progressBarWidth) * (max - min) + min;
            newValue = Math.Min(Math.Max(0, newValue), song.Duration.TotalSeconds);

            viewModel.ProgressBarPosition = TimeSpan.FromSeconds(newValue);
        }
    }
}
