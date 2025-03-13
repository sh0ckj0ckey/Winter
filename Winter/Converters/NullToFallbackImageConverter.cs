using System;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media.Imaging;

namespace Winter.Converters
{
    internal class NullToFallbackImageConverter : IValueConverter
    {
        private static BitmapImage? _defaultBitmapImage = null;

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is BitmapImage bitmapImage && bitmapImage != null)
            {
                return bitmapImage;
            }

            _defaultBitmapImage ??= new BitmapImage(new Uri("ms-appx:///Assets/Icons/WinterPlaceholderGray.png"))
            {
                DecodePixelType = DecodePixelType.Logical,
                DecodePixelWidth = 144,
            };

            return _defaultBitmapImage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
