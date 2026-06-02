using CSharp_Garage_Task.VehicleClasses;
using System;
using System.Collections.Generic;
using System.Text;
using static CSharp_Garage_Task.Garage<CSharp_Garage_Task.VehicleClasses.Vehicle>;
using static CSharp_Garage_Task.VehicleClasses.Vehicle;

namespace CSharp_Garage_Task
{
    internal class PredefinedGarages
    {
        internal static Garage<Vehicle> LuxuryGarage()
        {
            Garage<Vehicle> luxuryGarage = new(5);
            Vehicle fastCar = new Car("Wroom", "24", VehicleColors.Red, VehicleTypes.Car, 0.ToList(), Car.CarBrands.Porshe);
            luxuryGarage.AddVehicle(fastCar, 0);

            Vehicle yacht = new Boat("Yacht", "420", VehicleColors.Blue, VehicleTypes.Boat, 1.ToList(), true);
            luxuryGarage.AddVehicle(yacht, 1);

            List<int> zoomSpaces = [2, 3, 4];
            Vehicle coolPlane = new Airplane("Zoom", "87", VehicleColors.Silver, VehicleTypes.Airplane, zoomSpaces, 32);
            luxuryGarage.AddVehicle(coolPlane, zoomSpaces);
            return luxuryGarage;
        }

        internal static Garage<Vehicle> HugeGarage()
        {
            Garage<Vehicle> hugeGarage = new (25);
            Vehicle fastCar = new Car("Wroom", "24", VehicleColors.Red, VehicleTypes.Car, 0.ToList(), Car.CarBrands.Porshe);
            hugeGarage.AddVehicle(fastCar, 0);

            Vehicle mcBike = new Motorcycle("mcBike", "994MCC", VehicleColors.Black, VehicleTypes.Motorcycle, 1.ToList(), 120);
            hugeGarage.AddVehicle(mcBike, 1);

            Vehicle yesINeedASUV = new Car("yesINeedASUV", "221SUV", VehicleColors.White, VehicleTypes.Car, 2.ToList(), Car.CarBrands.Ford);
            hugeGarage.AddVehicle(yesINeedASUV, 2);

            Vehicle fancyElectric = new Car("fancyElectric", "410POW", VehicleColors.Silver, VehicleTypes.Car, 3.ToList(), Car.CarBrands.Renualt);
            hugeGarage.AddVehicle(fancyElectric, 3);

            Vehicle rustyButWorking = new Car("rustyButWorking", "822YER", VehicleColors.Green, VehicleTypes.Car, 4.ToList(), Car.CarBrands.Toyota);
            hugeGarage.AddVehicle(rustyButWorking, 4);

            Vehicle schoolBus = new Bus("schoolBus", "123BUS", VehicleColors.Yellow, VehicleTypes.Bus, 5.ToList(), 30);
            hugeGarage.AddVehicle(schoolBus, 5);

            Vehicle ordinaryBus = new Bus("ordinaryBus", "321BUS", VehicleColors.Red, VehicleTypes.Bus, 6.ToList(), 50);
            hugeGarage.AddVehicle(ordinaryBus, 6);

            Vehicle coolerBike = new Motorcycle("coolerBike", "995WIN", VehicleColors.Silver, VehicleTypes.Motorcycle, 7.ToList(), 150);
            hugeGarage.AddVehicle(coolerBike, 7);
            return hugeGarage;
        }

        internal static Garage<Vehicle> SpacedGarage()
        {
            Garage<Vehicle> spacedGarage = new (12);
            Vehicle fastCar = new Car("Wroom", "24", VehicleColors.Red, VehicleTypes.Car, 0.ToList(), Car.CarBrands.Porshe);
            spacedGarage.AddVehicle(fastCar, 0);

            Vehicle mcBike = new Motorcycle("mcBike", "994MCC", VehicleColors.Black, VehicleTypes.Motorcycle, 1.ToList(), 120);
            spacedGarage.AddVehicle(mcBike, 1);

            Vehicle fancyElectric = new Car("fancyElectric", "410POW", VehicleColors.Silver, VehicleTypes.Car, 3.ToList(), Car.CarBrands.Renualt);
            spacedGarage.AddVehicle(fancyElectric, 3);

            Vehicle coolerBike = new Motorcycle("coolerBike", "995WIN", VehicleColors.Silver, VehicleTypes.Motorcycle, 6.ToList(), 150);
            spacedGarage.AddVehicle(coolerBike, 6);

            List<int> daPlaneSpaces = [9, 10, 11];
            Vehicle daPlane = new Airplane("daPlane", "000FLY", VehicleColors.Blue, VehicleTypes.Airplane, daPlaneSpaces, 500);
            spacedGarage.AddVehicle(daPlane, daPlaneSpaces);

            Vehicle ohSeven = new Car("ohSeven", "007BON", VehicleColors.Silver, VehicleTypes.Car, 7.ToList(), Car.CarBrands.Saab);
            spacedGarage.AddVehicle(ohSeven, 7);
            return spacedGarage;
        }
    }
}

