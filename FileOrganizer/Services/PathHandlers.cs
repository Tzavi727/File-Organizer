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
                Console.WriteLine("=====================================================");
                Console.WriteLine("                 Choose a path");
                Console.WriteLine("=====================================================");
                Console.WriteLine("1 - Try Auto Find Donwloads Path\n2 - Manually Type Path");
                Console.WriteLine("=====================================================");
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
                Console.WriteLine("=====================================================");
                Console.WriteLine("Invalid option!\nPlease Type a valid option");
                Console.WriteLine("=====================================================");
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
            Console.WriteLine("=====================================================");
            Console.WriteLine("           Manually Type Path Below: ");
            Console.WriteLine("=====================================================");
            String path = Console.ReadLine();
            if (!Path.Exists(path) || string.IsNullOrWhiteSpace(path))
            {
                UIutils.CleanScreen();
                Console.WriteLine("=====================================================");
                Console.WriteLine("                  Path not found");
                Console.WriteLine("            Press ENTER to try continue:");
                Console.WriteLine("=====================================================");
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
