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
    private const int gridColumns = 5;
    private static readonly string[] Headers = { "Spaces", "Type", "Color", "Name", "ID" };

    private class DisplayRow
    {
        public bool IsEmpty { get; set; }
        public string[] Cells { get; set; } = new string[5]; // Spaces, Type, Color, Name, ID
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
                var v = handler.Garage.Vehicles[i];
                displayRows.Add(new DisplayRow
                {
                    IsEmpty = false,
                    Cells = 
                    [v.parkSpacesOccupied.ToCustomString(),
                    v.VehicleType.ToString(),
                    v.Color.ToString(),
                    v.Name,
                    v.RegisterID]
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
                for (int col = 0; col < gridColumns; col++)
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
