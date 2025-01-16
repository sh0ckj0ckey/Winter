using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Winter.Models;
using Winter.Models.MusicLibrary;
using Winter.Services;
using Winter.Services.Interfaces;

namespace Winter.ViewModels
{
    public class MusicPlaylistsViewModel : ObservableObject
    {
        private readonly IMusicPlaylistsService _musicPlaylistsService;

        private bool _loading = false;

        /// <summary>
        /// 是否正在加载歌单列表
        /// </summary>
        public bool Loading
        {
            get => _loading;
            private set => SetProperty(ref _loading, value);
        }

        /// <summary>
        /// 歌单列表
        /// </summary>
        public ObservableCollection<LibraryPlaylistItem> Playlists { get; } = new();

        public MusicPlaylistsViewModel(IMusicPlaylistsService musicPlaylistsService)
        {
            _musicPlaylistsService = musicPlaylistsService;
        }

        public async void LoadMusicPlaylists()
        {
            this.Loading = true;
            
            try
            {
                await _musicPlaylistsService.LoadMusicPlaylistsAsync();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
            finally
            {
                this.Loading = false;

                GroupPlaylistsByTitle();
            }
        }

        private void GroupPlaylistsByTitle()
        {
            try
            {
                if (this.Loading)
                {
                    return;
                }

                this.Playlists.Clear();

                var allPlaylists = _musicPlaylistsService.GetAllPlaylistItems();

                // 按照首字母分组
                var orderedByPinyinList = allPlaylists.OrderBy(x => x.Title).ToList();

                foreach (var item in orderedByPinyinList)
                {
                    this.Playlists.Add(item);
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
        }
    }
}
