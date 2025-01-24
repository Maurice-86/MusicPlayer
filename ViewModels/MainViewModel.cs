using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MusicPlayer.Enum;
using MusicPlayer.Models;
using MusicPlayer.Services;

namespace MusicPlayer.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        private readonly PlaylistService _playlistService;
        private readonly AudioPlayerService _audioPlayer;
        public MainViewModel(PlaylistService playlistService, AudioPlayerService audioPlayer)
        {
            _playlistService = playlistService;
            _audioPlayer = audioPlayer;
            Index = _playlistService.Index;
            volume = playlistService.Volume;
            Songs = _playlistService.Songs;
            if (Index < Songs.Count) Song = Songs[Index];
            if (Song != null) SliderValue = Song.Position.TotalSeconds;

            _playlistService.OnIndexChanged += () =>
            {
                Index = _playlistService.Index;
            };

            _audioPlayer.OnPlaybackStateChanged += (state) =>
            {
                IsPlaying = state == CSCore.SoundOut.PlaybackState.Playing;
            };
            _audioPlayer.OnSongChanged += (song) =>
            {
                Song = song;
                // 自动更新
                if (Song != null && !Song.IsDragging)
                {
                    SliderValue = Song.Position.TotalSeconds;
                }
            };
        }


        [ObservableProperty]
        private int index;

        private double sliderValue;
        public double SliderValue
        {
            get => sliderValue;
            set => SetProperty(ref sliderValue, value);
        }

        private float volume;   // 0.0 ~ 1.0
        private int volumeInt;
        public int VolumeInt
        {
            get => (int)(volume * 100);
            set
            {
                if (SetProperty(ref volumeInt, value))
                {
                    volume = value / 100.0f;
                    _audioPlayer.UpdateVolume(volume);
                }
            }
        }


        [ObservableProperty]
        private bool isPlaying;

        [ObservableProperty]
        private ObservableCollection<Song> songs;

        [ObservableProperty]
        private Song? song;

        [ObservableProperty]
        private Song? selectedItem;

        /// <summary>
        /// 播放上一首
        /// </summary>
        [RelayCommand]
        private void StepBackward()
        {
            _audioPlayer.Play(PlaybackOperation.Previous);
        }

        private bool isFirstTime = true;
        [RelayCommand]
        private void Excute(string action = "")
        {
            switch (action)
            {
                case "Play":
                    _audioPlayer.Play(PlaybackOperation.Normal, isUpdatePosition: isFirstTime);
                    isFirstTime = false;
                    break;
                case "Pause":
                    _audioPlayer.Pause();
                    break;
            }
        }

        /// <summary>
        /// 播放下一首
        /// </summary>
        [RelayCommand]
        private void StepForward()
        {
            _audioPlayer.Play(PlaybackOperation.Next);
        }

        /// <summary>
        /// 更新音频流位置
        /// </summary>
        public void UpdateWaveSourcePosition()
        {
            var postion = TimeSpan.FromSeconds(SliderValue);
            if (!_audioPlayer.UpdateWaveSourcePosition(postion))
            {
                isFirstTime = true;
            }
        }

        /// <summary>
        /// 双击 SelectedItem
        /// </summary>
        public void RowDoubleClick()
        {
            if (SelectedItem != null)
                _audioPlayer.Play(PlaybackOperation.ById, id: SelectedItem.Id);
        }
    }
}
