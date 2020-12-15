using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intel_Demo.Model
{
    class Log : INotifyPropertyChanged
    {
        private int logId;
        public int LogId
        {
            get
            {
                return logId;
            }
            set
            {
                logId = value;
                OnPropertyChanged("LogId");
            }
        }
         private string status;
        public string Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }

        private string message;
        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                message = value;
                OnPropertyChanged("Message");
            }
        }



        private string timeStamp;
        public string Timestamp
        {
            get
            {
                return timeStamp;
            }
            set
            {
                timeStamp = value;
                OnPropertyChanged("Timestamp");
            }
        }






        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
