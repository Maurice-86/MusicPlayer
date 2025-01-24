using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.Constants
{
    public static class FileConstants
    {
        public static string PlaylistFilePath { get; } = Path.Combine(AppContext.BaseDirectory, "playlist.json");
        public static string SettingsFilePath { get; } = Path.Combine(AppContext.BaseDirectory, "settings.json");
    }
}
