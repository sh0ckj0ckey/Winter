using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Windows.Storage.Search;
using Windows.Storage;
using Winter.Models;
using Windows.Storage.FileProperties;
using System.Collections.ObjectModel;

namespace Winter.ViewModels
{
    public class MusicLibraryViewModel : ObservableObject
    {
        /// <summary>
        /// 是否正在加载音乐库
        /// </summary>
        public bool Loading
        {
            get => _loading;
            private set => SetProperty(ref _loading, value);
        }
        private bool _loading = false;

        public ObservableCollection<MusicItem> AllMusic { get; set; } = new();

        public async void LoadMusicLibrary()
        {
            this.AllMusic.Clear();

            // 获取音乐库的存储文件夹
            StorageFolder musicFolder = KnownFolders.MusicLibrary;

            // 创建一个查询选项以查找所有音乐文件
            var queryOptions = new QueryOptions(CommonFileQuery.OrderByName, new List<string> { ".mp3", ".wav", ".aac", ".flac" });
            var query = musicFolder.CreateFileQueryWithOptions(queryOptions);

            // 获取所有音乐文件
            var files = await query.GetFilesAsync();

            // 遍历每个文件，获取其属性
            foreach (StorageFile file in files)
            {
                MusicProperties musicProperties = await file.Properties.GetMusicPropertiesAsync();
                StorageItemThumbnail thumbnail = await file.GetThumbnailAsync(ThumbnailMode.MusicView, 200, ThumbnailOptions.UseCurrentScale);

                this.AllMusic.Add(new MusicItem
                {
                    Title = string.IsNullOrWhiteSpace(musicProperties.Title) ? file.DisplayName : musicProperties.Title,
                    Artist = musicProperties.Artist,
                    Album = musicProperties.Album,
                    Duration = musicProperties.Duration.ToString(@"mm\:ss"),
                });
            }
        }
    }
}
