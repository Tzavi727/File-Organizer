namespace FileOrganizer.Interfaces;

public interface IPreferencesService
{
    string Theme { get; set; }
    bool IsDiscordEnabled { get; set; }
    
    void SavePreferences();
    void LoadPreferences();
}