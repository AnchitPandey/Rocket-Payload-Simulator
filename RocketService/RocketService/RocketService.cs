using RocketService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace RocketService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "RocketService" in both code and config file together.

    [ServiceBehavior(ConcurrencyMode=ConcurrencyMode.Multiple, InstanceContextMode =InstanceContextMode.PerCall)]
    public class RocketService : IRocketService
    {

        public List<Double> increaseAltitude (double time, double altitude)
        {
            time -= 1;
            Random rand = new Random();
            int range = 100;
            altitude = altitude + rand.NextDouble() * range;
            altitude = Math.Round(altitude, 2);
            List<Double> ans = new List<Double>();
            ans.Add(time);
            ans.Add(altitude);
            return ans;
        }

        public List<Double> GetTelemetryData(double timeToOrbit)
        {
            Random rand = new Random();
            // change these
            int tempRange = 25;
            int latRange = 90;
            int longRange = 180;
            List<Double> telemData = new List<double>();
            double latitude =  Math.Round(rand.NextDouble() * latRange,2);
            double longitude = Math.Round(rand.NextDouble() * longRange,2);
            double temperature = Math.Round(rand.NextDouble() * tempRange,2);
            telemData.Add(latitude);
            telemData.Add(longitude);
            telemData.Add(temperature);
            if (timeToOrbit != 0)
            {
                telemData.Add(0);
            }
            else
                telemData.Add(0);
            return telemData;
        }
    }
}
