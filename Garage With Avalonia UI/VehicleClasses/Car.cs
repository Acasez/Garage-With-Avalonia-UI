using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp_Garage_Task.VehicleClasses
{
    internal class Car : Vehicle
    {
        public enum CarBrands
        {
            Volvo,
            Saab,
            Porshe,
            Toyota,
            Volkswagen,
            Ford,
            Nissan,
            Honda,
            Renualt
        }
        public CarBrands Brand { get; private set; }
        public static readonly int vehicleSize = 1;
        public Car(string name, string registerID, VehicleColors color, VehicleTypes vehicleType, List<int> parkedNumber, CarBrands brand) : base(name, registerID, color, vehicleType, parkedNumber)
        {
            pluralName = "Cars";
            Brand = brand;
            Size = vehicleSize;
        }

        public override string ToString(bool showSpaces)
        {
            return base.ToString(showSpaces) + " of car brand " + Brand;
        }
    }
}
