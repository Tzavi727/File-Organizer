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
    private readonly IRichPresenceService _richPresenceService;
    private readonly ISorterService _sorterService;
    private readonly IUndoService _undoService;
    private readonly RuleService _ruleService;

    public HomeViewModel(IFilePickerService filePickerService, IRichPresenceService richPresenceService,
        ISorterService sorterService, IUndoService undoService, RuleService ruleService)
    {
        _filePickerService = filePickerService;
        _richPresenceService = richPresenceService;
        _sorterService = sorterService;
        _undoService = undoService;
        _ruleService = ruleService;
        UpdateExtensionsOptions();
        //I'm sure there is a better way to do this, but I'll leave this as-is for now.
        ruleService.RulesChanged += UpdateExtensionsOptions;
    }
    
    [ObservableProperty] private int _filesMoved;
    [ObservableProperty] private string _lastActionBlock = "Last Action: Waiting for input...";
    [ObservableProperty] private double _lastActionOpacity = 0.5;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(FullSortCommand))]
    [NotifyCanExecuteChangedFor(nameof(TargetSortCommand))]
    [NotifyPropertyChangedFor(nameof(FolderPathHasText))]
    private string? _folderPath;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(TargetSortCommand))]
    [NotifyPropertyChangedFor(nameof(HasSelectedExtension))]
    private string? _selectedExtension;

    [ObservableProperty] 
    [NotifyCanExecuteChangedFor(nameof(TargetSortCommand))]
    private bool _filterMode;
    
    [ObservableProperty] private ObservableCollection<string>? _extensionList;
    [ObservableProperty] private ObservableCollection<string>? _fileList;

    private bool CanTargetSort => !string.IsNullOrWhiteSpace(FolderPath) &&
                                  !string.IsNullOrWhiteSpace(SelectedExtension) &&
                                  Directory.Exists(FolderPath) &&
                                  FilterMode;
    private bool CanSort => !string.IsNullOrWhiteSpace(FolderPath) && Directory.Exists(FolderPath);
    public bool FolderPathHasText => !string.IsNullOrWhiteSpace(FolderPath);
    public bool HasSelectedExtension => !string.IsNullOrWhiteSpace(SelectedExtension);
    

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

    private void HandleSortResult(int moved)
    {
        if (moved > 0)
        {
            FilesMoved = moved;
            LastActionBlock = moved == 1
                ? "Last Action: 1 File organized"
                : $"Last Action: {moved} Files organized";
            LastActionOpacity = 1.0;

            _richPresenceService.SetActionPresence("Organizing Files...",
                $"Files organized this session: {FilesMoved}");
        }
        else
        {
            LastActionBlock = "Could not find any files to organize...";
            LastActionOpacity = 0.5;
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

    [RelayCommand(CanExecute = nameof(CanSort))]
    private void FullSort()
    {
        if (string.IsNullOrWhiteSpace(FolderPath) || !Directory.Exists(FolderPath))
            return;

        FilesMoved = _sorterService.ExecuteOrganization(FolderPath);

        HandleSortResult(FilesMoved);
        RefreshFileList(FolderPath);
    }

    [RelayCommand(CanExecute = nameof(CanTargetSort))]
    private void TargetSort()
    {
        if (string.IsNullOrWhiteSpace(FolderPath) || !Directory.Exists(FolderPath) ||
            string.IsNullOrWhiteSpace(SelectedExtension))
            return;

        if (!_ruleService.ContainsRule(SelectedExtension) ||
            !_ruleService.TryGetExtensionFolder(SelectedExtension, out string folderName))
            return;

        FilesMoved = _sorterService.ExecuteOrganizationByExtension(FolderPath, SelectedExtension, folderName);

        HandleSortResult(FilesMoved);
        RefreshFileList(FolderPath);
    }

    [RelayCommand]
    private void ExecuteUndo()
    {
        _undoService.ExecuteUndo();

        if (string.IsNullOrWhiteSpace(FolderPath) || !Directory.Exists(FolderPath))
            return;

        RefreshFileList(FolderPath);
    }

    private void UpdateExtensionsOptions()
    {
        var rules = _ruleService.GetAllRules();

        var extensions = rules.Keys.ToList();

        ExtensionList = new ObservableCollection<string>(extensions);
    }

    private void RefreshFileList(string path)
    {
        if (string.IsNullOrWhiteSpace(path) || !Directory.Exists(path))
        {
            FileList = null;
            return;
        }

        try
        {
            string[] files = Directory.GetFiles(path);

            var fileNames = files.Select(f => Path.GetFileName(f)).ToList();

            FileList = new ObservableCollection<string>(fileNames);
        }
        catch (Exception)
        {
            FileList = new ObservableCollection<string> { "Acess Denied or Error..." };
        }
    }
}