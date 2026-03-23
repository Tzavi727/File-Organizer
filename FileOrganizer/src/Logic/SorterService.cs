using FileOrganizer.FileOrganizer.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOrganizer.FileOrganizer.Services
{
    internal class SorterService
    {
        public static void MoveFile(string originalFile, string folderName, string path)
        {
            string targetFolder = Path.Combine(path, folderName);
            Directory.CreateDirectory(targetFolder);
            string fileNameOnly = Path.GetFileName(originalFile);
            string finalPath = Path.Combine(targetFolder, fileNameOnly);
            File.Move(originalFile, finalPath, true);
        }

        public static int ExecuteOrganization(string path)
        {
            int fileCounter = 0;
            if (path == null || !Path.Exists(path))
            {
                return 0;
            }
            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                string extension = Path.GetExtension(file).TrimStart('.');
                if (AppConfigs.ContainsRule(extension))
                {
                    if (!AppConfigs.TryGetExtensionFolder(extension, out string destinationFolder))
                    {
                        continue;
                    }
                    fileCounter++;
                    MoveFile(file, destinationFolder, path);
                }
            }
            return fileCounter;
        }

        public static int ExecuteOrganizationByExtension(string path, string extension, string folderName)
        {
            int fileCounter = 0;
            if (path == null || !Path.Exists(path))
            {
                return 0;
            }
            var targetFiles = Directory.GetFiles(path).Where(f => f.ToLower().EndsWith("." + extension));
            foreach (var file in targetFiles)
            {
                fileCounter++;
                MoveFile(file, folderName, path);
            }
            return fileCounter;
        }
    }
}
