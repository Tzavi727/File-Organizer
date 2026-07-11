using System;
using Avalonia;

namespace FileOrganizer
{
    // [Red] No Dependency Injection. Avalonia fully support DI and integrated with standard MS dependency injection framework. I pretty much already know what to expect in the rest of codebase, Singletons, Globals!
    // https://docs.avaloniaui.net/docs/app-development/dependency-injection
    
    internal class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args) => BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace();
    }
}
