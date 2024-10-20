using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Winter.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Winter.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private MainViewModel MainViewModel = null;

        private ContentDialog _tipsContentDialog = null;

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

            MainViewModel.Instance.ActSwitchAppTheme?.Invoke();
        }
    }
}
