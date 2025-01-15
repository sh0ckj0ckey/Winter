using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Media.Imaging;
using Winter.Models.MusicLibrary;

namespace Winter.Models
{
    public class MusicAlbum : ObservableObject
    {
        /// <summary>
        /// 专辑名
        /// </summary>
        public string Title { get; set; } = "";

        /// <summary>
        /// 艺术家
        /// </summary>
        public string Artist { get; set; } = "";

        /// <summary>
        /// 年份
        /// </summary>
        public uint Year { get; set; } = 0;

        /// <summary>
        /// 封面图
        /// </summary>
        public BitmapImage? CoverImage
        {
            get => _coverImage;
            set => SetProperty(ref _coverImage, value);
        }
        private BitmapImage? _coverImage = null;

        /// <summary>
        /// 音乐
        /// </summary>
        public ObservableCollection<LibraryMusicItem> Music { get; set; } = new();
    }
}
