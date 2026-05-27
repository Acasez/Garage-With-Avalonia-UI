using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp_Garage_Task
{
    internal interface IUI
    {
        static abstract void StartDisplay();
        static abstract bool LoopDisplay(bool looping, IHandler handler);

        internal static string? InputVehicleID(IHandler handler)
        {
            Helper.WriteMessage("Write register ID (6 Chars)");
            string? vehicleID = Console.ReadLine();
            if (vehicleID == null)
            {
                Helper.WriteWarningMessage("Cam't have null ID");
                return null;
            }
            else if (handler.GetVehicleByID(vehicleID) != null)
            {
                Helper.WriteWarningMessage("Another vehicle with same ID already parked here");
                return null;
            }
            else if (vehicleID.Length != 6)
            {
                Helper.WriteWarningMessage("Register ID should be 6 characthers long");
                return null;
            }
            return vehicleID;
        }
    }
}
