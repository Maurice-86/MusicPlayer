using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicPlayer.Models;
using MusicPlayer.Utils;

namespace MusicPlayer.Services
{
    public class SettingsService
    {
        public static SettingsService Instance { get; } = new();
        public Settings Model;

        private SettingsService()
        {
            Model = JsonUtils<Settings>.ReadInfoFromJson(Constants.FileConstants.SettingsFilePath) ?? new Settings();
        }

        public void SaveInfoToJson()
        {
            JsonUtils<Settings>.SaveInfoToJson(Model, Constants.FileConstants.SettingsFilePath);
        }
    }
}
