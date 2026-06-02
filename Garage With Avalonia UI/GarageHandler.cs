using CSharp_Garage_Task.VehicleClasses;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using static CSharp_Garage_Task.Garage<CSharp_Garage_Task.VehicleClasses.Vehicle>;
using static CSharp_Garage_Task.VehicleClasses.Vehicle;

namespace CSharp_Garage_Task
{
    internal class GarageHandler : IHandler
    {
        public Garage<Vehicle> Garage { get; private set; }

        public bool CreateGarage(int garageSpaces)
        {
            if (garageSpaces > 0)
            {
                Garage = new Garage<Vehicle>(garageSpaces);
            }
            else
            {
                switch (garageSpaces)
                {
                    case -1:
                        Garage = PredefinedGarages.LuxuryGarage();
                        break;
                    case -2:
                        Garage = PredefinedGarages.HugeGarage();
                        break;
                    case -3:
                        Garage = PredefinedGarages.SpacedGarage();
                        break;
                    default:
                        Helper.WriteErrorMessage("Invalid input, select a valid one.");
                        return false;
                }
            }
            return true;
        }

        public Vehicle? GetVehicleByID(string? ID)
        {
            if (ID == null)
            {
                return null;
            }
            foreach (Vehicle vehicle in Garage)
            {
                if (vehicle != null && vehicle.RegisterID.Equals(ID, StringComparison.CurrentCultureIgnoreCase))
                {
                    return vehicle;
                }
            }
            return null;
        }

        public void RemoveVehicle(Vehicle vehicle)
        {
            Helper.WriteMessage("Removed vehicle " + vehicle.ToString(false), ConsoleColor.Yellow);
            foreach (int newFreeSpace in vehicle.parkSpacesOccupied)
            {
                Garage.Vehicles[newFreeSpace] = null;
            }
            Garage.ParkedVehicles--;
        }

        public void ListVehiclesTypes()
        {
            var vehiclesByType = Garage.Where(v => v != null).DistinctBy(v => v.RegisterID).GroupBy(v => v.VehicleType).OrderBy(g => g.Key);

            foreach (var type in vehiclesByType)
            {
                Helper.WriteMessage("There are " + type.Count() + " " + type.Key + "s");
                foreach (var vehicle in type)
                {
                    Helper.WriteMessage(" - " + vehicle.ToString(true));
                }
            }
        }

        public Vehicle GetFirstVehicle()
        {
            return (Vehicle)Garage.Take(1);
        }

        public bool CheckForGarageSpace()
        {
            if (Garage.GarageCapacity > Garage.ParkedVehicles)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<int> GetLargestEmptyLot()
        {
            List<int> currentLot = [];
            List<int> largestLot = [];
            for (int i = 0; i < Garage.Vehicles.Length; i++)
            {
                if (Garage.Vehicles[i] == null)
                {
                    currentLot.Add(i);
                    if (currentLot.Count > largestLot.Count)
                    {
                        largestLot.Clear();
                        largestLot.AddRange(currentLot);
                    }
                }
                else
                {
                    currentLot.Clear();
                }
            }
            return largestLot;
        }

        private static void DisplayCurrentFilters(VehicleTypes? typeFilter, VehicleColors? colorFilter, int? wheelCountFilter)
        {
            if (typeFilter == null && colorFilter == null && wheelCountFilter == null)
            {
                Helper.WriteMessage("No filters currently", ConsoleColor.Green);
            }
            if (typeFilter != null)
            {
                Helper.WriteMessage("Type filter: " + typeFilter, ConsoleColor.Green);
            }
            if (colorFilter != null)
            {
                Helper.WriteMessage("Color filter: " + colorFilter, ConsoleColor.Green);
            }
            if (wheelCountFilter != null)
            {
                Helper.WriteMessage("Wheel count filter: " + wheelCountFilter, ConsoleColor.Green);
            }
        }
    }
}
