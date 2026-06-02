using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp_Garage_Task.VehicleClasses
{
    internal class Bus : Vehicle
    {
        public int Capacity { get; private set; }
        public static readonly int vehicleSize = 1;
        public Bus(string name, string registerID, VehicleColors color, VehicleTypes vehicleType, List<int> parkedNumber, int capacity) : base(name, registerID, color, vehicleType, parkedNumber)
        {
            pluralName = "Busses";
            Capacity = capacity;
            Size = vehicleSize;
        }

        public override string ToString(bool showSpaces)
        {
            return base.ToString(showSpaces) + " with a capacity of " + Capacity;
        }
        public override string GetSpecialValue()
        {
            return "Capacity: " + Capacity.ToString();
        }
    }
}
