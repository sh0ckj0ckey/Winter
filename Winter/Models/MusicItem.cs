using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winter.Models
{
    /// <summary>
    /// 音乐库列表项
    /// </summary>
    public class MusicItem
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; } = "";

        /// <summary>
        /// 艺术家
        /// </summary>
        public string Artist { get; set; } = "";

        /// <summary>
        /// 专辑
        /// </summary>
        public string Album { get; set; } = "";

        /// <summary>
        /// 专辑艺术家
        /// </summary>
        public string AlbumArtist { get; set; } = "";

        /// <summary>
        /// 时长
        /// </summary>
        public string Duration { get; set; } = "";

        /// <summary>
        /// 年份
        /// </summary>
        public uint Year { get; set; } = 0;
        
        /// <summary>
        /// 编号
        /// </summary>
        public uint TrackNumber { get; set; }

        /// <summary>
        /// 名称首字母
        /// </summary>
        public string FirstLetter { get; set; } = "#";
    }
}
