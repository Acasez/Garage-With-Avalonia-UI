using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp_Garage_Task.VehicleClasses;

abstract class Vehicle : IVehicle
{
    public enum VehicleTypes
    {
        Car,
        Motorcycle,
        Boat,
        Airplane,
        Bus
    }
    public enum VehicleColors
    {
        White,
        Black,
        Red,
        Blue,
        Green,
        Silver,
        Yellow
    }
    public string Name { get; private set; }
    public int Wheels { get; protected set; } = 4;
    public int Size { get; protected set; } = 4;
    public string RegisterID { get; private set; }
    public VehicleColors Color { get; private set; }
    public VehicleTypes VehicleType { get; private set; }

    public readonly List<int> parkSpacesOccupied = [];

    public static string pluralName = "Vehicles";
    public Vehicle (string name, string registerID, VehicleColors color, VehicleTypes vehicleType, List<int> parkedNumbers)
    {
        Name = name;
        RegisterID = registerID;
        Color = color;
        VehicleType = vehicleType;
        parkSpacesOccupied = parkedNumbers;
    }

    public virtual string ToString(bool showSpaces)
    {
        if (showSpaces)
        {
            return VehicleType.ToString() + ": " + Name + " with ID " + RegisterID + " of color " + Color.ToString() + " occupying " + Helper.WriteSpaces(parkSpacesOccupied.Count).ToLower() + parkSpacesOccupied.ToCustomString();
        }
        return VehicleType.ToString() + ": " + Name + " with ID " + RegisterID + " of color " + Color.ToString();
    }
}
