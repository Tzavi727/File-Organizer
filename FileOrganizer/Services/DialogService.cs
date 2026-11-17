using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Avalonia.Controls;
using FileOrganizer.Interfaces;
using FileOrganizer.Views;

namespace FileOrganizer.Services;

public class DialogService : IDialogService
{
    private readonly Window _window;
    private readonly RuleService _ruleService;

    public DialogService(Window window, RuleService ruleService)
    {
        _window = window;
        _ruleService = ruleService;
    }

    public async Task OpenGithubUrl()
    {
        string url = "https://github.com/Tzavi727/File-Organizer";
        try
        {
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }
        catch (Exception)
        {
            var dialog = new DialogBox("External process error", "Could not open your default browser.",
                DialogType.Error);
            await dialog.ShowDialog<bool>(_window);
        }
    }

    public async Task ResetDefaults()
    {
        var dialog = new DialogBox("Reset to defaults", "Reset settings to default?", DialogType.Confirm);
        bool result = await dialog.ShowDialog<bool>(_window);
        if (result)
        {
            _ruleService.RestoreDefault();
            //UpdateRulesList();
        }
    }

    public async Task<bool> ShowMessage(string message, string title, DialogType messageType)
    {
        var dialog = new DialogBox(title, message, messageType);
        return await dialog.ShowDialog<bool>(_window);
    }
}