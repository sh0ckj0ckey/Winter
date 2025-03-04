using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml;
using System.Diagnostics;

namespace Winter.Converters
{
    internal class IsEqualToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                return value?.ToString()?.ToLower() == parameter?.ToString()?.ToLower() ? Visibility.Visible : Visibility.Collapsed;
            }
            catch (Exception ex) { Trace.WriteLine(ex); }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null!;
        }
    }
}
