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
    (typeof(PayloadService.PayloadService)))
            {
                host.Open();
                Console.WriteLine("Payload started @ " + DateTime.Now.ToString());
                Console.ReadLine();
            }
        }
    }
}
