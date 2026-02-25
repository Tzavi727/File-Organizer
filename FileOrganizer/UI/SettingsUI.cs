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
        REMOVE_RULE,
        LIST_RULES,
        BACK
    }
    internal class SettingsUI
    {
        public static void SettingsMenu()
        {
            UIutils.CleanScreen();
            UIutils.PrintSeparator();
            UIutils.PrintCentered("Settings/Customizations");
            UIutils.PrintSeparator();
            Console.WriteLine("1 - Add new extension\n2 - Remove extension\n3 - Check supported extensions\n4 - Go back");
            UIutils.PrintSeparator();
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
                UIutils.PrintSeparator();
                Console.WriteLine("Invalid Input! Try again.");
                UIutils.PrintSeparator();
            }
        }

        public static void HandleSettingsMenuInput()
        {
            settingsMenuEnum userInput = GetSettingsMenuInput();
            switch (userInput)
            {
                case settingsMenuEnum.ADD_NEW_EXTENSION:
                    RuleManagerUI.SetNewRuleUI();
                    UIutils.WaitingForInput();
                    break;
                case settingsMenuEnum.REMOVE_RULE:
                    RuleManagerUI.RemoveRuleUI();
                    UIutils.WaitingForInput();
                    break;
                case settingsMenuEnum.LIST_RULES:
                    RuleManagerUI.ListRules();
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
