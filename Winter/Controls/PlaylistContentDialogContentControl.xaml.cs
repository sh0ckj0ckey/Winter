using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graphics.Canvas;
using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Streams;
using Winter.Models.MusicLibrary;
using Winter.Services.Interfaces;
using Winter.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Winter.Controls
{
    [INotifyPropertyChanged]
#pragma warning disable MVVMTK0049 // Using [INotifyPropertyChanged] is not AOT compatible for WinRT
    public sealed partial class PlaylistContentDialogContentControl : UserControl
#pragma warning restore MVVMTK0049 // Using [INotifyPropertyChanged] is not AOT compatible for WinRT
    {
        private readonly IMusicLibraryService _musicLibraryService;

        private readonly Action _hideDialogContent;

        private LibraryPlaylistItem? _playlist = null;

        private bool _isLoadingPlaylistMusic = false;

        /// <summary>
        /// ���ڲ鿴�Ĳ����б�
        /// </summary>
        public LibraryPlaylistItem? Playlist
        {
            get => _playlist;
            private set => SetProperty(ref _playlist, value);
        }

        /// <summary>
        /// ���ڼ��ز����б��е������б�
        /// </summary>
        public bool IsLoadingPlaylistMusic
        {
            get => _isLoadingPlaylistMusic;
            private set => SetProperty(ref _isLoadingPlaylistMusic, value);
        }

        public ObservableCollection<LibraryMusicItem> MusicItems = new();

        public PlaylistContentDialogContentControl(LibraryPlaylistItem libraryPlaylistItem, Action hideContentDialog)
        {
            _musicLibraryService = App.Current.Services.GetRequiredService<IMusicLibraryService>();
            _hideDialogContent = hideContentDialog;

            this.Playlist = libraryPlaylistItem;

            this.InitializeComponent();

            LoadPlaylistMusic();
            DrawHeaderBackgroundImage();
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

        private void PlaylistCloseButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            _hideDialogContent?.Invoke();
        }

        private async void DrawHeaderBackgroundImage()
        {
            try
            {
                HeaderBackgroundBorder.Visibility = Microsoft.UI.Xaml.Visibility.Collapsed;

                if (this.Playlist is null)
                {
                    return;
                }

                List<SoftwareBitmap> images = [];

                if (this.Playlist.MusicFilePaths?.Count > 0)
                {
                    foreach (var path in this.Playlist.MusicFilePaths)
                    {
                        try
                        {
                            if (images.Count >= 6)
                            {
                                break;
                            }

                            var file = await StorageFile.GetFileFromPathAsync(path);
                            var thumbnail = await file.GetThumbnailAsync(ThumbnailMode.MusicView, 64, ThumbnailOptions.ResizeThumbnail);
                            if (thumbnail is not null && thumbnail.Type != ThumbnailType.Icon)
                            {
                                var softwareBitmap = await GenerateSoftwareBitmap(thumbnail);
                                if (softwareBitmap is not null)
                                {
                                    images.Add(softwareBitmap);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Trace.WriteLine(ex);
                        }
                    }
                }

                if (images.Count <= 0)
                {
                    return;
                }

                int targetWidth = 64;
                int targetHeight = 64;
                int imageCountPerRow = 12;
                int imageRowCount = 6;
                int imageGap = 2;

                int canvasWidth = (targetWidth * imageCountPerRow) + (imageGap * (imageCountPerRow + 1));
                int canvasHeight = (targetHeight * imageRowCount) + (imageGap * (imageRowCount + 1));

                int imageCount = images.Count;

                CanvasRenderTarget renderTarget = new CanvasRenderTarget(CanvasDevice.GetSharedDevice(), canvasWidth, canvasHeight, 96);
                using (CanvasDrawingSession drawingSession = renderTarget.CreateDrawingSession())
                {
                    drawingSession.Clear(Colors.Transparent);
                    for (int i = 0; i < imageRowCount; i++)
                    {
                        for (int j = 0; j < imageCountPerRow; j++)
                        {
                            SoftwareBitmap image = images[(j + (i % imageCount)) % imageCount];
                            int x = j * (targetWidth + imageGap);
                            int y = i * (targetHeight + imageGap);
                            var drawRect = new Rect(x, y, targetWidth, targetHeight);

                            using var canvasBitmap = CanvasBitmap.CreateFromSoftwareBitmap(CanvasDevice.GetSharedDevice(), image);
                            drawingSession.DrawImage(canvasBitmap, drawRect);

                            //drawingSession.FillRoundedRectangle(drawRect, 4, 4, Colors.Red);
                        }
                    }
                }

                var softwareBitmapImage = await SoftwareBitmap.CreateCopyFromSurfaceAsync(renderTarget);
                using var stream = new InMemoryRandomAccessStream();
                var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);
                encoder.SetSoftwareBitmap(softwareBitmapImage);
                await encoder.FlushAsync();
                stream.Seek(0);
                var bitmapImage = new BitmapImage();
                await bitmapImage.SetSourceAsync(stream);

                HeaderBackgroundImageBrush.ImageSource = bitmapImage;
                HeaderBackgroundBorder.Visibility = Microsoft.UI.Xaml.Visibility.Visible;
                HeaderBackgroundBorder.Opacity = 0;
                HeaderBackgroundEnterStoryboard?.Begin();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex);
            }
        }

        private async Task<SoftwareBitmap?> GenerateSoftwareBitmap(IRandomAccessStream randomAccessStream)
        {
            try
            {
                randomAccessStream.Seek(0);
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(randomAccessStream);
                var targetSoftwareBitmap = await decoder.GetSoftwareBitmapAsync(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
                targetSoftwareBitmap.DpiX = 96;
                targetSoftwareBitmap.DpiY = 96;
                return targetSoftwareBitmap;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex);
            }
            finally
            {
                randomAccessStream?.Seek(0);
                randomAccessStream?.Dispose();
            }

            return null;
        }

        private void PlaylistPlayButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            var playerViewModel = App.Current.Services.GetRequiredService<MusicPlayerViewModel>();
            playerViewModel?.ClearPlayingList();
            playerViewModel?.AddMusicToPlayingList(MusicItems.ToList());

            _hideDialogContent?.Invoke();
        }

        private void PlaylistAddButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            var playerViewModel = App.Current.Services.GetRequiredService<MusicPlayerViewModel>();
            playerViewModel?.AddMusicToPlayingList(MusicItems.ToList());

            _hideDialogContent?.Invoke();
        }
    }
}
