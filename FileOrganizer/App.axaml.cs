using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using FileOrganizer.Configuration;
using FileOrganizer.Services;
using FileOrganizer.ViewModels;
using FileOrganizer.Views;

namespace FileOrganizer;

public class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            AppPaths.EnsureDirectoryExists(AppPaths.AppDirectory);
            var mainWindow = new MainWindow();
            var appConfigs = new RuleService();
            var filePicker = new AvaloniaFilePickerService(mainWindow, appConfigs);
            var dialogService = new DialogService(mainWindow, appConfigs);
            var logService = new LogService();
            var undoService = new UndoService();
            var preferencesService = new PreferencesService();
            var sorterService = new SorterService(logService, undoService, appConfigs);
            var themesService = new ThemesService(preferencesService);


            var mainViewModel = new MainViewModel
            (
                filePicker,
                sorterService,
                undoService,
                preferencesService,
                themesService,
                dialogService,
                appConfigs
            );
            mainWindow.DataContext = mainViewModel;
            desktop.MainWindow = mainWindow;

        }

        base.OnFrameworkInitializationCompleted();
    }
}