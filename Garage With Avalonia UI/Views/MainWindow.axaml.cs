using Avalonia.Controls;
using Avalonia.Interactivity;
using CSharp_Garage_Task;
using Metsys.Bson;
using System.Diagnostics;
using System.Reflection.Metadata;

namespace Garage_With_Avalonia_UI.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        int? garageSpacesGotten = CSharp_Garage_Task.Helper.GetIntFromAvalonia(Spaces.Text, 0);
        if (garageSpacesGotten == null)
        {
            return;
        }
        int garageSpaces = (int)garageSpacesGotten;
        Debug.WriteLine("Creating Garage with " + garageSpaces + " spaces");

        IHandler handler = new GarageHandler();

        bool looping = handler.CreateGarage(garageSpaces);
        GarageCreation.IsVisible = false;
        Garage.IsVisible = true;
        GarageSpaceCount.Text = "The garage has " + garageSpaces + " spaces";
    }

    private void Button_Add(object? sender, RoutedEventArgs e)
    {
        Debug.WriteLine("Adding vehicle");
    }
    private void Button_List(object? sender, RoutedEventArgs e)
    {
        Debug.WriteLine("Listing vehicles");
    }
    private void Button_Find(object? sender, RoutedEventArgs e)
    {
        Debug.WriteLine("Find vehicle");
    }
}