using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp_Garage_Task.VehicleClasses
{
    internal class Motorcycle : Vehicle
    {
        public int TopSpeed { get; private set; }
        public static readonly int vehicleSize = 1;
        public Motorcycle(string name, string registerID, VehicleColors color, VehicleTypes vehicleType, List<int> parkedNumber, int topSpeed) : base(name, registerID, color, vehicleType, parkedNumber)
        {
            Wheels = 2;
            pluralName = "Motorcycles";
            TopSpeed = topSpeed;
            Size = vehicleSize;
        }

        public override string ToString(bool showSpaces)
        {
            return base.ToString(showSpaces) + " with a top speed of " + TopSpeed;
        }

        public override string GetSpecialValue()
        {
            return "Top Speed: " + TopSpeed.ToString();
        }
    }
}
