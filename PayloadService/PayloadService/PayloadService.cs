using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace PayloadService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "PayloadService" in both code and config file together.
    public class PayloadService : IPayloadService
    {
        public List<Double> GetPayloadTelemetryData()
        {
            Random rand = new Random();
            // change these
            int tempRange = 30;
            int latRange = 90;
            int longRange = 180;
            List<Double> telemData = new List<double>();
            double latitude = Math.Round(rand.NextDouble() * latRange, 2);
            double longitude = Math.Round(rand.NextDouble() * longRange, 2);
            double temperature = Math.Round(rand.NextDouble() * tempRange, 2);
            int altRange = 5000;
            double altitude = Math.Round(rand.NextDouble() * altRange, 2);
            telemData.Add(altitude);
            telemData.Add(latitude);
            telemData.Add(longitude);
            telemData.Add(temperature);
            return telemData;
        }

        public List<Double> GetPayloadData(String type, int index)
        {
            Random rand = new Random();
            // change these
            int tempRange = 100;
            int latRange = 200;
          
            int longRange = 340;
            List<Double> telemData = new List<double>();
            double latitude = Math.Round(rand.NextDouble() * latRange, 2);
            double longitude = Math.Round(rand.NextDouble() * longRange, 2);
            double temperature = Math.Round(rand.NextDouble() * tempRange, 2);
            //int altRange = 500;
            //double altitude = Math.Round(rand.NextDouble() * altRange, 2);
            //telemData.Add(altitude);
            telemData.Add(latitude);
            telemData.Add(longitude);

            if(type == "communication" || type == "scientific")
                telemData.Add(temperature);
            else
            {
                index = index + 1;
                index = index % 3;
                telemData.Add(index);
            }
            return telemData;
        }
    }
}
