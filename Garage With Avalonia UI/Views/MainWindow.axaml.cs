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
    private SortableColumn? currentSortColumn = null; // Make nullable
    private bool isAscending = true;

    private enum SortableColumn {Spaces, Type, Color, NameId }

    private class DisplayRow
    {
        public bool IsEmpty { get; set; }
        public string[] Cells { get; set; } = new string[4]; // Spaces, Type, Color, NameId
    }

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
        VehicleListGrid.Children.Add(CreateGarageGrid());
        VehicleListGrid.IsVisible = true;
    }

    private Grid CreateGarageGrid()
    {
        var grid = new Grid { ShowGridLines = true, Margin = new Thickness(5) };

        // Define 4 columns
        for (int i = 0; i < 4; i++)
            grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));

        // --- Header Row with Sort Buttons ---
        grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
        var headers = new[] { "Spaces", "Type", "Color", "Name (ID)" };

        for (int col = 0; col < 4; col++)
        {
            var btn = new Button
            {
                Content = headers[col],
                Margin = new Thickness(5),
                FontWeight = FontWeight.Bold,
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                Tag = (SortableColumn)col
            };
            btn.Click += SortButton_Click;
            Grid.SetRow(btn, 0);
            Grid.SetColumn(btn, col);
            grid.Children.Add(btn);
        }

        // --- Build Display Rows ---
        var displayRows = new List<DisplayRow>();
        Vehicle? lastVehicle = null;
        List<int> currentNullSpaces = [];

        for (int i = 0; i < handler.Garage.Vehicles.Length; i++)
        {
            if (handler.Garage.Vehicles[i] == lastVehicle && handler.Garage.Vehicles[i] != null) continue;

            // Add empty spaces group
            if (handler.Garage.Vehicles[i] != null && currentNullSpaces.Count > 0)
            {
                displayRows.Add(new DisplayRow
                {
                    IsEmpty = true,
                    Cells = [currentNullSpaces.ToCustomString(), "", "", "No vehicles parked"]
                });
                currentNullSpaces.Clear();
            }

            // Add vehicle
            if (handler.Garage.Vehicles[i] != null)
            {
                var v = handler.Garage.Vehicles[i];
                displayRows.Add(new DisplayRow
                {
                    IsEmpty = false,
                    Cells = [v.parkSpacesOccupied.ToCustomString(),
                    v.VehicleType.ToString(),
                    v.Color.ToString(),
                    $"{v.Name} (ID: {v.RegisterID})"]
                });
            }
            else
            {
                currentNullSpaces.Add(i);
            }

            lastVehicle = handler.Garage.Vehicles[i];
        }

        // Add trailing empty spaces
        if (currentNullSpaces.Count > 0)
        {
            displayRows.Add(new DisplayRow
            {
                IsEmpty = true,
                Cells = [currentNullSpaces.ToCustomString(), "", "", "No vehicles parked"]
            });
        }

        // --- Sort Rows if Needed ---
        if (currentSortColumn.HasValue)
        {
            displayRows = SortDisplayRows(displayRows, currentSortColumn.Value, isAscending);
        }

        // --- Add Data Rows ---
        for (int rowIdx = 0; rowIdx < displayRows.Count; rowIdx++)
        {
            var row = displayRows[rowIdx];
            grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));

            if (row.IsEmpty)
            {
                var txt = new TextBlock
                {
                    Text = $"{row.Cells[0]} - {row.Cells[3]}",
                    Margin = new Thickness(5)
                };
                Grid.SetRow(txt, rowIdx + 1);
                Grid.SetColumnSpan(txt, 4);
                grid.Children.Add(txt);
            }
            else
            {
                for (int col = 0; col < 4; col++)
                {
                    var txt = new TextBlock { Text = row.Cells[col], Margin = new Thickness(5) };
                    Grid.SetRow(txt, rowIdx + 1);
                    Grid.SetColumn(txt, col);
                    grid.Children.Add(txt);
                }
            }
        }

        return grid;
    }

    private void SortButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is SortableColumn column)
        {
            if (currentSortColumn == column)
                isAscending = !isAscending; // Toggle direction
            else
            {
                currentSortColumn = column; // New column
                isAscending = true; // Default to ascending
            }

            // Rebuild grid with new sorting
            VehicleListGrid.Children.Clear();
            VehicleListGrid.Children.Add(CreateGarageGrid());
        }
    }

    private static List<DisplayRow> SortDisplayRows(List<DisplayRow> rows, SortableColumn column, bool ascending)
    {
        // Separate empty and vehicle rows for better sorting
        var emptyRows = rows.Where(r => r.IsEmpty).ToList();
        var vehicleRows = rows.Where(r => !r.IsEmpty).ToList();

        // Sort vehicle rows by selected column
        vehicleRows = column switch
        {
            SortableColumn.Spaces => ascending
                ? vehicleRows.OrderBy(r => r.Cells[0]).ToList()
                : vehicleRows.OrderByDescending(r => r.Cells[0]).ToList(),
            SortableColumn.Type => ascending
                ? vehicleRows.OrderBy(r => r.Cells[1]).ToList()
                : vehicleRows.OrderByDescending(r => r.Cells[1]).ToList(),
            SortableColumn.Color => ascending
                ? vehicleRows.OrderBy(r => r.Cells[2]).ToList()
                : vehicleRows.OrderByDescending(r => r.Cells[2]).ToList(),
            SortableColumn.NameId => ascending
                ? vehicleRows.OrderBy(r => r.Cells[3]).ToList()
                : vehicleRows.OrderByDescending(r => r.Cells[3]).ToList(),
            _ => vehicleRows
        };

        // Sort empty rows by spaces
        emptyRows = ascending
            ? emptyRows.OrderBy(r => r.Cells[0]).ToList()
            : emptyRows.OrderByDescending(r => r.Cells[0]).ToList();

        // Combine: vehicles first, then empty spaces
        return vehicleRows.Concat(emptyRows).ToList();
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
            _ => throw new NotImplementedException(),
        };
    }
}