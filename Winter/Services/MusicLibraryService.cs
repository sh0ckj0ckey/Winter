using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Search;
using Winter.Helpers;
using Winter.Models.MusicLibrary;
using Winter.Services.Interfaces;

namespace Winter.Services
{
    public class MusicLibraryService : IMusicLibraryService
    {
        private readonly List<LibraryMusicItem> _allMusicItems = [];

        private readonly List<LibraryAlbumItem> _allAlbumItems = [];

        private readonly Dictionary<string, LibraryMusicItem> _pathToMusicItem = [];

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

            _allMusicItems.Clear();
            _allAlbumItems.Clear();
            _pathToMusicItem.Clear();

            foreach (var file in files)
            {
                try
                {
                    var musicItem = await LoadMusicFromFileAsync(file);

                    if (musicItem is null)
                    {
                        continue;
                    }

                    _allMusicItems.Add(musicItem);
                    _pathToMusicItem[musicItem.MusicFilePath] = musicItem;

                    if (!string.IsNullOrWhiteSpace(musicItem.Album))
                    {
                        var album = _allAlbumItems.FirstOrDefault(albumItem => albumItem.Title == musicItem.Album && albumItem.AlbumArtist == musicItem.AlbumArtist);
                        if (album is null)
                        {
                            album = new LibraryAlbumItem
                            {
                                Title = musicItem.Album,
                                AlbumArtist = musicItem.AlbumArtist,
                                Year = musicItem.Year
                            };

                            _allAlbumItems.Add(album);
                        }
                        else
                        {
                            album.Year = Math.Max(album.Year, musicItem.Year);
                        }

                        if (album.AlbumCover.Image is null)
                        {
                            _ = album.AlbumCover.LoadCoverImageFromFile(file, 96 * 2);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }

            Debug.WriteLine("Loaded music library.");
        }

        public List<LibraryMusicItem> GetAllMusicItems() => _allMusicItems;

        public List<LibraryAlbumItem> GetAllAlbumItems() => _allAlbumItems;

        public List<string> GetAllArtistNames() => _allMusicItems.Select(music => music.Artist.Split(';')).SelectMany(artists => artists).Distinct().ToList();

        public List<LibraryMusicItem> GetMusicItemsByArtist(string artist) =>
            _allMusicItems.Where(music => music.Artist.Split(';').Any(a => a.Trim().Equals(artist, StringComparison.Ordinal))).ToList();

        public List<LibraryAlbumItem> GetAlbumItemsByArtist(string artist) =>
            _allAlbumItems.Where(album => album.AlbumArtist.Split(';').Any(a => a.Trim().Equals(artist, StringComparison.Ordinal))).ToList();

        public List<LibraryMusicItem> GetMusicItemsByAlbum(LibraryAlbumItem album) =>
            _allMusicItems.Where(music => music.Album == album.Title && music.AlbumArtist == album.AlbumArtist).ToList();

        public async Task<LibraryMusicItem?> GetMusicItemByPathAsync(string path)
        {
            _pathToMusicItem.TryGetValue(path, out LibraryMusicItem? musicItem);
            if (musicItem is null)
            {
                try
                {
                    // 如果这个音乐文件存在，但是不存在于列表中，则加载它到字典里，但是不添加到列表
                    var file = await StorageFile.GetFileFromPathAsync(path);
                    if (file is not null)
                    {
                        musicItem = await LoadMusicFromFileAsync(file);
                        _pathToMusicItem[path] = musicItem;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }

            return musicItem;
        }

        private async Task<LibraryMusicItem> LoadMusicFromFileAsync(StorageFile file)
        {
            MusicProperties musicProperties = await file.Properties.GetMusicPropertiesAsync();

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

            return musicItem;
        }

    }
}
