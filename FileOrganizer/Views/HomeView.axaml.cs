using Avalonia.Controls;
using Avalonia.Interactivity;
using FileOrganizer.src.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileOrganizer;

public partial class HomeView : UserControl
{
    public HomeView()
    {
        InitializeComponent();
        UpdateExtOptions();
        AppConfigs.RulesChanged += UpdateExtOptions;
    }

    private void PathBox_TextChanged(object? sender, TextChangedEventArgs e)
    {
        string path = PathBox.Text;

        if (!string.IsNullOrEmpty(path))
        {
            if (!PathBox.Classes.Contains("HasText"))
                PathBox.Classes.Add("HasText");
        }
        else
        {
            PathBox.Classes.Remove("HasText");
        }

        if (!string.IsNullOrWhiteSpace(path) && Directory.Exists(path))
        {
            try
            {
                string[] files = Directory.GetFiles(path);

                var fileNames = files.Select(f => Path.GetFileName(f)).ToList();

                FilesList.ItemsSource = fileNames;
            }
            catch (Exception ex)
            {
                FilesList.ItemsSource = new List<string> { "Acess Denied or Error..." };
            }
        }
        else
        {
            FilesList.ItemsSource = null;
        }
    }

    private void AutoDetect_Click(object? sender, RoutedEventArgs e)
    {
        string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");

        PathBox.Text = downloadsPath;
    }

    private async void BrowseFolder_Click(object? sender, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel == null) return;
        var folders = await topLevel.StorageProvider.OpenFolderPickerAsync(new Avalonia.Platform.Storage.FolderPickerOpenOptions
        {
            Title = "Select the folder to organize",
            AllowMultiple = false
        });

        if (folders.Any())
        {
            string folderPath = folders[0].Path.LocalPath;
            PathBox.Text = folderPath;
        }
    }

    // Im not an avalonia expert, but I have a feeling with MVVM all of this can be outsourced to XAML, something like
    // <Style Selector="ComboBox.HasSelection">
    // ...
    // </Style>
    
    private void ExtensionComboBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (ExtensionComboBox.SelectedItem != null)
        {
            if (!ExtensionComboBox.Classes.Contains("HasSelection"))
                ExtensionComboBox.Classes.Add("HasSelection");
        }
        else
        {
            ExtensionComboBox.Classes.Remove("HasSelection");
        }
    }

    // [Red] :( Homework question: List the issues in this method!
    private void FullSort_Click(object? sender, RoutedEventArgs e)
    {
        string path = PathBox.Text;

        if (!string.IsNullOrWhiteSpace(path) && Directory.Exists(path))
        {
            int filesMoved = SorterService.ExecuteOrganization(path);
            PathBox_TextChanged(null, null);
            if (filesMoved == 1)
            {
                LastActionBlock.Text = $"Last Action: {filesMoved} File organized";
                LastActionBlock.Opacity = 1.0;
                RichPresenceService.SetActionPresence("Organizing File...", $"File organized this session: {filesMoved}");
            }
            else if (filesMoved == 0)
            {
                LastActionBlock.Text = "Could not find any files to organize...";
                LastActionBlock.Opacity = 0.4;
            }
            else
            {
                LastActionBlock.Text = $"Last Action: {filesMoved} Files organized";
                LastActionBlock.Opacity = 1.0;
                RichPresenceService.SetActionPresence("Organizing Files...", $"Files organized this session: {filesMoved}");
            }
        }
    }

    private void TargetSort_Click(object? sender, RoutedEventArgs e)
    {
        string path = PathBox.Text;
        string extension = ExtensionComboBox.SelectedItem?.ToString() ?? "";


        if (!string.IsNullOrWhiteSpace(path) && Directory.Exists(path))
        {
            if (AppConfigs.ContainsRule(extension))
            {
                if (!AppConfigs.TryGetExtensionFolder(extension, out string folderName))
                {
                    return;
                }
                int filesMoved = SorterService.ExecuteOrganizationByExtension(path, extension, folderName);
                PathBox_TextChanged(null, null);
                if (filesMoved == 1)
                {
                    LastActionBlock.Text = $"Last Action: {filesMoved} File organized";
                    LastActionBlock.Opacity = 1.0;
                    RichPresenceService.SetActionPresence("Organizing File...", $"File organized this session: {filesMoved}");
                }
                else if (filesMoved == 0)
                {
                    LastActionBlock.Text = "Could not find any files to organize...";
                    LastActionBlock.Opacity = 0.4;
                }
                else
                {
                    LastActionBlock.Text = $"Last Action: {filesMoved} Files organized";
                    LastActionBlock.Opacity = 1.0;
                    RichPresenceService.SetActionPresence("Organizing Files...", $"Files organized this session: {filesMoved}");
                }
            }
        }
    }
    public void ExecuteUndo()
    {
        UndoService.ExecuteUndo();
        PathBox_TextChanged(null, null);

        LastActionBlock.Text = "Last Action: Undo executed successfully.";
        LastActionBlock.Opacity = 1.0;
    }

    private void UpdateExtOptions()
    {
        var rules = AppConfigs.GetAllRules();

        var extensions = rules.Keys.ToList();

        ExtensionComboBox.ItemsSource = extensions;
    }
}
