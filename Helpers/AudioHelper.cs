using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagLib;
namespace MusicPlayer.Helpers
{
    public static class AudioHelper
    {
        /// <summary>
        /// 解析音频文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static (string Title, string Artist, TimeSpan Duration) ParseAudioFile(string filePath)
        {
            // 使用音频解析库（如NAudio等）解析音频文件
            // 返回歌名、歌手和时长
            // 示例代码，具体实现取决于你使用的音频解析库
            TagLib.File file = TagLib.File.Create(filePath);
            var title = file.Tag.Title;
            var artist = file.Tag.FirstPerformer;
            var duration = file.Properties.Duration; // 获取音频文件的时长
            return (title, artist, duration);
        }
    }
}
