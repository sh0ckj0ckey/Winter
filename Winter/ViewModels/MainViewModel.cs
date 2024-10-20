﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Winter.Helpers;

namespace Winter.ViewModels
{
    public partial class MainViewModel
    {
        private static Lazy<MainViewModel> _lazyVM = new Lazy<MainViewModel>(() => new MainViewModel());
        public static MainViewModel Instance => _lazyVM.Value;

        public SettingsService AppSettings { get; set; } = new SettingsService();

        /// <summary>
        /// 控制主窗口根据当前的主题进行切换
        /// </summary>
        public Action ActSwitchAppTheme { get; set; } = null;

        /// <summary>
        /// 控制主窗口根据当前的设置更改背景材质
        /// </summary>
        public Action ActChangeBackdrop { get; set; } = null;

        /// <summary>
        /// 弹出提示框
        /// </summary>
        public Action<string, string> ActShowTipDialog { get; set; } = null;

        public MainViewModel()
        {
            AppSettings.OnAppearanceSettingChanged += (index) => { ActSwitchAppTheme?.Invoke(); };
            AppSettings.OnBackdropSettingChanged += (index) => { ActChangeBackdrop?.Invoke(); };
        }

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
