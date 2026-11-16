using FileOrganizer.Interfaces;
using FileOrganizer.Views;

namespace FileOrganizer.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public HomeViewModel Home { get; }

    public MainViewModel(IFilePickerService filePickerService)
    {
        Home = new HomeViewModel(filePickerService);
    }
}