using System;
using System.Diagnostics;
using System.IO;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Windows.UI.ViewManagement;
using Winter.ViewModels;
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
        private UISettings _uiSettings;

        private Microsoft.UI.Dispatching.DispatcherQueue _dispatcherQueue = null;

        public MainWindow()
        {
            this.InitializeComponent();
            this.SystemBackdrop = MainViewModel.Instance.AppSettings.BackdropIndex == 1 ? new Microsoft.UI.Xaml.Media.DesktopAcrylicBackdrop() : new Microsoft.UI.Xaml.Media.MicaBackdrop();
            this.AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/Icon/Winter.ico"));
            this.PersistenceId = "HoneypotMainWindow";
            this.ExtendsContentIntoTitleBar = true;
            //this.SetTitleBar(AppTitleBar);

            _dispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread();

            MainViewModel.Instance.ActSwitchAppTheme = this.SwitchAppTheme;
            MainViewModel.Instance.ActChangeBackdrop = () =>
            {
                this.SystemBackdrop = MainViewModel.Instance.AppSettings.BackdropIndex == 1 ?
                                      new Microsoft.UI.Xaml.Media.DesktopAcrylicBackdrop() :
                                      new Microsoft.UI.Xaml.Media.MicaBackdrop();
            };

            // ����ϵͳ����仯
            ListenThemeColorChange();

            Debug.WriteLine("MainWindow Initialized");
        }

        /// <summary>
        /// MainFrame ������ɺ󣬵�������ҳ��ע���ݼ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMainFrameLoaded(object sender, RoutedEventArgs e)
        {
            // ��ʼ����ҳ��
            MainFrame.Navigate(typeof(MainPage));
        }

        /// <summary>
        /// �����ڹرպ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnMainWindowClosed(object sender, WindowEventArgs args)
        {
            Application.Current.Exit();
        }

        /// <summary>
        /// ����ϵͳ��ɫ���ñ������������Ϊ"����ϵͳ"ʱ�����л�
        /// </summary>
        private void ListenThemeColorChange()
        {
            _uiSettings = new UISettings();
            _uiSettings.ColorValuesChanged += (s, args) =>
            {
                if (MainViewModel.Instance.AppSettings.AppearanceIndex == 0)
                {
                    _dispatcherQueue.TryEnqueue(() =>
                    {
                        SwitchAppTheme();
                    });
                }
            };
        }

        /// <summary>
        /// �л�Ӧ�ó��������
        /// </summary>
        private void SwitchAppTheme()
        {
            try
            {
                // ���ñ�������ɫ
                bool isLight = true;
                if (MainViewModel.Instance.AppSettings.AppearanceIndex == 0) // ���� 0-System 1-Dark 2-Light
                {
                    var color = _uiSettings?.GetColorValue(UIColorType.Foreground) ?? Colors.Black;
                    var g = color.R * 0.299 + color.G * 0.587 + color.B * 0.114;
                    isLight = g < 100; // gԽС����ɫԽ��
                }
                else
                {
                    isLight = MainViewModel.Instance.AppSettings.AppearanceIndex == 2;
                }

                // �޸ı�������ť��ɫ
                // TitleBarHelper.UpdateTitleBar(App.MainWindow, isLight ? ElementTheme.Light : ElementTheme.Dark);
                var titleBar = App.MainWindow.AppWindow.TitleBar;
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

                // ����Ӧ�ó�����ɫ
                if (App.MainWindow.Content is FrameworkElement rootElement)
                {
                    if (MainViewModel.Instance.AppSettings.AppearanceIndex == 1)
                    {
                        rootElement.RequestedTheme = ElementTheme.Dark;
                    }
                    else if (MainViewModel.Instance.AppSettings.AppearanceIndex == 2)
                    {
                        rootElement.RequestedTheme = ElementTheme.Light;
                    }
                    else
                    {
                        rootElement.RequestedTheme = ElementTheme.Default;
                    }
                }
            }
            catch { }
        }
    }
}
