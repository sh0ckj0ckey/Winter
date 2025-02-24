using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using CommunityToolkit.Mvvm.ComponentModel;
using Winter.Models.MusicLibrary;
using System.Collections.ObjectModel;
using Microsoft.Extensions.DependencyInjection;
using Winter.Services.Interfaces;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Winter.Controls
{
    [INotifyPropertyChanged]
#pragma warning disable MVVMTK0049 // Using [INotifyPropertyChanged] is not AOT compatible for WinRT
    public sealed partial class PlaylistControl : UserControl
#pragma warning restore MVVMTK0049 // Using [INotifyPropertyChanged] is not AOT compatible for WinRT
    {
        private readonly IMusicLibraryService _musicLibraryService;

        private LibraryPlaylistItem? _playlist = null;

        private bool _isLoadingPlaylistMusic = false;

        /// <summary>
        /// 正在查看的播放列表
        /// </summary>
        public LibraryPlaylistItem? Playlist
        {
            get => _playlist;
            private set => SetProperty(ref _playlist, value);
        }

        /// <summary>
        /// 正在加载播放列表中的音乐列表
        /// </summary>
        public bool IsLoadingPlaylistMusic
        {
            get => _isLoadingPlaylistMusic;
            private set => SetProperty(ref _isLoadingPlaylistMusic, value);
        }

        public ObservableCollection<LibraryMusicItem> MusicItems = new();

        public PlaylistControl(LibraryPlaylistItem libraryPlaylistItem)
        {
            _musicLibraryService = App.Current.Services.GetRequiredService<IMusicLibraryService>();

            this.InitializeComponent();

            this.Playlist = libraryPlaylistItem;

            LoadPlaylistMusic();

            GenerateHeaderBackgroundImage();
        }

        private async void LoadPlaylistMusic()
        {
            this.IsLoadingPlaylistMusic = true;

            try
            {
                this.MusicItems.Clear();

                if (this.Playlist is null)
                {
                    return;
                }

                foreach (var musicFilePath in this.Playlist.MusicFilePaths)
                {
                    try
                    {
                        var musicItem = await _musicLibraryService.GetMusicItemByPathAsync(musicFilePath);
                        if (musicItem is not null)
                        {
                            this.MusicItems.Add(musicItem);
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.WriteLine(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex);
            }
            finally
            {
                this.IsLoadingPlaylistMusic = false;
            }
        }

        private async void GenerateHeaderBackgroundImage()
        {
            try
            {
                if (this.Playlist is null)
                {
                    return;
                }

                var images = new List<BitmapImage>();

                if (this.Playlist.PlaylistMainCover?.Image is not null)
                {
                    images.Add(this.Playlist.PlaylistMainCover.Image);
                }

                if (this.Playlist.PlaylistSecondaryCover?.Image is not null)
                {
                    images.Add(this.Playlist.PlaylistSecondaryCover.Image);
                }

                if (this.Playlist.PlaylistTertiaryCover?.Image is not null)
                {
                    images.Add(this.Playlist.PlaylistTertiaryCover.Image);
                }

                if (images.Count <= 0)
                {
                    return;
                }

                int targetWidth = 64;
                int targetHeight = 64;
                int imageCountPerRow = 10;

                var stackPanel = new StackPanel { Orientation = Orientation.Horizontal };

                for (int i = 0; i < imageCountPerRow; i++)
                {
                    var img = new Microsoft.UI.Xaml.Controls.Image
                    {
                        Source = images[i % images.Count],
                        Width = targetWidth,
                        Height = targetHeight,
                        Stretch = Stretch.UniformToFill,
                    };
                    stackPanel.Children.Add(img);
                }

                stackPanel.Measure(new Size(targetWidth * imageCountPerRow, targetHeight));
                stackPanel.Arrange(new Rect(0, 0, targetWidth * imageCountPerRow, targetHeight));

                var renderTargetBitmap = new RenderTargetBitmap();
                await renderTargetBitmap.RenderAsync(stackPanel);

                var pixelBuffer = await renderTargetBitmap.GetPixelsAsync();
                var finalImage = new BitmapImage();
                using var stream = new InMemoryRandomAccessStream();
                var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);
                encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied, 
                    (uint)renderTargetBitmap.PixelWidth, (uint)renderTargetBitmap.PixelHeight, 96, 96, pixelBuffer.ToArray());
                await encoder.FlushAsync();
                stream.Seek(0);
                await finalImage.SetSourceAsync(stream);

                this.HeaderBackgroundImageBrush.ImageSource = finalImage;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex);
            }
        }
    }
}
