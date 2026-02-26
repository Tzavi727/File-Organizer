using FileOrganizer.FileOrganizer.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FileOrganizer.FileOrganizer.Settings
{
    internal class AppSettings
    {
        public static void SaveRules()
        {
            var json = JsonSerializer.Serialize(RuleManager.GetRulesForSave(),
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
                    var loadedRules = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonText);
                    RuleManager.ReplaceRules(loadedRules);
                }
                catch (Exception)
                {
                    RuleManager.SetDefaultRules();
                    SaveRules();
                }
            }
            else
            {
                RuleManager.SetDefaultRules();
                SaveRules();
            }
        }

        public static void RestoreDefault()
        {
            RuleManager.ClearRules();
            RuleManager.SetDefaultRules();
            SaveRules();
        }
    }
}
