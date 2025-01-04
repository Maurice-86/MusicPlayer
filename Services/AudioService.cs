using MusicPlayer.Helps;
using MusicPlayer.Models;
using NAudio.Wave;
using System.IO;

namespace MusicPlayer.Services
{
    public class AudioService : IDisposable
    {
        private AudioFileReader? audioFile;
        private WaveOutEvent? outputDevice;

        private Timer? timer;
        public event Action<Song>? OnSongInfoChanged;
        public event Action<PlaybackState>? OnPlayStateChanged;

        public void Play()
        {
            if (outputDevice != null && outputDevice.PlaybackState == PlaybackState.Paused)
            {
                OnPlayStateChanged?.Invoke(PlaybackState.Playing);
                outputDevice.Play();
                return;
            }

            CommonPlay();
        }

        public void PlayPrev()
        {
            var song = PlayListHelp.GetCurrentSong();

            PlayListHelp.SetCurrentIdxPrev();
            CommonPlay();

            if (song != null)
                song.Position = TimeSpan.Zero;
        }

        public void PlayNext()
        {
            var song = PlayListHelp.GetCurrentSong();

            PlayListHelp.SetCurrentIdxNext();
            CommonPlay();

            if (song != null)
                song.Position = TimeSpan.Zero;
        }

        public void AutoPlayNext()
        {
            PlayListHelp.AutoSetCurrentIdx();
            CommonPlay();
        }

        public void Pause()
        {
            outputDevice?.Pause();  // 暂停音乐
            OnPlayStateChanged?.Invoke(PlaybackState.Paused);
        }

        public void Stop()
        {
            outputDevice?.Stop();    // 停止音乐 
            OnPlayStateChanged?.Invoke(PlaybackState.Stopped);
        }

        public void SetPositionAndPlay(TimeSpan position)
        {
            var playList = PlayListHelp.GetPlayList();
            if (playList == null) return;

            PlayListHelp.SetPosition(position);

            if (outputDevice?.PlaybackState == PlaybackState.Playing)
            {
                Pause();

                var song = PlayListHelp.GetCurrentSong();
                if (song == null) return;

                // 设置播放位置
                audioFile!.CurrentTime = song.Position;

                outputDevice.Play();    // 异步执行    

                OnPlayStateChanged?.Invoke(PlaybackState.Playing);
            }
        }

        public void SetVolume(float volume)
        {
            if (outputDevice == null) return;
            outputDevice.Volume = volume;
        }

        public void Dispose()
        {
            outputDevice?.Dispose();
            audioFile?.Close();
        }

        /// <summary>
        /// paly palynext palyprev autoplaynext 公共的部分
        /// </summary>
        private void CommonPlay()
        {
            var playList = PlayListHelp.GetPlayList();
            if (playList == null) return;

            var song = PlayListHelp.GetCurrentSong();
            if (song == null || !File.Exists(song.Path)) return;

            Stop();

            audioFile?.Close();
            audioFile = new AudioFileReader(song.Path);

            // 设置播放位置
            audioFile.CurrentTime = song.Position;

            if (outputDevice == null)
            {
                outputDevice = new WaveOutEvent();
                outputDevice.Volume = App.Current.Settings.Volume;
            }

            outputDevice.Init(audioFile);

            if (timer == null)
            {
                timer = new Timer(UpdateSongInfo, null, 0, 500);    // 定时器
            }
            else
            {
                // 重新启动计时器
                timer?.Change(0, 500); // 从现在开始，每500毫秒触发一次
            }

            outputDevice.Play();    // 异步执行    

            OnPlayStateChanged?.Invoke(PlaybackState.Playing);
        }

        private void UpdateSongInfo(object? state)
        {
            var song = PlayListHelp.GetCurrentSong();
            if (song == null) return;

            if (outputDevice?.PlaybackState == PlaybackState.Playing)
            {
                song.Position = GetPosition();
            }
            else
            {
                if (outputDevice?.PlaybackState == PlaybackState.Stopped)
                {
                    // 停止计时器
                    timer?.Change(Timeout.Infinite, 0);
                    song.Position = TimeSpan.Zero;
                    AutoPlayNext(); // 自动播放下一首
                }
            }
            OnSongInfoChanged?.Invoke(song);
        }

        private TimeSpan GetPosition()
        {
            if (audioFile == null)
                return TimeSpan.Zero;

            return TimeSpan.FromSeconds(audioFile.Position / audioFile.WaveFormat.AverageBytesPerSecond);
        }
    }
}

