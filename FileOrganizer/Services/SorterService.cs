using System.IO;
using System.Linq;

namespace FileOrganizer.src.Services
{
    internal class SorterService
    {
        public static void MoveFile(string file, string destinationFolder, string path)
        {
            string targetFolder = Path.Combine(path, destinationFolder);
            Directory.CreateDirectory(targetFolder);
            string fileName = Path.GetFileName(file);
            string finalPath = Path.Combine(targetFolder, fileName);
            File.Move(file, finalPath, true);
            UndoService.AddToCurrentAction(Path.GetFileName(file), file, finalPath);
            LogService.RecordMove(Path.GetFileName(file), file, finalPath);
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
            LogService.CommitSession();
            LogService.SaveLog();
            UndoService.CommitLastAction();
            return fileCounter;
        }

        public static int ExecuteOrganizationByExtension(string path, string extension, string destinationFolder)
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
                MoveFile(file, destinationFolder, path);
            }
            LogService.SaveLog();
            UndoService.CommitLastAction();
            return fileCounter;
        }
    }
}
