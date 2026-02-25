using FileOrganizer.FileOrganizer.Services;
using FileOrganizer.FileOrganizer.Settings;
using FileOrganizer.FileOrganizer.UI;

AppSettings.LoadRules();
while (true)
{
    string selectedPath = null;
    MainMenuUI.ShowMainMenu();
    selectedPath = MainMenuUI.HandleMainMenuInput();
    if (selectedPath != null)
    {
        SorterServiceUI.ExecuteOrganization(selectedPath);
    }
}