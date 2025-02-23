using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Windows.UI.ViewManagement;
using Winter.Services.Interfaces;
using Winter.Views;
using WinUIEx;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Winter
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : WindowEx
    {
        private readonly UISettings _uiSettings;

        private readonly ISettingsService _settingsService;

        public MainWindow()
        {
            _settingsService = App.Current.Services.GetRequiredService<ISettingsService>();

            this.InitializeComponent();
            this.PersistenceId = "WinterMainWindow";
            this.ExtendsContentIntoTitleBar = true;
            //this.SetTitleBar(AppTitleBar);

            string iconPath = Path.Combine(AppContext.BaseDirectory, "Assets/Icons/Winter.ico");
            this.SetIcon(iconPath);
            this.SetTaskBarIcon(Icon.FromFile(iconPath));

            UpdateAppBackdrop();

            _settingsService.AppearanceSettingChanged += (_, _) =>
            {
                UpdateAppTheme();
            };

            _settingsService.BackdropSettingChanged += (_, _) =>
            {
                UpdateAppBackdrop();
            };

            // 监听系统主题变化
            var dispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread();
            _uiSettings = new UISettings();
            _uiSettings.ColorValuesChanged += (s, args) =>
            {
                if (_settingsService.AppearanceIndex == 0)
                {
                    dispatcherQueue.TryEnqueue(() =>
                    {
                        UpdateAppTheme();
                    });
                }
            };

            // 首次启动设置默认窗口尺寸
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            if (localSettings.Values["firstRun"] == null)
            {
                localSettings.Values["firstRun"] = true;
                this.Height = 680;
                this.Width = 960;
                this.CenterOnScreen();
            }
        }

        /// <summary>
        /// MainFrame 加载完成后，导航到首页，注册快捷键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMainFrameLoaded(object sender, RoutedEventArgs e)
        {
            // 初始导航页面
            MainFrame.Navigate(typeof(MainPage));

            UpdateAppTheme();
        }

        /// <summary>
        /// 主窗口关闭后
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnMainWindowClosed(object sender, WindowEventArgs args)
        {
            Application.Current.Exit();
        }

        /// <summary>
        /// 更新应用程序的主题
        /// </summary>
        private void UpdateAppTheme()
        {
            try
            {
                // 设置标题栏颜色 主题 0-System 1-Dark 2-Light
                bool isLight = true;
                if (_settingsService.AppearanceIndex == 0)
                {
                    var color = _uiSettings?.GetColorValue(UIColorType.Foreground) ?? Colors.Black;

                    // g越小，颜色越深
                    var g = color.R * 0.299 + color.G * 0.587 + color.B * 0.114;
                    isLight = g < 100;
                }
                else
                {
                    isLight = _settingsService.AppearanceIndex == 2;
                }

                // 修改标题栏按钮颜色
                // TitleBarHelper.UpdateTitleBar(App.MainWindow, isLight ? ElementTheme.Light : ElementTheme.Dark);
                var titleBar = this.AppWindow.TitleBar;
                // Set active window colors
                // Note: No effect when app is running on Windows 10 since color customization is not supported.
                titleBar.ForegroundColor = isLight ? Colors.Black : Colors.White;
                titleBar.BackgroundColor = Colors.Transparent;
                titleBar.ButtonForegroundColor = isLight ? Colors.Black : Colors.White;
                titleBar.ButtonBackgroundColor = Colors.Transparent;
                titleBar.ButtonHoverForegroundColor = isLight ? Colors.Black : Colors.White;
                titleBar.ButtonHoverBackgroundColor = isLight ? Windows.UI.Color.FromArgb(10, 0, 0, 0) : Windows.UI.Color.FromArgb(16, 255, 255, 255);
                titleBar.ButtonPressedForegroundColor = isLight ? Colors.Black : Colors.White;
                titleBar.ButtonPressedBackgroundColor = isLight ? Windows.UI.Color.FromArgb(08, 0, 0, 0) : Windows.UI.Color.FromArgb(10, 255, 255, 255);

                // Set inactive window colors
                // Note: No effect when app is running on Windows 10 since color customization is not supported.
                titleBar.InactiveForegroundColor = Colors.Gray;
                titleBar.InactiveBackgroundColor = Colors.Transparent;
                titleBar.ButtonInactiveForegroundColor = Colors.Gray;
                titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;

                // 设置应用程序颜色
                if (this.Content is FrameworkElement rootElement)
                {
                    if (_settingsService.AppearanceIndex == 1)
                    {
                        rootElement.RequestedTheme = ElementTheme.Dark;
                    }
                    else if (_settingsService.AppearanceIndex == 2)
                    {
                        rootElement.RequestedTheme = ElementTheme.Light;
                    }
                    else
                    {
                        rootElement.RequestedTheme = ElementTheme.Default;
                    }
                }
            }
            catch (Exception ex) { System.Diagnostics.Trace.WriteLine(ex); }
        }

        /// <summary>
        /// 更新应用程序的背景
        /// </summary>
        private void UpdateAppBackdrop()
        {
            this.SystemBackdrop = _settingsService.BackdropIndex == 1 ?
                new Microsoft.UI.Xaml.Media.DesktopAcrylicBackdrop() :
                new Microsoft.UI.Xaml.Media.MicaBackdrop();
        }
    }
}
