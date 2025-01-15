using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.FileProperties;
using Windows.Storage.Search;
using Windows.Storage;
using Winter.Helpers;
using Winter.Models.MusicLibrary;
using Winter.Services.Interfaces;
using System.IO;
using Microsoft.UI.Xaml.Media.Imaging;

namespace Winter.Services
{
    public class MusicPlaylistsService : IMusicPlaylistsService
    {
        private readonly List<LibraryPlaylistItem> _allPlaylistItems = [];

        private readonly Dictionary<string, LibraryPlaylistItem> _pathToPlaylistItem = new();

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

            // 获取所有播放列表文件
            var files = await query.GetFilesAsync();

            // 定义音乐文件的扩展名
            var musicExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ".mp3", ".wav", ".flac", ".aac", ".m4a", ".wma" };

            // 遍历每个文件，加载播放列表
            var tasks = files.Select(async file =>
            {
                try
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

                        var extension = Path.GetExtension(line);
                        if (musicExtensions.Contains(extension))
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

                    playlistItem.TitleFirstLetter = PinyinHelper.GetFirstSpell(playlistItem.Title);

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

                            uint iamgeSize = coverImages.Count switch
                            {
                                0 => 96 * 2,
                                1 => 88 * 2,
                                2 => 80 * 2,
                                _ => 96 * 2
                            };

                            var musicFile = await StorageFile.GetFileFromPathAsync(musicFilePath);
                            var thumbnail = await musicFile.GetThumbnailAsync(ThumbnailMode.MusicView, iamgeSize, ThumbnailOptions.UseCurrentScale);

                            if (thumbnail is not null)
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

                    _pathToPlaylistItem[file.Path] = playlistItem;
                    return playlistItem;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    return null;
                }
            }).ToList();

            var playlistItems = await Task.WhenAll(tasks);

            _allPlaylistItems.Clear();

            _allPlaylistItems.AddRange(playlistItems.Where(item => item != null)!);

            Debug.WriteLine("Loaded music playlists.");
        }

        public List<LibraryPlaylistItem> GetAllPlaylistItems() => _allPlaylistItems;

        public LibraryPlaylistItem? GetPlaylistItemByPath(string path)
        {
            _pathToPlaylistItem.TryGetValue(path, out LibraryPlaylistItem? playlistItem);
            return playlistItem;
        }
    }
}
