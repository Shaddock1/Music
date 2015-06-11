using Music.Load;
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
    /// <summary>a
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
        LoadImage image;
        //托盘
        private System.Windows.Forms.NotifyIcon icon;

        public MainWindow()
        {
            InitializeComponent();
            son = MusicCalc.GetSong(@"C:\Users\Admin\Desktop\mp3\爱笑的眼睛.mp3");
            InitText();
            InitTimer();
            this.TotalTime.Text = son.MusicTime;
            player.Prepare(son);
            var tuple = new Tuple<Stopwatch, object, object, object, object, double>(watch, LyrContainer,PlayProcess, SongImageEllipse, NowTime, son.MusicSecond);
            this.power = new LrcPower(@"C:\Users\Admin\Desktop\mp3\lyric\爱笑的眼睛.lrc", tuple);

            InitText();
            InitTray();
 }

        /// <summary>
        /// 初始化
        /// </summary>
        private void InitTimer()
        {
            timer = new Timer();
            timer.Elapsed += new ElapsedEventHandler(TimerInvoke);
            timer.Interval = 500;

            player = new MusicPlayer();
            watch = new Stopwatch();
        }

        /// <summary>
        /// 初始化歌曲标题
        /// </summary>
        private void InitText()
        {
            string artist,title;
            if (son.Artist != "")
            {
                artist = son.Artist;
            }
            else
            {
                artist = power.Lrc.Artist;
            }
            if (son.Title != "")
            {
                title = son.Title;
            }
            else
            {
                title = power.Lrc.Title;
            }

            this.SongName.Text = artist + " — " + title;
            image =new LoadImage(artist);
            image.SetImage(LrcBack, SongImage);
        }



        private void TimerInvoke(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action<LoadImage>(power.BeginAnimate), image);
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
                icon.Dispose();
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

        #region 最小化到托盘
        /// <summary>
        /// 初始化托盘
        /// </summary>
        private void InitTray()
        {
            //设置托盘数据
            icon = new System.Windows.Forms.NotifyIcon();
            icon.BalloonTipText = "Hello，Music！";
            icon.Text = "Music";
            icon.Icon = new System.Drawing.Icon(@"../../Images/music.ico");
            icon.Visible = true;
            icon.ShowBalloonTip(1000);
            icon.MouseClick += new System.Windows.Forms.MouseEventHandler(TrayMouseLeftClick);

            //退出菜单项
            System.Windows.Forms.MenuItem exit = new System.Windows.Forms.MenuItem("退出Music");
            exit.Click += new EventHandler(Exit);

            //关联托盘控件
            System.Windows.Forms.MenuItem[] children = new System.Windows.Forms.MenuItem[] { exit };
            icon.ContextMenu = new System.Windows.Forms.ContextMenu(children);
            this.StateChanged += new EventHandler(HideMinWindow);
        }

        /// <summary>
        /// 窗体最小化时，隐藏窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HideMinWindow(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
            {
                this.Hide();
            }
        }

        /// <summary>
        /// 退出按钮
        /// </summary>
        private void Exit(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定退出Music吗？", "退出", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                icon.Dispose();
                System.Windows.Application.Current.Shutdown();
            }
        }

        /// <summary>
        /// 鼠标单击托盘图标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void TrayMouseLeftClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (this.Visibility == Visibility.Visible)
                {
                    this.Hide();
                }
                else
                {
                    //this.Visibility=Visibility.Visible;
                    this.Show();
                    this.WindowState = WindowState.Normal;
                    this.Activate();
                }
            }
        }
        #endregion

    }
}
