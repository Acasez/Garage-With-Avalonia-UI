using CSharp_Garage_Task.VehicleClasses;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using static CSharp_Garage_Task.VehicleClasses.Vehicle;

namespace CSharp_Garage_Task
{
    internal class Garage <T>: IEnumerable<T> where T : Vehicle 
    {
        public enum FilterOptions
        {
            Exit,
            Type,
            Color,
            Wheels
        }
        public Vehicle[] Vehicles { get; private set; }
        public int GarageCapacity { get; private set; }
        public int ParkedVehicles { get; set; }
        public Garage(int size)
        {
            if (size > 0)
            {
                GarageCapacity = size;
                Vehicles = new Vehicle[size];
            }
            else
            {
                throw new ArgumentException("Garage cannot be smaller than 0");
            }
        }
        public IEnumerator<T> GetEnumerator()
        {
            foreach (Vehicle vehicle in Vehicles)
            {
                yield return (T)vehicle;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        internal void AddVehicle(Vehicle vehicle, List<int> spaces, bool log = false)
        {
            if (log)
            {
                Helper.WriteMessage("Added vehicle " + vehicle.ToString(false) + " to garage space " + spaces.ToCustomString());
            }
            foreach (int space in spaces)
            {
                Vehicles[space] = vehicle;
                ParkedVehicles++;
            }
        }

    }
}
