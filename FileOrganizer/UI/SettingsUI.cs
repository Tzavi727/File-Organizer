using FileOrganizer.FileOrganizer.Config;
using FileOrganizer.FileOrganizer.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOrganizer.UI
{
    public enum settingsMenuEnum
    {
        ADD_NEW_EXTENSION = 1,
        LIST_RULES,
        BACK
    }
    internal class SettingsUI
    {
        public static void SettingsMenu()
        {
            UIutils.CleanScreen();
            Console.WriteLine("=====================================================");
            Console.WriteLine("             Settings/Customizations");
            Console.WriteLine("=====================================================");
            Console.WriteLine("1 - Add new extension\n2 - Check supported extensions\n3 - Go back");
            Console.WriteLine("=====================================================");
            HandleSettingsMenuInput();
        }

        public static settingsMenuEnum GetSettingsMenuInput()
        {
            while (true)
            {
                string userInput = Console.ReadLine();
                if (Enum.TryParse<settingsMenuEnum>(userInput, true, out settingsMenuEnum userInputChoice)
                    && Enum.IsDefined(typeof(settingsMenuEnum), userInputChoice))
                {
                    return userInputChoice;
                }
                Console.WriteLine("=====================================================");
                Console.WriteLine("Invalid Input! Try again.");
                Console.WriteLine("=====================================================");
            }
        }

        public static void HandleSettingsMenuInput()
        {
            settingsMenuEnum userInput = GetSettingsMenuInput();
            switch (userInput)
            {
                case settingsMenuEnum.ADD_NEW_EXTENSION:
                    RuleManagerUI.SetNewRuleUI();
                    break;
                case settingsMenuEnum.LIST_RULES:
                    RuleManagerUI.listRules();
                    UIutils.WaitingForInput();
                    break;
                case settingsMenuEnum.BACK:
                    return;
                default:
                    return;
            }
        }
    }
}
