using CommunityToolkit.Mvvm.ComponentModel;
using MaterialDesignThemes.Wpf;
using MusicPlayer.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using MusicPlayer.Extensions;

namespace MusicPlayer.Services
{
    public class MessageService : INotifyPropertyChanged
    {
        private MessageService() { }
        public static MessageService Instance { get; } = new();

        private bool messageActive;
        public bool MessageActive
        {
            get => messageActive;
            set
            {
                if (messageActive == value) return;
                messageActive = value;
                OnPropertyChanged();
            }
        }

        public SnackbarMessageQueue MessageQueue { get; } = new();

        public void ShowMessage(string message)
        {
            // 发送消息并添加手动关闭按钮
            MessageQueue.Enqueue(
                content: message,       // 消息内容
                actionContent: new PackIcon
                {
                    Kind = PackIconKind.Close, // 关闭图标
                    Width = 18,
                    Height = 18,
                    Foreground = ((SolidColorBrush)App.Current.Resources["WhiteForeground"]).Invert(),
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                },
                actionHandler: (param) =>
                {
                    // 点击按钮后的操作：手动关闭 Snackbar
                    MessageActive = false;
                },
                actionArgument: null,
                promote: false,
                neverConsiderToBeDuplicate: false,
                durationOverride: TimeSpan.FromSeconds(1)
            );
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
