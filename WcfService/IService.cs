using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

namespace WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService" in both code and config file together.
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "conf/{conf}", Method = "POST")]
        void RecvConf(string conf);

        [OperationContract]
        [WebInvoke(UriTemplate = "ip/{ip}", Method = "POST")]
        void RecvIp(string ip);

        [OperationContract]
        [WebInvoke(UriTemplate = "ip", Method = "GET")]
        string SendIp();

        [OperationContract]
        [WebInvoke(UriTemplate = "operation", Method = "GET")]
        String SendOperation();

        [OperationContract]
        [WebInvoke(UriTemplate = "picture/TCP/{numBytes}", Method = "POST")]
        int RecvPictureTCP(string numBytes);
    }

}
