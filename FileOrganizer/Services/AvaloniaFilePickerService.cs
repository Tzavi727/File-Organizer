using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using FileOrganizer.Interfaces;

namespace FileOrganizer.Services;

public class AvaloniaFilePickerService : IFilePickerService
{
    private readonly Visual _parentVisual;
    private readonly RuleService _ruleService;

    public AvaloniaFilePickerService(Visual parentVisual, RuleService ruleService)
    {
        _parentVisual = parentVisual;
        _ruleService = ruleService;
    }

    public async Task<string?> OpenFolderAsync()
    {
        var topLevel = TopLevel.GetTopLevel(_parentVisual);
        if (topLevel == null) return null;
        var folders = await topLevel.StorageProvider.OpenFolderPickerAsync(
            new FolderPickerOpenOptions
            {
                Title = "Select the folder to organize",
                AllowMultiple = false
            });

        return folders.Any() ? folders[0].Path.LocalPath : null;
    }

    public async Task LoadSettingsAsync()
    {
        var topLevel = TopLevel.GetTopLevel(_parentVisual);
        if (topLevel == null) return;
        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Import your rules",
            FileTypeFilter = new[] { new FilePickerFileType("JSON files") { Patterns = new[] { "*.json" } } },
            AllowMultiple = true
        });

        if (files.Count > 0)
        {
            string path = files[0].Path.LocalPath;
            _ruleService.ImportRules(path);
        }
    }

    public async Task SaveSettingsAsync()
    {
        var topLevel = TopLevel.GetTopLevel(_parentVisual);
        if (topLevel == null) return;
        var file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Export your rules",
            SuggestedFileName = "My_custom_rules.json",
            FileTypeChoices = new[] { new FilePickerFileType("JSON files") { Patterns = new[] { "*.json" } } }
        });

        if (file != null)
        {
            string path = file.Path.LocalPath;
            await _ruleService.ExportRules(path);
        }
    }
}