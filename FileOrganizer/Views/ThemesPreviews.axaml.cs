using Avalonia.Controls;
using FileOrganizer.ViewModels;

namespace FileOrganizer.Views;

public partial class ThemesPreviews : Window
{
    //Default constructor to avoid null warnings.
    public ThemesPreviews()
    {
    }

    public ThemesPreviews(ThemesPreviewViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;

        viewModel.RequestClose += Close;
    }
}