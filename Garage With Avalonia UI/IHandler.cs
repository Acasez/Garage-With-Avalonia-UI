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
    Vehicle? GetVehicleByID(string? ID);
    void ListVehiclesTypes();
    bool CheckForGarageSpace();
    List<int> GetLargestEmptyLot();

    internal static List<VehicleTypes> GetFittingVehicles(int sizeLimit = 5)
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
        return fittingVehicles;
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
}
