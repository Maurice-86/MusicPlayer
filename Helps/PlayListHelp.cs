using MusicPlayer.Enum;
using MusicPlayer.Models;

namespace MusicPlayer.Helps
{
    public static class PlayListHelp
    {
        public static PlayList? _playList;

        public static void SetPlayList(PlayList? playList)
        {
            _playList = playList;
        }

        public static void AddSong(Song song)
        {
            _playList?.Songs?.Add(song);
        }

        public static void SetCurrentIdxPrev()
        {
            var count = _playList?.Songs?.Count ?? 0;
            if (count > 0)
            {
                var currentIdx = _playList!.CurrentIdx;
                _playList!.CurrentIdx = (currentIdx - 1 + count) % count;
            }
        }

        public static void SetCurrentIdxNext()
        {
            var count = _playList?.Songs?.Count ?? 0;
            if (count > 0)
            {
                var currentIdx = _playList!.CurrentIdx;
                _playList!.CurrentIdx = (currentIdx + 1) % count;
            }
        }

        public static void AutoSetCurrentIdx()
        {
            var mode = App.Current.Settings.PlayMode;
            switch (mode)
            {
                case PlayModeEnum.Loop:
                    var count = _playList?.Songs?.Count ?? 0;
                    if (count > 0)
                    {
                        var currentIdx = _playList!.CurrentIdx;
                        _playList!.CurrentIdx = (currentIdx + 1) % count;
                    }
                    break;
                case PlayModeEnum.Random:
                    // ToDo
                    break;
                case PlayModeEnum.Single:
                    break;
            }
        }

        public static void SetPosition(TimeSpan position)
        {
            var song = GetCurrentSong();
            if (song != null)
                song.Position = position;
        }

        public static PlayList? GetPlayList() => _playList;

        public static Song? GetCurrentSong() => _playList?.Songs?[_playList.CurrentIdx];
    }
}
