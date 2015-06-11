using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Music.Lyric
{
    internal class Lrc
    {
        public Lrc()
        {
            LrcWord = new Dictionary<double, string>();
        }

        /// <summary>
        /// 歌曲名字
        /// </summary>
        public string Title { set; get; }

        /// <summary>
        /// 歌手
        /// </summary>
        public string Artist { set; get; }

        /// <summary>
        /// 专辑  
        /// </summary>
        public string Album { set; get; }

        /// <summary>
        /// 偏移量
        /// </summary>
        public string Offset { set; get; }

        /// <summary>
        /// 歌词信息
        /// </summary>
        public Dictionary<double, string> LrcWord { set; get; }

        /// <summary>
        /// 初始化歌词信息
        /// </summary>
        /// <param name="lrcPath"></param>
        /// <returns></returns>
        public static Lrc InitLrc(string lrcPath)
        {
            if (!File.Exists(lrcPath))
            {
                throw new FileNotFoundException("文件"+lrcPath+"未找到");
            }
            Lrc lrc = new Lrc();
            string lrcFormat= lrcPath.Split('.').Last();
            using (FileStream file = new FileStream(lrcPath, FileMode.Open, FileAccess.Read))
            {
                if (lrcFormat == "lrc")
                {
                    LrcParse(lrc,file);
                }

            }
            return lrc;
        }

        /// <summary>
        /// 解析lrc格式歌词
        /// </summary>
        /// <param name="file"></param>
        private static void LrcParse(Lrc lrc , FileStream file)
        {
            string line;
            using (StreamReader reader = new StreamReader(file))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if(line.Trim().StartsWith("["))
                    {
                    if (line.StartsWith("[ti:"))
                    {
                        lrc.Title = SplitInfo(line);
                    }
                    else if (line.StartsWith("[ar:"))
                    {
                        lrc.Artist = SplitInfo(line);
                    }
                    else if (line.StartsWith("[al:"))
                    {
                        lrc.Album = SplitInfo(line);
                    }
                    else if (line.StartsWith("[offset:"))
                    {
                        lrc.Offset = SplitInfo(line);
                    }
                    else
                    {                        
                        var list = line.Split(new char[] { '[', ']' },StringSplitOptions.RemoveEmptyEntries);
                        if(list.Count()==2)
                        {
                           Regex regex = new Regex("^[A-Z]|^[a-z]");
                           if(!regex.Match(list[0]).Success){
                               lrc.LrcWord.Add(Trans(list[0]),list[1]);
                           }
                       }
                    }
                 }
              }
            }

        }

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private static string SplitInfo(string line)
        {
            return line.Substring(line.IndexOf(":")+1).TrimEnd(']');
        }

        /// <summary>
        /// 转化字符串时间到秒数
        /// </summary>
        /// <param name="time"></param>
        private static double Trans(string time)
        {
            if (time == "" || time == null)
            {
                throw new ArgumentNullException("歌词转化时间错误");
            }
            double d = 0;
            var list  = time.Split(':');
            for (int i = list.Count()-1,j=0; i >= 0; i--,j++)
            {
                d += Convert.ToDouble(list[i]) * Math.Pow(60,j)*1000;
            }
            return d;
        }

    }
}
