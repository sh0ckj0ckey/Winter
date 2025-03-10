using System.Collections.Generic;
using System.Threading.Tasks;
using Winter.Models.MusicModels;

namespace Winter.Services.Interfaces
{
    public interface IMusicLibraryService
    {
        /// <summary>
        /// 加载音乐库
        /// </summary>
        /// <returns></returns>
        Task InitializeMusicLibraryAsync();

        /// <summary>
        /// 获取音乐库中的所有音乐项
        /// </summary>
        /// <returns></returns>
        List<MusicLibraryItem> GetAllMusicItems();

        /// <summary>
        /// 通过路径获取音乐项
        /// </summary>
        /// <param name="path">音乐文件路径</param>
        /// <returns></returns>
        Task<MusicLibraryItem?> GetMusicItemByPathAsync(string path);
    }
}
