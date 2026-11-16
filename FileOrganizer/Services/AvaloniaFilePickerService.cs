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
    public AvaloniaFilePickerService(Visual parentVisual) => _parentVisual = parentVisual;

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
}