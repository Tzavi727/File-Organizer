using System.Collections.Generic;
using System.IO;

namespace FileOrganizer.Services
{
    internal class UndoService
    {
        public record UndoRecord(string FileName, string OldPath, string NewPath);

        private static List<UndoRecord> currentAction = new();

        private static Stack<List<UndoRecord>> undoStack = new();

        public static void AddToCurrentAction(string fileName, string oldPath, string newPath)
        {
            currentAction.Add(new UndoRecord(fileName, oldPath, newPath));
        }

        public static void CommitLastAction()
        {
            if (currentAction.Count > 0)
            {
                undoStack.Push(new List<UndoRecord>(currentAction));
                ClearCurrentActionList();
            }
        }
        public static void ClearCurrentActionList()
        {
            currentAction.Clear();
        }

        public static void ExecuteUndo()
        {
            if (undoStack.Count > 0)
            {
                var lastMovedFiles = undoStack.Pop();

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
}
