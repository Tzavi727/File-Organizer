using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using System;

namespace FileOrganizer.Services
{
    internal class ThemesService
    {
        public static void SwitchTheme(string themeName)
        {
            var uri = new Uri($"avares://FileOrganizer/Themes/{themeName}.axaml");
            var newTheme = (ResourceDictionary)AvaloniaXamlLoader.Load(uri);

            if (Application.Current != null)
            {
                Application.Current.Resources.MergedDictionaries.Clear();
                Application.Current.Resources.MergedDictionaries.Add(newTheme);

                Application.Current.RequestedThemeVariant = themeName == "Dark" ? ThemeVariant.Dark : ThemeVariant.Light;

                PreferencesService.Preferences.Theme = themeName;
                PreferencesService.SavePreferences();
            }
        }
    }
}
