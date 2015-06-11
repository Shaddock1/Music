using Music.Lyric;
using Music.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Media;

namespace Music.MusicPush
{
    /// <summary>
    /// 音乐播放
    /// </summary>
    internal class MusicPlayer
    {
        MediaPlayer player =new MediaPlayer();
        SongMessage song;
        static bool IsPlaying;

        public void Prepare(SongMessage model)
        {
            song = model;
            IsPlaying = false;
            player.Open(new Uri(model.SongPath));
        }

        public void PlayToggle(Timer timer,LrcPower model,Stopwatch watch)
        {
            if (!IsPlaying)
            {
                if (watch.IsRunning == false)
                {
                    watch.Start();
                }
                    timer.Start();
                    player.Play();
                    IsPlaying = true;
            }
            else
            {
                watch.Stop();
                   player.Pause();
                   IsPlaying = false;
                   timer.Stop();
                   model.StopAnimate();
            }

        }

    }
}
