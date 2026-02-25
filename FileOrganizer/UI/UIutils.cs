using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOrganizer.UI
{
    internal class UIutils
    {
        private const int WIDTH = 61;
        private const char SYMBOL = '=';

        public static void PrintSeparator()
        {
            Console.WriteLine(new string(SYMBOL,WIDTH));
        }

        public static void PrintCentered(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                PrintSeparator();
                return;
            }
            int spaces = (WIDTH - text.Length) / 2;

            string centeredText = text.PadLeft(spaces + text.Length).PadRight(WIDTH);

            Console.WriteLine(centeredText);
        }
        public static void CleanScreen()
        {
            Console.Clear();
        }

        public static void FilesOrganizedMessage()
        {
            CleanScreen();
            PrintSeparator();
            PrintCentered("Your downloads folder should now be organized!");
            PrintSeparator();
            WaitingForInput();
        }

        public static void WaitingForInput()
        {
            PrintCentered("Press ENTER to continue:");
            PrintSeparator();
            Console.ReadLine();
        }

        public static void OperationCanceled()
        {
            CleanScreen();
            PrintSeparator();
            PrintCentered("Operation canceled.");
            PrintSeparator();
        }
    }
}
