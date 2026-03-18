using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FileOrganizer.FileOrganizer.Config
{
    internal class AppConfigs
    {
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
        }

        public static void SetDefaultRules()
        {
            rules.Clear();

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
        }

        public static void SetNewRule(string extension, string folderName)
        {
            rules[extension] = folderName;
        }

        public static void RemoveRule(string extension)
        {
            rules.Remove(extension);
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

        public static void SaveRules()
        {
            var json = JsonSerializer.Serialize(GetRulesForSave(),
                new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("rules.json", json);
        }

        public static void LoadRules()
        {
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

        public static async Task ExportRules(string path)
        {
            try
            {
                var json = JsonSerializer.Serialize(GetRulesForSave(),
                new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(path, json);
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