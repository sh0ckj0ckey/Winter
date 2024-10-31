using System;
using System.Diagnostics;
using Winter.Helpers;

namespace Winter.ViewModels
{
    public partial class MainViewModel
    {
        private readonly static Lazy<MainViewModel> _lazyVM = new(() => new MainViewModel());
        public static MainViewModel Instance => _lazyVM.Value;

        public SettingsService AppSettings { get; set; } = new SettingsService();

        /// <summary>
        /// 歌单
        /// </summary>
        public PlaylistsViewModel PlaylistsVM { get; set; } = new();

        /// <summary>
        /// 音乐库
        /// </summary>
        public MusicLibraryViewModel MusicLibraryVM { get; set; } = new();

        /// <summary>
        /// 弹出提示框
        /// </summary>
        public Action<string, string>? ActShowTipDialog { get; set; } = null;

        /// <summary>
        /// 弹出对话框提示用户特定内容
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        public void ShowTipsContentDialog(string title, string content)
        {
            try
            {
                ActShowTipDialog?.Invoke(title, content);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }

}
