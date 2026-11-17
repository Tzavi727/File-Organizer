using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.Input;
using FileOrganizer.Configuration;
using FileOrganizer.Interfaces;
using FileOrganizer.Services;
using FileOrganizer.Views;

namespace FileOrganizer.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public HomeViewModel Home { get; }
    public SettingsViewModel Settings { get; }
    private readonly IFilePickerService _filePickerService;
    private readonly IRichPresenceService _richPresenceService;
    private readonly ISorterService _sorterService;
    private readonly IUndoService _undoService;
    private readonly IPreferencesService _preferencesService;
    private readonly IThemesService _themesService;
    private readonly IDialogService _dialogService;
    private readonly RuleService _ruleService;

    public MainViewModel(IFilePickerService filePickerService,
        IRichPresenceService richPresenceService,
        ISorterService sorterService,
        IUndoService undoService,
        IPreferencesService preferencesService,
        IThemesService themesService,
        IDialogService dialogService,
        RuleService ruleService)
    {
        _filePickerService = filePickerService;
        _richPresenceService = richPresenceService;
        _sorterService = sorterService;
        _undoService = undoService;
        _preferencesService = preferencesService;
        _themesService = themesService;
        _dialogService = dialogService;
        _ruleService = ruleService;

        preferencesService.LoadPreferences();
        ruleService.LoadRules();
        themesService.SwitchTheme(preferencesService.Theme);
        richPresenceService.InitializeRpc();
        richPresenceService.SetIdlePresence();
        
        Settings = new SettingsViewModel(ruleService, richPresenceService, preferencesService);
        Home = new HomeViewModel(filePickerService, richPresenceService, sorterService, undoService, ruleService);
    }

    [RelayCommand]
    private void Quit()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            desktop.Shutdown();
    }

    [RelayCommand]
    private void ManualSave()
    {
        _ruleService.SaveRules();
    }

    [RelayCommand]
    private async Task OpenGithubUrl()
    {
        await _dialogService.OpenGithubUrl();
    }

    [RelayCommand]
    private async Task OpenHistory()
    {
        var logFilePath = AppPaths.LogFilePath;

        if (!File.Exists(logFilePath))
        {
            await _dialogService.ShowMessage("No History Found", "No log file detected.", DialogType.Error);
            return;
        }

        try
        {
            Process.Start(new ProcessStartInfo(logFilePath) { UseShellExecute = true });
        }
        catch
        {
            await _dialogService.ShowMessage("External process error", "Could not open history file.",
                DialogType.Error);
        }
    }

    [RelayCommand]
    private void OpenAbout()
    {
        var aboutWindow = new AboutWindow();

        aboutWindow.Show();
    }

    [RelayCommand]
    private async Task LoadSettings()
    {
        await _filePickerService.LoadSettingsAsync();
    }

    [RelayCommand]
    private async Task SaveSettingsAs()
    {
        await _filePickerService.SaveSettingsAsync();
    }

    [RelayCommand]
    private void OpenThemes()
    {
        var themesWindow = new ThemesPreviews(new ThemesPreviewViewModel(_themesService));

        themesWindow.Show();
    }

    [RelayCommand]
    private void Undo()
    {
        Home.ExecuteUndoCommand.Execute(null);
    }

    [RelayCommand]
    private async Task ResetDefaults()
    {
        bool confirmed = await _dialogService.ShowMessage(
            "Reset to defaults", "Reset settings to default?", DialogType.Confirm
        );
        if (confirmed)
        {
            _ruleService.RestoreDefault();
        }
    }
}