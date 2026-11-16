using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using FileOrganizer.Interfaces;
using FileOrganizer.Views;

namespace FileOrganizer.Services;

public class DialogService : IDialogService
{
    private readonly Window _window;
    public DialogService(Window window) => _window = window;

    public async Task OpenGithubUrl()
    {
        string url = "https://github.com/Tzavi727/File-Organizer";
        try
        {
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }
        catch (Exception ex)
        {
            var dialog = new DialogBox("External process error", "Could not open your default browser.",
                DialogType.Error);
            await dialog.ShowDialog<bool>(_window);
        }
    }
}