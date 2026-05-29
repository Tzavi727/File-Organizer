using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOrganizer.src.Logic
{
    internal class UndoService
    {
        private static List<(string Name, string OldPath, string NewPath)> currentAction = new();

        private static Stack<List<(string Name, string OldPath, string NewPath)>> undoStack = new();

        public static void AddToCurrentAction(string fileName, string oldPath, string newPath)
        {
            currentAction.Add((fileName, oldPath, newPath));
        }

        public static void CommitLastAction()
        {
            if (currentAction.Count > 0)
            {
                undoStack.Push(new List<(string Name, string OldPath, string NewPath)>(currentAction));
                ClearCurrentActionList();
            }
        }
        public static void ClearCurrentActionList()
        {
            currentAction.Clear();
        }
    }
}
