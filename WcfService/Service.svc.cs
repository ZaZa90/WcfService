using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;

namespace WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service.svc or Service.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Service : IService
    {

        private Database database = new Database();

        public void RecvConf(string conf)
        {
            string[] config = conf.Split(',');
            database.setConf(float.Parse(config[0]), float.Parse(config[1]), float.Parse(config[2]), float.Parse(config[3])) ;
            Debug.Print("[Service] Received config:" + conf);
        }

        public void RecvIp(string ip) {
            database.SetIp(ip);
            Debug.Print("[Service] Received ip:"+ip);
        }

        public string SendIp() { return database.GetIp(); }

        public String SendOperation() { Debug.Print("[SERVER] Operation list counts "+ database.getOperations().Count +" elems"); return database.getOperation(); return database.getNextLocalTarget();  }

        public int RecvPictureTCP(string numBytes)
        {
    
            TcpListener l = new TcpListener(IPAddress.Loopback, 0);
            l.Start();
            int port = ((IPEndPoint)l.LocalEndpoint).Port;
            l.Stop();

            TcpServer tcpServer = new TcpServer(Int32.Parse(numBytes), port);
            return port;
        }
    }
}
