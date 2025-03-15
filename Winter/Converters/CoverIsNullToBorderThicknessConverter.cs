using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media.Imaging;

namespace Winter.Converters
{
    internal class CoverIsNullToBorderThicknessConverter : IValueConverter
    {
        private static Thickness _thickness0px = new Thickness(0);
        private static Thickness _thickness1px = new Thickness(1);

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is BitmapImage bitmapImage && bitmapImage != null)
            {
                return _thickness0px;
            }

            return _thickness1px;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
