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
        public string Key { get; set; } = "#";

        /// <summary>
        /// 分组音乐
        /// </summary>
        public ObservableCollection<MusicItem> GroupedMusic { get; set; } = new();
    }
}
