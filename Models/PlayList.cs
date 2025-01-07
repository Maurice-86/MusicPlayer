using System.Collections.ObjectModel;

namespace MusicPlayer.Models
{
    public class PlayList
    {
        public int CurrentIdx { get; set; } = -1;
        public ObservableCollection<Song>? Songs { get; set; } = [];
    }
}
