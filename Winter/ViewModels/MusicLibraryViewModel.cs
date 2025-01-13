using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Search;
using Winter.Helpers;
using Winter.Models;

namespace Winter.ViewModels
{
    public class MusicLibraryViewModel : ObservableObject
    {
        private bool _loading = false;

        private int _groupType = 0;

        /// <summary>
        /// 是否正在加载音乐库
        /// </summary>
        public bool Loading
        {
            get => _loading;
            private set => SetProperty(ref _loading, value);
        }

        /// <summary>
        /// 列表分组依据
        /// </summary>
        public int GroupType
        {
            get => _groupType;
            set
            {
                SetProperty(ref _groupType, value);

                if (value == 0)
                {
                    GroupMusicByTitle();
                }
                else if (value == 1)
                {
                    GroupMusicByArtist();
                }
                else if (value == 2)
                {
                    GroupMusicByAlbum();
                }
            }
        }

        /// <summary>
        /// 所有的歌曲
        /// </summary>
        private readonly List<MusicItem> _allMusic = [];

        /// <summary>
        /// 排序的歌曲列表
        /// </summary>
        public ObservableCollection<MusicGroup> GroupedMusic { get; } = new();

        /// <summary>
        /// 专辑列表
        /// </summary>
        public ObservableCollection<MusicAlbum> MusicAlbums { get; } = new();

        public async void LoadMusicLibrary()
        {
            this.Loading = true;

            try
            {
                _allMusic.Clear();

                Debug.WriteLine("Loading music library...");

                // 获取音乐库的存储文件夹
                StorageFolder musicFolder = KnownFolders.MusicLibrary;

                // 创建一个查询选项以查找所有音乐文件
                var queryOptions = new QueryOptions(CommonFileQuery.OrderByName, new List<string> { ".mp3", ".wav", ".aac", ".flac" });
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

                        var musicItem = new MusicItem
                        {
                            Title = string.IsNullOrWhiteSpace(musicProperties.Title) ? file.DisplayName : musicProperties.Title,
                            Album = string.IsNullOrWhiteSpace(musicProperties.Album) ? "未知专辑" : musicProperties.Album,
                            AlbumArtist = string.IsNullOrWhiteSpace(musicProperties.AlbumArtist) ? "未知艺术家" : musicProperties.AlbumArtist,
                            Duration = musicProperties.Duration.ToString(@"mm\:ss"),
                            Year = musicProperties.Year,
                            TrackNumber = musicProperties.TrackNumber,
                        };

                        string artist = string.Join("&", musicProperties.Producers);
                        musicItem.Artist = string.IsNullOrWhiteSpace(artist) ? (string.IsNullOrWhiteSpace(musicProperties.Artist) ? "未知艺术家" : musicProperties.Artist) : artist;

                        musicItem.FirstLetter = PinyinHelper.GetFirstSpell(musicItem.Title);

                        return musicItem;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                        return null;
                    }
                }).ToList();

                var musicItems = await Task.WhenAll(tasks);

                _allMusic.Clear();

                if (musicItems is not null)
                {
                    _allMusic.AddRange(musicItems.Where(item => item != null)!);
                }

                Debug.WriteLine("Loaded music library.");
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
            finally
            {
                this.Loading = false;

                if (this.GroupType == 0)
                {
                    GroupMusicByTitle();
                }
                else if (this.GroupType == 1)
                {
                    GroupMusicByArtist();
                }
                else if (this.GroupType == 2)
                {
                    GroupMusicByAlbum();
                }
            }
        }

        private void GroupMusicByTitle()
        {
            try
            {
                if (this.Loading)
                {
                    return;
                }

                this.GroupedMusic.Clear();

                // 按照首字母分组
                var orderedByPinyinList =
                    (from item in _allMusic
                     group item by item.FirstLetter into newItems
                     select
                     new MusicGroup
                     {
                         Key = newItems.Key,
                         GroupedMusic = new(newItems.ToList())
                     }).OrderBy(x => x.Key).ToList();

                foreach (var item in orderedByPinyinList)
                {
                    this.GroupedMusic.Add(item);
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
        }

        private void GroupMusicByArtist()
        {
            try
            {
                if (this.Loading)
                {
                    return;
                }

                this.GroupedMusic.Clear();

                // 按照艺术家分组
                var orderedByArtistList =
                    (from item in _allMusic
                     group item by item.Artist into newItems
                     select
                     new MusicGroup
                     {
                         Key = newItems.Key,
                         GroupedMusic = new(newItems.ToList())
                     }).OrderBy(x => x.Key).ToList();

                foreach (var item in orderedByArtistList)
                {
                    this.GroupedMusic.Add(item);
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
        }

        private void GroupMusicByAlbum()
        {
            try
            {
                if (this.Loading)
                {
                    return;
                }

                this.MusicAlbums.Clear();

                var orderedByArtistList =
                    (from item in _allMusic
                     group item by new { item.Album, item.Year } into newItems
                     select
                     new MusicAlbum
                     {
                         Title = newItems.Key.Album,
                         Year = newItems.Key.Year,
                         Music = new(newItems.OrderBy(x => x.TrackNumber).ToList())
                     }).OrderByDescending(x => x.Year).ThenBy(x => x.Title).ToList();

                foreach (var item in orderedByArtistList)
                {
                    this.MusicAlbums.Add(item);
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
        }
    }
}
