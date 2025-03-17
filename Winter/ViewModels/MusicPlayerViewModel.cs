using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Media.Imaging;
using Winter.Models.MusicModels;

namespace Winter.ViewModels
{
    public class MusicPlayerViewModel : ObservableObject
    {
        private BitmapImage? _defaultBitmapImage = null;

        private MusicLibraryItem? _playingMusic = null;

        private BitmapImage? _coverBitmapImage = null;

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
            private set
            {
                SetProperty(ref _playingMusic, value);

                UpdatePlayerCover(value?.MusicFilePath);
            }
        }

        /// <summary>
        /// 当前正在播放的音乐封面
        /// </summary>
        public BitmapImage CoverBitmapImage
        {
            get
            {
                if (_coverBitmapImage is null)
                {
                    _defaultBitmapImage ??= new BitmapImage(new Uri("ms-appx:///Assets/Icons/WinterPlaceholderGray.png"))
                    {
                        DecodePixelType = DecodePixelType.Logical,
                        DecodePixelWidth = 512,
                    };

                    return _defaultBitmapImage;
                }
                else
                {
                    return _coverBitmapImage;
                }
            }
            private set => SetProperty(ref _coverBitmapImage, value);
        }

        private async void UpdatePlayerCover(string? musicFilePath)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
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
