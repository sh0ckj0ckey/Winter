using System;
using CommunityToolkit.WinUI.Controls;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Windows.Foundation;
using Winter.Services;
using Winter.Services.Interfaces;
using WinUIEx;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Winter.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private int _previousSelectedIndex = 0;

        public MainPage()
        {
            if (App.Current.Services.GetService<IContentDialogService>() is ContentDialogService contentDialogService)
            {
                contentDialogService.XamlRootGetter = () => this.XamlRoot;
                contentDialogService.ElementThemeGetter = () => this.RequestedTheme;
            }

            this.Loaded += (_, _) =>
            {
                // 设置标题栏允许交互的区域
                SetNonClientArea();

                // 导航至默认页面
                MainNavSegmented.SelectedIndex = 0;
            };

            this.InitializeComponent();
        }

        private void Segmented_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is Segmented segmented)
            {
                int selectedIndex = segmented.SelectedIndex;
                Type pageType = selectedIndex switch
                {
                    0 => typeof(MusicPlaylistsPage),
                    1 => typeof(MusicLibraryPage),
                    2 => typeof(SettingsPage),
                    _ => typeof(MusicPlaylistsPage)
                };

                var slideNavigationTransitionEffect = selectedIndex - _previousSelectedIndex >= 0 ? SlideNavigationTransitionEffect.FromRight : SlideNavigationTransitionEffect.FromLeft;

                MainFrame.Navigate(pageType, null, new SlideNavigationTransitionInfo() { Effect = slideNavigationTransitionEffect });

                _previousSelectedIndex = selectedIndex;
            }
        }

        private void SetNonClientArea()
        {
            try
            {
                if (App.Current.MainWindow is null)
                {
                    throw new InvalidOperationException("Main window is not initialized");
                }

                Window window = App.Current.MainWindow;
                var nonClientInputSrc = InputNonClientPointerSource.GetForWindowId(window.AppWindow.Id);

                GeneralTransform transformTxtBox = MainNavSegmented.TransformToVisual(null);
                Rect bounds = transformTxtBox.TransformBounds(new Rect(0, 0, MainNavSegmented.ActualWidth, MainNavSegmented.ActualHeight));

                float scale = (float)HwndExtensions.GetDpiForWindow(window.GetWindowHandle()) / 96f;

                var transparentRect = new Windows.Graphics.RectInt32(
                    _X: (int)Math.Round(bounds.X * scale),
                    _Y: (int)Math.Round(bounds.Y * scale),
                    _Width: (int)Math.Round(bounds.Width * scale),
                    _Height: (int)Math.Round(bounds.Height * scale)
                );
                var rects = new Windows.Graphics.RectInt32[] { transparentRect };

                nonClientInputSrc.SetRegionRects(NonClientRegionKind.Passthrough, rects);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message);
            }
        }

    }
}
