using System;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Winter.Services.Interfaces;

namespace Winter.Services
{
    public class ContentDialogService : IContentDialogService
    {
        public Func<XamlRoot?>? XamlRootGetter { get; set; }

        public Func<ElementTheme>? ElementThemeGetter { get; set; }

        public async Task ShowDialogAsync(string title, string content, string closeButtonText)
        {
            if (this.XamlRootGetter is null)
            {
                throw new Exception("Must set XamlRootGetter before calling ShowDialog");
            }

            if (this.ElementThemeGetter is null)
            {
                throw new Exception("Must set ElementThemeGetter before calling ShowDialog");
            }

            var xamlRoot = this.XamlRootGetter.Invoke();
            var elementTheme = this.ElementThemeGetter.Invoke();

            if (xamlRoot is not null)
            {
                var contentDialog = new ContentDialog
                {
                    XamlRoot = xamlRoot,
                    Title = title,
                    Content = content,
                    CloseButtonText = closeButtonText,
                    RequestedTheme = elementTheme,
                };

                await contentDialog.ShowAsync();
            }
        }
    }
}
