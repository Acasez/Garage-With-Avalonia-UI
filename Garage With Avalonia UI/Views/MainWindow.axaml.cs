using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
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
    GridCreator gridCreator;

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

        CreateGarage(garageSpaces);
    }

    private void CreateGarage(int garageSpaces)
    {
        handler = new GarageHandler();

        handler.CreateGarage(garageSpaces);
        GarageCreation.IsVisible = false;
        Garage.IsVisible = true;
        GarageSpaceCount.Text = "The garage has " + handler.Garage.GarageCapacity + " spaces";
        gridCreator = new(handler, VehicleListGrid);
    }
    private void Button_Luxury(object? sender, RoutedEventArgs e)
    {
        CreateGarage(-1);
    }
    private void Button_Huge(object? sender, RoutedEventArgs e)
    {
        CreateGarage(-2);
    }
    private void Button_Spaced(object? sender, RoutedEventArgs e)
    {
        CreateGarage(-3);
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

        foreach (ComboBoxItem item in VehicleType.Items.Cast<ComboBoxItem>())
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
        VehicleListGrid.Children.Add(gridCreator.CreateGarageGrid());
        VehicleListGrid.IsVisible = true;
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
        Debug.WriteLine("Creating vehicle with name " + vehicleName + " and ID " + vehicleID);
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
            _ => throw new NotImplementedException(),
        };
    }
}