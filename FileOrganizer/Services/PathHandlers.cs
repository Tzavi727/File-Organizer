using FileOrganizer.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOrganizer.Services
{
    internal class PathHandlers
    {
        public static string GetPath()
        {
            while (true)
            {
                UIutils.CleanScreen();
                UIutils.PrintSeparator();
                UIutils.PrintCentered("Choose a path");
                UIutils.PrintSeparator();
                Console.WriteLine("1 - Try Auto Find Donwloads Path\n2 - Manually Type Path");
                UIutils.PrintSeparator();
                string userInputString = Console.ReadLine();
                if (int.TryParse(userInputString, out int userInputInt))
                {
                    switch (userInputInt)
                    {
                        case 1:
                            return AutoFindDownloadsPath();
                        case 2:
                            return GetManualPath();
                    }
                }
                UIutils.PrintSeparator();
                UIutils.PrintCentered("Invalid option!");
                UIutils.PrintCentered("Please Type a valid option");
                UIutils.PrintSeparator();
                UIutils.WaitingForInput();
            }
        }

        public static string AutoFindDownloadsPath()
        {
            string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            return downloadsPath;
        }

        public static string GetManualPath()
        {
            UIutils.CleanScreen();
            UIutils.PrintSeparator();
            UIutils.PrintCentered("Manually Type Path Below:");
            UIutils.PrintSeparator();
            String path = Console.ReadLine();
            if (!Path.Exists(path) || string.IsNullOrWhiteSpace(path))
            {
                UIutils.CleanScreen();
                UIutils.PrintSeparator();
                UIutils.PrintCentered("Path not found");
                UIutils.PrintSeparator();
                UIutils.PrintCentered("Press ENTER to try continue:");
                UIutils.PrintSeparator();
                Console.ReadLine();
                return null;
            }
            else
            {
                return path;
            }
        }
    }
}
