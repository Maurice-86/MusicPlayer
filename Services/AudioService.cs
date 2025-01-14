using MusicPlayer.Enum;
using MusicPlayer.Helps;
using MusicPlayer.Models;
using NAudio.Wave;

namespace MusicPlayer.Services
{
    public class AudioService : IDisposable
    {
        private AudioFileReader? audioFile;
        private WaveOutEvent? outputDevice;
        private Timer? timer;
        public event Action<Song?>? OnSongInfoChanged;
        public event Action<PlaybackState>? OnPlayStateChanged;

        public void Play(PlaybackOperation operation, int id = -1, bool isUpdateCurrentTime = false)
        {
            switch (operation)
            {
                case PlaybackOperation.Play:
                    if (outputDevice?.PlaybackState == PlaybackState.Paused)
                    {
                        // 重新启动计时器
                        timer?.Change(0, 500); // 从现在开始，每500毫秒触发一次
                        outputDevice.Play();
                        OnPlayStateChanged?.Invoke(PlaybackState.Playing);
                        return;
                    }
                    break;
                default:
                    PlaylistHelper.UpdateCurrentSongTime(TimeSpan.Zero);
                    break;
            }
            PlaylistHelper.UpdateIndex(operation, id);

            // TODO: 有些歌即使指定了位置但还是从头开始播放

            var song = PlaylistHelper.GetCurrentSong();
            if (song == null) return;

            Stop();     // Pause 或不处理会触发 outputDevice Init 异常

            audioFile?.Dispose();
            audioFile = new AudioFileReader(song.Path);

            if (isUpdateCurrentTime)
            {
                // 设置播放位置
                audioFile.CurrentTime = song.CurrentTime;
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

        public void Pause()
        {
            // 停止计时器
            timer?.Change(Timeout.Infinite, 0);
            outputDevice?.Pause();  // 暂停音乐
            OnPlayStateChanged?.Invoke(PlaybackState.Paused);
        }

        public void Stop()
        {
            // // 停止计时器
            // timer?.Change(Timeout.Infinite, 0);
            outputDevice?.Stop();    // 停止音乐 
            OnPlayStateChanged?.Invoke(PlaybackState.Stopped);
        }

        public void UpdateAudioFileCurrentTime(TimeSpan time)
        {
            var song = PlaylistHelper.GetCurrentSong();
            if (song == null) return;

            song.CurrentTime = time;
            if (audioFile != null)
            {
                // 设置播放位置
                audioFile.CurrentTime = song.CurrentTime;
            }
        }

        public void UpdateVolume(float volume)
        {
            if (outputDevice == null) return;
            outputDevice.Volume = volume;
        }

        public void Dispose()
        {
            outputDevice?.Dispose();
            audioFile?.Dispose();
            timer?.Dispose();
        }

        private void UpdateSongInfo(object? state)
        {
            var song = PlaylistHelper.GetCurrentSong();
            if (song != null)
            {
                var diff = song.CurrentTime.TotalMilliseconds - song.Duration.TotalMilliseconds;
                if (diff >= 0 || outputDevice?.PlaybackState == PlaybackState.Stopped)
                {
                    // 自动播放下一首
                    Play(PlaybackOperation.AutoPlayNext);
                }
                else if (outputDevice?.PlaybackState == PlaybackState.Playing)
                {
                    song.CurrentTime = audioFile?.CurrentTime ?? TimeSpan.Zero;
                }
            }
            OnSongInfoChanged?.Invoke(song);
        }
    }
}

