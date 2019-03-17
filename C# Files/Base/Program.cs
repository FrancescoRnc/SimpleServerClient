using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleServerClient
{
    class Program
    {
        static Server server;
        static Transport transport;

        static void Main(string[] args)
        {
            transport = new Transport();
            transport.Bind("192.168.3.228", 9876);

            server = new Server(transport);
            server.Run();
        }
    }
}
