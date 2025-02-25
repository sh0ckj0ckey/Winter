﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
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

            foreach (var file in files)
            {
                try
                {
                    var playlistItem = await LoadPlaylistFromFileAsync(file);

                    if (playlistItem is null)
                    {
                        continue;
                    }

                    _allPlaylistItems.Add(playlistItem);
                    _pathToPlaylistItem[playlistItem.PlaylistFilePath] = playlistItem;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }

            Debug.WriteLine("Loaded music playlists.");
        }

        public List<LibraryPlaylistItem> GetAllPlaylistItems() => _allPlaylistItems;

        public async Task<LibraryPlaylistItem?> GetPlaylistItemByPathAsync(string path)
        {
            _pathToPlaylistItem.TryGetValue(path, out LibraryPlaylistItem? playlistItem);

            if (playlistItem is null)
            {
                // 如果这个播放列表文件存在，但是不存在于列表中，则加载它到字典里，但是不添加到列表
                var file = await StorageFile.GetFileFromPathAsync(path);
                if (file is not null)
                {
                    playlistItem = await LoadPlaylistFromFileAsync(file);

                    if (playlistItem is not null)
                    {
                        _pathToPlaylistItem[path] = playlistItem;
                    }
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
            int coverIndex = 0;
            for (; coverIndex < musicFilePaths.Count; coverIndex++)
            {
                if (playlistItem.PlaylistMainCover.Image is not null)
                {
                    break;
                }

                await playlistItem.PlaylistMainCover.LoadCoverImageFromPath(musicFilePaths[coverIndex], 96 * 2);
            }
            for (; coverIndex < musicFilePaths.Count; coverIndex++)
            {
                if (playlistItem.PlaylistSecondaryCover.Image is not null)
                {
                    break;
                }

                await playlistItem.PlaylistSecondaryCover.LoadCoverImageFromPath(musicFilePaths[coverIndex], 88 * 2);
            }
            for (; coverIndex < musicFilePaths.Count; coverIndex++)
            {
                if (playlistItem.PlaylistTertiaryCover.Image is not null)
                {
                    break;
                }

                await playlistItem.PlaylistTertiaryCover.LoadCoverImageFromPath(musicFilePaths[coverIndex], 80 * 2);
            }

            return playlistItem;
        }
    }
}
