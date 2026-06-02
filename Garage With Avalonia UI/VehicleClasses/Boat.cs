using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp_Garage_Task.VehicleClasses
{
    internal class Boat : Vehicle
    {
        public bool Sail { get; private set; }
        public static readonly int vehicleSize = 2;
        public Boat(string name, string registerID, VehicleColors color, VehicleTypes vehicleType, List<int> parkedNumber, bool sail) : base(name, registerID, color, vehicleType, parkedNumber)
        {
            Wheels = 0;
            pluralName = "Boats";
            Sail = sail;
            Size = vehicleSize;
        }

        public override string ToString(bool showSpaces)
        {
            return base.ToString(showSpaces) + " with" + (Sail ? " sails" : " no sails");
        }
        public override string GetSpecialValue()
        {
            return Sail.ToString();
        }
    }
}
