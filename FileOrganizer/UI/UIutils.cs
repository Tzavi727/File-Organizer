using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOrganizer.UI
{
    internal class UIutils
    {
        public static void CleanScreen()
        {
            Console.Clear();
        }

        public static void FilesOrganizedMessage()
        {
            CleanScreen();
            Console.WriteLine("=====================================================");
            Console.WriteLine("| Your downloads folder should now be organized! |");
            Console.WriteLine("=====================================================");
            WaitingForInput();
        }

        public static void WaitingForInput()
        {
            Console.WriteLine("Press ENTER to continue: ");
            Console.WriteLine("=====================================================");
            Console.ReadLine();
        }
    }
}
