using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.UI.Dispatching;
using Microsoft.Windows.AppLifecycle;

namespace Winter
{
    public static class Program
    {
        private static App? _app;

        [STAThread]
        public static void Main(string[] args)
        {
            WinRT.ComWrappersSupport.InitializeComWrappers();

            var isRedirect = DecideRedirection().GetAwaiter().GetResult();

            if (!isRedirect)
            {
                Microsoft.UI.Xaml.Application.Start((p) =>
                {
                    var dispatcherQueue = DispatcherQueue.GetForCurrentThread();
                    var context = new DispatcherQueueSynchronizationContext(dispatcherQueue);
                    SynchronizationContext.SetSynchronizationContext(context);
                    _app = new App();
                });
            }
        }

        private static async Task<bool> DecideRedirection()
        {
            AppInstance keyInstance = AppInstance.FindOrRegisterForKey("WINTER-90243CFA-FDE1-4513-9D22-ED828D9082D9");
            AppActivationArguments args = AppInstance.GetCurrent().GetActivatedEventArgs();

            bool isRedirect = false;
            if (keyInstance.IsCurrent)
            {
                keyInstance.Activated += (_, _) => { _app?.ShowMainWindow(); };
            }
            else
            {
                isRedirect = true;
                await keyInstance.RedirectActivationToAsync(args);
            }

            return isRedirect;
        }
    }

}
