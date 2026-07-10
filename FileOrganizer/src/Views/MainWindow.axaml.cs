using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using FileOrganizer.src.Services;
using System;

namespace FileOrganizer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            PreferencesService.LoadPreferences();
            this.DataContext = PreferencesService.Preferences;
            AppConfigs.LoadRules();
            ThemesService.SwitchTheme(PreferencesService.Preferences.Theme);
            RichPresenceService.InitializeRpc();
            RichPresenceService.SetIdlePresence();
        }

        private void Quit_Click(object? sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ManualSave_Click(object? sender, RoutedEventArgs e)
        {
            AppConfigs.SaveRules();
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

        private async void OpenHistory_Click(object? sender, RoutedEventArgs e)
        {
            string log = "FileOrganizerLog.json";
            if (!System.IO.File.Exists(log))
            {
                var dialog = new DialogBox("No History Found", "No log file detected.", DialogType.Error);
                await dialog.ShowDialog<bool>(this);
                return;
            }
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(log) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                var dialog = new DialogBox("External process error", "Could not open history file.", DialogType.Error);
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
                await AppConfigs.ExportRules(path);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            RichPresenceService.DisposeClient();
            base.OnClosed(e);
        }

        private void Themes_Click(object? sender, RoutedEventArgs e)
        {
            var ThemesWindow = new ThemesPreviews();

            ThemesWindow.Show();
        }

        private void Undo_Click(object? sender, RoutedEventArgs e)
        {
            HomeViewControl.ExecuteUndo();
        }

        private async void ResetDefaults_Click(object? sender, RoutedEventArgs e)
        {
            await SettingsViewControl.ResetToDefaults(this);
        }
    }
}