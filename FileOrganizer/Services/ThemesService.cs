using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using FileOrganizer.Interfaces;

namespace FileOrganizer.Services;

internal class ThemesService : IThemesService
{
    private readonly IPreferencesService _preferencesService;

    public ThemesService(IPreferencesService preferencesService)
    {
        _preferencesService = preferencesService;
    }

    public void SwitchTheme(string themeName)
    {
        var uri = new Uri($"avares://FileOrganizer/Themes/{themeName}.axaml");
        var newTheme = (ResourceDictionary)AvaloniaXamlLoader.Load(uri);

        if (Application.Current == null)
            return;

        Application.Current.Resources.MergedDictionaries.Clear();
        Application.Current.Resources.MergedDictionaries.Add(newTheme);

        Application.Current.RequestedThemeVariant = themeName == "Dark" ? ThemeVariant.Dark : ThemeVariant.Light;

        _preferencesService.Theme = themeName;
        _preferencesService.SavePreferences();
    }
}