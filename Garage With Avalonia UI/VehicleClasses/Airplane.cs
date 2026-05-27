using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp_Garage_Task.VehicleClasses
{
    internal class Airplane : Vehicle
    {
        public int FlightHours { get; private set; }
        public static readonly int vehicleSize = 3;
        public Airplane(string name, string registerID, VehicleColors color, VehicleTypes vehicleType, List<int> parkedNumber, int flightHours) : base(name, registerID, color, vehicleType, parkedNumber)
        {
            Wheels = 2;
            pluralName = "Airplanes";
            FlightHours = flightHours;
            Size = vehicleSize;
        }

        public override string ToString(bool showSpaces)
        {
            return base.ToString(showSpaces) + " with " + FlightHours + " flight hours";
        }
    }
}
