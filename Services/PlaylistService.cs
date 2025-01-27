using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicPlayer.Enum;
using MusicPlayer.Models;
using MusicPlayer.Utils;

namespace MusicPlayer.Services
{
    public class PlaylistService
    {
        public PlaylistService()
        {
            // 初始化
            Initialize();
        }

        private void Initialize()
        {
            // 初始化
            var playlist = JsonUtils<Playlist>.ReadInfoFromJson(Constants.FileConstants.PlaylistFilePath);
            if (playlist != null)
            {
                var count = playlist.Songs.Count;
                int startIndex = 0;
                for (int i = count - 1; i >= 0; i--)
                {
                    if (!File.Exists(playlist.Songs[i].FilePath))
                    {
                        playlist.Songs.RemoveAt(i);
                        startIndex = i;
                    }
                }
                if (count != playlist.Songs.Count)
                {
                    for (int i = startIndex; i < playlist.Songs.Count; i++) playlist.Songs[i].Id = i;
                }
                Index = playlist.Index;
                Volume = playlist.Volume;
                PlaybackMode = playlist.PlaybackMode;
                Songs = [.. playlist.Songs];
            }
        }

        /// <summary>
        /// 保存信息到本地
        /// </summary>
        public void SaveInfoToJson()
        {
            var playlist = new Playlist()
            {
                Index = Index,
                Volume = Volume,
                PlaybackMode = PlaybackMode,
                Songs = [.. Songs]
            };
            JsonUtils<Playlist>.SaveInfoToJson(playlist, Constants.FileConstants.PlaylistFilePath);
        }

        private int index;
        public int Index
        {
            get => index;
            set
            {
                if (index == value) return;
                index = value;
                OnIndexChanged?.Invoke();   // 唤起委托 
            }
        }
        public float Volume { get; set; }
        public PlaybackMode PlaybackMode { get; set; }
        public ObservableCollection<Song> Songs { get; set; } = [];
        public event Action? OnIndexChanged;

        /// <summary>
        /// 根据索引获得 Song
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Song? Get(int? index = null)
        {
            var i = index == null ? Index : index.Value;
            if (i < 0 || i >= Songs.Count) return null;
            return Songs[i];
        }

        public void Add(Song song)
        {
            InitializeStrings(out HashSet<string> strings);
            string key = song.Title + song.Artist + song.Duration.TotalSeconds;
            if (strings.Contains(key)) return;

            song.Id = Songs.Count;
            Songs.Add(song);
        }

        public int Add(IEnumerable<Song> songs)
        {
            InitializeStrings(out HashSet<string> strings);
            int count = 0;
            foreach (var song in songs)
            {
                string key = song.Title + song.Artist + song.Duration.TotalSeconds;
                if (strings.Contains(key)) continue;

                song.Id = Songs.Count;
                Songs.Add(song);
                strings.Add(key);
                count++;
            }
            return count;
        }

        public bool Remove(int id)
        {
            if (id < 0 || id >= Songs.Count) return false;
            var song = Songs[id];
            if (song.Id != id) return false;

            Songs.RemoveAt(id);
            AssignSongIds(startIndex: id);
            return true;
        }

        /// <summary>
        /// 根据 PlaybackOperation 修改 Index
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="id"></param>
        public void UpdateIndex(PlaybackOperation operation, int id = -1)
        {
            if (operation == PlaybackOperation.Normal) return;
            else if (operation == PlaybackOperation.Removed)
            {
                Index = -1;
                return;
            }
            else if (operation == PlaybackOperation.ById)
            {
                if (id < 0 || id >= Songs.Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(id), "ID 超出范围");
                }
                var song = Songs[id];
                if (song.Id != id)
                {
                    throw new Exception("歌曲ID不匹配");
                }
                Index = id;
                return;
            }
            // 防止除以0
            if (Songs.Count == 0) return;   

            if (operation == PlaybackOperation.Previous)
                Index = (index - 1 + Songs.Count) % Songs.Count;
            else if (operation == PlaybackOperation.Next)
                Index = (index + 1) % Songs.Count;
            else if (operation == PlaybackOperation.Auto)
            {
                if (PlaybackMode == PlaybackMode.Ordered)
                    Index = (index + 1) % Songs.Count;
                else if (PlaybackMode == PlaybackMode.Random)
                {
                    // 计算总权重
                    double totalWeight = Songs.Sum(song => CalculateWeight(song.PlayCount));
                    // 生成随机数
                    double randomValue = new Random().NextDouble() * totalWeight;
                    // 累计权重，找到对应的歌曲索引
                    double cumulativeWeight = 0;
                    for (int i = 0; i < Songs.Count; i++)
                    {
                        cumulativeWeight += CalculateWeight(Songs[i].PlayCount);
                        if (randomValue <= cumulativeWeight)
                        {
                            Index = i;
                            break;
                        }
                    }
                }
            }
        }

        private static double CalculateWeight(int playCount)
        {
            // 权重计算公式：播放次数越多，权重越小
            // 这里使用一个简单的反比例公式，可以根据需要调整
            return 1.0 / (playCount + 1);
        }

        /// <summary>
        /// 为歌曲分配ID
        /// </summary>
        private void AssignSongIds(int startIndex = 0)
        {
            for (int i = startIndex; i < Songs.Count; i++) Songs[i].Id = i;
        }

        /// <summary>
        /// 初始化 hashset
        /// </summary>
        /// <param name="strings"></param>
        private void InitializeStrings(out HashSet<string> strings)
        {
            strings = [];
            foreach (var song in Songs)
            {
                string key = song.Title + song.Artist + song.Duration.TotalSeconds;
                strings.Add(key);
            }
        }
    }
}
