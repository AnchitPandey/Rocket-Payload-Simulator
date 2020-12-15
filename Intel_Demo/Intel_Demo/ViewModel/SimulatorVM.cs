using Intel_Demo.Model;
using Intel_Demo.ViewModel.Commands;
using Intel_Demo.ViewModel.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Intel_Demo.ViewModel
{
    class SimulatorVM : INotifyPropertyChanged
    {
        private string myFile = "C:/Users/Anchit Pandey/Documents/Visual Studio 2017/Projects/Intel_Demo/Intel_Demo/assets/";    
        private ObservableCollection<Rocket> currentRockets;
        private Dictionary<string, Rocket> rocketNameToObjectMap;
        List<String> imageList;

        Dictionary<string, string> PayLoadToRocketMap;
       
        HashSet<string> DecomRocketsSet;
        public Dictionary<string, Thread> rocketLaunchThreadMap, rocketTelemetryThreadMap, payloadTelemetryThreadMap, payloadDataThreadMap;
        public LaunchCommand LaunchCommand { get; set; }
        public DeployPayloadCommand DeployPayloadCommand { get; set; }
        public DecommissionCommand DecommissionCommand { get; set; }

        public RocketDeorbitCommand RocketDeorbitCommand { get; set; }
        public StartTelemetryCommand StartTelemetryCommand { get; set; }
        public StopTelemetryCommand StopTelemetryCommand { get; set; }
        public StartPayloadTelemetryCommand StartPayloadTelemetryCommand { get; set; }
        public StopPayloadTelemetryCommand StopPayloadTelemetryCommand { get; set; }

        public StartPayloadDataCommand StartPayloadDataCommand { get; set; }
        public StopPayloadDataCommand StopPayloadDataCommand { get; set; }


        WCFGetterFunctions wcfObject;
        int flag = -1;

        public ObservableCollection<Rocket> CurrentRockets
        {
            get { return currentRockets; }
            set { currentRockets = value;
            }
        }

        private ObservableCollection<Log> logs;
 
        public ObservableCollection<Log> Logs
        {
            get { return logs; }
            set
            {
                logs = value;
            }
        }

        public SimulatorVM ()
        {

          //   wcfObject = new WCFGetterFunctions();
          SearchCommand = new SearchCommand(this);
            imageList = new List<String>();
            InitializeImageList(imageList);
            rocketsYetToLaunch = new ObservableCollection<Rocket>();
            logs = new ObservableCollection<Log>();
            rocketNameToObjectMap = new Dictionary<string, Rocket>();
            rocketLaunchThreadMap = new Dictionary<string, Thread>();
            rocketTelemetryThreadMap = new Dictionary<string, Thread>();
            payloadTelemetryThreadMap = new Dictionary<string, Thread>();
            payloadDataThreadMap = new Dictionary<string, Thread>();
            currentRockets = new ObservableCollection<Rocket>();
            currentPayloads = new ObservableCollection<Payload>();
            // Read Rocket data from config file
            string path = Directory.GetCurrentDirectory();  
          string upper = "..\\..\\Config Files\\Rockets.json";
          string finalPath = Path.Combine(path, upper);
          PayLoadToRocketMap = new Dictionary<string, string>();
          DecomRocketsSet = new HashSet<string>();
          dynamic rockets = JsonConvert.DeserializeObject(File.ReadAllText(finalPath));
            // optimize below initialization
          bool firstItem = false;
          rockets = rockets["rockets"]; 
          foreach (var rocket in rockets )
            {

                Rocket r = new Rocket();
                r.RocketName = rocket["name"];
                r.OrbitRadius = rocket["orbit"];
                r.PayloadId = rocket["payload"];
                r.PayloadStatus = "Not Deployed";
                //r.TimeToOrbit = 3;
                r.TimeToOrbit = Math.Round((double)r.OrbitRadius / 3600 + 10, 2);
                r.TimeToReachInitialOrbit = r.TimeToOrbit;
                //r.TimeToReachInitialOrbit = 3;
                r.rocketId = rocket["rocketId"];
                rocketNameToObjectMap.Add(r.RocketName, r);
                rocketsYetToLaunch.Add(r);
                if (!firstItem)
                {
                    rocketYetToLaunch = r;
                    firstItem = true;
                }      
            }
            LaunchCommand = new LaunchCommand(this);
            RocketDeorbitCommand = new RocketDeorbitCommand(this);
            StartTelemetryCommand = new StartTelemetryCommand(this);
            StopTelemetryCommand = new StopTelemetryCommand(this);
            DeployPayloadCommand = new DeployPayloadCommand(this);
            StartPayloadTelemetryCommand = new StartPayloadTelemetryCommand(this);
            StopPayloadTelemetryCommand = new StopPayloadTelemetryCommand(this);
            StartPayloadDataCommand = new StartPayloadDataCommand(this);
            StopPayloadDataCommand = new StopPayloadDataCommand(this);
            DecommissionCommand = new DecommissionCommand(this);
        }

        public void Decommission()
        {
            if (CurrentPayload == null)
                return;
            AddToLogs("Payload  " + CurrentPayload.PayloadId + " decommissioned ", "Success");
            string rId = PayLoadToRocketMap[CurrentPayload.PayloadId];
            foreach(var rk in currentRockets)
            {
                if (rk.rocketId == rId)
                {
                    rk.PayloadStatus = "Decommissioned";
                    DecomRocketsSet.Add(rk.rocketId);
                    break;
                }
            }
            currentPayloads.Remove(CurrentPayload);
        }

        public void InitializeImageList (List<String> imageList)
        {
            imageList.Add("Grass.jpeg");
            imageList.Add("alien.jpg");
            imageList.Add("bricks.jpeg");
            imageList.Add("space_station.jpg");
        }

        public void deployPayload()
        {
            Rocket temp = currentRocket;
            if (temp == null)
                return;
            if (temp.TimeToReachInitialOrbit > 0)
            {
                // show message box saying orbit has not been reached
                AddToLogs("Not Deployed because " + CurrentRocket.RocketName + " orbit not reached", "Failed");
                return;
            }
           else if (DecomRocketsSet.Contains(temp.rocketId))
            {
                // show message box saying payload has been decomissioned
                AddToLogs("Not Deployed because "+CurrentRocket.RocketName+"'s payload is already decommissioned", "Failed");
                return;
            }
            else if (PayLoadToRocketMap.ContainsKey(temp.PayloadId))
            {
                // show message box saying payload has been deployed
                AddToLogs("Payload "+ temp.PayloadId+" was already deployed", "Failed");
                return;
            }

            AddToLogs("Payload " + temp.PayloadId + " deployed", "Success");
            PayLoadToRocketMap.Add(temp.PayloadId, temp.rocketId);
            string path = Directory.GetCurrentDirectory();
            string upper = "..\\..\\Config Files\\Payloads.json";
            string finalPath = Path.Combine(path, upper);
            dynamic payloads = JsonConvert.DeserializeObject(File.ReadAllText(finalPath));
            foreach (var p in payloads["payloads"])
            {

                var t1 = (string) p["payloadId"];
                var t2 = temp.PayloadId;
                
                if (t1 == temp.PayloadId)
               // if (p["payloadId"].Equals(temp.PayloadId))
                {
                    string newPath = "..\\..\\Config Files\\Payload_Data\\"+p["payloadId"]+".json";
                    System.IO.File.WriteAllText(newPath, "");
                    Payload pay = new Payload();
                    string type = (string)p["type"];
                    
                    pay.PayloadId = t1;
                    type = type.ToLower();
                    pay.Type = type;
                    if (type == "scientific")
                    {
                        pay.DataLabel1 = "Rainfall (mm)";
                        pay.DataLabel2 = "Humidity (%)";
                        pay.DataLabel3 = "Snow (inch)";
 
                        pay.PayloadId = (string)p["payloadId"];
                         pay.TelemetryFlag = false;
                        pay.DataFlag = false;
                    //    CurrentPayloads.Add(pay);
                    }
                    else if (type == "communication") { 
                    
                        pay.DataLabel1 = "Uplink (Mbps)";
                        pay.DataLabel2 = "Downlink (Mbps)";
                        pay.DataLabel3 = "Active Transponders";
                    }

                    else if (type == "spy")
                    {
                        pay.DataLabel1 = "Image Size";
                        pay.DataLabel2 = "Transmission Rate";
                        pay.DataLabel3 = "Image";
                    }
                    pay.ImageURL = "a";
                    CurrentPayloads.Add(pay);
                    temp.PayloadStatus = "Deployed";
                    /// TODO:  add for image
                    break;
                }
            }
        }


        public void AddToLogs (String message, String status)
        {
            Log log = new Log();
            int newId;
            if (logs.Count == 0)
                newId = 1;

            else
            {
                if (logs.Count == 10)
                {
                    App.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
                    {
                        logs.RemoveAt(0);
                    });
                }

                    Log temp = logs[0];
                newId = temp.LogId + logs.Count;
            }
            log.LogId = newId;
            log.Status = status;
            log.Message = message;
            DateTime localDate = DateTime.Now;
            var culture = new CultureInfo("en-US");
            log.Timestamp = localDate.ToString(culture);
            App.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
            {
                logs.Add(log);
            });
        }


        private ObservableCollection<Rocket> rocketsYetToLaunch;

        public ObservableCollection<Rocket> RocketsYetToLaunch
        {
            get { return rocketsYetToLaunch; }
            set
            {
                rocketsYetToLaunch = value;
            }
        }

        public void StartTelemetry()
        {
            if (CurrentRocket == null)
                return;
            if (CurrentRocket.TelemetryFlag)
            {
                AddToLogs("Rocket " + CurrentRocket.RocketName + " telemetry already on", "Failed");
                return;
            }
            CurrentRocket.TelemetryFlag = true;
            AddToLogs("Rocket " + CurrentRocket.RocketName + " telemetry started", "Success");
            Thread t = new Thread(()=> SimulateStartTelemetry(CurrentRocket));
            rocketTelemetryThreadMap.Add(CurrentRocket.rocketId, t);
            t.Start();
            return;
        }

        public void StartPayloadData()
        {
            if (CurrentPayload == null)
                return;
            if (CurrentPayload.DataFlag)
            {
                AddToLogs("Payload " + CurrentPayload.PayloadId + " data already on", "Failed");
                return;
            }
            AddToLogs("Payload " + CurrentPayload.PayloadId + " data on", "Success");
            CurrentPayload.DataFlag = true;
            Thread t = new Thread(() => SimulateStartPayloadData(CurrentPayload));
            payloadDataThreadMap.Add(CurrentPayload.PayloadId, t);
            t.Start();
            return;
        }

        public void SimulateStartPayloadData(Payload payload)
        {

            PayloadService.PayloadServiceClient rock = new PayloadService.PayloadServiceClient();
            Payload temp = CurrentPayload;
            int timer = 0;
            if (temp.Type == "scientific")
                timer = 60000;
            else if (temp.Type == "communication")
                timer = 5000;

            else if (temp.Type == "spy")
                timer = 10000;

            int start = 0;

            while (temp.DataFlag == true)
            {
                // change this
                double[] telemData = rock.GetPayloadData(temp.Type.ToLower(), start); 
                temp.DataValue1 = telemData[0].ToString();
                temp.DataValue2 = telemData[1].ToString();
                if (temp.Type.ToLower() == "spy")
                {
                    int index = (int)telemData[2];
                    temp.DataValue3 = "";
                    // temp.ImageURL = imageList[index];
                    temp.ImageURL = myFile + imageList[index];
                   // temp.ImageURL = "https://www.shutterstock.com/image-photo/shortlink-welded-chain-zinc-plated-steel-1830931787";
                }
                else
                {
                    temp.DataValue3 = telemData[2].ToString();
                    temp.ImageURL = "";
                }
                    // payload.Altitude += telemData[3];
                Thread.Sleep(timer);
                start += 1;
            }
        }





        public void StartPayloadTelemetry()
        {     
            if (CurrentPayload == null)
                return;
            
            if (CurrentPayload.TelemetryFlag)
            {
                AddToLogs("Payload " + CurrentPayload.PayloadId + " telemetry already on", "Failed");
                return;
            }
     
            AddToLogs("Payload " + CurrentPayload.PayloadId + " telemetry on", "Success");
            CurrentPayload.TelemetryFlag = true;
            Thread t = new Thread(() => SimulatePayloadStartTelemetry(CurrentPayload));
            payloadTelemetryThreadMap.Add(CurrentPayload.PayloadId, t);
            t.Start();
            return;
        }

        public void SimulatePayloadStartTelemetry (Payload payload)
        {

            PayloadService.PayloadServiceClient rock = new PayloadService.PayloadServiceClient();
            Payload temp = CurrentPayload;
            while (temp.TelemetryFlag == true)
            {
                // change this 

                double[] telemData = rock.GetPayloadTelemetryData();
                payload.Latitude = telemData[0];
                payload.Longitude = telemData[1];
                payload.Temperature = telemData[2];
                payload.Altitude += telemData[3];
                Thread.Sleep(1000);
            }

        }


        public void StopPayloadTelemetry()
        {
            if (CurrentPayload == null)
                return;
            if (!CurrentPayload.TelemetryFlag)
            {
                AddToLogs("Payload " + CurrentPayload.PayloadId + " telemetry already off", "Failed");
                return;
            }
            AddToLogs("Payload " + CurrentPayload.PayloadId + " telemetry off", "Success");
            // change these to some other values
            Thread t =payloadTelemetryThreadMap[CurrentPayload.PayloadId];
            t.Abort();
            payloadTelemetryThreadMap.Remove(CurrentPayload.PayloadId);
           // CurrentPayload.Longitude = 0;
           // CurrentPayload.Latitude = 0;
            CurrentPayload.TelemetryFlag = false;
           // CurrentPayload.Temperature = 0;
        }

        public void StopPayloadData()
        {
            if (CurrentPayload == null)
                return;
            if (!CurrentPayload.DataFlag)
            {
                AddToLogs("Payload " + CurrentPayload.PayloadId + " data already off", "Failed");
                return;
            }
            AddToLogs("Payload " + CurrentPayload.PayloadId + " data off", "Success");
            // change these to some other values
            Thread t = payloadDataThreadMap[CurrentPayload.PayloadId];
            t.Abort();
            payloadDataThreadMap.Remove(CurrentPayload.PayloadId);
            CurrentPayload.DataValue1 = "";
            CurrentPayload.DataValue2 = "";
            CurrentPayload.DataFlag = false;
            CurrentPayload.DataValue3 = "";
            CurrentPayload.ImageURL = "";
        }


        public void SimulateStartTelemetry(Rocket rocket)
        {

            RocketService.RocketServiceClient rock = new RocketService.RocketServiceClient();
            Rocket temp = currentRocket;
            while (temp.TelemetryFlag == true)
            {
                double[] telemData = rock.GetTelemetryData(rocket.Altitude);
                rocket.Latitude = telemData[0];
                rocket.Longitude = telemData[1];
                rocket.Temperature = telemData[2];
                rocket.Altitude += telemData[3];
                Thread.Sleep(1000);
            }
        }

        public void StopTelemetry ()
        {
            if (CurrentRocket == null)
                return;
            if (!CurrentRocket.TelemetryFlag)
            {
                AddToLogs("Rocket " + CurrentRocket.RocketName + " telemetry already off", "Failed");
                return;
            }
            AddToLogs("Rocket " + CurrentRocket.RocketName + " telemetry off", "Success");
            CurrentRocket.TelemetryFlag = false;
            Thread t = rocketTelemetryThreadMap[CurrentRocket.rocketId];
            t.Abort();
            rocketTelemetryThreadMap.Remove(CurrentRocket.rocketId);
            //CurrentRocket.Longitude = 0;
            //CurrentRocket.Latitude = 0;
            //CurrentRocket.TelemetryFlag =false;
            //CurrentRocket.Temperature = 0;
        }


        public void ShiftRocketFromInActiveToActive()
        {
            if (rocketsYetToLaunch.Count == 0)
            {
                AddToLogs("No rockets to launch", "Failed");
                return;
            }
            AddToLogs("Rocket " + rocketYetToLaunch.RocketName + " launched ", "Success");
            
            CurrentRockets.Add(rocketYetToLaunch);

            // Send command to Rocket Endpoint to launch
            Thread t = new Thread(() => simulateLaunch(rocketYetToLaunch));
            t.Start();
            // Add Launch thread
            rocketLaunchThreadMap.Add(rocketYetToLaunch.rocketId,t);

            // Adjust the UI
           
            CurrentRocket = rocketYetToLaunch;
            rocketsYetToLaunch.Remove(rocketYetToLaunch);
            if (rocketsYetToLaunch.Count > 0)      
                rocketYetToLaunch = rocketsYetToLaunch.First();
            
            /// TODO: Create Threads for altitude reaching & Updating DSN...
                   
            /// 
        }

       
        public void simulateLaunch (Rocket r)
        {
            // calling Rocket Endpoint
            // WCF code
            r.TimeToReachInitialOrbit = r.TimeToOrbit;
            RocketService.RocketServiceClient rock = new RocketService.RocketServiceClient();
            while (r.TimeToReachInitialOrbit > 0)
            {
               double[] ans = rock.increaseAltitude(r.TimeToReachInitialOrbit, r.Altitude);
                r.TimeToReachInitialOrbit = ans[0];
                r.Altitude = ans[1];
                //OrbitTime = r.timeToReachInitialOrbit.ToString()+" s";
                Thread.Sleep(1000);
            }
            if (r.TimeToReachInitialOrbit < 0)
                r.TimeToReachInitialOrbit = 0;
          
            AddToLogs("Rocket " + r.RocketName + " has orbitted", "Success");
            // Remove thread from hashmap
            rocketLaunchThreadMap.Remove(r.rocketId);
            // Stop current thread
            Thread.CurrentThread.Abort();
        }

        public void ShiftRocketFromActiveToInActive()
        {
            if (currentRockets.Count == 0)
            {
                AddToLogs("No rockets to deorbit", "Failed");
                return;
            }
            AddToLogs("Rocket " + CurrentRocket.RocketName + " deorbitted", "Success");

            rocketsYetToLaunch.Add(currentRocket);
            rocketYetToLaunch = currentRocket;
            rocketLaunchThreadMap.Remove(currentRocket.rocketId);
            currentRockets.Remove(currentRocket);
            if (currentRockets.Count > 0)
                currentRocket = currentRockets.First();
        }

        // Current Rocket 
        private Rocket currentRocket;
        public Rocket CurrentRocket
        {
            get { return currentRocket; }
            set { currentRocket = value;
                OnPropertyChanged("CurrentRocket");
            }
        }

        private Payload currentPayload;
        public Payload CurrentPayload
        {
            get { return currentPayload; }
            set
            {
                currentPayload = value;
                OnPropertyChanged("CurrentPayload");
            }
        }

        // Search Command Object
        public SearchCommand SearchCommand { get; set; }

        // Rocket About to be launched

        private Rocket rocketYetToLaunch;
        public Rocket RocketYetToLaunch
        {
            get { return rocketYetToLaunch; }
            set
            {
                rocketYetToLaunch = value;
                OnPropertyChanged("RocketYetToLaunch");
            }
        }


        // Rocket Data

        private string rocketAltitude;
        public string RocketAltitude
        {
            get { return rocketAltitude; }
            set { rocketAltitude = value;
                OnPropertyChanged("RocketAltitude");
            }
        }

        private string rocketLatitude;
        public string RocketLatitude
        {
            get { return rocketLatitude; }
            set { rocketLatitude = value;
                OnPropertyChanged("RocketLatitude");

            }
        }

        private string rocketLongitude;
        public string RocketLongitude
        {
            get { return rocketLongitude; }
            set { rocketLongitude = value;
                OnPropertyChanged("RocketLongitude");
            }
        }

        private string payloadStatus;
        public string PayloadStatus
        {
            get { return payloadStatus; }
            set { payloadStatus = value;

                OnPropertyChanged("PayloadStatus");
            }
        }

        private string telemetryStatus;
        public string TelemetryStatus
        {
            get { return telemetryStatus; }
            set { telemetryStatus = value;
                OnPropertyChanged("TelemetryStatus");

            }
        }

        private string orbitRadius;
        public string OrbitRadius
        {
            get { return orbitRadius; }
            set { orbitRadius = value;
                OnPropertyChanged("OrbitRadius");

            }
        }

        private string orbitTime;
        public string OrbitTime
        {
            get { return orbitTime; }
            set { orbitTime = value;

                OnPropertyChanged("OrbitTime");

            }
        }

        private string temperature;
        public string Temperature
        {
            get { return temperature; }
            set { temperature = value;
                OnPropertyChanged("Temperature");

            }
        }


        // For Payload

        private string payloadAltitude;
        public string PayloadAltitude
        {
            get { return payloadAltitude; }
            set { payloadAltitude = value;
                OnPropertyChanged("PayloadAltitude");

            }
        }

        private string payloadLatitude;
        public string PayloadLatitude
        {
            get { return payloadLatitude; }
            set { payloadLatitude = value;
                OnPropertyChanged("PayloadLatitude");

            }
        }

        private string payloadLongitude;
        public string PayloadLongitude
        {
            get { return payloadLongitude; }
            set { payloadLongitude = value;
                OnPropertyChanged("PayloadLongitude");

            }
        }

        
        private string payloadData1;
        public string PayloadData1
        {
            get { return payloadData1; }
            set { payloadData1 = value;
                OnPropertyChanged("PayloadData1");
            }
        }

        private string payloadTelemetryStatus;
        public string PayloadTelemetryStatus
        {
            get { return payloadTelemetryStatus; }
            set { payloadTelemetryStatus = value;
                OnPropertyChanged("PayloadTelemetryStatus");
            }
        }

        private string payloadData2;
        public string PayloadData2
        {
            get { return payloadData2; }
            set { payloadData2 = value;
                OnPropertyChanged("PayloadData2");
            }
        }

        private string payLoadDataStatus;
        public string PayLoadDataStatus
        {
            get { return payLoadDataStatus; }
            set { payLoadDataStatus = value;
                OnPropertyChanged("PayLoadDataStatus");

            }
        }

        private string payloadData3;
        public string PayloadData3
        {
            get { return payloadData3; }
            set { payloadData3 = value;
                OnPropertyChanged("PayloadData3");
            }
        }


        private ObservableCollection<Payload> currentPayloads;

        public ObservableCollection<Payload> CurrentPayloads
        {
            get { return currentPayloads; }
            set {
                currentPayloads = value;

            }
        }

        private Payload currentPayLoad;
        public Payload CurrentPayLoad
        {
            get { return currentPayLoad; }
            set { currentPayLoad = value;
                OnPropertyChanged("CurrentPayLoad");
            }
        }


        private Rocket unlaunchedCurrentRocket;
        public Rocket UnlaunchedCurrentRocket
        {
            get { return unlaunchedCurrentRocket; }
            set { unlaunchedCurrentRocket = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged (string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void getRocketData()
        {
            Console.WriteLine(flag);
            wcfObject.sendRequestForRocket();
        }

        public void getPayloadData()
        {
            wcfObject.sendRequestForPayload();
        }
    }
}
