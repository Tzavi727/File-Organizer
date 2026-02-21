using FileOrganizer.FileOrganizer.Settings;
using FileOrganizer.FileOrganizer.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FileOrganizer.FileOrganizer.Config
{
    internal class RuleManager
    {
        public static Dictionary<string, string> rules = new Dictionary<string, string>();

        public static void setRules()
        {
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

        public static void setNewRule()
        {
            MenuManager.CleanScreen();
            Console.WriteLine("=====================================================");
            Console.WriteLine("           Add new extension and folder");
            Console.WriteLine("   For the extension dont type the '.' write only the name");
            Console.WriteLine("               (e.g., exe or zip)");
            Console.WriteLine("=====================================================");
            Console.WriteLine("Extension: ");
            Console.WriteLine("=====================================================");
            String extension = Console.ReadLine().Trim().ToLower();
            if (string.IsNullOrWhiteSpace(extension))
            {
                extension = "Non-Specified extension";
            }
            Console.WriteLine("=====================================================");
            Console.WriteLine("Folder Name: ");
            Console.WriteLine("=====================================================");
            String folderName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(folderName))
            {
                folderName = "default_folder";
            }
            Console.WriteLine("=====================================================");
            Console.WriteLine($"| Added: | Extension: '{extension}' | To | Folder '{folderName}' |");
            Console.WriteLine("=====================================================");
            rules[extension] = folderName;
            Console.WriteLine("Press ENTER to continue: ");
            Console.WriteLine("=====================================================");
            Console.ReadLine();
            AppSettings.SaveRules();
        }

        public static void listRules()
        {
            MenuManager.CleanScreen();
            Console.WriteLine("=====================================================");
            Console.WriteLine("            Listing supported extensions...");
            Console.WriteLine("=====================================================");
            foreach (var rule in rules)
            {
                Console.WriteLine($"| Extension: {rule.Key,-3} | -> | Folder: {rule.Value,-3} | ");
                Console.WriteLine("=====================================================");
            }
        }
    }
}