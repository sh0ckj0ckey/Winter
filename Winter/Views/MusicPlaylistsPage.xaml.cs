using System;
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
    public sealed partial class MusicPlaylistsPage : Page
    {
        private readonly MusicPlaylistsViewModel _viewModel;

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
            if (sender is ScrollViewer scrollViewer)
            {
                var verticalOffset = scrollViewer.VerticalOffset;
                var maxOffset = Math.Min(16, scrollViewer.ScrollableHeight);

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
