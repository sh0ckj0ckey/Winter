using CommunityToolkit.Mvvm.ComponentModel;

namespace Winter.Models
{
    /// <summary>
    /// 歌单
    /// </summary>
    public class PlaylistItem : ObservableObject
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
        private string _title = "";

        /// <summary>
        /// 封面
        /// </summary>
        public string Cover
        {
            get => _cover;
            set => SetProperty(ref _cover, value);
        }
        private string _cover = "";

        /// <summary>
        /// 歌曲数量
        /// </summary>
        public int Count
        {
            get => _count;
            set => SetProperty(ref _count, value);
        }
        private int _count = 0;

        /// <summary>
        /// 封面背景颜色
        /// </summary>
        public string BackgroundColor
        {
            get => _backgroundColor;
            set => SetProperty(ref _backgroundColor, value);
        }
        private string _backgroundColor = "#00000000";

        /// <summary>
        /// 封面字体颜色
        /// </summary>
        public string ForegroundColor
        {
            get => _foregroundColor;
            set => SetProperty(ref _foregroundColor, value);
        }
        private string _foregroundColor = "#00000000";
    }
}
