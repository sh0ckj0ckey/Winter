using System.Collections.ObjectModel;
using Winter.Models;

namespace Winter.ViewModels
{
    public class MusicPlaylistsViewModel
    {
        /// <summary>
        /// 正在播放的音乐列表
        /// </summary>
        public ObservableCollection<PlayingItem> PlayingList { get; set; } = new()
        {
            new PlayingItem()
            {
                Title = "第一次爱的人",
                Duration = 233,
                Artist = "王心凌",
                Album = "爱你",
            },
            new PlayingItem()
            {
                Title = "原来这才是真的你",
                Duration = 263,
                Artist = "王心凌",
                Album = "Magic Cyndi",
            },
            new PlayingItem()
            {
                Title = "我很好，那么你呢？",
                Duration = 233,
                Artist = "王心凌",
                Album = "心电心",
            },
            new PlayingItem()
            {
                Title = "下一页的我",
                Duration = 349,
                Artist = "王心凌",
                Album = "黏黏黏黏",
            },
            new PlayingItem()
            {
                Title = "忘了我也不错",
                Duration = 269,
                Artist = "王心凌",
                Album = "爱不爱",
            },
            new PlayingItem()
            {
                Title = "從未到過的地方",
                Duration = 233,
                Artist = "王心凌",
                Album = "第十个王心凌",
            },
            new PlayingItem()
            {
                Title = "在青春迷失的咖啡館",
                Duration = 217,
                Artist = "王心凌",
                Album = "CYNDILOVES2SING",
            },
            new PlayingItem()
            {
                Title = "最想你的",
                Duration = 258,
                Artist = "王心凌",
                Album = "BITE BACK",
            },
        };

        public ObservableCollection<PlaylistItem> Playlists { get; set; } = new()
        {
            new PlaylistItem()
            {
                Title = "爱你",
                Cover = "ms-appx:///Assets/Test/1.jpg",
                Count = 18,
            },
            new PlaylistItem()
            {
                Title = "Magic Cyndi",
                Cover = "ms-appx:///Assets/Test/2.jpg",
                Count = 25,
            },
            new PlaylistItem()
            {
                Title = "心电心",
                Cover = "ms-appx:///Assets/Test/3.jpg",
                Count = 12,
            },
            new PlaylistItem()
            {
                Title = "爱不爱",
                Cover = "ms-appx:///Assets/Test/5.jpg",
                Count = 21,
            },
            new PlaylistItem()
            {
                Title = "CYNDILOVES2SING",
                Cover = "ms-appx:///Assets/Test/7.jpg",
                Count = 15,
            },
        };
    }
}
