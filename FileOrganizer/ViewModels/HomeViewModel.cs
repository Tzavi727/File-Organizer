using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FileOrganizer.Interfaces;
using FileOrganizer.Services;

namespace FileOrganizer.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    private readonly IFilePickerService _filePickerService;

    public HomeViewModel(IFilePickerService filePickerService)
    {
        _filePickerService = filePickerService;
        UpdateExtensionsOptions();
        //I'm sure there is a better way to do this, but I'll leave this as-is for now.
        AppConfigs.RulesChanged += UpdateExtensionsOptions;
    }

    public bool HasExtensionSelected => SelectedExtension != null;
    public bool HasPathText => !string.IsNullOrWhiteSpace(FolderPath);
    [ObservableProperty] private int _filesMoved;
    [ObservableProperty] private string? _lastActionBlock = "Last Action: Waiting for input...";
    [ObservableProperty] private string? _folderPath;
    [ObservableProperty] private string? _selectedExtension;
    [ObservableProperty] private bool _filterMode;

    [ObservableProperty] private ObservableCollection<string>? _extensionList;
    [ObservableProperty] private ObservableCollection<string>? _fileList;

    partial void OnFolderPathChanged(string? value)
    {
        if (string.IsNullOrWhiteSpace(value) || !Directory.Exists(value))
        {
            FileList = null;
            return;
        }

        try
        {
            string[] files = Directory.GetFiles(value);

            var fileNames = files.Select(f => Path.GetFileName(f)).ToList();

            FileList = new ObservableCollection<string>(fileNames);
        }
        catch (Exception)
        {
            FileList = new ObservableCollection<string> { "Acess Denied or Error..." };
        }
    }

    partial void OnFilesMovedChanged(int value)
    {
        if (value == 1)
        {
            LastActionBlock = $"Last Action: {value} File organized";
            RichPresenceService.SetActionPresence("Organizing File...", $"File organized this session: {FilesMoved}");
        }
        else if (value == 0)
        {
            LastActionBlock = "Could not find any files to organize...";
        }
        else
        {
            LastActionBlock = $"Last Action: {value} Files organized";
            RichPresenceService.SetActionPresence("Organizing Files...", $"Files organized this session: {FilesMoved}");
        }
    }

    [RelayCommand]
    private void AutoFindPath()
    {
        string downloadsPath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");

        FolderPath = downloadsPath;
    }

    [RelayCommand]
    private async Task BrowseFolderAsync()
    {
        string? path = await _filePickerService.OpenFolderAsync();

        if (!string.IsNullOrEmpty(path))
            FolderPath = path;
    }

    [RelayCommand]
    private void FullSort()
    {
        if (string.IsNullOrWhiteSpace(FolderPath) || !Directory.Exists(FolderPath))
            return;

        FilesMoved = SorterService.ExecuteOrganization(FolderPath);
        FolderPath = null;
    }

    [RelayCommand]
    private void TargetSort()
    {
        if (string.IsNullOrWhiteSpace(FolderPath) || !Directory.Exists(FolderPath) ||
            string.IsNullOrWhiteSpace(SelectedExtension))
            return;

        if (!AppConfigs.ContainsRule(SelectedExtension) ||
            !AppConfigs.TryGetExtensionFolder(SelectedExtension, out string folderName))
            return;

        FilesMoved = SorterService.ExecuteOrganizationByExtension(FolderPath, SelectedExtension, folderName);
        FolderPath = null;
    }

    [RelayCommand]
    private void ExecuteUndo()
    {
        UndoService.ExecuteUndo();
        FolderPath = null;
    }

    private void UpdateExtensionsOptions()
    {
        var rules = AppConfigs.GetAllRules();

        var extensions = rules.Keys.ToList();

        ExtensionList = new ObservableCollection<string>(extensions);
    }
}