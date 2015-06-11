using Music.MusicPush;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Music.Process
{
    internal class PlayProcess
    {
        private ProgressBar pbar;
        private Ellipse ellipse;
        private TextBlock text;
        private Storyboard sb;
        private DoubleAnimation animation;
        private double allSecond;
        private bool IsFirst = true;

        public PlayProcess(object bar,object elpse,object tx,double all)
        {
            this.pbar = bar as ProgressBar ;
            this.ellipse = elpse as Ellipse;
            this.text = tx as TextBlock;
            this.allSecond = all;
            InitAnimation();
        }

        /// <summary>
        /// 初始化动画
        /// </summary>
        private void InitAnimation()
        {
            sb = new Storyboard();
            animation = new DoubleAnimation(0, 360, new Duration(TimeSpan.FromSeconds(40)));
            animation.RepeatBehavior = RepeatBehavior.Forever;
            Storyboard.SetTargetProperty(animation, new PropertyPath("(UIElement.RenderTransform).(RotateTransform.Angle)"));
            sb.Children.Add(animation);
        }

        /// <summary>
        /// 更新进度条信息
        /// </summary>
        /// <param name="totalSeconds"></param>
        /// <param name="flag"></param>
        public bool  UpdateProcess(double totalSeconds)
        {
                if (totalSeconds > allSecond)
                {
                    return true;
                }
                if (IsFirst == true)
                {
                    sb.Begin(ellipse, true);
                    IsFirst = false;
                }
                else
                {
                    if (sb.GetIsPaused(ellipse) == true)
                    {
                        sb.Resume(ellipse);
                    }
                }
                this.text.Text = MusicCalc.GetSecond(totalSeconds);
                this.pbar.Value = totalSeconds * 100 / allSecond;
                return false;
        }

        /// <summary>
        /// 停止旋转
        /// </summary>
        public void StopProcess()
        {
            sb.Pause(ellipse);
        }

        public void Finaliz()
        {
            this.pbar.Value = 0;
            this.text.Text = "00:00";
            sb.Stop(ellipse);
        }

    }
}
