using FileOrganizer.FileOrganizer.Services;
using FileOrganizer.FileOrganizer.Settings;
using FileOrganizer.FileOrganizer.UI;

AppSettings.LoadRules();
while (true)
{
    string selectedPath = null;
    MenuManager.showMainMenu();
    selectedPath = MenuManager.HandleMainMenuInput();
    if (selectedPath != null)
    {
        SorterService.ExecuteOrganization(selectedPath);
    }
}