using FileOrganizer.FileOrganizer.Config;
using FileOrganizer.FileOrganizer.UI;
using FileOrganizer.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOrganizer.FileOrganizer.Services
{
    internal class SorterServiceUI
    {
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
            UIutils.CleanScreen();
            foreach (string file in files)
            {
                string extension = Path.GetExtension(file).TrimStart('.');
                if (RuleManager.ContainsRule(extension))
                {
                    if (!RuleManager.TryGetExtensionFolder(extension, out string destinationFolder))
                    {
                        RuleManagerUI.InvalidExtensionErrorMessage();
                        return;
                    }
                    MoveFile(file, destinationFolder, path);
                }
            }
            UIutils.FilesOrganizedMessage();
        }

        public static void ExecuteOrganizationByExtension(string path, string extension, string folderName)
        {
            if (path == null || !Path.Exists(path))
            {
                Console.WriteLine("Invalid Path! Cannot organize.");
                return;
            }
            UIutils.CleanScreen();
            var targetFiles = Directory.GetFiles(path).Where(f => f.ToLower().EndsWith("." + extension));
            foreach (var file in targetFiles)
            {
                MoveFile(file, folderName, path);
            }
        }
    }
}
