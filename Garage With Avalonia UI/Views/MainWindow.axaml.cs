using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using CSharp_Garage_Task;
using CSharp_Garage_Task.VehicleClasses;
using Metsys.Bson;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using static CSharp_Garage_Task.VehicleClasses.Car;
using static CSharp_Garage_Task.VehicleClasses.Vehicle;
using Helper = CSharp_Garage_Task.Helper;

namespace Garage_With_Avalonia_UI.Views;

public partial class MainWindow : Window
{
    GarageHandler handler = new();
    List<VehicleTypes> currentNonFittingVehicles = new List<VehicleTypes>();
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        if (!Helper.TryGetIntFromAvalonia(Spaces.Text, -3, 999, out int garageSpaces))
        {
            return;
        }
        Debug.WriteLine("Creating Garage with " + garageSpaces + " spaces");

        handler = new GarageHandler();

        handler.CreateGarage(garageSpaces);
        GarageCreation.IsVisible = false;
        Garage.IsVisible = true;
        GarageSpaceCount.Text = "The garage has " + handler.Garage.GarageCapacity + " spaces";
    }

    private void HideSubmenues()
    {
        AddVehicleSetup.IsVisible = false;
        VehicleListGrid.IsVisible = false;
    }

    private void Button_Add(object? sender, RoutedEventArgs e)
    {
        HideSubmenues();
        Debug.WriteLine("Adding vehicle");
        AddVehicleSetup.IsVisible = true;
        List<int> largestEmptyLot = handler.GetLargestEmptyLot();
        VehicleSpaces.Text = "Adding Vehicle to " + Helper.WriteSpaces(largestEmptyLot.Count) + "[" + largestEmptyLot.ToCustomString() + "]";

        List<VehicleTypes> fittingVehicles = IHandler.GetFittingVehicles(largestEmptyLot.Count);

        foreach (ComboBoxItem item in VehicleType.Items)
        {
            if (item.Content is string vehicleName && Enum.TryParse<VehicleTypes>(vehicleName, out var vehicleType))
            {
                item.IsEnabled = fittingVehicles.Contains(vehicleType);
            }
        }

        VehicleType.SelectedIndex = VehicleType.Items.OfType<ComboBoxItem>().ToList().FindIndex(x => x.IsEnabled);

    }
    private void Button_List(object? sender, RoutedEventArgs e)
    {
        HideSubmenues();
        Debug.WriteLine("Listing vehicles");
        VehicleListGrid.Children.Clear();
        VehicleListGrid.Children.Add(CreateGarageGrid());
        VehicleListGrid.IsVisible = true;
    }
    public Grid CreateGarageGrid()
    {
        var grid = new Grid
        {
            ShowGridLines = true,
            Margin = new Thickness(5)
        };

        // Define 4 columns: Spaces, Type, Color, Name+ID
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); // Spaces
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); // Type
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); // Color
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); // Name+ID

        int rowIndex = 0;
        Vehicle? lastVehicle = null;
        List<int> currentNullSpaces = new List<int>();

        for (int i = 0; i < handler.Garage.Vehicles.Length; i++)
        {
            // Skip duplicates
            if (handler.Garage.Vehicles[i] == lastVehicle && handler.Garage.Vehicles[i] != null) continue;

            // Handle empty spaces group
            if (handler.Garage.Vehicles[i] != null && currentNullSpaces.Count > 0)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                var emptyText = new TextBlock
                {
                    Text = currentNullSpaces.ToCustomString() + "- No vehicles parked",
                    Margin = new Thickness(0, 5, 0, 5)
                };
                Grid.SetRow(emptyText, rowIndex);
                Grid.SetColumnSpan(emptyText, 4); // Span all columns
                grid.Children.Add(emptyText);
                rowIndex++;
                currentNullSpaces.Clear();
            }

            // Handle vehicle
            if (handler.Garage.Vehicles[i] != null)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                // Spaces
                var spacesText = new TextBlock
                {
                    Text = handler.Garage.Vehicles[i].parkSpacesOccupied.ToCustomString(),
                    Margin = new Thickness(5, 5, 5, 5)
                };
                Grid.SetRow(spacesText, rowIndex);
                Grid.SetColumn(spacesText, 0);
                grid.Children.Add(spacesText);

                // Type
                var typeText = new TextBlock
                {
                    Text = handler.Garage.Vehicles[i].VehicleType.ToString(),
                    Margin = new Thickness(5, 5, 5, 5)
                };
                Grid.SetRow(typeText, rowIndex);
                Grid.SetColumn(typeText, 1);
                grid.Children.Add(typeText);

                // Color
                var colorText = new TextBlock
                {
                    Text = handler.Garage.Vehicles[i].Color.ToString(),
                    Margin = new Thickness(5, 5, 5, 5)
                };
                Grid.SetRow(colorText, rowIndex);
                Grid.SetColumn(colorText, 2);
                grid.Children.Add(colorText);

                // Name + ID
                var nameIdText = new TextBlock
                {
                    Text = handler.Garage.Vehicles[i].Name + " (ID: " + handler.Garage.Vehicles[i].RegisterID + ")",
                    Margin = new Thickness(5, 5, 5, 5)
                };
                Grid.SetRow(nameIdText, rowIndex);
                Grid.SetColumn(nameIdText, 3);
                grid.Children.Add(nameIdText);

                rowIndex++;
            }
            else
            {
                currentNullSpaces.Add(i);
            }

            lastVehicle = handler.Garage.Vehicles[i];
        }

        // Handle trailing empty spaces
        if (currentNullSpaces.Count > 0)
        {
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            var emptyText = new TextBlock
            {
                Text = currentNullSpaces.ToCustomString() + " - No vehicles parked",
                Margin = new Thickness(0, 5, 0, 5)
            };
            Grid.SetRow(emptyText, rowIndex);
            Grid.SetColumnSpan(emptyText, 4);
            grid.Children.Add(emptyText);
        }

        return grid;
    }

    private void Button_Find(object? sender, RoutedEventArgs e)
    {
        HideSubmenues();
        Debug.WriteLine("Find vehicle");
    }

    private void Button_Create(object? sender, RoutedEventArgs e) //Not quite done yet
    {
        List<int> largestEmptyLot = handler.GetLargestEmptyLot();
        VehicleTypes vehicleType = (VehicleTypes)VehicleType.SelectedIndex;
        if (!Helper.TryGetStringFromAvalonia(VehicleName.Text, out string vehicleName))
        {
            Debug.WriteLine("Vehicle name cannot be null");
            return;
        }
        if (!Helper.TryGetStringFromAvalonia(VehicleID.Text, out string vehicleID))
        {
            Debug.WriteLine("Vehicle ID cannot be null");
            return;
        }
        VehicleColors vehicleColor = (VehicleColors)VehicleColor.SelectedIndex;

        List<int> garageSpace = largestEmptyLot.GetRange(0, IHandler.GetSizeOfVehicle(vehicleType));
        Vehicle newVehicle = GetNewVehicle(vehicleType, vehicleName, vehicleID, vehicleColor, garageSpace);
         
        handler.Garage.AddVehicle(newVehicle, garageSpace, true);
        HideSubmenues();
    }

    private static Vehicle GetNewVehicle(VehicleTypes vehicleType, string vehicleName, string vehicleID, VehicleColors vehicleColor, List<int> garageSpace)
    {
        //TODO, don't hardcode specialvalues
        return vehicleType switch
        {
            VehicleTypes.Car => new Car(vehicleName, vehicleID, vehicleColor, vehicleType, garageSpace, CarBrand.Volvo),
            VehicleTypes.Motorcycle => new Motorcycle(vehicleName, vehicleID, vehicleColor, vehicleType, garageSpace, 50),
            VehicleTypes.Boat => new Boat(vehicleName, vehicleID, vehicleColor, vehicleType, garageSpace, false),
            VehicleTypes.Airplane => new Airplane(vehicleName, vehicleID, vehicleColor, vehicleType, garageSpace, 300),
            VehicleTypes.Bus => new Bus(vehicleName, vehicleID, vehicleColor, vehicleType, garageSpace, 25),
            _ => throw new System.NotImplementedException(),
        };
    }
}