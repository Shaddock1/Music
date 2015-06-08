using Music.Model;
using Shell32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music.MusicPush
{
    /// <summary>
    /// 初始化一个音乐文件信息
    /// </summary>
    internal class MusicCalc
    {
        /// <summary>
        /// 把秒数化成时间表示
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string GetSecond(double num )
        {
            StringBuilder builder = new StringBuilder();
            int minute =(int)num / 60;
            int second = (int)num % 60;                        
            builder.Append(minute.ToString("00"));
            builder.Append(":");
            builder.Append(second.ToString("00"));
            return builder.ToString();
        }


        public static SongMessage GetSong(string path)
        {
            SongMessage model = null;
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("歌曲"+path+"不存在");
            }
            else
            {
                model = new SongMessage();
                model.SongPath = path;
                GetSongPlayBaseMessage(model);
            }
            return model;
        }

        /// <summary>
        /// 获取音乐文件的总长度
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static void GetSongPlayBaseMessage(SongMessage model)
        {
            ShellClass sh = new ShellClass();
            Folder dir = sh.NameSpace(System.IO.Path.GetDirectoryName(model.SongPath));
            FolderItem item = dir.ParseName(System.IO.Path.GetFileName(model.SongPath));
            string str = dir.GetDetailsOf(item, 27);
            model.MusicTime = str;
            model.Artist = dir.GetDetailsOf(item,13);
            model.Album = dir.GetDetailsOf(item,14);
            model.Year = dir.GetDetailsOf(item,15);
            model.Title = dir.GetDetailsOf(item,21);            
            model.Comment = dir.GetDetailsOf(item,24);
            GetSongTransformMes(model);
        }

        /// <summary>
        /// 获取一些额外的歌曲信息
        /// </summary>
        /// <param name="model"></param>
        private static void GetSongTransformMes(SongMessage model)
        {
            FileStream file = new FileStream(model.SongPath , FileMode.Open,FileAccess.Read);
            model.TotalBytes = file.Length;
            file.Close();
            model.Size = TransBytes(model.TotalBytes);
            model.MusicSecond = TransSecond(model.MusicTime);
        }

        /// <summary>
        /// 字节转换为字符串大小
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private static string TransBytes(long fbyte)
        {
            StringBuilder builder = new StringBuilder();
            int n = 0;
            double bytes = fbyte;
            if (bytes > 1024)
            {
                while (bytes > 1024)
                {
                    n++;
                    bytes = bytes / 1024;
                }
                if (n == 1)
                {
                    builder.Append(bytes.ToString("0.00")+"KB");
                }
                else if (n == 2)
                {
                    builder.Append(bytes.ToString("0.00") + "MB");
                }
                else
                {
                    builder.Append(bytes.ToString("0.00") + "GB");
                }
            }
            else
            {
                builder.Append(bytes+"B");
            }
            return builder.ToString();
        }

        /// <summary>
        /// 音乐时间转化成秒数
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private static long TransSecond(string time)
        {
            if(time==""||time==null)
            {
                return 0;
            }
            var list = time.Split(new string[]{":"},StringSplitOptions.RemoveEmptyEntries);
            long result = 0;
            for (int j=0,i = list.Length - 1; i >= 0; i--,j++)
            {
                result += Convert.ToInt64(list[i]) * (long)Math.Pow(60,j);
            }
            return result;
        }

    }
}
