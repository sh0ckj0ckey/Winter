using System;
using System.ComponentModel;

namespace Winter.Services.Interfaces
{
    public enum PlayingRandomMode
    {
        NoRandom = 0,
        Random = 1,
    }

    public enum PlayingRepeatMode
    {
        NoRepeat = 0,
        RepeatAll = 1,
        RepeatOne = 2,
    }

    public interface ISettingsService : INotifyPropertyChanged
    {
        event EventHandler<int>? AppearanceSettingChanged;

        event EventHandler<int>? BackdropSettingChanged;

        int AppearanceIndex { get; set; }

        int BackdropIndex { get; set; }

        PlayingRandomMode PlayingRandomMode { get; set; }

        PlayingRepeatMode PlayingRepeatMode { get; set; }
    }
}
