using System;
using CommunityToolkit.WinUI.Controls;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Composition;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Hosting;
using Microsoft.UI.Xaml.Media.Animation;
using Winter.Services.Interfaces;
using Winter.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Winter.Views
{
    public sealed partial class MusicPlayerView : UserControl
    {
        private readonly ISettingsService _settingsService;

        private readonly MusicPlayerViewModel _viewModel;

        private readonly Visual _visual;

        public MusicPlayerView()
        {
            this.InitializeComponent();

            _settingsService = App.Current.Services.GetRequiredService<ISettingsService>();
            _viewModel = App.Current.Services.GetRequiredService<MusicPlayerViewModel>();
            _visual = ElementCompositionPreview.GetElementVisual(this);
        }

        private void Segmented_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is Segmented segmented)
            {
                bool showPlayingList = segmented.SelectedIndex == 1;

                if (PlayingStackPanel is not null)
                {
                    PlayingStackPanel.Visibility = showPlayingList ? Microsoft.UI.Xaml.Visibility.Collapsed : Microsoft.UI.Xaml.Visibility.Visible;
                }

                if (PlayingListGrid is not null)
                {
                    PlayingListGrid.Visibility = showPlayingList ? Microsoft.UI.Xaml.Visibility.Visible : Microsoft.UI.Xaml.Visibility.Collapsed;
                }

                if (PlayingStackPanel is null || PlayingListGrid is null)
                {
                    return;
                }

                ConnectedAnimationService.GetForCurrentView().DefaultDuration = TimeSpan.FromSeconds(0.6);
                ConnectedAnimationService.GetForCurrentView().DefaultEasingFunction = CompositionEasingFunction.CreateCubicBezierEasingFunction(_visual.Compositor, new(0.41f, 0.52f), new(0.0f, 0.94f));

                if (showPlayingList)
                {
                    var coverAnimation = ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("cover", ConnectedPlayingMusicCoverBorder);
                    coverAnimation.Configuration = new BasicConnectedAnimationConfiguration();
                    coverAnimation.IsScaleAnimationEnabled = true;
                    var titleAnimation = ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("title", ConnectedPlayingMusicTitleTextBlock);
                    titleAnimation.Configuration = new BasicConnectedAnimationConfiguration();
                    titleAnimation.IsScaleAnimationEnabled = true;
                    var artistAnimation = ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("artist", ConnectedPlayingMusicArtistTextBlock);
                    artistAnimation.Configuration = new BasicConnectedAnimationConfiguration();
                    artistAnimation.IsScaleAnimationEnabled = true;

                    coverAnimation?.TryStart(DestinationPlayingMusicCoverBorder);
                    titleAnimation?.TryStart(DestinationPlayingMusicTitleTextBlock);
                    artistAnimation?.TryStart(DestinationPlayingMusicArtistTextBlock);

                    ShowPlayingListStoryboard?.Begin();
                }
                else
                {
                    var coverAnimation = ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("coverBack", DestinationPlayingMusicCoverBorder);
                    coverAnimation.Configuration = new BasicConnectedAnimationConfiguration();
                    coverAnimation.IsScaleAnimationEnabled = true;
                    var titleAnimation = ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("titleBack", DestinationPlayingMusicTitleTextBlock);
                    titleAnimation.Configuration = new BasicConnectedAnimationConfiguration();
                    titleAnimation.IsScaleAnimationEnabled = true;
                    var artistAnimation = ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("artistBack", DestinationPlayingMusicArtistTextBlock);
                    artistAnimation.Configuration = new BasicConnectedAnimationConfiguration();
                    artistAnimation.IsScaleAnimationEnabled = true;

                    coverAnimation?.TryStart(ConnectedPlayingMusicCoverBorder);
                    titleAnimation?.TryStart(ConnectedPlayingMusicTitleTextBlock);
                    artistAnimation?.TryStart(ConnectedPlayingMusicArtistTextBlock);

                    HidePlayingListStoryboard?.Begin();
                }
            }
        }

        private void PlayingNoRandomButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            _settingsService.PlayingRandomMode = PlayingRandomMode.Random;
        }

        private void PlayingRandomButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            _settingsService.PlayingRandomMode = PlayingRandomMode.NoRandom;
        }

        private void PlayingNoRepeatButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            _settingsService.PlayingRepeatMode = PlayingRepeatMode.RepeatAll;
        }

        private void PlayingRepeatAllButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            _settingsService.PlayingRepeatMode = PlayingRepeatMode.RepeatOne;
        }

        private void PlayingRepeatOneButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            _settingsService.PlayingRepeatMode = PlayingRepeatMode.NoRepeat;
        }
    }
}
