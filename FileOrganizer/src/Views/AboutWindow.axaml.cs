using Avalonia.Controls;
using System;

namespace FileOrganizer;

public partial class AboutWindow : Window
{
    public AboutWindow()
    {
        InitializeComponent();
    }

    private void OpenGithubRepository(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        string url = "https://github.com/Tzavi727/File-Organizer";
        try
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(url) { UseShellExecute = true });
        }
        catch (Exception ex)
        {
            return;
        }
    }
}