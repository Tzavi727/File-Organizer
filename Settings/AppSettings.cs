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
            var json = JsonSerializer.Serialize(RuleManager.rules,
                new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("rules.json", json);
        }
        public static void LoadRules()
        {
            if (File.Exists("rules.json"))
            {
                string jsonText = File.ReadAllText("rules.json");
                RuleManager.rules = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonText);
            }
            else
            {
                RuleManager.setRules();
                SaveRules();
            }
        }
    }
}
