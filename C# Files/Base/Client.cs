using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace SimpleServerClient
{
    public class Client
    {
        ITransport transport;
                
        byte[] dataToSend;

        string lastInvertedReceived;
        public string LastInvertedMessageReceived { get { return lastInvertedReceived; } }

        public Client(ITransport transport)
        {
            this.transport = transport;
        }

        public void Run()
        {
            while(true)
            {
                Step();
            }
        }

        public void Step()
        {
            EndPoint endPoint = transport.CreateEndPoint();
            byte[] data = transport.Receive(256, ref endPoint);
            if (data != null)
            {
                string messageReceived = Encoding.UTF8.GetString(data);
                lastInvertedReceived = messageReceived;
                Console.WriteLine(messageReceived);
            }

            //Console.WriteLine("Inserisci messaggio:");
            //string s = Console.ReadLine();
            //byte[] data = Encoding.UTF8.GetBytes(s);

            if (dataToSend != null)
            {
                transport.Send(dataToSend, endPoint);
                dataToSend = null;
            }

            //data = new byte[256];
            //Console.WriteLine("Messaggio ricevuto:\n" + Encoding.UTF8.GetString(data));
        }

        public void SendMessage(string message)
        {
            if (message != "")
                dataToSend = Encoding.UTF8.GetBytes(message.ToCharArray());
        }


        //Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        //EndPoint endPoint = new IPEndPoint(IPAddress.Parse("192.168.3.228"), 9876);
        //
        //    while (true)
        //    {
        //        Console.WriteLine("Inserisci messaggio:");
        //        string s = Console.ReadLine();
        //byte[] data = Encoding.Default.GetBytes(s);
        //
        //int rlen = socket.SendTo(data, endPoint);
        //data = new byte[256];
        //
        //        socket.ReceiveFrom(data, ref endPoint);
        //        Console.WriteLine("Messaggio ricevuto:\n" + Encoding.UTF8.GetString(data));
        //    }
    }
}
