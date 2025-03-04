using System;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using Windows.Storage;
using Winter.Services.Interfaces;

namespace Winter.Services
{
    public class SettingsService : ObservableObject, ISettingsService
    {
        private const string SETTING_NAME_APPEARANCE_INDEX = "AppearanceIndex";

        private const string SETTING_NAME_BACKDROP_INDEX = "BackdropIndex";

        private const string SETTING_NAME_RANDOM_INDEX = "RandomIndex";

        private const string SETTING_NAME_REPEAT_INDEX = "RepeatIndex";

        private ApplicationDataContainer _localSettings = ApplicationData.Current.LocalSettings;

        public event EventHandler<int>? AppearanceSettingChanged;

        public event EventHandler<int>? BackdropSettingChanged;

        private int _appearanceIndex = -1;

        private int _backdropIndex = -1;

        private int _playingRandomMode = -1;

        private int _playingRepeatMode = -1;

        /// <summary>
        /// 设置的应用程序的主题 0-System 1-Dark 2-Light
        /// </summary>
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
                catch (Exception ex) { Trace.WriteLine(ex); }
                if (_appearanceIndex < 0) _appearanceIndex = 0;
                return _appearanceIndex < 0 ? 0 : _appearanceIndex;
            }
            set
            {
                SetProperty(ref _appearanceIndex, value);
                ApplicationData.Current.LocalSettings.Values[SETTING_NAME_APPEARANCE_INDEX] = _appearanceIndex;
                AppearanceSettingChanged?.Invoke(this, _appearanceIndex);
            }
        }

        /// <summary>
        /// 设置的应用程序的背景材质 0-Mica 1-Acrylic
        /// </summary>
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
                catch (Exception ex) { Trace.WriteLine(ex); }
                if (_backdropIndex < 0) _backdropIndex = 0;
                return _backdropIndex < 0 ? 0 : _backdropIndex;
            }
            set
            {
                SetProperty(ref _backdropIndex, value);
                ApplicationData.Current.LocalSettings.Values[SETTING_NAME_BACKDROP_INDEX] = _backdropIndex;
                BackdropSettingChanged?.Invoke(this, _backdropIndex);
            }
        }

        /// <summary>
        /// 随机播放模式
        /// </summary>
        public PlayingRandomMode PlayingRandomMode
        {
            get
            {
                try
                {
                    if (_playingRandomMode < 0)
                    {
                        if (_localSettings.Values[SETTING_NAME_RANDOM_INDEX] == null)
                        {
                            _playingRandomMode = 0;
                        }
                        else if (_localSettings.Values[SETTING_NAME_RANDOM_INDEX]?.ToString() == "0")
                        {
                            _playingRandomMode = 0;
                        }
                        else if (_localSettings.Values[SETTING_NAME_RANDOM_INDEX]?.ToString() == "1")
                        {
                            _playingRandomMode = 1;
                        }
                        else
                        {
                            _playingRandomMode = 0;
                        }
                    }
                }
                catch (Exception ex) { Trace.WriteLine(ex); }
                if (_playingRandomMode < 0) _playingRandomMode = 0;
                return (PlayingRandomMode)_playingRandomMode;
            }
            set
            {
                SetProperty(ref _playingRandomMode, (int)value);
                ApplicationData.Current.LocalSettings.Values[SETTING_NAME_RANDOM_INDEX] = _playingRandomMode;
            }
        }

        /// <summary>
        /// 循环播放模式
        /// </summary>
        public PlayingRepeatMode PlayingRepeatMode
        {
            get
            {
                try
                {
                    if (_playingRepeatMode < 0)
                    {
                        if (_localSettings.Values[SETTING_NAME_REPEAT_INDEX] == null)
                        {
                            _playingRepeatMode = 0;
                        }
                        else if (_localSettings.Values[SETTING_NAME_REPEAT_INDEX]?.ToString() == "0")
                        {
                            _playingRepeatMode = 0;
                        }
                        else if (_localSettings.Values[SETTING_NAME_REPEAT_INDEX]?.ToString() == "1")
                        {
                            _playingRepeatMode = 1;
                        }
                        else if (_localSettings.Values[SETTING_NAME_REPEAT_INDEX]?.ToString() == "2")
                        {
                            _playingRepeatMode = 2;
                        }
                        else
                        {
                            _playingRepeatMode = 0;
                        }
                    }
                }
                catch (Exception ex) { Trace.WriteLine(ex); }
                if (_playingRepeatMode < 0) _playingRepeatMode = 0;
                return (PlayingRepeatMode)_playingRepeatMode;
            }
            set
            {
                SetProperty(ref _playingRepeatMode, (int)value);
                ApplicationData.Current.LocalSettings.Values[SETTING_NAME_REPEAT_INDEX] = _playingRepeatMode;
            }
        }
    }

}
