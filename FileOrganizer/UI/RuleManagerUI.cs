using FileOrganizer.FileOrganizer.Config;
using FileOrganizer.FileOrganizer.Settings;
using FileOrganizer.FileOrganizer.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOrganizer.UI
{
    internal class RuleManagerUI
    {
        public static string GetExtensionName()
        {
            while (true)
            {
                string extension = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(extension))
                {
                    extension = extension.Trim().ToLower();
                    if (extension.StartsWith("."))
                        extension = extension.Substring(1);
                    return extension;
                }
                Console.WriteLine("Extension cannot be empty. Try again.");
                Console.WriteLine("=====================================================");
            }
        }

        public static string GetFolderName()
        {
            while (true)
            {
                string folderName = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(folderName))
                {
                    return folderName;
                }
                Console.WriteLine("Folder name cannot be empty. Try again.");
                Console.WriteLine("=====================================================");
            }
        }

        public static void SetNewRuleUI()
        {
            UIutils.CleanScreen();
            Console.WriteLine("=====================================================");
            Console.WriteLine("           Add new extension and folder");
            Console.WriteLine("   For the extension dont type the '.' write only the name");
            Console.WriteLine("               (e.g., exe or zip)");
            Console.WriteLine("=====================================================");
            Console.WriteLine("Extension: ");
            Console.WriteLine("=====================================================");
            String extension = GetExtensionName();
            Console.WriteLine("=====================================================");
            Console.WriteLine("Folder Name: ");
            Console.WriteLine("=====================================================");
            String folderName = GetFolderName();
            Console.WriteLine("=====================================================");
            Console.WriteLine($"| Added: | Extension: '{extension}' | To | Folder '{folderName}' |");
            Console.WriteLine("=====================================================");
            RuleManager.SetNewRule(extension, folderName);
            Console.WriteLine("Press ENTER to continue: ");
            Console.WriteLine("=====================================================");
            Console.ReadLine();
            AppSettings.SaveRules();
        }

        public static void listRules()
        {
            UIutils.CleanScreen();
            Console.WriteLine("=====================================================");
            Console.WriteLine("            Listing supported extensions...");
            Console.WriteLine("=====================================================");
            foreach (var rule in RuleManager.GetAllRules())
            {
                Console.WriteLine($"| Extension: {rule.Key,-3} | -> | Folder: {rule.Value,-3} | ");
                Console.WriteLine("=====================================================");
            }
        }

        public static void InvalidExtensionErrorMessage()
        {
            UIutils.CleanScreen();
            Console.WriteLine("=====================================================");
            Console.WriteLine("Extension does not exist on the list.");
            Console.WriteLine("=====================================================");
            UIutils.WaitingForInput();
        }
    }
}
