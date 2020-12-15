using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace PayloadService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IPayloadService" in both code and config file together.
    [ServiceContract]
    public interface IPayloadService
    {
        [OperationContract]
        List<Double> GetPayloadData(String type, int index);

        [OperationContract]
        List<Double> GetPayloadTelemetryData();
    }
}
