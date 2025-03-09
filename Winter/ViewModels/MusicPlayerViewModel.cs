using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Winter.Models.MusicLibrary;

namespace Winter.ViewModels
{
    public class MusicPlayerViewModel : ObservableObject
    {
        private LibraryMusicItem? _playingMusic = null;

        /// <summary>
        /// 播放队列
        /// </summary>
        public ObservableCollection<LibraryMusicItem> PlayingMusicList { get; } = new();

        /// <summary>
        /// 当前正在播放的音乐
        /// </summary>
        public LibraryMusicItem? PlayingMusic
        {
            get => _playingMusic;
            private set => SetProperty(ref _playingMusic, value);
        }

        public void AddMusicToPlayingList(LibraryMusicItem musicItem)
        {
            PlayingMusicList.Add(musicItem);
        }

        public void AddMusicToPlayingList(IEnumerable<LibraryMusicItem> musicItems)
        {
            foreach (var musicItem in musicItems)
            {
                PlayingMusicList.Add(musicItem);
            }
        }

        public void RemoveMusicFromPlayingList(LibraryMusicItem musicItem)
        {
            PlayingMusicList.Remove(musicItem);
        }

        public void ClearPlayingList()
        {
            PlayingMusicList.Clear();
        }
    }
}
