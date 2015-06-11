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
        SongMessage son;
        LrcPower power;
        Stopwatch watch;

        public MainWindow()
        {
            InitializeComponent();
            InitTimer();
            watch = new Stopwatch();
            son = MusicCalc.GetSong(@"C:\Users\Admin\Desktop\mp3\亲爱的那不是爱情.mp3");
            this.TotalTime.Text = son.MusicTime;
            player = new MusicPlayer();
            player.Prepare(son);
            var tuple =new Tuple<object,object,object,double>(PlayProcess,SongImageEllipse,NowTime,son.MusicSecond);
            this.power = new LrcPower(watch, @"C:\Users\Admin\Desktop\mp3\lyric\亲爱的那不是爱情.lrc", LyrContainer, tuple);
 }

        /// <summary>
        /// 初始化定时器
        /// </summary>
        private void InitTimer()
        {
            timer = new Timer();
            timer.Elapsed += new ElapsedEventHandler(TimerInvoke);
            timer.Interval = 500;
        }

        private void TimerInvoke(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(power.BeginAnimate));
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
            player.PlayToggle(timer, power,watch);
        }


    }
}
