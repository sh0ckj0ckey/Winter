using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Winter.Models;
using Winter.Services.Interfaces;

namespace Winter.ViewModels
{
    public class MusicLibraryViewModel : ObservableObject
    {
        private readonly IMusicLibraryService _musicLibraryService;

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
        public ObservableCollection<MusicAlbum> MusicAlbums { get; } = new();

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

                this.GroupedMusic.Clear();

                var allMusic = _musicLibraryService.GetAllMusicItems();

                // 按照首字母分组
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

                this.MusicAlbums.Clear();

                var allMusic = _musicLibraryService.GetAllMusicItems();

                var orderedByArtistList =
                    (from item in allMusic
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
