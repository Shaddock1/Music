using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Music.Load
{
    internal class LoadImage
    {
        public  IList<string> ImagePathList { get;private set; }
        public  string HeadImage { get;private set; }
        private string StarName{set;get;}
        private string BaseDirecoty;
        private const string defaultHead = ".//StarImage//defaulthead.jpg";
        private const string defaultBack= ".//StarImage//defaultback.jpg";
        private const double time = 20000;
        private object back;
        private Timer timer;
        private int position;

        public LoadImage(string name)
        {
            this.StarName = name;
            this.HeadImage = string.Empty;
            this.BaseDirecoty = ".//StarImage//"+name;
            this.ImagePathList = new List<string>();
            InitImage();
        }

        /// <summary>
        /// 初始化找到图片
        /// </summary>
        public void InitImage()
        {
            if (Directory.Exists(BaseDirecoty))
            {
                DirectoryInfo info =new DirectoryInfo(BaseDirecoty);
                foreach (var str in info.GetFiles())
                {
                    this.ImagePathList.Add(BaseDirecoty+"//"+str.Name);
                }
                string head = BaseDirecoty +"//Head";
                DirectoryInfo info2 = new DirectoryInfo(head);
                this.HeadImage = head +"//"+ info2.GetFiles()[0].Name;
            }
        }

        /// <summary>
        /// 设置背景图片的头像图片
        /// </summary>
        /// <param name="back"></param>
        /// <param name="head"></param>
        public void SetImage(object back, object head)
        {
            this.back = back;
            ImageBrush brush = head as ImageBrush;
            if (HeadImage != "")
            {
                brush.ImageSource = new BitmapImage(new Uri(HeadImage, UriKind.Relative));
            }
            else
            {
                brush.ImageSource = new BitmapImage(new Uri(defaultHead, UriKind.Relative));
            }
            Setback();
        }

        /// <summary>
        /// 设置歌词背景图
        /// </summary>
        private void Setback()
        {
            if (ImagePathList.Count() == 0)
            {
                (back as ImageBrush).ImageSource = new BitmapImage(new Uri(defaultBack, UriKind.Relative));
                return;
            }
            Begin();
            timer = new Timer();
            timer.Elapsed += new ElapsedEventHandler(TimerInvoke);
            timer.Interval = time;
            timer.Start();
        }

        private void TimerInvoke(object sender, ElapsedEventArgs e)
        {
            (back as ImageBrush).Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(Begin));
        }

        private void Begin()
        {
            if(position ==ImagePathList.Count())
            {
                position =0;
            }
            (back as ImageBrush).ImageSource = new BitmapImage(new Uri(ImagePathList[position], UriKind.Relative));
            position++;
        }

        public void Finaliz()
        {
            if (timer != null)
            {
                timer.Stop();
            }
        }

    }
}
