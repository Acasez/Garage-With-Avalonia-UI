using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Diagnostics;

namespace Garage_With_Avalonia_UI.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        Debug.WriteLine("Creating Garage with " + Spaces.Text + " spaces");
    }
}