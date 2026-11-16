using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using FileOrganizer.Interfaces;
using FileOrganizer.Services;
using FileOrganizer.ViewModels;
using FileOrganizer.Views;

namespace FileOrganizer
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var mainWindow = new MainWindow();
                var filePicker = new AvaloniaFilePickerService(mainWindow);
                var mainViewModel = new MainViewModel(filePicker);
                
                mainWindow.DataContext = mainViewModel;
                desktop.MainWindow = mainWindow;
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}