using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage.FileProperties;
using Windows.Storage.Search;
using Windows.Storage;
using Winter.Helpers;
using Winter.Models.MusicLibrary;
using Winter.Services.Interfaces;

namespace Winter.Services
{
    public class MusicLibraryService : IMusicLibraryService
    {
        private readonly List<LibraryMusicItem> _allMusicItems = [];

        private readonly List<LibraryAlbumItem> _allAlbumItems = [];

        private readonly Dictionary<string, LibraryMusicItem> _pathToMusicItem = new();

        public async Task LoadMusicLibraryAsync()
        {
            _allMusicItems.Clear();
            _allAlbumItems.Clear();
            _pathToMusicItem.Clear();

            Debug.WriteLine("Loading music library...");

            // 获取音乐库的存储文件夹
            // 注意这里并非Music目录，是支持各软件进行修改的
            StorageFolder musicFolder = KnownFolders.MusicLibrary;

            // 创建一个查询选项以查找所有音乐文件
            var queryOptions = new QueryOptions(CommonFileQuery.OrderByName, [".mp3", ".wav", ".aac", ".flac"]);
            var query = musicFolder.CreateFileQueryWithOptions(queryOptions);

            // 获取所有音乐文件
            var files = await query.GetFilesAsync();

            // 遍历每个文件，获取其属性
            var tasks = files.Select(async file =>
            {
                try
                {
                    MusicProperties musicProperties = await file.Properties.GetMusicPropertiesAsync();
                    // StorageItemThumbnail thumbnail = await file.GetThumbnailAsync(ThumbnailMode.MusicView, 200, ThumbnailOptions.UseCurrentScale);

                    LibraryMusicItem musicItem = new()
                    {
                        MusicFilePath = file.Path,
                        Title = !string.IsNullOrWhiteSpace(musicProperties.Title) ? musicProperties.Title : file.DisplayName,
                        Artist = !string.IsNullOrWhiteSpace(musicProperties.Artist) ? musicProperties.Artist : "未知艺术家",
                        Album = !string.IsNullOrWhiteSpace(musicProperties.Album) ? musicProperties.Album : "",
                        AlbumArtist = !string.IsNullOrWhiteSpace(musicProperties.AlbumArtist) ? musicProperties.AlbumArtist : "未知艺术家",
                        Duration = musicProperties.Duration.ToString(@"mm\:ss"),
                        Year = musicProperties.Year,
                        TrackNumber = musicProperties.TrackNumber
                    };

                    musicItem.TitleFirstLetter = PinyinHelper.GetFirstSpell(musicItem.Title);

                    _pathToMusicItem[musicItem.MusicFilePath] = musicItem;

                    return musicItem;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    return null;
                }
            }).ToList();

            var musicItems = await Task.WhenAll(tasks);

            _allMusicItems.Clear();
            _allAlbumItems.Clear();

            if (musicItems is null)
            {
                return;
            }

            foreach (var musicItem in musicItems)
            {
                if (musicItem is null)
                {
                    continue;
                }

                _allMusicItems.Add(musicItem);

                if (!string.IsNullOrWhiteSpace(musicItem.Album))
                {
                    var album = _allAlbumItems.FirstOrDefault(albumItem => albumItem.Title == musicItem.Album && albumItem.AlbumArtist == musicItem.AlbumArtist);
                    if (album is null)
                    {
                        _allAlbumItems.Add(new LibraryAlbumItem
                        {
                            Title = musicItem.Album,
                            AlbumArtist = musicItem.AlbumArtist,
                            Year = musicItem.Year
                        });
                    }
                    else
                    {
                        album.Year = Math.Max(album.Year, musicItem.Year);
                    }
                }
            }

            Debug.WriteLine("Loaded music library.");
        }

        public List<LibraryMusicItem> GetAllMusicItems() => _allMusicItems;

        public List<LibraryAlbumItem> GetAllAlbumItems() => _allAlbumItems;

        public List<LibraryMusicItem> GetMusicItemsByArtist(string artist) =>
            _allMusicItems.Where(music => music.Artist.Split(';').Any(a => a.Trim().Equals(artist, StringComparison.Ordinal))).ToList();

        public List<LibraryAlbumItem> GetAlbumItemsByArtist(string artist) =>
            _allAlbumItems.Where(album => album.AlbumArtist.Split(';').Any(a => a.Trim().Equals(artist, StringComparison.Ordinal))).ToList();

        public List<LibraryMusicItem> GetMusicItemsByAlbum(LibraryAlbumItem album) =>
            _allMusicItems.Where(music => music.Album == album.Title && music.AlbumArtist == album.AlbumArtist).ToList();

        public LibraryMusicItem? GetMusicItemByPath(string path)
        {
            _pathToMusicItem.TryGetValue(path, out LibraryMusicItem? musicItem);
            return musicItem;
        }
    }
}
