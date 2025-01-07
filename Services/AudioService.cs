using MusicPlayer.Helps;
using MusicPlayer.Models;
using NAudio.Gui;
using NAudio.Wave;
using System.IO;

namespace MusicPlayer.Services
{
    public class AudioService : IDisposable
    {
        private AudioFileReader? audioFile;
        private WaveOutEvent? outputDevice;

        private Timer? timer;
        public event Action<Song?>? OnSongInfoChanged;
        public event Action<PlaybackState>? OnPlayStateChanged;

        public void Play()
        {
            var song = PlayListHelp.GetCurrentSong();
            if (song == null) return;

            if (outputDevice?.PlaybackState == PlaybackState.Paused)
            {

                // 重新启动计时器
                timer?.Change(0, 500); // 从现在开始，每500毫秒触发一次
                outputDevice.Play();
                OnPlayStateChanged?.Invoke(PlaybackState.Playing);
                return;
            }

            CommonPlay(isSetCurrentTime: true);
        }

        public void PlayPrev()
        {
            // SetCurrentSongPosition: Music Free 的逻辑
            PlayListHelp.SetCurrentSongPosition(TimeSpan.Zero);
            PlayListHelp.SetCurrentIdx(isPrev: true);
            CommonPlay();
        }

        public void PlayNext()
        {
            PlayListHelp.SetCurrentSongPosition(TimeSpan.Zero);
            PlayListHelp.SetCurrentIdx(isNext: true);
            CommonPlay();
        }

        public void PlayById(int id)
        {
            PlayListHelp.SetCurrentSongPosition(TimeSpan.Zero);
            PlayListHelp.SetCurrentIdx(id: id);
            CommonPlay();
        }

        public void AutoPlayNext()
        {
            PlayListHelp.AutoSetCurrentIdx();
            CommonPlay();
        }

        public void Pause()
        {
            // 停止计时器
            timer?.Change(Timeout.Infinite, 0);
            outputDevice?.Pause();  // 暂停音乐
            OnPlayStateChanged?.Invoke(PlaybackState.Paused);
        }

        public void Stop()
        {
            // 停止计时器
            timer?.Change(Timeout.Infinite, 0);
            outputDevice?.Stop();    // 停止音乐 
            OnPlayStateChanged?.Invoke(PlaybackState.Stopped);
        }

        public void SetAudioFileCurrentTime(TimeSpan timeSpan)
        {
            var song = PlayListHelp.GetCurrentSong();
            if (song == null) return;

            song.Position = timeSpan;
            PlayListHelp.SetCurrentSongPosition(timeSpan);

            if (audioFile != null)
            {
                // 设置播放位置
                audioFile!.CurrentTime = song.Position;
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
        private void CommonPlay(bool isSetCurrentTime = false)
        {
            // ToDo: 有些歌即使指定了位置但还是从头开始播放
            
            var song = PlayListHelp.GetCurrentSong();
            if (song == null) return;

            Stop();     // Pause 或不处理会触发 outputDevice Init 异常

            audioFile?.Close();
            audioFile = new AudioFileReader(song.Path);

            if (isSetCurrentTime)
            {
                // 设置播放位置
                audioFile.CurrentTime = song.Position;
            }

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
            if (song != null)
            {
                if (outputDevice?.PlaybackState == PlaybackState.Playing)
                {
                    song.Position = GetPosition();
                }
                else
                {
                    if (outputDevice?.PlaybackState == PlaybackState.Stopped)
                    {
                        if (Math.Abs(song.Position.TotalMilliseconds - song.Duration.TotalMilliseconds) < 1000)
                        {
                            song.Position = TimeSpan.Zero;
                            AutoPlayNext(); // 自动播放下一首
                        }
                    }
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

