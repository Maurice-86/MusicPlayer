using MusicPlayer.Enums;
using MusicPlayer.Models;

namespace MusicPlayer.Helps
{
    public static class PlaylistHelp
    {
        public static Playlist? _playlist;

        public static void UpdatePlaylist(Playlist? playlist)
        {
            _playlist = playlist;
        }

        public static void AddSong(Song song)
        {
            _playlist?.Songs?.Add(song);
        }

        /// <summary>
        /// 删除歌曲
        /// </summary>
        /// <param name="id"></param>
        /// <returns>isCurrentSongRemoved: 删除的歌曲是否是当前的歌曲</returns>
        public static bool RemoveSong(int id)
        {
            bool isCurrentSongRemoved = false;
            var songToRemove = _playlist?.Songs?.FirstOrDefault(s => s.Id == id);
            if (songToRemove != null)
            {
                var index = _playlist?.Songs?.IndexOf(songToRemove) ?? -1;
                isCurrentSongRemoved = _playlist!.Index == index;
                
                if (_playlist!.Index >= index)
                {
                    _playlist!.Index--;
                }
                _playlist!.Songs!.RemoveAt(index);
                
            }
            return isCurrentSongRemoved;
        }

        public static void UpdateIndex(PlaybackOperation operation, int id = -1)
        {
            if (operation == PlaybackOperation.Play)
            {
                return;
            }
            else if (operation == PlaybackOperation.PlayRemove)
            {
                _playlist!.Index = -1;
                return;
            }
            else if (operation == PlaybackOperation.PlayById)
            {
                var song = _playlist?.Songs?.FirstOrDefault(s => s.Id == id);
                if (song != null)
                {
                    _playlist!.Index = _playlist?.Songs?.IndexOf(song) ?? -1;
                }
                return;
            }

            var count = _playlist?.Songs?.Count ?? 0;
            if (count == 0) return;

            var mode = App.Current.Settings.PlayMode;
            var index = _playlist!.Index;
            if (operation == PlaybackOperation.PlayPrevious)
                _playlist.Index = (index - 1 + count) % count;
            else if (operation == PlaybackOperation.PlayNext || (operation == PlaybackOperation.AutoPlayNext && mode == PlayMode.Loop))
                _playlist.Index = (index + 1) % count;
            else if (operation == PlaybackOperation.AutoPlayNext)
            {
                if (mode == PlayMode.Random)
                {
                    // TODO
                }
                // mode == Single
            }
        }

        public static void UpdateCurrentSongTime(TimeSpan time)
        {
            var song = GetCurrentSong();
            if (song != null)
            {
                song.CurrentTime = time;
            }
        }

        public static Playlist? GetPlaylist() => _playlist;

        public static Song? GetCurrentSong()
            => _playlist?.Songs?.ElementAtOrDefault(_playlist.Index);

        public static void ClearPlaylist()
        {
            _playlist!.Index = -1;
            _playlist?.Songs?.Clear();
        }
    }
}
