using Music.Process;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Music.Lyric
{
    /// <summary>
    /// 歌词动画
    /// </summary>
    internal class LrcPower
    {
        public Lrc Lrc {private set; get; }
        public StackPanel LyrContainer {private set; get; }
        private Stopwatch watch;
        private int time = 0;
        private DoubleAnimation fontBegin, fontEnd;
        private ColorAnimation colorBegin, colorEnd;
        private ThicknessAnimation tickAnimate;
        private PlayProcess process;
        private SolidColorBrush beginBrush,endBrush;
        private Duration duration = new Duration(TimeSpan.FromSeconds(1));

        public LrcPower(Stopwatch watch ,string lrcAddress, object lrcContainer,Tuple<object,object,object,double> tuple):
        this(watch,Lrc.InitLrc(lrcAddress),lrcContainer,tuple)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="lrc"></param>
        /// <param name="lrcContainer"></param>
        public LrcPower(Stopwatch wat,Lrc lrc, object lrcContainer,Tuple<object,object,object,double> tuple)
        {
            this.Lrc = lrc;
            this.watch = wat;
            this.LyrContainer = lrcContainer as StackPanel;
            fontBegin = new DoubleAnimation(29, duration);
            fontEnd = new  DoubleAnimation(22, duration);
            tickAnimate = new ThicknessAnimation();
            this.process = new PlayProcess(tuple.Item1,tuple.Item2,tuple.Item3,tuple.Item4);
            AddLyc();
            InitColor();
        }

        private void InitColor()
        {
            beginBrush = new SolidColorBrush();
            endBrush = new SolidColorBrush();

            colorBegin = new ColorAnimation();
            colorBegin.To = Colors.Orange;
            colorBegin.Duration = duration;


            colorEnd = new ColorAnimation();
            colorEnd.To = Colors.White;
            colorEnd.Duration = new Duration(TimeSpan.FromSeconds(0));
        }

        /// <summary>
        /// 添加歌词到容器中去
        /// </summary>
        private void AddLyc()
        {
            if (this.Lrc!=null && this.LyrContainer != null)
            {
                foreach (var text in Lrc.LrcWord.Values)
                {
                    TextBox box = new TextBox();
                    box.Style = (System.Windows.Style)LyrContainer.FindResource("LyrWord");
                    box.Text = text;
                    LyrContainer.Children.Add(box);
                }
            }
        }

        /// <summary>
        /// 开始动画
        /// </summary>
        public void BeginAnimate()
        {
            if (process.UpdateProcess(watch.Elapsed.TotalSeconds))
            {
                Finaliz(200);
            }
            else
            {
                ChangeStyle();
            }
        }

        /// <summary>
        /// 歌词
        /// </summary>
        private void ContainerTop()
        {
            double nowMarginTop = LyrContainer.Margin.Top;
            tickAnimate.From = new Thickness(0, nowMarginTop, 0, 0);
            tickAnimate.To = new Thickness(0, nowMarginTop - 42, 0, 0);
            tickAnimate.Duration = duration;
            LyrContainer.BeginAnimation(FrameworkElement.MarginProperty, tickAnimate);
        }


        /// <summary>
        /// 暂停动画
        /// </summary>
        public void StopAnimate()
        {
            process.StopProcess();
        }

        /// <summary>
        /// 销毁当前歌词
        /// </summary>
        /// <param name="initContainerMarginTop"></param>
        public void Finaliz(double initContainerMarginTop)
        {
            LyrContainer.Children.Clear();
            LyrContainer.SetValue(FrameworkElement.MarginProperty,new Thickness(0,initContainerMarginTop,0,0));
            process.Finaliz();
        }

        /// <summary>
        /// 改变当前行的样式
        /// </summary>
        private void ChangeStyle()
        {
            if (time >= Lrc.LrcWord.Count())
            {
                return;
            }
            if (Lrc.LrcWord.Keys.Skip(time).First() < watch.ElapsedMilliseconds)
            {
                if (time == 0)
                {
                    LineBeginAnimate(LyrContainer.Children[time] as TextBox);
                }
                else
                {
                    LineBeginAnimate(LyrContainer.Children[time] as TextBox);
                    LineEndAnimatie(LyrContainer.Children[time - 1] as TextBox);
                }
                ContainerTop();
                time++;
            }
        }

        /// <summary>
        /// 文字变大
        /// </summary>
        /// <param name="sender"></param>
        private  void LineBeginAnimate(TextBox sender)
        {
            fontBegin.From = sender.FontSize;
            colorBegin.From =((SolidColorBrush)sender.Foreground).Color;
            sender.BeginAnimation(Control.FontSizeProperty, fontBegin);
            sender.Foreground = beginBrush;
            beginBrush.BeginAnimation(SolidColorBrush.ColorProperty, colorBegin);
        }

        /// <summary>
        /// 文字变小
        /// </summary>
        /// <param name="sender"></param>
        private void LineEndAnimatie(TextBox sender)
        {
            fontEnd.From = sender.FontSize;
            colorEnd.From = ((SolidColorBrush)sender.Foreground).Color;
            sender.BeginAnimation(Control.FontSizeProperty, fontEnd);
            sender.Foreground = endBrush;
            endBrush.BeginAnimation(SolidColorBrush.ColorProperty, colorEnd);
        }

    }
}
