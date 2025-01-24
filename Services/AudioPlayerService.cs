using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSCore;
using CSCore.Codecs;
using CSCore.SoundOut;
using MusicPlayer.Enum;
using MusicPlayer.Models;

namespace MusicPlayer.Services
{
    public class AudioPlayerService
    {
        private ISoundOut? soundOut; // 声音输出
        private IWaveSource? waveSource; // 音频流
        private Timer? timer;   // 定时器
        public event Action<PlaybackState>? OnPlaybackStateChanged;
        public event Action<Song?>? OnSongChanged;

        private readonly PlaylistService _playlistService;

        public AudioPlayerService(PlaylistService playlistService)
        {
            _playlistService = playlistService;
        }

        public void Play(PlaybackOperation operation, int id = -1, bool isUpdatePosition = false)
        {
            Song? song = null;
            switch (operation)
            {
                case PlaybackOperation.Normal:
                    if (soundOut?.PlaybackState == PlaybackState.Paused)
                    {
                        // 重新启动计时器
                        timer?.Change(0, 500); // 从现在开始，每500毫秒触发一次
                        soundOut.Play();
                        OnPlaybackStateChanged?.Invoke(PlaybackState.Playing);
                        return;
                    }
                    break;
                default:
                    song = _playlistService.Get();
                    if (song != null) song.Position = TimeSpan.Zero;
                    break;
            }
            // 更新 Index
            _playlistService.UpdateIndex(operation, id);
            // 获得歌曲
            song = _playlistService.Get();
            if (song == null) return;
            
            // 音频流
            waveSource?.Dispose();
            waveSource = CodecFactory.Instance.GetCodec(song.FilePath);
            if (isUpdatePosition) waveSource.SetPosition(song.Position);

            // 声音输出
            if (soundOut == null) soundOut = new WasapiOut();

            if (soundOut.PlaybackState != PlaybackState.Stopped) Stop();
            soundOut.Initialize(waveSource);    // 确保处于 Stopped 状态
            soundOut.Volume = _playlistService.Volume;
            soundOut.Play();    // 开始播放

            // 定时器
            if (timer == null)
            {
                timer = new Timer(OnTimerTick, null, 0, 500);
            }
            else
            {
                timer?.Change(0, 500);  // 重新启动定时器
            }

            OnPlaybackStateChanged?.Invoke(PlaybackState.Playing);
        }

        public void Pause()
        {
            soundOut?.Pause();
            timer?.Change(Timeout.Infinite, 0); // 停止定时器
            OnPlaybackStateChanged?.Invoke(PlaybackState.Paused);
        }

        /// <summary>
        /// 调整音量
        /// </summary>
        /// <param name="volume"></param>
        public void UpdateVolume(float volume)
        {
            if (soundOut == null) return;
            soundOut.Volume = volume;
            _playlistService.Volume = volume;
        }

        private void Stop()
        {
            timer?.Change(Timeout.Infinite, 0); // 先停止定时器
            soundOut?.Stop();
            OnPlaybackStateChanged?.Invoke(PlaybackState.Stopped);
        }

        /// <summary>
        /// 更新音频流位置
        /// </summary>
        /// <param name="postion"></param>
        /// <returns>
        /// false: 在暂停时更新
        /// true: 在播放时更新
        /// </returns>
        public bool UpdateWaveSourcePosition(TimeSpan postion)
        {
            var song = _playlistService.Get();
            if (song != null)
            {
                song.Position = postion;
                waveSource?.SetPosition(postion);
            }
            return soundOut?.PlaybackState == PlaybackState.Playing;
        }

        /// <summary>
        /// 定时器的回调函数
        /// </summary>
        /// <param name="state"></param>
        private void OnTimerTick(object? state)
        {
            var song = _playlistService.Get();
            if (song != null)
            {
                var diff = song.Position.TotalSeconds - song.Duration.TotalSeconds;
                // 歌曲播放结束
                if (diff > 0 || soundOut?.PlaybackState == PlaybackState.Stopped)
                {
                    song.PlayCount++;
                    Play(PlaybackOperation.Auto);
                }
                else if (soundOut?.PlaybackState == PlaybackState.Playing)
                {
                    try
                    {
                        if (waveSource != null)
                            song.Position = waveSource.GetPosition();
                    }
                    catch (NullReferenceException ex)
                    {
                        // 记录错误信息
                        Console.WriteLine("NullReferenceException in Timer_Tick: " + ex.Message);
                    }
                }
            }
            OnSongChanged?.Invoke(song);
        }

        public void Dispose()
        {
            Stop();
            soundOut?.Dispose();
            waveSource?.Dispose();
            timer?.Dispose();
        }
    }
}
