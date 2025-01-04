using System.Collections.ObjectModel;

namespace MusicPlayer.Models
{
    public class PlayList
    {
        public int CurrentIdx { get; set; }
        public ObservableCollection<Song>? Songs { get; set; }
    }
}
