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
    private bool isUpdating = false;

    private void Celcius_TextChanged(object? sender, TextChangedEventArgs e)
    {
        if (isUpdating) return;
        isUpdating = true;

        if (string.IsNullOrEmpty(Celsius.Text) || Celsius.Text == "-")
        {
            Fahrenheit.Text = "";
        }
        else if (double.TryParse(Celsius.Text, out double C))
        {
            var F = C * (9d / 5d) + 32;
            Fahrenheit.Text = F.ToString("0.0");
        }
        else
        {
            Celsius.Text = "0";
            Fahrenheit.Text = "0";
        }
        isUpdating = false;
    }

    private void Fahrenheit_TextChanged(object? sender, TextChangedEventArgs e)
    {
        if (isUpdating) return;
        isUpdating = true;

        if (string.IsNullOrEmpty(Fahrenheit.Text) || Fahrenheit.Text == "-")
        {
            Celsius.Text = "";
        }
        else if (double.TryParse(Fahrenheit.Text, out double F))
        {
            var C = (F - 32) * (5d / 9d); 
            Celsius.Text = C.ToString("0.0");
        }
        else
        {
            Celsius.Text = "0";
            Fahrenheit.Text = "0";
        }

        isUpdating = false;
    }
}