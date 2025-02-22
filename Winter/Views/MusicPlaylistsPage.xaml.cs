using System;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
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
            _viewModel = App.Current.Services.GetRequiredService<MusicPlaylistsViewModel>();
            this.InitializeComponent();
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
                double availableWidth = e.NewSize.Width - 20;
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
    }
}
