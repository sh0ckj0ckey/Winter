using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Winter.Models.MusicModels;

namespace Winter.ViewModels
{
    public class MusicPlayerViewModel : ObservableObject
    {
        private BitmapImage? _emptyBitmapImage = null;

        private BitmapImage? _defaultBitmapImage = null;

        private MusicLibraryItem? _playingMusic = null;

        private BitmapImage? _playingMusicCover = null;

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
            set
            {
                SetProperty(ref _playingMusic, value);
                UpdatePlayerCover(value?.MusicFilePath);
            }
        }

        /// <summary>
        /// 当前正在播放的音乐封面
        /// </summary>
        public BitmapImage PlayingMusicCover
        {
            get
            {
                if (_playingMusicCover is null)
                {
                    if (this.PlayingMusic is null)
                    {
                        _emptyBitmapImage ??= new BitmapImage(new Uri("ms-appx:///Assets/Icons/WinterPlayerDefaultGray.png"))
                        {
                            DecodePixelType = DecodePixelType.Logical,
                            DecodePixelWidth = 512,
                        };

                        return _emptyBitmapImage;
                    }
                    else
                    {
                        _defaultBitmapImage ??= new BitmapImage(new Uri("ms-appx:///Assets/Icons/WinterPlaceholderGray.png"))
                        {
                            DecodePixelType = DecodePixelType.Logical,
                            DecodePixelWidth = 512,
                        };

                        return _defaultBitmapImage;
                    }
                }
                else
                {
                    return _playingMusicCover;
                }
            }
            private set => SetProperty(ref _playingMusicCover, value);
        }

        private async void UpdatePlayerCover(string? musicFilePath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(musicFilePath))
                {
                    _emptyBitmapImage ??= new BitmapImage(new Uri("ms-appx:///Assets/Icons/WinterPlayerDefaultGray.png"))
                    {
                        DecodePixelType = DecodePixelType.Logical,
                        DecodePixelWidth = 512,
                    };

                    this.PlayingMusicCover = _emptyBitmapImage;
                }

                try
                {
                    var file = await StorageFile.GetFileFromPathAsync(musicFilePath);
                    var thumbnail = await file.GetThumbnailAsync(ThumbnailMode.MusicView, 512, ThumbnailOptions.ResizeThumbnail);
                    if (thumbnail is not null && thumbnail.Type != ThumbnailType.Icon)
                    {
                        var bitmapImage = new BitmapImage();
                        bitmapImage.DecodePixelType = DecodePixelType.Logical;
                        bitmapImage.DecodePixelWidth = 512;
                        await bitmapImage.SetSourceAsync(thumbnail);

                        this.PlayingMusicCover = bitmapImage;
                    }
                    else
                    {
                        _defaultBitmapImage ??= new BitmapImage(new Uri("ms-appx:///Assets/Icons/WinterPlaceholderGray.png"))
                        {
                            DecodePixelType = DecodePixelType.Logical,
                            DecodePixelWidth = 512,
                        };

                        this.PlayingMusicCover = _defaultBitmapImage;
                    }
                }
                catch (Exception)
                {
                    _emptyBitmapImage ??= new BitmapImage(new Uri("ms-appx:///Assets/Icons/WinterPlayerDefaultGray.png"))
                    {
                        DecodePixelType = DecodePixelType.Logical,
                        DecodePixelWidth = 512,
                    };

                    this.PlayingMusicCover = _emptyBitmapImage;
                    throw;
                }
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
