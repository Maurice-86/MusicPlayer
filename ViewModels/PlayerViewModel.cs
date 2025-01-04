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

            CurrentSong = PlayListHelp.GetCurrentSong();
            ProgressBarPosition = CurrentSong == null ? TimeSpan.Zero : CurrentSong.Position;

            this.audioService.OnSongInfoChanged += (song) =>
            {
                CurrentSong = song;
                if (!CurrentSong.IsDragging)
                {
                    ProgressBarPosition = CurrentSong.Position;
                }
            };

            this.audioService.OnPlayStateChanged += (state) =>
            {
                IsPlaying = state == PlaybackState.Playing;
            };

            this.localMusicViewModel = localMusicViewModel;
            this.localMusicViewModel.RowDoubleClicked += OnLocalMusicRowDoubleClicked;

        }

        [ObservableProperty]
        private bool isPlaying;

        // 事件处理方法
        private void OnLocalMusicRowDoubleClicked()
        {
            switch (App.Current.Settings.RowDoubleClickedMode)
            {
                case RowDoubleClickedModeEnum.ReplaceCurrentPlayList:
                    audioService.Stop();
                    Task.Run(() => audioService.Play());
                    break;
                case RowDoubleClickedModeEnum.AddToPlayList:
                    break;
            }
        }

        [RelayCommand]
        private void Excute(string action)
        {
            switch (action)
            {
                case "Play":
                    audioService.Play();
                    break;
                case "Pause":
                    audioService.Pause();
                    break;
            }
        }

        [RelayCommand]
        private void StepForward()
        {
            audioService.PlayNext();
        }

        [RelayCommand]
        private void StepBackward()
        {
            audioService.PlayPrev();
        }

        public void SetPosition()
        {
            audioService.SetPositionAndPlay(ProgressBarPosition);
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
                    audioService.SetVolume(volume);
                }
            }
        }
    }
}

