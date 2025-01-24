using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MusicPlayer.Models
{
    public class Song : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Artist { get; set; } = string.Empty;
        private TimeSpan position = TimeSpan.Zero;
        public TimeSpan Position
        {
            get => position;
            set
            {
                if (position == value) return;
                position = value;
                OnPropertyChanged();
            }
        }
        public TimeSpan Duration { get; set; } = TimeSpan.Zero;
        public string? FilePath { get; set; }

        /// <summary>
        /// 辅助属性：播放次数，方便随机播放
        /// </summary>
        [JsonIgnore] // 忽略序列化
        public int PlayCount { get; set; }

        /// <summary>
        /// 辅助属性：区分用户拖动和自动更新的情况
        /// </summary>
        [JsonIgnore] // 忽略序列化
        public bool IsDragging { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
