namespace FileOrganizer.Interfaces;

public interface IUndoService
{
    void CommitLastAction();
    void ClearCurrentActionList();
    void ExecuteUndo();
}