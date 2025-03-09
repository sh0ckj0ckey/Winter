using System.Collections.Generic;
using Winter.Models.MusicLibrary;

namespace Winter.Models
{
    public class MusicGroup
    {
        /// <summary>
        /// 索引
        /// </summary>
        public string Key { get; set; } = "#";

        /// <summary>
        /// 分组音乐
        /// </summary>
        public List<LibraryMusicItem> GroupedMusic { get; set; } = new();
    }
}
