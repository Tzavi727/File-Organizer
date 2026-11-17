using System.Threading.Tasks;
using FileOrganizer.Views;

namespace FileOrganizer.Interfaces;

public interface IDialogService
{
    Task OpenGithubUrl();
    Task ResetDefaults();
    Task<bool> ShowMessage(string message, string title, DialogType messageType);
}