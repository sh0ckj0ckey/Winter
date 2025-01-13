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
    public sealed partial class MusicPlaylistsPage : Page
    {
        private readonly MusicPlaylistsViewModel _viewModel;

        public MusicPlaylistsPage()
        {
            _viewModel = App.Current.Services.GetRequiredService<MusicPlaylistsViewModel>();

            this.InitializeComponent();
        }
    }
}
