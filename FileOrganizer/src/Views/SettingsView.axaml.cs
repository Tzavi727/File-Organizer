using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using FileOrganizer.src.Services;
using System.Linq;
using System.Threading.Tasks;

namespace FileOrganizer;

public partial class SettingsView : UserControl
{
    public SettingsView()
    {
        InitializeComponent();
        UpdateRulesList();
        AppConfigs.RulesChanged += UpdateRulesList;
    }
    private void UpdateRulesList()
    {
        var rules = AppConfigs.GetAllRules();

        RulesListBox.ItemsSource = rules.ToList();
    }

    private void ExtensionToAddBox_TextChanged(object? sender, TextChangedEventArgs e)
    {
        var ext = ExtensionToAddBox.Text;
        if (!string.IsNullOrEmpty(ext))
        {
            if (!ExtensionToAddBox.Classes.Contains("HasText"))
                ExtensionToAddBox.Classes.Add("HasText");
        }
        else
        {
            ExtensionToAddBox.Classes.Remove("HasText");
        }
    }

    private void FolderBox_TextChanged(object? sender, TextChangedEventArgs e)
    {
        var folder = FolderBox.Text;
        if (!string.IsNullOrEmpty(folder))
        {
            if (!FolderBox.Classes.Contains("HasText"))
                FolderBox.Classes.Add("HasText");
        }
        else
        {
            FolderBox.Classes.Remove("HasText");
        }
    }

    private void RemoveExtensionBox_TextChanged(object? sender, TextChangedEventArgs e)
    {
        var folder = ExtensionToDeleteBox.Text;
        if (!string.IsNullOrEmpty(folder))
        {
            if (!ExtensionToDeleteBox.Classes.Contains("HasText"))
                ExtensionToDeleteBox.Classes.Add("HasText");
        }
        else
        {
            ExtensionToDeleteBox.Classes.Remove("HasText");
        }
    }

    private void RichPresenceCheckBoxChanged(object? sender, RoutedEventArgs e)
    {
        bool isOn = RichPresenceCheckBox.IsChecked ?? false;
        PreferencesService.Preferences.IsDiscordEnabled = isOn;
        PreferencesService.SavePreferences();
        if (isOn)
        {
            RichPresenceService.InitializeRpc();
            RichPresenceService.SetIdlePresence();
        }
        else
        {
            RichPresenceService.DisposeClient();
        }
    }

    private void RemoveExtension_Click(object? sender, RoutedEventArgs e)
    {
        string extension = ExtensionToDeleteBox.Text;

        if (string.IsNullOrWhiteSpace(extension))
        {
            return;
        }
        AppConfigs.RemoveRule(extension);
        AppConfigs.SaveRules();
        ExtensionToDeleteBox.Text = "";
    }

    private void AddExtension_Click(object? sender, RoutedEventArgs e)
    {
        string extension = ExtensionToAddBox.Text?.Trim().ToLower();
        if (extension.StartsWith("."))
        {
            extension = extension.TrimStart('.');
        }
        string folderName = FolderBox.Text?.Trim().ToLower();
        if (string.IsNullOrWhiteSpace(extension) || string.IsNullOrWhiteSpace(folderName))
        {
            return;
        }
        AppConfigs.SetNewRule(extension, folderName);
        AppConfigs.SaveRules();
        ExtensionToAddBox.Text = "";
        FolderBox.Text = "";
    }

    public async Task ResetToDefaults()
    {
        var topLevel = TopLevel.GetTopLevel(this) as Window;
        var dialog = new DialogBox("Reset to defaults", "Reset settings to default?", DialogType.Confirm);
        if (topLevel == null) return;
        bool result = await dialog.ShowDialog<bool>(topLevel);
        if (result)
        {
            AppConfigs.RestoreDefault();
            UpdateRulesList();
        }
    }
}