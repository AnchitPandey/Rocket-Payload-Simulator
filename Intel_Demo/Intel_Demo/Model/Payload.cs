using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intel_Demo.Model
{
    class Data
    {
        string dataType;
        string dataVar1;
        string dataVar2;
        string dataVar3;
    }

    class Payload: INotifyPropertyChanged
    {
        private bool dataFlag;
        public bool DataFlag
        {
            get
            {
                return dataFlag;
            }
            set
            {
                dataFlag = value;
                OnPropertyChanged("DataFlag");
            }
        }

        private string payloadId;

        public string PayloadId
        {
            get { return payloadId; }
            set { payloadId = value; OnPropertyChanged("PayloadId"); }


        }

        private string payloadName;
        public string PayloadName
        {
            get { return payloadName; }
            set { payloadName = value; OnPropertyChanged("PayloadName"); }
   

        }

        private bool decommissionFlag;
         public bool DecommissionFlag
        {
            get
            {
                return decommissionFlag;
            }
            set
            {
                decommissionFlag = value;
                OnPropertyChanged("DecommissionFlag");
            }
        }

        private bool telemetryFlag;
        public bool TelemetryFlag
        {
            get
            {
                return telemetryFlag;
            }
            set
            {
                telemetryFlag = value;
                OnPropertyChanged("TelemetryFlag");
            }
        }
        
        
        private double altitude { get; set; }
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



        private double temperature { get; set; }
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
        private string type;
        public string Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
                OnPropertyChanged("Type");
            }
        }


        private string dataLabel2;
      

        public string DataLabel2
        {
            get
            {
                return dataLabel2;
            }
            set
            {
                dataLabel2 = value;
                OnPropertyChanged("DataLabel2");
            }

        }

        private string dataLabel3;


        public string DataLabel3
        {
            get
            {
                return dataLabel3;
            }
            set
            {
                dataLabel3 = value;
                OnPropertyChanged("DataLabel3");
            }

        }

        private string dataLabel1;


        public string DataLabel1
        {
            get
            {
                return dataLabel1;
            }
            set
            {
                dataLabel1 = value;
                OnPropertyChanged("DataLabel1");
            }

        }

        private string dataValue1;
        public string DataValue1
        {
            get
            {
                return dataValue1;
            }
            set
            {
                dataValue1 = value;
                OnPropertyChanged("DataValue1");
            }

        }

        private string dataValue2;
        public string DataValue2
        {
            get
            {
                return dataValue2;
            }
            set
            {
                dataValue2 = value;
                OnPropertyChanged("DataValue2");
            }

        }



        private string dataValue3;
        public string DataValue3
        {
            get
            {
                return dataValue3;
            }
            set
            {
                dataValue3 = value;
                OnPropertyChanged("DataValue3");
            }

        }
        
        private string imageURL;
        public string ImageURL
        {
            get
            {
                return imageURL;
            }
            set
            {
                imageURL = value;
                OnPropertyChanged("ImageURL");
            }
        }
        
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        //public Data data { get; set; }
    }
}
