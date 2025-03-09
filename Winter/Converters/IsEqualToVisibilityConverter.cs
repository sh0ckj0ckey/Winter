using System;
using System.Diagnostics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

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
