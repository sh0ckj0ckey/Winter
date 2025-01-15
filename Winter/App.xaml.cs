using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Winter.Services;
using Winter.Services.Interfaces;
using Winter.ViewModels;
using WinUIEx;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Winter
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        private readonly Microsoft.UI.Dispatching.DispatcherQueue _dispatcherQueue;

        /// <summary>
        /// Gets the current <see cref="App"/> instance in use
        /// </summary>
        public new static App Current => (App)Application.Current;

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> instance to resolve application services.
        /// </summary>
        public IServiceProvider Services { get; }

        public MainWindow? MainWindow { get; private set; }

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            Services = ConfigureServices();

            _dispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread();

            this.InitializeComponent();

            UnhandledException += (s, e) =>
            {
                System.Diagnostics.Trace.WriteLine(e);
                e.Handled = true;
            };
        }

        /// <summary>
        /// Configures the services for the application.
        /// </summary>
        private static IServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton<IContentDialogService, ContentDialogService>()
                .AddSingleton<ISettingsService, SettingsService>()
                .AddSingleton<IMusicLibraryService, MusicLibraryService>()
                .AddSingleton<IMusicPlaylistsService, MusicPlaylistsService>()
                .AddSingleton<MusicPlayerViewModel>()
                .AddTransient<MusicPlaylistsViewModel>()
                .AddTransient<MusicLibraryViewModel>()
                .BuildServiceProvider();
        }


        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            MainWindow = new MainWindow();
            MainWindow.Activate();
        }

        public void ShowMainWindow()
        {
            _dispatcherQueue.TryEnqueue(() =>
            {
                MainWindow?.Restore();
                MainWindow?.BringToFront();
                MainWindow?.Activate();
            });
        }
    }
}
