using System.Collections.ObjectModel;

namespace MusicPlayer.Models
{
    public class Playlist
    {
        public int Index { get; set; } = -1;
        public ObservableCollection<Song>? Songs { get; set; }
    }
}
