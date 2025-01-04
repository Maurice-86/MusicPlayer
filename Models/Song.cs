using MusicPlayer.Enum;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MusicPlayer.Models
{
    public class Song : INotifyPropertyChanged
    {
        private TimeSpan position;
        public TimeSpan Position
        {
            get => position;
            set
            {
                if (position != value)
                {
                    position = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Artist { get; set; }
        public string? Album { get; set; }
        public TimeSpan Duration { get; set; }
        public OriginEnum Origin { get; set; }
        public string? Path { get; set; }

        /// <summary>
        /// 辅助属性：区分用户拖动和自动更新的情况
        /// </summary>
        public bool IsDragging { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
