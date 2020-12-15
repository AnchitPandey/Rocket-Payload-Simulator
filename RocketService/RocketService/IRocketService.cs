using RocketService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;


namespace RocketService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IRocketService" in both code and config file together.
    [ServiceContract]
    public interface IRocketService
    {

        [OperationContract]
        List<Double> increaseAltitude(double time, double altitude);

        [OperationContract]
        List<Double> GetTelemetryData(double timeToOrbit);

    }

}
