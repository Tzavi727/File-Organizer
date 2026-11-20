using System;
using System.Collections.ObjectModel;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FileOrganizer.Interfaces;

namespace FileOrganizer.ViewModels;

public partial class ThemesPreviewViewModel : ViewModelBase
{
    private readonly IThemesService _themesService;
    [ObservableProperty] private string? _selectedTheme;
    [ObservableProperty] private Bitmap? _previewBitmap;
    public event Action? RequestClose;

    public ThemesPreviewViewModel(IThemesService themesService)
    {
        _themesService = themesService;
    }

    [ObservableProperty] private ObservableCollection<string> _themePreviews = ["Light", "Dark"];

    [RelayCommand]
    private void SaveTheme()
    {
        if (!string.IsNullOrEmpty(SelectedTheme))
        {
            _themesService.SwitchTheme(SelectedTheme);
        }
    }

    [RelayCommand]
    private void Ok()
    {
        SaveTheme();
        RequestClose?.Invoke();
    }

    partial void OnSelectedThemeChanged(string? value)
    {
        if (string.IsNullOrEmpty(value)) return;

        string previewFileName = value switch
        {
            "Light" => "LightPreview.jpeg",
            "Dark" => "DarkPreview.jpeg",
            _ => "LightPreview.jpeg"
        };

        try
        {
            var uri = new Uri($"avares://FileOrganizer/Assets/ThemesPreviews/{previewFileName}");
            using var stream = AssetLoader.Open(uri);
            PreviewBitmap = new Bitmap(stream);
        }
        catch
        {
            PreviewBitmap = null;
        }
    }
}