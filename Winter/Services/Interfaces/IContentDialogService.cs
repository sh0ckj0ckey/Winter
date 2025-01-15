using System.Threading.Tasks;

namespace Winter.Services.Interfaces
{
    public interface IContentDialogService
    {
        Task ShowDialogAsync(string title, string content, string closeButtonText);
    }
}
