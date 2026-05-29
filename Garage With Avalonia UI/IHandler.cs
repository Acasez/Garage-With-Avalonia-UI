using CSharp_Garage_Task;
using CSharp_Garage_Task.VehicleClasses;
using System;
using System.Collections.Generic;
using System.Text;
using static CSharp_Garage_Task.Garage<CSharp_Garage_Task.VehicleClasses.Vehicle>;
using static CSharp_Garage_Task.VehicleClasses.Vehicle;

internal interface IHandler
{
    bool CreateGarage(int garageSpaces);
    void DisplayGarageSpaces();
    Vehicle? GetVehicleByID(string? ID);
    void FindVehicleById();
    void ListVehiclesTypes();
    void ListAllVehiclesFilterable();
    bool CheckForGarageSpace();
    void AddVehicle();
    List<int> GetLargestEmptyLot();

    Vehicle GetFirstVehicle();

    #region Filters
    internal static VehicleColors GetVehicleColor()
    {
        while (true)
        {
            foreach (VehicleColors type in Enum.GetValues<VehicleColors>())
            {
                Helper.WriteMessage((int)type + ": Color " + type.ToString());
            }
            if (!int.TryParse(Helper.GetInput(), out int vehicleColorInt))
            {
                Helper.WriteErrorMessage("Error, not a interger");
                continue;
            }
            if (!Enum.IsDefined(typeof(VehicleColors), vehicleColorInt))
            {
                Helper.WriteErrorMessage("Invalid input, select a valid vehicle color.");
                continue;
            }
            return (VehicleColors)vehicleColorInt;
        }
    }

    internal static VehicleTypes GetVehicleType(int sizeLimit = 5)
    {
        List<VehicleTypes> nonFittingVehicles = [];
        List<VehicleTypes> fittingVehicles = [];
        foreach (VehicleTypes type in Enum.GetValues<VehicleTypes>())
        {
            if (GetSizeOfVehicle(type) > sizeLimit)
            {
                nonFittingVehicles.Add(type);
            }
            else
            {
                fittingVehicles.Add(type);
            }
        }

        while (true)
        {
            Helper.WriteMessage("-1: Cancel");
            foreach (VehicleTypes type in fittingVehicles)
            {
                Helper.WriteMessage((int)type + ": " + type.ToString());
            }
            foreach (VehicleTypes type in nonFittingVehicles)
            {
                Helper.WriteWarningMessage("Can't fit " + (int)type + ": " + type.ToString());
            }
            if (!int.TryParse(Helper.GetInput(), out int vehicleTypeInt))
            {
                Helper.WriteErrorMessage("Error, not an integer. Try again.");
                continue;
            }
            if (!Enum.IsDefined(typeof(VehicleTypes), vehicleTypeInt) && !(vehicleTypeInt == -1))
            {
                Helper.WriteErrorMessage("Invalid input, select a valid vehicle type. Try again.");
                continue; 
            }
            if (nonFittingVehicles.Contains((VehicleTypes)vehicleTypeInt))
            {
                Helper.WriteErrorMessage("Garage cannot fit vehicle");
                continue;
            }

            return (VehicleTypes)vehicleTypeInt;
        }
    }

    internal static int GetSizeOfVehicle(VehicleTypes vehicleType)
    {
        switch (vehicleType)
        {
            case VehicleTypes.Airplane:
                return Airplane.vehicleSize;
            case VehicleTypes.Boat:
                return Boat.vehicleSize;
            case VehicleTypes.Bus:
                return Bus.vehicleSize;
            case VehicleTypes.Car:
                return Car.vehicleSize;
            case VehicleTypes.Motorcycle:
                return Motorcycle.vehicleSize;
            default: 
                return 1;
        }
    }

    internal static FilterOptions GetFilterOption()
    {
        while (true)
        {
            foreach (FilterOptions type in Enum.GetValues<FilterOptions>())
            {
                Helper.WriteMessage((int)type + (type == 0 ? ": " : ": Vehicle ") + type.ToString());
            }
            if (!int.TryParse(Helper.GetInput(), out int vehicleFilterInt))
            {
                Helper.WriteErrorMessage("Error, not a interger");
                continue;
            }
            if (!Enum.IsDefined(typeof(FilterOptions), vehicleFilterInt))
            {
                Helper.WriteErrorMessage("Invalid input, select a valid vehicle filter.");
                continue;
            }
            return (FilterOptions)vehicleFilterInt;
        }
    }

    #endregion
}
