namespace FileOrganizer.Interfaces;

public interface IPreferencesService
{
    string Theme { get; set; }

    void SavePreferences();
    void LoadPreferences();
}