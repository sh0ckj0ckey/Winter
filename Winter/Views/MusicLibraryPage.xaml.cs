using CommunityToolkit.WinUI.Controls;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using Winter.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Winter.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MusicLibraryPage : Page
    {
        private readonly MusicLibraryViewModel _viewModel;

        private readonly MusicPlayerViewModel _playerViewModel;

        public MusicLibraryPage()
        {
            _viewModel = App.Current.Services.GetRequiredService<MusicLibraryViewModel>();
            _playerViewModel = App.Current.Services.GetRequiredService<MusicPlayerViewModel>();
            this.InitializeComponent();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                _viewModel.FilteringArtistName = comboBox.SelectedIndex == 0 ? string.Empty : comboBox.SelectedItem?.ToString() ?? string.Empty;
            }
        }

        private void Segmented_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.IsLoaded && sender is Segmented segmented)
            {
                if (segmented.SelectedIndex == 0)
                {
                    ShowMusicGroupsGridStoryboard?.Begin();
                }
                else
                {
                    ShowMusicAlbumsGridStoryboard?.Begin();
                }
            }
        }
    }
}
