using Avalonia.Controls;
using Avalonia.Interactivity;
using FileOrganizer.FileOrganizer.Config;
using FileOrganizer.FileOrganizer.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
                SorterService.ExecuteOrganization(path);
                PathBox_TextChanged(null, null);
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
                    SorterService.ExecuteOrganizationByExtension(path, extension, folderName);
                    PathBox_TextChanged(null, null);
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
            string extension = ExtensionToAddBox.Text;
            if (extension.StartsWith("."))
            {
                extension = extension.TrimStart('.');
            }
            string folderName = FolderBox.Text;
            if(string.IsNullOrWhiteSpace(extension) || string.IsNullOrWhiteSpace(folderName))
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
    }
}