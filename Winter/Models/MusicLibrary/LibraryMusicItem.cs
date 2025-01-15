namespace Winter.Models.MusicLibrary
{
    /// <summary>
    /// 音乐库音乐项
    /// </summary>
    public class LibraryMusicItem
    {
        /// <summary>
        /// 音乐标题首字母
        /// </summary>
        public string TitleFirstLetter { get; set; } = "#";

        /// <summary>
        /// 音乐文件路径
        /// </summary>
        public string MusicFilePath { get; set; } = "";

        /// <summary>
        /// 音乐标题
        /// </summary>
        public string Title { get; set; } = "";

        /// <summary>
        /// 参与艺术家
        /// </summary>
        public string Artist { get; set; } = "";

        /// <summary>
        /// 所属专辑标题
        /// </summary>
        public string Album { get; set; } = "";

        /// <summary>
        /// 所属专辑艺术家
        /// </summary>
        public string AlbumArtist { get; set; } = "";

        /// <summary>
        /// 音乐时长
        /// </summary>
        public string Duration { get; set; } = "";

        /// <summary>
        /// 音乐年份
        /// </summary>
        public uint Year { get; set; } = 0;

        /// <summary>
        /// 音乐编号
        /// </summary>
        public uint TrackNumber { get; set; }
    }
}
