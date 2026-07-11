using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using FileOrganizer.src.Services;
using System;

namespace FileOrganizer;

public partial class ThemesPreviews : Window
{
    public ThemesPreviews()
    {
        InitializeComponent();
    }

    private void ThemeList_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        var selectedItem = ThemeList.SelectedItem as ListBoxItem;
        var preview = selectedItem?.Tag as string;
        if (preview == null) return;
        var uri = new Uri($"avares://FileOrganizer/Assets/ThemesPreviews/{preview}");
        using (var stream = AssetLoader.Open(uri))
        {
            var bitmap = new Bitmap(stream);

            ThemePreview.Source = bitmap;
        }
    }

    private void SaveTheme_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var selectedItem = ThemeList.SelectedItem as ListBoxItem;
        if (selectedItem != null && selectedItem.Content != null)
        {
            string themeName = selectedItem.Content.ToString() ?? "Light";

            ThemesService.SwitchTheme(themeName);
        }
    }

    private void OK_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        this.Close();
    }
}