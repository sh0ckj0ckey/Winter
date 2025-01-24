using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Search;
using Winter.Models.MusicLibrary;
using Winter.Services.Interfaces;

namespace Winter.Services
{
    public class MusicPlaylistsService : IMusicPlaylistsService
    {
        private readonly List<LibraryPlaylistItem> _allPlaylistItems = [];

        private readonly Dictionary<string, LibraryPlaylistItem> _pathToPlaylistItem = [];

        // 音乐类型文件的扩展名
        private readonly HashSet<string> _musicExtensions = new(StringComparer.OrdinalIgnoreCase) { ".mp3", ".wav", ".flac", ".aac", ".m4a", ".wma" };

        public async Task LoadMusicPlaylistsAsync()
        {
            _allPlaylistItems.Clear();
            _pathToPlaylistItem.Clear();

            Debug.WriteLine("Loading music playlists...");

            // 获取音乐库的存储文件夹
            // 注意这里并非Music目录，是支持各软件进行修改的
            StorageFolder musicFolder = KnownFolders.MusicLibrary;

            // 创建一个查询选项以查找所有播放列表文件
            var queryOptions = new QueryOptions(CommonFileQuery.OrderByName, [".m3u", ".m3u8"]);
            var query = musicFolder.CreateFileQueryWithOptions(queryOptions);
            var files = await query.GetFilesAsync();

            // 遍历每个文件，加载播放列表
            //var playlistItems = await Task.WhenAll(files.Select(async file =>
            //{
            //    try
            //    {
            //        return await LoadPlaylistFromFileAsync(file);
            //    }
            //    catch (Exception ex)
            //    {
            //        Debug.WriteLine(ex);
            //        return null;
            //    }
            //}));

            List<LibraryPlaylistItem?> playlistItems = [];
            foreach (var file in files)
            {
                try
                {
                    playlistItems.Add(await LoadPlaylistFromFileAsync(file));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }

            _allPlaylistItems.Clear();
            _pathToPlaylistItem.Clear();

            if (playlistItems is null)
            {
                return;
            }

            foreach (var playlistItem in playlistItems)
            {
                if (playlistItem is null)
                {
                    continue;
                }

                _allPlaylistItems.Add(playlistItem);
                _pathToPlaylistItem[playlistItem.PlaylistFilePath] = playlistItem;
            }

            Debug.WriteLine("Loaded music playlists.");
        }

        public List<LibraryPlaylistItem> GetAllPlaylistItems() => _allPlaylistItems;

        public async Task<LibraryPlaylistItem?> GetPlaylistItemByPathAsync(string path)
        {
            _pathToPlaylistItem.TryGetValue(path, out LibraryPlaylistItem? playlistItem);
            if (playlistItem is null)
            {
                try
                {
                    // 如果这个播放列表文件存在，但是不存在于列表中，则加载它到字典里，但是不添加到列表
                    var file = await StorageFile.GetFileFromPathAsync(path);
                    if (file is not null)
                    {
                        playlistItem = await LoadPlaylistFromFileAsync(file);
                        _pathToPlaylistItem[path] = playlistItem;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }

            return playlistItem;
        }

        private async Task<LibraryPlaylistItem> LoadPlaylistFromFileAsync(StorageFile file)
        {
            IList<string> lines;
            try
            {
                // 尝试使用 UTF-8 编码读取文件
                using var stream = await file.OpenStreamForReadAsync();
                using var reader = new StreamReader(stream, Encoding.UTF8, true);
                var content = await reader.ReadToEndAsync();
                lines = content.Split(["\r\n", "\r", "\n"], StringSplitOptions.None);
            }
            catch
            {
                // 如果 UTF-8 读取失败，使用默认编码读取文件
                using var stream = await file.OpenStreamForReadAsync();
                using var reader = new StreamReader(stream, Encoding.Default, true);
                var content = await reader.ReadToEndAsync();
                lines = content.Split(["\r\n", "\r", "\n"], StringSplitOptions.None);
            }

            var musicFilePaths = new List<string>();

            foreach (var line in lines)
            {
                if (line.StartsWith('#'))
                {
                    continue;
                }

                if (_musicExtensions.Contains(Path.GetExtension(line)))
                {
                    musicFilePaths.Add(line);
                }
            }

            var playlistItem = new LibraryPlaylistItem
            {
                PlaylistFilePath = file.Path,
                Title = Path.GetFileNameWithoutExtension(file.DisplayName),
                MusicFilePaths = musicFilePaths
            };

            // 获取封面图
            var coverImages = new List<BitmapImage>();
            foreach (var musicFilePath in musicFilePaths)
            {
                try
                {
                    if (coverImages.Count >= 3)
                    {
                        break;
                    }

                    uint imageSize = coverImages.Count switch
                    {
                        0 => 96 * 2,
                        1 => 88 * 2,
                        2 => 80 * 2,
                        _ => 96 * 2
                    };

                    var musicFile = await StorageFile.GetFileFromPathAsync(musicFilePath);
                    var thumbnail = await musicFile.GetThumbnailAsync(ThumbnailMode.MusicView, imageSize, ThumbnailOptions.UseCurrentScale);

                    if (thumbnail is not null && thumbnail.Type != ThumbnailType.Icon)
                    {
                        var bitmapImage = new BitmapImage();
                        await bitmapImage.SetSourceAsync(thumbnail);
                        coverImages.Add(bitmapImage);
                    }
                }
                catch
                {
                    // ignored
                }
            }

            playlistItem.PlaylistMainCover = coverImages.ElementAtOrDefault(0);
            playlistItem.PlaylistSecondaryCover = coverImages.ElementAtOrDefault(1);
            playlistItem.PlaylistTertiaryCover = coverImages.ElementAtOrDefault(2);

            return playlistItem;
        }
    }
}
