using System;
using System.IO;

namespace FileOrganizer.Configuration;

public static class AppPaths
{
    private static readonly string AppDirectory = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "FileOrganizer"
    );

    public static readonly string LogFilePath = Path.Combine(AppDirectory, "FileLog.json");

    public static readonly string RulesFilePath = Path.Combine(AppDirectory, "Rules.json");

    public static readonly string PreferencesFilePath = Path.Combine(AppDirectory, "Preferences.json");
}