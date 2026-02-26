using FileOrganizer.FileOrganizer.Config;
using FileOrganizer.FileOrganizer.Settings;
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
        RESTORE_DEFAULT,
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
            Console.WriteLine("1 - Add new extension\n2 - Remove extension\n3 - Check supported extensions\n4 - Restore default settings\n5 - Go back");
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
                case settingsMenuEnum.RESTORE_DEFAULT:
                    RestoreDefaultUI();
                    UIutils.WaitingForInput();
                    break;
                case settingsMenuEnum.BACK:
                    return;
                default:
                    return;
            }
        }

        public static void RestoreDefaultUI()
        {
            UIutils.CleanScreen();
            UIutils.PrintSeparator();
            UIutils.PrintCentered("PAY ATTENTION!");
            UIutils.PrintCentered("THIS WILL RESTORE ALL YOUR EXTENSIONS TO DEFAULT");
            UIutils.PrintSeparator();
            UIutils.PrintCentered("ARE YOU SURE YOU WANT TO PROCEED?");
            UIutils.PrintSeparator();
            Console.WriteLine("1 - Yes\n2 - No");
            UIutils.PrintSeparator();
            while (true)
            {
                string userConfirmationString = Console.ReadLine();
                if (!int.TryParse(userConfirmationString, out int userConfirmation))
                {
                    UIutils.PrintSeparator();
                    Console.WriteLine("Invalid input. Type only 1 or 2");
                    UIutils.PrintSeparator();
                }
                switch (userConfirmation)
                {
                    case 1:
                        UIutils.CleanScreen();
                        AppSettings.RestoreDefault();
                        UIutils.PrintSeparator();
                        UIutils.PrintCentered("EXTENSIONS RESTORED TO THE DEFAULT");
                        UIutils.PrintSeparator();
                        return;
                    case 2:
                        UIutils.OperationCanceled();
                        return;
                }
            }
        }
    }
}
