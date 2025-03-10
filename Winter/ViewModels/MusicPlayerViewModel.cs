using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Winter.Models.MusicModels;

namespace Winter.ViewModels
{
    public class MusicPlayerViewModel : ObservableObject
    {
        private MusicLibraryItem? _playingMusic = null;

        /// <summary>
        /// 播放队列
        /// </summary>
        public ObservableCollection<MusicLibraryItem> PlayingMusicList { get; } = new();

        /// <summary>
        /// 当前正在播放的音乐
        /// </summary>
        public MusicLibraryItem? PlayingMusic
        {
            get => _playingMusic;
            private set => SetProperty(ref _playingMusic, value);
        }

        public void AddMusicToPlayingList(MusicLibraryItem musicItem)
        {
            PlayingMusicList.Add(musicItem);
        }

        public void AddMusicToPlayingList(IEnumerable<MusicLibraryItem> musicItems)
        {
            foreach (var musicItem in musicItems)
            {
                PlayingMusicList.Add(musicItem);
            }
        }

        public void RemoveMusicFromPlayingList(MusicLibraryItem musicItem)
        {
            PlayingMusicList.Remove(musicItem);
        }

        public void ClearPlayingList()
        {
            PlayingMusicList.Clear();
        }
    }
}
