using System.Collections.ObjectModel;

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
        public ObservableCollection<MusicItem> GroupedMusic { get; set; } = new();
    }
}
