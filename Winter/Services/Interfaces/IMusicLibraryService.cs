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
        /// 获取音乐库中的所有音乐
        /// </summary>
        /// <returns></returns>
        List<LibraryMusicItem> GetAllMusicItems();

        /// <summary>
        /// 获取音乐库中的所有专辑
        /// </summary>
        /// <returns></returns>
        List<LibraryAlbumItem> GetAllAlbumItems();

        /// <summary>
        /// 获取音乐库中的所有艺术家名字
        /// </summary>
        /// <returns></returns>
        List<string> GetAllArtistNames();

        /// <summary>
        /// 获取指定艺术家的所有音乐
        /// </summary>
        /// <param name="artist">艺术家名字</param>
        /// <returns></returns>
        List<LibraryMusicItem> GetMusicItemsByArtist(string artist);

        /// <summary>
        /// 获取指定艺术家的所有专辑
        /// </summary>
        /// <param name="artist">艺术家名字</param>
        /// <returns></returns>
        List<LibraryAlbumItem> GetAlbumItemsByArtist(string artist);

        /// <summary>
        /// 获取指定专辑的所有音乐
        /// </summary>
        /// <param name="album">专辑</param>
        /// <returns></returns>
        List<LibraryMusicItem> GetMusicItemsByAlbum(LibraryAlbumItem album);

        /// <summary>
        /// 通过路径获取音乐
        /// </summary>
        /// <param name="path">音乐文件路径</param>
        /// <returns></returns>
        Task<LibraryMusicItem?> GetMusicItemByPathAsync(string path);
    }
}
