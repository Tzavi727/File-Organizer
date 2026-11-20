using System.IO;
using FileOrganizer.Interfaces;

namespace FileOrganizer.Services;

internal class SorterService : ISorterService
{
    private readonly ILogService _logService;
    private readonly IUndoService _undoService;
    private readonly RuleService _ruleService;

    public SorterService(ILogService logService, IUndoService undoService, RuleService ruleService)
    {
        _logService = logService;
        _undoService = undoService;
        _ruleService = ruleService;
    }

    private void MoveFile(string file, string destinationFolder, string path)
    {
        string targetFolder = Path.Combine(path, destinationFolder);
        Directory.CreateDirectory(targetFolder);
        string fileName = Path.GetFileName(file);
        string finalPath = Path.Combine(targetFolder, fileName);
        File.Move(file, finalPath, true);

        _undoService.AddToCurrentAction(Path.GetFileName(file), file, finalPath);
        _logService.RecordMove(Path.GetFileName(file), file, finalPath);
    }

    public int ExecuteOrganization(string path)
    {
        var fileCounter = 0;
        if (string.IsNullOrWhiteSpace(path) || !Path.Exists(path))
        {
            return 0;
        }

        string[] files = Directory.GetFiles(path);
        foreach (string file in files)
        {
            string extension = Path.GetExtension(file).TrimStart('.');

            if (!_ruleService.ContainsRule(extension))
                continue;

            if (!_ruleService.TryGetExtensionFolder(extension, out string destinationFolder))
            {
                continue;
            }

            fileCounter++;
            MoveFile(file, destinationFolder, path);
        }

        _logService.CommitSession();
        _logService.SaveLog();
        _undoService.CommitLastAction();
        return fileCounter;
    }

    public int ExecuteOrganizationByExtension(string path, string extension, string destinationFolder)
    {
        var fileCounter = 0;
        if (string.IsNullOrWhiteSpace(path) || !Path.Exists(path))
        {
            return 0;
        }

        string normalizedTargetExt = extension.TrimStart('.').ToLowerInvariant();

        string[] files = Directory.GetFiles(path);
        foreach (var file in files)
        {
            string fileExt = Path.GetExtension(file).TrimStart('.').ToLowerInvariant();
            if (fileExt == normalizedTargetExt)
            {
                fileCounter++;
                MoveFile(file, destinationFolder, path);
            }
        }

        _logService.CommitSession();
        _logService.SaveLog();
        _undoService.CommitLastAction();
        return fileCounter;
    }
}