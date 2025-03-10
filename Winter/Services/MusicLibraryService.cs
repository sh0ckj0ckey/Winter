using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Search;
using Winter.Helpers;
using Winter.Models.MusicModels;
using Winter.Services.Interfaces;

namespace Winter.Services
{
    public class MusicLibraryService : IMusicLibraryService
    {
        private readonly List<MusicLibraryItem> _allMusicItems = [];

        private readonly Dictionary<string, MusicLibraryItem> _pathToMusicItem = [];

        public async Task InitializeMusicLibraryAsync()
        {
            _allMusicItems.Clear();
            _pathToMusicItem.Clear();

            Debug.WriteLine("Loading music library...");

            // 获取音乐库的存储文件夹
            // 注意这里并非Music目录，是支持各软件进行修改的
            StorageFolder musicFolder = KnownFolders.MusicLibrary;

            // 创建一个查询选项以查找所有音乐文件
            var queryOptions = new QueryOptions(CommonFileQuery.OrderByName, [".mp3", ".wav", ".flac", ".aac", ".m4a", ".wma"]);
            var query = musicFolder.CreateFileQueryWithOptions(queryOptions);
            var files = await query.GetFilesAsync();

            // 遍历每个文件，获取其属性
            //var musicItems = await Task.WhenAll(files.Select(async file =>
            //{
            //    try
            //    {
            //        return await LoadMusicFromFileAsync(file);
            //    }
            //    catch (Exception ex)
            //    {
            //        Debug.WriteLine(ex);
            //        return null;
            //    }
            //}));

            foreach (var file in files)
            {
                try
                {
                    MusicLibraryItem musicItem = await LoadMusicFromFileAsync(file);
                    _allMusicItems.Add(musicItem);
                    _pathToMusicItem[musicItem.MusicFilePath] = musicItem;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }

            Debug.WriteLine("Loaded music library.");
        }

        public List<MusicLibraryItem> GetAllMusicItems() => _allMusicItems;

        public async Task<MusicLibraryItem?> GetMusicItemByPathAsync(string path)
        {
            _pathToMusicItem.TryGetValue(path, out MusicLibraryItem? musicItem);

            if (musicItem is null)
            {
                // 如果这个音乐文件存在，但是不存在于列表中，则加载它到字典里，但是不添加到列表
                var file = await StorageFile.GetFileFromPathAsync(path);
                if (file is not null)
                {
                    musicItem = await LoadMusicFromFileAsync(file);
                    _pathToMusicItem[musicItem.MusicFilePath] = musicItem;
                }
            }

            return musicItem;
        }

        private async Task<MusicLibraryItem> LoadMusicFromFileAsync(StorageFile file)
        {
            MusicProperties musicProperties = await file.Properties.GetMusicPropertiesAsync();

            MusicLibraryItem musicItem = new()
            {
                MusicFilePath = file.Path,
                Title = !string.IsNullOrWhiteSpace(musicProperties.Title) ? musicProperties.Title : file.DisplayName,
                Artist = !string.IsNullOrWhiteSpace(musicProperties.Artist) ? musicProperties.Artist : "未知艺术家",
                Album = !string.IsNullOrWhiteSpace(musicProperties.Album) ? musicProperties.Album : "",
                AlbumArtist = !string.IsNullOrWhiteSpace(musicProperties.AlbumArtist) ? musicProperties.AlbumArtist : "未知艺术家",
                Duration = musicProperties.Duration.ToString(@"mm\:ss"),
                Year = musicProperties.Year,
                TrackNumber = musicProperties.TrackNumber,
            };

            musicItem.TitleFirstLetter = PinyinHelper.GetFirstSpell(musicItem.Title);
            _ = musicItem.MusicCover.LoadCoverImageFromFile(file, 72 * 2);

            return musicItem;
        }

    }
}
