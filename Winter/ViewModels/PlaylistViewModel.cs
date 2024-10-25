using System.Collections.ObjectModel;
using Winter.Models;

namespace Winter.ViewModels
{
    public class PlaylistViewModel
    {
        public ObservableCollection<PlaylistItem> Playlist { get; set; } = new ObservableCollection<PlaylistItem>()
        {
            new PlaylistItem()
            {
                Title = "第一次爱的人",
                Duration = 233,
                Artist = "王心凌",
                Album = "爱你",
            },
            new PlaylistItem()
            {
                Title = "原来这才是真的你",
                Duration = 263,
                Artist = "王心凌",
                Album = "Magic Cyndi",
            },
            new PlaylistItem()
            {
                Title = "我很好，那么你呢？",
                Duration = 233,
                Artist = "王心凌",
                Album = "心电心",
            },
            new PlaylistItem()
            {
                Title = "下一页的我",
                Duration = 349,
                Artist = "王心凌",
                Album = "黏黏黏黏",
            },
            new PlaylistItem()
            {
                Title = "忘了我也不错",
                Duration = 269,
                Artist = "王心凌",
                Album = "爱不爱",
            },
            new PlaylistItem()
            {
                Title = "從未到過的地方",
                Duration = 233,
                Artist = "王心凌",
                Album = "第十个王心凌",
            },
            new PlaylistItem()
            {
                Title = "在青春迷失的咖啡館",
                Duration = 217,
                Artist = "王心凌",
                Album = "CYNDILOVES2SING",
            },
            new PlaylistItem()
            {
                Title = "最想你的",
                Duration = 258,
                Artist = "王心凌",
                Album = "BITE BACK",
            },
        };
    }
}
