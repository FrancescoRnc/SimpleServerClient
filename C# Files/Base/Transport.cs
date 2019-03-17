using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace SimpleServerClient
{   
    class Transport : ITransport
    {
        string address;
        int port;
        EndPoint endpoint;

        Socket socket;

        public Transport()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //socket.Blocking = false;
        }

        public void Bind(string address, int port)
        {
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(address), port);
            socket.Bind(endpoint);
        }

        public EndPoint CreateEndPoint()
        {
            return new IPEndPoint(0, 0);
        }

        public byte[] Receive(int buffersize, ref EndPoint sender)
        {
            
            byte[] data = new byte[buffersize];
            int rlen = socket.ReceiveFrom(data, ref sender);
            
            byte[] trueData = new byte[rlen];
            Buffer.BlockCopy(data, 0, trueData, 0, rlen);
            return trueData;
        }

        public bool Send(byte[] data, EndPoint destination)
        {
            socket.SendTo(data, destination);

            return true;
        }
    }
}
