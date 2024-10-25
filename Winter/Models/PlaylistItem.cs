namespace Winter.Models
{
    /// <summary>
    /// 播放列表项
    /// </summary>
    public class PlaylistItem
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; } = "";

        /// <summary>
        /// 时长
        /// </summary>
        public int Duration { get; set; } = 0;

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
