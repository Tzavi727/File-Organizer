using System.Threading.Tasks;

namespace FileOrganizer.Interfaces;

public interface IFilePickerService
{
    Task<string?> OpenFolderAsync();
    Task LoadSettingsAsync();
    Task SaveSettingsAsync();
}