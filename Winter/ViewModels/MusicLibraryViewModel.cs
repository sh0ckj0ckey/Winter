using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Media.Imaging;
using Winter.Models;
using Winter.Models.MusicLibrary;
using Winter.Services.Interfaces;

namespace Winter.ViewModels
{
    public class MusicLibraryViewModel : ObservableObject
    {
        private readonly IMusicLibraryService _musicLibraryService;

        private BitmapImage? _defaultAlbumCoverImage = null;

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
                    GroupMusicByAlbum();
                }
            }
        }

        /// <summary>
        /// 排序的歌曲列表
        /// </summary>
        public ObservableCollection<MusicGroup> GroupedMusic { get; } = new();

        /// <summary>
        /// 专辑列表
        /// </summary>
        public ObservableCollection<LibraryAlbumItem> MusicAlbums { get; } = new();

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
                await _musicLibraryService.LoadMusicLibraryAsync();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
            finally
            {
                this.Loading = false;

                this.ArtistNames.Clear();
                this.ArtistNames.Add("全部");
                _musicLibraryService.GetAllArtistNames().ForEach(x => this.ArtistNames.Add(x));

                if (this.GroupType == 0)
                {
                    GroupMusicByTitle();
                }
                else if (this.GroupType == 1)
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

                if (this.GroupedMusic.Count > 0)
                {
                    return;
                }

                this.GroupedMusic.Clear();

                var allMusic = _musicLibraryService.GetAllMusicItems();

                var orderedByPinyinList =
                    (from item in allMusic
                     group item by item.TitleFirstLetter into newItems
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

        private void GroupMusicByAlbum()
        {
            try
            {
                if (this.Loading)
                {
                    return;
                }

                if (this.MusicAlbums.Count > 0)
                {
                    return;
                }

                this.MusicAlbums.Clear();

                var allAlbums = _musicLibraryService.GetAllAlbumItems();

                var orderedByYearList = allAlbums.OrderByDescending(x => x.Year);

                foreach (var item in orderedByYearList)
                {
                    this.MusicAlbums.Add(item);

                    if (item.AlbumCover.Image is null)
                    {
                        _defaultAlbumCoverImage ??= new BitmapImage(new Uri("ms-appx:///Assets/Icon/Winter_placeholder.png"))
                        {
                            DecodePixelType = DecodePixelType.Logical,
                            DecodePixelWidth = 144,
                        };

                        item.AlbumCover.SetImage(_defaultAlbumCoverImage);
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
        }
    }
}
