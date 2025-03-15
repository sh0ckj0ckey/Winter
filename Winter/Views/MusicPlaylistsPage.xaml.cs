using System;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Winter.Models.MusicModels;
using Winter.Services.Interfaces;
using Winter.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Winter.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    [INotifyPropertyChanged]
#pragma warning disable MVVMTK0049 // Using [INotifyPropertyChanged] is not AOT compatible for WinRT
    public sealed partial class MusicPlaylistsPage : Page
#pragma warning restore MVVMTK0049 // Using [INotifyPropertyChanged] is not AOT compatible for WinRT
    {
        private readonly IMusicLibraryService _musicLibraryService;

        private readonly MusicPlaylistsViewModel _viewModel;

        private double _playlistItemWidth = 304;

        /// <summary>
        /// 歌单列表卡片自适应宽度
        /// </summary>
        public double PlaylistItemWidth
        {
            get => _playlistItemWidth;
            private set => SetProperty(ref _playlistItemWidth, value);
        }

        public MusicPlaylistsPage()
        {
            _musicLibraryService = App.Current.Services.GetRequiredService<IMusicLibraryService>();
            _viewModel = App.Current.Services.GetRequiredService<MusicPlaylistsViewModel>();
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is MusicPlaylistItem playlist)
            {
                var dialog = new ContentDialog
                {
                    XamlRoot = this.XamlRoot,
                    Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                    RequestedTheme = this.ActualTheme,
                };

                dialog.Content = new Controls.PlaylistContentDialogContentControl(playlist, () => { dialog?.Hide(); });

                _ = await dialog.ShowAsync();
            }
        }

        private void Button_PointerEntered(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                Storyboard? sb = btn.Resources["PointerEnterStoryboard"] as Storyboard;
                sb?.Begin();
            }
        }

        private void Button_PointerExited(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                Storyboard? sb = btn.Resources["PointerLeaveStoryboard"] as Storyboard;
                sb?.Begin();
            }
        }

        private void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            UpdateHeaderSeparatorBorder();
        }

        private void ScrollViewer_SizeChanged(object sender, Microsoft.UI.Xaml.SizeChangedEventArgs e)
        {
            try
            {
                double availableWidth = e.NewSize.Width - 28;
                int desiredItemCount = ((int)availableWidth / 304);
                this.PlaylistItemWidth = availableWidth / desiredItemCount;

                UpdateHeaderSeparatorBorder();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
        }

        private void UpdateHeaderSeparatorBorder()
        {
            if (PlaylistsScrollViewer is not null)
            {
                if (PlaylistsScrollViewer.ScrollableHeight <= 0)
                {
                    HeaderSeparatorBorder.Opacity = 0;
                    return;
                }

                var verticalOffset = PlaylistsScrollViewer.VerticalOffset;
                var maxOffset = Math.Min(16, PlaylistsScrollViewer.ScrollableHeight);

                if (maxOffset <= 1) return;

                // 透明度按滚动比例变化，从全透明到不透明
                double newOpacity = verticalOffset / maxOffset;
                if (newOpacity > 1)
                {
                    newOpacity = 1;
                }
                if (newOpacity < 0)
                {
                    newOpacity = 0;
                }

                HeaderSeparatorBorder.Opacity = newOpacity;
            }
        }

        private async void PlaylistPlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is MusicPlaylistItem playlist)
            {
                var playerViewModel = App.Current.Services.GetRequiredService<MusicPlayerViewModel>();
                playerViewModel?.ClearPlayingList();

                foreach (var musicFilePath in playlist.MusicFilePaths)
                {
                    try
                    {
                        var musicItem = await _musicLibraryService.GetMusicItemByPathAsync(musicFilePath);
                        if (musicItem is not null)
                        {
                            playerViewModel?.AddMusicToPlayingList(musicItem);
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.WriteLine(ex);
                    }
                }
            }
        }

        private async void PlaylistAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is MusicPlaylistItem playlist)
            {
                var playerViewModel = App.Current.Services.GetRequiredService<MusicPlayerViewModel>();

                foreach (var musicFilePath in playlist.MusicFilePaths)
                {
                    try
                    {
                        var musicItem = await _musicLibraryService.GetMusicItemByPathAsync(musicFilePath);
                        if (musicItem is not null)
                        {
                            playerViewModel?.AddMusicToPlayingList(musicItem);
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.WriteLine(ex);
                    }
                }
            }
        }
    }
}
