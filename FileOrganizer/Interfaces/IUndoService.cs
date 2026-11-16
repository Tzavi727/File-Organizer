namespace FileOrganizer.Interfaces;

public interface IUndoService
{
    void AddToCurrentAction(string fileName, string oldPath, string newPath);
    void CommitLastAction();
    void ClearCurrentActionList();
    void ExecuteUndo();
}