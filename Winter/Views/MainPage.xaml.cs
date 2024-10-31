using System;
using Microsoft.UI.Xaml.Controls;
using Winter.ViewModels;
using Winter.Views.Controls;

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

        public MainPage()
        {
            MainViewModel = MainViewModel.Instance;

            _tipsContentDialog = new ContentDialog
            {
                XamlRoot = this.XamlRoot,
                Title = "",
                Content = "",
                CloseButtonText = "我知道了"
            };

            MainViewModel.Instance.ActShowTipDialog = async (title, content) =>
            {
                _tipsContentDialog.Title = title;
                _tipsContentDialog.Content = content;
                _tipsContentDialog.XamlRoot = this.XamlRoot;
                _tipsContentDialog.RequestedTheme = this.ActualTheme;
                await _tipsContentDialog.ShowAsync();
            };

            this.InitializeComponent();

            App.MainWindow.SwitchAppTheme();
        }

        private void Page_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            PlaylistContainer.Child = new PlaylistsControl(MainViewModel!.PlaylistsVM);
            MusicLibraryContainer.Child = new MusicLibraryControl(MainViewModel!.MusicLibraryVM);
        }
    }
}
