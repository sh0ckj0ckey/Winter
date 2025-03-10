using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Winter.Models.MusicModels;
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
        public ObservableCollection<MusicPlaylistItem> Playlists { get; } = new();

        public MusicPlaylistsViewModel(IMusicPlaylistsService musicPlaylistsService)
        {
            _musicPlaylistsService = musicPlaylistsService;
        }

        public async void LoadMusicPlaylists()
        {
            this.Loading = true;

            try
            {
                await _musicPlaylistsService.InitializeMusicPlaylistsAsync();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
            finally
            {
                this.Loading = false;

                OrderPlaylistsByTitle();
            }
        }

        private void OrderPlaylistsByTitle()
        {
            try
            {
                if (this.Loading)
                {
                    return;
                }

                this.Playlists.Clear();

                var allPlaylists = _musicPlaylistsService.GetAllPlaylistItems();

                // 按照歌单标题分组
                var orderedByTitleList = allPlaylists.OrderBy(x => x.Title).ToList();

                foreach (var item in orderedByTitleList)
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
