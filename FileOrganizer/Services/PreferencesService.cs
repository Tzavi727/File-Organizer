using System.IO;
using System.Text.Json;

namespace FileOrganizer.Services
{
    // [Red] Same problems as the LogService. Not repeating.
    
    internal class PreferencesService
    {
        public string Theme { get; set; } = "Light";

        public bool IsDiscordEnabled { get; set; } = false;

        // [Red] Here comes the Singleton :(
        public static PreferencesService Preferences = new PreferencesService();

        public static PreferencesService Get() => Preferences;

        public static void SavePreferences()
        {
            var json = JsonSerializer.Serialize(Preferences,
                new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("preferences.json", json);
        }

        public static void LoadPreferences()
        {
            if (File.Exists("preferences.json"))
            {
                try
                {
                    string jsonText = File.ReadAllText("preferences.json");
                    var loadedPreferences = JsonSerializer.Deserialize<PreferencesService>(jsonText);

                    if (loadedPreferences != null)
                    {
                        Preferences = loadedPreferences;
                    }
                }
                catch
                {
                    Preferences = new PreferencesService();
                    SavePreferences();
                }
            }
            else
            {
                SavePreferences();
            }
        }
    }
}
