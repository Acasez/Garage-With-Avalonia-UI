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
        const string vehicleFilter = "What should we filter for? \n";
        const string vehicleCreation = "Lets create a vehicle. What type do you want?";
        const string vehicleColorChoice = "What color should our vehicle be? \n";
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

        public void AddVehicle()
        {
            if (!CheckForGarageSpace())
            {
                Helper.WriteWarningMessage("Garage Full");
                return;
            }
            List<int> largestEmptyLot = GetLargestEmptyLot();

            Helper.WriteMessage("Largest empty lot is " + largestEmptyLot.Count + " long.");
            Helper.WriteMessage(vehicleCreation);
            VehicleTypes vehicleType = IHandler.GetVehicleType(largestEmptyLot.Count);
            if (!Enum.IsDefined(vehicleType))
            {
                return;
            }
            List<int> garageSpace = largestEmptyLot.GetRange(0, IHandler.GetSizeOfVehicle(vehicleType));

            Helper.WriteMessage("Creating " + vehicleType);

            Helper.WriteMessage("Write vehicle name: ");
            string? vehicleName = Helper.GetInput();
            if (vehicleName == null)
            {
                Helper.WriteWarningMessage("Cam't have null name");
                return;
            }

            string? vehicleID = IUI.InputVehicleID(this);
            if (vehicleID == null)
            {
                return;
            }

            Helper.WriteMessage(vehicleColorChoice);
            VehicleColors vehicleColor = IHandler.GetVehicleColor();
            if (!Enum.IsDefined(vehicleColor))
            {
                return;
            }

            Vehicle? newVehicle = null;
            switch (vehicleType)
            {
                case VehicleTypes.Car:
                    Helper.WriteMessage("What's the car brand?");
                    Car.CarBrand carBrand = Car.GetCarBrand();
                    newVehicle = new Car(vehicleName, vehicleID, vehicleColor, vehicleType, garageSpace, carBrand);
                    break;
                case VehicleTypes.Motorcycle:
                    Helper.WriteMessage("What's the top speed");
                    int topSpeed = Helper.GetIntFromInput(0);
                    newVehicle = new Motorcycle(vehicleName, vehicleID, vehicleColor, vehicleType, garageSpace, topSpeed);
                    break;
                case VehicleTypes.Boat:
                    Helper.WriteMessage("Does the boat have sails? \n1: Yes \n2: No ");
                    bool sails = Helper.GetBoolFromInput();
                    newVehicle = new Boat(vehicleName, vehicleID, vehicleColor, vehicleType, garageSpace, sails);
                    break;
                case VehicleTypes.Airplane:
                    Helper.WriteMessage("How many flight hours do the plane have?");
                    int flightHours = Helper.GetIntFromInput(0);
                    newVehicle = new Airplane(vehicleName, vehicleID, vehicleColor, vehicleType, garageSpace, flightHours);
                    break;
                case VehicleTypes.Bus:
                    Helper.WriteMessage("How many people does the bus fit?");
                    int capacity = Helper.GetIntFromInput(0);
                    newVehicle = new Bus(vehicleName, vehicleID, vehicleColor, vehicleType, garageSpace, capacity);
                    break;
                default:
                    Helper.WriteErrorMessage("Invalid input, select a valid one.");
                    break;
            }
            if (newVehicle != null)
            {
                Garage.AddVehicle(newVehicle, garageSpace, true);
            }
        }


        public void DisplayGarageSpaces()
        {
            Vehicle? lastVehicle = null; //TODO, would like to format this as a grid
            List<int> currentNullSpaces = [];
            Helper.WriteMessage("There are " + Garage.ParkedVehicles + " vehicles and " + Garage.Vehicles.Length + " spaces.");
            Helper.WriteHorizontalLine();
            for (int i = 0; i < Garage.Vehicles.Length; i++)
            {
                if (Garage.Vehicles[i] == lastVehicle && Garage.Vehicles[i] != null) { continue; }
                if (Garage.Vehicles[i] != null)
                {
                    if (currentNullSpaces.Count > 0)
                    {
                        Helper.WriteMessage(Helper.WriteSpaces(currentNullSpaces.Count) + currentNullSpaces.ToCustomString() + " - No vehicles parked");
                        currentNullSpaces.Clear();
                    }
                    Helper.WriteMessage(Helper.WriteSpaces(Garage.Vehicles[i].parkSpacesOccupied.Count) + Garage.Vehicles[i].parkSpacesOccupied.ToCustomString() + " - " + Garage.Vehicles[i].ToString(false));
                }
                else
                {
                    currentNullSpaces.Add(i);
                }
                lastVehicle = Garage.Vehicles[i];
            }
            if (currentNullSpaces.Count > 0)
            {
                Helper.WriteMessage(Helper.WriteSpaces(currentNullSpaces.Count) + currentNullSpaces.ToCustomString() + " - No vehicles parked");
                currentNullSpaces.Clear();
            }
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

        public void FindVehicleById()
        {
            DisplayGarageSpaces();
            Helper.WriteMessage("Enter the ID of the vehicle you wish to find");
            string? vehicleID = Helper.GetInput();
            Vehicle? vehicle = GetVehicleByID(vehicleID);
            if (vehicle != null)
            {
                Helper.WriteMessage("Found vehicle " + vehicle.ToString(true));
                Helper.WriteMessage("Do you wish to remove the vehicle? \n1: Yes \n2: No ");
                bool remove = Helper.GetBoolFromInput();
                if (remove)
                {
                    Helper.WriteMessage("Removed vehicle " + vehicle.ToString(false), ConsoleColor.Yellow);
                    foreach (int newFreeSpace in vehicle.parkSpacesOccupied)
                    {
                        Garage.Vehicles[newFreeSpace] = null;
                    }
                    Garage.ParkedVehicles--;
                }
                else if (remove)
                {
                    Helper.WriteMessage("Not removing vehicle");
                }
                return;
            }
            else
            {
                Helper.WriteWarningMessage("Couldn't find vehicle witht that ID");
            }
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

        public void ListAllVehiclesFilterable()
        {
            VehicleTypes? typeFilter = null;
            VehicleColors? colorFilter = null;
            int? wheelCountFilter = null;
            bool looping = true;
            while (looping)
            {
                DisplayCurrentFilters(typeFilter, colorFilter, wheelCountFilter);
                List<Vehicle>? filteredVehicles = Garage.Where(v => v != null)
                    .Where(v => typeFilter == null || v.VehicleType == typeFilter)
                    .Where(v => colorFilter == null || v.Color == colorFilter)
                    .Where(v => wheelCountFilter == null || v.Wheels == wheelCountFilter)
                    .DistinctBy(v => v.RegisterID).ToList();

                filteredVehicles.ForEach(v => Helper.WriteMessage(v.ToString(true)));
                int fittingVehicles = filteredVehicles.Count;
                if (fittingVehicles == 0)
                {
                    Helper.WriteWarningMessage("No vehicles fitting filters");
                }
                Helper.WriteMessage(vehicleFilter);
                FilterOptions filter = IHandler.GetFilterOption();
                Helper.WriteMessage("Setup " + filter);
                switch (filter)
                {
                    case FilterOptions.Exit:
                        Helper.WriteMessage("Exiting view");
                        looping = false;
                        break;
                    case FilterOptions.Type:
                        typeFilter = IHandler.GetVehicleType();
                        break;
                    case FilterOptions.Color:
                        colorFilter = IHandler.GetVehicleColor();
                        break;
                    case FilterOptions.Wheels:
                        int wheelCount = Helper.GetIntFromInput(0);
                        wheelCountFilter = wheelCount;
                        break;
                    default:
                        Helper.WriteErrorMessage("Invalid input, select a valid one.");
                        break;
                }
            }
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
