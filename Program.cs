using System;
using Avalonia;
using EasyBankingZinsüberschuss.Datenpräsentation;

namespace EasyBankingZinsüberschuss
{
    class Program
    {
        static void Main(string[] args)
        {
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }

        private static AppBuilder BuildAvaloniaApp()
        {
            return AppBuilder.Configure<App>()
                .UsePlatformDetect();
        }
    }
}
