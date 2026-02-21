using FileOrganizer.FileOrganizer.Config;
using FileOrganizer.FileOrganizer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOrganizer.FileOrganizer.UI
{
    public enum mainMenu
    {
        AUTO_FIND_PATH = 1,
        MANUAL_PATH,
        ORGANIZE_BY_EXTENSION,
        SETTINGS,
        END_PROGRAM
    }

    public enum settingsMenuEnum
    {
        ADD_NEW_EXTENSION = 1,
        LIST_RULES,
        BACK
    }
    internal class MenuManager
    {
        public static void showMainMenu()
        {
            CleanScreen();
            Console.WriteLine("=====================================================");
            Console.WriteLine("               FILE ORGANIZER V1.7");
            Console.WriteLine("     Automatically sort your files into folders");
            Console.WriteLine("=====================================================");
            Console.WriteLine("1 - Try Auto Find Donwloads Path\n2 - Manually Type Path\n3 - Organize by Extension\n4 - Settings\n5 - End Program");
            Console.WriteLine("=====================================================");
        }

        public static mainMenu getUserMainMenuInput()
        {
            while (true)
            {
                String userInput = Console.ReadLine();
                if (Enum.TryParse<mainMenu>(userInput, true, out mainMenu userMenuChoice) && Enum.IsDefined(typeof(mainMenu), userMenuChoice))
                {
                    return userMenuChoice;
                }
                Console.WriteLine("=====================================================");
                Console.WriteLine("Invalid Input! Try again.");
                Console.WriteLine("=====================================================");
            }
        }

        public static string HandleMainMenuInput()
        {
            mainMenu userMainMenuInput = getUserMainMenuInput();
            switch (userMainMenuInput)
            {
                case mainMenu.AUTO_FIND_PATH:
                    return SorterService.AutoFindDownloadsPath();
                case mainMenu.MANUAL_PATH:
                    return SorterService.GetManualPath();
                case mainMenu.ORGANIZE_BY_EXTENSION:
                    organizeByExtension();
                    return null;
                case mainMenu.SETTINGS:
                    settingsMenu();
                    return null;
                case mainMenu.END_PROGRAM:
                    CleanScreen();
                    Console.WriteLine("Exiting... Goodbye!");
                    Environment.Exit(0);
                    return null;
                default:
                    return null;
            }

        }

        public static void organizeByExtension()
        {
            CleanScreen();
            organizeByExtensionMenu();
            string extension = handleOrganizeByExtensionMenuInput();
            if (extension.Equals("CANCEL"))
            {
                return;
            }
            CleanScreen();
            string path = SorterService.GetPath();
            string folderName = RuleManager.rules[extension];
            SorterService.ExecuteOrganizationByExtension(path, extension, folderName);
            MenuManager.FilesOrganizedMessage();
        }

        public static void organizeByExtensionMenu()
        {
            RuleManager.listRules();
            Console.WriteLine("Choose a extension to organize or type 'back' to cancel");
            Console.WriteLine("=====================================================");
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
                if (RuleManager.rules.ContainsKey(userChoosenExtension))
                {
                    return userChoosenExtension;
                }
                else
                {
                    CleanScreen();
                    Console.WriteLine("=====================================================");
                    Console.WriteLine($"Extension '{userChoosenExtension}' not found on the list");
                    Console.WriteLine("Try again or type 'back' to cancel");
                    Console.WriteLine("=====================================================");
                    WaitingForInput();
                }
                organizeByExtensionMenu();
            }
        }

        public static void settingsMenu()
        {
            CleanScreen();
            Console.WriteLine("=====================================================");
            Console.WriteLine("             Settings/Customizations");
            Console.WriteLine("=====================================================");
            Console.WriteLine("1 - Add new extension\n2 - Check supported extensions\n3 - Go back");
            Console.WriteLine("=====================================================");
            handleSettingsMenuInput();
        }

        public static settingsMenuEnum getSettingsMenuInput()
        {
            while (true)
            {
                string userInput = Console.ReadLine();
                if (Enum.TryParse<settingsMenuEnum>(userInput, true, out settingsMenuEnum userInputChoice)
                    && Enum.IsDefined(typeof(settingsMenuEnum), userInputChoice))
                {
                    return userInputChoice;
                }
                Console.WriteLine("=====================================================");
                Console.WriteLine("Invalid Input! Try again.");
                Console.WriteLine("=====================================================");
            }
        }

        public static void handleSettingsMenuInput()
        {
            settingsMenuEnum userInput = getSettingsMenuInput();
            switch (userInput)
            {
                case settingsMenuEnum.ADD_NEW_EXTENSION:
                    RuleManager.setNewRule();
                    break;
                case settingsMenuEnum.LIST_RULES:
                    RuleManager.listRules();
                    WaitingForInput();
                    break;
                case settingsMenuEnum.BACK:
                    return;
                default:
                    return;
            }
        }

        public static void WaitingForInput()
        {
            Console.WriteLine("Press ENTER to continue: ");
            Console.WriteLine("=====================================================");
            Console.ReadLine();
        }

        public static void CleanScreen()
        {
            Console.Clear();
        }

        public static void FilesOrganizedMessage()
        {
            CleanScreen();
            Console.WriteLine("=====================================================");
            Console.WriteLine("| Your downloads folder should now be organized! |");
            Console.WriteLine("=====================================================");
            WaitingForInput();
        }
    }
}