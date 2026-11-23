using System;
using System.IO;

namespace FileOrganizer.Configuration;

public static class AppPaths
{
    public static readonly string AppDirectory = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "FileOrganizer"
    );

    public static readonly string LogFilePath = Path.Combine(AppDirectory, "FileLog.json");

    public static readonly string RulesFilePath = Path.Combine(AppDirectory, "Rules.json");

    public static readonly string PreferencesFilePath = Path.Combine(AppDirectory, "Preferences.json");

    public static string EnsureParentDirectory(string filePath)
    {
        var directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        return filePath;
    }
    public static void EnsureDirectoryExists(string directoryPath)
    {
        if (!string.IsNullOrWhiteSpace(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
    }
}