using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp_Garage_Task.VehicleClasses
{
    internal class Car : Vehicle
    {
        public enum CarBrand
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
        public CarBrand Brand { get; private set; }
        public static readonly int vehicleSize = 1;
        public Car(string name, string registerID, VehicleColors color, VehicleTypes vehicleType, List<int> parkedNumber, CarBrand brand) : base(name, registerID, color, vehicleType, parkedNumber)
        {
            pluralName = "Cars";
            Brand = brand;
            Size = vehicleSize;
        }

        public override string ToString(bool showSpaces)
        {
            return base.ToString(showSpaces) + " of car brand " + Brand;
        }

        internal static CarBrand GetCarBrand()
        {
            foreach (CarBrand type in Enum.GetValues<CarBrand>())
            {
                Helper.WriteMessage((int)type + ": " + type.ToString());
            }
            if (!int.TryParse(Console.ReadLine(), out int carBrandInt))
            {
                Helper.WriteErrorMessage("Error, not a interger");
            }
            if (!Enum.IsDefined(typeof(CarBrand), carBrandInt))
            {
                Helper.WriteErrorMessage("Invalid input, select a valid car brand.");
            }
            return (CarBrand)carBrandInt;
        }
    }
}
