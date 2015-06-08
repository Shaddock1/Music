﻿using Music.Model;
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

        public MainWindow()
        {
            InitializeComponent();
            InitTimer();
             son  =  MusicCalc.GetSong( @"C:\Users\Admin\Desktop\天后.mp3");
            this.TotalTime.Text = son.MusicTime;
            this.SongName.Text = son.Title + " — " + son.Artist;
            player = new MusicPlayer();
            player.Prepare(son);


        }

        /// <summary>
        /// 初始化定时器
        /// </summary>
        private void InitTimer()
        {
            watch = new Stopwatch();
            timer = new Timer();
            timer.Elapsed += new ElapsedEventHandler(TimerUse);
            timer.Interval = 400;
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
            this.PlayProcess.Value = watch.Elapsed.TotalSeconds*100/son.MusicSecond;
            this.NowTime.Text = MusicCalc.GetSecond(watch.Elapsed.TotalSeconds);
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
            player.PlayToggle(timer,watch);
        }


    }
}