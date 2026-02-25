using FileOrganizer.FileOrganizer.Config;
using FileOrganizer.FileOrganizer.Services;
using FileOrganizer.Services;
using FileOrganizer.UI;
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

    internal class MainMenuUI
    {
        public static void ShowMainMenu()
        {
            UIutils.CleanScreen();
            Console.WriteLine("=====================================================");
            Console.WriteLine("               FILE ORGANIZER V1.8");
            Console.WriteLine("     Automatically sort your files into folders");
            Console.WriteLine("=====================================================");
            Console.WriteLine("1 - Try Auto Find Donwloads Path\n2 - Manually Type Path\n3 - Organize by Extension\n4 - Settings\n5 - End Program");
            Console.WriteLine("=====================================================");
        }

        public static mainMenu GetUserMainMenuInput()
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
            mainMenu userMainMenuInput = GetUserMainMenuInput();
            switch (userMainMenuInput)
            {
                case mainMenu.AUTO_FIND_PATH:
                    return PathHandlers.AutoFindDownloadsPath();
                case mainMenu.MANUAL_PATH:
                    return PathHandlers.GetManualPath();
                case mainMenu.ORGANIZE_BY_EXTENSION:
                    OrganizeByExtensionUI.organizeByExtension();
                    return null;
                case mainMenu.SETTINGS:
                    SettingsUI.SettingsMenu();
                    return null;
                case mainMenu.END_PROGRAM:
                    UIutils.CleanScreen();
                    Console.WriteLine("Exiting... Goodbye!");
                    Environment.Exit(0);
                    return null;
                default:
                    return null;
            }

        }
    }
}