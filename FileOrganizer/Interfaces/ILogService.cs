namespace FileOrganizer.Interfaces;

public interface ILogService
{
    void RecordMove(string fileName, string oldPath, string newPath);
    void CommitSession();
    void SaveLog();
}