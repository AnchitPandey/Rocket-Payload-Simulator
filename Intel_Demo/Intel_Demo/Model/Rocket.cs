using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intel_Demo.Model
{
    class Rocket: INotifyPropertyChanged
    {
        private bool telemetryFlag;
        public bool TelemetryFlag
        {
            get
            {
                return telemetryFlag;
            }
            set {
                telemetryFlag = value;
                OnPropertyChanged("TelemetryFlag");
            }
        }

        private double orbitRadius;

        public double OrbitRadius
        {
            get
            {
                return orbitRadius;
            }
            set
            {
                orbitRadius = value;
                OnPropertyChanged("OrbitRadius");
            }
        }

        private bool deployPayload;
        public bool DeployPayload
        {
            get
            {
                return deployPayload;
            }
            set
            {
                deployPayload = value;
                OnPropertyChanged("DeployPayload");
            }
        }

        private double altitude;
        public double Altitude
        {
            get
            {
                return altitude;
            }
            set
            {
                altitude = value;
                OnPropertyChanged("Altitude");
            }
        }

        private double longitude;
        public double Longitude
        {
            get
            {
                return longitude;
            }
            set
            {
                longitude = value;
                OnPropertyChanged("Longitude");
            }
        }

        private double latitude;
        public double Latitude
        {
            get
            {
                return latitude;
            }
            set
            {
                latitude = value;
                OnPropertyChanged("Latitude");
            }
        }





        private double temperature;
        public double Temperature
        {
            get
            {
                return temperature;
            }
            set
            {
                temperature = value;
                OnPropertyChanged("Temperature");
            }
        }

        private double timeToOrbit { get; set; }
        public double TimeToOrbit
        {
            get
            {
                return timeToOrbit;
            }
            set
            {
                timeToOrbit = value;
                OnPropertyChanged("TimeToOrbit");
            }
        }

        private string rocketName { get; set; }
        public string RocketName
        {
            get
            {
                return rocketName;
            }
            set
            {
                rocketName = value;
                OnPropertyChanged("RocketName");
            }
        }


        private double timeToReachInitialOrbit;
        public double TimeToReachInitialOrbit
        {
            get
            {
                return timeToReachInitialOrbit;
            }
            set {
                timeToReachInitialOrbit = value;
                OnPropertyChanged("TimeToReachInitialOrbit");
            }
        }


        public string rocketId { get; set; }
        public string PayloadId { get; set; }


        private string payloadStatus;
        public string PayloadStatus
        {
            get
            {
                return payloadStatus;
            }
            set
            {
                payloadStatus = value;
                OnPropertyChanged("PayloadStatus");
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


       


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
