using MusicPlayer.Enum;

namespace MusicPlayer.Models
{
    public class Settings
    {
        public PlayModeEnum PlayMode { get; set; }
        public ExitModeEnum ExitMode { get; set; }
        public RowDoubleClickedModeEnum RowDoubleClickedMode { get; set; }
        public PlayList? PlayList { get; set; }
        public float Volume { get; set; } = 0.25f;
    }
}
