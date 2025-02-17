using System.Collections.Generic;

namespace Winter.Models.MusicLibrary
{
    /// <summary>
    /// 音乐库专辑项
    /// </summary>
    public class LibraryAlbumItem
    {
        /// <summary>
        /// 专辑标题
        /// </summary>
        public string Title { get; set; } = "";

        /// <summary>
        /// 专辑艺术家
        /// </summary>
        public string AlbumArtist { get; set; } = "";

        /// <summary>
        /// 专辑年份
        /// </summary>
        public uint Year { get; set; } = 0;

        /// <summary>
        /// 专辑封面
        /// </summary>
        public AsyncCoverImage AlbumCover { get; set; } = new();

        /// <summary>
        /// 专辑中的音乐
        /// </summary>
        public List<LibraryMusicItem> Music { get; set; } = new List<LibraryMusicItem>();
    }
}
