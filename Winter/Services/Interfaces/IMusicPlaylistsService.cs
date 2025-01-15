using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winter.Models.MusicLibrary;

namespace Winter.Services.Interfaces
{
    public interface IMusicPlaylistsService
    {
        /// <summary>
        /// 加载音乐库中的所有播放列表
        /// </summary>
        /// <returns></returns>
        Task LoadMusicPlaylistsAsync();

        /// <summary>
        /// 获取音乐库中的所有播放列表
        /// </summary>
        /// <returns></returns>
        List<LibraryPlaylistItem> GetAllPlaylistItems();

        /// <summary>
        /// 通过路径获取播放列表
        /// </summary>
        /// <param name="path">播放列表文件路径</param>
        /// <returns></returns>
        LibraryPlaylistItem? GetPlaylistItemByPath(string path);
    }
}
