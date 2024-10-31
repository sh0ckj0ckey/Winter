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
        /// 时长
        /// </summary>
        public double Duration { get; set; } = 0;

        /// <summary>
        /// 艺术家
        /// </summary>
        public string Artist { get; set; } = "";

        /// <summary>
        /// 专辑
        /// </summary>
        public string Album { get; set; } = "";
    }
}
