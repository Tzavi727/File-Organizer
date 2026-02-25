using FileOrganizer.FileOrganizer.Config;
using FileOrganizer.FileOrganizer.Services;
using FileOrganizer.FileOrganizer.UI;
using FileOrganizer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOrganizer.UI
{
    internal class OrganizeByExtensionUI
    {
        public static void organizeByExtension()
        {
            UIutils.CleanScreen();
            organizeByExtensionMenu();
            string extension = handleOrganizeByExtensionMenuInput();
            if (extension.Equals("CANCEL"))
            {
                return;
            }
            UIutils.CleanScreen();
            string path = PathHandlers.GetPath();
            if(!RuleManager.TryGetExtensionFolder(extension, out string folderName))
            {
                RuleManagerUI.InvalidExtension();
                return;
            }
            SorterServiceUI.ExecuteOrganizationByExtension(path, extension, folderName);
            UIutils.FilesOrganizedMessage();
        }

        public static void organizeByExtensionMenu()
        {
            RuleManagerUI.ListRules();
            UIutils.PrintCentered("Choose a extension to organize or type 'back' to cancel");
            UIutils.PrintSeparator();
        }

        public static string handleOrganizeByExtensionMenuInput()
        {
            while (true)
            {
                string userChoosenExtension = Console.ReadLine().ToLower().Trim();
                if (userChoosenExtension.Equals("back"))
                {
                    return "CANCEL";
                }
                if (RuleManager.ContainsRule(userChoosenExtension))
                {
                    return userChoosenExtension;
                }
                else
                {
                    UIutils.CleanScreen();
                    UIutils.PrintSeparator();
                    Console.WriteLine($"Extension '{userChoosenExtension}' not found on the list");
                    Console.WriteLine("Try again or type 'back' to cancel");
                    UIutils.PrintSeparator();
                    UIutils.WaitingForInput();
                }
                organizeByExtensionMenu();
            }
        }
    }
}
