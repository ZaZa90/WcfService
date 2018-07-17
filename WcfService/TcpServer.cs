using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Web;

namespace WcfService
{
    public class TcpServer
    {

        static Database database = new Database();
        
        public TcpServer(int numBytes,int port)
        {
            Thread t = null;

            t = new Thread(() => TcpServerThread(numBytes,port));
            t.Start();
        }


        static void TcpServerThread(int numBytes,int port)
        {
            // Establish the local endpoint for the socket.
            // Dns.GetHostName returns the name of the 
            // host running the application.
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse("0.0.0.0"), port);

            // Create a TCP/IP socket.
            Socket listener = new Socket(AddressFamily.InterNetwork,SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and 
            // listen for incoming connections.
            listener.Bind(localEndPoint);
            listener.Listen(1);

            // Start listening for connections.
            Console.WriteLine("Waiting for a connection...");
            // Program is suspended while waiting for an incoming connection.
            Socket s = listener.Accept();

            Byte[] bytes = new Byte[numBytes];

            int current = 0;
            int tcpBufSize = 4096;
            int rcv;
            while (current < numBytes)
            {

                if ((current + tcpBufSize) > numBytes)
                    tcpBufSize = numBytes - current;
                rcv = s.Receive(bytes, current, tcpBufSize, SocketFlags.None);
                current += rcv;
            }
            

            Picture p = new Picture();
            p.CreatePicture(bytes, "picture" + database.NewPictureId().ToString());
            int result = p.ScanQR();
            database.SetPicture(p);

            byte[] resp = BitConverter.GetBytes(result);
            s.Send(resp);

            s.Close();
            listener.Close();
         }            
    }
}
