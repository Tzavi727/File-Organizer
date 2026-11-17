using System;
using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Input;

namespace FileOrganizer.Views;

public partial class AboutWindow : Window
{
    public AboutWindow()
    {
        InitializeComponent();
    }

    private void OpenGithubRepository(object? sender, PointerPressedEventArgs e)
    {
        string url = "https://github.com/Tzavi727/File-Organizer";
        try
        {
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to open Github repository: {ex.Message}");
        }
    }
}