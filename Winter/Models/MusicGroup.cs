using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Media.Imaging;

namespace Winter.Models
{
    public class MusicGroup : ObservableObject
    {
        /// <summary>
        /// 索引
        /// </summary>
        public char Key { get; set; } = '#';

        /// <summary>
        /// 封面路径
        /// </summary>
        public string Cover { get; set; } = "";

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
        /// 分组音乐
        /// </summary>
        public ObservableCollection<MusicItem> GroupedMusic { get; set; } = new ObservableCollection<MusicItem>();
    }
}
