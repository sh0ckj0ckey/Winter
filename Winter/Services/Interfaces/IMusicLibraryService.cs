using System.Collections.Generic;
using System.Threading.Tasks;
using Winter.Models.MusicLibrary;

namespace Winter.Services.Interfaces
{
    public interface IMusicLibraryService
    {
        /// <summary>
        /// 加载音乐库
        /// </summary>
        /// <returns></returns>
        Task LoadMusicLibraryAsync();

        /// <summary>
        /// 获取音乐库中的所有音乐项
        /// </summary>
        /// <returns></returns>
        List<LibraryMusicItem> GetAllMusicItems();

        /// <summary>
        /// 通过路径获取音乐项
        /// </summary>
        /// <param name="path">音乐文件路径</param>
        /// <returns></returns>
        Task<LibraryMusicItem?> GetMusicItemByPathAsync(string path);
    }
}
