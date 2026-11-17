using System.Collections.Generic;
using System.IO;
using FileOrganizer.Interfaces;

namespace FileOrganizer.Services;

internal class UndoService : IUndoService
{
    public record UndoRecord(string FileName, string OldPath, string NewPath);

    private List<UndoRecord> _currentAction = new();

    private Stack<List<UndoRecord>> _undoStack = new();

    public void AddToCurrentAction(string fileName, string oldPath, string newPath)
    {
        _currentAction.Add(new UndoRecord(fileName, oldPath, newPath));
    }

    public void CommitLastAction()
    {
        if (_currentAction.Count > 0)
        {
            _undoStack.Push(new List<UndoRecord>(_currentAction));
            ClearCurrentActionList();
        }
    }

    public void ClearCurrentActionList()
    {
        _currentAction.Clear();
    }

    public void ExecuteUndo()
    {
        if (_undoStack.Count > 0)
        {
            var lastMovedFiles = _undoStack.Pop();

            foreach (UndoRecord move in lastMovedFiles)
            {
                if (File.Exists(move.NewPath))
                {
                    File.Move(move.NewPath, move.OldPath, overwrite: true);
                }
            }
        }
    }
}