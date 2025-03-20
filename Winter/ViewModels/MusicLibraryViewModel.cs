using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Windows.Storage;
using Winter.Models;
using Winter.Models.MusicModels;
using Winter.Services.Interfaces;

namespace Winter.ViewModels
{
    public class MusicLibraryViewModel : ObservableObject
    {
        private readonly IMusicLibraryService _musicLibraryService;

        private bool _loading = false;

        private string _filteringArtistName = string.Empty;

        /// <summary>
        /// 是否正在加载音乐库
        /// </summary>
        public bool Loading
        {
            get => _loading;
            private set => SetProperty(ref _loading, value);
        }

        /// <summary>
        /// 过滤艺术家名字
        /// </summary>
        public string FilteringArtistName
        {
            get => _filteringArtistName;
            set
            {
                SetProperty(ref _filteringArtistName, value);

                GroupMusicByTitle(value);
                GroupMusicByAlbum(value);
            }
        }

        /// <summary>
        /// 排序的歌曲列表
        /// </summary>
        public ObservableCollection<MusicGroup> MusicGroups { get; } = new();

        /// <summary>
        /// 专辑列表
        /// </summary>
        public ObservableCollection<MusicAlbum> MusicAlbums { get; } = new();

        /// <summary>
        /// 艺术家名字列表
        /// </summary>
        public ObservableCollection<string> ArtistNames { get; } = new ObservableCollection<string>();

        public MusicLibraryViewModel(IMusicLibraryService musicLibraryService)
        {
            _musicLibraryService = musicLibraryService;
        }

        public async void LoadMusicLibrary()
        {
            this.Loading = true;

            try
            {
                await _musicLibraryService.InitializeMusicLibraryAsync();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
            finally
            {
                this.Loading = false;

                // 加载艺术家名字列表
                {
                    this.FilteringArtistName = string.Empty;
                    this.ArtistNames.Clear();
                    this.ArtistNames.Add("全部艺术家");
                    _musicLibraryService.GetAllMusicItems()
                       .Select(music => music.Artist.Split(';'))
                       .SelectMany(artists => artists)
                       .Select(artist => artist.Trim())
                       .Distinct()
                       .OrderBy(artists => artists)
                       .ToList()
                       .ForEach(x => this.ArtistNames.Add(x));
                }

                GroupMusicByTitle(this.FilteringArtistName);
                GroupMusicByAlbum(this.FilteringArtistName);
            }
        }

        private void GroupMusicByTitle(string filterArtistName)
        {
            try
            {
                if (this.Loading)
                {
                    return;
                }

                this.MusicGroups.Clear();

                IEnumerable<MusicLibraryItem>? musicToGroup = null;
                if (string.IsNullOrWhiteSpace(filterArtistName))
                {
                    musicToGroup = _musicLibraryService.GetAllMusicItems();
                }
                else
                {
                    musicToGroup = _musicLibraryService.GetAllMusicItems()
                                    .Where(music => music.Artist.Split(';').Any(a => a.Trim().Equals(filterArtistName, StringComparison.Ordinal)));
                }

                var groupedByPinyinList = (from item in musicToGroup
                                           group item by item.TitleFirstLetter into newItems
                                           select
                                           new MusicGroup
                                           {
                                               Key = newItems.Key,
                                               GroupedMusic = new(newItems)
                                           }).OrderBy(x => x.Key).ToList();

                groupedByPinyinList.ForEach(group => this.MusicGroups.Add(group));
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
        }

        private void GroupMusicByAlbum(string filterArtistName)
        {
            try
            {
                if (this.Loading)
                {
                    return;
                }

                this.MusicAlbums.Clear();

                IEnumerable<MusicLibraryItem>? musicToGroup = null;
                if (string.IsNullOrWhiteSpace(filterArtistName))
                {
                    musicToGroup = _musicLibraryService.GetAllMusicItems();
                }
                else
                {
                    musicToGroup = _musicLibraryService.GetAllMusicItems()
                                    .Where(music => !string.IsNullOrWhiteSpace(music.Album)
                                                                      && music.Artist.Split(';').Any(a => a.Trim().Equals(filterArtistName, StringComparison.Ordinal)));
                }

                var groupedByAlbumList = musicToGroup
                                                       .GroupBy(musicItem => new { musicItem.Album, musicItem.AlbumArtist })
                                                       .Select(g => new MusicAlbum
                                                       {
                                                           Title = g.Key.Album,
                                                           AlbumArtist = g.Key.AlbumArtist,
                                                           Year = g.Max(musicItem => musicItem.Year),
                                                           AlbumMusic = g.OrderBy(musicItem => musicItem.TrackNumber).ToList(),
                                                       })
                                                       .OrderByDescending(album => album.Year)
                                                       .ToList();

                groupedByAlbumList.ForEach(album =>
                {
                    this.MusicAlbums.Add(album);
                    _ = LoadAlbumCoverAsync(album);
                });
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
        }

        /// <summary>
        /// 取出专辑中第一首具有封面的音乐，将其封面作为专辑封面
        /// </summary>
        /// <param name="album"></param>
        /// <returns></returns>
        private async Task LoadAlbumCoverAsync(MusicAlbum album)
        {
            foreach (var music in album.AlbumMusic)
            {
                try
                {
                    if (album.AlbumCover.Image is not null)
                    {
                        break;
                    }

                    var fileToTertiaryCover = await StorageFile.GetFileFromPathAsync(music.MusicFilePath);
                    await album.AlbumCover.LoadCoverImageFromFile(fileToTertiaryCover, 72 * 2);
                }
                catch (Exception ex) { Trace.WriteLine(ex); }
            }
        }
    }
}
