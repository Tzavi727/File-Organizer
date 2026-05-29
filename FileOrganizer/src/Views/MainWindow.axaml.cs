using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using FileOrganizer.FileOrganizer.Config;
using FileOrganizer.FileOrganizer.Services;
using FileOrganizer.src.Logic;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Tmds.DBus.Protocol;

namespace FileOrganizer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AppConfigs.LoadRules();
            UpdateExtOptions();
            UpdateRulesList();
        }

        private void AutoDetect_Click(object? sender, RoutedEventArgs e)
        {
            string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");

            PathBox.Text = downloadsPath;
        }

        private async void BrowseFolder_Click(object? sender, RoutedEventArgs e)
        {
            var folders = await this.StorageProvider.OpenFolderPickerAsync(new Avalonia.Platform.Storage.FolderPickerOpenOptions
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

        private void PathBox_TextChanged(object? sender, TextChangedEventArgs e)
        {
            string path = PathBox.Text;

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
                    }
                }
            }
        }

        private void UpdateExtOptions()
        {
            var rules = AppConfigs.GetAllRules();

            var extensions = rules.Keys.ToList();

            ExtensionComboBox.ItemsSource = extensions;
        }

        private void UpdateRulesList()
        {
            var rules = AppConfigs.GetAllRules();

            RulesListBox.ItemsSource = rules.ToList();
        }

        private void RemoveExtension_Click(object? sender, RoutedEventArgs e)
        {
            string extension = ExtensionToDelete.Text;

            if (string.IsNullOrWhiteSpace(extension))
            {
                return;
            }
            AppConfigs.RemoveRule(extension);
            AppConfigs.SaveRules();
            UpdateRulesList();
            UpdateExtOptions();
            ExtensionToDelete.Text = "";
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
            UpdateRulesList();
            UpdateExtOptions();
            ExtensionToAddBox.Text = "";
            FolderBox.Text = "";
        }

        private void Quit_Click(object? sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ManualSave_Click(object? sender, RoutedEventArgs e)
        {
            AppConfigs.SaveRules();
        }

        private async void ResetDefaults_Click(object? sender, RoutedEventArgs e)
        {
            var dialog = new DialogBox("Reset to defaults", "Reset settings to default?", DialogType.Confirm);
            bool result = await dialog.ShowDialog<bool>(this);
            if (result)
            {
                AppConfigs.RestoreDefault();
                UpdateRulesList();
                UpdateExtOptions();
            }
        }

        private async void OpenGithubPage_Click(object? sender, RoutedEventArgs e)
        {
            string url = "https://github.com/Tzavi727/File-Organizer";
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(url) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                var dialog = new DialogBox("External process error", "Could not open your default browser.", DialogType.Error);
                await dialog.ShowDialog<bool>(this);
                return;
            }
        }

        private void About_Click(object? sender, RoutedEventArgs e)
        {
            var aboutWindow = new AboutWindow();

            aboutWindow.Show();
        }

        private async void LoadSettings_Click(object? sender, RoutedEventArgs e)
        {
            var files = await this.StorageProvider.OpenFilePickerAsync(new Avalonia.Platform.Storage.FilePickerOpenOptions
            {
                Title = "Import your rules",
                FileTypeFilter = new[] { new FilePickerFileType("JSON files") { Patterns = new[] { "*.json" } } },
                AllowMultiple = true
            });

            if (files.Count > 0)
            {
                string path = files[0].Path.LocalPath;
                AppConfigs.ImportRules(path);
                UpdateRulesList();
                UpdateExtOptions();
            }
        }

        private async void SaveSettingsAs_Click(object? sender, RoutedEventArgs e)
        {
            var file = await this.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
            {
                Title = "Export your rules",
                SuggestedFileName = "My_custom_rules.json",
                FileTypeChoices = new[] { new FilePickerFileType("JSON files") { Patterns = new[] { "*.json" } } }
            });

            if (file != null)
            {
                string path = file.Path.LocalPath;
                AppConfigs.ExportRules(path);
            }
        }

        private void Undo_Click(object? sender, RoutedEventArgs e)
        {
            UndoService.ExecuteUndo();
            PathBox_TextChanged(null, null);

            LastActionBlock.Text = "Last Action: Undo executed successfully.";
            LastActionBlock.Opacity = 1.0;
        }
    }
}