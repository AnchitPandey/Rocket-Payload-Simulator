using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using System.Threading;
namespace Intel_Demo.View
{
    /// <summary>
    /// Interaction logic for LaunchSimulator.xaml
    /// </summary>
    public partial class LaunchSimulator : Window
    {
        public LaunchSimulator()
        {
            InitializeComponent();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
         // Thread t = new Thread(go);
                   
        }
        /*
        private void increaseAltitude(Rocket r)
        {
            int totalTime;
            while (r.orbitTime < totalTime)
            {
                // increase
                // backend call alt, long,lat
                // r.alt = alt iff r.telemetryFlag
                // sleep
            }
        }

        private void go()
        {
            while (1==1)
            {
                MessageBox.Show("Being printed by "+ Thread.CurrentThread.Name);
               // Console.WriteLine("Being printed by " + Thread.CurrentThread.Name);
               Thread.Sleep(5000);
                break;
            }
            MessageBox.Show("Thread getting finished - " + Thread.CurrentThread.Name);
        }
        */
    }
}
