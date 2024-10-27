using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Winter.ViewModels;
using Microsoft.UI.Xaml.Media.Animation;
using System.Diagnostics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Winter.Views.Controls
{
    public sealed partial class PlaylistsControl : UserControl
    {
        private MainViewModel? MainViewModel = null;

        public PlaylistsControl()
        {
            MainViewModel = MainViewModel.Instance;

            this.InitializeComponent();
        }

        private void CardGrid_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            try
            {
                if (sender is Grid grid)
                {
                    Storyboard? sb = grid.Resources["PointerOverStoryboard"] as Storyboard;
                    sb?.Begin();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void CardGrid_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            try
            {
                if (sender is Grid grid)
                {
                    Storyboard? sb = grid.Resources["PointerExitStoryboard"] as Storyboard;
                    sb?.Begin();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void CardGrid_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            try
            {
                if (sender is Grid grid)
                {
                    Storyboard? sb = grid.Resources["PointerPressStoryboard"] as Storyboard;
                    sb?.Begin();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void CardGrid_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            try
            {
                if (sender is Grid grid)
                {
                    Storyboard? sb = grid.Resources["PointerReleaseStoryboard"] as Storyboard;
                    sb?.Begin();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
