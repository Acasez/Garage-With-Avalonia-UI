using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using CSharp_Garage_Task;
using CSharp_Garage_Task.VehicleClasses;
using System.Collections.Generic;
using System.Linq;

namespace Garage_With_Avalonia_UI.Views;

internal class GridCreator(GarageHandler handlerRef, StackPanel vehicleListGridRef)
{
    private SortableColumn? currentSortColumn = null; // Make nullable
    private bool isAscending = true;
    private enum SortableColumn { Spaces, Type, Color, Name, ID }
    private readonly GarageHandler handler = handlerRef;
    private readonly StackPanel vehicleListGrid = vehicleListGridRef;
    private const int gridColumns = 6;
    private static readonly string[] Headers = { "Spaces", "Type", "Color", "Name", "ID", "Remove" };

    private class DisplayRow
    {
        public bool IsEmpty { get; set; }
        public string[] Cells { get; set; } = new string[6]; // Spaces, Type, Color, Name, ID, RemoveButton
        public Vehicle? Vehicle { get; set; }
    }

    internal Grid CreateGarageGrid()
    {
        var grid = new Grid { ShowGridLines = true, Margin = new Thickness(5) };

        // Define the grid columns
        for (int i = 0; i < gridColumns; i++)
            grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));

        // --- Header Row with Sort Buttons ---
        grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));

        for (int col = 0; col < gridColumns; col++)
        {
            var btn = new Button
            {
                Content = Headers[col],
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
                Vehicle vehicle = handler.Garage.Vehicles[i];
                displayRows.Add(new DisplayRow
                {
                    IsEmpty = false,
                    Vehicle = vehicle,
                    Cells =
                    [
                        vehicle.parkSpacesOccupied.ToCustomString(),
                        vehicle.VehicleType.ToString(),
                        vehicle.Color.ToString(),
                        vehicle.Name,
                        vehicle.RegisterID
                    ]
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
                Cells = [currentNullSpaces.ToCustomString(), "", "", "", "No vehicles parked"]
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
                Grid.SetColumnSpan(txt, gridColumns);
                grid.Children.Add(txt);
            }
            else
            {
                for (int col = 0; col < 5; col++)
                {
                    var txt = new TextBlock
                    {
                        Text = row.Cells[col],
                        Margin = new Thickness(5)
                    };

                    Grid.SetRow(txt, rowIdx + 1);
                    Grid.SetColumn(txt, col);
                    grid.Children.Add(txt);
                }

                var deleteButton = new Button
                {
                    Content = "Delete",
                    Tag = row.Vehicle,
                    Margin = new Thickness(5)
                };

                deleteButton.Click += DeleteButton_Click;

                Grid.SetRow(deleteButton, rowIdx + 1);
                Grid.SetColumn(deleteButton, 5);
                grid.Children.Add(deleteButton);
            }
        }

        return grid;
    }

    private void DeleteButton_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button &&
            button.Tag is Vehicle vehicle)
        {
            // Your existing deletion code here
            handler.RemoveVehicle(vehicle);

            vehicleListGrid.Children.Clear();
            vehicleListGrid.Children.Add(CreateGarageGrid());
        }
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
            vehicleListGrid.Children.Clear();
            vehicleListGrid.Children.Add(CreateGarageGrid());
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
            SortableColumn.Name => ascending
                ? vehicleRows.OrderBy(r => r.Cells[3]).ToList()
                : vehicleRows.OrderByDescending(r => r.Cells[3]).ToList(),
            SortableColumn.ID => ascending
                ? vehicleRows.OrderBy(r => r.Cells[4]).ToList()
                : vehicleRows.OrderByDescending(r => r.Cells[4]).ToList(),
            _ => vehicleRows
        };

        // Sort empty rows by spaces
        emptyRows = ascending
            ? emptyRows.OrderBy(r => r.Cells[0]).ToList()
            : emptyRows.OrderByDescending(r => r.Cells[0]).ToList();

        // Combine: vehicles first, then empty spaces
        return vehicleRows.Concat(emptyRows).ToList();
    }
}
