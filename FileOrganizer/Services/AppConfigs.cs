using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace FileOrganizer.Services
{
    // [Yellow] Bad name for what looks like a RuleCatalog, RuleRepository or something like that.
    // [Red] All issues from other classes equally applicable to this class.
    
    internal class AppConfigs
    {
        public static event Action? RulesChanged;
        public static IReadOnlyDictionary<string, string> GetAllRules()
        {
            return new Dictionary<string, string>(rules);
        }
        private static Dictionary<string, string> rules = new();

        public static Dictionary<string, string> GetRulesForSave()
        {
            return rules;
        }

        public static void ReplaceRules(Dictionary<string, string> newRules)
        {
            rules = newRules ?? new Dictionary<string, string>();
            RulesChanged?.Invoke();
        }

        public static void SetDefaultRules()
        {
            rules.Clear();

            // [Yellow] Magic strings
            // [White] Consider loading from embedded resource
            
            // compressed
            rules.Add("7z", "compressed");
            rules.Add("rar", "compressed");
            rules.Add("zip", "compressed");
            // executables
            rules.Add("exe", "executables");
            // images
            rules.Add("jpg", "images");
            rules.Add("jpeg", "images");
            rules.Add("png", "images");
            rules.Add("gif", "images");
            // videos
            rules.Add("mp4", "videos");
            // documents
            rules.Add("pdf", "documents");
            rules.Add("docx", "documents");
            rules.Add("txt", "documents");

            RulesChanged?.Invoke();
        }

        public static void SetNewRule(string extension, string folderName)
        {
            rules[extension] = folderName;
            RulesChanged?.Invoke();
        }

        public static void RemoveRule(string extension)
        {
            rules.Remove(extension);
            RulesChanged?.Invoke();
        }

        public static bool ContainsRule(string rule)
        {
            return rules.ContainsKey(rule);
        }

        public static bool TryGetExtensionFolder(string extension, out string folderName)
        {
            return rules.TryGetValue(extension, out folderName);
        }

        public static void ClearRules()
        {
            rules.Clear();
        }

        // [Red] Responsibility overload, PErsistence belongs in a separate abstraction. See SRP principle
        
        public static void SaveRules()
        {
            var json = JsonSerializer.Serialize(GetRulesForSave(),
                new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("rules.json", json);
        }

        // [Red] SRP violation, DI violation (already explained in LogService)
        public static void LoadRules()
        {
            // [White] Logic can be simplified. DRY violations.
            
            if (File.Exists("rules.json"))
            {
                try
                {
                    string jsonText = File.ReadAllText("rules.json");
                    var loadedRules = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonText) ?? new Dictionary<string, string>();
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

        // [Red] SRP violation, DI violation
        public static void ImportRules(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    string jsonText = File.ReadAllText(path);
                    var loadedRules = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonText) ?? new Dictionary<string, string>();
                    ReplaceRules(loadedRules);
                    SaveRules();
                }
                catch (Exception)
                {
                    return;
                }
            }
        }

        // [Red] SRP violation, DI violation
        public static async Task ExportRules(string path)
        {
            try
            {
                var json = JsonSerializer.Serialize(GetRulesForSave(),
                new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(path, json);
            }
            catch
            {
                return;
            }
        }

        public static void RestoreDefault()
        {
            ClearRules();
            SetDefaultRules();
            SaveRules();
        }
    }
}
