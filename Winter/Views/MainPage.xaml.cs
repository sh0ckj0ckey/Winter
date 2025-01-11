using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Windows.Foundation;
using Winter.Services;
using Winter.Services.Interfaces;
using Winter.ViewModels;
using Winter.Views.Controls;
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
        private MainViewModel? MainViewModel = null;

        private ContentDialog? _tipsContentDialog = null;

        int _previousSelectedIndex = 0;

        public MainPage()
        {
            MainViewModel = MainViewModel.Instance;

            if (App.Current.Services.GetService<IContentDialogService>() is ContentDialogService contentDialogService)
            {
                contentDialogService.XamlRootGetter = () => this.XamlRoot;
                contentDialogService.ElementThemeGetter = () => this.RequestedTheme;
            }

            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            PlaylistContainer.Child = new PlaylistsControl(MainViewModel!.PlaylistsVM);
            MusicLibraryContainer.Child = new MusicLibraryControl(MainViewModel!.MusicLibraryVM);

            SetNonClientArea();
        }

        private void SelectorBar_SelectionChanged(SelectorBar sender, SelectorBarSelectionChangedEventArgs args)
        {
            SelectorBarItem selectedItem = sender.SelectedItem;
            int currentSelectedIndex = sender.Items.IndexOf(selectedItem);
            Type pageType = currentSelectedIndex switch
            {
                1 => typeof(MusicLibraryPage),
                2 => typeof(SettingsPage),
                _ => typeof(PlaylistsPage),
            };

            var slideNavigationTransitionEffect = currentSelectedIndex - _previousSelectedIndex > 0 ? SlideNavigationTransitionEffect.FromRight : SlideNavigationTransitionEffect.FromLeft;

            ContentFrame.Navigate(pageType, null, new SlideNavigationTransitionInfo() { Effect = slideNavigationTransitionEffect });

            _previousSelectedIndex = currentSelectedIndex;
        }

        private void SetNonClientArea()
        {
            try
            {
                Window window = App.MainWindow;
                var nonClientInputSrc = InputNonClientPointerSource.GetForWindowId(window.AppWindow.Id);

                GeneralTransform transformTxtBox = SelectorBarNonClientAreaBorder.TransformToVisual(null);
                Rect bounds = transformTxtBox.TransformBounds(new Rect(0, 0, SelectorBarNonClientAreaBorder.ActualWidth, SelectorBarNonClientAreaBorder.ActualHeight));

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
