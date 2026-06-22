using Avalonia.Controls;

namespace FileOrganizer;

public enum DialogType { Error, Confirm }
public partial class DialogBox : Window
{
    public DialogBox(string title, string message, DialogType type)
    {
        InitializeComponent();
        this.Title = title;
        DialogMessage.Text = message;

        switch (type)
        {
            case DialogType.Error:
                ErrorIcon.IsVisible = true;
                OkButton.Content = "Ok";
                break;

            case DialogType.Confirm:
                QuestionIcon.IsVisible = true;
                AbortButton.IsVisible = true;
                OkButton.Content = "Ok";
                break;
        }
    }

    private void OkButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        this.Close(true);
    }

    private void AbortButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        this.Close(false);
    }
}