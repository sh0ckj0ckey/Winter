using System;
using System.ComponentModel;

namespace Winter.Services.Interfaces
{
    public interface ISettingsService : INotifyPropertyChanged
    {
        event EventHandler<int>? AppearanceSettingChanged;

        event EventHandler<int>? BackdropSettingChanged;

        int AppearanceIndex { get; set; }

        int BackdropIndex { get; set; }
    }
}
