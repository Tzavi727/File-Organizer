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
                if (extension.Trim().ToLower().Equals("back"))
                {
                    return "CANCEL";
                }
                if (!string.IsNullOrWhiteSpace(extension))
                {
                    extension = extension.Trim().ToLower();
                    if (extension.StartsWith("."))
                        extension = extension.Substring(1);
                    return extension;
                }
                UIutils.PrintSeparator();
                Console.WriteLine("Extension cannot be empty. Try again.");
                UIutils.PrintSeparator();
            }
        }

        public static string GetFolderName()
        {
            while (true)
            {
                string folderName = Console.ReadLine();
                if (folderName.Trim().ToLower().Equals("back"))
                {
                    UIutils.OperationCanceled();
                    return "CANCEL";
                }
                if (!string.IsNullOrWhiteSpace(folderName))
                {
                    return folderName;
                }
                UIutils.PrintSeparator();
                Console.WriteLine("Folder name cannot be empty. Try again.");
                UIutils.PrintSeparator();
            }
        }

        public static void SetNewRuleUI()
        {
            UIutils.CleanScreen();
            UIutils.PrintSeparator();
            UIutils.PrintCentered("Add new extension and folder");
            UIutils.PrintCentered("For the extension dont type the '.' write only the name");
            UIutils.PrintCentered("(e.g., exe or zip)");
            UIutils.PrintSeparator();
            UIutils.PrintCentered("Or type 'back' to cancel");
            UIutils.PrintSeparator();
            Console.WriteLine("Extension: ");
            UIutils.PrintSeparator();
            string extension = GetExtensionName();
            if (extension.Equals("CANCEL"))
            {
                UIutils.OperationCanceled();
                return;
            }
            UIutils.PrintSeparator();
            Console.WriteLine("Folder Name: ");
            UIutils.PrintSeparator();
            string folderName = GetFolderName();
            if (folderName.Equals("CANCEL"))
            {
                UIutils.OperationCanceled();
                return;
            }
            UIutils.PrintSeparator();
            UIutils.PrintCentered($"| Added: | Extension: '{extension}' | To | Folder '{folderName}' |");
            UIutils.PrintSeparator();
            RuleManager.SetNewRule(extension, folderName);
            AppSettings.SaveRules();
        }

        public static void RemoveRuleUI()
        {
            ListRules();
            UIutils.PrintCentered("Type the extension you want to remove or 'back' to cancel:");
            UIutils.PrintSeparator();
            string extension = GetExtensionName();
            if (extension.Equals("CANCEL"))
            {
                UIutils.OperationCanceled();
                return;
            }
            if (!RuleManager.ContainsRule(extension))
            {
                UIutils.CleanScreen();
                UIutils.PrintSeparator();
                UIutils.PrintCentered($"Extension '{extension}' not found.");
                UIutils.PrintSeparator();
                return;
            }
            UIutils.CleanScreen();
            UIutils.PrintSeparator();
            Console.WriteLine($"Are you sure you want to delete the extension '{extension}' ?");
            UIutils.PrintSeparator();
            Console.WriteLine("1 - Yes \n2 - No");
            UIutils.PrintSeparator();
            while (true)
            {
                string userConfirmationString = Console.ReadLine().Trim().ToLower();
                if (int.TryParse(userConfirmationString, out int userConfirmationInt))
                {
                    if (userConfirmationInt == 1)
                    {
                        UIutils.CleanScreen();
                        UIutils.PrintSeparator();
                        UIutils.PrintCentered($"Extension '{extension}' deleted from your list");
                        UIutils.PrintSeparator();
                        RuleManager.RemoveRule(extension);
                        AppSettings.SaveRules();
                        return;
                    }
                    else if (userConfirmationInt == 2)
                    {
                        UIutils.OperationCanceled();
                        return;
                    }
                }
                UIutils.PrintSeparator();
                Console.WriteLine("Invalid Input. Choose between 1 and 2.");
                UIutils.PrintSeparator();
            }
        }

        public static void ListRules()
        {
            UIutils.CleanScreen();
            UIutils.PrintSeparator();
            UIutils.PrintCentered("Listing supported extensions...");
            UIutils.PrintSeparator();
            foreach (var rule in RuleManager.GetAllRules())
            {
                UIutils.PrintCentered($"| Extension: {rule.Key,-3} | -> | Folder: {rule.Value,-3} | ");
                UIutils.PrintSeparator();
            }
        }

        public static void InvalidExtension()
        {
            UIutils.CleanScreen();
            UIutils.PrintSeparator();
            UIutils.PrintCentered("Extension does not exist on the list.");
            UIutils.PrintSeparator();
            UIutils.WaitingForInput();
        }
    }
}
