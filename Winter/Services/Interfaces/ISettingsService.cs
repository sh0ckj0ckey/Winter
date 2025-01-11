using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winter.Services.Interfaces
{
    interface ISettingsService : INotifyPropertyChanged
    {
        event EventHandler<int>? AppearanceSettingChanged;

        event EventHandler<int>? BackdropSettingChanged;

        int AppearanceIndex { get; set; }

        int BackdropIndex { get; set; }
    }
}
