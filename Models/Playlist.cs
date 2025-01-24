using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicPlayer.Enum;

namespace MusicPlayer.Models
{
    public class Playlist
    {
        public int Index { get; set; }
        public float Volume { get; set; }
        /// <summary>
        /// 播放模式
        /// </summary>
        public PlaybackMode PlaybackMode { get; set; } 
        public List<Song> Songs { get; set; } = [];
    }
}
