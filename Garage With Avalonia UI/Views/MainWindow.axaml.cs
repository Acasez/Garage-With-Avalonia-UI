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

        if (string.IsNullOrEmpty(Celsius.Text) || Celsius.Text == "-")
        {
            isUpdating = true;
            Fahrenheit.Text = "";
            isUpdating = false;
        }
        else if (double.TryParse(Celsius.Text, out double C))
        {
            var F = C * (9d / 5d) + 32;
            isUpdating = true;
            Fahrenheit.Text = F.ToString("0.0");
            isUpdating = false;
        }
        else
        {
            isUpdating = true;
            Celsius.Text = "0";
            Fahrenheit.Text = "0";
            isUpdating = false;
        }
    }

    private void Fahrenheit_TextChanged(object? sender, TextChangedEventArgs e)
    {
        if (isUpdating) return;

        if (string.IsNullOrEmpty(Fahrenheit.Text) || Fahrenheit.Text == "-")
        {
            isUpdating = true;
            Celsius.Text = "";
            isUpdating = false;
        }
        else if (double.TryParse(Fahrenheit.Text, out double F))
        {
            var C = (F - 32) * (5d / 9d); 
            isUpdating = true;
            Celsius.Text = C.ToString("0.0");
            isUpdating = false;
        }
        else
        {
            isUpdating = true;
            Celsius.Text = "0";
            Fahrenheit.Text = "0";
            isUpdating = false;
        }
    }
}