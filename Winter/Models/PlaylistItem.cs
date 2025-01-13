using CommunityToolkit.Mvvm.ComponentModel;

namespace Winter.Models
{
    /// <summary>
    /// 歌单
    /// </summary>
    public class PlaylistItem : ObservableObject
    {
        private string _title = "";
        private string _cover = "";
        private int _count = 0;

        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        /// <summary>
        /// 封面
        /// </summary>
        public string Cover
        {
            get => _cover;
            set => SetProperty(ref _cover, value);
        }

        /// <summary>
        /// 歌曲数量
        /// </summary>
        public int Count
        {
            get => _count;
            set => SetProperty(ref _count, value);
        }
    }
}
