using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using FileOrganizer.Configuration;

namespace FileOrganizer.Services;

public class RuleService
{
    private readonly string _rulesFileName = AppPaths.RulesFilePath;
    public event Action? RulesChanged;

    public IReadOnlyDictionary<string, string> GetAllRules()
    {
        return new Dictionary<string, string>(_rules);
    }

    private Dictionary<string, string> _rules = new();

    public Dictionary<string, string> GetRulesForSave()
    {
        return _rules;
    }

    public void ReplaceRules(Dictionary<string, string> newRules)
    {
        _rules = newRules;
        RulesChanged?.Invoke();
    }

    public void SetDefaultRules()
    {
        _rules.Clear();

        // compressed
        _rules.Add("7z", "compressed");
        _rules.Add("rar", "compressed");
        _rules.Add("zip", "compressed");
        // executables
        _rules.Add("exe", "executables");
        // images
        _rules.Add("jpg", "images");
        _rules.Add("jpeg", "images");
        _rules.Add("png", "images");
        _rules.Add("gif", "images");
        // videos
        _rules.Add("mp4", "videos");
        // documents
        _rules.Add("pdf", "documents");
        _rules.Add("docx", "documents");
        _rules.Add("txt", "documents");

        RulesChanged?.Invoke();
    }

    public void SetNewRule(string extension, string folderName)
    {
        _rules[extension] = folderName;
        RulesChanged?.Invoke();
    }

    public void RemoveRule(string extension)
    {
        _rules.Remove(extension);
        RulesChanged?.Invoke();
    }

    public bool ContainsRule(string rule)
    {
        return _rules.ContainsKey(rule);
    }

    public bool TryGetExtensionFolder(string extension, out string folderName)
    {
        return _rules.TryGetValue(extension, out folderName!);
    }

    private void ClearRules()
    {
        _rules.Clear();
    }

    public void SaveRules()
    {
        var json = JsonSerializer.Serialize(GetRulesForSave(),
            new JsonSerializerOptions { WriteIndented = true });

        AppPaths.EnsureParentDirectory(_rulesFileName);
        File.WriteAllText(_rulesFileName, json);
    }

    public void LoadRules()
    {
        if (File.Exists(_rulesFileName))
        {
            try
            {
                var jsonText = File.ReadAllText(_rulesFileName);
                var loadedRules = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonText) ??
                                  new Dictionary<string, string>();
                ReplaceRules(loadedRules);
            }
            catch (Exception)
            {
                SetDefaultRules();
                SaveRules();
            }
        }
        else
        {
            SetDefaultRules();
            SaveRules();
        }
    }

    public void ImportRules(string path)
    {
        if (!File.Exists(path))
            return;

        try
        {
            var jsonText = File.ReadAllText(path);
            var loadedRules = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonText) ??
                              new Dictionary<string, string>();
            ReplaceRules(loadedRules);
            SaveRules();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"FileOrganizer couldn't import rules from {path}: {ex.Message}");
        }
    }

    public async Task ExportRules(string path)
    {
        try
        {
            var json = JsonSerializer.Serialize(GetRulesForSave(),
                new JsonSerializerOptions { WriteIndented = true });

            await Task.Run(() => AppPaths.EnsureParentDirectory(path));
            await File.WriteAllTextAsync(path, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"FileOrganizer couldn't export rules from {path}: {ex.Message}");
        }
    }

    public void RestoreDefault()
    {
        ClearRules();
        SetDefaultRules();
        SaveRules();
    }
}