using System.Collections.Generic;
using System.ServiceModel;

namespace BlackJackSL.Web.BlackJack.JSON
{
    [ServiceContract(Namespace="http://DotNetByExample/JSONDemoService")]
    public interface IJSONService
    {
        [OperationContract]
        SampleDataObject GetSingleObject(int id, string name);

        [OperationContract]
        ICollection<SampleDataObject> GetMultipleObjects(int number);
    }
}