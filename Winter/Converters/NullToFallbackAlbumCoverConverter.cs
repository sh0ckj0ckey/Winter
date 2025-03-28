﻿using System;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media.Imaging;

namespace Winter.Converters
{
    internal class NullToFallbackAlbumCoverConverter : IValueConverter
    {
        private static BitmapImage? _defaultBitmapImage = null;

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is BitmapImage bitmapImage && bitmapImage != null)
            {
                return bitmapImage;
            }

            _defaultBitmapImage ??= new BitmapImage(new Uri("ms-appx:///Assets/Icons/WinterPlaceholderGrayFilled.png"))
            {
                DecodePixelType = DecodePixelType.Logical,
                DecodePixelWidth = 72/* * 2*/,
            };

            return _defaultBitmapImage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
