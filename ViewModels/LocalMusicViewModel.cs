using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MusicPlayer.Enum;
using MusicPlayer.Helps;
using MusicPlayer.Models;
using System.Collections.ObjectModel;
using System.IO;

namespace MusicPlayer.ViewModels
{
    public partial class LocalMusicViewModel : ViewModelBase
    {
        [ObservableProperty]
        private ObservableCollection<Song> songs = [];

        public LocalMusicViewModel()
        {
            LoadSongs();
        }

        [RelayCommand]
        private void LoadSongs()
        {
            // 从本地音乐库中加载音乐项
            // 将加载的音乐项添加到 Songs 集合中
            var folder = @"C:\Users\wengym\Music\MusicFreeData";
            var files = Directory.GetFiles(folder)
                                 .Where(file => file.EndsWith(".mp3") || file.EndsWith(".m4a") || file.EndsWith(".flac") || file.EndsWith(".aac") || file.EndsWith(".wav") || file.EndsWith(".ogg") || file.EndsWith(".wma") || file.EndsWith(".aiff") || file.EndsWith(".ape") || file.EndsWith(".mpc") || file.EndsWith(".mp2") || file.EndsWith(".mp1") || file.EndsWith(".tta") || file.EndsWith(".wv") || file.EndsWith(".dsd") || file.EndsWith(".dsf") || file.EndsWith(".opus") || file.EndsWith(".mka") || file.EndsWith(".m3u") || file.EndsWith(".pls") || file.EndsWith(".cue") || file.EndsWith(".m3u8") || file.EndsWith(".m3u_generator"))
                                 .ToList();

            Songs.Clear();

            int id = 0;
            foreach (var file in files)
            {
                var audio = TagLib.File.Create(file);

                var Song = new Song
                {
                    Id = id++,
                    Title = audio.Tag.Title,
                    Artist = audio.Tag.FirstPerformer,
                    Album = audio.Tag.Album,
                    Duration = audio.Properties.Duration,
                    Source = AudioSource.Local,
                    Path = file
                };
                Songs.Add(Song);
            }
        }

        [ObservableProperty]
        private int selectedIndex;

        [ObservableProperty]
        private Song? selectedItem;

        // 定义事件，供 PlayerViewModel 订阅
        public event Action? RowDoubleClicked;

        [RelayCommand]
        private void RowDoubleClick()
        {
            if (SelectedItem == null) return;

            switch (App.Current.Settings.PlaylistAddMode)
            {
                case PlaylistAddMode.Replace:
                    PlaylistHelper.UpdatePlaylist(new Playlist
                    {
                        Index = SelectedIndex,
                        Songs = [.. Songs]
                    });
                    break;
                case PlaylistAddMode.Append:
                    PlaylistHelper.AddSong(SelectedItem);
                    break;
            }

            RowDoubleClicked?.Invoke();
        }
    }
}


