using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;

namespace Winter.Services.Interfaces
{
    public interface IContentDialogService
    {
        Task<ContentDialogResult> ShowDialogAsync(string title, string content, string closeButtonText);

        Task<ContentDialogResult> ShowDialogAsync(string title, string content, string confirmButtonText, string closeButtonText);
    }
}
