using System.Collections.Generic;
using Winter.Models.MusicModels;

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
        public List<MusicLibraryItem> GroupedMusic { get; set; } = new();
    }
}
