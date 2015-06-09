using Music.Lyric;
using Music.Model;
using Music.MusicPush;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Music
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        private Timer timer;
        private delegate void UpdateProcess();
        private MusicPlayer player;
        private Stopwatch watch;
        SongMessage son;
        DoubleAnimation animate;
        Storyboard sb;
        Lrc lrc;
        int time = 0;

        public MainWindow()
        {
            InitializeComponent();
            InitTimer();
            son = MusicCalc.GetSong(@"C:\Users\Admin\Desktop\mp3\李易峰 - 剑伤.mp3");
            this.TotalTime.Text = son.MusicTime;
            this.SongName.Text = son.Title + " — " + son.Artist;
            player = new MusicPlayer();
            player.Prepare(son);
            InitLyr();
        }

        private void InitLyr()
        {
            lrc = Lrc.InitLrc(@"C:\Users\Admin\Desktop\mp3\lyric\剑伤.lrc");
             int i = 0;
             foreach (var value in lrc.LrcWord.Values)
              {
                  TextBox text = new TextBox();
                text.Style = (System.Windows.Style)this.FindResource("LyrWord");               
                text.Text = value;
                this.LyrContent.Children.Add(text);
                i++;
            }
        }

        /// <summary>
        /// 初始化定时器
        /// </summary>
        private void InitTimer()
        {
            sb = new Storyboard();
            animate = new DoubleAnimation(0, 360, new Duration(TimeSpan.FromSeconds(40)));
            animate.RepeatBehavior = RepeatBehavior.Forever;
            sb.Children.Add(animate);
            Storyboard.SetTargetProperty(animate,new PropertyPath("(UIElement.RenderTransform).(RotateTransform.Angle)"));


            watch = new Stopwatch();
            timer = new Timer();
            timer.Elapsed += new ElapsedEventHandler(TimerUse);
            timer.Interval = 500;
        }


        private void TimerUse(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,new Action(UpdatePlayUI));
        }

        /// <summary>
        /// 更新时间和进度条
        /// </summary>
        /// <param name="model"></param>
        private void UpdatePlayUI()
        {
            this.PlayProcess.Value = watch.Elapsed.TotalSeconds * 100 / son.MusicSecond;
            this.NowTime.Text = MusicCalc.GetSecond(watch.Elapsed.TotalSeconds);
            if (lrc.LrcWord.Keys.Skip(time).First() < watch.ElapsedMilliseconds)
            {
                if (time == 0)
                {
                    LineAnimation(LyrContent.Children[time] as TextBox);
                }
                else
                {
                    LineAnimation(LyrContent.Children[time] as TextBox);
                    LineEndAnimate(LyrContent.Children[time - 1] as TextBox);
                }
                LrcAnimation();
                time++;
            }
        }

        /// <summary>
        /// 歌词动画
        /// </summary>
        /// <param name="List"></param>
        private void LrcAnimation()
        {
            TranslateTransform tt = new TranslateTransform();
            double marginTop =(LyrContent as StackPanel).Margin.Top;
            ThicknessAnimation lrcAnimate = new ThicknessAnimation(new Thickness(0, marginTop, 0, 0), new Thickness(0, marginTop-44, 0, 0), new Duration(TimeSpan.FromSeconds(1.5)));
            LyrContent.RenderTransform = tt;
            LyrContent.BeginAnimation(MarginProperty, lrcAnimate);
        }

        /// <summary>
        /// 改变字体
        /// </summary>
        /// <param name="sender"></param>
        private void LineAnimation(TextBox sender)
        {
            TranslateTransform tt = new TranslateTransform();
            DoubleAnimation animate = new DoubleAnimation(sender.FontSize,30,new Duration(TimeSpan.FromSeconds(2)));
            sender.RenderTransform = tt;
            sender.BeginAnimation(FontSizeProperty, animate);
        }

        private void LineEndAnimate(TextBox sender)
        {
            TranslateTransform tt = new TranslateTransform();
            DoubleAnimation animate = new DoubleAnimation(sender.FontSize,22,new Duration(TimeSpan.FromSeconds(2)));
            sender.RenderTransform = tt;
            sender.BeginAnimation(FontSizeProperty, animate);
        }

        /// <summary>
        /// 歌曲图片旋转动画
        /// </summary>
        /// <param name="signal"></param>
        private void Animation(int signal)
        {
            if (signal == 1)
            {
                if ((this.SongImageEllipse.RenderTransform as RotateTransform).Angle > 0)
                {
                    sb.Resume(this.SongImageEllipse);
                }
                else
                {
                    sb.Begin(this.SongImageEllipse, true);
                }
            }
            else
            {
                sb.Pause(this.SongImageEllipse);
            }
        }

        /// <summary>
        /// 界面顶部拖拽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Top_Drag(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        /// <summary>
        /// 关闭或最小化主窗口事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeWindowState(object sender, MouseButtonEventArgs e)
        {
            if ((sender as Image).Name == "MinWindow")
            {
                this.WindowState= WindowState.Minimized;
            }
            else if ((sender as Image).Name == "CloseWindow")
            {
                Application.Current.Shutdown();
            }
        }

        /// <summary>
        /// 播放或者停止音乐
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayToggle(object sender, MouseButtonEventArgs e)
        {
            Image model = sender as Image;
            if (model.Source.ToString().Split('/').Last() == "play_on.png")
            {
                model.Source = new BitmapImage(new Uri("../../Images/pause_on.png", UriKind.Relative));
            }
            else
            {
                model.Source = new BitmapImage(new Uri("../../Images/play_on.png", UriKind.Relative));
            }
            player.PlayToggle(timer, watch, Animation);
        }


    }
}
