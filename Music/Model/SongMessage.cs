using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music.Model
{
    /// <summary>
    /// 歌曲文件信息
    /// </summary>
    public class SongMessage
    {
        private string _musictime;
        /// <summary>
        /// 音乐时长
        /// </summary>
        public string MusicTime { 
            set
            {
                var a = value.Split(':');
                this._musictime = a[1] +":"+a[2];
            }
            get
            {
                return this._musictime;
            }
        }

        /// <summary>
        /// 音乐总共的秒数
        /// </summary>
        public long MusicSecond { set; get; }

        /// <summary>
        /// 发行年
        /// </summary>
        public string Year { set; get; }

        /// <summary>
        /// 所属唱片
        /// </summary>
        public string Album { set; get; }

        /// <summary>
        /// 演唱者
        /// </summary>
        public string Artist { set; get; }

        /// <summary>
        /// 歌曲名
        /// </summary>
        public string Title { set; get; }

        /// <summary>
        /// 注释
        /// </summary>
        public string Comment { set; get; }

        /// <summary>
        /// 歌曲路径
        /// </summary>
        public string SongPath { set; get; }
        
        /// <summary>
        /// 总字节数
        /// </summary>
        public long TotalBytes { set; get; }

        /// <summary>
        /// 大小(转化为KB MB等等)
        /// </summary>
        public string Size { set; get; }

    }
}
