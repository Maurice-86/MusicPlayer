using MusicPlayer.Enum;
using MusicPlayer.Models;

namespace MusicPlayer.Helps
{
    public static class PlayListHelp
    {
        public static PlayList? _playList;

        public static void SetPlayList(PlayList playList)
        {
            _playList = playList;
        }

        public static void AddSong(Song song)
        {
            _playList?.Songs?.Add(song);
        }

        public static void DeleteSong(int id)
        {
            var songToRemove = _playList?.Songs?.FirstOrDefault(s => s.Id == id);
            if (songToRemove != null)
            {
                _playList!.Songs!.Remove(songToRemove);
            }
        }

        public static void SetCurrentIdx(int? id = null, bool isPrev = false, bool isNext = false)
        {
            if (id != null)
            {
                _playList!.CurrentIdx = id.Value;
                return;
            }

            var count = _playList?.Songs?.Count ?? 0;
            if (count > 0)
            {
                var currentIdx = _playList!.CurrentIdx;
                if (isPrev)
                    _playList.CurrentIdx = (currentIdx - 1 + count) % count;
                else if (isNext)
                    _playList.CurrentIdx = (currentIdx + 1) % count;
            }
        }

        public static void AutoSetCurrentIdx()
        {
            var mode = App.Current.Settings.PlayMode;
            switch (mode)
            {
                case PlayModeEnum.Loop:
                    SetCurrentIdx(isNext: true);
                    break;
                case PlayModeEnum.Random:
                    // ToDo
                    break;
                case PlayModeEnum.Single:
                    break;
            }
        }

        public static void SetCurrentSongPosition(TimeSpan position)
        {
            var song = GetCurrentSong();
            if (song != null)
            {
                song.Position = position;
            }
        }


        public static PlayList? GetPlayList() => _playList;

        public static Song? GetCurrentSong()
        {
            var song = _playList?.Songs?.FirstOrDefault(s => s.Id == _playList!.CurrentIdx);
            return song;
        }

        public static void ClearPlayList()
        {
            _playList!.CurrentIdx = -1;
            _playList?.Songs?.Clear();
        }
    }
}
