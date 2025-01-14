using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MusicPlayer.Enum;
using MusicPlayer.Helps;
using MusicPlayer.Models;
using MusicPlayer.Services;
using NAudio.Wave;

namespace MusicPlayer.ViewModels
{
    public partial class PlayerViewModel : ViewModelBase
    {
        private readonly AudioService audioService;
        private readonly LocalMusicViewModel localMusicViewModel;

        public PlayerViewModel(AudioService audioService, LocalMusicViewModel localMusicViewModel)
        {
            this.audioService = audioService;

            CurrentSong = PlaylistHelper.GetCurrentSong();
            ProgressBarPosition = CurrentSong?.CurrentTime ??TimeSpan.Zero;

            this.audioService.OnSongInfoChanged += (song) =>
            {
                CurrentSong = song;
                if (CurrentSong != null && !CurrentSong.IsDragging)
                {
                    ProgressBarPosition = CurrentSong.CurrentTime;
                }
            };

            this.audioService.OnPlayStateChanged += (state) =>
            {
                IsPlaying = state == PlaybackState.Playing;
            };

            this.localMusicViewModel = localMusicViewModel;
            this.localMusicViewModel.RowDoubleClicked += OnLocalMusicRowDoubleClicked;

        }

        public event Action<bool>? PlaylistVisibilityChanged;
        private bool isPlaylistVisible;

        [RelayCommand]
        private void TogglePlaylist()
        {
            isPlaylistVisible = !isPlaylistVisible;
            PlaylistVisibilityChanged?.Invoke(isPlaylistVisible);
        }

        [ObservableProperty]
        private bool isPlaying;

        // 事件处理方法
        private void OnLocalMusicRowDoubleClicked()
        {
            switch (App.Current.Settings.PlaylistAddMode)
            {
                case PlaylistAddMode.Replace:
                    audioService.Play(PlaybackOperation.Play);
                    break;
                case PlaylistAddMode.Append:
                    break;
            }
        }

        [RelayCommand]
        private void Excute(string action)
        {
            switch (action)
            {
                case "Play":
                    audioService.Play(PlaybackOperation.Play, isUpdateCurrentTime: true);
                    break;
                case "Pause":
                    audioService.Pause();
                    break;
            }
        }

        [RelayCommand]
        private void StepForward()
        {
            audioService.Play(PlaybackOperation.PlayNext);
        }

        [RelayCommand]
        private void StepBackward()
        {
            audioService.Play(PlaybackOperation.PlayPrevious);
        }

        public void UpdateAudioFileCurrentTime()
        {
            audioService.UpdateAudioFileCurrentTime(ProgressBarPosition);
        }

        [ObservableProperty]
        private Song? currentSong;

        [ObservableProperty]
        private TimeSpan progressBarPosition;

        private float volume = App.Current.Settings.Volume;

        private int volumeInt;
        public int VolumeInt
        {
            get => (int)(volume * 100);
            set
            {
                if (SetProperty(ref volumeInt, value))
                {
                    volume = value / 100.0f;
                    audioService.UpdateVolume(volume);
                }
            }
        }
    }
}

