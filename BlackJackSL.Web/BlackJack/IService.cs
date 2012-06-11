using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web;

namespace BlackJackSL.Web.BlackJack
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        String SayHello(String name);
    }
}
