using System.IO;
using System.Text.Json;
using FileOrganizer.Configuration;
using FileOrganizer.Interfaces;

namespace FileOrganizer.Services;

public class PreferencesService : IPreferencesService
{
    public string Theme { get; set; } = "Light";
    public bool IsDiscordEnabled { get; set; }
    private readonly string _preferencesFileName = AppPaths.PreferencesFilePath;

    public void SavePreferences()
    {
        var json = JsonSerializer.Serialize(this,
            new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_preferencesFileName, json);
    }

    public void LoadPreferences()
    {
        if (File.Exists(_preferencesFileName))
        {
            try
            {
                var jsonText = File.ReadAllText(_preferencesFileName);
                var loadedPreferences = JsonSerializer.Deserialize<PreferencesService>(jsonText);

                if (loadedPreferences == null)
                    return;

                Theme = loadedPreferences.Theme;
                IsDiscordEnabled = loadedPreferences.IsDiscordEnabled;
            }
            catch
            {
                Theme = "Light";
                IsDiscordEnabled = false;
                SavePreferences();
            }
        }
        else
        {
            SavePreferences();
        }
    }
}