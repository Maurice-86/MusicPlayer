using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MusicPlayer.Enums;
using MusicPlayer.Helps;
using MusicPlayer.Models;
using MusicPlayer.Services;
using System.Collections.ObjectModel;

namespace MusicPlayer.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        private readonly NavigationService navigationService;
        private readonly AudioService audioService;

        public MainViewModel(NavigationService navigationService, AudioService audioService, PlayerViewModel playerViewModel)
        {
            MenuItems =
            [
                new MenuItem { Icon = "Home", Name = "Home"},
                new MenuItem { Icon = "Home", Name = "Home"},
                new MenuItem { Icon = "Home", Name = "Home"},
                new MenuItem { Icon = "FolderOutline", Name = "本地音乐", ViewModelType = typeof(LocalMusicViewModel)},
                new MenuItem { Icon = "AccountArrowUp", Name = "插件管理"},
                new MenuItem { Icon = "History", Name = "历史播放"}
            ];

            this.navigationService = navigationService;
            this.navigationService.CurrentViewModelChanged += () =>
            {
                CurrentViewModel = this.navigationService.CurrentViewModel;
            };

            this.audioService = audioService;

            PlayerViewModel = playerViewModel;
            playerViewModel.PlaylistVisibilityChanged += (isVisible) =>
            {
                IsPlaylistVisible = isVisible;
                Playlist = PlaylistHelp.GetPlaylist();
            };
        }

        [ObservableProperty]
        private Song? selectedItem;

        [RelayCommand]
        private void RowDoubleClick()
        {
            if (SelectedItem == null) return;

            audioService.Play(PlaybackOperation.PlayById, id: SelectedItem.Id);
        }

        [RelayCommand]
        private void Remove(Song? song)
        {
            if (song == null)
            {
                audioService.Stop();
                PlaylistHelp.ClearPlaylist();
            }
            else
            {
                if (PlaylistHelp.RemoveSong(song.Id))
                {
                    PlaylistHelp.UpdateIndex(PlaybackOperation.PlayRemove);
                    audioService.Stop();
                }
            }
            Playlist = PlaylistHelp.GetPlaylist();
        }

        [ObservableProperty]
        private bool isPlaylistVisible;

        [ObservableProperty]
        private Playlist? playlist;

        [ObservableProperty]
        private ViewModelBase? currentViewModel;

        [ObservableProperty]
        private ViewModelBase? playerViewModel;

        [ObservableProperty]
        private ObservableCollection<MenuItem>? menuItems;

        [RelayCommand]
        private void NavigateToView(MenuItem? menuItem)
        {
            if (menuItem != null && menuItem.ViewModelType != null)
            {
                navigationService.NavigateTo(menuItem.ViewModelType);
            }
        }

        [RelayCommand]
        private void GoSettings()
        {
            navigationService.NavigateTo(typeof(SettingsViewModel));
        }
    }
}
