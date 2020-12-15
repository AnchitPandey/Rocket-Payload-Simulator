using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Host
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost
    (typeof(RocketService.RocketService)))
            {
                host.Open();
                Console.WriteLine("Rocket started at Host started @ " + DateTime.Now.ToString());
                Console.ReadLine();
            }
        }
    }
}
