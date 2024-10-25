using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Windows.Storage;

namespace Winter.Helpers
{
    public class SettingsService : ObservableObject
    {
        private const string SETTING_NAME_APPEARANCE_INDEX = "AppearanceIndex";
        private const string SETTING_NAME_BACKDROP_INDEX = "BackdropIndex";
        private const string SETTING_NAME_ENABLE_LOCK = "EnableLock";

        private ApplicationDataContainer _localSettings = ApplicationData.Current.LocalSettings;

        public Action<int>? OnAppearanceSettingChanged { get; set; } = null;
        public Action<int>? OnBackdropSettingChanged { get; set; } = null;

        // 设置的应用程序的主题 0-System 1-Dark 2-Light
        private int _appearanceIndex = -1;
        public int AppearanceIndex
        {
            get
            {
                try
                {
                    if (_appearanceIndex < 0)
                    {
                        if (_localSettings.Values[SETTING_NAME_APPEARANCE_INDEX] == null)
                        {
                            _appearanceIndex = 0;
                        }
                        else if (_localSettings.Values[SETTING_NAME_APPEARANCE_INDEX]?.ToString() == "0")
                        {
                            _appearanceIndex = 0;
                        }
                        else if (_localSettings.Values[SETTING_NAME_APPEARANCE_INDEX]?.ToString() == "1")
                        {
                            _appearanceIndex = 1;
                        }
                        else if (_localSettings.Values[SETTING_NAME_APPEARANCE_INDEX]?.ToString() == "2")
                        {
                            _appearanceIndex = 2;
                        }
                        else
                        {
                            _appearanceIndex = 0;
                        }
                    }
                }
                catch { }
                if (_appearanceIndex < 0) _appearanceIndex = 0;
                return _appearanceIndex < 0 ? 0 : _appearanceIndex;
            }
            set
            {
                SetProperty(ref _appearanceIndex, value);
                ApplicationData.Current.LocalSettings.Values[SETTING_NAME_APPEARANCE_INDEX] = _appearanceIndex;
                OnAppearanceSettingChanged?.Invoke(_appearanceIndex);
            }
        }

        // 设置的应用程序的背景材质 0-Mica 1-Acrylic
        private int _backdropIndex = -1;
        public int BackdropIndex
        {
            get
            {
                try
                {
                    if (_backdropIndex < 0)
                    {
                        if (_localSettings.Values[SETTING_NAME_BACKDROP_INDEX] == null)
                        {
                            _backdropIndex = 0;
                        }
                        else if (_localSettings.Values[SETTING_NAME_BACKDROP_INDEX]?.ToString() == "0")
                        {
                            _backdropIndex = 0;
                        }
                        else if (_localSettings.Values[SETTING_NAME_BACKDROP_INDEX]?.ToString() == "1")
                        {
                            _backdropIndex = 1;
                        }
                        else
                        {
                            _backdropIndex = 0;
                        }
                    }
                }
                catch { }
                if (_backdropIndex < 0) _backdropIndex = 0;
                return _backdropIndex < 0 ? 0 : _backdropIndex;
            }
            set
            {
                SetProperty(ref _backdropIndex, value);
                ApplicationData.Current.LocalSettings.Values[SETTING_NAME_BACKDROP_INDEX] = _backdropIndex;
                OnBackdropSettingChanged?.Invoke(_backdropIndex);
            }
        }

        // 是否使用Windows Hello锁定
        private bool? _enableLock = null;
        public bool EnableLock
        {
            get
            {
                try
                {
                    if (_enableLock is null)
                    {
                        if (_localSettings.Values[SETTING_NAME_ENABLE_LOCK] == null)
                        {
                            _enableLock = false;
                        }
                        else if (_localSettings.Values[SETTING_NAME_ENABLE_LOCK]?.ToString() == "True")
                        {
                            _enableLock = true;
                        }
                        else
                        {
                            _enableLock = false;
                        }
                    }
                }
                catch { }
                _enableLock ??= false;
                return _enableLock == true;
            }
            set
            {
                SetProperty(ref _enableLock, value);
                ApplicationData.Current.LocalSettings.Values[SETTING_NAME_ENABLE_LOCK] = _enableLock;
            }
        }
    }

}
