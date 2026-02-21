using FileOrganizer.FileOrganizer.Config;
using FileOrganizer.FileOrganizer.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOrganizer.FileOrganizer.Services
{
    internal class SorterService
    {
        public static string GetPath()
        {
            while (true)
            {
                MenuManager.CleanScreen();
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
                MenuManager.WaitingForInput();
            }
        }

        public static string AutoFindDownloadsPath()
        {
            string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            return downloadsPath;
        }

        public static string GetManualPath()
        {
            MenuManager.CleanScreen();
            Console.WriteLine("=====================================================");
            Console.WriteLine("           Manually Type Path Below: ");
            Console.WriteLine("=====================================================");
            String path = Console.ReadLine();
            if (!Path.Exists(path) || string.IsNullOrWhiteSpace(path))
            {
                MenuManager.CleanScreen();
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

        public static void MoveFile(string originalFile, string folderName, string path)
        {
            string targetFolder = Path.Combine(path, folderName);
            Directory.CreateDirectory(targetFolder);
            string fileNameOnly = Path.GetFileName(originalFile);
            string finalPath = Path.Combine(targetFolder, fileNameOnly);
            File.Move(originalFile, finalPath, true);
        }

        public static void ExecuteOrganization(string path)
        {
            if (path == null || !Path.Exists(path))
            {
                Console.WriteLine("Invalid Path! Cannot organize.");
                return;
            }
            string[] files = Directory.GetFiles(path);
            MenuManager.CleanScreen();
            foreach (string file in files)
            {
                string extension = Path.GetExtension(file).TrimStart('.');
                if (RuleManager.rules.ContainsKey(extension))
                {
                    string destinationFolder = RuleManager.rules[extension];
                    MoveFile(file, destinationFolder, path);
                }
            }
            MenuManager.FilesOrganizedMessage();
        }

        public static void ExecuteOrganizationByExtension(string path, string extension, string folderName)
        {
            if (path == null || !Path.Exists(path))
            {
                Console.WriteLine("Invalid Path! Cannot organize.");
                return;
            }
            MenuManager.CleanScreen();
            var targetFiles = Directory.GetFiles(path).Where(f => f.ToLower().EndsWith("." + extension));
            foreach (var file in targetFiles)
            {
                MoveFile(file, folderName, path);
            }
        }
    }
}
