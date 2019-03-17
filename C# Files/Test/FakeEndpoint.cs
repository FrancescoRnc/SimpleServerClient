using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace SimpleServerClient.Test
{
    public class FakeEndpoint : EndPoint
    {
        string address;
        public string Address { get { return address; } }
        int port;
        public int Port { get { return port; } }

        public FakeEndpoint()
        {

        }

        public FakeEndpoint(string address, int port)
        {
            this.address = address;
            this.port = port;
        }

        public override bool Equals(object obj)
        {
            FakeEndpoint other = obj as FakeEndpoint;
            if (other == null)
                return false;
            return (this.address == other.address) && (this.port == other.port);
        }

        public override int GetHashCode()
        {
            return Address.GetHashCode() + Port;
        }
    }
}
