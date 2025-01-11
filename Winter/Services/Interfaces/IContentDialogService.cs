using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winter.Services.Interfaces
{
    interface IContentDialogService
    {
        Task ShowDialogAsync(string title, string content, string closeButtonText);
    }
}
