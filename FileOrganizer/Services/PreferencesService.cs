using System.IO;
using System.Text.Json;
using FileOrganizer.Configuration;
using FileOrganizer.Interfaces;

namespace FileOrganizer.Services;

public class PreferencesService : IPreferencesService
{
    public string Theme { get; set; } = "Light";
    private readonly string _preferencesFileName = AppPaths.PreferencesFilePath;

    public void SavePreferences()
    {
        var json = JsonSerializer.Serialize(this,
            new JsonSerializerOptions { WriteIndented = true });
        
        AppPaths.EnsureParentDirectory(_preferencesFileName);
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
            }
            catch
            {
                Theme = "Light";
                SavePreferences();
            }
        }
        else
        {
            SavePreferences();
        }
    }
}