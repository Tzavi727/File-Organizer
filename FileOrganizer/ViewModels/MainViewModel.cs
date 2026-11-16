using FileOrganizer.Interfaces;

namespace FileOrganizer.ViewModels;

public class MainViewModel : ViewModelBase
{
    public HomeViewModel Home { get; }

    public MainViewModel(IFilePickerService filePickerService)
    {
        Home = new HomeViewModel(filePickerService);
    }
}