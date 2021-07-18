using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace congestion.calculator
{
    public class Motorbike : Vehicle
    {
        public string GetVehicleType()
        {
            return "Motorbike";
        }
    }

    public class Tractor : Vehicle
    {
        public string GetVehicleType()
        {
            return "Tractor";
        }
    }
    public class Emergency : Vehicle
    {
        public string GetVehicleType()
        {
            return "Emergency";
        }
    }
    public class Diplomat : Vehicle
    {
        public string GetVehicleType()
        {
            return "Diplomat";
        }
    }
    public class Foreign : Vehicle
    {
        public string GetVehicleType()
        {
            return "Foreign";
        }
    }
    public class Military : Vehicle
    {
        public string GetVehicleType()
        {
            return "Military";
        }
    }
}