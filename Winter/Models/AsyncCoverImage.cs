using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Storage;
using Windows.Storage.FileProperties;

namespace Winter.Models
{
    public class AsyncCoverImage : ObservableObject
    {
        private BitmapImage? _image = null;

        public BitmapImage? Image
        {
            get => _image;
            private set => SetProperty(ref _image, value);
        }

        public async Task LoadCoverImageFromFile(StorageFile file, uint size)
        {
            try
            {
                var thumbnail = await file.GetThumbnailAsync(ThumbnailMode.MusicView, size, ThumbnailOptions.ResizeThumbnail);
                if (thumbnail is not null && thumbnail.Type != ThumbnailType.Icon)
                {
                    var bitmapImage = new BitmapImage();
                    bitmapImage.DecodePixelType = DecodePixelType.Logical;
                    bitmapImage.DecodePixelWidth = (int)size;
                    await bitmapImage.SetSourceAsync(thumbnail);
                    this.Image = bitmapImage;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex);
            }
        }
    }
}
