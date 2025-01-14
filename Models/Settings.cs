using MusicPlayer.Enum;

namespace MusicPlayer.Models
{
    public class Settings
    {
        public PlayMode PlayMode { get; set; }
        public WindowExitMode WindowExitMode { get; set; }
        public PlaylistAddMode PlaylistAddMode { get; set; }
        public Playlist? Playlist { get; set; }
        public float Volume { get; set; } = 0.25f;
    }
}
