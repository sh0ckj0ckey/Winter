﻿using System.Collections.Generic;
using Microsoft.UI.Xaml.Media.Imaging;

namespace Winter.Models.MusicLibrary
{
    public class LibraryPlaylistItem
    {
        /// <summary>
        /// 播放列表标题
        /// </summary>
        public string Title { get; set; } = "";

        /// <summary>
        /// 播放列表文件路径
        /// </summary>
        public string PlaylistFilePath { get; set; } = "";

        /// <summary>
        /// 播放列表中的音乐路径
        /// </summary>
        public List<string> MusicFilePaths { get; set; } = [];

        /// <summary>
        /// 播放列表封面
        /// </summary>
        public AsyncCoverImage PlaylistMainCover { get; set; } = new();

        /// <summary>
        /// 播放列表第二封面
        /// </summary>
        public AsyncCoverImage PlaylistSecondaryCover { get; set; } = new();

        /// <summary>
        /// 播放列表第三封面
        /// </summary>
        public AsyncCoverImage PlaylistTertiaryCover { get; set; } = new();
    }
}
